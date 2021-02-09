using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.Objects;

using Palmary.Loyalty.Common;
using Palmary.Loyalty.BO.Database;
using Palmary.Loyalty.BO.DataTransferObjects.Security;
using Palmary.Loyalty.BO.DataTransferObjects.DB;
using Palmary.Loyalty.BO.DataTransferObjects.Transaction;
using Palmary.Loyalty.BO.DataTransferObjects.Role;

using Palmary.Loyalty.BO.Modules.Administration.Security;
using Palmary.Loyalty.BO.Modules.Member;
using Palmary.Loyalty.BO.Modules.Utility;
using Palmary.Loyalty.BO.Modules.Administration.SystemControl;

namespace Palmary.Loyalty.BO.Modules.Transaction
{
    public class TransactionManager
    {
        private static LoyaltyDBDataContext db = new LoyaltyDBDataContext();
        private string _module = CommonConstant.Module.transaction;
        private AccessManager _accessManager;
        private RolePrivilegeObject _privilege;
        private AccessObject _accessObject;
        private static LogManager _logManager;

        // For backend, using BO Session to access
        public TransactionManager()
        {
            // access object
            _accessManager = new AccessManager();
            _accessObject = _accessManager.GetAccessObjectBySession();

            // privilege and log
            _privilege = _accessManager.AccessModule(_module);
            _logManager = new LogManager();
        }

        // For Webservices or other not using BO Session to access
        public TransactionManager(AccessObject accessObject)
        {
            // access object
            _accessManager = new AccessManager();
            _accessObject = accessObject;

            // privilege and log
            _privilege = _accessManager.AccessModule(_module, _accessObject.type, _accessObject.id);
            _logManager = new LogManager(_accessObject);
        }

        public CommonConstant.SystemCode Create(

            TransactionObject dataObject,

            ref int? new_transaction_id
        )
        {
            int? sql_result = 0;
            var system_code = CommonConstant.SystemCode.undefine;

            if (_privilege.insert_status == 1)
            {
                var result = db.sp_CreateTransaction(
                    _accessObject.id,
                       _accessObject.type,
                    (int)dataObject.type,
                    dataObject.source_id,
                    dataObject.location_id,
                    dataObject.member_id,
                    dataObject.point_change,
                    dataObject.point_status,
                    dataObject.point_expiry_date,
                    dataObject.display,
                    dataObject.void_date,
                    dataObject.remark,
                    dataObject.status,
                  
                    ref new_transaction_id,
                    ref sql_result
                );

                system_code = (CommonConstant.SystemCode)sql_result.Value;
            }
            else
            {
                system_code = CommonConstant.SystemCode.no_permission;
            }

            return system_code;
        }

        public CommonConstant.SystemCode Update(TransactionObject dataObject)
        {
            int? sql_result = 0;
            var system_code = CommonConstant.SystemCode.undefine;

            if (_privilege.update_status == 1)
            {
                var result = db.sp_UpdateTransaction(
                    _accessObject.id,
    _accessObject.type,

                    dataObject.transaction_id,
                    dataObject.type,
                    dataObject.source_id,
                    dataObject.location_id,
                    dataObject.member_id,
                    dataObject.point_change,
                    dataObject.point_status,
                    dataObject.point_expiry_date,
                    dataObject.display,
                    dataObject.void_date,
                    dataObject.remark,
                    dataObject.status,
                    ref sql_result);

                system_code = (CommonConstant.SystemCode)sql_result.Value;
            }
            else
            {
                system_code = CommonConstant.SystemCode.no_permission;
            }

            return system_code;
        }

        // Get List with paging, dynamic search, dynamic sorting
        public List<TransactionObject> GetList(int rowIndexStart, int rowLimit, List<SearchParmObject> searchParmList, string sortColumn, CommonConstant.SortOrder sortOrder, ref CommonConstant.SystemCode systemCode, ref int totalRow)
        {
            List<TransactionObject> resultList;

            if (_privilege.read_status == 1)
            {
                var query = (from t in db.transactions
                             join m in db.member_profiles on t.member_id equals m.member_id
                             join li_t in db.listing_items on t.type equals li_t.value
                             join l_t in db.listings on li_t.list_id equals l_t.list_id

                             where (
                                t.record_status != (int)CommonConstant.RecordStatus.deleted
                                && l_t.code == "TransactionType"
                            )
                             select new TransactionObject
                             {
                                 transaction_id = t.transaction_id,
                                 type = t.type,
                                 source_id = t.source_id,
                                 location_id = t.location_id,
                                 member_id = t.member_id,
                                 point_change = t.point_change,
                                 point_status = t.point_status,
                                 display = t.display,
                                 void_date = t.void_date,
                                 remark = t.remark,
                                 status = t.status,
                                 crt_date = t.crt_date,
                                 crt_by_type = t.crt_by_type,
                                 crt_by = t.crt_by,
                                 upd_date = t.upd_date,
                                 upd_by_type = t.upd_by_type,
                                 upd_by = t.upd_by,
                                 record_status = t.record_status,

                                 // additional info 
                                 type_name = li_t.name,
                                 //member_name = MemberManager.GetFullName(m.firstname, m.middlename, m.lastname),
                                 member_no = m.member_no
                             });

                // dynamic search
                foreach (var f in searchParmList)
                {
                    if (f.property == "transaction_id")
                    {
                        if (!String.IsNullOrEmpty(f.value))
                            query = query.Where(x => x.transaction_id == int.Parse(f.value));
                    }
                    else if (f.property == "member_no")
                    {
                        query = query.Where(x => x.member_no.Contains(f.value));
                    }
                    else if (f.property == "type_name")
                    {
                        if (!String.IsNullOrEmpty(f.value))
                            query = query.Where(x => x.type == int.Parse(f.value));
                    }
                    else if (f.property == "point_larger")
                    {
                        if (!String.IsNullOrEmpty(f.value))
                            query = query.Where(x => x.point_change >= int.Parse(f.value));
                    }
                    else if (f.property == "point_lower")
                    {
                        if (!String.IsNullOrEmpty(f.value))
                            query = query.Where(x => x.point_change <= int.Parse(f.value));
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
                Func<TransactionObject, Object> orderByFunc = null;
                if (sortColumn == "transaction_id")
                    orderByFunc = x => x.transaction_id;
                else if (sortColumn == "type_name")
                    orderByFunc = x => x.type_name;
                else if (sortColumn == "member_no")
                    orderByFunc = x => x.member_no;
                else if (sortColumn == "member_name")
                    orderByFunc = x => x.member_name;
                else if (sortColumn == "point_change")
                    orderByFunc = x => x.point_change;
                else if (sortColumn == "crt_date")
                    orderByFunc = x => x.crt_date;
                else
                {
                    sortOrder = CommonConstant.SortOrder.desc;
                    orderByFunc = x => x.crt_date;
                }

                // row total
                totalRow = query.Count();

                if (sortOrder == CommonConstant.SortOrder.desc)
                    resultList = query.OrderByDescending(orderByFunc).Skip(rowIndexStart).Take(rowLimit).ToList();
                else
                    resultList = query.OrderBy(orderByFunc).Skip(rowIndexStart).Take(rowLimit).ToList();

                systemCode = CommonConstant.SystemCode.normal;
            }
            else
            {
                resultList = new List<TransactionObject>();
                systemCode = CommonConstant.SystemCode.no_permission;
            }

            return resultList;
        }

        public List<TransactionObject> GetListAll(List<SearchParmObject> searchParmList, ref CommonConstant.SystemCode systemCode)
        {
            List<TransactionObject> resultList;

            if (_privilege.read_status == 1)
            {
                var query = (from t in db.transactions
                             join m in db.member_profiles on t.member_id equals m.member_id
                             join li_t in db.listing_items on t.type equals li_t.value
                             join l_t in db.listings on li_t.list_id equals l_t.list_id

                             where (
                                t.record_status != (int)CommonConstant.RecordStatus.deleted
                                && l_t.code == "TransactionType"
                            )
                             select new TransactionObject
                             {
                                 transaction_id = t.transaction_id,
                                 type = t.type,
                                 source_id = t.source_id,
                                 location_id = t.location_id,
                                 member_id = t.member_id,
                                 point_change = t.point_change,
                                 point_status = t.point_status,
                                 display = t.display,
                                 void_date = t.void_date,
                                 remark = t.remark,
                                 status = t.status,
                                 crt_date = t.crt_date,
                                 crt_by_type = t.crt_by_type,
                                 crt_by = t.crt_by,
                                 upd_date = t.upd_date,
                                 upd_by_type = t.upd_by_type,
                                 upd_by = t.upd_by,
                                 record_status = t.record_status,

                                 // additional info 
                                 type_name = li_t.name,
                                // member_name = MemberManager.GetFullName(m.firstname, m.middlename, m.lastname),
                                 member_no = m.member_no
                             });

                // dynamic search
                foreach (var f in searchParmList)
                {
                    if (f.property == "transaction_id")
                    {
                        if (!String.IsNullOrEmpty(f.value))
                            query = query.Where(x => x.transaction_id == int.Parse(f.value));
                    }
                    else if (f.property == "member_no")
                    {
                        query = query.Where(x => x.member_no.Contains(f.value));
                    }
                    else if (f.property == "type_name")
                    {
                        if (!String.IsNullOrEmpty(f.value))
                            query = query.Where(x => x.type == int.Parse(f.value));
                    }
                    else if (f.property == "point_larger")
                    {
                        if (!String.IsNullOrEmpty(f.value))
                            query = query.Where(x => x.point_change >= int.Parse(f.value));
                    }
                    else if (f.property == "point_lower")
                    {
                        if (!String.IsNullOrEmpty(f.value))
                            query = query.Where(x => x.point_change <= int.Parse(f.value));
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

                resultList = query.OrderByDescending(x => x.crt_date).ToList();

                systemCode = CommonConstant.SystemCode.normal;
            }
            else
            {
                resultList = new List<TransactionObject>();
                systemCode = CommonConstant.SystemCode.no_permission;
            }

            return resultList;
        }

        // Get List by Member with paging, dynamic search, dynamic sorting
        public List<TransactionObject> GetList(int member_id, int rowIndexStart, int rowLimit, List<SearchParmObject> searchParmList, string sortColumn, CommonConstant.SortOrder sortOrder, ref CommonConstant.SystemCode systemCode, ref int totalRow)
        {
            systemCode = CommonConstant.SystemCode.undefine;
            List<TransactionObject> resultList;

            if (_privilege.read_status == 1)
            {
                var query = (from t in db.transactions
                             join m in db.member_profiles on t.member_id equals m.member_id
                             join li_t in db.listing_items on t.type equals li_t.value
                             join l_t in db.listings on li_t.list_id equals l_t.list_id

                             join li_ps in db.listing_items on t.point_status equals li_ps.value
                             join l_ps in db.listings on li_ps.list_id equals l_ps.list_id

                             join li_s in db.listing_items on t.status equals li_s.value
                             join l_s in db.listings on li_s.list_id equals l_s.list_id

                             where (
                                t.record_status != (int)CommonConstant.RecordStatus.deleted
                                && l_t.code == "TransactionType"
                                && l_ps.code == "PointStatus"
                                && l_s.code == "TransactionStatus"
                                && t.member_id == member_id
                            )
                             select new TransactionObject
                             {
                                 transaction_id = t.transaction_id,
                                 type = t.type,
                                 source_id = t.source_id,
                                 location_id = t.location_id,
                                 member_id = t.member_id,
                                 point_change = t.point_change,
                                 point_status = t.point_status,
                                 point_expiry_date = t.point_expiry_date,
                                 display = t.display,
                                 void_date = t.void_date,
                                 remark = t.remark,
                                 status = t.status,
                                 crt_date = t.crt_date,
                                 crt_by_type = t.crt_by_type,
                                 crt_by = t.crt_by,
                                 upd_date = t.upd_date,
                                 upd_by_type = t.upd_by_type,
                                 upd_by = t.upd_by,
                                 record_status = t.record_status,

                                 // additional info 
                                 type_name = li_t.name,
                                // member_name = MemberManager.GetFullName(m.firstname, m.middlename, m.lastname),
                                 point_status_name = li_ps.name,
                                 status_name = li_s.name
                             });

                // dynamic search
                //foreach (var f in searchParmList)
                //{
                //}

                // dynamic sort
                var orderByColumn = "";
                if (sortColumn == "transaction_id" 
                    || sortColumn == "type_name"
                    || sortColumn == "member_name"
                    || sortColumn == "point_change"
                    || sortColumn == "crt_date"
                    || sortColumn == "point_status_name"
                    || sortColumn == "point_expiry_date")
                    orderByColumn = sortColumn;
                else
                { //default
                    sortOrder = CommonConstant.SortOrder.desc;
                    orderByColumn = "crt_date";
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
                resultList = new List<TransactionObject>();
                systemCode = CommonConstant.SystemCode.no_permission;
            }

            return resultList;
        }

        // Get List by Member with paging, dynamic search, dynamic sorting
        public List<TransactionObject> GetListWithName(int member_id, int rowIndexStart, int rowLimit, List<SearchParmObject> searchParmList, string sortColumn, CommonConstant.SortOrder sortOrder, ref CommonConstant.SystemCode systemCode, ref int totalRow)
        {
            List<TransactionObject> resultList;

            if (_privilege.read_status == 1)
            {
                var query = (from t in db.transactions
                             join m in db.member_profiles on t.member_id equals m.member_id
                             join li_t in db.listing_items on t.type equals li_t.value
                             join l_t in db.listings on li_t.list_id equals l_t.list_id

                             join li_ps in db.listing_items on t.point_status equals li_ps.value
                             join l_ps in db.listings on li_ps.list_id equals l_ps.list_id

                             join li_s in db.listing_items on t.status equals li_s.value
                             join l_s in db.listings on li_s.list_id equals l_s.list_id

                             join gr_t in db.gift_redemptions on t.transaction_id equals gr_t.transaction_id into gr_t_table
                             from gr_t in gr_t_table.DefaultIfEmpty() // left outer join

                             join gl_t in db.gift_langs on gr_t.gift_id equals gl_t.gift_id into gl_t_table
                             from gl_t in gl_t_table.DefaultIfEmpty() // left outer join
                            
                             where (
                                t.record_status != (int)CommonConstant.RecordStatus.deleted
                                && l_t.code == "TransactionType"
                                && l_ps.code == "PointStatus"
                                && l_s.code == "TransactionStatus"
                                && t.member_id == member_id

                                && ((t.type == (int)CommonConstant.TransactionType.redemption && gl_t.lang_id == (int)CommonConstant.LangCode.en) || gl_t.lang_id == null)
                               
                            )
                             select new TransactionObject
                             {
                                 transaction_id = t.transaction_id,
                                 type = t.type,
                                 source_id = t.source_id,
                                 location_id = t.location_id,
                                 member_id = t.member_id,
                                 point_change = t.point_change,
                                 point_status = t.point_status,
                                 point_expiry_date = t.point_expiry_date,
                                 display = t.display,
                                 void_date = t.void_date,
                                 remark = t.remark,
                                 status = t.status,
                                 crt_date = t.crt_date,
                                 crt_by_type = t.crt_by_type,
                                 crt_by = t.crt_by,
                                 upd_date = t.upd_date,
                                 upd_by_type = t.upd_by_type,
                                 upd_by = t.upd_by,
                                 record_status = t.record_status,

                                 // additional info 
                                 type_name = li_t.name,
                                 //member_name = MemberManager.GetFullName(m.firstname, m.middlename, m.lastname),
                                 point_status_name = li_ps.name,
                                 status_name = li_s.name,
                                 gift_name = gl_t.name
                             });

                // dynamic search
                //foreach (var f in searchParmList)
                //{
                //}

                // dynamic sort
                Func<TransactionObject, Object> orderByFunc = null;
                if (sortColumn == "transaction_id")
                    orderByFunc = x => x.transaction_id;
                else if (sortColumn == "type_name")
                    orderByFunc = x => x.type_name;
                else if (sortColumn == "member_name")
                    orderByFunc = x => x.member_name;
                else if (sortColumn == "point_change")
                    orderByFunc = x => x.point_change;
                else if (sortColumn == "crt_date")
                    orderByFunc = x => x.crt_date;
                else if (sortColumn == "point_status_name")
                    orderByFunc = x => x.point_status_name;
                else if (sortColumn == "point_expiry_date")
                    orderByFunc = x => x.point_expiry_date;
                else
                {
                    sortOrder = CommonConstant.SortOrder.desc;
                    orderByFunc = x => x.crt_date;
                }

                // row total
                totalRow = query.Count();

                if (sortOrder == CommonConstant.SortOrder.desc)
                    resultList = query.OrderByDescending(orderByFunc).Skip(rowIndexStart).Take(rowLimit).ToList();
                else
                    resultList = query.OrderBy(orderByFunc).Skip(rowIndexStart).Take(rowLimit).ToList();

                systemCode = CommonConstant.SystemCode.normal;
            }
            else
            {
                resultList = new List<TransactionObject>();
                systemCode = CommonConstant.SystemCode.no_permission;
            }
            return resultList;
        }

        // Get whole list (limited data) status=active(point not used up), not expired 
        //public List<TransactionObject> GetList_member(int member_id, ref CommonConstant.SystemCode systemCode)
        //{
        //    List<TransactionObject> resultList;

        //    if (_privilege.read_status == 1)
        //    {
        //        var query = (from t in db.transactions
                             
        //                     where (
        //                        t.record_status != (int)CommonConstant.RecordStatus.deleted
        //                        && t.member_id == member_id
        //                    )
        //                     select new TransactionObject
        //                     {
        //                         transaction_id = t.transaction_id,
        //                         type = t.type,
        //                         source_id = t.source_id,
        //                         location_id = t.location_id,
        //                         member_id = t.member_id,
        //                         point_change = t.point_change,
        //                         point_status = t.point_status,
        //                         display = t.display,
        //                         void_date = t.void_date,
        //                         remark = t.remark,
        //                         status = t.status,
        //                         crt_date = t.crt_date,
        //                         crt_by_type = t.crt_by_type,
        //                         crt_by = t.crt_by,
        //                         upd_date = t.upd_date,
        //                         upd_by_type = t.upd_by_type,
        //                         upd_by = t.upd_by,
        //                         record_status = t.record_status
        //                     });

        //        resultList = query.OrderByDescending(x => x.crt_date).ToList();

        //        systemCode = CommonConstant.SystemCode.normal;
        //    }
        //    else
        //    {
        //        resultList = new List<TransactionObject>();
        //        systemCode = CommonConstant.SystemCode.no_permission;
        //    }

        //    return resultList;
        //}

        // Get Detail
        public TransactionObject GetDetail(int transaction_id, bool rootAccess, ref CommonConstant.SystemCode systemCode)
        {
            TransactionObject resultObj;

            if (_privilege.read_status == 1 || rootAccess)
            {
                var query = (from t in db.transactions

                             join crt_ao in db.v_accessObjects on new { type = t.crt_by_type, target_id = t.crt_by } equals new { type = crt_ao.type, target_id = crt_ao.target_id } 

                             join m in db.member_profiles on t.member_id equals m.member_id
                             join li_t in db.listing_items on t.type equals li_t.value
                             join l_t in db.listings on li_t.list_id equals l_t.list_id

                             join li_ps in db.listing_items on t.point_status equals li_ps.value
                             join l_ps in db.listings on li_ps.list_id equals l_ps.list_id

                             where (
                                t.record_status != (int)CommonConstant.RecordStatus.deleted
                                && l_t.code == "TransactionType"
                                && l_ps.code == "PointStatus"
                                && t.transaction_id == transaction_id
                            )
                             select new TransactionObject
                             {
                                 transaction_id = t.transaction_id,
                                 type = t.type,
                                 source_id = t.source_id,
                                 member_id = t.member_id,
                                 point_change = t.point_change,
                                 point_status = t.point_status,
                                 point_expiry_date = t.point_expiry_date,
                                 display = t.display,
                                 void_date = t.void_date,
                                 remark = t.remark,
                                 status = t.status,
                                 crt_date = t.crt_date,
                                 crt_by_type = t.crt_by_type,
                                 crt_by = t.crt_by,
                                 upd_date = t.upd_date,
                                 upd_by_type = t.upd_by_type,
                                 upd_by = t.upd_by,
                                 record_status = t.record_status,

                                 // additional info 
                                 type_name = li_t.name,
                                 member_no = m.member_no,
                                 crt_by_name = crt_ao.name,
                                 point_status_name = li_ps.name
                             });

                resultObj = query.FirstOrDefault();
                systemCode = CommonConstant.SystemCode.normal;
            }
            else
            {
                resultObj = new TransactionObject();
                systemCode = CommonConstant.SystemCode.no_permission;
            }

            return resultObj;
        }

        public decimal GetAvailablePoint(int member_id)
        {
            var transactionEarnManger = new TransactionEarnManager(_accessObject);
            var systemCode = CommonConstant.SystemCode.undefine;
            var availbleList = transactionEarnManger.GetList_memberAvailablePoint(member_id, ref systemCode);

            var point_earn = availbleList.Sum(d => d.point_earn);
            var point_used = availbleList.Sum(d => d.point_used);

            decimal availablePoint = (decimal)point_earn - (decimal)point_used;  // Floating-Point Arithmetic problem, need to change double to decimal
            return availablePoint;
        }

        public void GetPointSummary(
            int member_id, 
            ref decimal point_current_realized,
            ref decimal point_current_unrealized,
            ref decimal point_earned, 
            ref decimal point_used, 
            ref decimal point_expired,
            ref decimal point_expiring_2month
            )
        {
            var systemCode = CommonConstant.SystemCode.undefine;
            var earnManager = new TransactionEarnManager();
            var earnlist = earnManager.GetList_member(member_id, ref systemCode);

            var point_earned_non_expired = earnlist.Where(e => e.point_expiry_date > DateTime.Now).Sum(d => d.point_earn);
            var point_used_non_expired = earnlist.Where(e => e.point_expiry_date > DateTime.Now).Sum(d => d.point_used);

            var point_earned_expired = earnlist.Where(e => e.point_expiry_date < DateTime.Now).Sum(d => d.point_earn);
            var point_used_expired = earnlist.Where(e => e.point_expiry_date < DateTime.Now).Sum(d => d.point_used);

            point_current_realized = (decimal)earnlist.Where(e => e.point_expiry_date > DateTime.Now && e.point_status == (int)CommonConstant.PointStauts.realized).Sum(d => d.point_earn - d.point_used);
            point_current_unrealized = (decimal)earnlist.Where(e => e.point_expiry_date > DateTime.Now && e.point_status == (int)CommonConstant.PointStauts.unrealized).Sum(d => d.point_earn - d.point_used);
            point_expiring_2month = (decimal)earnlist.Where(e => e.point_expiry_date > DateTime.Now && e.point_expiry_date <= DateTime.Now.AddMonths(2)).Sum(d => d.point_earn - d.point_used);

            // Floating-Point Arithmetic problem, need to change double to decimal
            point_earned = (decimal)point_earned_non_expired + (decimal)point_earned_expired;
            point_used = (decimal)point_used_non_expired + (decimal)point_used_expired;
            point_expired = (decimal)point_earned_expired - (decimal)point_used_expired;
        }

        public bool AddPoint(int location_id, int member_id, double point, int point_status, 
            DateTime? point_expiry_date, int transactionType, int source_id, string remark, ref int? new_transaction_id)
        {
            var result = false;
            var dbResult = CommonConstant.SystemCode.undefine;

            if (point > 0)
            {
                // create transaction (add point record)
                var transactionObject = new TransactionObject()
                {
                    type = transactionType,
                    source_id = source_id,
                    location_id = location_id,
                    member_id = member_id,
                    point_change = point,
                    point_status = point_status,
                    point_expiry_date = point_expiry_date,
                    display = true,
                    void_date = null,
                    remark = remark,
                    status = CommonConstant.Status.active
                };

                dbResult = Create(transactionObject, ref new_transaction_id);

                // create earn detail
                if (transactionType == (int)CommonConstant.TransactionType.passcode_registration || 
                    transactionType == (int)CommonConstant.TransactionType.point_adjustment || 
                    transactionType == (int)CommonConstant.TransactionType.point_transfer ||
                    transactionType == (int)CommonConstant.TransactionType.promotion_rule ||
                    transactionType == (int)CommonConstant.TransactionType.location_presence ||
                    transactionType == (int)CommonConstant.TransactionType.member_referral)
                {
                    if (dbResult == CommonConstant.SystemCode.normal)
                    {
                        
                        var transactionEarnObject = new TransactionEarnObject()
                        {
                            transaction_id = new_transaction_id.Value,
                            source_id = _accessObject.id,

                            point_earn = point,
                            point_status = point_status,
                            point_expiry_date = point_expiry_date == null ? CommonConstant.Default.point_expiry_date : point_expiry_date.Value,
                            point_used = 0,

                            status = CommonConstant.TransactionStatus.active,
                        };

                        var earnManager = new TransactionEarnManager(_accessObject);
                        dbResult = earnManager.Create(transactionEarnObject);

                     
                    }
                }
                // upgrade member level 
                // update cache of available point 
                UpgradeLevelAndAvailablePoint(member_id);
            }

            if (dbResult == CommonConstant.SystemCode.normal)
                result = true;

            return result;
        }

        public bool UsePoint(int location_id, int member_id, double point_required, int transactionType, int source_id, string remark, ref int? new_transaction_id)
        {
            PointDeductActor actor = new PointDeductActor(_accessObject);
            var result = actor.DeductPoint(location_id, member_id, point_required, transactionType, source_id, remark, ref new_transaction_id);

            if (result)
            {
                // update cache of member available point 
                UpdateAvailablePoint(member_id);
            }

            return result;
        }

        public CommonConstant.SystemCode UpdateAvailablePoint(int member_id)
        {
            var memberManager = new MemberManager(_accessObject);
            var systemCode = CommonConstant.SystemCode.undefine;
            var member = memberManager.GetDetail(member_id, true, ref systemCode);

            // update member point cache 
            member.available_point = (double)GetAvailablePoint(member_id);

            systemCode = memberManager.Update_directCore(member);

            return systemCode;
        }

        public bool UpgradeLevelAndAvailablePoint(int member_id)
        {
            var memberManager = new MemberManager(_accessObject);
            var systemCode = CommonConstant.SystemCode.undefine;
            var member = memberManager.GetDetail(member_id, true, ref systemCode);

            // update member point cache 
            member.available_point = (double)GetAvailablePoint(member_id);

            // upgrade member level if valid
            // check purchase record in 1 year
            var transactionEarnManger = new TransactionEarnManager(_accessObject);
            var fromDate = DateTime.Now.AddYears(-1);
            var availbleList = transactionEarnManger.GetList_memberAvailablePoint_fromDate(member_id, fromDate, ref systemCode);
            var pointEarn= availbleList.Sum(d => d.point_earn);

            var memberLevelManager = new MemberLevelManager(_accessObject);
            var targetMemberLevelID = memberLevelManager.GetMemberLevelIDFromPoint(pointEarn);

            if (targetMemberLevelID > member.member_level_id)
                member.member_level_id = targetMemberLevelID;

            // update member record
            systemCode = memberManager.Update_directCore(member);


            if (systemCode == CommonConstant.SystemCode.normal)
                return true;
            else
                return false;
        }

        public CommonConstant.SystemCode MemberReferralParentGetPoint(int parent_member_id, int child_member_id)
        {
            var location_id = 0;

            var sql_remark = "";
            var systemConfigManager = new SystemConfigManager(_accessObject);
            var point_expiry_month = int.Parse(systemConfigManager.GetSystemConfigValue("point_expiry_month", ref sql_remark));
            var point_expiry_date = DateTime.Now.AddMonths(point_expiry_month);
            
            var remark = "";
            int? new_transaction_id = 0;

            var point_earn = 300;
            var result = AddPoint(location_id, parent_member_id, point_earn, (int)CommonConstant.PointStauts.realized,
            point_expiry_date, (int)CommonConstant.TransactionType.member_referral, child_member_id, remark, ref new_transaction_id);

            var systemCode = CommonConstant.SystemCode.undefine;
            if (result)
            {
                systemCode = CommonConstant.SystemCode.normal;
            }
            else
            {
                systemCode = CommonConstant.SystemCode.record_invalid;
            }

            return systemCode;
        }
    }
}