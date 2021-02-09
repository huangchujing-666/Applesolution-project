using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


using Palmary.Loyalty.Common;
using Palmary.Loyalty.BO.DataTransferObjects.Media;
using Palmary.Loyalty.BO.Modules.Utility;
using Palmary.Loyalty.Web_backend.ObjectModels;
using Palmary.Loyalty.Web_backend.Modules.Utility;
using Palmary.Loyalty.Web_backend.Modules.GlobalHandlers;

using System.Configuration;
using System.Collections; //for Hashtable
using System.IO;
using System.Globalization; // for DateTimeFormatInfo
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Palmary.Loyalty.Web_backend.Modules.Utility
{
    public class PhotoHelper
    {
        public string _module;
        
        public PhotoHelper(string module)
        {
            _module = module;
        }

        public bool SaveAndGenerateAllSizePhoto(HttpPostedFileBase file, out PhotoObject photo)
        {
            photo = new PhotoObject();

            var memssage = "";

            var fileName = DateTime.Now.ToString("yyyyMMddHHmmss_ffff", DateTimeFormatInfo.InvariantInfo);
            var file_extension = Path.GetExtension(file.FileName).ToLower();

            var photoFlag = true;

            // genereate and save all images
            for (int i = 0; i < CommonConstant.ImageSizeName_postfix.Count(); i++)
            {
                var saveFalg = SaveFile(file, fileName, file_extension, i, ref memssage);

                if (saveFalg)
                {
                    // record all Path
                    var file_full_name = fileName + Common.CommonConstant.ImageSizeName_postfix[i] + file_extension;
                    var thePath = PathHandler.GetStoragePhoto_relativePath(_module, file_full_name);
                    photo.photo_full_path_list.Add(i, thePath);
                }
                else
                    photoFlag = false;
            }
            return photoFlag;
        }

        public bool SaveFile(HttpPostedFileBase file, string file_full_name, string file_extension, int image_size, ref string message)
        {
            var extTable = new Hashtable();
            extTable.Add("image", "gif,jpg,jpeg,png,bmp");
            const string dirName = "image";
            const int maxSize = 10000000;

            if (file == null)
            {
                message = "file can't empty!";
                return false;
            }
            var dirPath = string.Format("{0}/{1}/", PathHandler.GetStoragePhotoFolder_serverPath(), _module);
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
            var fileName = file.FileName;
            if (file.InputStream == null || file.InputStream.Length > maxSize)
            {
                message = "File size too large";
                return false;
            }

            if (String.IsNullOrEmpty(file_extension) || Array.IndexOf(((String)extTable[dirName]).Split(','), file_extension.Substring(1).ToLower()) == -1)
            {
                message = string.Format("The type of image is error。\nonly {0} type!", extTable[dirName]);
                return false;
            }

            var original_full_path = PathHandler.GetStoragePhoto_serverFullPath(_module, file_full_name, file_extension, (int)CommonConstant.ImageSizeType.orginial);
          
            if (image_size == (int)CommonConstant.ImageSizeType.orginial)
            {
                file.SaveAs(original_full_path);
            }
            else
            {
                var changeToSize = new Size();

                switch (image_size)
                {
                    case (int)CommonConstant.ImageSizeType.large:
                        changeToSize.Width = CommonConstant.ImageSize.Large.width;
                        changeToSize.Height = CommonConstant.ImageSize.Large.height;
                        break;
                    case (int)CommonConstant.ImageSizeType.middle:
                        changeToSize.Width = CommonConstant.ImageSize.Middle.width;
                        changeToSize.Height = CommonConstant.ImageSize.Middle.height;
                        break;
                    case (int)CommonConstant.ImageSizeType.thumb:
                        changeToSize.Width = CommonConstant.ImageSize.Thumb.width;
                        changeToSize.Height = CommonConstant.ImageSize.Thumb.height;
                        break;
                    default:
                        break;
                }

                System.Drawing.Imaging.ImageFormat imageType;
                switch (file_extension)
                {
                    case ".gif":
                        imageType = System.Drawing.Imaging.ImageFormat.Gif;
                        break;
                    case ".png":
                        imageType = System.Drawing.Imaging.ImageFormat.Png;
                        break;
                    case ".bmp":
                        imageType = System.Drawing.Imaging.ImageFormat.Bmp;
                        break;
                    default:
                        imageType = System.Drawing.Imaging.ImageFormat.Jpeg;
                        break;
                }

                Image theImage = Image.FromFile(original_full_path);
                theImage = ResizeImage(theImage, new Size(changeToSize.Width, changeToSize.Height));
                theImage.Save(PathHandler.GetStoragePhoto_serverFullPath(_module, file_full_name, file_extension, image_size), imageType);
            }
            return true;
        }

        private static Image ResizeImage(Image imgToResize, Size size)
        {
            int sourceWidth = imgToResize.Width;
            int sourceHeight = imgToResize.Height;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)size.Width / (float)sourceWidth);
            nPercentH = ((float)size.Height / (float)sourceHeight);

            if (nPercentH < nPercentW)
                nPercent = nPercentH;
            else
                nPercent = nPercentW;

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage((Image)b);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();

            return (Image)b;
        }
    }
}