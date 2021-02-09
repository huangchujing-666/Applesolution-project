using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Web.Routing;
using Palmary.Loyalty.BO.Modules;
using Palmary.Loyalty.BO.DataTransferObjects.Media;
using Palmary.Loyalty.Web_backend.Modules.GlobalHandlers;
using Palmary.Loyalty.Web_backend.Modules.Utility;
using Palmary.Loyalty.Web_backend.ObjectModels;
using System.Configuration;
using Palmary.Loyalty.Web_backend;
using Palmary.Loyalty.BO.Modules.Location;
using Palmary.Loyalty.Common;
using Palmary.Loyalty.BO.DataTransferObjects.Security;

namespace Palmary.Loyalty.Web_backend.Controllers.Location
{
    [Authorize]
    public class LocationPhotoController : Controller
    {
        private string _module = CommonConstant.Module.location;

        public LocationPhotoController()
        {
        }
              
        public string Upload()
        {
            var file = Request.Files["fileData"];
                       
            var message = "";
            int new_photo_id;
            PhotoObject photo;
            var saveFlag = AddPhoto(file, out new_photo_id, out photo, ref message);
            string fileName_withPath = photo.photo_full_path_list[(int)CommonConstant.ImageSizeType.thumb];

            object result;
            if (saveFlag)
            {
                result = new { success = true, message = message, file = photo.photo_full_path_list[(int)CommonConstant.ImageSizeType.thumb], url = "", fileName = fileName_withPath };
            }
            else
            {
                result = new { success = false, message = message };
            }

            return result.ToJson();
        }

        public bool AddPhoto(HttpPostedFileBase file, out int new_photo_id, out PhotoObject photo, ref string message )
        {
            string photoFileName;
            string thumFileName;
            var fileExtension = "";

            var photoHelper = new PhotoHelper(_module);
            photo = new PhotoObject();
            var flag = photoHelper.SaveAndGenerateAllSizePhoto(file, out photo);

            new_photo_id = 1111;

            //var result = SavePhoto(file, out photoFileName, out fileExtension, out thumFileName);
            //var fileFullName = photoFileName + fileExtension;
            //new_photo_id = 0;
            //fileName_withPath = "";

            //if (result)
            //{
            //    fileName_withPath = string.Format("/{0}/{1}/{2}/{3}", ConfigurationManager.AppSettings["storageFolder"], ConfigurationManager.AppSettings["photoFolder"], _module, fileFullName);

            //    new_photo_id = 1111; // (int)_giftPhotoId.Value;
            //}

            return flag; // (int)status.Value == 1;
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
            //System.Diagnostics.Debug.WriteLine(photoFileName, "photoFileName");
            //System.Diagnostics.Debug.WriteLine(fileExtension, "fileExtension");
            //var fileName_withPath = string.Format("{0}{1}/{2}/{3}/{4}", this.Request.PhysicalApplicationPath, ConfigurationManager.AppSettings["storageFolder"], ConfigurationManager.AppSettings["photoFolder"], _module, fullName);

            //thumb_fullName = string.Format("{0}_thumb.jpg", photoFileName);

            //var thumbnailImage = new ThumbnailImage();
            //var thumFlag = thumbnailImage.GetThumbnail(fileName_withPath, string.Format("{0}/{1}", _module, thumb_fullName));

            return false;// thumFlag && photoFlag;
        }
    }
}
