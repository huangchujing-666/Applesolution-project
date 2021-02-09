using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Palmary.Loyalty.BO.Modules.Administration.Security;
using Palmary.Loyalty.BO.Modules.Location;
using Palmary.Loyalty.BO.Database;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.ExtJsEntity;
using Palmary.Loyalty.Web_backend.Modules.GlobalHandlers;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.PayoadKey;
using Palmary.Loyalty.Web_backend.Modules.Utility;
using Palmary.Loyalty.Web_backend.Modules.ExtJS;
using Palmary.Loyalty.Web_backend;
using Palmary.Loyalty.Common;
using Palmary.Loyalty.Common.Languages;
using System.Web.Routing;
using Palmary.Loyalty.BO.DataTransferObjects.Security;

namespace Palmary.Loyalty.Web_backend.Controllers.Location
{
    [Authorize]
    public class LocationController : Controller
    {
        private LocationManager _locationManager;
        private LocationLangManager _locationLangManager;
        private int _location_id;

        public LocationController()
        {
            
            _locationManager = new LocationManager();
            _locationLangManager = new LocationLangManager();
        }

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            var id = RouteData.Values["id"] ?? "";
            if (!string.IsNullOrEmpty(id.ToString()))
            {
                _location_id = int.Parse(id.ToString());
            }
            SystemHandler.InitSystemParameters(this.HttpContext.ApplicationInstance as MvcApplication);
        }
        
        // Create new
        public string Insert()
        {
            var formTableJSON = TableFormHandler.GetFormByModule(new sp_GetLocationDetailResult(), new List<sp_GetLocationLangDetailResult>());
            return formTableJSON;
        }

        // Edit detail
        public string GetModule()
        {
            System.Diagnostics.Debug.WriteLine("GetModule, _location_id: " + _location_id);

            var sql_result = false;
            var location = _locationManager.GetLocationDetail(SessionManager.Current.obj_id, _location_id, ref sql_result);
            var location_lang = _locationLangManager.GetLocationLangDetail(SessionManager.Current.obj_id, _location_id, ref sql_result);
            var formTableJSON = TableFormHandler.GetFormByModule(location, location_lang);
            return formTableJSON;
        }

        // Edit detail toolbar
        public string ToolbarData()
        {
            var toolData = new List<ExtJsButton>();
            toolData.Add(new ExtJsButton("button", "view") { text = "View", hidden = true, iconUrl = "iconView" });
            toolData.Add(new ExtJsButton("button", "edit") { text = "Edit", iconUrl = "iconRole16" });

            var result = new { toolData = toolData }.ToJson();

            // remove double quotation for herf:function
            result = result.Replace(@"""href"":""f", @"""href"":f");
            result = result.Replace(@"""}]}", @"}]}");

            return result;
        }

        public string Update(FormCollection collection)
        {            
            var location_id = collection.GetFormValue(PayloadKeys.Location.location_id);
            var location_no = collection.GetFormValue(PayloadKeys.Location.location_no);
          
            var latitude = collection.GetFormValue(PayloadKeys.Location.latitude);
            var longitude = collection.GetFormValue(PayloadKeys.Location.longitude);
            var phone = collection.GetFormValue(PayloadKeys.Location.phone);
            var fax = collection.GetFormValue(PayloadKeys.Location.fax);
            var address_district = collection.GetFormValue(PayloadKeys.Location.address_district);
            var address_region = collection.GetFormValue(PayloadKeys.Location.address_region);
            var display_order = collection.GetFormValue(PayloadKeys.Location.display_order);
            var status = collection.GetFormValue(PayloadKeys.Location.status);
            var crt_date = collection.GetFormValue(PayloadKeys.Location.crt_date);
            var crt_by_type = collection.GetFormValue(PayloadKeys.Location.crt_by_type);
            var crt_by = collection.GetFormValue(PayloadKeys.Location.crt_by);
            var upd_date = collection.GetFormValue(PayloadKeys.Location.upd_date);
            var upd_by_type = collection.GetFormValue(PayloadKeys.Location.upd_by_type);
            var upd_by = collection.GetFormValue(PayloadKeys.Location.upd_by);
            var record_status = collection.GetFormValue(PayloadKeys.Location.record_status);

            // photo file name
            var path = collection["fileData"].Split('/');
            var fileFullName = path.Last();
            var fileNameList = fileFullName.Split('.');
            var photo_file_name = fileNameList[0];
            var photo_file_extension = "." + fileNameList[1];
            photo_file_name = photo_file_name.Replace((string)CommonConstant.ImageSizeName_postfix[(int)CommonConstant.ImageSizeType.thumb], ""); // remove _thumb (for view edit form)
            
            // lang variables
            var name_tc = collection.GetFormValue(PayloadKeys.varWithLang("name", "tc"));
            var description_tc = collection.GetFormValue(PayloadKeys.varWithLang("description", "tc"));
            var operation_info_tc = collection.GetFormValue(PayloadKeys.varWithLang("operation_info", "tc"));
            var address_unit_tc = collection.GetFormValue(PayloadKeys.varWithLang("address_unit", "tc"));
            var address_building_tc = collection.GetFormValue(PayloadKeys.varWithLang("address_building", "tc"));
            var address_street_tc = collection.GetFormValue(PayloadKeys.varWithLang("address_street", "tc"));

            var name_en = collection.GetFormValue(PayloadKeys.varWithLang("name", "en"));
            var description_en = collection.GetFormValue(PayloadKeys.varWithLang("description", "en"));
            var operation_info_en = collection.GetFormValue(PayloadKeys.varWithLang("operation_info", "en"));
            var address_unit_en = collection.GetFormValue(PayloadKeys.varWithLang("address_unit", "en"));
            var address_building_en = collection.GetFormValue(PayloadKeys.varWithLang("address_building", "en"));
            var address_street_en = collection.GetFormValue(PayloadKeys.varWithLang("address_street", "en"));

            var name_sc = collection.GetFormValue(PayloadKeys.varWithLang("name", "sc"));
            var description_sc = collection.GetFormValue(PayloadKeys.varWithLang("description", "sc"));
            var operation_info_sc = collection.GetFormValue(PayloadKeys.varWithLang("operation_info", "sc"));
            var address_unit_sc = collection.GetFormValue(PayloadKeys.varWithLang("address_unit", "sc"));
            var address_building_sc = collection.GetFormValue(PayloadKeys.varWithLang("address_building", "sc"));
            var address_street_sc = collection.GetFormValue(PayloadKeys.varWithLang("address_street", "sc"));

            var sql_remark = "";
            var new_location_id = 0;
            var type = (int)CommonConstant.LocationType.giftShop;

            if (location_id == 0)
            {
                var addFlag = _locationManager.Create(
                    SessionManager.Current.obj_id,

                    location_no,
                    type, // type
                    photo_file_name,
                    photo_file_extension,
                    latitude,
                    longitude,
                    phone,
                    fax,
                    address_district,
                    address_region,
                    display_order,
                    status,
                   
                    ref new_location_id,
                    ref sql_remark);

                // Create Lang TC
                _locationLangManager.Create(SessionManager.Current.obj_id, 
                                            new_location_id, 
                                            (int)Common.CommonConstant.LangCode.tc,
                                            name_tc,
                                            description_tc,
                                            operation_info_tc,
                                            address_unit_tc,
                                            address_building_tc,
                                            address_street_tc,
                                            1, //status_tc,

                                            ref sql_remark
                                            );

                // Create Lang EN
                _locationLangManager.Create(SessionManager.Current.obj_id,
                                            new_location_id,
                                            (int)Common.CommonConstant.LangCode.en,
                                            name_en,
                                            description_en,
                                            operation_info_en,
                                            address_unit_en,
                                            address_building_en,
                                            address_street_en,
                                            1, //status_en,

                                            ref sql_remark
                                            );

                // Create Lang SC
                _locationLangManager.Create(SessionManager.Current.obj_id,
                                            new_location_id,
                                            (int)Common.CommonConstant.LangCode.sc,
                                            name_sc,
                                            description_sc,
                                            operation_info_sc,
                                            address_unit_sc,
                                            address_building_sc,
                                            address_street_sc,
                                            1, //status_sc,

                                            ref sql_remark
                                            );

                return addFlag ? "{success:true,url:'',msg:'Add Success'}" : "{success:true,url:'',msg:'Add Failed: " + sql_remark + "'}";
            }
            else if (location_id > 0)
            {
                var updateFlag = _locationManager.Update(
                    SessionManager.Current.obj_id,

                    location_id,
                    location_no,
                    type, // type
                    photo_file_name,
                    photo_file_extension,
                    latitude,
                    longitude,
                    phone,
                    fax,
                    address_district,
                    address_region,
                    display_order,
                    status,
                   
                    ref sql_remark);

                // Update Lang TC
                _locationLangManager.Update(SessionManager.Current.obj_id,
                                            location_id,
                                            (int)Common.CommonConstant.LangCode.tc,
                                            name_tc,
                                            description_tc,
                                            operation_info_tc,
                                            address_unit_tc,
                                            address_building_tc,
                                            address_street_tc,
                                            1, //status_tc,

                                            ref sql_remark
                                            );

                // Update Lang EN
                _locationLangManager.Update(SessionManager.Current.obj_id,
                                            location_id,
                                            (int)Common.CommonConstant.LangCode.en,
                                            name_en,
                                            description_en,
                                            operation_info_en,
                                            address_unit_en,
                                            address_building_en,
                                            address_street_en,
                                            1, //status_en,

                                            ref sql_remark
                                            );

                // Update Lang SC
                _locationLangManager.Update(SessionManager.Current.obj_id,
                                            location_id,
                                            (int)Common.CommonConstant.LangCode.sc,
                                            name_sc,
                                            description_sc,
                                            operation_info_sc,
                                            address_unit_sc,
                                            address_building_sc,
                                            address_street_sc,
                                            1, //status_sc,

                                            ref sql_remark
                                            );

                return updateFlag ? "{success:true,url:'',msg:''}" : "{success:true,url:'',msg:'Saved Failed: " + sql_remark + "'}";

               // var result = true;
               //var url = "";
               //var msg = "<a href='http://hk.yahoo.com' target='_blank'>Download Excel</a>";
               // return new { success = result, url = url, msg = msg }.ToJson();
            }
            else
                return "{success:true,url:'',msg:'Saved Failed: Invalid Data'}";
        }

        public string Upload()
        {
            object result;
            var file = Request.Files["fileData"];
            var _module = "LocationTest";
            var fileHandler = new FileHandler();

            if (file == null)
                System.Diagnostics.Debug.WriteLine("fileData = null");
            else
                System.Diagnostics.Debug.WriteLine("fileData != null");

            var fileType = "Image";
            string fileName;
            var message = "";
            long fileSize = 0;
            var saveFlag = fileHandler.SaveFile(file, fileType, _module, out fileName, ref fileSize, ref message);
            var url = PathHandler.GetStorage_relativePath(_module, fileName);

            if (saveFlag)
            {
                result = new { success = true, message = message,url = url, fileName = fileName, fileSize = fileSize.ToString(), imageSrc = "http://localhost:65394/Storage/Photo/Gift/20140416161307_5327_thumb.jpg", imageId = 0, file = "http://localhost:65394/Storage/Photo/Gift/20140416161307_5327_thumb.jpg" };
            }
            else
            {
                result = new { success = false, message = message };
            }

            return result.ToJson();
        }
    }
}
