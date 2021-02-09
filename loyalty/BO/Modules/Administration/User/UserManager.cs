using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Palmary.Loyalty.BO.Database;

using System.Data.Objects;
using HashLib;

using Palmary.Loyalty.Common;
using Palmary.Loyalty.BO.Modules.Administration.Security;
using Palmary.Loyalty.BO.Modules.Administration.SystemControl;

using Palmary.Loyalty.BO.Modules.Utility;
using Palmary.Loyalty.BO.DataTransferObjects.Security;
using Palmary.Loyalty.BO.DataTransferObjects.Form;
using Palmary.Loyalty.BO.DataTransferObjects.User;

using System.Configuration;
using System.Data.Linq;
using System.Linq.Expressions;
using Palmary.Loyalty.BO.DataTransferObjects.DB;
using Palmary.Loyalty.BO.DataTransferObjects.Role;

using Palmary.Loyalty.BO.Modules.Administration.Role;

namespace Palmary.Loyalty.BO.Modules.Administration.User
{
    public class UserManager
    {
        private static LoyaltyDBDataContext db = new LoyaltyDBDataContext();
        private AccessManager _accessManager;
        private string _module = CommonConstant.Module.user;
        private RolePrivilegeObject _privilege;
        private static LogManager _logManager;
        private AccessObject _accessObject;

        // For backend, using BO Session to access
        public UserManager()
        {
            // access object
            _accessManager = new AccessManager();
            _accessObject = _accessManager.GetAccessObjectBySession();

            // privilege and log
            _privilege = _accessManager.AccessModule(_module);
            _logManager = new LogManager();
        }

        // For Webservices or other not using BO Session to access
        public UserManager(AccessObject accessObject)
        {
            // access object
            _accessManager = new AccessManager();
            _accessObject = accessObject;

            // privilege and log
            _privilege = _accessManager.AccessModule(_module, _accessObject.type, _accessObject.id);
            _logManager = new LogManager(_accessObject);
        }

        public CommonConstant.SystemCode Create(
            UserObject userObj,

            ref int new_user_id)
        {
            // LINQ to Store Procedures
            var systemCode = CommonConstant.SystemCode.undefine;

            if (_privilege.insert_status == 1)
            {
                var dataValid = true;
                // check valid
                // login_id
                var exist_object = GetDetail(0, userObj.login_id, null, true, ref systemCode);

                if (exist_object.user_id > 0)
                {
                    systemCode = CommonConstant.SystemCode.err_loginId_exist;
                    dataValid = false;
                }
                else
                {
                    // email
                    exist_object = GetDetail(0, null, userObj.email, true, ref systemCode);
                    if (exist_object.user_id > 0)
                    {
                        systemCode = CommonConstant.SystemCode.err_email_exist;
                        dataValid = false;
                    }
                }

                if (dataValid)
                {
                    // [START] Create object into object table
                    //var systemObjectManager = new SystemObjectManager();
                    //var new_object_id = 0;
                    //var new_object_sql_remark = "";

                    //var object_name = userObj.name;
                    //var power_search_content = new List<string>();
                    //if (!String.IsNullOrWhiteSpace(userObj.login_id)) power_search_content.Add(userObj.login_id);
                    //if (!String.IsNullOrWhiteSpace(userObj.name)) power_search_content.Add(userObj.name);
                    //if (!String.IsNullOrWhiteSpace(userObj.email)) power_search_content.Add(userObj.email);
                    //var power_search = String.Join(" ", power_search_content.ToArray());

                    //var create_result = systemObjectManager.Create(
                    //   SessionManager.Current.obj_id,

                    //   Common.CommonConstant.ObjectType.user,
                    //   object_name,
                    //   userObj.status,
                    //   power_search,
                    //   ref new_object_id,
                    //   ref new_object_sql_remark
                    // );
                    // [END] Create Object


                    int? sql_result = 0;
                    var sql_remark = "";

                    // create user
                    //var encryptedPassword = AccessManager.encryptPassword(userObj.password, new_object_id.ToString());
                    int? new_obj_id = 0;

                    var result = db.sp_CreateUser(
                        _accessObject.id,
 _accessObject.type,

                        //new_object_id,
                        userObj.login_id,
                        userObj.name,
                        userObj.email,
                        userObj.password,
                        userObj.status,

                        ref new_obj_id,
                        ref sql_result);

                    systemCode = (CommonConstant.SystemCode)sql_result.Value;

                    if (systemCode == CommonConstant.SystemCode.normal)
                    {
                        new_user_id = new_obj_id.Value;
                        userObj.user_id = new_obj_id.Value;
                        // update with encrypted password
                        ChangedField[] theChangedFields = new ChangedField[0];
                        Update(userObj, theChangedFields);

                        // role
                        var userRoleLinkManager = new UserRoleLinkManager();
                        foreach (var role in userObj.role_list)
                        {
                            var addFlag = userRoleLinkManager.Create(new_user_id, role.role_id, CommonConstant.Status.active, ref sql_remark);
                        }

                        // Take Log with detail
                        var oldObj = new UserObject() { user_id = userObj.user_id };
                        LogAndCompare(CommonConstant.ActionType.create, oldObj, userObj);

                        systemCode = CommonConstant.SystemCode.normal;
                    }
                    else
                    {  
                        systemCode = CommonConstant.SystemCode.record_invalid;
                    }
                }
            }
            else
            {
                systemCode = CommonConstant.SystemCode.no_permission;
            }

            return systemCode;
        }

        public List<UserObject> GetList(int rowIndexStart, int rowLimit, List<SearchParmObject> searchParmList, string sortColumn, CommonConstant.SortOrder sortOrder, ref CommonConstant.SystemCode systemCode, ref int totalRow)
        {
            //test
           // var plainText = "palmary)^23427972*!MC0001@palmary.com.hk&#20150615$!kenneth(#leo%!raymond@(anthonywong&~jeff{}pang;'++palmary)^23427972*!MC0001@palmary.com.hk&#20150615$!kenneth(#leo%!raymond@(anthonywong&~jeff{}pang;'";
            var plainText = "20150622180001MC000112345678";
            var plainText2 = "20150622180001123456712345678";
            var a = CryptographyManager.EncryptAES256_CBC(CommonConstant.Cryptography.tl_key, CommonConstant.Cryptography.tl_iv, plainText);
            System.Diagnostics.Debug.WriteLine(a);
            var a2 = CryptographyManager.EncryptAES256_CBC(CommonConstant.Cryptography.tl_key, CommonConstant.Cryptography.tl_iv, plainText2);
            System.Diagnostics.Debug.WriteLine(a2);
            var b = CryptographyManager.DecryptAES256_CBC(CommonConstant.Cryptography.tl_key, CommonConstant.Cryptography.tl_iv, a);
            System.Diagnostics.Debug.WriteLine(b);
           // var a3 = Convert.ToBase64String(Encoding.Unicode.GetBytes(a2));
            //var b = CryptographyManager.EncryptRJ256_ECB(CommonConstant.Cryptography.tl_key, "", "abcdef123456adasdasdacxzcswew");
           // var b1 = b.Length;
            //end test

            // LINQ to SQL
            systemCode = CommonConstant.SystemCode.undefine;
            var resultList = new List<UserObject>();

            if (_privilege.read_status == 1)
            {
                var query = (from u in db.user_profiles
                             join li in db.listing_items on  u.status equals li.value
                             join l in db.listings on li.list_id equals l.list_id
                             where (
                                u.record_status != (int)CommonConstant.RecordStatus.deleted
                                && l.code == "Status")
                             select new UserObject
                             {
                                 user_id = u.user_id,
                                 login_id = u.login_id,
                                 name = u.name,
                                 password = u.password,
                                 email = u.email,
                                 action_ip = u.action_ip,
                                 action_date = u.action_date,

                                 status = u.status,
                                 status_name = li.name,

                                 crt_date = u.crt_date,
                                 crt_by_type = u.crt_by_type,
                                 crt_by = u.crt_by,
                                 upd_date = u.upd_date,
                                 upd_by_type = u.upd_by_type,
                                 upd_by = u.upd_by,

                                 record_status = u.record_status
                             });

                // dynamic search
                foreach (var f in searchParmList)
                {
                    if (!String.IsNullOrEmpty(f.value))
                    {
                        if (f.property == "name")
                        {
                            query = query.Where(x => x.name.ToLower().Contains(f.value.ToLower()));
                        }
                        else if (f.property == "email")
                        {
                            query = query.Where(x => x.email.Contains(f.value));
                        }
                        else if (f.property == "login_id")
                        {
                            query = query.Where(x => x.login_id.Contains(f.value));
                        }
                        else if (f.property == "status_name")
                        {
                            if (!String.IsNullOrEmpty(f.value))
                                query = query.Where(x => x.status == int.Parse(f.value));
                        }
                    }
                }

                // dynamic sort
                var orderByColumn = "";
                if (sortColumn == "email" || sortColumn == "name"
                    || sortColumn == "status_name" || sortColumn == "login_id")
                    orderByColumn = sortColumn;
                else
                { //default
                    sortOrder = CommonConstant.SortOrder.asc;
                    orderByColumn = "login_id";
                }

                // row total
                totalRow = query.Count();

                if (sortOrder == CommonConstant.SortOrder.desc)
                    resultList = query.OrderBy(orderByColumn, true).Skip(rowIndexStart).Take(rowLimit).ToList();
                else
                    resultList = query.OrderBy(orderByColumn, false).Skip(rowIndexStart).Take(rowLimit).ToList();

                systemCode = CommonConstant.SystemCode.normal;
            }
            else
            {
                systemCode = CommonConstant.SystemCode.no_permission;
            }

            return resultList;
        }

        public UserObject GetDetail(int? search_user_id, string search_user_login_id, string search_user_email, bool rootAccess, ref CommonConstant.SystemCode systemCode)
        {
            // LINQ to SQL
            systemCode = CommonConstant.SystemCode.undefine;
            var resultObj = new UserObject();

            if (_privilege.read_status == 1 || rootAccess)
            {
                var query = (from u in db.user_profiles
                             
                             join li in db.listing_items on u.status equals li.value
                             join l in db.listings on li.list_id equals l.list_id
                             where (
                                u.record_status != (int)CommonConstant.RecordStatus.deleted
                                && l.code == "Status"
                                && (u.user_id == search_user_id || u.login_id == search_user_login_id || u.email == search_user_email)
                            )
                             select new UserObject
                             {
                                 user_id = u.user_id,
                                 login_id = u.login_id,
                                 name = u.name,
                                 password = u.password,
                                 email = u.email,
                                 action_ip = u.action_ip,
                                 action_date = u.action_date,
                                 status = u.status,

                                 crt_date = u.crt_date,
                                 crt_by_type = u.crt_by_type,
                                 crt_by = u.crt_by,
                                 upd_date = u.upd_date,
                                 upd_by_type = u.upd_by_type,
                                 upd_by = u.upd_by,

                                 record_status = u.record_status,

                                 // additional
                                 status_name = li.name
                             });


                if (query.Count() > 0)
                {
                    resultObj = query.FirstOrDefault();
                    systemCode = CommonConstant.SystemCode.normal;
                }
                else
                    systemCode = CommonConstant.SystemCode.record_invalid;
            }
            else
            {
                systemCode = CommonConstant.SystemCode.no_permission;
            }

            return resultObj;
        }

        public CommonConstant.SystemCode Update(
            UserObject obj,
            ChangedField[] changedFields
            )
        {
            // LINQ to Store Procedures

            var systemCode = CommonConstant.SystemCode.undefine;

            if (_privilege.update_status == 1)
            {
                int? sql_result = 0;
                string hashedPass = "";

                // check valid
                var dataValid = true;
                var existObj = GetDetail(0, null, obj.email, true, ref systemCode);
                if (existObj.user_id > 0 && existObj.user_id != obj.user_id)
                {
                    systemCode = CommonConstant.SystemCode.err_email_exist;
                    dataValid = false;
                }

                if (dataValid)
                {
                    // cache original data
                    var oldObject = GetDetail(obj.user_id, "", null, true, ref systemCode);
                    var roleAccessManager = new RoleAccessManager();
                    oldObject.role_list = roleAccessManager.GetListOwnByUser(_accessObject.id, obj.user_id);

                    if (String.IsNullOrEmpty(obj.password)) // didnt change, keep old password
                    {
                        var user_org = GetDetail(obj.user_id, null, null, true, ref systemCode);
                        hashedPass = user_org.password;
                    }
                    else
                    {
                        hashedPass = AccessManager.encryptPassword(obj.password, obj.user_id.ToString());
                    }

                    var result = db.sp_UpdateUser(
                       
                           _accessObject.type, _accessObject.id,
                        obj.user_id,
                        obj.login_id,
                        obj.name,
                        hashedPass,
                        obj.email,
                        obj.action_ip,
                        obj.action_date,
                        obj.status,
                        ref sql_result);

                    systemCode = (CommonConstant.SystemCode)sql_result.Value;

                    if (systemCode == CommonConstant.SystemCode.normal)
                    {
                        // Update role
                        var sql_remark = "";

                        var userRoleLinkManager = new UserRoleLinkManager();
                        var flag = userRoleLinkManager.Delete(obj.user_id, ref sql_remark);

                        foreach (var role in obj.role_list)
                        {
                            flag = userRoleLinkManager.Create(obj.user_id, role.role_id, CommonConstant.Status.active, ref sql_remark);
                        }

                        // Update object table
                        //SystemObjectManager systemObjectManager = new SystemObjectManager();

                        //var object_name = obj.name;
                        //var power_search_content = new List<string>();
                        //if (!String.IsNullOrWhiteSpace(obj.login_id)) power_search_content.Add(obj.login_id);
                        //if (!String.IsNullOrWhiteSpace(obj.name)) power_search_content.Add(obj.name);
                        //if (!String.IsNullOrWhiteSpace(obj.email)) power_search_content.Add(obj.email);
                        //var power_search = String.Join(" ", power_search_content.ToArray());

                        //var sql_update_obj_remark = "";
                        //var so_result = systemObjectManager.Update(
                        //    SessionManager.Current.obj_id, //access_user_id
                        //    obj.user_id,        //object_id
                        //    object_name,
                        //    obj.status,         //status
                        //    power_search,   //power_search
                        //    ref sql_update_obj_remark
                        //);

                        // Take Log with detail
                        LogAndCompare(CommonConstant.ActionType.update, oldObject, obj);

                        systemCode = CommonConstant.SystemCode.normal;
                    }
                }
            }
            else
            {
                systemCode = CommonConstant.SystemCode.no_permission;
            }

            return systemCode;
        }

        // log with change detail
        public CommonConstant.SystemCode LogAndCompare(int action, UserObject oldObj, UserObject newObj)
        {
            var result = CommonConstant.SystemCode.undefine;

            if (oldObj.user_id != newObj.user_id || oldObj.user_id == 0 || newObj.user_id == 0)
                result = CommonConstant.SystemCode.record_invalid;
            else
            { // valid objects
                var logDetailList = new List<LogDetailObject>();

                if (oldObj.login_id != newObj.login_id)
                {
                    logDetailList.Add(new LogDetailObject()
                    {
                        field_name = UtilityManager.GetPropertyDisplayName(() => newObj.login_id),
                        old_value = oldObj.login_id,
                        new_value = newObj.login_id
                    });
                }

                if (oldObj.name != newObj.name)
                {
                    logDetailList.Add(new LogDetailObject()
                    {
                        field_name = UtilityManager.GetPropertyDisplayName(() => newObj.name),
                        old_value = oldObj.name,
                        new_value = newObj.name
                    });
                }

                if (!String.IsNullOrEmpty(newObj.password))
                {
                    logDetailList.Add(new LogDetailObject()
                    {
                        field_name = UtilityManager.GetPropertyDisplayName(() => newObj.password),
                        old_value = "***",
                        new_value = "***"
                    });
                }

                if (oldObj.email != newObj.email)
                {
                    logDetailList.Add(new LogDetailObject()
                    {
                        field_name = UtilityManager.GetPropertyDisplayName(() => newObj.email),
                        old_value = oldObj.email,
                        new_value = newObj.email
                    });
                }
            
                if (oldObj.status != newObj.status)
                {
                    logDetailList.Add(new LogDetailObject()
                    {
                        field_name = UtilityManager.GetPropertyDisplayName(() => newObj.status),
                        old_value = oldObj.status_name,
                        new_value = newObj.status.ToListingItemName("status")
                    });
                }

                if (oldObj.role_list != newObj.role_list)
                {
                    var old_valueList = new List<string> { };
                    var new_valueList = new List<string> { };

                    foreach (var x in oldObj.role_list)
                    {
                        old_valueList.Add(x.name);
                    }

                    if (newObj.role_list.Count > 0)
                    {
                        var roleManager = new RoleManager();
                        var systemCode = CommonConstant.SystemCode.undefine;
                        foreach (var x in newObj.role_list)
                        {
                            var theRole = roleManager.GetDetail(x.role_id, true, ref systemCode);
                            new_valueList.Add(theRole.name);
                        }
                    }

                    logDetailList.Add(new LogDetailObject()
                    {
                        field_name = UtilityManager.GetPropertyDisplayName(() => newObj.role_list),
                        old_value = string.Join(",", old_valueList.ToArray()),
                        new_value = string.Join(",", new_valueList.ToArray())
                    });
                }

                // log with change detail
                _logManager.LogObject(action, CommonConstant.ObjectType.user, newObj.user_id, newObj.name, logDetailList);
            }
            return result;
        }

        public CommonConstant.SystemCode SoftDelete(int user_id, ref string sql_remark)
        {
            // LINQ to Store Procedures
            var systemCode = CommonConstant.SystemCode.undefine;

            if (_privilege.delete_status == 1)
            {
                int? get_sql_result = 0;

                var userObj = GetDetail(user_id, null, null, true, ref systemCode);
                var flag = false;

                if (userObj.user_id > 0)
                {
                    db.sp_SoftDeleteByModule(_accessObject.id, _accessObject.type,  CommonConstant.Module.user, user_id, ref get_sql_result, ref sql_remark);
                    flag = (int)get_sql_result.Value == 1 ? true : false;

                    //if (delete_result)
                    //{
                    //    // also need to soft delete in system object table 
                    //    var systemObjectManager = new SystemObjectManager();
                    //    flag = systemObjectManager.SoftDelete(SessionManager.Current.obj_id, user_id);
                    //}
                }

                if (flag)
                {
                    systemCode = CommonConstant.SystemCode.normal;

                    // take log
                    _logManager.LogObject(CommonConstant.ActionType.delete, CommonConstant.ObjectType.user, userObj.user_id, userObj.name);
                }
                else
                    systemCode = CommonConstant.SystemCode.record_invalid;
            }
            else
                systemCode = CommonConstant.SystemCode.no_permission;

            return systemCode;
        }

        // Update current access user's password
        public bool UpdatePassword(int user_id, string oldPassword, string newPassword)
        {
            string hashedOldPass = AccessManager.encryptPassword(oldPassword, user_id.ToString());
            string hashedPass = AccessManager.encryptPassword(newPassword, user_id.ToString());

            int? status = 0;
            var remark = "";

            db.sp_UpdateUserPassword(user_id, _accessObject.id, _accessObject.type, hashedOldPass, hashedPass, ref status, ref remark);

            return (int.Parse(status.Value.ToString()) == 1 ? true : false);
        }

        public bool CheckDuplicateLoginID(string login_id)
        {
            var query = from u in db.user_profiles
                        where u.login_id == login_id
                        select u;

            if (query.Count() > 0)
            {
                return true;
            }
            else
                return false;
        }

        public bool CheckDuplicateEmail(string email)
        {
            var query = from u in db.user_profiles
                        where u.email == email
                        select u;

            if (query.Count() > 0)
            {
                return true;
            }
            else
                return false;
        }

    }
}