using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Palmary.Loyalty.BO.Modules;
using Palmary.Loyalty.BO.Modules.Passcode;
using Palmary.Loyalty.BO.Modules.Administration.Security;
using Palmary.Loyalty.BO.Database;
using Palmary.Loyalty.Web_backend.Modules;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.ExtJsEntity;
using Palmary.Loyalty.Web_backend.Modules.GlobalHandlers;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.PayoadKey;
using Palmary.Loyalty.Web_backend.Modules.Utility;
using System.Web.Routing;
using System.Configuration;
using System.IO;
using System.Data.OleDb;
using System.Data;
using Palmary.Loyalty.BO.DataTransferObjects.Security;

using Palmary.Loyalty.Common;

namespace Palmary.Loyalty.Web_backend.Controllers
{
    [Authorize]
    public class PasscodeExcelController : Controller
    {
        public static string module = "PasscodeExcel";
        public FileHandler _fileHandler = new FileHandler();

        public PasscodeExcelController()
        {
        }

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            SystemHandler.InitSystemParameters(this.HttpContext.ApplicationInstance as MvcApplication);
        }

        // Upload Excel File
        public string Upload()
        {
            object result;
            var file = Request.Files["fileData"];

            if (file == null)
                System.Diagnostics.Debug.WriteLine("fileData = null");
            else
                System.Diagnostics.Debug.WriteLine("fileData != null");

            var fileType = "Excel";
            string fileName;
            var message = "";
            long fileSize = 0;
            var uploadFlag = _fileHandler.SaveFile(file, fileType, module, out fileName, ref fileSize, ref message);
            var url = string.Format("/{0}/{1}/{2}", ConfigurationManager.AppSettings["storageFolder"], module, fileName);

            if (uploadFlag)
            {
                var result_message = "";
                var importResult = ReadExcel(fileName, ref result_message);
                if (importResult)
                    result = new { success = true, message = message, file = fileName, url = url };
                else
                    result = new { success = false, message = "Import Error: " + result_message };
            }
            else
                result = new { success = false, message = message };

            return result.ToJson();
        }

        public bool ReadExcel(string fileName, ref string result_message)
        {
            string filePath = _fileHandler.GetPhysicalPath(fileName, module);
            var connectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0; data source={0}; Extended Properties=Excel 8.0;", filePath);
            var adapter = new OleDbDataAdapter("SELECT * FROM [Sheet1$]", connectionString);
            var excelDataSet = new DataSet();

            adapter.Fill(excelDataSet, "DataList");
            PasscodeImportManager _passcodeImportManager = new PasscodeImportManager();
            
            var result = _passcodeImportManager.Import(SessionManager.Current.obj_id, excelDataSet, ref result_message);
            
            return result;
        }
    }
}