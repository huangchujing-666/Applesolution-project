using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data.Objects;
using Palmary.Loyalty.Web_backend.ObjectModels;
using Palmary.Loyalty.Web_backend.Modules;
using Palmary.Loyalty.BO.Database;

//using Palmary.Loyalty.Web_backend.Infrastructure.ExtJsEntity;

using Palmary.Loyalty.Web_backend.Modules.Utility;

using Palmary.Loyalty.BO.Modules.Administration.Security;
using Palmary.Loyalty.BO.Modules.Member;
using Palmary.Loyalty.BO.Modules.Product;
using Palmary.Loyalty.BO.Modules.Passcode;
using Palmary.Loyalty.BO.Modules.Administration.Table;
using Palmary.Loyalty.BO.Modules.Administration.User;
using System.Web.Mvc;
using System.Web.Routing;
using Palmary.Loyalty.Common;

using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.PayoadKey;
using System.Dynamic;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.TableData;
using Palmary.Loyalty.Web_backend.Modules.GlobalHandlers;
using Palmary.Loyalty.BO.DataTransferObjects.DB;

namespace Palmary.Loyalty.Web_backend.Modules.ExtJS
{
    public class TableListHandler
    {
        private static LoyaltyDBDataContext db = new LoyaltyDBDataContext();
        private TableManager _tableManager;

        public TableListHandler()
        {
           
            _tableManager = new TableManager();
        }

        public IEnumerable<sp_GetExtJSSearchColumnsResult> GetExtJsSearchColumns(int user_id, string _module, ref string message)
        {
            var rows = _tableManager.GetExtJsSearchColumns(user_id, _module, ref message);

            return rows;
        }

        public object GetListDataBy(int userId, int id, int startRowIndex, int rowLimit, string sort, CommonConstant.SortOrder sortOrder, string module_name, List<SearchParmObject> searchParmList, ref int rowTotal)
        {
            var status = new ObjectParameter("status", typeof(int));
            var totalOut = new ObjectParameter("total", typeof(int));
            var remark = new ObjectParameter("remark", typeof(string));
            var json = new ObjectParameter("json", typeof(object));
            
            var module = new Module(module_name);

            try
            { 
                if (module_name == "productcategory")
                {
                    ProductCategoryManager _productManager = new ProductCategoryManager();

                    var resultList = _productManager.GetProductCategoryLists().ToList();

                    var list = new List<ExtJsDataRowProductCategory> { };

                    int insertCounter = 0;

                    foreach (var item in resultList)
                    {
                        System.Diagnostics.Debug.WriteLine(item.category_id, "item.category_id");

                        list.Insert(insertCounter, new ExtJsDataRowProductCategory
                        {
                            name = item.name,
                            id = item.category_id,
                            status = item.status,

                            photo = "/Storage/Product/oenobiol_sp01.jpg",
                            href = "new com.embraiz.tag().openNewTag('EDIT_P_UID:1','Product Category EDIT:"+item.name+"','com.palmary.productCategory.js.edit','iconRole16','iconRole16','iconRole16','1')" //"new com.embraiz.tag().openNewTag('EDIT_P_UID:" + item.category_id + "','Product Category EDIT:" + item.name + "','com.palmary.productCategory.js.edit','iconRole16','iconRole16','iconRole16','" + item.category_id + "')"
                        }); 
                        System.Diagnostics.Debug.WriteLine(id, "id");
                        insertCounter++;
                    }

                    return list.ToJson();
                }
                else if (module_name == "passcodeformat")
                {
                    PasscodeFormatManager _passcodeFormatManager = new PasscodeFormatManager();
                    bool sql_result = false;

                    var format_id = 1; //force to 1
                    var thePasscode = _passcodeFormatManager.GetPasscodeFormatDetailBy(SessionManager.Current.obj_id, format_id, ref sql_result);
                    var list = new List<ExtJsDataRowSetting> { };

                    var insertCounter = 0;

                    list.Insert(insertCounter, new ExtJsDataRowSetting
                        { setting_name = "Passcode Format",
                          setting_value = thePasscode.passcode_format,
                          id = insertCounter
                        });
                    insertCounter++;

                    list.Insert(insertCounter, new ExtJsDataRowSetting
                    {
                        setting_name = "Safety Limit",
                        setting_value = thePasscode.safetyLimit_precent+"%",
                        id = insertCounter
                    });
                    insertCounter++;

                    list.Insert(insertCounter, new ExtJsDataRowSetting
                    {
                        setting_name = "Number Combination",
                        setting_value = (thePasscode.number_combination).ToString(),
                        id = insertCounter
                    });
                    insertCounter++;

                    list.Insert(insertCounter, new ExtJsDataRowSetting
                    {
                        setting_name = "Maximum Generate",
                        setting_value = (thePasscode.maximum_generate).ToString(),
                        id = insertCounter
                    });
                    insertCounter++;

                    list.Insert(insertCounter, new ExtJsDataRowSetting
                    {
                        setting_name = "No of Year Expiry",
                        setting_value = (thePasscode.expiry_month/12).ToString(),
                        id = insertCounter
                    });
                    insertCounter++;

                    return list.ToJson();
                }
                else if (module_name == "passcode")
                {
                    var reuslt = module.LoadListDataToExtJSJson( id, startRowIndex, rowLimit, searchParmList, sort, sortOrder, ref rowTotal);
                   
                    return reuslt; // list.ToJson();
                }
                else if (module_name == "passcodegenerate")
                {
                    var reuslt = module.LoadListDataToExtJSJson( id, startRowIndex, rowLimit, searchParmList, sort, sortOrder, ref rowTotal);
                    return reuslt;
                }
                else if (module_name == "passcodeusagesummary")
                {
                    PasscodeManager _passcodeManager = new PasscodeManager();
                    var resultList = _passcodeManager.GetPasscodeUsageSummary(SessionManager.Current.obj_id).ToList();
   
                    var list = new List<ExtJsDataRow_passcodeGenerateSummary> { };

                    int insertCounter = 0;

                    var counter = 0;

                    foreach (var item in resultList)
                    {
                        counter++;
                        rowTotal = 0; // item.rowTotal ?? 0;
                        System.Diagnostics.Debug.WriteLine("passcodeusagesummary,  item.product_id: " + item.product_id);
                        list.Insert(insertCounter, new ExtJsDataRow_passcodeGenerateSummary
                        {
                            id = item.product_id,
                            product_id = item.product_id,
                            product_no = item.product_no,
                            product_name = item.product_name,
                            no_of_imported = item.no_of_imported ?? 0,
                            no_of_registered = item.no_of_registered ?? 0,
                   
                            href = "new com.embraiz.tag().openNewTag()"
                        });
                        insertCounter++;
                    }

                    return list.ToJson();
                }
                else
                {
                    var result = module.LoadListDataToExtJSJson(id, startRowIndex, rowLimit, searchParmList, sort, sortOrder, ref rowTotal);

                    return result; // list.ToJson();
                }
                //return "";
            }
            catch (Exception ex)
            {
                return "[]";
            }
        }
    }
}