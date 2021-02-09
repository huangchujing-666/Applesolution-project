using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Palmary.Loyalty.BO.DataTransferObjects.Security;
using Palmary.Loyalty.BO.DataTransferObjects.Product;
using Palmary.Loyalty.BO.Modules.Product;
using Palmary.Loyalty.Web_backend.Modules.GlobalHandlers;
using Palmary.Loyalty.Web_backend.Modules.Utility;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.TableData;
using Palmary.Loyalty.Web_backend;

using Palmary.Loyalty.Common;

namespace Palmary.Loyalty.Web_backend.Controllers.Product
{
    [Authorize]
    public class ProductOrderController : Controller
    {
        private int _id;
        
        public ProductOrderController()
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

        public string GetGridHeader()
        {
            var result = new
            {
                title = "Product",
                pageSize = 20,
                add_url = "",
                add_hidden = true,
                search_text_hidden = true,
                delete_hidden = true,
                checkbox_hidden = true,
                delete_url = "",
                columns = new[]{
                      new {header = "Display Order", dataIndex="display_order", width = 120, type = "input", renderer = "", sortable = true, column = true},
                      new {header = "Product Code", dataIndex="product_no", width = 120, type = "input", renderer = "", sortable = true, column = true},
                      new {header = "Product Name", dataIndex="name", width = 160, type = "input", renderer = "", sortable = true, column = true},
                      new {header = "Action", dataIndex="action", width = 120, type = "input", renderer = "", sortable = true, column = true},
                },
                fields = new string[] { "id", "display_order", "product_no", "name", "action"}
            };

            return result.ToJson();
        }

        public string ListData()
        {
            if (!String.IsNullOrEmpty(Request.QueryString["category_id"]))
            {
                var cat_id = int.Parse(Request.QueryString["category_id"]);  //from GET

                var manager = new ProductManager();
                var list = manager.GetProductListsByCategory(cat_id);

                var newList = list.Select(x => new
                {
                    display_order = x.display_order,
                    product_no = x.product_no,
                    name = x.name,
                    action = "<button type='button' style=\"display:\" onclick=\"new com.embraiz.tag().open_pop_up('poe:" + x.product_id + "','ProductOrder:edit','com.palmary.ProductOrder.js.edit','iconRemarkList','iconRemarkList','iconRemarkList'," + cat_id + ",'owner','com.palmary.productorder.js.index')\">Change Order</button>"
                });

                var result = new
                {
                    success = true,
                    items = newList,
                    totalCount = newList.Count()
                };

                return result.ToJson();
            }
            else
                return "";
        }

        public string EditForm(FormCollection collection)
        {
            List<ExtJSField> fieldList = new List<ExtJSField>();

            var product_id = _id;
            var product_category_id = int.Parse(collection["category_id"]);
           
            var linkManager = new ProductCategoryLinkManager();
            var linkObject = linkManager.GetDetail(product_id, product_category_id);

            fieldList.Add(new ExtJSField
            {
                name = "product_name_display",
                fieldLabel = "Product Name",
                type = "input",
                colspan = 2,
                tabIndex = "1",
                value = linkObject.product_name,
                readOnly = true
            });

            fieldList.Add(new ExtJSField
            {
                name = "category_name_display",
                fieldLabel = "Category Name",
                type = "input",
                colspan = 2,
                tabIndex = "2",
                value = linkObject.category_name,
                readOnly = true
            });

            fieldList.Add(new ExtJSField
            {
                name = "display_order",
                fieldLabel = "Display Order",
                type = "input",
                colspan = 2,
                tabIndex = "3",
                value = linkObject.display_order.ToString()
            });

            // Hidden Fields
            List<ExtJSField_hidden> hiddenList = new List<ExtJSField_hidden>();
            hiddenList.Add(new ExtJSField_hidden
            {
                type = "hidden",
                name = "product_id",
                value = product_id.ToString()
            });

            hiddenList.Add(new ExtJSField_hidden
            {
                type = "hidden",
                name = "product_category_id",
                value = product_category_id.ToString()
            });

            var formTableJSON = new
            {
                row = fieldList,
                rowhidden = hiddenList,

                column = 2,
                post_url = "",  //<-
                post_header = "", //<-
                title = "Config",
                icon = "iconRemarkList",
                post_params = Url.Action("Update"),

                button_text = "Save",
                button_icon = "iconSave",
                value_changes = true
            }.ToJson();

            return formTableJSON;
        }

        public string Update(FormCollection collection)
        { 
            if (!String.IsNullOrEmpty(collection["product_id"]) && !String.IsNullOrEmpty(collection["product_category_id"]) && !String.IsNullOrEmpty(collection["display_order"]))
            {
                var product_id = int.Parse(collection["product_id"]);
                var product_category_id = int.Parse(collection["product_category_id"]);
                var display_order = int.Parse(collection["display_order"]);

                var targetObject = new ProductCategoryLinkObject
                {
                    product_id = product_id,
                    category_id = product_category_id,
                    display_order = display_order
                };

                var linkManager = new ProductCategoryLinkManager();
                
                var resultCode = linkManager.UpdateDisplayOrder(targetObject);

                return new {success = true, url = "", msg = "Saved Success"}.ToJson();
            
            } else
            {
                return new {success = false, url = "", msg = "Saved Failed"}.ToJson();
            }
        }
    }
}
