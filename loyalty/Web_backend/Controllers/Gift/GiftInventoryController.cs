using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Web.Routing;
using Palmary.Loyalty.Web_backend;

using Palmary.Loyalty.Common;
using Palmary.Loyalty.BO.Database;
using Palmary.Loyalty.BO.DataTransferObjects.Security;
using Palmary.Loyalty.BO.DataTransferObjects.Gift;
using Palmary.Loyalty.BO.Modules.Gift;
using Palmary.Loyalty.BO.Modules.Administration.Security;
using Palmary.Loyalty.Web_backend.Modules.GlobalHandlers;
using Palmary.Loyalty.Web_backend.Modules.Utility;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.ExtJsEntity;
using Palmary.Loyalty.Web_backend.Modules.ExtJS;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.PayoadKey;

namespace Palmary.Loyalty.Web_backend.Controllers.Gift
{
    [Authorize]
    public class GiftInventoryController : Controller
    {
        private int _gift_id;

        public GiftInventoryController()
        {
          
        }

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            var id = RouteData.Values["id"] ?? "";
            if (!string.IsNullOrEmpty(id.ToString()))
            {
                _gift_id = int.Parse(id.ToString());
            }
            SystemHandler.InitSystemParameters(this.HttpContext.ApplicationInstance as MvcApplication);
        }

       
        // Create New Form
        public string Insert()
        {
            var gift_inventory = new GiftInventoryObject()
            {
                gift_id = _gift_id
            };
            
            var formTableJSON = TableFormHandler.GetFormByModule(gift_inventory);
            return formTableJSON;
        }

        public string Update(FormCollection collection)
        {
            var gift_inventory_id = collection.GetFormValue(PayloadKeys.Gift_inventory.gift_inventory_id);
            var gift_id = collection.GetFormValue(PayloadKeys.Gift_inventory.gift_id);
            var stock_change = collection.GetFormValue(PayloadKeys.Gift_inventory.stock_change);
            var type_id = collection.GetFormValue(PayloadKeys.Gift_inventory.type_id);
            var remark = collection.GetFormValue(PayloadKeys.Gift_inventory.remark);
           
            var sql_remark = "";

            var _gift_inventoryManager = new GiftInventoryManager();


            var giftInventoryObject = new GiftInventoryObject()
            {
                inventory_id = gift_inventory_id,
                source_id = SessionManager.Current.obj_id,
                location_id = 0,
                gift_id = gift_id,
                stock_change_type = (int)CommonConstant.StockChangeType.stock_adjustment,
                stock_change = stock_change,
                remark = remark,
                status = (int) CommonConstant.Status.active
            };


            if (gift_inventory_id == 0 && giftInventoryObject.stock_change != 0)
            {
                var systemCode = _gift_inventoryManager.StockChange(giftInventoryObject);
                var addFlag = false;

                if (systemCode == CommonConstant.SystemCode.normal)
                    addFlag = true;

                if (systemCode == CommonConstant.SystemCode.err_not_enough_stcok)
                    sql_remark = "Not Enough Stock";

                return addFlag ? "{success:true,url:'',msg:'Stock Change Complete'}" : "{success:false,url:'',msg:'Stcok Change Fail: " + sql_remark + "'}";
            }
            else
            {
                return "{success:false, url:'', msg:'Stcok Change Fail: Invalid Data'}";
            }
            
        }

        public string CoreForm()
        {
            var systemCode = CommonConstant.SystemCode.undefine;
            int current_stock = 0;
            int redeem_count = 0;

            var giftInventoryManager = new GiftInventoryManager();
            var giftManager = new GiftManager();
            
            giftInventoryManager.GetGiftStockSummery(_gift_id, ref current_stock, ref redeem_count);
            var gift = giftManager.GetDetail(_gift_id);

            //basic table data
            var extTable = new ExtJsTable
            {
                column = 2,
                post_url = "",
                post_header = "",
                title = "Detail",
                icon = "iconRemarkList",
                //post_params = Url.Action("Perform"),
                isType = true,
                //button_text = "Save",     //no neet button
                //button_icon = "iconSave",
                value_changes = true,
            };

            var rowFieldInput_str = new ExtJsFieldLabelInput<string>(PayloadKeys.Dummy, gift.gift_no)
            {
                fieldLabel = "Gift No",
                type = "input",
                colspan = 2,
                tabIndex = 1,
                allowBlank = false,
                readOnly = true
            };
            extTable.AddFieldLabelToRow(rowFieldInput_str);

            rowFieldInput_str = new ExtJsFieldLabelInput<string>(PayloadKeys.Dummy, gift.name)
            {
                fieldLabel = "Gift Name",
                type = "input",
                colspan = 2,
                tabIndex = 1,
                allowBlank = false,
                readOnly = true
            };
            extTable.AddFieldLabelToRow(rowFieldInput_str);

            rowFieldInput_str = new ExtJsFieldLabelInput<string>(PayloadKeys.Dummy, current_stock.ToString())
            {
                fieldLabel = "Current Stock",
                type = "input",
                colspan = 2,
                tabIndex = 1,
                allowBlank = false,
                readOnly = true
            };
            extTable.AddFieldLabelToRow(rowFieldInput_str);

            rowFieldInput_str = new ExtJsFieldLabelInput<string>(PayloadKeys.Dummy, gift.alert_level.ToString())
            {
                fieldLabel = "Alert Level",
                type = "input",
                colspan = 2,
                tabIndex = 1,
                allowBlank = false,
                readOnly = true
            };
            extTable.AddFieldLabelToRow(rowFieldInput_str);

            rowFieldInput_str = new ExtJsFieldLabelInput<string>(PayloadKeys.Dummy, redeem_count.ToString())
            {
                fieldLabel = "Redeem Count",
                type = "input",
                colspan = 2,
                tabIndex = 1,
                allowBlank = false,
                readOnly = true
            };
            extTable.AddFieldLabelToRow(rowFieldInput_str);

            return extTable.ToJson();
        }
    }
}
