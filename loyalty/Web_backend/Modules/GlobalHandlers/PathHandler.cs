using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Configuration;
using System.IO;
using System.Web.Mvc;


namespace Palmary.Loyalty.Web_backend.Modules.GlobalHandlers
{
    public static class PathHandler
    {
        public static string GetControllerPath(string actionName, string controllerName)
        {
            var urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
            var url = urlHelper.Action(actionName, controllerName);

            return url;
        }

        public static string GetApplicationPhysicalPath()
        {
            var physicalPath = HttpContext.Current.Request.PhysicalApplicationPath; //System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
            return physicalPath;
        }

        public static string GetStorage_serverPath()
        {
            var storagePath = string.Format("{0}{1}", GetApplicationPhysicalPath(), ConfigurationManager.AppSettings["storageFolder"]);
            return storagePath;
        }

        public static string GetStorage_serverFullPath(string module)
        {
            var full_path = string.Format("{0}/{1}",
                                        GetStorage_serverPath(),
                                        module
                                        );
            return full_path;
        }

        public static string GetStorage_serverFullPath(string module, string file_full_name)
        {
            var full_path = string.Format("{0}/{1}/{2}",
                                        GetStorage_serverPath(),
                                        module,
                                        file_full_name);
            return full_path;
        }

        public static string GetStoragePhotoFolder_serverPath()
        {
            var storagePhotoPath = string.Format("{0}{1}/{2}", GetApplicationPhysicalPath(), ConfigurationManager.AppSettings["storageFolder"], ConfigurationManager.AppSettings["photoFolder"]);
            return storagePhotoPath;
        }

        public static string GetStoragePhoto_serverFullPath(string module, string file_full_name)
        {
            var full_path = string.Format("{0}/{1}/{2}", GetStoragePhotoFolder_serverPath(), module, file_full_name);
            return full_path;
        }

        public static string GetStoragePhoto_serverFullPath(string module, string file_name, string file_extension, int file_size)
        {
            var full_path = string.Format("{0}/{1}/{2}{3}{4}",
                                        GetStoragePhotoFolder_serverPath(),
                                        module,
                                        file_name,
                                        Common.CommonConstant.ImageSizeName_postfix[file_size],
                                        file_extension);
            return full_path;
        }

        public static string GetStoragePhotoFolder_relativePath()
        {
            var thePath = string.Format("{0}/{1}", ConfigurationManager.AppSettings["storageFolder"], ConfigurationManager.AppSettings["photoFolder"]);
            return thePath;
        }

        public static string GetStoragePhoto_relativePath(string module, string file_full_name)
        {
            var thePath = string.Format("../{0}/{1}/{2}/{3}", ConfigurationManager.AppSettings["storageFolder"], ConfigurationManager.AppSettings["photoFolder"], module, file_full_name);
            return thePath;
        }

        public static string GetStorage_relativePath(string module, string file_full_name)
        {
            var thePath = string.Format("../{0}/{1}/{2}", ConfigurationManager.AppSettings["storageFolder"], module, file_full_name);
            return thePath;
        }

       

        public static string GetStorage_relativePath_changeExt(string module, string file_full_name, string new_ext)
        {
            var file_name = Path.GetFileNameWithoutExtension(file_full_name);
            var new_file_full_name = file_name + new_ext;

            var thePath = string.Format("../{0}/{1}/{2}", ConfigurationManager.AppSettings["storageFolder"], module, new_file_full_name);

            return thePath;
        }
    }
}