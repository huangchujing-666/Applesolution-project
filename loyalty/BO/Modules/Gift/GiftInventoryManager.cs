using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Palmary.Loyalty.BO.Database;
using Palmary.Loyalty.BO.DataTransferObjects.Security;
using System.Data.Objects;

using Palmary.Loyalty.Common;
using Palmary.Loyalty.BO.DataTransferObjects.DB;
using Palmary.Loyalty.BO.DataTransferObjects.Role;
using Palmary.Loyalty.BO.DataTransferObjects.Gift;

using Palmary.Loyalty.BO.Modules.Administration.Security;

namespace Palmary.Loyalty.BO.Modules.Gift
{
    public class GiftInventoryManager
    {
        private static LoyaltyDBDataContext db = new LoyaltyDBDataContext();
        private AccessManager _accessManager;
        private string _module = CommonConstant.Module.giftInventory;
        private RolePrivilegeObject _privilege;
        private static LogManager _logManager;
        private AccessObject _accessObject;

        // For backend, using BO Session to access
        public GiftInventoryManager()
        {
            // access object
            _accessManager = new AccessManager();
            _accessObject = _accessManager.GetAccessObjectBySession();

            // privilege and log
            _privilege = _accessManager.AccessModule(_module);
            _logManager = new LogManager();
        }

        // For Webservices or other not using BO Session to access
        public GiftInventoryManager(AccessObject accessObject)
        {
            // access object
            _accessManager = new AccessManager();
            _accessObject = accessObject;

            // privilege and log
            _privilege = _accessManager.AccessModule(_module, _accessObject.type, _accessObject.id);
            _logManager = new LogManager(_accessObject);
        }

        private CommonConstant.SystemCode Create(

            GiftInventoryObject dataObject
        )
        {
            int? sql_result = 0;
            var system_code = CommonConstant.SystemCode.undefine;

            if (_privilege.insert_status == 1)
            {
                var result = db.sp_CreateGiftInventory(
                    _accessObject.id, 
                    _accessObject.type, 

                    dataObject.source_id,
                    dataObject.location_id,
                    dataObject.gift_id,
                    dataObject.stock_change_type,
                    dataObject.stock_change,
                    dataObject.remark,
                    dataObject.status,
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

        // Get List with paging, dynamic search, dynamic sorting
        public List<GiftInventoryObject> GetList(int gift_id, int rowIndexStart, int rowLimit, List<SearchParmObject> searchParmList, string sortColumn, CommonConstant.SortOrder sortOrder, ref CommonConstant.SystemCode systemCode, ref int totalRow)
        {
            List<GiftInventoryObject> resultList;

            if (_privilege.read_status == 1)
            {
                var query = (from gi in db.gift_inventories
                             join so in db.system_objects on gi.gift_id equals so.object_id
                             join g in db.gifts on gi.gift_id equals g.gift_id

                             join li_ct in db.listing_items on gi.stock_change_type equals li_ct.value
                             join l_ct in db.listings on li_ct.list_id equals l_ct.list_id

                             where (
                                gi.record_status != (int)CommonConstant.RecordStatus.deleted
                                && l_ct.code == "StockChangeType"
                                && gi.gift_id == gift_id
                            )
                             select new GiftInventoryObject
                             {
                                 inventory_id = gi.inventory_id,
                                 source_id = gi.source_id,
                                 location_id = gi.location_id,
                                 gift_id = gi.gift_id,
                                 stock_change_type = gi.stock_change_type,
                                 stock_change = gi.stock_change,
                                 remark = gi.remark,
                                 status = gi.status,
                                 crt_date = gi.crt_date,
                                 crt_by_type = gi.crt_by_type,
                                 crt_by = gi.crt_by,
                                 upd_date = gi.upd_date,
                                 upd_by_type = gi.upd_by_type,
                                 upd_by = gi.upd_by,
                                 record_status = gi.record_status,

                                 //-- additional info
                                 gift_no = g.gift_no,
                                 gift_name = so.name,
                                 stock_change_type_name = li_ct.name
                             });

                // dynamic search
                //foreach (var f in searchParmList)
                //{
                //}

                // dynamic sort
                Func<GiftInventoryObject, Object> orderByFunc = null;
                if (sortColumn == "gift_no")
                    orderByFunc = x => x.gift_no;
                else if (sortColumn == "gift_name")
                    orderByFunc = x => x.gift_name;
                else if (sortColumn == "stock_change_type_name")
                    orderByFunc = x => x.stock_change_type_name;
                else if (sortColumn == "stock_change")
                    orderByFunc = x => x.stock_change;
                else if (sortColumn == "remark")
                    orderByFunc = x => x.remark;
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
                resultList = new List<GiftInventoryObject>();
                systemCode = CommonConstant.SystemCode.no_permission;
            }

            return resultList;
        }

        // Get whole list (limited data) for calculate stock summary
        public List<GiftInventoryObject> GetList(int gift_id, ref CommonConstant.SystemCode systemCode)
        {
            List<GiftInventoryObject> resultList;

            if (_privilege.read_status == 1)
            {
                var query = (from gi in db.gift_inventories
                             where (
                                gi.record_status != (int)CommonConstant.RecordStatus.deleted
                               
                                && gi.gift_id == gift_id
                            )
                             select new GiftInventoryObject
                             {
                                 inventory_id = gi.inventory_id,
                                 source_id = gi.source_id,
                                 location_id = gi.location_id,
                                 gift_id = gi.gift_id,
                                 stock_change_type = gi.stock_change_type,
                                 stock_change = gi.stock_change,
                                 remark = gi.remark,
                                 status = gi.status,
                                 crt_date = gi.crt_date,
                                 crt_by_type = gi.crt_by_type,
                                 crt_by = gi.crt_by,
                                 upd_date = gi.upd_date,
                                 upd_by_type = gi.upd_by_type,
                                 upd_by = gi.upd_by,
                                 record_status = gi.record_status,

                             });


                resultList = query.OrderByDescending(x => x.crt_date).ToList();

                systemCode = CommonConstant.SystemCode.normal;
            }
            else
            {
                resultList = new List<GiftInventoryObject>();
                systemCode = CommonConstant.SystemCode.no_permission;
            }

            return resultList;
        

        }

        public CommonConstant.SystemCode GetGiftStockSummery(int gift_id, ref int current_stock, ref int redeem_count)
        {
            var systemCode = CommonConstant.SystemCode.undefine;

            var list = GetList(gift_id, ref systemCode);
            current_stock = list.Sum(x => x.stock_change);
            redeem_count = list.Where(x=> x.stock_change_type == (int)CommonConstant.StockChangeType.redemption).Sum(x => x.stock_change);

            redeem_count = 0 - redeem_count; //change to positive value

            return systemCode;
        }

        public CommonConstant.SystemCode StockChange(GiftInventoryObject giftInventoryObject)
        {
            int current_stock = 0;
            int redeem_count = 0;

            var systemCode = CommonConstant.SystemCode.undefine;

            GetGiftStockSummery(giftInventoryObject.gift_id, ref current_stock, ref redeem_count);

            if (giftInventoryObject.stock_change <0) // minus stock
            {
                if (current_stock < (0 - giftInventoryObject.stock_change)) // not enough for minus
                    systemCode = CommonConstant.SystemCode.err_not_enough_stcok;
                else
                    systemCode = Create(giftInventoryObject);
            }
            else // add stock
                systemCode = Create(giftInventoryObject);

            if (systemCode == CommonConstant.SystemCode.normal)
            { 
                // update gift stock cache
                var giftManager = new GiftManager();
                var g = giftManager.GetDetail(giftInventoryObject.gift_id);

                // get latest current stock
                GetGiftStockSummery(giftInventoryObject.gift_id, ref current_stock, ref redeem_count);

                g.available_stock = current_stock;

                var sql_remark = "";
                var updateFlag = giftManager.Update_directCore(g, ref sql_remark);

                if (updateFlag)
                    systemCode = CommonConstant.SystemCode.normal;
                else
                    systemCode = CommonConstant.SystemCode.record_invalid;
            }

            return systemCode;
        }

       
    }
}