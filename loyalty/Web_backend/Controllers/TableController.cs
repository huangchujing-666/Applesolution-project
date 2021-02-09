using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.IO;
using System.Web.Routing;

using Palmary.Loyalty.BO.Modules;
using Palmary.Loyalty.BO.Database;
using Palmary.Loyalty.BO.Modules.Administration.Table;
using Palmary.Loyalty.Web_backend.ObjectModels;
using Newtonsoft.Json;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.ExtJsEntity;
using Palmary.Loyalty.Web_backend.Modules.GlobalHandlers;
using Palmary.Loyalty.Web_backend.Modules.Utility;

using Palmary.Loyalty.Web_backend;
using Palmary.Loyalty.Web_backend.Modules.Administration;
using Palmary.Loyalty.Web_backend.Modules.ExtJS;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.PayoadKey;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.TableData;
using Palmary.Loyalty.BO.Modules.Location;
using Palmary.Loyalty.BO.Modules.Gift;
using Palmary.Loyalty.BO.Modules.Administration.User;
using Palmary.Loyalty.BO.Modules.Product;
using Palmary.Loyalty.BO.Modules.Member;
using Palmary.Loyalty.BO.Modules.Service;
using Palmary.Loyalty.BO.Modules.Export;
using Palmary.Loyalty.BO.DataTransferObjects.Security;
using Palmary.Loyalty.BO.DataTransferObjects.DB;
using Palmary.Loyalty.Common;
using Palmary.Loyalty.BO.DataTransferObjects.Tree;
using Palmary.Loyalty.BO.DataTransferObjects.Gift;
using Palmary.Loyalty.BO.DataTransferObjects.Service;
using Palmary.Loyalty.BO.Modules.Utility;
using Palmary.Loyalty.BO.Modules.Administration.Role;
using Palmary.Loyalty.BO.Modules.Administration.Security;
using Palmary.Loyalty.BO.Modules.Reminder;

namespace Palmary.Loyalty.Web_backend.Controllers
{
    [Authorize]
    public class TableController : Controller
    {
        //
        // GET: /Common/
        private string _module;
        private int _userId = SessionManager.Current.obj_id;
        private int _id = 0;

        private TableListHandler _tableList;
     
        public TableController()
        {
            _tableList = new TableListHandler();
        }

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
        
            if (RouteData.Values["Module"] != null)
            {
                _module = RouteData.Values["Module"].ToString().ToLower();
            }

            if (RouteData.Values["Id"] != null)
            {
                _id = int.Parse(RouteData.Values["Id"].ToString());
            }
      
            SystemHandler.InitSystemParameters(this.HttpContext.ApplicationInstance as MvcApplication);
        }


        //step1: for init table (Method A)
        public string InitWithSearchColumn()
        {

            if (!string.IsNullOrEmpty(_module))
            {             
                var message = "";
                var user_id = 1; // int.Parse(Session["userId"].ToString());
                var rows = _tableList.GetExtJsSearchColumns(user_id, _module, ref message);
                if (string.IsNullOrEmpty(message))
                {
                    var thePost_url = "";
                    var header_url = "";
                    if (_id == 0)
                    {
                        thePost_url = Url.Action("ListData", new { Module = _module });
                        header_url = Url.Action("GridHeader", new { Module = _module });
                    }
                    else
                    {
                        thePost_url = Url.Action("ListData/" + _module + "/" + _id.ToString());
                        header_url = Url.Action("GridHeader/" + _module + "/" + _id.ToString());
                    }
                    
                    var json = new
                    {
                        post_url = thePost_url,
                        post_header = header_url,
                        title = "Search",
                        icon = "iconSearch",
                        post_params = Url.Action("SearchParams", new { Module = _module }),
                        button_text = "Search",
                        value_changes = false,
                        sub_title = "Advance Search",
                        isType = true,
                        row = rows
                    }.ToJson();

                    return json;
                }
            }
            return new { success = false, items = "", totalCount = 0 }.ToJson();
        }

        //step1: for init table (Method B)
        public string Init()
        {
            if (_module == "userrole")
            {
                RoleAccessHandler theRole = new RoleAccessHandler();
                
                var listDataJson = theRole.Get_RoleDropDownListData();

                //var result = new { success = true, items = "{0}", totalCount = 0 }.ToJson();
                //result = result.Replace("{", "{{");
                //result = result.Replace("}", "}}");
                //result = result.Replace("\"items\":\"{{0}}\"", "\"items\":{0}");
                //result = string.Format(result, listDataJson);

                var result = listDataJson.ToJson();
                result = result.Replace("{[", "[");
                result = result.Replace("]}", "]");
                return result;
            }
            else if (_module == "passcodeexcel")
            {
                return TableFormHandler.UploadFormPasscodeExcel();
            }
            return new { success = false, items = "", totalCount = 0 }.ToJson();
        }

        //step2: for data grid header
        public string GridHeader()
        {
            if (!string.IsNullOrEmpty(_module))
            {
                string title = "";
                string link = "";

                int pageSize = 0;
                int status = 0;
                bool addHidden = false;
                bool searchHidden = false;
                bool deleteHidden = false;
                bool export_hidden = false;
                bool checkboxHidden = false;

                var tableManager = new TableManager();
                var gridHeaders = tableManager.GetExtJsGridHeaders(_module, ref status, ref title, ref link, ref pageSize,
                          ref searchHidden, ref addHidden, ref deleteHidden, ref export_hidden, ref checkboxHidden);

                var accessManager = new AccessManager();
                var privilege = accessManager.AccessModule(_module);

                if (!addHidden)
                {
                    // premission of access object
                    if (privilege.insert_status == 0)
                        addHidden = true;
                }

                if (!deleteHidden)
                {
                    // premission of access object
                    if (privilege.delete_status == 0)
                    {
                        deleteHidden = true;
                        checkboxHidden = true;
                    }
                }

                string addUrl = "";
                var buttonList = new List<ExtJsGridButton>();

                if (!addHidden)
                    addUrl = string.Format("new {0}().addTag", link);

                if (_module == CommonConstant.Module.member)
                {
                    var button = new ExtJsGridButton()
                    {
                        xtype = "button",
                        text = "Import",
                        name = "Lead:import",
                        iconCls = "iconDatabase_go",
                        handler = "new com.embraiz.tag().openNewTag('MI:" + 0 + "','Member Import','com.palmary.memberimport.js.index','iconRole16','iconRole16','iconRole16','" + 0 + "')"
                    };
                    buttonList.Add(button);

                    //buttonList.Add(new ExtJsGridButton()
                    //{
                    //    xtype = "button",
                    //    text = "Member Field",
                    //    name = "Lead:import",
                    //    iconCls = "iconRole16",
                    //    handler = "new com.embraiz.tag().openNewTag('MF:" + 0 + "','Member Field','com.palmary.memberfield.js.index','iconRole16','iconRole16','iconRole16','" + 0 + "')"
                    //});

                    //buttonList.Add(new ExtJsGridButton()
                    //{
                    //    xtype = "button",
                    //    text = "Member Group",
                    //    name = "Lead:import",
                    //    iconCls = "iconRole16",
                    //    handler = "new com.embraiz.tag().openNewTag('MG:" + 0 + "','Member Group','com.palmary.membergroup.js.index','iconRole16','iconRole16','iconRole16','" + 0 + "')"
                    //});
                }
                else if (_module == "memberimport")
                {
                    //buttonList.Add(new ExtJsGridButton()
                    //{
                    //    xtype = "button",
                    //    text = "Update",
                    //    name = "Lead:import",
                    //    iconCls = "iconRole16",
                    //    handler = "new com.embraiz.tag().open_pop_up('MI:" + (int)CommonConstant.MemberImportType.update + "','Update ','com.palmary.memberimport.js.insert_popupform','iconRole16','iconRole16','iconRole16','" + 0 + "')"
                    //});
                }
                else if (_module == "transactionhistory")
                {
                    var button = new ExtJsGridButton()
                    {
                        xtype = "button",
                        text = "Import",
                        name = "Lead:import",
                        iconCls = "iconDatabase_go",
                        handler = "new com.embraiz.tag().openNewTag('TI:" + 0 + "','Transaction Import','com.palmary.transactionimport.js.index','iconRole16','iconRole16','iconRole16','" + 0 + "')"
                    };
                    buttonList.Add(button);

                    //buttonList.Add(new ExtJsGridButton()
                    //{
                    //    xtype = "button",
                    //    text = "Transaction Field",
                    //    name = "Lead:import",
                    //    iconCls = "iconRole16",
                    //    handler = "new com.embraiz.tag().openNewTag('TF:" + 0 + "','Transaction Field','com.palmary.transactionfield.js.index','iconRole16','iconRole16','iconRole16','" + 0 + "')"
                    //});

                    //buttonList.Add(new ExtJsGridButton()
                    //{
                    //    xtype = "button",
                    //    text = "Transaction Group",
                    //    name = "Lead:import",
                    //    iconCls = "iconRole16",
                    //    handler = "new com.embraiz.tag().openNewTag('TG:" + 0 + "','Transaction Group','com.palmary.transactiongroup.js.index','iconRole16','iconRole16','iconRole16','" + 0 + "')"
                    //});
                }
                else if (_module == "transactionimport")
                {
                    //buttonList.Add(new ExtJsGridButton()
                    //{
                    //    xtype = "button",
                    //    text = "Add PostPaid Service Payment ",
                    //    name = "Lead:import",
                    //    iconCls = "iconDatabase_go",
                    //    handler = "new com.embraiz.tag().open_pop_up('TI:" + (int)CommonConstant.TransactionType.postpaidservice + "','Add PostPaid Service Payment','com.palmary.transactionimport.js.insert_popupform','iconRole16','iconRole16','iconRole16','" + 0 + "')"
                    //});

                    buttonList.Add(new ExtJsGridButton()
                    {
                        xtype = "button",
                        text = "Add Product Purchase",
                        name = "Lead:import",
                        iconCls = "iconDatabase_go",
                        handler = "new com.embraiz.tag().open_pop_up('TI:" + (int)CommonConstant.TransactionType.purchase_product + "','Add Product Purchase','com.palmary.transactionimport.js.insert_popupform','iconRole16','iconRole16','iconRole16','" + 0 + "')"
                    });

                    //buttonList.Add(new ExtJsGridButton()
                    //{
                    //    xtype = "button",
                    //    text = "Bulk Reward/Deduct",
                    //    name = "Lead:import",
                    //    iconCls = "iconDatabase_go",
                    //    handler = "new com.embraiz.tag().open_pop_up('TI:" + (int)CommonConstant.TransactionType.point_adjustment + "','Bulk Reward/Deduct','com.palmary.transactionimport.js.insert_popupform','iconRole16','iconRole16','iconRole16','" + 0 + "')"
                    //});
                }

                return
                   new
                   {
                       module = _module,
                       title = title,
                       pageSize = pageSize,
                       add_url = addUrl,
                       add_hidden = addHidden,
                       search_text_hidden = searchHidden,
                       delete_hidden = deleteHidden,
                       export_hidden = export_hidden,
                       checkbox_hidden = checkboxHidden,
                       delete_url = Url.Action("MultiDelete", _module),
                       //Url.Action("Delete", new { Module = _module }),
                       columns =
                           gridHeaders != null
                               ? gridHeaders.Where(x => x.column)
                               : new List<sp_GetExtJSGridHeadersResult>(),
                       fields =
                           gridHeaders != null ? gridHeaders.Select(x => x.dataIndex) : new List<string>(),
                       button_items = buttonList

                   }.ToJson();
                
            }
            return "";
        }

        //step3: for listing data
        public string ListData(FormCollection collection)
        {
            // table with drop down select
            if (_module == "roleaccessdetail")
            {
                RoleAccessHandler roleAccessHandler = new RoleAccessHandler();

                var role_id = collection.GetFormValue(PayloadKeys.RoleId);

                var listDataJson = roleAccessHandler.Get_RoleAccessDetail(SessionManager.Current.obj_id, int.Parse(collection.GetFormValue(PayloadKeys.RoleId)));

                var result = new {  items = "{0}" }.ToJson();
                result = result.Replace("{", "{{");
                result = result.Replace("}", "}}");
                result = result.Replace("\"items\":\"{{0}}\"", "\"items\":{0}");
                result = string.Format(result, listDataJson);
                
                return result;
            }
          
            // basic table 
            if (!string.IsNullOrEmpty(_module))
            {
                int startRowIndex = 0;
                int rowLimit = 0;

                if (!String.IsNullOrWhiteSpace(Request.QueryString["start"]) && !String.IsNullOrWhiteSpace(Request.QueryString["limit"]))
                {
                    startRowIndex = int.Parse(Request.QueryString["start"]);
                    rowLimit = int.Parse(Request.QueryString["limit"]);
                }

                //Get SearchParams
                var filter = "";
                if (Request.QueryString["filter"] != null && Request.QueryString["filter"] != "")
                {
                    filter = Request.QueryString["filter"];
                }
             
                var sortStr = Request.QueryString["sort"];
                var sortOrder = CommonConstant.SortOrder.desc;
                var sort = "";

                if (!string.IsNullOrEmpty(sortStr))
                {
                    var sortObj = JsonConvert.DeserializeObject<List<ExtJsSort>>(sortStr).First();
                    sort = sortObj.property;
                    if (sortObj.direction.ToLower().Equals("asc"))
                    {
                        sortOrder = CommonConstant.SortOrder.asc;
                    }
                }
                //var id = 0;
                //if (RouteData.Values["id"] != null)
                //{
                //    id = int.Parse(RouteData.Values["id"].ToString());
                //}
                //rowTotal.Diagnostics.Debug.WriteLine("id: " + id);

                //var table = JsonConvert.DeserializeObject<dynamic>(searchParams);

                int rowTotal = 0;

                List<SearchParmObject> searchParmList;

                if (filter == "")
                {
                    searchParmList = new List<SearchParmObject>();
                }
                else
                {
                    var jsonExtractHelper = new JsonExtractHelper();
                    searchParmList = jsonExtractHelper.ExtJSFormSearchParm(filter);
                }

                var listDataJson = _tableList.GetListDataBy(_userId, _id, startRowIndex, rowLimit, sort, sortOrder, _module, searchParmList, ref rowTotal);

                var result = new { success = true, items = "{0}", totalCount = rowTotal }.ToJson();
                result = result.Replace("{", "{{");
                result = result.Replace("}", "}}");
                result = result.Replace("\"items\":\"{{0}}\"", "\"items\":{0}");
                result = string.Format(result, listDataJson);
                return result;
            }
            return new { success = false, items = "", totalCount = 0 }.ToJson();
        }

        public string GetListItems()
        {
            var resultCode = CommonConstant.SystemCode.undefine;

            if (_module == "CompareCondition_onlyrange".ToLower())
            {
                var _tableManager = new TableManager();
                var itemList = _tableManager.GetListingItemsByCode(SessionManager.Current.obj_id, "CompareCondition");

                itemList = itemList.Where(x => x.id >= 3);
                return new {success = true, data = itemList}.ToJson();
            }
            else if (_module == "memberlevel")
            {
                var _memberLevelManager = new MemberLevelManager();
                var systemCode = CommonConstant.SystemCode.undefine;
                var resultList = _memberLevelManager.GetListAll(true, ref systemCode);

                var itemList = new List<ExtJsDataRowListItem> { };
                
                //int insertCounter = 0;
                //foreach (var item in resultList)
                //{
                //    itemList.Insert(insertCounter, new ExtJsDataRowListItem
                //    {
                //        id = item.level_id,
                //        value = item.name
                //    });
                //    insertCounter++;
                //}

                foreach (var item in resultList)
                {
                    itemList.Add(new ExtJsDataRowListItem
                    {
                        id = item.level_id,
                        value = item.name
                    });
                }

                return string.Format(@"{{""success"":true, ""data"":{0}}}", itemList.ToJson());
            }
            else if (_module == "giftcategory")
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
                foreach (var n in treeList)
                {
                    var item = new ExtJsDataRowListItem
                    {
                        id = n.id,
                        value = n.text
                    };
                    itemList.Add(item);
                }
                //var itemList = new List<ExtJsDataRowListItem> { };
                //int insertCounter = 0;

                //foreach (var item in resultList)
                //{
                //    itemList.Insert(insertCounter, new ExtJsDataRowListItem
                //    {
                //        id = item.category_id,
                //        value = item.name
                //    });
                //    insertCounter++;
                //}

                return string.Format(@"{{""success"":true, ""data"":{0}}}", itemList.ToJson());
            }
            else if (_module == "productcategory")
            {
                ProductCategoryManager _productCategoryManager = new ProductCategoryManager();

                var resultList = _productCategoryManager.GetProductCategoryLists();

                var catNodeList = new List<CategoryNode>();
                foreach (var x in resultList)
                {
                    var node = new CategoryNode() { id = x.category_id, ParentID = x.parent_id, text = x.name, expanded = true };
                    catNodeList.Add(node);
                }

                var treeList = TreeManager.BuildTree_selectList(catNodeList);
                var itemList = new List<ExtJsDataRowListItem> { };
                foreach (var n in treeList)
                {
                    var item =  new ExtJsDataRowListItem
                    {
                        id = n.id,
                        value = n.text
                    };
                    itemList.Add(item);
                }

                //var itemList = new List<ExtJsDataRowListItem> { };
                //int insertCounter = 0;

                //foreach (var item in resultList)
                //{
                //    itemList.Insert(insertCounter, new ExtJsDataRowListItem
                //    {
                //        id = item.category_id,
                //        value = item.name
                //    });
                //    insertCounter++;
                //}

                return string.Format(@"{{""success"":true, ""data"":{0}}}", itemList.ToJson());
            }
            else if (_module == "location")
            {
                LocationManager _locationManager = new LocationManager();

                var resultList = _locationManager.GetLocationLists(SessionManager.Current.obj_id, 0, 0, "");

                var itemList = new List<ExtJsDataRowListItem> { };
                int insertCounter = 0;

                foreach (var item in resultList)
                {
                    itemList.Insert(insertCounter, new ExtJsDataRowListItem
                    {
                        id = item.location_id,
                        value = item.name
                    });
                    insertCounter++;
                }

                return string.Format(@"{{""success"":true, ""data"":{0}}}", itemList.ToJson());
            }
            else if (_module == "gift")
            {
                GiftManager _giftManager = new GiftManager();

              
                var resultList = new List<GiftObject>();

                if (_id > 0)
                    resultList = _giftManager.GetListByMember(_id, ref resultCode);
                else
                    resultList = _giftManager.GetList(ref resultCode);

                var itemList = new List<ExtJsDataRowListItem> { };
                int insertCounter = 0;

                foreach (var item in resultList)
                {
                    itemList.Insert(insertCounter, new ExtJsDataRowListItem
                    {
                        id = item.gift_id,
                        value = item.gift_no + " - " + item.name
                    });
                    insertCounter++;
                }

                return string.Format(@"{{""success"":true, ""data"":{0}}}", itemList.ToJson());
            }
            else if (_module == "role")
            {
                var roleManager = new RoleManager();

                var searchParmList = new List<SearchParmObject>();
                var rowTotal = 0;
                var resultList = roleManager.GetList(0, 100, searchParmList, "", CommonConstant.SortOrder.asc, ref resultCode, ref rowTotal).ToList();

                var itemList = new List<ExtJsDataRowListItem> { };

                foreach (var x in resultList)
                {
                    if (x.status == CommonConstant.Status.active)
                    {
                        itemList.Add(new ExtJsDataRowListItem()
                        {
                            id = x.role_id,
                            value = x.name
                        });
                    }
                }

                return string.Format(@"{{""success"":true, ""data"":{0}}}", itemList.ToJson());
            }
            else if (_module == "product")
            {
                ProductManager _productManager = new ProductManager();

   
                var resultList = _productManager.GetList(ref resultCode);

                var itemList = new List<ExtJsDataRowListItem> { };
                int insertCounter = 0;

                foreach (var item in resultList)
                {
                    itemList.Insert(insertCounter, new ExtJsDataRowListItem
                    {
                        id = item.product_id,
                        value = item.product_no + " - " + item.name
                    });
                    insertCounter++;
                }

                return string.Format(@"{{""success"":true, ""data"":{0}}}", itemList.ToJson());
            }
            else if (_module == "product_any")
            {
                ProductManager _productManager = new ProductManager();

                var resultList = _productManager.GetList(ref resultCode);

                var itemList = new List<ExtJsDataRowListItem> { };
                int insertCounter = 0;
                itemList.Insert(insertCounter, new ExtJsDataRowListItem
                {
                    id = -1,
                    value = "Any"
                });
                insertCounter++;

                foreach (var item in resultList)
                {
                    itemList.Insert(insertCounter, new ExtJsDataRowListItem
                    {
                        id = item.product_id,
                        value = item.name
                    });
                    insertCounter++;
                }

                return string.Format(@"{{""success"":true, ""data"":{0}}}", itemList.ToJson());
            }
            else if (_module == "membercategory")
            {
                var memberCategoryManager = new MemberCategoryManager();
                var system_code = CommonConstant.SystemCode.undefine;
                var resultList = memberCategoryManager.GetMemberCategory_list(ref system_code);
                var itemList = new List<ExtJsDataRowListItem> { };
                int insertCounter = 0;

                foreach (var item in resultList)
                {
                    itemList.Insert(insertCounter, new ExtJsDataRowListItem
                    {
                        id = item.category_id,
                        value = item.name
                    });
                    insertCounter++;
                }

                return string.Format(@"{{""success"":true, ""data"":{0}}}", itemList.ToJson());
            }
            else if (_module == "servicecategory")
            {
                var serviceCategoryManager = new ServiceCategoryManager();
              
                var resultList = new List<ServiceCategoryObject>();

                resultList = serviceCategoryManager.GetList(ref resultCode);

                var catNodeList = new List<CategoryNode>();
                foreach (var x in resultList)
                {
                    var node = new CategoryNode() { id = x.category_id, ParentID = x.parent_id, text = x.name, expanded = true };
                    catNodeList.Add(node);
                }

                var treeList = TreeManager.BuildTree_selectList(catNodeList);
                var itemList = new List<ExtJsDataRowListItem> { };
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
            else if (_module == "membercustomgroup")
            {
                
                var itemList = new List<ExtJsDataRowListItem> { };

                var item = new ExtJsDataRowListItem
                {
                    id = 1,
                    value = "Loyalty_group_1"
                };
                itemList.Add(item);

                var item2 = new ExtJsDataRowListItem
                {
                    id = 2,
                    value = "Loyatly_group_2"
                };
                itemList.Add(item2);

                var item3 = new ExtJsDataRowListItem
                {
                    id = 3,
                    value = "CTM_group_1"
                };
                itemList.Add(item3);


                return string.Format(@"{{""success"":true, ""data"":{0}}}", itemList.ToJson());
            }
            else if (_module == "transactioncustomgroup")
            {

                var itemList = new List<ExtJsDataRowListItem> { };

                var item = new ExtJsDataRowListItem
                {
                    id = 1,
                    value = "Loyalty_tran_1"
                };
                itemList.Add(item);

                var item2 = new ExtJsDataRowListItem
                {
                    id = 2,
                    value = "Loyatly_tran_2"
                };
                itemList.Add(item2);

                var item3 = new ExtJsDataRowListItem
                {
                    id = 3,
                    value = "CTM_tran_1"
                };
                itemList.Add(item3);

                return string.Format(@"{{""success"":true, ""data"":{0}}}", itemList.ToJson());
            }
            else if (_module == "remindertemplate")
            {
                var systemCode = CommonConstant.SystemCode.undefine;
                var reminderTemplateManager = new ReminderTemplateManager();
                var objList = reminderTemplateManager.GetListAll(true, ref systemCode);

                var itemList = new List<ExtJsDataRowListItem> { };

                foreach (var o in objList)
                {
                    var item = new ExtJsDataRowListItem
                    {
                        id = o.reminder_template_id,
                        value = o.name
                    };
                    itemList.Add(item);
                }

                return string.Format(@"{{""success"":true, ""data"":{0}}}", itemList.ToJson());
            }
            else if (_module == "reportmemberdemographictype")
            {
                var systemCode = CommonConstant.SystemCode.undefine;
                var reminderTemplateManager = new ReminderTemplateManager();
                var objList = reminderTemplateManager.GetListAll(true, ref systemCode);

                var itemList = new List<ExtJsDataRowListItem> { };
                itemList.Add(new ExtJsDataRowListItem() { 
                    id = 1,
                    value = "Member Level"
                }
                );

                itemList.Add(new ExtJsDataRowListItem()
                {
                    id = 2,
                    value = "Gender"
                }
              );

                itemList.Add(new ExtJsDataRowListItem()
                {
                    id = 3,
                    value = "Age Range"
                }
              );

                return string.Format(@"{{""success"":true, ""data"":{0}}}", itemList.ToJson());
            }
            else
            {
                TableManager _tableManager = new TableManager();
                var itemList = _tableManager.GetListingItemsByCode(SessionManager.Current.obj_id, _module);
                return string.Format(@"{{""success"":true, ""data"":{0}}}", itemList.ToJson());
            }
        }

        public string Delete(string module)
        {
            var _moduleManager = new ModuleManager();

            var ids = Request.Form["id"];
            System.Diagnostics.Debug.WriteLine("ids: " + ids);
            var idArrary = ids.Split(',');

            var deleteFlag = true;
            var delete_remark = "";
            foreach (var idStr in idArrary)
            {
                var rec_id = int.Parse(idStr);

                if (rec_id > 0)
                {
                    var result = _moduleManager.SoftDelete(_module, rec_id, SessionManager.Current.obj_id, ref delete_remark);
                    if (!result) deleteFlag = false;
                }
            }

            return deleteFlag ? "{success:true,url:'',msg:'Delete Success'}" : "{success:true,url:'',msg:'Delete Failed: <br/>" + delete_remark + "'}";
        }

        public string SearchParams(FormCollection collection)
        {
            if (!string.IsNullOrEmpty(_module))
            {
                switch (_module)
                {
                    //case "passcode":
                    //{
                    //    var pars = new List<SearchFilter>();

                    //    // passcode_id
                    //    if (!String.IsNullOrWhiteSpace(collection["pin_value"]))
                    //    pars.Add(new SearchFilter()
                    //    {
                    //        property = "pin_value",
                    //        value = collection.GetFormValue(PayloadKeys.pin_value)
                    //    });

                    //    // Active_date
                    //    if (collection.GetFormValue(PayloadKeys.Active_date) != default(DateTime)) // 0001-01-01
                    //    pars.Add(new SearchFilter()
                    //    {
                    //        property = "active_date",
                    //        value = collection.GetFormValue(PayloadKeys.Active_date).ToString("yyyy-MM-dd")
                    //    });

                    //    // Expiry_date
                    //    if (collection.GetFormValue(PayloadKeys.Expiry_date) != default(DateTime)) // 0001-01-01
                    //    pars.Add(new SearchFilter()
                    //    {
                    //        property = "expiry_date",
                    //        value = collection.GetFormValue(PayloadKeys.Expiry_date).ToString("yyyy-MM-dd")
                    //    });

                    //    // Point
                    //    if (collection.GetFormValue(PayloadKeys.Point) != 0)
                    //    pars.Add(new SearchFilter()
                    //    {
                    //        property = "point",
                    //        value = collection.GetFormValue(PayloadKeys.Point).ToString()
                    //    });

                    //    // member_no
                    //    if (!String.IsNullOrWhiteSpace(collection["member_no"]))
                    //    pars.Add(new SearchFilter()
                    //    {
                    //        property = "member_no",
                    //        value = collection.GetFormValue(PayloadKeys.Member_no).ToString()
                    //    });

                    //    // point_range_lower
                    //    if (collection.GetFormValue(PayloadKeys.point_range_lower) != 0)
                    //    pars.Add(new SearchFilter()
                    //    {
                    //        property = PayloadKeys.point_range_lower.ToString(),
                    //        value = collection.GetFormValue(PayloadKeys.point_range_lower).ToString()
                    //    });

                    //    // point_range_upper
                    //    if (collection.GetFormValue(PayloadKeys.point_range_upper) != 0)
                    //    pars.Add(new SearchFilter()
                    //    {
                    //        property = PayloadKeys.point_range_upper.ToString(),
                    //        value = collection.GetFormValue(PayloadKeys.point_range_upper).ToString()
                    //    });

                    //    return new { success = true, @params = pars }.ToJson();
                    //}
                    default:
                    {
                        var pars = new List<SearchFilter>();
                        foreach (var coll in collection)
                        {
                            if (coll.Equals("change_fields"))
                                continue;
                            pars.Add(new SearchFilter()
                            {
                                property = coll.ToString(),
                                value = collection[coll.ToString()]
                            });
                        }
                        return new { success = true, @params = pars }.ToJson();
                    }
                }
            }
            return "";
        }

        public string Export(FormCollection collection)
        {          
            if (!string.IsNullOrEmpty(_module))
            {
                var module_export = "Export/" + _module;

                var file_full_name = DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".csv";
                var writeFullPath = PathHandler.GetStorage_serverFullPath(module_export, file_full_name);
                var fileDownloadPath = PathHandler.GetStorage_relativePath(module_export, file_full_name);

                var dirPath = Path.Combine(PathHandler.GetStorage_serverPath(), module_export);
                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }

                var exportManager = new ExportManager();
                var systemCode = CommonConstant.SystemCode.undefine;

                var searchParmList = new List<SearchParmObject>();
                foreach (var coll in collection)
                {
                    if (coll.Equals("change_fields"))
                        continue;
                    searchParmList.Add(new SearchParmObject()
                    {
                        property = coll.ToString(),
                        value = collection[coll.ToString()]
                    });
                }

                if (_module == CommonConstant.Module.member.ToLower())
                    systemCode = exportManager.ExportMember(writeFullPath, searchParmList);
                else if (_module == CommonConstant.Module.transactionHistory.ToLower())
                    systemCode = exportManager.ExportTransactionHistory(writeFullPath, searchParmList);
                else if (_module == CommonConstant.Module.giftRedemptionHistory.ToLower())
                    systemCode = exportManager.ExportGiftRedemptionHistory(writeFullPath, searchParmList);
                else if (_module == CommonConstant.Module.memberLevelChange.ToLower())
                    systemCode = exportManager.ExportMemberLevelChange(writeFullPath, searchParmList);

                if (systemCode == CommonConstant.SystemCode.normal)
                    return new { success = true, fileDownloadPath = fileDownloadPath }.ToJson();
                else
                    return new { success = false}.ToJson();
            }
            return new { success = false }.ToJson();
        }
    }
}
