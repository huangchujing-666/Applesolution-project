using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Web.Routing;
using Palmary.Loyalty.BO.DataTransferObjects.Security;
using Palmary.Loyalty.BO.DataTransferObjects.Media;
using Palmary.Loyalty.BO.Modules.Product;
using Palmary.Loyalty.Common;

using Palmary.Loyalty.Web_backend.Modules.GlobalHandlers;
using Palmary.Loyalty.Web_backend.Modules.Utility;
using Palmary.Loyalty.Web_backend.ObjectModels;
using System.Configuration;
using Palmary.Loyalty.Web_backend;

namespace Palmary.Loyalty.Web_backend.Controllers
{
    [Authorize]
    public class ProductPhotoController : Controller
    {
        //
        // GET: /ProductPhoto/
        private ProductManager _productManager;
        private ProductPhotoManager _productPhotoManager;
        private int _product_id;
        private string _module = CommonConstant.Module.product;

        public ProductPhotoController()
        {
            _productManager = new ProductManager();
            _productPhotoManager = new ProductPhotoManager();
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

        public string GetEditForm()
        {
            var systemCode = CommonConstant.SystemCode.undefine;
            var productPhoto = _productPhotoManager.GetList(_product_id, ref systemCode);
           
            var images =
                productPhoto.Select(
                    theProductPhoto =>
                    new
                    {
                        src =
                    Url.Content(string.Format("/{0}/{1}/{2}/{3}",
                                              ConfigurationManager.AppSettings["storageFolder"],
                                              ConfigurationManager.AppSettings["photoFolder"],
                                              _module,
                                              theProductPhoto.file_name)),
                        id = theProductPhoto.photo_id
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
                        uploadUrl = Url.Action("Upload", new {id = _product_id, ASPSESSID=Session.SessionID}),
                        editUrl = Url.Action("SaveEdit", new {id = _product_id}),
                        editDataUrl = Url.Action("EditData", new {id = _product_id}),
                        removeUrl = Url.Action("Remove", new {id = _product_id}),
                    }
                },
                column = 2,
                post_url = Url.Action("ListData", "Common"),
                post_header = Url.Action("PostHeader"),
                title = "Product Photo",
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
            if (file == null)
                System.Diagnostics.Debug.WriteLine("fileData = null");
            else
                System.Diagnostics.Debug.WriteLine("fileData != null");

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
            return result.ToJson();
        }

        public string Reorder()
        {
            var result = new { success = true, message = ""};
            return result.ToJson();
        }

        public bool AddPhoto(HttpPostedFileBase file, int userId, out int new_product_photo_id, out string fileName_withPath, ref string message)
        {
            //var ip = CommonService.GetIPAddress(HttpContext.Current.Request);
            //var remark = new ObjectParameter("remark", typeof(string));
            //var status = new ObjectParameter("status", typeof(int));
            //var _giftPhotoId = new ObjectParameter("gift_photo_id", typeof(int));
            string photoFileName;
            string thumFileName;
            string fileExtension;
            fileName_withPath = "";

            var photoHelper = new PhotoHelper(_module);
            var photo = new PhotoObject();
            var flag = photoHelper.SaveAndGenerateAllSizePhoto(file, out photo);
            System.Diagnostics.Debug.WriteLine(photo.photo_full_path_list[0], "photo.photo_full_path_list[0]");
            fileName_withPath = photo.photo_full_path_list[0];
            //var data =
            //    new
            //    {
            //        name = "name",
            //        giftId = _product_id,
            //        photo = photoFileName,
            //        caption = "caption",
            //        thumbnail = thumFileName,
            //        width = 0,
            //        height = 0,
            //        thumbnail_width = 0,
            //        thumbnail_height = 0,
            //        displayOrder = 1,
            //        status = 1
            //    }.ToJson();

            //fileName_withPath = string.Format("/{0}/{1}/{2}/{3}", ConfigurationManager.AppSettings["storageFolder"], ConfigurationManager.AppSettings["photoFolder"], _module, photoFileName);

            //var result = _productManager.CreateProductPhoto(
            //      userId,
            //      _product_id,
            //      "name",
            //      photoFileName,
            //      "caption",
            //      thumFileName,
            //      0, //width,
            //      0, //height,
            //      0, //thumbnail_width,
            //      0, //thumbnail_height,
            //      0 //display_order
            //    );

            new_product_photo_id = 1111; // (int)_giftPhotoId.Value;
            return false;//  result; // (int)status.Value == 1;
        }

        //private bool SavePhoto(HttpPostedFileBase file, out string photoFileName, out string fileExtension, out string thumb_fullName)
        //{
        //    PhotoHelper photoHelper = new PhotoHelper(_module);

        //    thumb_fullName = "";
        //    var memssage = "";

        //    var photoFlag = photoHelper.SavePhoto(file, out photoFileName, out fileExtension, out memssage);
        //    var fullName = photoFileName + fileExtension;
        //    var fileName_withPath = string.Format("{0}{1}/{2}/{3}/{4}", this.Request.PhysicalApplicationPath, ConfigurationManager.AppSettings["storageFolder"], ConfigurationManager.AppSettings["photoFolder"], _module, fullName);

        //    thumb_fullName = string.Format("{0}_thumb{1}", photoFileName, fileExtension);

        //    var thumbnailImage = new ThumbnailImage();
        //    var thumFlag = thumbnailImage.GetThumbnail(fileName_withPath, string.Format("{0}/{1}", _module, thumb_fullName));

        //    return thumFlag && photoFlag;
        //}

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
