using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


using Palmary.Loyalty.Common;
using System.Web.Routing;
using Palmary.Loyalty.BO.Modules;
using Palmary.Loyalty.Web_backend.Modules.GlobalHandlers;
using Palmary.Loyalty.Web_backend.Modules.Utility;
using Palmary.Loyalty.Web_backend.ObjectModels;
using System.Configuration;
using Palmary.Loyalty.Web_backend;
using Palmary.Loyalty.BO.Modules.Gift;
using Palmary.Loyalty.BO.Modules.Administration.Security;
using Palmary.Loyalty.BO.DataTransferObjects.Media;
using Palmary.Loyalty.BO.DataTransferObjects;
using Palmary.Loyalty.BO.DataTransferObjects.Security;

namespace Palmary.Loyalty.Web_backend.Controllers.Gift
{
    [Authorize]
    public class GiftPhotoController : Controller
    {
        private GiftPhotoManager _giftPhotoManager;
        private int _gift_id;
        private string _module = CommonConstant.Module.gift;

        public GiftPhotoController()
        {
            _giftPhotoManager = new GiftPhotoManager();
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
            
        public string GetEditForm()
        {
            var sql_result = false;
            var giftPhoto = _giftPhotoManager.GetGiftPhotoListBy(SessionManager.Current.obj_id, _gift_id, ref sql_result);

            var images =
                giftPhoto.Select(
                    thePhoto =>
                    new
                    {
                        src =
                            Url.Content(string.Format("/{0}/{1}/{2}/{3}",
                                                      ConfigurationManager.AppSettings["storageFolder"],
                                                      ConfigurationManager.AppSettings["photoFolder"],
                                                      _module,
                                                      thePhoto.file_name + thePhoto.file_extension)),
                        id = thePhoto.gift_photo_id
                    });

            var result = new
            {
                row = new[]
                        {
                            new
                                {
                                    type = "multiUploadDialog",
                                    colspan = 2,
                                    name = "images",
                                    images = images,
                                    uploadUrl = Url.Action("Upload", new {id = _gift_id, ASPSESSID=Session.SessionID}),
                                    editUrl = Url.Action("SaveEdit", new {id = _gift_id}),
                                    editDataUrl = Url.Action("EditData", new {id = _gift_id}),
                                    removeUrl = Url.Action("Remove", new {id = _gift_id}),
                                }
                        },
                column = 2,
                post_url = Url.Action("ListData", "Common"),
                post_header = Url.Action("PostHeader"),
                title = "Gift Photo",
                icon = "",
                post_params = Url.Action("Reorder"),
                button_text = "Save",
                button_icon = "iconSave",
                value_changes = true
            };

            return result.ToJson();
        }

        public string Upload()
        {
            var file = Request.Files["fileData"];
         
            var message = "";
   
            var photoHelper = new PhotoHelper(_module);
            var photo = new PhotoObject();
            var saveFlag = photoHelper.SaveAndGenerateAllSizePhoto(file, out photo);

            string fileName_withPath = photo.photo_full_path_list[(int)CommonConstant.ImageSizeType.thumb];

            object result;
            if (saveFlag)
            {
                result = new { success = true, message = message, file = fileName_withPath, imageSrc = photo.photo_full_path_list[(int)CommonConstant.ImageSizeType.thumb], imageID = 0, url = "", fileName = fileName_withPath };
            }
            else
            {
                result = new { success = false, message = message };
            }

            //string fileName_withPath;
            //var message = "";
            //int new_gift_photo_id;
            //var saveFlag = AddPhoto(file, out new_gift_photo_id, out fileName_withPath, ref message);
            //var result = new { success = true, imageSrc = Url.Content(fileName_withPath), imageId = new_gift_photo_id };
            return result.ToJson();
        }

        public string Reorder()
        {
            var result = new { success = true, message = "" };
            return result.ToJson();
        }

        public bool AddPhoto(HttpPostedFileBase file, out int new_photo_id, out string fileName_withPath, ref string message)
        {
            string photoFileName;
            string thumFileName;
            var fileExtension = "";
            var result = SavePhoto(file, out photoFileName, out fileExtension, out thumFileName);
            var fileFullName = photoFileName + fileExtension;
            new_photo_id = 0;
            fileName_withPath = "";

            if (result)
            {
                fileName_withPath = string.Format("/{0}/{1}/{2}/{3}", ConfigurationManager.AppSettings["storageFolder"], ConfigurationManager.AppSettings["photoFolder"], _module, fileFullName);

                var sql_remark = "";
                result = _giftPhotoManager.Create(
                      SessionManager.Current.obj_id,
                      _gift_id,
                      photoFileName,
                      fileExtension,
                      "name",
                      "caption",
                      0, // display_order
                      0, // status

                      ref sql_remark
                    );

                new_photo_id = 1111; // (int)_giftPhotoId.Value;
            }
            
            return result; // (int)status.Value == 1;
        }

        private bool SavePhoto(HttpPostedFileBase file, out string photoFileName, out string fileExtension, out string thumb_fullName)
        {
            photoFileName = "";
            fileExtension = "";
            thumb_fullName = "";
            //var photo = new Photo();

            //var memssage = "";
            //var photoFlag = photo.SaveFile(file, _module, out photoFileName, out fileExtension, ref memssage);
            //var fullName = photoFileName + fileExtension;

            //var fileName_withPath = string.Format("{0}{1}/{2}/{3}/{4}", this.Request.PhysicalApplicationPath, ConfigurationManager.AppSettings["storageFolder"], ConfigurationManager.AppSettings["photoFolder"], _module, fullName);
            //thumb_fullName = string.Format("{0}_thumb.jpg", photoFileName);
            
            //var thumbnailImage = new ThumbnailImage();
            //var thumFlag = thumbnailImage.GetThumbnail(fileName_withPath, string.Format("{0}/{1}", _module, thumb_fullName));

            return false;// thumFlag && photoFlag;
        }

        // always return true, and delete in Gift Controller after user clicking Submit button
        public string Delete(FormCollection collection)
        {
            //var photoIDs = collection["ids"];

            //if (!string.IsNullOrEmpty(photoIDs))
            //{
            //    var idList = photoIDs.Split(',');
            //    foreach (var theID in idList)
            //    {
            //        // do delete photo
            //    }
            //}

            return new { result = true }.ToJson();
        }
    }
}