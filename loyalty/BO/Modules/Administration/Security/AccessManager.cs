using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data.Objects;
using HashLib;
using Palmary.Loyalty.Common;
using Palmary.Loyalty.BO.Database;
using Palmary.Loyalty.BO.Modules.Administration.User;
using Palmary.Loyalty.BO.DataTransferObjects.Security;
using Palmary.Loyalty.BO.DataTransferObjects.Role;
using Palmary.Loyalty.BO.DataTransferObjects.User;
using Palmary.Loyalty.BO.Modules.Administration.SystemControl;
using Palmary.Loyalty.BO.Modules.Administration.Role;
using System.Web.Security;
using Palmary.Loyalty.BO.Modules.Utility;
using Palmary.Loyalty.BO.Modules.Member;


namespace Palmary.Loyalty.BO.Modules.Administration.Security
{
    public class AccessManager
    {
        private static LoyaltyDBDataContext db = new LoyaltyDBDataContext();
       
        // This is top level manager, cannot init access object in constructor
        // This class is mostly for other manager to get current access object and privilege

        // perform login, and set session for taking log
        // backend CMS
        public CommonConstant.SystemCode CMSLogin(string login_id, string password, string action_ip, ref int user_id, ref string name)
        {
            var systemCode = CommonConstant.SystemCode.undefine;

            var userManager = new UserManager();

            var theUser = new UserObject();
            var getUserInfo = false;
            try
            {
                theUser = userManager.GetDetail(null, login_id, null, true, ref systemCode);
                getUserInfo = true;
            }
            catch(Exception e)
            {   
                // db error or other error
                systemCode = CommonConstant.SystemCode.database_error;
                getUserInfo = false;
            }

            if (getUserInfo)
            {
                var hashedPass = encryptPassword(password, theUser.user_id.ToString());

                int? status = 0;
                var remark = "";
                int? get_user_id = 0;

                db.sp_UserLogin(login_id, hashedPass, action_ip, ref status, ref remark, ref get_user_id, ref name);

                if (status == 1)
                {
                    user_id = get_user_id ?? 0;

                    // need to set session before taking Log
                    FormsAuthentication.SetAuthCookie(name, false);

                    SessionManager.Current.obj_name = name;
                    //SessionManager.Current.user_login_id = login_id;
                    SessionManager.Current.obj_id = user_id;
                    SessionManager.Current.obj_type = (int)CommonConstant.ObjectType.user;
                    SessionManager.Current.obj_ip = action_ip;
                    SessionManager.Current.obj_loginTime = DateTime.Now;
                    SessionManager.Current.obj_language_id = (int)CommonConstant.LangCode.en;
                    SessionManager.Current.obj_action_channel = CommonConstant.ActionChannel.cms_backend;

                    // take log
                    var log = new LogManager();
                    log.LogAction(CommonConstant.ActionType.login);

                    systemCode = CommonConstant.SystemCode.normal;
                }
                else
                {
                    user_id = 0;
                    systemCode = CommonConstant.SystemCode.no_permission;
                }
            }

            return systemCode;
        }

        //backend
        public void CMSLogout(int user_id)
        {
            // take log
            var log = new LogManager();
            log.LogAction(CommonConstant.ActionType.logout); 
        }

        // FrontEnd
        public bool MemberLogin(AccessObject accessObject, string login_token, string password, ref int member_id, ref string session)
        {
            var memberManager = new MemberManager(accessObject);

            int getMember_id = memberManager.GetMemberID(login_token);
            if (getMember_id == -1)
                return false;

            member_id = getMember_id;

            var hashedPass = AccessManager.encryptPassword(password, getMember_id.ToString());

            int? get_sql_result = 0;
            var sql_remark = "";

            DateTime time = DateTime.Now;

            session = AccessManager.encryptPassword(time.ToString("yyyy-MM-dd_HH:mm:ss"), getMember_id.ToString());

            var result = db.sp_MemberLogin(getMember_id, hashedPass, session, ref get_sql_result, ref sql_remark);

            // log, use specific accessObject and LogManager
            // because currently should be using system
            var theLog = new LogManager(accessObject);
            theLog.LogAction(CommonConstant.ActionType.login);

            return get_sql_result == 1 ? true : false;
        }


        public AccessObject GetAccessObjectBySession()
        {
            var accessObject = new AccessObject
            {
                type = SessionManager.Current.obj_type,
                id = SessionManager.Current.obj_id,
                name = SessionManager.Current.obj_name,
                actionChannel = SessionManager.Current.obj_action_channel,
                loginTime = SessionManager.Current.obj_loginTime,
                ip = SessionManager.Current.obj_ip,
                languageID = SessionManager.Current.obj_language_id
            };

            return accessObject;
        }

        public static string encryptPassword(string inputPassword, string hashKey)
        {
            // Calculate HMAC.
            IHMAC hmac = HashFactory.HMAC.CreateHMAC(HashFactory.Crypto.CreateSHA256());
            hmac.Key = Converters.ConvertStringToBytes(hashKey);
            HashResult hashedPass = hmac.ComputeString(inputPassword);
            return hashedPass.ToString();
        }
        
        // Access Module by user (BO session, thorugh backend)
        public RolePrivilegeObject AccessModule(string module)
        {
            var specificPrivilege = AccessModule(module, SessionManager.Current.obj_type, SessionManager.Current.obj_id);
            return specificPrivilege;
        }

        // Access Module by other (non BO session)
        public RolePrivilegeObject AccessModule(string module, int objectType, int objectID)
        {
            var specificPrivilege = new RolePrivilegeObject();

            if (objectType == CommonConstant.ObjectType.system 
                && objectID == CommonConstant.SystemObject.cms_bo)
            {
                specificPrivilege.insert_status = 1;
                specificPrivilege.update_status = 1;
                specificPrivilege.read_status = 1;
                specificPrivilege.delete_status = 1;
            }
            else if (objectType == CommonConstant.ObjectType.member)
            {
                if (module == CommonConstant.Module.log
                    || module == CommonConstant.Module.member
                    || module == CommonConstant.Module.transaction
                    || module == CommonConstant.Module.combineRedemption
                    || module == CommonConstant.Module.giftRedemption
                    || module == CommonConstant.Module.productPurchase
                    )
                {
                    specificPrivilege.insert_status = 1;
                    specificPrivilege.update_status = 1;
                    specificPrivilege.read_status = 1;
                    specificPrivilege.delete_status = 0;
                }
                else if (module == CommonConstant.Module.product
                    || module == CommonConstant.Module.productCategory
                    || module == CommonConstant.Module.gift
                    || module == CommonConstant.Module.giftCategory
                    || module == CommonConstant.Module.giftInventory
                    || module == CommonConstant.Module.passcode
                )
                {
                    specificPrivilege.insert_status = 0;
                    specificPrivilege.update_status = 0;
                    specificPrivilege.read_status = 1;
                    specificPrivilege.delete_status = 0;
                }
                else
                {
                    specificPrivilege.insert_status = 0;
                    specificPrivilege.update_status = 0;
                    specificPrivilege.read_status = 0;
                    specificPrivilege.delete_status = 0;
                }
            }
            else if (objectType == CommonConstant.ObjectType.user)
            {
                var query = from p in db.privileges
                            join ur in db.user_roles on p.object_id equals ur.role_id
                            join s in db.sections on p.section_id equals s.section_id
                            where (
                                ur.user_id == objectID
                                 && s.module.ToUpper() == module.ToUpper()
                                 && p.object_type == objectType
                            )
                            select new RolePrivilegeObject
                            {
                                privilege_id = p.privilege_id,
                                object_type = p.object_type,
                                object_id = p.object_id,
                                section_id = p.section_id,
                                read_status = p.read_status,
                                insert_status = p.insert_status,
                                update_status = p.update_status,
                                delete_status = p.delete_status,
                                status = p.status,
                                crt_date = p.crt_date,
                                upd_date = p.upd_date,
                                crt_by = p.crt_by,
                                upd_by = p.upd_by
                            };

                var resultList = query.ToList();

                foreach (var access in resultList)
                {
                    if (access.insert_status == 1)
                    {
                        specificPrivilege.insert_status = 1;
                    }

                    if (access.update_status == 1)
                    {
                        specificPrivilege.update_status = 1;
                    }

                    if (access.read_status == 1)
                    {
                        specificPrivilege.read_status = 1;
                    }

                    if (access.delete_status == 1)
                    {
                        specificPrivilege.delete_status = 1;
                    }
                }
            }
            return specificPrivilege;
        }

        // access manager no need check permission
        public CommonConstant.SystemCode CreateAndGenerateToken(
            int accessObjectType,
            int accessObjectID,
            ref AccessTokenObject accessTokenObject
        )
        {
            var token = RandomManager.GenerateNumberAndLetter(12);
            
            var systemCode = CommonConstant.SystemCode.undefine;
            int? sql_result = 0;
            int? get_new_obj_id = 0;

            var result = db.sp_CreateAccessToken(
                accessObjectType,
                accessObjectID,

                token,
                   
                ref get_new_obj_id,
                ref sql_result
                );

            systemCode = (CommonConstant.SystemCode)sql_result.Value;

            var new_obj_id = get_new_obj_id.Value;
            accessTokenObject = new AccessTokenObject(){
                rec_id = new_obj_id,
                token = token,
                crt_by_type = accessObjectType,
                crt_by = accessObjectID
            };

            return systemCode;
        }

        //public bool AccessModule(string module, int action_type, ref string sql_remark)
        //{
        //    bool access_result = false;

        //    bool get_result = false;
        //    SystemObjectManager systemObjectManager = new SystemObjectManager();
        //    var theObject = systemObjectManager.GetSystemObject_detail(SessionManager.Current.obj_id, SessionManager.Current.obj_id, ref get_result);

        //    if (theObject.type == CommonConstant.ObjectType.user)
        //    {
        //        var roleAccessManager = new RoleAccessManager();
        //        var accessList = roleAccessManager.GetUserSectionAccess(SessionManager.Current.obj_id, module);

        //        foreach (var access in accessList)
        //        {
        //            if (action_type == CommonConstant.ActionType.create && access.insert_status == 1)
        //            {
        //                access_result = true;
        //                break;
        //            }
        //            else if (action_type == CommonConstant.ActionType.update && access.update_status == 1)
        //            {
        //                access_result = true;
        //                break;
        //            }
        //            else if (action_type == CommonConstant.ActionType.read && access.read_status == 1)
        //            {
        //                access_result = true;
        //                break;
        //            }
        //            else if (action_type == CommonConstant.ActionType.delete && access.delete_status == 1)
        //            {
        //                access_result = true;
        //                break;
        //            }
        //            else
        //                sql_remark = CommonConstant.SystemWord[1111];
        //        }
        //    }
        //    else
        //        access_result = true;

        //    return access_result;
        //}
    }
}