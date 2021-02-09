using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Palmary.Loyalty.Web_backend;
using System.Web.Routing;
using Palmary.Loyalty.Common;
using Palmary.Loyalty.BO.DataTransferObjects.Security;
using Palmary.Loyalty.BO.DataTransferObjects.Product;
using Palmary.Loyalty.BO.Modules.Product;

using Palmary.Loyalty.Web_backend.Modules.GlobalHandlers;
using Palmary.Loyalty.Web_backend.Modules.Utility;
using Palmary.Loyalty.Web_backend.Modules.ExtJS;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.PayoadKey;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.ExtJsEntity;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.TableData;

namespace Palmary.Loyalty.Web_backend.Controllers.Product
{
    public class ProductPurchaseController : Controller
    {
        private int _id;

        public ProductPurchaseController()
        {
        }

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            var id = RouteData.Values["id"] ?? "";
            if (!string.IsNullOrEmpty(id.ToString()))
            {
                _id = int.Parse(id.ToString());
            }
            SystemHandler.InitSystemParameters(this.HttpContext.ApplicationInstance as MvcApplication);
        }

        public string Update(FormCollection collection)
        {
            var member_id = collection.GetFormValue(PayloadKeys.Member_id);
            var purchaseList = new List<ProductPurchaseObject>();

            var product_id = collection.GetFormValue(PayloadKeys.ProductPurchase.product_id);
            var quantity = collection.GetFormValue(PayloadKeys.ProductPurchase.quantity);

            if (product_id > 0 && quantity >0)
            { 
                var productPurchaseObject = new ProductPurchaseObject()
                {
                    member_id = member_id,
                    product_id = product_id,
                    quantity = quantity,
                    status = CommonConstant.Status.active
                };

                purchaseList.Add(productPurchaseObject);
            }

            var product_id_2 = collection.GetFormValue(PayloadKeys.ProductPurchase.product_id_2);
            var quantity_2 = collection.GetFormValue(PayloadKeys.ProductPurchase.quantity_2);

            if (product_id_2 > 0 && quantity_2 > 0)
            {
                var productPurchaseObject = new ProductPurchaseObject()
                {
                    member_id = member_id,
                    product_id = product_id_2,
                    quantity = quantity_2,
                    status = CommonConstant.Status.active
                };

                purchaseList.Add(productPurchaseObject);
            }

            var product_id_3 = collection.GetFormValue(PayloadKeys.ProductPurchase.product_id_3);
            var quantity_3 = collection.GetFormValue(PayloadKeys.ProductPurchase.quantity_3);

            if (product_id_3 > 0 && quantity_3 > 0)
            {
                var productPurchaseObject = new ProductPurchaseObject()
                {
                    member_id = member_id,
                    product_id = product_id_3,
                    quantity = quantity_3,
                    status = CommonConstant.Status.active
                };

                purchaseList.Add(productPurchaseObject);
            }

            var product_id_4 = collection.GetFormValue(PayloadKeys.ProductPurchase.product_id_4);
            var quantity_4 = collection.GetFormValue(PayloadKeys.ProductPurchase.quantity_4);

            if (product_id_4 > 0 && quantity_4 > 0)
            {
                var productPurchaseObject = new ProductPurchaseObject()
                {
                    member_id = member_id,
                    product_id = product_id_4,
                    quantity = quantity_4,
                    status = CommonConstant.Status.active
                };

                purchaseList.Add(productPurchaseObject);
            }

            var product_id_5 = collection.GetFormValue(PayloadKeys.ProductPurchase.product_id_5);
            var quantity_5 = collection.GetFormValue(PayloadKeys.ProductPurchase.quantity_5);

            if (product_id_5 > 0 && quantity_5 > 0)
            {
                var productPurchaseObject = new ProductPurchaseObject()
                {
                    member_id = member_id,
                    product_id = product_id_5,
                    quantity = quantity_5,
                    status = CommonConstant.Status.active
                };

                purchaseList.Add(productPurchaseObject);
            }

            if (purchaseList.Count > 0)
            {
                var manager = new ProductPurchaseManager();

                var systemCode = manager.Purchase(
                        purchaseList
                    );

                var result = "";
                if (systemCode == CommonConstant.SystemCode.normal)
                    result = new { success = true, msg = "Created Purchase Record", url = "com.palmary.memberProfile.js.edit" }.ToJson();
                else
                    result = new { success = false, msg = "Fail to Create Purchase Record"  }.ToJson();

                return result;
            }
            else
                return "{success:true,url:'',msg:'Saved Failed: Invalid Data'}";
        }

        public string GenerateForm()
        {
            //basic table data
            var extTable = new ExtJsTable
            {
                column = 2,
                post_url = "",
                post_header = "",
                title = "Purchase Detail",
                icon = "iconRemarkList",
                post_params = Url.Action("Update"),
                isType = true,
                button_text = "Save",
                button_icon = "iconSave",
                value_changes = true,
            };

            var rowFieldSelect = new ExtJsFieldLabelSelect<int>(PayloadKeys.ProductPurchase.product_id, "")
            {
                fieldLabel = "Product",
                datasource = "../Table/GetListItems/product",
                colspan = 1
               
            };
            extTable.AddFieldLabelToRow(rowFieldSelect);

            var rowFieldInput = new ExtJsFieldLabelInput<int>(PayloadKeys.ProductPurchase.quantity, "")
            {
                fieldLabel = "Quantity",
                colspan = 1
            };
            extTable.AddFieldLabelToRow(rowFieldInput);

            rowFieldSelect = new ExtJsFieldLabelSelect<int>(PayloadKeys.ProductPurchase.product_id_2, "")
            {
                fieldLabel = "Product 2",
                datasource = "../Table/GetListItems/product",
                colspan = 1,
                allowBlank = true

            };
            extTable.AddFieldLabelToRow(rowFieldSelect);

            rowFieldInput = new ExtJsFieldLabelInput<int>(PayloadKeys.ProductPurchase.quantity_2, "")
            {
                fieldLabel = "Quantity 2",
                colspan = 1,
                allowBlank = true
            };
            extTable.AddFieldLabelToRow(rowFieldInput);

            rowFieldSelect = new ExtJsFieldLabelSelect<int>(PayloadKeys.ProductPurchase.product_id_3, "")
            {
                fieldLabel = "Product 3",
                datasource = "../Table/GetListItems/product",
                colspan = 1,
                allowBlank = true

            };
            extTable.AddFieldLabelToRow(rowFieldSelect);

            rowFieldInput = new ExtJsFieldLabelInput<int>(PayloadKeys.ProductPurchase.quantity_3, "")
            {
                fieldLabel = "Quantity 3",
                colspan = 1,
                allowBlank = true
            };
            extTable.AddFieldLabelToRow(rowFieldInput);

            rowFieldSelect = new ExtJsFieldLabelSelect<int>(PayloadKeys.ProductPurchase.product_id_4, "")
            {
                fieldLabel = "Product 4",
                datasource = "../Table/GetListItems/product",
                colspan = 1,
                allowBlank = true

            };
            extTable.AddFieldLabelToRow(rowFieldSelect);

            rowFieldInput = new ExtJsFieldLabelInput<int>(PayloadKeys.ProductPurchase.quantity_4, "")
            {
                fieldLabel = "Quantity 4",
                colspan = 1,
                allowBlank = true
            };
            extTable.AddFieldLabelToRow(rowFieldInput);

            rowFieldSelect = new ExtJsFieldLabelSelect<int>(PayloadKeys.ProductPurchase.product_id_5, "")
            {
                fieldLabel = "Product 5",
                datasource = "../Table/GetListItems/product",
                colspan = 1,
                allowBlank = true

            };
            extTable.AddFieldLabelToRow(rowFieldSelect);

            rowFieldInput = new ExtJsFieldLabelInput<int>(PayloadKeys.ProductPurchase.quantity_5, "")
            {
                fieldLabel = "Quantity 5",
                colspan = 1,
                allowBlank = true
            };
            extTable.AddFieldLabelToRow(rowFieldInput);

            // Hidden Fields
            var hiddenLabel = new ExtJsFieldLabelHidden<int>(PayloadKeys.ProductPurchase.member_id, _id.ToString());
            extTable.AddFieldLabelToHiddenRow(hiddenLabel);

            return extTable.ToJson();
        }
    }
}
