using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data.Objects;
using Palmary.Loyalty.Common;
using Palmary.Loyalty.BO.Database;
using Palmary.Loyalty.BO.DataTransferObjects.DB;
using Palmary.Loyalty.BO.DataTransferObjects.Security;
using Palmary.Loyalty.BO.Modules.Administration.Security;
using Palmary.Loyalty.BO.Modules.Administration.SystemControl;
using Palmary.Loyalty.BO.Modules.Transaction;
using Palmary.Loyalty.BO.DataTransferObjects.Form;
using Palmary.Loyalty.BO.DataTransferObjects.Member;
using Palmary.Loyalty.BO.DataTransferObjects.Role;

using Palmary.Loyalty.BO.Modules.Utility;

namespace Palmary.Loyalty.BO.Modules.Member
{
    public class MemberManager
    {
        private static LoyaltyDBDataContext db = new LoyaltyDBDataContext();
        private AccessManager _accessManager;
        private string _module = CommonConstant.Module.member;

        private RolePrivilegeObject _privilege;
        private static LogManager _logManager;
        private AccessObject _accessObject;

        // For backend, using BO Session to access
        public MemberManager()
        {
            // access object from session
            _accessManager = new AccessManager();
            _accessObject = _accessManager.GetAccessObjectBySession();

            // privilege and log
            _privilege = _accessManager.AccessModule(_module);
            _logManager = new LogManager();
        }

        // For Webservices or other not using BO Session to access
        public MemberManager(AccessObject accessObject)
        {
            // access object from passing in
            _accessManager = new AccessManager();
            _accessObject = accessObject;

            // privilege and log
            _privilege = _accessManager.AccessModule(_module, _accessObject.type, _accessObject.id);
            _logManager = new LogManager(_accessObject);
        }

        public IEnumerable<sp_GetMemberListsResult> GetMemberList_sp(int user_id, long rowIndexStart, int rowLimit, string searchParams)
        {
            int? get_sql_result = 0;
            var sql_remark = "";

            var result = db.sp_GetMemberLists(user_id, rowIndexStart, rowLimit, searchParams, ref get_sql_result, ref sql_remark);

            return result;
        }

        public List<MemberObject> GetList(int rowIndexStart, int rowLimit, List<SearchParmObject> searchParmList, string sortColumn, CommonConstant.SortOrder sortOrder, ref CommonConstant.SystemCode systemCode, ref int totalRow)
        {
            var resultList = new List<MemberObject>();
           
            if (_privilege.read_status == 1)
            {
                var query = (from m in db.member_profiles
                             join ml_l in db.member_level_langs on m.member_level_id equals ml_l.level_id
                             where (
                                m.record_status != (int)CommonConstant.RecordStatus.deleted
                                && ml_l.lang_id == (int)CommonConstant.LangCode.en
                            )
                             select new MemberObject
                             {
                                 member_id = m.member_id,
                                 member_no = m.member_no,
                                 password = m.password,
                                 email = m.email,
                                 fbid = m.fbid,
                                 fbemail = m.fbemail,
                                 mobile_no = m.mobile_no,
                                 salutation = m.salutation,
                                 firstname = m.firstname,
                                 middlename = m.middlename,
                                 lastname = m.lastname,
                                 fullname = m.fullname, 
                                 birth_year = m.birth_year,
                                 birth_month = m.birth_month,
                                 birth_day = m.birth_day,
                                 gender = m.gender,
                                 hkid = m.hkid,
                                 address1 = m.address1,
                                 address2 = m.address2,
                                 address3 = m.address3,
                                 district = m.district,
                                 region = m.region,
                                 reg_source = m.reg_source,
                                 referrer = m.referrer,
                                 reg_status = m.reg_status,
                                 reg_ip = m.reg_ip,
                                 activate_key = m.activate_key,
                                 hash_key = m.hash_key,
                                 session = m.session,
                                 status = m.status,
                                 opt_in = m.opt_in,
                                 member_level_id = m.member_level_id,
                                 member_category_id = m.member_category_id,
                                 available_point = m.available_point,
                                 crt_date = m.crt_date,
                                 upd_date = m.upd_date,
                                 crt_by_type = m.crt_by_type,
                                 crt_by = m.crt_by,
                                 upd_by_type = m.upd_by_type,
                                 upd_by = m.upd_by,
                                 record_status = m.record_status,

                                 //-- additional info
                                 member_level_name = ml_l.name
                             });

                // dynamic search
                foreach (var f in searchParmList)
                {
                    if (f.property == "member_no")
                    {
                        query = query.Where(x => x.member_no.ToLower().Contains(f.value.ToLower()));
                    }
                    else if (f.property == "name")
                    {
                        query = query.Where(x => x.fullname.ToLower().Contains(f.value.ToLower()));
                    }
                    else if (f.property == "email")
                    {
                        query = query.Where(x => x.email.Contains(f.value));
                    }
                    else if (f.property == "hkid")
                    {
                        query = query.Where(x => x.hkid.Contains(f.value));
                    }
                    else if (f.property == "member_no")
                    {
                        query = query.Where(x => x.member_no.Contains(f.value));
                    }
                    else if (f.property == "available_point_larger")
                    {
                        if (!String.IsNullOrEmpty(f.value))
                            query = query.Where(x => x.available_point >= double.Parse(f.value));
                    }
                    else if (f.property == "available_point_lower")
                    {
                        if (!String.IsNullOrEmpty(f.value))
                            query = query.Where(x => x.available_point <= double.Parse(f.value));
                    }
                    else if (f.property == "mobile_no")
                    {
                        query = query.Where(x => x.mobile_no.Contains(f.value));
                    }
                    else if (f.property == "member_level_name")
                    {
                        if (!String.IsNullOrEmpty(f.value))
                            query = query.Where(x => x.member_level_id == int.Parse(f.value));
                    }
                    else if (f.property == "member_category")
                    {
                        if (!String.IsNullOrEmpty(f.value))
                            query = query.Where(x => x.member_category_id == int.Parse(f.value));
                    }
                    else if (f.property == "gender")
                    {
                        if (!String.IsNullOrEmpty(f.value))
                            query = query.Where(x => x.gender == int.Parse(f.value));
                    }
                    else if (f.property == "status")
                    {
                        if (!String.IsNullOrEmpty(f.value))
                            query = query.Where(x => x.status == int.Parse(f.value));
                    }
                    else if (f.property == "crt_date_from")
                    {
                        if (!String.IsNullOrEmpty(f.value))
                            query = query.Where(x => x.crt_date >= Convert.ToDateTime(f.value));
                    }
                    else if (f.property == "crt_date_to")
                    {
                        if (!String.IsNullOrEmpty(f.value))
                            query = query.Where(x => x.crt_date < Convert.ToDateTime(f.value).AddDays(1));
                    }
                }

                // dynamic sort
                var orderByColumn = "";
                if (sortColumn == "member_no")
                    orderByColumn = sortColumn;
                else if (sortColumn == "name")
                    orderByColumn = "fullname"; //
                else if (sortColumn == "email")
                    orderByColumn = sortColumn;
                else if (sortColumn == "hkid")
                    orderByColumn = sortColumn;
                else if (sortColumn == "mobile_no")
                    orderByColumn = sortColumn;
                else if (sortColumn == "available_point")
                    orderByColumn = sortColumn;
                else if (sortColumn == "member_level_name")
                    orderByColumn = sortColumn;
                else
                { //default
                    sortOrder = CommonConstant.SortOrder.asc;
                    orderByColumn = "member_no";
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
                resultList = new List<MemberObject>();
                systemCode = CommonConstant.SystemCode.no_permission;
            }

            return resultList;
        }

        public List<MemberObject> GetListAll(List<SearchParmObject> searchParmList, ref CommonConstant.SystemCode systemCode)
        {
            List<MemberObject> resultList;

            if (_privilege.read_status == 1)
            {
                var query = (from m in db.member_profiles
                             join ml_l in db.member_level_langs on m.member_level_id equals ml_l.level_id
                             where (
                                m.record_status != (int)CommonConstant.RecordStatus.deleted
                                && ml_l.lang_id == (int)CommonConstant.LangCode.en
                            )
                             select new MemberObject
                             {
                                 member_id = m.member_id,
                                 member_no = m.member_no,
                                 password = m.password,
                                 email = m.email,
                                 fbid = m.fbid,
                                 fbemail = m.fbemail,
                                 mobile_no = m.mobile_no,
                                 salutation = m.salutation,
                                 firstname = m.firstname,
                                 middlename = m.middlename,
                                 lastname = m.lastname,
                                 fullname = m.fullname, 
                                 birth_year = m.birth_year,
                                 birth_month = m.birth_month,
                                 birth_day = m.birth_day,
                                 gender = m.gender,
                                 hkid = m.hkid,
                                 address1 = m.address1,
                                 address2 = m.address2,
                                 address3 = m.address3,
                                 district = m.district,
                                 region = m.region,
                                 reg_source = m.reg_source,
                                 referrer = m.referrer,
                                 reg_status = m.reg_status,
                                 reg_ip = m.reg_ip,
                                 activate_key = m.activate_key,
                                 hash_key = m.hash_key,
                                 session = m.session,
                                 status = m.status,
                                 opt_in = m.opt_in,
                                 member_level_id = m.member_level_id,
                                 member_category_id = m.member_category_id,
                                 available_point = m.available_point,
                                 crt_date = m.crt_date,
                                 upd_date = m.upd_date,
                                 crt_by_type = m.crt_by_type,
                                 crt_by = m.crt_by,
                                 upd_by_type = m.upd_by_type,
                                 upd_by = m.upd_by,
                                 record_status = m.record_status,

                                 //-- additional info
                                 member_level_name = ml_l.name
                             });


                // dynamic search
                foreach (var f in searchParmList)
                {
                    if (f.property == "member_no")
                    {
                        query = query.Where(x => x.member_no.ToLower().Contains(f.value.ToLower()));
                    }
                    else if (f.property == "name")
                    {
                        query = query.Where(x => x.fullname.ToLower().Contains(f.value.ToLower()));
                    }
                    else if (f.property == "email")
                    {
                        query = query.Where(x => x.email.Contains(f.value));
                    }
                    else if (f.property == "hkid")
                    {
                        query = query.Where(x => x.hkid.Contains(f.value));
                    }
                    else if (f.property == "member_no")
                    {
                        query = query.Where(x => x.member_no.Contains(f.value));
                    }
                    else if (f.property == "available_point_larger")
                    {
                        if (!String.IsNullOrEmpty(f.value))
                            query = query.Where(x => x.available_point >= double.Parse(f.value));
                    }
                    else if (f.property == "available_point_lower")
                    {
                        if (!String.IsNullOrEmpty(f.value))
                            query = query.Where(x => x.available_point <= double.Parse(f.value));
                    }
                    else if (f.property == "mobile_no")
                    {
                        query = query.Where(x => x.mobile_no.Contains(f.value));
                    }
                    else if (f.property == "member_level_name")
                    {
                        if (!String.IsNullOrEmpty(f.value))
                            query = query.Where(x => x.member_level_id == int.Parse(f.value));
                    }
                    else if (f.property == "member_category")
                    {
                        if (!String.IsNullOrEmpty(f.value))
                            query = query.Where(x => x.member_category_id == int.Parse(f.value));
                    }
                    else if (f.property == "gender")
                    {
                        if (!String.IsNullOrEmpty(f.value))
                            query = query.Where(x => x.gender == int.Parse(f.value));
                    }
                    else if (f.property == "status")
                    {
                        if (!String.IsNullOrEmpty(f.value))
                            query = query.Where(x => x.status == int.Parse(f.value));
                    }
                    else if (f.property == "crt_date_from")
                    {
                        if (!String.IsNullOrEmpty(f.value))
                            query = query.Where(x => x.crt_date >= Convert.ToDateTime(f.value));
                    }
                    else if (f.property == "crt_date_to")
                    {
                        if (!String.IsNullOrEmpty(f.value))
                            query = query.Where(x => x.crt_date < Convert.ToDateTime(f.value).AddDays(1));
                    }
                }
                
                resultList = query.OrderBy(x => x.member_no).ToList();

                systemCode = CommonConstant.SystemCode.normal;
            }
            else
            {
                resultList = new List<MemberObject>();
                systemCode = CommonConstant.SystemCode.no_permission;
            }

            return resultList;
        }

        public bool CheckDuplicateEmail(string email)
        {
            var result = false;

            var query = (from m in db.member_profiles
                           
                        where (
                            m.record_status != (int)CommonConstant.RecordStatus.deleted
                            && m.email == email
                        )
                        select new MemberObject
                        {
                            member_id = m.member_id,
                        });

            if (query.Count() > 0)
                result = true;

            return result;
        }

        //public sp_GetMemberDetailByResult GetMemberDetailBy_sp(int user_id, int member_id, string session, ref bool sql_result)
        //{
        //    int? get_sql_result = 0;
        //    var sql_remark = "";

        //    var result = db.sp_GetMemberDetailBy(_accessObject.id, member_id, session, ref get_sql_result, ref sql_remark);
            
        //    var totalOut = new ObjectParameter("total", typeof(int));  
        //    var remark = new ObjectParameter("remark", typeof(int));

        //    return result.FirstOrDefault() ?? new sp_GetMemberDetailByResult();
        //}

        public MemberObject GetDetail(int member_id, bool root_access, ref CommonConstant.SystemCode systemCode)
        {
            MemberObject resultObject;

            if (_privilege.read_status == 1 || root_access)
            {
                var query = (from m in db.member_profiles
                             join mll in db.member_level_langs on m.member_level_id equals mll.level_id

                             where (
                                m.record_status != (int)CommonConstant.RecordStatus.deleted
                                && m.member_id == member_id
                                && mll.lang_id == (int)CommonConstant.LangCode.tc
                            )
                             select new MemberObject
                             {
                                 member_id = m.member_id,
                                 member_no = m.member_no,
                                 password = m.password,
                                 email = m.email,
                                 fbid = m.fbid,
                                 fbemail = m.fbemail,
                                 mobile_no = m.mobile_no,
                                 salutation = m.salutation,
                                 firstname = m.firstname,
                                 middlename = m.middlename,
                                 lastname = m.lastname,
                                 fullname = m.fullname, 
                                 birth_year = m.birth_year,
                                 birth_month = m.birth_month,
                                 birth_day = m.birth_day,
                                 gender = m.gender,
                                 hkid = m.hkid,
                                 address1 = m.address1,
                                 address2 = m.address2,
                                 address3 = m.address3,
                                 district = m.district,
                                 region = m.region,
                                 reg_source = m.reg_source,
                                 referrer = m.referrer,
                                 reg_status = m.reg_status,
                                 reg_ip = m.reg_ip,
                                 activate_key = m.activate_key,
                                 hash_key = m.hash_key,
                                 session = m.session,
                                 status = m.status,
                                 opt_in = m.opt_in,
                                 member_level_id = m.member_level_id,
                                 member_category_id = m.member_category_id,
                                 available_point = m.available_point,

                                 crt_date = m.crt_date,
                                 upd_date = m.upd_date,
                                 crt_by_type = m.crt_by_type,
                                 crt_by = m.crt_by,
                                 upd_by_type = m.upd_by_type,
                                 upd_by = m.upd_by,
                                 record_status = m.record_status,

                                 // additional info
                                 member_level_name = mll.name
                             });

                resultObject = query.FirstOrDefault();

                systemCode = CommonConstant.SystemCode.normal;
            }
            else
            {
                resultObject = new MemberObject();
                systemCode = CommonConstant.SystemCode.no_permission;
            }

            return resultObject;
        }

        public MemberObject GetDetail_byMemberNo(string member_no, ref CommonConstant.SystemCode systemCode)
        {
            MemberObject resultObject;

            if (_privilege.read_status == 1)
            {
                var query = (from m in db.member_profiles
                             
                             where (
                                m.record_status != (int)CommonConstant.RecordStatus.deleted
                                && m.member_no == member_no
                            )
                             select new MemberObject
                             {
                                 member_id = m.member_id,
                                 member_no = m.member_no,
                                 password = m.password,
                                 email = m.email,
                                 fbid = m.fbid,
                                 fbemail = m.fbemail,
                                 mobile_no = m.mobile_no,
                                 salutation = m.salutation,
                                 firstname = m.firstname,
                                 middlename = m.middlename,
                                 lastname = m.lastname,
                                 fullname = m.fullname, 
                                 birth_year = m.birth_year,
                                 birth_month = m.birth_month,
                                 birth_day = m.birth_day,
                                 gender = m.gender,
                                 hkid = m.hkid,
                                 address1 = m.address1,
                                 address2 = m.address2,
                                 address3 = m.address3,
                                 district = m.district,
                                 region = m.region,
                                 reg_source = m.reg_source,
                                 referrer = m.referrer,
                                 reg_status = m.reg_status,
                                 reg_ip = m.reg_ip,
                                 activate_key = m.activate_key,
                                 hash_key = m.hash_key,
                                 session = m.session,
                                 status = m.status,
                                 opt_in = m.opt_in,
                                 member_level_id = m.member_level_id,
                                 member_category_id = m.member_category_id,
                                 available_point = m.available_point,
                                 crt_date = m.crt_date,
                                 upd_date = m.upd_date,
                                 crt_by_type = m.crt_by_type,
                                 crt_by = m.crt_by,
                                 upd_by_type = m.upd_by_type,
                                 upd_by = m.upd_by,
                                 record_status = m.record_status,

                             });

                if (query.Count() == 1)
                {
                    resultObject = query.FirstOrDefault();
                    systemCode = CommonConstant.SystemCode.normal;
                }
                else
                {
                    resultObject = new MemberObject();
                    systemCode = CommonConstant.SystemCode.err_member_not_exist;
                }
            }
            else
            {
                resultObject = new MemberObject();
                systemCode = CommonConstant.SystemCode.no_permission;
            }

            return resultObject;
        }

        public bool Update(
            MemberObject m,
            ChangedField[] changedFields,
            ref string sql_remark)
        {
         
            var update_result = false;

            if (_privilege.update_status == 1)
            {
                int? get_sql_result = 0;

                var systemCode = CommonConstant.SystemCode.undefine;
                var existM = GetDetail(m.member_id, true, ref systemCode);

                var password = "";

                if (!String.IsNullOrEmpty(m.password) && m.password != existM.password)
                {
                    password = AccessManager.encryptPassword(m.password, m.member_id.ToString());   
                }
                else
                    password = existM.password;

                var result = db.sp_UpdateMember(
                    _accessObject.id,
                    _accessObject.type, 

                    m.member_id,
                    m.member_no,

                    password,
                    m.email,
                    m.fbid,
                    m.fbemail,
                    m.mobile_no,
                    m.salutation,
                    m.firstname,
                    m.middlename,
                    m.lastname,
                    m.GetFullname(), 
                    m.birth_year,
                    m.birth_month,
                    m.birth_day,
                    m.gender,
                    m.hkid,

                    m.address1,
                    m.address2,
                    m.address3,
                    m.district,
                    m.region,

                    m.reg_source,
                    m.referrer,
                    m.reg_status,
                    m.reg_ip,
                    m.activate_key,
                    m.hash_key,
                    m.status,
                    m.opt_in,
                    m.member_level_id,
                    m.member_category_id,
                    m.available_point,

                    m.crt_by_type,
                    m.crt_by,
                    m.upd_by_type,
                    m.upd_by,

                    ref get_sql_result, ref sql_remark);

                update_result = get_sql_result == 1 ? true : false;

                if (update_result)
                {
                    // Update object table
                    // Update_systemObject(m);

                    // Take Log
                    //LogManager logManager = new LogManager();
                    //var theLog = new LogObject()
                    //{
                    //    action_ip = AccessObject.ip,
                    //    action_channel = AccessObject.action_channel,
                    //    action_type = Common.CommonConstant.ActionType.update,
                    //    target_obj_id = m.member_id,
                    //    target_obj_type_id = null,
                    //    target_obj_name = null,
                    //    action_detail = null
                    //};
                    //logManager.Create(theLog);
                }
            }
            else
            {
                sql_remark = "No Access";
            }

            return update_result;
        }

        // system internal use, skip logging, permission
        public CommonConstant.SystemCode Update_directCore(
            MemberObject m
        )
        {
            int? get_sql_result = 0;
            var sql_remark = "";

            if (m.member_id == 0 && !String.IsNullOrEmpty(m.member_no)) // from import update
            {
                var member_id = GetMemberID(m.member_no);
                if (member_id > 0)
                    m.member_id = member_id;
            }

            var result = db.sp_UpdateMember(
                _accessObject.id,
                _accessObject.type, 

                m.member_id,
                m.member_no,

                m.password,
                m.email,
                m.fbid,
                m.fbemail,
                m.mobile_no,
                m.salutation,
                m.firstname,
                m.middlename,
                m.lastname,
                m.GetFullname(), 

                m.birth_year,
                m.birth_month,
                m.birth_day,
                m.gender,
                m.hkid,

                m.address1,
                m.address2,
                m.address3,
                m.district,
                m.region,

                m.reg_source,
                m.referrer,
                m.reg_status,
                m.reg_ip,
                m.activate_key,
                m.hash_key,
                m.status,
                m.opt_in,
                m.member_level_id,
                m.member_category_id,
                m.available_point,

                m.crt_by_type,
                m.crt_by,
                m.upd_by_type,
                m.upd_by,

                ref get_sql_result, ref sql_remark);

            var update_result = get_sql_result == 1 ? CommonConstant.SystemCode.normal : CommonConstant.SystemCode.record_invalid;

            //if (update_result == CommonConstant.SystemCode.normal)
            //{
            //    // Update object table
            //    Update_systemObject(m);
            //}

            return update_result;
        }

        //public CommonConstant.SystemCode Update_systemObject(MemberObject m)
        //{
        //    // Update object table
        //    SystemObjectManager systemObjectManager = new SystemObjectManager();

        //    var object_name = m.GetFullname();

        //    var power_search_content = new List<string>();
        //    if (!String.IsNullOrWhiteSpace(m.firstname)) power_search_content.Add(m.firstname);
        //    if (!String.IsNullOrWhiteSpace(m.middlename)) power_search_content.Add(m.middlename);
        //    if (!String.IsNullOrWhiteSpace(m.lastname)) power_search_content.Add(m.lastname);
        //    if (!String.IsNullOrWhiteSpace(m.mobile_no)) power_search_content.Add(m.mobile_no);
        //    if (!String.IsNullOrWhiteSpace(m.address1)) power_search_content.Add(m.address1);
        //    if (!String.IsNullOrWhiteSpace(m.address2)) power_search_content.Add(m.address2);
        //    if (!String.IsNullOrWhiteSpace(m.address3)) power_search_content.Add(m.address3);
        //    var power_search = String.Join(" ", power_search_content.ToArray());

        //    var sql_update_obj_remark = "";
        //    var update_result = systemObjectManager.Update(
        //        SessionManager.Current.obj_id,         // access_user_id
        //        m.member_id,   // object_id
        //        object_name,
        //        m.status,    // status
        //        power_search,           // power_search
        //        ref sql_update_obj_remark
        //    );

        //    if (update_result)
        //        return CommonConstant.SystemCode.normal;
        //    else
        //        return CommonConstant.SystemCode.record_invalid;
        //}

        #region Create Member
        public CommonConstant.SystemCode Create(
            MemberObject m,
            ref string sql_remark,
            ref int new_member_id
        )
        {            
            var system_code = CommonConstant.SystemCode.undefine;

            if (_privilege.insert_status == 1)
            {
                var existMember = false;

                if (!String.IsNullOrEmpty(m.member_no))
                {
                    var theExistMemberID = GetMemberID(m.member_no);
                    if (theExistMemberID > 0)
                    {
                        existMember = true;
                        sql_remark = "Member Code Exist";


                        system_code = CommonConstant.SystemCode.err_memberNo_exist;
                
                    }
                }

                if (CheckDuplicateEmail(m.email))
                {
                    existMember = true;
                    system_code = CommonConstant.SystemCode.err_email_exist;
                }

                if (!existMember)
                {
                    int? get_sql_result = 0;
                    
                    // [START] Create object into object table
                    //SystemObjectManager systemObjectManager = new SystemObjectManager();
                    //var new_object_id = 0;
                    //var new_object_sql_remark = "";
                    //var system_object_current_id = ModuleManager.GetTableCurrentIdentity(CommonConstant.Table.system_object);
                    
                    //var power_search_content = new List<string>();
                    //if (!String.IsNullOrWhiteSpace(m.firstname)) power_search_content.Add(m.firstname);
                    //if (!String.IsNullOrWhiteSpace(m.middlename)) power_search_content.Add(m.middlename);
                    //if (!String.IsNullOrWhiteSpace(m.lastname)) power_search_content.Add(m.lastname);
                    //if (!String.IsNullOrWhiteSpace(m.mobile_no)) power_search_content.Add(m.mobile_no);
                    //if (!String.IsNullOrWhiteSpace(m.address1)) power_search_content.Add(m.address1);
                    //if (!String.IsNullOrWhiteSpace(m.address2)) power_search_content.Add(m.address2);
                    //if (!String.IsNullOrWhiteSpace(m.address3)) power_search_content.Add(m.address3);

                    //var power_search = String.Join("", power_search_content.ToArray());

                    //var create_result = systemObjectManager.Create(
                    //   SessionManager.Current.obj_id,

                    //   CommonConstant.ObjectType.member,
                    //   m.GetFullname(),
                    //   CommonConstant.Status.active,
                    //   power_search,
                    //   ref new_object_id,
                    //   ref new_object_sql_remark
                    // );
                    // [END] create object 

                    var passValue = ""; // String.IsNullOrEmpty(m.password) ? "123456" : m.password;
                   // var hashedPass = AccessManager.encryptPassword(passValue, new_object_id.ToString());

                    int? new_obj_id = 0;

                    //if (String.IsNullOrEmpty(m.member_no))
                      //  m.member_no = "SB" + new_object_id;
                    var create_result = false;

                    if (String.IsNullOrEmpty(m.member_no))
                    {
                        m.member_no = m.email;
                    }

                    try
                    {
                        var result = db.sp_CreateMember(
                            _accessObject.id,
                            _accessObject.type, 
                            //new_object_id, //member_id
                            m.member_no,
                            passValue,
                            m.email,
                            m.fbid,
                            m.fbemail,
                            m.mobile_no,
                            m.salutation,
                            m.firstname,
                            m.middlename,
                            m.lastname,
                            m.GetFullname(), 
                            m.birth_year,
                            m.birth_month,
                            m.birth_day,
                            m.gender,
                            m.hkid,

                            m.address1,
                            m.address2,
                            m.address3,
                            m.district,
                            m.region,

                            m.reg_source,
                            m.referrer,
                            m.reg_status,
                            m.reg_ip,
                            m.activate_key,
                            m.hash_key,
                            m.status,
                            m.opt_in,
                            m.member_level_id,
                            m.member_category_id,
                            m.available_point,

                            ref new_obj_id,
                            ref get_sql_result, ref sql_remark);

                        create_result = get_sql_result == 1 ? true : false;
                    }
                    catch (Exception ex)
                    {
                        create_result = false;
                        sql_remark = ex.Message;
                        system_code = CommonConstant.SystemCode.record_invalid;
                    }

                    if (create_result)
                    {
                        new_member_id = new_obj_id.Value;
                        m.member_id = new_obj_id.Value;

                        // set default password and update member no
                        ChangedField[] theChangedField = new ChangedField[0];
                        var the_sql_remark = "";
                        
                        if (String.IsNullOrEmpty(m.password))
                            m.password = "123456";

                        if (m.member_no == m.email)
                        {
                            m.member_no = "MC" + m.member_id.ToString("00000");
                        }

                        Update(m, theChangedField, ref the_sql_remark);

                        // Member Referral point earn
                        if (m.referrer != 0)
                        {
                            var transactionManager = new TransactionManager(_accessObject);
                            system_code = transactionManager.MemberReferralParentGetPoint(m.referrer.Value, m.member_id);

                        }
                        else
                            system_code = CommonConstant.SystemCode.normal;

                        // Take Log
                        //LogManager logManager = new LogManager();
                        //var theLog = new LogObject()
                        //{
                        //    action_ip = AccessObject.ip,
                        //    action_channel = AccessObject.action_channel,
                        //    action_type = Common.CommonConstant.ActionType.create,
                        //    target_obj_id = new_object_id,
                        //    target_obj_type_id = null,
                        //    target_obj_name = null,
                        //    action_detail = null
                        //};
                        //logManager.Create(theLog);

                       
                    }
                }
              
            }
            else
            {
                sql_remark = "No Access";
                system_code = CommonConstant.SystemCode.no_permission;
            }
            return system_code;
        }
        #endregion

        // will depreciate this function
        public bool CreateMemberWithFB(string fbid, string fbemail,
            string firstname,
            string lastname,
            string middlename,
            int birth_year,
            int birth_month,
            int birth_day,
            int gender, ref string sql_remark)
        {
            var member = new MemberObject()
            {
                member_no = fbemail,
                password = "123456",
                email = fbemail,
                fbid = fbid,
                fbemail = fbemail,
                mobile_no = "",
                salutation = 0,
                firstname = firstname,
                middlename = middlename,
                lastname = lastname,
                birth_year = birth_year,
                birth_month = birth_month,
                birth_day = birth_day,
                gender = gender,
                hkid = "",
                address1 = "",
                address2 = "",
                address3 = "",
                district = 0,
                region = 0,
                reg_source = 0,
                referrer = 0,
                reg_status = 0,
                reg_ip = "",
                activate_key = "",
                hash_key = "",
                session = "",
                status = CommonConstant.Status.active,
                opt_in = 0,
                member_level_id = 1,
                member_category_id = 1
            };

            var new_member_id = 0;
            var resultCode = Create(member, ref sql_remark, ref new_member_id);
            var result = false;

            if (resultCode == CommonConstant.SystemCode.normal)
                result = true;

            return result;
        }

        //public bool Login(string login_token, string password, ref int member_id, ref string session)
        //{
        //    int getMember_id = GetMemberID(login_token);
        //    if (getMember_id == -1)
        //        return false;

        //    member_id = getMember_id;

        //    var hashedPass = AccessManager.encryptPassword(password, getMember_id.ToString());
       
        //    int? get_sql_result = 0;
        //    var sql_remark = "";

        //    DateTime time = DateTime.Now;

        //    session = AccessManager.encryptPassword(time.ToString("yyyy-MM-dd_HH:mm:ss"), getMember_id.ToString());

        //    var result = db.sp_MemberLogin(getMember_id, hashedPass, session, ref get_sql_result, ref sql_remark);

        //    // log, use specific accessObject and LogManager
        //    // because currently should be using system
        //    var theAccessObject = _accessObject;
        //    theAccessObject.id = member_id;
        //    theAccessObject.type = CommonConstant.ObjectType.member;
        //    var theLog = new LogManager(theAccessObject);
        //    theLog.LogAction(CommonConstant.ActionType.login);

        //    return get_sql_result == 1 ? true : false;
        //}

        public bool ConnectWithFB(ref MemberObject member)
        {
            var result = false;
            var systemCode = CommonConstant.SystemCode.undefine;

            var existFlag = LoginWithFB(member.fbid);

            if (existFlag == 1) //Existing fbid
                result = true;
            else if (existFlag == 2)  //New fbid
            {
                var sql_remark = "";

                var exist_memberID = GetMemberID(member.email);

                if (exist_memberID > 0) //Exist member, add back fbid
                {
                    var exist_member = GetDetail(exist_memberID, true, ref systemCode);

                    exist_member.fbid = member.fbid;
                    exist_member.fbemail = member.fbemail;

                    result = Update(exist_member, null, ref sql_remark);
                }
                else // New member
                {
                    var new_member_id = 0;
                    systemCode = Create(member, ref sql_remark, ref new_member_id);

                    if (systemCode == CommonConstant.SystemCode.normal)
                        result = true;
                }
            }

            if (existFlag == 1 || existFlag == 2)
            {
                var member_id = GetMemberID(member.fbid);
                member = GetDetail(member_id, true, ref systemCode);
            }
            else
                member = new MemberObject();

            return result;
        }

        private int LoginWithFB(string fbid)
        {
            int? get_sql_result = 0;
            var sql_remark = "";
            var result = db.sp_MemberLoginWithFB(fbid, ref get_sql_result, ref sql_remark);

            //get_sql_result: 2: new fbid
            //get_sql_result: 1: valid fbid
            //get_sql_result: 0: inactive fbid/invalid data (sql exception)
            return int.Parse(get_sql_result.Value.ToString());
        }

        // member_token: member_no, email, mobile_no, fbid
        public int GetMemberID(string member_token)
        {
            var memberID = 0;

            var query = (from m in db.member_profiles
                         
                            where (
                            m.record_status != (int)CommonConstant.RecordStatus.deleted
                            && (m.member_no.ToLower() == member_token.ToLower() || m.email.ToLower() == member_token.ToLower() || m.mobile_no == member_token || m.fbid == member_token)
                        )
                            select new MemberObject
                            {
                                member_id = m.member_id,
                            });
                
            if (query.Count() > 0 )
                memberID = query.FirstOrDefault().member_id;
            
            return memberID;
        }

        public CommonConstant.SystemCode SoftDelete(int member_id, ref string sql_remark)
        {
            // LINQ to Store Procedures
            var systemCode = CommonConstant.SystemCode.undefine;

            if (_privilege.delete_status == 1)
            {
                int? get_sql_result = 0;

                var memberObj = GetDetail(member_id, true, ref systemCode);
                var flag = false;

                if (memberObj.member_id > 0)
                {
                    db.sp_SoftDeleteByModule(_accessObject.id, _accessObject.type, CommonConstant.Module.member, member_id, ref get_sql_result, ref sql_remark);
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
                    _logManager.LogObject(CommonConstant.ActionType.delete, CommonConstant.ObjectType.member, memberObj.member_id, memberObj.fullname);
                }
                else
                    systemCode = CommonConstant.SystemCode.record_invalid;
            }
            else
                systemCode = CommonConstant.SystemCode.no_permission;

            return systemCode;
        }
    }
}
