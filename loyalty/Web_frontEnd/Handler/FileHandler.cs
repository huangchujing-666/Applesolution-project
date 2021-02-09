using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Configuration;
using System.Collections; //for Hashtable
using System.IO;
using System.Globalization; // for DateTimeFormatInfo
using Palmary.Loyalty.Common;

namespace Palmary.Loyalty.Web_frontend.Handler
{
    public class FileHandler
    {
        
        private readonly string _savePath = string.Format("{0}{1}", HttpContext.Current.Request.PhysicalApplicationPath, ConfigurationManager.AppSettings["storageFolder"]);

        public bool SaveFile(HttpPostedFileBase file, string fileType, string modulePath, out string outFileName, ref long fileSize, ref string message)
        {
            var extTable = new Hashtable();
            extTable.Add("Excel", "xls,xlsx");
            extTable.Add("Image", "gif,jpg,jpeg,png,bmp");

            const int maxSize = 10485760; // unit in byte , 10485760 = 10MB
            fileSize = file.InputStream.Length;

            outFileName = "";
            if (file == null)
            {
                message = "Empty File.";
                return false;
            }
            if (file.InputStream == null || file.InputStream.Length > maxSize)
            {
                message = "File too large.";
                return false;
            }

            var dirPath = string.Format("{0}/{1}", _savePath, modulePath);
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
            var fileName = file.FileName;
            var fileExt = Path.GetExtension(fileName).ToLower();

            if (extTable[fileType] == null)
            {
                message = string.Format("System define wrong type of '{0}'.", fileType);
                return false;
            }

            if (String.IsNullOrEmpty(fileExt) || Array.IndexOf(((String)extTable[fileType]).Split(','), fileExt.Substring(1).ToLower()) == -1)
            {
                message = string.Format("Incorrect file type. Please upload: {0}", extTable[fileType]);
                return false;
            }
        
            var newFileName = DateTime.Now.ToString("yyyyMMddHHmmss_ffff", DateTimeFormatInfo.InvariantInfo) + fileExt;
            var filePath = string.Format("{0}/{1}", dirPath, newFileName);
            file.SaveAs(filePath);
            outFileName = newFileName;
            return true;
        }

        public string GetPhysicalPath(string fileName, string module)
        {
            var dirPath = string.Format("{0}/{1}", _savePath, module);
            var filePath = string.Format("{0}/{1}", dirPath, fileName);

            return filePath;
        }

        public string GetImagePath(string fileName, string fileExtension, string module, int size_type)
        {
            var fileFullName = fileName + CommonConstant.ImageSizeName_postfix[size_type] + fileExtension;

            var dirPath = string.Format("{0}/{1}", _savePath, module);
            var filePath = string.Format("{0}/{1}", dirPath, fileFullName);
            var fileName_withPath = string.Format("../{0}/{1}/{2}/{3}", ConfigurationManager.AppSettings["storageFolder"], ConfigurationManager.AppSettings["photoFolder"], module, fileFullName);

            return fileName_withPath;
        }

        public string GetImagePathFromBackend(string fileName, string fileExtension, string module, int size_type)
        {
            var fileFullName = fileName + CommonConstant.ImageSizeName_postfix[size_type] + fileExtension;

            var dirPath = string.Format("{0}/{1}", _savePath, module);
            var filePath = string.Format("{0}/{1}", dirPath, fileFullName);
            var fileName_withPath = string.Format("{0}/{1}/{2}/{3}/{4}", ConfigurationManager.AppSettings["backend"], ConfigurationManager.AppSettings["storageFolder"], ConfigurationManager.AppSettings["photoFolder"], module, fileFullName);

            return fileName_withPath;
        }
    }
}