using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Palmary.Loyalty.BO.DataTransferObjects.Security;
using Palmary.Loyalty.BO.DataTransferObjects.Gift;
using Palmary.Loyalty.BO.Modules.Gift;
using Palmary.Loyalty.BO.Modules.Utility;
using Palmary.Loyalty.BO.Modules.Administration.Security;
using Palmary.Loyalty.Web_backend.Modules.GlobalHandlers;
using Palmary.Loyalty.Web_backend.Modules.Utility;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.TableData;
using Palmary.Loyalty.Web_backend;
using Palmary.Loyalty.BO.DataTransferObjects.Tree;

using Palmary.Loyalty.Common;

namespace Palmary.Loyalty.Web_backend.Modules.Gift
{
    [Authorize]
    public class GiftOrderController : Controller
    {
        private int _id;

        public GiftOrderController()
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
                title = "Gift",
                pageSize = 20,
                add_url = "",
                add_hidden = true,
                search_text_hidden = true,
                delete_hidden = true,
                checkbox_hidden = true,
                delete_url = "",
                columns = new[]{
                      new {header = "Display Order", dataIndex="display_order", width = 120, type = "input", renderer = "", sortable = true, column = true},
                      new {header = "Product Code", dataIndex="product_no", width = 140, type = "input", renderer = "", sortable = true, column = true},
                      new {header = "Product Name", dataIndex="name", width = 160, type = "input", renderer = "", sortable = true, column = true},
                      new {header = "Action", dataIndex="action", width = 120, type = "input", renderer = "", sortable = true, column = true},
                },
                fields = new string[] { "id", "display_order", "product_no", "name", "action" }
            };

            return result.ToJson();
        }

        public string ListData()
        {
            if (!String.IsNullOrEmpty(Request.QueryString["category_id"]))
            {
                var cat_id = int.Parse(Request.QueryString["category_id"]);  //from GET

                var manager = new GiftManager();

                if (cat_id == -2) // hot item
                {
                    var list = manager.GetGiftLists_isHotItem();

                    var newList = list.Select(x => new
                    {
                        display_order = x.hot_item_display_order,
                        product_no = x.gift_no,
                        name = x.name,
                        action = "<button type='button' style=\"display:\" onclick=\"new com.embraiz.tag().open_pop_up('poe:" + x.gift_id + "','Gift Order Edit','com.palmary.giftorder.js.edit','iconRemarkList','iconRemarkList','iconRemarkList'," + cat_id + ",'owner','com.palmary.giftorder.js.index')\">Change Order</button>"
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
                {
                    var list = manager.GetGiftListsByCategory(cat_id);

                    var newList = list.Select(x => new
                    {
                        display_order = x.display_order,
                        product_no = x.gift_no,
                        name = x.name,
                        action = "<button type='button' style=\"display:\" onclick=\"new com.embraiz.tag().open_pop_up('poe:" + x.gift_id + "','Gift Order Edit','com.palmary.giftorder.js.edit','iconRemarkList','iconRemarkList','iconRemarkList'," + cat_id + ",'owner','com.palmary.giftorder.js.index')\">Change Order</button>"
                    });

                    var result = new
                    {
                        success = true,
                        items = newList,
                        totalCount = newList.Count()
                    };

                    return result.ToJson();
                }

            }
            else
                return "";
        }

        public string EditForm(FormCollection collection)
        {
            List<ExtJSField> fieldList = new List<ExtJSField>();

            var gift_id = _id;
            var gift_category_id = int.Parse(collection["category_id"]);

            var gift_name = "";
            var category_name = "";
            var display_order = "";

            if (gift_category_id == -2)
            {
                category_name = "Hot Item";

                var giftManager = new GiftManager();
                var sql_result = false;
                var gift = giftManager.GetGiftDetailBy(SessionManager.Current.obj_id, gift_id, ref sql_result);
                gift_name = gift.name;
                display_order = gift.hot_item_display_order.ToString();
            }
            else
            {
                var linkManager = new GiftCategoryLinkManager();
                var linkObject = linkManager.GetDetail(gift_id, gift_category_id);
                gift_name = linkObject.gift_name;
                category_name = linkObject.category_name;
                display_order = linkObject.display_order.ToString();
            } 


            fieldList.Add(new ExtJSField
            {
                name = "gift_name_display",
                fieldLabel = "Gift Name",
                type = "input",
                colspan = 2,
                tabIndex = "1",
                value = gift_name,
                readOnly = true
            });

            fieldList.Add(new ExtJSField
            {
                name = "category_name_display",
                fieldLabel = "Category Name",
                type = "input",
                colspan = 2,
                tabIndex = "2",
                value = category_name,
                readOnly = true
            });

            fieldList.Add(new ExtJSField
            {
                name = "display_order",
                fieldLabel = "Display Order",
                type = "input",
                colspan = 2,
                tabIndex = "3",
                value = display_order
            });

            // Hidden Fields
            List<ExtJSField_hidden> hiddenList = new List<ExtJSField_hidden>();
            hiddenList.Add(new ExtJSField_hidden
            {
                type = "hidden",
                name = "gift_id",
                value = gift_id.ToString()
            });

            hiddenList.Add(new ExtJSField_hidden
            {
                type = "hidden",
                name = "gift_category_id",
                value = gift_category_id.ToString()
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

        public string GetListItems()
        {
            GiftCategoryManager _giftCategoryManager = new GiftCategoryManager();

            var resultList = _giftCategoryManager.GetGiftCategoryLists(SessionManager.Current.obj_id, 0, 0, "");

            var catNodeList = new List<CategoryNode>();
            foreach (var x in resultList)
            {
                var node = new CategoryNode() { id = x.category_id, ParentID = x.parent_id, text = x.name, expanded = true };
                catNodeList.Add(node);
            }

            var treeList = TreeManager.BuildTree_selectList(catNodeList);
            var itemList = new List<ExtJsDataRowListItem> { };

            itemList.Add(new ExtJsDataRowListItem { 
                        id = -2,
                        value = "Hot Item"
                    }
                );
            
            foreach (var n in treeList)
            {
                var item = new ExtJsDataRowListItem
                {
                    id = n.id,
                    value = n.text
                };
                itemList.Add(item);
            }
   
            return string.Format(@"{{""success"":true, ""data"":{0}}}", itemList.ToJson());
        }

        public string Update(FormCollection collection)
        {
            if (!String.IsNullOrEmpty(collection["gift_id"]) && !String.IsNullOrEmpty(collection["gift_category_id"]) && !String.IsNullOrEmpty(collection["display_order"]))
            {
                var gift_id = int.Parse(collection["gift_id"]);
                var gift_category_id = int.Parse(collection["gift_category_id"]);
                var display_order = int.Parse(collection["display_order"]);

                if (gift_category_id == -2) // Hot item
                { 
                    var giftManager = new GiftManager();
                    var gift = giftManager.GetDetail(gift_id);

                    gift.hot_item_display_order = display_order;
                    var resultCode = giftManager.UpdateHotItemDisplayOrder(gift);

                }
                else
                { 
                    var targetObject = new GiftCategoryLinkObject
                    {
                        gift_id = gift_id,
                        category_id = gift_category_id,
                        display_order = display_order
                    };

                    var linkManager = new GiftCategoryLinkManager();

                    var resultCode = linkManager.UpdateDisplayOrder(targetObject); 
                }
                
                return new { success = true, url = "", msg = "Saved Success" }.ToJson();

            }
            else
            {
                return new { success = false, url = "", msg = "Saved Failed" }.ToJson();
            }
        }
    }
}
