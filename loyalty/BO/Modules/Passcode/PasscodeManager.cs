using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Palmary.Loyalty.Common;
using Palmary.Loyalty.BO.Database;
using Palmary.Loyalty.BO.DataTransferObjects.Role;
using Palmary.Loyalty.BO.DataTransferObjects.Security;
using Palmary.Loyalty.BO.DataTransferObjects.Passcode;
using Palmary.Loyalty.BO.DataTransferObjects.Product;
using Palmary.Loyalty.BO.Modules.Product;
using Palmary.Loyalty.BO.Modules.Administration.Security;
using System.Data.Objects;
using Palmary.Loyalty.BO.DataTransferObjects.DB;
using Palmary.Loyalty.BO.Modules.Utility;
using Palmary.Loyalty.BO.Modules.Transaction;

namespace Palmary.Loyalty.BO.Modules.Passcode
{
    public class PasscodeManager
    {
        private static LoyaltyDBDataContext db = new LoyaltyDBDataContext();
        private AccessManager _accessManager;
        private string _module = CommonConstant.Module.passcode;
        private RolePrivilegeObject _privilege;
        private static LogManager _logManager;
        private AccessObject _accessObject;

        // For backend, using BO Session to access
        public PasscodeManager()
        {
            // access object
            _accessManager = new AccessManager();
            _accessObject = _accessManager.GetAccessObjectBySession();

            // privilege and log
            _privilege = _accessManager.AccessModule(_module);
            _logManager = new LogManager();
        }

        // For Webservices or other not using BO Session to access
        public PasscodeManager(AccessObject accessObject)
        {
            // access object
            _accessManager = new AccessManager();
            _accessObject = accessObject;

            // privilege and log
            _privilege = _accessManager.AccessModule(_module, _accessObject.type, _accessObject.id);
            _logManager = new LogManager(_accessObject);
        }


        public bool Create(
            int user_id,
            
            string pin_value,
            int generate_id,
            int passcode_prefix_id,
            DateTime active_date,
            DateTime expiry_date,

            int product_id,
            double point,

            int status,
            DateTime? void_date,
            int? void_reason,
            
            ref long new_passcode_id,
            ref string sql_remark
        )
        {
            long? get_passcode_id = 0;
            int? get_sql_result = 0;
      
            var result = db.sp_CreatePasscode(
                    _accessObject.id,
                _accessObject.type, 
                pin_value,
                generate_id,
                passcode_prefix_id,
                active_date,
                expiry_date,

                product_id,
                point,

                status,
                void_date,
                void_reason,                
                
                ref get_passcode_id,
                ref get_sql_result, ref sql_remark);

            new_passcode_id = get_passcode_id.Value;
            return get_sql_result.Value == 1 ? true : false;
        }

        public IEnumerable<sp_GetPasscodeListsResult> GetPasscodeLists_sp(int user_id, int passcode_prefix_id, long rowIndexStart, int rowLimit, string searchParams)
        {
            int? get_sql_result = 0;
            var sql_remark = "";

            IEnumerable<sp_GetPasscodeListsResult> result = null;

            try
            {
                result = db.sp_GetPasscodeLists(SessionManager.Current.obj_id, passcode_prefix_id, rowIndexStart, rowLimit, searchParams, ref get_sql_result, ref sql_remark);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString(), "get passcode error");
            }

            return result;
        }

        public List<PasscodeObject> GetList(int rowIndexStart, int rowLimit, List<SearchParmObject> searchParmList, string sortColumn, CommonConstant.SortOrder sortOrder, ref CommonConstant.SystemCode systemCode, ref int totalRow)
        {
            List<PasscodeObject> resultList;

            if (_privilege.read_status == 1)
            {
                var query = (from p in db.passcodes
                             join pt in db.products on p.product_id equals pt.product_id
                             join p_l in db.product_langs on p.product_id equals p_l.product_id

                             join li in db.listing_items on p.registered equals li.value
                             join l in db.listings on li.list_id equals l.list_id

                             join t in db.transactions on p.passcode_id equals t.source_id into t_table
                             from t in t_table.DefaultIfEmpty() // left outer join

                             join m in db.member_profiles on t.member_id equals m.member_id into m_table
                             from m in m_table.DefaultIfEmpty() // left outer join

                             where (
                                p.record_status != (int)CommonConstant.RecordStatus.deleted
                                && l.code == "YesNo"
                                && p_l.lang_id == _accessObject.languageID
                            )
                             select new PasscodeObject
                             {
                                 passcode_id = p.passcode_id,
                                 pin_value = p.pin_value,
                                 generate_id = p.generate_id,
                                 passcode_prefix_id = p.passcode_prefix_id,
                                 active_date = p.active_date,
                                 expiry_date = p.expiry_date,
                                 point = p.point,
                                 product_id = p.product_id,
                                 status = p.status,
                                 registered = p.registered,
                                 void_date = p.void_date,
                                 void_reason = p.void_reason,
                                 crt_date = p.crt_date,
                                 crt_by_type = p.crt_by_type,
                                 crt_by = p.crt_by,
                                 upd_date = p.upd_date,
                                 upd_by_type = p.upd_by_type,
                                 upd_by = p.upd_by,
                                 record_status = p.record_status,

                                 product_name = p_l.name,
                                 member_id = m.member_id,
                                 member_no = m.member_no,
                                 registered_name = li.name,
                             });

                // dynamic search
                foreach (var f in searchParmList)
                {
                    if (!String.IsNullOrEmpty(f.value))
                    {
                        if (f.property == "pin_value")
                        {
                            query = query.Where(a => a.pin_value.ToLower().Contains(f.value.ToLower()));
                        }
                        else if (f.property == "active_date_str")
                        {
                            var theDate = DateTime.Parse(f.value);
                            query = query.Where(a => a.active_date == theDate);
                        }
                        else if (f.property == "expiry_date_str")
                        {
                            var theDate = DateTime.Parse(f.value);
                            query = query.Where(a => a.expiry_date == theDate);
                        }
                        else if (f.property == "point")
                        {
                            var thePoint = Double.Parse(f.value);
                            query = query.Where(a => a.point == thePoint);
                        }
                        else if (f.property == "member_no")
                        {
                            query = query.Where(a => a.member_no.ToLower().Contains(f.value.ToLower()));
                        }
                    }
                }

                // dynamic Order by
                var orderByColumn = "";
                if (sortColumn == "active_date_str")
                    orderByColumn = "active_date";
                else if (sortColumn == "expiry_date_str")
                    orderByColumn = "expiry_date";
                else if (sortColumn == "pin_value"
                    || sortColumn == "point"
                    || sortColumn == "product_name"
                    || sortColumn == "registered_name"
                    || sortColumn == "member_no")
                    orderByColumn = sortColumn;
                else
                { //default
                    sortOrder = CommonConstant.SortOrder.asc;
                    orderByColumn = "pin_value";
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
                resultList = new List<PasscodeObject>();
                systemCode = CommonConstant.SystemCode.no_permission;
            }

            return resultList;
        }

        public PasscodeObject GetDetail(string pin_value, bool rootAccess, ref CommonConstant.SystemCode systemCode)
        {
            var resultObj = new PasscodeObject();

            if (_privilege.read_status == 1 || rootAccess)
            {
                var query = (from p in db.passcodes
                             join pt in db.products on p.product_id equals pt.product_id
                             join pt_l in db.product_langs on p.product_id equals pt_l.product_id

                             join li in db.listing_items on p.registered equals li.value
                             join l in db.listings on li.list_id equals l.list_id

                             join t in db.transactions on p.passcode_id equals t.source_id into t_table
                             from t in t_table.DefaultIfEmpty() // left outer join

                             join m in db.member_profiles on t.member_id equals m.member_id into m_table
                             from m in m_table.DefaultIfEmpty() // left outer join

                             where (
                                p.record_status != (int)CommonConstant.RecordStatus.deleted
                                && pt_l.lang_id == (int)CommonConstant.LangCode.en
                                && l.code == "YesNo"
                                && p.pin_value == pin_value
                            )
                             select new PasscodeObject
                             {
                                 passcode_id = p.passcode_id,
                                 pin_value = p.pin_value,
                                 generate_id = p.generate_id,
                                 passcode_prefix_id = p.passcode_prefix_id,
                                 active_date = p.active_date,
                                 expiry_date = p.expiry_date,
                                 point = p.point,
                                 product_id = p.product_id,
                                 status = p.status,
                                 registered = p.registered,
                                 void_date = p.void_date,
                                 void_reason = p.void_reason,
                                 crt_date = p.crt_date,
                                 crt_by_type = p.crt_by_type,
                                 crt_by = p.crt_by,
                                 upd_date = p.upd_date,
                                 upd_by_type = p.upd_by_type,
                                 upd_by = p.upd_by,
                                 record_status = p.record_status,

                                 product_name = pt_l.name,
                                 member_id = m.member_id,
                                 member_no = m.member_no,
                                 registered_name = li.name,

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

        public List<PasscodeObject> GetListByPasscodePrefix(int passcode_prefix_id, int rowIndexStart, int rowLimit, List<SearchParmObject> searchParmList, string sortColumn, CommonConstant.SortOrder sortOrder, ref CommonConstant.SystemCode systemCode, ref int totalRow)
        {
            List<PasscodeObject> resultList;

            if (_privilege.read_status == 1)
            {
                var query = (from p in db.passcodes
                             join pt in db.products on p.product_id equals pt.product_id
                             join p_l in db.product_langs on p.product_id equals p_l.product_id

                             join li in db.listing_items on p.registered equals li.value
                             join l in db.listings on li.list_id equals l.list_id

                             join t in db.transactions on p.passcode_id equals t.source_id into t_table
                             from t in t_table.DefaultIfEmpty() // left outer join

                             join m in db.member_profiles on t.member_id equals m.member_id into m_table
                             from m in m_table.DefaultIfEmpty() // left outer join

                             where (
                                p.record_status != (int)CommonConstant.RecordStatus.deleted
                                && l.code == "YesNo"
                                && p.passcode_prefix_id == passcode_prefix_id
                                && p_l.lang_id == _accessObject.languageID
                            )
                             select new PasscodeObject
                             {
                                 passcode_id = p.passcode_id,
                                 pin_value = p.pin_value,
                                 generate_id = p.generate_id,
                                 passcode_prefix_id = p.passcode_prefix_id,
                                 active_date = p.active_date,
                                 expiry_date = p.expiry_date,
                                 point = p.point,
                                 product_id = p.product_id,
                                 status = p.status,
                                 registered = p.registered,
                                 void_date = p.void_date,
                                 void_reason = p.void_reason,
                                 crt_date = p.crt_date,
                                 crt_by_type = p.crt_by_type,
                                 crt_by = p.crt_by,
                                 upd_date = p.upd_date,
                                 upd_by_type = p.upd_by_type,
                                 upd_by = p.upd_by,
                                 record_status = p.record_status,

                                 product_name = p_l.name,
                                 member_id = m.member_id,
                                 member_no = m.member_no,
                                 registered_name = li.name,

                             });


                // dynamic search
                foreach (var f in searchParmList)
                {
                    if (!String.IsNullOrEmpty(f.value))
                    {
                        if (f.property == "pin_value")
                        {
                            query = query.Where(a => a.pin_value.ToLower().Contains(f.value.ToLower()));
                        }
                        else if (f.property == "active_date_str")
                        {
                            var theDate = DateTime.Parse(f.value);
                            query = query.Where(a => a.active_date == theDate);
                        }
                        else if (f.property == "expiry_date_str")
                        {
                            var theDate = DateTime.Parse(f.value);
                            query = query.Where(a => a.expiry_date == theDate);
                        }
                        else if (f.property == "point")
                        {
                            var thePoint = Double.Parse(f.value);
                            query = query.Where(a => a.point == thePoint);
                        }
                        else if (f.property == "member_no")
                        {
                            query = query.Where(a => a.member_no.ToLower().Contains(f.value.ToLower()));
                        }
                    }
                }

                // dynamic Order by
                var orderByColumn = "";
                if (string.IsNullOrEmpty(sortColumn))
                {
                    sortOrder = CommonConstant.SortOrder.asc;
                    orderByColumn = "pin_value";
                }
                else if (sortColumn == "active_date_str")
                    orderByColumn = "active_date";
                else if (sortColumn == "expiry_date_str")
                    orderByColumn = "expiry_date";
                else
                    orderByColumn = sortColumn;

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
                resultList = new List<PasscodeObject>();
                systemCode = CommonConstant.SystemCode.no_permission;
            }

            return resultList;
        }

        public IEnumerable<sp_GetPasscodeUsageSummaryResult> GetPasscodeUsageSummary(int user_id)
        {
            int? get_sql_result = 0;
            var sql_remark = "";

            var result = db.sp_GetPasscodeUsageSummary(user_id, ref get_sql_result, ref sql_remark);
            return result;
        }
 
        public bool Generate(int user_id, int passcode_prefix_id, long noToGenerate, DateTime active_date)
        {
            int? get_sql_result = 0;
            var sql_remark = "";

            //PasscodePrefixManager _passcodePrefixManager = new PasscodePrefixManager();
            //PasscodeFormatManager _passcodeFormatManager = new PasscodeFormatManager();
            //System.Diagnostics.Debug.WriteLine("PasscodeManager - Generate");
            //System.Diagnostics.Debug.WriteLine(user_id, "user_id");
            //System.Diagnostics.Debug.WriteLine(passcode_prefix_id , "passcode_prefix_id");
            
            //var thePrefix = _passcodePrefixManager.GetPasscodePrefixDetailBy(user_id, passcode_prefix_id, ref sql_result);
            //var theFormat = _passcodeFormatManager.GetPasscodeFormatDetailBy(user_id, passcode_prefix_id, ref sql_result);

            //string passcode = thePrefix.passcode_format;

            var result = db.sp_GeneratePasscode(
                SessionManager.Current.obj_id,

                passcode_prefix_id,

                noToGenerate,
                active_date,
                
                CommonConstant.Status.active, //status,
                null, //void_date,
                null, //void_reason,

                ref get_sql_result, ref sql_remark);

            return (int.Parse(get_sql_result.Value.ToString()) == 1 ? true : false);
        }


        /// <summary>
        /// Delete Passcode By fail generate job  
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="generate_id"></param>
        /// <returns></returns>
        public bool DeletePasscodeByFail(int user_id, int generate_id)
        {
            int? get_sql_result = 0;
            var sql_remark = "";

            var result = db.sp_DeletePasscodeByFail(
             SessionManager.Current.obj_id,
             generate_id,

             ref get_sql_result, ref sql_remark);

            return get_sql_result == 1 ? true : false;
        }

        public sp_GetPasscodeDetailResult GetPasscodeDetail(int user_id, long? passcode_id, string pin_value, ref bool sql_result)
        {
            int? get_sql_result = 0;
            var sql_remark = "";

            var result = db.sp_GetPasscodeDetail(SessionManager.Current.obj_id, passcode_id, pin_value, ref get_sql_result, ref sql_remark);

            return result.FirstOrDefault() ?? new sp_GetPasscodeDetailResult();
        }

        public IEnumerable<sp_GetPasscodeRegistryListsResult> GetPasscodeRegistryLists(int user_id, int member_id, long rowIndexStart, int rowLimit, string searchParams)
        {
            int? get_sql_result = 0;
            var sql_remark = "";

            IEnumerable<sp_GetPasscodeRegistryListsResult> result = null;

            try
            {
                result = db.sp_GetPasscodeRegistryLists(user_id, member_id, ref get_sql_result, ref sql_remark); // rowIndexStart, rowLimit, searchParams, get_sql_result, get_sql_remark);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString(), "get passcode error");
            }

            return result;
        }

        //public bool Register(
        //    int user_id,
        //    int member_id,

        //    long passcode_id,
        //    string pin_value,
        //    int generate_id,
        //    int passcode_prefix_id,
        //    DateTime active_date,
        //    DateTime expiry_date,
        //    double point,
        //    int product_id,
        //    int status,
        //    int registered,
        //    DateTime? void_date,
        //    string void_reason,

        //    ref string sql_remark
        //    ) 
        //{ 
        //    var result = Update(
        //        user_id,

        //        passcode_id,
        //        pin_value,
        //        generate_id,
        //        passcode_prefix_id,
        //        active_date,
        //        expiry_date,
        //        point,
        //        product_id,
        //        status,
        //        registered,
        //        void_date,
        //        void_reason,
        //        ref sql_remark
        //    );

        //    if (result)
        //    {
        //        var productPurchaseManger = new ProductPurchaseManager();

        //        var productPurchaseObject = new ProductPurchaseObject()
        //        {
        //            member_id = member_id,
        //            product_id = product_id,
        //            quantity = 1,
        //            status = CommonConstant.Status.active
        //        };

        //        productPurchaseManger.CreateByPasscode(productPurchaseObject);
        //    }
        //    return result;
        //}

        //public bool Update(
        //    int user_id,

        //    long passcode_id,
        //    string pin_value,
        //    int generate_id,
        //    int passcode_prefix_id,
        //    DateTime active_date,
        //    DateTime expiry_date,
        //    double point,
        //    int product_id,
        //    int status,
        //    int registered,
        //    DateTime? void_date,
        //    string void_reason,
         
        //    ref string sql_remark
        //)
        //{
        //    int? get_sql_result = 0;
            
        //    var result = db.sp_UpdatePasscode(
        //        SessionManager.Current.user_id,

        //        passcode_id,
        //        pin_value,
        //        generate_id,
        //        passcode_prefix_id,
        //        active_date,
        //        expiry_date,
        //        point,
        //        product_id,
        //        status,
        //        registered,
        //        void_date,
        //        void_reason,

        //        ref get_sql_result, ref sql_remark);

        //    return get_sql_result.Value == 1 ? true : false;
        //}

        public CommonConstant.SystemCode Update(
           PasscodeObject theObj
       )
        {
            var systemCode = CommonConstant.SystemCode.undefine;

            int? sql_result = 0;
           
            var result = db.sp_UpdatePasscode(
               _accessObject.id,
                _accessObject.type, 

                theObj.passcode_id,
                theObj.pin_value,
                theObj.generate_id,
                theObj.passcode_prefix_id,
                theObj.active_date,
                theObj.expiry_date,
                theObj.point,
                theObj.product_id,
                theObj.status,
                theObj.registered,
                theObj.void_date,
                theObj.void_reason,

                ref sql_result);

            systemCode = (CommonConstant.SystemCode)sql_result.Value;

            return systemCode;
        }

        public CommonConstant.SystemCode Register(
            int member_id, string pin_value
       )
        {
            var systemCode = CommonConstant.SystemCode.undefine;

            var thePasscode = GetDetail(pin_value, true, ref systemCode);

            // TODO: should reutrn the system code with condition used passcode
            if (thePasscode.registered == 0 && thePasscode.expiry_date > DateTime.Now)
            {
                thePasscode.registered = 1;
                Update(thePasscode);

                var source_id = unchecked((int)thePasscode.passcode_id);
                var remark = "";
                int? new_transaction_id = 0;
                var location_id = 0;
                var point_status = (int)CommonConstant.PointStauts.realized;
                var transactionManager = new TransactionManager();
                var addFlag = transactionManager.AddPoint(location_id, member_id, thePasscode.point, point_status, thePasscode.expiry_date, (int)CommonConstant.TransactionType.passcode_registration, source_id, remark, ref new_transaction_id);

                if (addFlag)
                    systemCode = CommonConstant.SystemCode.normal;
                else
                    systemCode = CommonConstant.SystemCode.record_invalid;
            }
            else
                systemCode = CommonConstant.SystemCode.err_passcodeInvalid;

            return systemCode;
        }
    }
}