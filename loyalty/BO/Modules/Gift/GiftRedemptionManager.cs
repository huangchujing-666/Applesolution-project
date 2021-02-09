using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.Objects;
using Palmary.Loyalty.Common;
using Palmary.Loyalty.BO.Database;
using Palmary.Loyalty.BO.DataTransferObjects.Gift;
using Palmary.Loyalty.BO.DataTransferObjects.Security;
using Palmary.Loyalty.BO.DataTransferObjects.Role;
using Palmary.Loyalty.BO.DataTransferObjects.DB;
using Palmary.Loyalty.BO.Modules.Transaction;
using Palmary.Loyalty.BO.Modules.Administration.Security;
using Palmary.Loyalty.BO.Modules.Utility;
using Palmary.Loyalty.BO.Modules.Administration.SystemControl;
using Palmary.Loyalty.BO.Modules.Member;

namespace Palmary.Loyalty.BO.Modules.Gift
{
    public class GiftRedemptionManager
    {
        private static LoyaltyDBDataContext db = new LoyaltyDBDataContext();
        private AccessManager _accessManager;
        private string _module = CommonConstant.Module.giftRedemption;
        private RolePrivilegeObject _privilege;
        private static LogManager _logManager;
        private AccessObject _accessObject;

        // For backend, using BO Session to access
        public GiftRedemptionManager()
        {
            // access object
            _accessManager = new AccessManager();
            _accessObject = _accessManager.GetAccessObjectBySession();

            // privilege and log
            _privilege = _accessManager.AccessModule(_module);
            _logManager = new LogManager();
        }

        // For Webservices or other not using BO Session to access
        public GiftRedemptionManager(AccessObject accessObject)
        {
            // access object
            _accessManager = new AccessManager();
            _accessObject = accessObject;

            // privilege and log
            _privilege = _accessManager.AccessModule(_module, _accessObject.type, _accessObject.id);
            _logManager = new LogManager(_accessObject);
        }

        private CommonConstant.SystemCode Create(
           GiftRedemptionObject dataObject,
            ref int? new_redemption_id
        )
        {
            int? sql_result = 0;
            var system_code = CommonConstant.SystemCode.undefine;
    
            if (_privilege.insert_status == 1)
            {
                var result = db.sp_CreateGiftRedemption(
                                   _accessObject.id,
                _accessObject.type, 
                    dataObject.transaction_id,
                    dataObject.redemption_code,
                    dataObject.redemption_channel,
                    dataObject.member_id,
                    dataObject.gift_id,
                    dataObject.quantity,
                    dataObject.point_used,
                    dataObject.redemption_status,
                    dataObject.collect_date,
                    dataObject.collect_location_id,
                    dataObject.void_date,
                    dataObject.void_user_id,
                    dataObject.remark,
                    dataObject.status,

                    ref new_redemption_id,
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
        public List<GiftRedemptionObject> GetList(int rowIndexStart, int rowLimit, List<SearchParmObject> searchParmList, string sortColumn, CommonConstant.SortOrder sortOrder, ref CommonConstant.SystemCode systemCode, ref int totalRow)
        {
            List<GiftRedemptionObject> resultList;

            if (_privilege.read_status == 1)
            {
                var query = (from gr in db.gift_redemptions
                             join g_l in db.gift_langs on gr.gift_id equals g_l.gift_id
                             join m in db.member_profiles on gr.member_id equals m.member_id
                             join ll in db.location_langs on gr.collect_location_id equals ll.location_id
                             join g in db.gifts on gr.gift_id equals g.gift_id

                             join li_rs in db.listing_items on gr.redemption_status equals li_rs.value
                             join l_rs in db.listings on li_rs.list_id equals l_rs.list_id

                             where (
                                gr.record_status != (int)CommonConstant.RecordStatus.deleted
                                && ll.lang_id == (int)CommonConstant.LangCode.en
                                && l_rs.code == "GiftRedeemStatus"
                                && g_l.lang_id == _accessObject.languageID
                            )
                             select new GiftRedemptionObject
                             {
                                 redemption_id = gr.redemption_id,
                                 transaction_id = gr.transaction_id,
                                 redemption_code = gr.redemption_code,
                                 redemption_channel = gr.redemption_channel,
                                 member_id = gr.member_id,
                                 gift_id = gr.gift_id,
                                 quantity = gr.quantity,
                                 point_used = gr.point_used,
                                 redemption_status = gr.redemption_status,
                                 collect_date = gr.collect_date,
                                 collect_location_id = gr.collect_location_id,
                                 void_date = gr.void_date,
                                 void_user_id = gr.void_user_id,
                                 remark = gr.remark,
                                 status = gr.status,
                                 crt_date = gr.crt_date,
                                 crt_by_type = gr.crt_by_type,
                                 crt_by = gr.crt_by,
                                 upd_date = gr.upd_date,
                                 upd_by_type = gr.upd_by_type,
                                 upd_by = gr.upd_by,
                                 record_status = gr.record_status,

                                 //-- additional info
                                 gift_no = g.gift_no,
                                 gift_name = g_l.name,
                                 location_name = ll.name,
                                 member_no = m.member_no,
                                 //member_name = MemberManager.GetFullName(m.firstname, m.middlename, m.lastname),
                                 redemption_status_name = li_rs.name
                             });

                // dynamic search
                foreach (var f in searchParmList)
                {
                    if (f.property == "redemption_code")
                    {
                        query = query.Where(x => x.redemption_code.ToLower().Contains(f.value.ToLower()));
                    }
                    else if (f.property == "member_no")
                    {
                        query = query.Where(x => x.member_no.ToLower().Contains(f.value.ToLower()));
                    }
                    else if (f.property == "gift_no")
                    {
                        query = query.Where(x => x.gift_no.ToLower().Contains(f.value.ToLower()));
                    }
                    else if (f.property == "redemption_status_name")
                    {
                        if (!String.IsNullOrEmpty(f.value))
                            query = query.Where(x => x.redemption_status == int.Parse(f.value));
                    }
                    else if (f.property == "quantity_larger")
                    {
                        if (!String.IsNullOrEmpty(f.value))
                            query = query.Where(x => x.quantity >= int.Parse(f.value));
                    }
                    else if (f.property == "quantity_lower")
                    {
                        if (!String.IsNullOrEmpty(f.value))
                            query = query.Where(x => x.quantity <= int.Parse(f.value));
                    }
                    else if (f.property == "point_used_larger")
                    {
                        if (!String.IsNullOrEmpty(f.value))
                            query = query.Where(x => x.point_used >= int.Parse(f.value));
                    }
                    else if (f.property == "point_used_lower")
                    {
                        if (!String.IsNullOrEmpty(f.value))
                            query = query.Where(x => x.point_used <= int.Parse(f.value));
                    }
                    else if (f.property == "location")
                    {
                        if (!String.IsNullOrEmpty(f.value))
                            query = query.Where(x => x.collect_location_id == int.Parse(f.value));
                    }
                    else if (f.property == "collect_date_larger")
                    {
                        if (!String.IsNullOrEmpty(f.value))
                            query = query.Where(x => x.collect_date >= Convert.ToDateTime(f.value));
                    }
                    else if (f.property == "collect_date_lower")
                    {
                        if (!String.IsNullOrEmpty(f.value))
                            query = query.Where(x => x.collect_date <= Convert.ToDateTime(f.value + " 23:59:59.999"));
                    }
                    else if (f.property == "crt_date_larger")
                    {
                        if (!String.IsNullOrEmpty(f.value))
                            query = query.Where(x => x.crt_date >= Convert.ToDateTime(f.value));
                    }
                    else if (f.property == "crt_date_lower")
                    {
                        if (!String.IsNullOrEmpty(f.value))
                            query = query.Where(x => x.crt_date <= Convert.ToDateTime(f.value + " 23:59:59.999"));
                    }
                }

                // dynamic sort
                Func<GiftRedemptionObject, Object> orderByFunc = null;
                if (sortColumn == "member_no")
                    orderByFunc = x => x.member_no;
                else if (sortColumn == "member_name")
                    orderByFunc = x => x.member_name;
                else if (sortColumn == "gift_no")
                    orderByFunc = x => x.gift_no;
                else if (sortColumn == "gift_name")
                    orderByFunc = x => x.gift_name;
                else if (sortColumn == "quantity")
                    orderByFunc = x => x.quantity;
                else if (sortColumn == "point_used")
                    orderByFunc = x => x.point_used;
                else if (sortColumn == "location_name")
                    orderByFunc = x => x.location_name;
                else if (sortColumn == "redemption_status_name")
                    orderByFunc = x => x.redemption_status_name;
                else if (sortColumn == "collect_date")
                    orderByFunc = x => x.collect_date;
                else if (sortColumn == "crt_date")
                    orderByFunc = x => x.crt_date;
                else if (sortColumn == "redemption_code")
                    orderByFunc = x => x.redemption_code;
                else if (sortColumn == "location")
                    orderByFunc = x => x.location_name;
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
                resultList = new List<GiftRedemptionObject>();
                systemCode = CommonConstant.SystemCode.no_permission;
            }

            return resultList;
        }

        public List<GiftRedemptionObject> GetList_whole(List<SearchParmObject> searchParmList, ref CommonConstant.SystemCode systemCode)
        {
            List<GiftRedemptionObject> resultList;

            if (_privilege.read_status == 1)
            {
                var query = (from gr in db.gift_redemptions
                             join so in db.system_objects on gr.gift_id equals so.object_id
                             join m in db.member_profiles on gr.member_id equals m.member_id
                             join so_m in db.system_objects on m.member_id equals so_m.object_id
                             join ll in db.location_langs on gr.collect_location_id equals ll.location_id
                             join g in db.gifts on gr.gift_id equals g.gift_id

                             join li_rs in db.listing_items on gr.redemption_status equals li_rs.value
                             join l_rs in db.listings on li_rs.list_id equals l_rs.list_id

                             where (
                                gr.record_status != (int)CommonConstant.RecordStatus.deleted
                                && ll.lang_id == (int)CommonConstant.LangCode.en
                                && l_rs.code == "GiftRedeemStatus"
                            )
                             select new GiftRedemptionObject
                             {
                                 redemption_id = gr.redemption_id,
                                 transaction_id = gr.transaction_id,
                                 redemption_code = gr.redemption_code,
                                 redemption_channel = gr.redemption_channel,
                                 member_id = gr.member_id,
                                 gift_id = gr.gift_id,
                                 quantity = gr.quantity,
                                 point_used = gr.point_used,
                                 redemption_status = gr.redemption_status,
                                 collect_date = gr.collect_date,
                                 collect_location_id = gr.collect_location_id,
                                 void_date = gr.void_date,
                                 void_user_id = gr.void_user_id,
                                 remark = gr.remark,
                                 status = gr.status,
                                 crt_date = gr.crt_date,
                                 crt_by_type = gr.crt_by_type,
                                 crt_by = gr.crt_by,
                                 upd_date = gr.upd_date,
                                 upd_by_type = gr.upd_by_type,
                                 upd_by = gr.upd_by,
                                 record_status = gr.record_status,

                                 //-- additional info
                                 gift_no = g.gift_no,
                                 gift_name = so.name,
                                 location_name = ll.name,
                                 member_no = m.member_no,
                                 member_name = so_m.name,
                                 redemption_status_name = li_rs.name
                             });

                // dynamic search
                foreach (var f in searchParmList)
                {
                    if (f.property == "redemption_code")
                    {
                        query = query.Where(x => x.redemption_code.ToLower().Contains(f.value.ToLower()));
                    }
                    else if (f.property == "member_no")
                    {
                        query = query.Where(x => x.member_no.ToLower().Contains(f.value.ToLower()));
                    }
                    else if (f.property == "gift_no")
                    {
                        query = query.Where(x => x.gift_no.ToLower().Contains(f.value.ToLower()));
                    }
                    else if (f.property == "redemption_status_name")
                    {
                        if (!String.IsNullOrEmpty(f.value))
                            query = query.Where(x => x.redemption_status == int.Parse(f.value));
                    }
                    else if (f.property == "quantity_larger")
                    {
                        if (!String.IsNullOrEmpty(f.value))
                            query = query.Where(x => x.quantity >= int.Parse(f.value));
                    }
                    else if (f.property == "quantity_lower")
                    {
                        if (!String.IsNullOrEmpty(f.value))
                            query = query.Where(x => x.quantity <= int.Parse(f.value));
                    }
                    else if (f.property == "point_used_larger")
                    {
                        if (!String.IsNullOrEmpty(f.value))
                            query = query.Where(x => x.point_used >= int.Parse(f.value));
                    }
                    else if (f.property == "point_used_lower")
                    {
                        if (!String.IsNullOrEmpty(f.value))
                            query = query.Where(x => x.point_used <= int.Parse(f.value));
                    }
                    else if (f.property == "location")
                    {
                        if (!String.IsNullOrEmpty(f.value))
                            query = query.Where(x => x.collect_location_id == int.Parse(f.value));
                    }
                    else if (f.property == "collect_date_larger")
                    {
                        if (!String.IsNullOrEmpty(f.value))
                            query = query.Where(x => x.collect_date >= Convert.ToDateTime(f.value));
                    }
                    else if (f.property == "collect_date_lower")
                    {
                        if (!String.IsNullOrEmpty(f.value))
                            query = query.Where(x => x.collect_date <= Convert.ToDateTime(f.value + " 23:59:59.999"));
                    }
                    else if (f.property == "crt_date_larger")
                    {
                        if (!String.IsNullOrEmpty(f.value))
                            query = query.Where(x => x.crt_date >= Convert.ToDateTime(f.value));
                    }
                    else if (f.property == "crt_date_lower")
                    {
                        if (!String.IsNullOrEmpty(f.value))
                            query = query.Where(x => x.crt_date <= Convert.ToDateTime(f.value + " 23:59:59.999"));
                    }
                }

                resultList = query.OrderByDescending(x => x.crt_date).ToList();

                systemCode = CommonConstant.SystemCode.normal;
            }
            else
            {
                resultList = new List<GiftRedemptionObject>();
                systemCode = CommonConstant.SystemCode.no_permission;
            }

            return resultList;
        }

        public List<GiftRedemptionObject> GetListByMember(int member_id, int rowIndexStart, int rowLimit, List<SearchParmObject> searchParmList, string sortColumn, CommonConstant.SortOrder sortOrder, ref CommonConstant.SystemCode systemCode, ref int totalRow)
        {
            List<GiftRedemptionObject> resultList;
           
            if (_privilege.read_status == 1)
            {
                var query = (from gr in db.gift_redemptions
                             join g_l in db.gift_langs on gr.gift_id equals g_l.gift_id
                             join m in db.member_profiles on gr.member_id equals m.member_id
                             
                             join ll in db.location_langs on gr.collect_location_id equals ll.location_id
                             join g in db.gifts on gr.gift_id equals g.gift_id

                             join li_rs in db.listing_items on gr.redemption_status equals li_rs.value
                             join l_rs in db.listings on li_rs.list_id equals l_rs.list_id

                             where (
                                gr.record_status != (int)CommonConstant.RecordStatus.deleted
                                && ll.lang_id == (int)CommonConstant.LangCode.en
                                && l_rs.code == "GiftRedeemStatus"
                                && gr.member_id == member_id
                                && g_l.lang_id == _accessObject.languageID
                            )
                             select new GiftRedemptionObject
                             {
                                 redemption_id = gr.redemption_id,
                                 transaction_id = gr.transaction_id,
                                 redemption_code = gr.redemption_code,
                                 redemption_channel = gr.redemption_channel,
                                 member_id = gr.member_id,
                                 gift_id = gr.gift_id,
                                 quantity = gr.quantity,
                                 point_used = gr.point_used,
                                 redemption_status = gr.redemption_status,
                                 collect_date = gr.collect_date,
                                 collect_location_id = gr.collect_location_id,
                                 void_date = gr.void_date,
                                 void_user_id = gr.void_user_id,
                                 remark = gr.remark,
                                 status = gr.status,
                                 crt_date = gr.crt_date,
                                 crt_by_type = gr.crt_by_type,
                                 crt_by = gr.crt_by,
                                 upd_date = gr.upd_date,
                                 upd_by_type = gr.upd_by_type,
                                 upd_by = gr.upd_by,
                                 record_status = gr.record_status,

                                 //-- additional info
                                 gift_no = g.gift_no,
                                 gift_name = g_l.name,
                                 location_name = ll.name,
                                 member_no = m.member_no,
                                // member_name = MemberManager.GetFullName(m.firstname, m.middlename, m.lastname),
                                 redemption_status_name = li_rs.name
                             });

                // dynamic search
                foreach (var f in searchParmList)
                {
                    //if (f.property == "name")
                    //{
                    //    query = query.Where(x => x.name.ToLower().Contains(f.value.ToLower()));
                    //}
                    //else if (f.property == "product_no")
                    //{
                    //    query = query.Where(x => x.product_no.Contains(f.value));
                    //}
                }

                // dynamic sort
               
                Func<GiftRedemptionObject, Object> orderByFunc = null;
                if (sortColumn == "member_no")
                    orderByFunc = x => x.member_no;
                else if (sortColumn == "member_name")
                    orderByFunc = x => x.member_name;
                else if (sortColumn == "gift_no")
                    orderByFunc = x => x.gift_no;
                else if (sortColumn == "gift_name")
                    orderByFunc = x => x.gift_name;
                else if (sortColumn == "quantity")
                    orderByFunc = x => x.quantity;
                else if (sortColumn == "point_used")
                    orderByFunc = x => x.point_used;
                else if (sortColumn == "location_name")
                    orderByFunc = x => x.location_name;
                else if (sortColumn == "redemption_status_name")
                    orderByFunc = x => x.redemption_status_name;
                else if (sortColumn == "collect_date")
                    orderByFunc = x => x.collect_date;
                else if (sortColumn == "crt_date")
                    orderByFunc = x => x.crt_date;
                else if (sortColumn == "redemption_code")
                    orderByFunc = x => x.redemption_code;
                else if (sortColumn == "location")
                    orderByFunc = x => x.location_name;
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
                resultList = new List<GiftRedemptionObject>();
                systemCode = CommonConstant.SystemCode.no_permission;
            }

            return resultList;
        }

        public List<GiftRedemptionObject> GetListByTransaction(int transaction_id, int rowIndexStart, int rowLimit, List<SearchParmObject> searchParmList, string sortColumn, CommonConstant.SortOrder sortOrder, ref CommonConstant.SystemCode systemCode, ref int totalRow)
        {
            List<GiftRedemptionObject> resultList;

            if (_privilege.read_status == 1)
            {
                var query = (from gr in db.gift_redemptions
                             join g_l in db.gift_langs on gr.gift_id equals g_l.gift_id
                             join m in db.member_profiles on gr.member_id equals m.member_id
                            
                             join ll in db.location_langs on gr.collect_location_id equals ll.location_id
                             join g in db.gifts on gr.gift_id equals g.gift_id

                             join li_rs in db.listing_items on gr.redemption_status equals li_rs.value
                             join l_rs in db.listings on li_rs.list_id equals l_rs.list_id

                             where (
                                gr.record_status != (int)CommonConstant.RecordStatus.deleted
                                && ll.lang_id == (int)CommonConstant.LangCode.en
                                && l_rs.code == "GiftRedeemStatus"
                                && gr.transaction_id == transaction_id
                                && g_l.lang_id == _accessObject.languageID
                            )
                             select new GiftRedemptionObject
                             {
                                 redemption_id = gr.redemption_id,
                                 transaction_id = gr.transaction_id,
                                 redemption_code = gr.redemption_code,
                                 redemption_channel = gr.redemption_channel,
                                 member_id = gr.member_id,
                                 gift_id = gr.gift_id,
                                 quantity = gr.quantity,
                                 point_used = gr.point_used,
                                 redemption_status = gr.redemption_status,
                                 collect_date = gr.collect_date,
                                 collect_location_id = gr.collect_location_id,
                                 void_date = gr.void_date,
                                 void_user_id = gr.void_user_id,
                                 remark = gr.remark,
                                 status = gr.status,
                                 crt_date = gr.crt_date,
                                 crt_by_type = gr.crt_by_type,
                                 crt_by = gr.crt_by,
                                 upd_date = gr.upd_date,
                                 upd_by_type = gr.upd_by_type,
                                 upd_by = gr.upd_by,
                                 record_status = gr.record_status,

                                 //-- additional info
                                 gift_no = g.gift_no,
                                 gift_name = g_l.name,
                                 location_name = ll.name,
                                 member_no = m.member_no,
                                 redemption_status_name = li_rs.name
                             });

                // dynamic search
                foreach (var f in searchParmList)
                {
                    //if (f.property == "name")
                    //{
                    //    query = query.Where(x => x.name.ToLower().Contains(f.value.ToLower()));
                    //}
                    //else if (f.property == "product_no")
                    //{
                    //    query = query.Where(x => x.product_no.Contains(f.value));
                    //}
                }

                // dynamic sort
                Func<GiftRedemptionObject, Object> orderByFunc = null;
                //if (sortColumn == "product_no")
                //    orderByFunc = x => x.product_no;
                //else if (sortColumn == "name")
                //    orderByFunc = x => x.name;
                //else if (sortColumn == "status")
                //    orderByFunc = x => x.status;
                //else
                //{
                //    sortOrder = CommonConstant.SortOrder.asc;
                //    orderByFunc = x => x.product_no;
                //}

                // row total
                totalRow = query.Count();

                //if (sortOrder == CommonConstant.SortOrder.desc)
                //    resultList = query.OrderByDescending(orderByFunc).Skip(rowIndexStart).Take(rowLimit).ToList();
                //else
                //    resultList = query.OrderBy(orderByFunc).Skip(rowIndexStart).Take(rowLimit).ToList();

                resultList = query.OrderByDescending(x => x.crt_date).Skip(rowIndexStart).Take(rowLimit).ToList();

                systemCode = CommonConstant.SystemCode.normal;
            }
            else
            {
                resultList = new List<GiftRedemptionObject>();
                systemCode = CommonConstant.SystemCode.no_permission;
            }

            return resultList;
        }

        // get raw data
        public GiftRedemptionObject GetDetail(int redemption_id, ref CommonConstant.SystemCode systemCode)
        {
            GiftRedemptionObject resultObject;

            if (_privilege.read_status == 1)
            {
                var query = (from gr in db.gift_redemptions
                             join g_l in db.gift_langs on gr.gift_id equals g_l.gift_id

                             join crt_ao in db.v_accessObjects on new { type = gr.crt_by_type, target_id = gr.crt_by } equals new { type = crt_ao.type, target_id = crt_ao.target_id }
                             join upd_ao in db.v_accessObjects on new { type = gr.upd_by_type, target_id = gr.upd_by } equals new { type = upd_ao.type, target_id = upd_ao.target_id } 

                             join m in db.member_profiles on gr.member_id equals m.member_id
                             
                             join ll in db.location_langs on gr.collect_location_id equals ll.location_id
                             join g in db.gifts on gr.gift_id equals g.gift_id

                             join li_rs in db.listing_items on gr.redemption_status equals li_rs.value
                             join l_rs in db.listings on li_rs.list_id equals l_rs.list_id

                             where (
                                gr.record_status != (int)CommonConstant.RecordStatus.deleted
                                && ll.lang_id == (int)CommonConstant.LangCode.en
                                && l_rs.code == "GiftRedeemStatus"
                                && gr.redemption_id == redemption_id
                                && g_l.lang_id == _accessObject.languageID
                            )
                             select new GiftRedemptionObject
                             {
                                 redemption_id = gr.redemption_id,
                                 transaction_id = gr.transaction_id,
                                 redemption_code = gr.redemption_code,
                                 redemption_channel = gr.redemption_channel,
                                 member_id = gr.member_id,
                                 gift_id = gr.gift_id,
                                 quantity = gr.quantity,
                                 point_used = gr.point_used,
                                 redemption_status = gr.redemption_status,
                                 collect_date = gr.collect_date,
                                 collect_location_id = gr.collect_location_id,
                                 void_date = gr.void_date,
                                 void_user_id = gr.void_user_id,
                                 remark = gr.remark,
                                 status = gr.status,
                                 crt_date = gr.crt_date,
                                 crt_by_type = gr.crt_by_type,
                                 crt_by = gr.crt_by,
                                 upd_date = gr.upd_date,
                                 upd_by_type = gr.upd_by_type,
                                 upd_by = gr.upd_by,
                                 record_status = gr.record_status,

                                 //-- additional info
                                 gift_no = g.gift_no,
                                 gift_name = g_l.name,
                                 location_name = ll.name,
                                 redemption_status_name = li_rs.name,
                                 member_no = m.member_no,
                                // member_name = MemberManager.GetFullName(m.firstname, m.middlename, m.lastname),
                                 crt_by_name = crt_ao.name,
                                 upd_by_name = upd_ao.name
                             });

                resultObject = query.FirstOrDefault();

                systemCode = CommonConstant.SystemCode.normal;
            }
            else
            {
                resultObject = new GiftRedemptionObject();
                systemCode = CommonConstant.SystemCode.no_permission;
            }

            return resultObject;
        }

        public CommonConstant.SystemCode Update(GiftRedemptionObject dataObject)
        {
            int? sql_result = 0;
            var system_code = CommonConstant.SystemCode.undefine;

            if (_privilege.update_status == 1)
            {
                var result = db.sp_UpdateGiftRedemption(
                                  _accessObject.id,
                _accessObject.type, 
                    dataObject.redemption_id,
                    dataObject.transaction_id,
                    dataObject.redemption_code,
                    dataObject.redemption_channel,
                    dataObject.member_id,
                    dataObject.gift_id,
                    dataObject.quantity,
                    dataObject.point_used,
                    dataObject.redemption_status,
                    dataObject.collect_date,
                    dataObject.collect_location_id,
                    dataObject.void_date,
                    dataObject.void_user_id,
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

        public bool CheckAllowRedeem(int member_id, GiftObject gift, ref string message)
        {
            var allow_redeem = true;
          
            // Check gift allow redeem date range
            if (gift.redeem_active_date > DateTime.Now || gift.redeem_expiry_date < DateTime.Now)
            {
                allow_redeem = false;
                message = gift.gift_no + " is not active for redemption";
            }

             // Check gift record_status
            if (gift.record_status == CommonConstant.RecordStatus.deleted)
            {
                allow_redeem = false;
                message = gift.gift_no + " has been deleted";
            }

            // Check gift inventory
            var inventoryManger = new GiftInventoryManager(_accessObject);
            int current_stock = 0;
            int redeem_count = 0;
            inventoryManger.GetGiftStockSummery(gift.gift_id, ref current_stock, ref redeem_count);

            if (current_stock <= 0 )
            {
                allow_redeem = false;
                message = gift.gift_no + " is insufficient stock";
            }

            // Check member privilege
            var giftManager = new GiftManager(_accessObject);
            var systemCode = CommonConstant.SystemCode.undefine;
            var allowGiftList = giftManager.GetListByMember(member_id, ref systemCode);

            var count = allowGiftList.Where(x=> x.gift_id == gift.gift_id).Count();
            if (count!=1)
            {
                allow_redeem = false;
                message = "member does not has enough privilege";
            }

            var memberManger = new MemberManager(_accessObject);
            var m = memberManger.GetDetail(member_id, true, ref systemCode);
            if (m.status != CommonConstant.Status.active)
            {
                allow_redeem = false;
                message = "Inactive member";
            }

            return allow_redeem;
        }

        public bool Redeem(List<GiftRedemptionObject> redeemList, ref string message)
        {
            var result = false;

            if (redeemList.Count() > 0)
            {
                var transactionManger = new TransactionManager(_accessObject);
                var giftManager = new GiftManager(_accessObject);
                var member_id = redeemList[0].member_id;
                var availablePoint = (double)transactionManger.GetAvailablePoint(member_id);

                double totalPointRequire = 0;
                var allow_redeem = true;
                foreach (var r in redeemList)
                {
                    var theGift = giftManager.GetDetail(r.gift_id);

                    if (theGift.discount == true && theGift.discount_active_date < DateTime.Now && theGift.discount_expiry_date > DateTime.Now)
                    {
                        r.point_used = theGift.discount_point.Value * r.quantity;
                    }
                    else
                        r.point_used = theGift.point * r.quantity;

                    totalPointRequire += r.point_used;

                    // Check gift permission
                    if (!CheckAllowRedeem(member_id, theGift, ref message))
                        allow_redeem = false;
                }

                // Check enough point 
                if (availablePoint < totalPointRequire)
                {
                    allow_redeem = false;
                    message = "Member does not have enough point";
                }

                if (allow_redeem)
                {
                    // Create transaction - use point
                    var redeem_location_id = 0;
                    var source_id = 0;
                    var remark = "";
                    int? new_transaction_id = 0;
                    var usePoint = transactionManger.UsePoint(redeem_location_id, member_id, totalPointRequire, (int)CommonConstant.TransactionType.redemption, source_id, remark, ref new_transaction_id);

                    // create gift redemption record & change stock

                    var reedemValid = true;

                    foreach (var r in redeemList)
                    {
                        // create redemption record
                        int? new_redemption_id = 0;
                        r.transaction_id = new_transaction_id.Value;
                        r.redemption_code = "";
                        var system_code = Create(r, ref new_redemption_id);

                        if (system_code == CommonConstant.SystemCode.normal)
                        {
                            r.redemption_id = new_redemption_id.Value;

                            // generate redemption code
                            var redemption_code = GenerateRedemptionCode(r.redemption_id);

                            // update redemption code
                            r.redemption_code = redemption_code;
                            system_code = Update(r);

                            // chage stock
                            var stock_change = 0;
                            if (r.quantity > 0)
                                stock_change -= r.quantity; // change to negative value

                            var giftInventoryObject = new GiftInventoryObject()
                            {
                                source_id = new_redemption_id.Value,
                                location_id = r.collect_location_id,
                                gift_id = r.gift_id,
                                stock_change_type = (int)CommonConstant.StockChangeType.redemption,
                                stock_change = stock_change,
                                remark = r.remark,
                                status = (int)CommonConstant.Status.active
                            };
                            var inventoryManager = new GiftInventoryManager(_accessObject);
                            inventoryManager.StockChange(giftInventoryObject);

                        }
                        else
                            reedemValid = false;
                    }

                    if (usePoint && reedemValid)
                        result = true;
                }
            }

            return result;
        }

        private string GenerateRedemptionCode(int redemption_id)
        {
            var redemptionCode = "";

            var yyMM = DateTime.Now.ToString("yyMM");
            var redemption_id_str = redemption_id.ToString("D6");

            var checkDigit = CheckDigitManager.GetLuhnCheckDigit(yyMM + redemption_id_str);

            redemptionCode = yyMM + redemption_id_str + checkDigit;

            return redemptionCode;
        }

        public CommonConstant.SystemCode RedemptionCollect(int redemption_id)
        {
            var systemCode = CommonConstant.SystemCode.undefine;
            var r = GetDetail(redemption_id, ref systemCode);

            r.redemption_status = (int)CommonConstant.GiftRedeemStatus.collected;
            r.collect_date = DateTime.Now;

            systemCode = Update(r);

            return systemCode;
        }

        public CommonConstant.SystemCode RedemptionCancel(int redemption_id, string remark)
        {
            var systemCode = CommonConstant.SystemCode.undefine;
            var r = GetDetail(redemption_id, ref systemCode);

            // update redemption record
            r.redemption_status = (int)CommonConstant.GiftRedeemStatus.voided;
            r.void_date = DateTime.Now;
            r.remark = remark;

            systemCode = Update(r);

            // add back used point
            var transactionManager = new TransactionManager();
            var t = transactionManager.GetDetail(r.transaction_id, true, ref systemCode);
            var location_id = 0;
            double point_earn = r.point_used;
            var point_status = (int)CommonConstant.PointStauts.realized;
            var transactionType = (int)CommonConstant.TransactionType.redemption_cancel;
            var source_id = r.redemption_id;

            var systemConfigManager = new SystemConfigManager();
            var bo_remark = "";
            var point_expiry_month = int.Parse(systemConfigManager.GetSystemConfigValue("point_expiry_month", ref bo_remark));

            DateTime? point_expiry_date = DateTime.Now.AddMonths(point_expiry_month);
            int? new_transaction_id = 0;
            var result = transactionManager.AddPoint(location_id, r.member_id, point_earn, point_status, point_expiry_date, transactionType, source_id, remark, ref new_transaction_id);

            // add back inventory
            var giftInventoryObject = new GiftInventoryObject()
            {
                source_id = redemption_id,
                location_id = r.collect_location_id,
                gift_id = r.gift_id,
                stock_change_type = (int)CommonConstant.StockChangeType.redemption_cancel,
                stock_change = r.quantity,
                remark = r.remark,
                status = (int)CommonConstant.Status.active
            };
            var inventoryManager = new GiftInventoryManager();
            inventoryManager.StockChange(giftInventoryObject);

            return systemCode;
        }
    }
}
