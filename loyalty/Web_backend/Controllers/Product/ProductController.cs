using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Palmary.Loyalty.BO.Modules.Product;
using Palmary.Loyalty.BO.Database;
using Palmary.Loyalty.BO.DataTransferObjects.Security;
using Palmary.Loyalty.BO.DataTransferObjects.Product;
using Palmary.Loyalty.BO.DataTransferObjects.Media;
using Palmary.Loyalty.Common;

using Palmary.Loyalty.BO.Modules.Administration.Security;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.ExtJsEntity;
using Palmary.Loyalty.Web_backend.Modules.GlobalHandlers;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.PayoadKey;
using Palmary.Loyalty.Web_backend.Modules.Utility;
using System.Web.Routing;
using Palmary.Loyalty.Web_backend.Modules.ExtJS;
using Palmary.Loyalty.Web_backend;

namespace Palmary.Loyalty.Web_backend.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        //
        // GET: /Product/
        private ProductManager _productManager;
        private ProductLangManager _productLangManager;

        private int _product_id;

        public ProductController()
        {
            _productManager = new ProductManager();
            _productLangManager = new ProductLangManager();
        }

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            var id = RouteData.Values["id"] ?? "";
            if (!string.IsNullOrEmpty(id.ToString()))
            {
                _product_id = int.Parse(id.ToString());
            }
            SystemHandler.InitSystemParameters(this.HttpContext.ApplicationInstance as MvcApplication);
        }

        public string GetModule()
        {  
            var sql_result = false;
            var product = _productManager.GetProductDetailBy(SessionManager.Current.obj_id, _product_id, null, ref sql_result);
            var lang_list = _productLangManager.GetProductLang_ownedList(_product_id, ref sql_result);
            var formTableJSON = TableFormHandler.GetFormByModule(product, lang_list);
            return formTableJSON;
        }

        public string Update(FormCollection collection)
        {
            var product = new ProductObject();

            product.product_id = collection.GetFormValue(PayloadKeys.Id);
            product.product_no = collection.GetFormValue(PayloadKeys.No);

            // Category
            var categoryString_list = collection.GetFormValue(PayloadKeys.Product.category).Split(',');
            var category_list = new List<ProductCategoryObject>();

            foreach (var c in categoryString_list)
            {
                var obj = new ProductCategoryObject { 
                    category_id = int.Parse(c)
                };
                
                category_list.Add(obj);
            }

            product.category_list = category_list;

            product.price = collection.GetFormValue(PayloadKeys.Price);
            product.point = 0;
            product.consumption_period = collection.GetFormValue(PayloadKeys.Product.Consumption_period);
            product.lost_customer_period = collection.GetFormValue(PayloadKeys.Product.Lost_customer_period);
            //product.passcode_prefix = collection.GetFormValue(PayloadKeys.passcode_prefix);
            
            product.status = collection.GetFormValue(PayloadKeys.Status);
           
            // change_fields
            var change_fields = collection.GetFormValue(PayloadKeys.change_fields);
            var jsonExtractHelper = new JsonExtractHelper();
            var changedFields = jsonExtractHelper.ExtJSFormChangedFields(change_fields);

            // Photo
            var photosJSON = collection.GetFormValue(PayloadKeys.Product.photos);
            var photoField_list = jsonExtractHelper.ExtJSFormPhotosField(photosJSON);

            var photos_list = new List<PhotoObject>();
            foreach (var p in photoField_list)
            {
                var photo = new PhotoObject();
                photo.photo_id = int.Parse(p.id);

                // photo file name
                var path = p.src.Split('/');
                var fileFullName = path.Last();
                var fileNameList = fileFullName.Split('.');
                var photo_file_name = fileNameList[0];

                photo.file_extension = "." + fileNameList[1];
                photo.file_name = photo_file_name.Replace((string)CommonConstant.ImageSizeName_postfix[(int)CommonConstant.ImageSizeType.thumb], ""); // remove _thumb (for view edit form)

                photo.display_order = int.Parse(p.orderedID);
                photo.status = 1;
                photo.name = "";
                photo.caption = "";
                photos_list.Add(photo);
            }
            product.photo_list = photos_list;

            // Languages  
            var lang_list = new List<ProductLangObject>();

            var theLang_tc = new ProductLangObject();
            theLang_tc.name = collection.GetFormValue(PayloadKeys.varWithLang("name", "tc"));
            theLang_tc.product_id = product.product_id;
            theLang_tc.lang_id = (int)CommonConstant.LangCode.tc;
            theLang_tc.description = collection.GetFormValue(PayloadKeys.varWithLang("description", "tc"));
            lang_list.Add(theLang_tc);

            var theLang_en = new ProductLangObject();
            theLang_en.name = collection.GetFormValue(PayloadKeys.varWithLang("name", "en"));
            theLang_en.product_id = product.product_id;
            theLang_en.lang_id = (int)CommonConstant.LangCode.en;
            theLang_en.description = collection.GetFormValue(PayloadKeys.varWithLang("description", "en"));
            lang_list.Add(theLang_en);

            var theLang_sc = new ProductLangObject();
            theLang_sc.name = collection.GetFormValue(PayloadKeys.varWithLang("name", "sc"));
            theLang_sc.product_id = product.product_id;
            theLang_sc.lang_id = (int)CommonConstant.LangCode.sc;
            theLang_sc.description = collection.GetFormValue(PayloadKeys.varWithLang("description", "sc"));
            lang_list.Add(theLang_sc);

            product.lang_list = lang_list;


            var sql_remark = "";
            if (product.product_id == 0)
            {
                var addFlag = _productManager.Create(
                    product,
                    ref sql_remark);

                return addFlag ? "{success:true,url:'',msg:'Add Success'}" : "{success:true,url:'',msg:'Add Failed: " + sql_remark + "'}";
            }
            else
            {
                var updateFlag = _productManager.Update(
                    product,
                    changedFields,
                ref sql_remark);

                return updateFlag ? "{success:true,url:'',msg:'Saved Success'}" : "{success:true,url:'',msg:'Saved Failed: " + sql_remark + "'}";
            }
        }

        public string ToolbarData()
        {
            var toolData = new List<ExtJsButton>();
            toolData.Add(new ExtJsButton("button", "view") { text = "View", hidden = true, iconUrl = "iconView" });
            toolData.Add(new ExtJsButton("button", "edit") { text = "Edit", iconUrl = "iconRole16" });
            var theStr = "function(){new com.embraiz.tag().open_pop_up('','Head Office Fee','com.palmary.productPasscode.js.index','iconHeadOffice','iconHeadOffice','iconHeadOffice','3');}";
            //toolData.Add(new ExtJsButton("button", "passcode") { text = "Passcode", iconUrl = "iconRole16", href = theStr });
             
            var result = new { toolData = toolData }.ToJson();

            // remote double quotation for herf:function
            result = result.Replace(@"""href"":""f", @"""href"":f");
            result = result.Replace(@"""}]}", @"}]}");
            
            return result;
        }

        public string Insert()
        {
            var formTableJSON = TableFormHandler.GetFormByModule(new sp_GetProductDetailByResult(), new List<sp_GetProductLang_ownedListResult>());
            return formTableJSON;
        }

        public string CheckDuplicateProductNo(FormCollection collection)
        {
            var productNo = collection["value"];
            System.Diagnostics.Debug.WriteLine(productNo, "productNo");

            var duplicate = _productManager.CheckDuplicateProductNo(productNo);
            var message = "";
            if (duplicate)
                message = "Product code is duplicate";

            return new { success = true, duplicate = duplicate, message = message }.ToJson();
        }
    }
}