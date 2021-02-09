using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Palmary.Loyalty.Web_backend;
using System.Web.Routing;
using Palmary.Loyalty.Common;
using Palmary.Loyalty.BO.DataTransferObjects.Security;

using Palmary.Loyalty.Web_backend.Modules.GlobalHandlers;
using Palmary.Loyalty.Web_backend.Modules.Utility;
using Palmary.Loyalty.Web_backend.Modules.ExtJS;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.PayoadKey;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.ExtJsEntity;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.TableData;
using System.Configuration;

using System.Data.OleDb;
using System.Data;

using Palmary.Loyalty.BO.Modules.Transaction;
using Palmary.Loyalty.BO.Modules.Utility;
using Palmary.Loyalty.Web_backend;

namespace Palmary.Loyalty.Web_backend.Controllers.Transaction
{
    [Authorize]
    public class TransactionImportController : Controller
    {
        private string _module = CommonConstant.Module.transactionImport;
        private int _id;

        public TransactionImportController()
        {
        }

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            var id = RouteData.Values["id"] ?? "";
            if (!string.IsNullOrEmpty(id.ToString()))
            {
                _id = int.Parse(id.ToString());
            }
            SystemHandler.InitSystemParameters(this.HttpContext.ApplicationInstance as MvcApplication);
        }

        public string GenerateForm()
        {
            var type_name = "";
            var template_filename = "";

            if (_id == (int)CommonConstant.TransactionType.postpaidservice)
            {
                type_name = "PostPaid Service";
                template_filename = "transaction_import_postpaid_template.xlsx";
            }
            else if (_id == (int)CommonConstant.TransactionType.purchase_product)
            {
                type_name = "Retail Purchase";
                template_filename = "transaction_import_productpurchase_template.xlsx";
            }
			else if (_id == (int)CommonConstant.TransactionType.point_adjustment)
            {
                type_name = "Point Adjustment";
                template_filename = "transaction_import_pointadjustment_template.xlsx";
            }
            //basic table data
            var extTable = new ExtJsTable
            {
                column = 2,
                post_url = "",
                post_header = "",
                title = "Upload Detail",
                icon = "iconRemarkList",
                post_params = Url.Action("Update"),
                isType = true,
                button_text = "Save",
                button_icon = "iconSave",
                value_changes = true,
            };

            var templateURL = PathHandler.GetStorage_relativePath(_module, template_filename);
            var rowFieldInput = new ExtJsFieldLabelInput<string>(PayloadKeys.TransactionImport.template, "")
            {
                fieldLabel = type_name + " Template",
                colspan = 2,
                value = "<a href='" + templateURL + "' target='_blank'>Download</a>",
                readOnly = true
            };
            extTable.AddFieldLabelToRow(rowFieldInput);

            var rowFieldUpload = new ExtJsFieldLabelUpload<string>(PayloadKeys.TransactionImport.fileData, "")
            {
                fieldLabel = "Upload",
                upType = "file",
                upload_url = "../TransactionImport/Upload",
                fileName = "",
                colspan = 2,
                allowBlank = false
            };
            extTable.AddFieldLabelToRow(rowFieldUpload);

            var hiddenLabel = new ExtJsFieldLabelHidden<int>(PayloadKeys.TransactionImport.importType, _id.ToString());
            extTable.AddFieldLabelToHiddenRow(hiddenLabel);

            return extTable.ToJson();
        }

        // Upload Excel File
        public string Upload()
        {
            object result;
            var file = Request.Files["fileData"];

            var fileType = "Excel";
            string fileName;
            var message = "";

            var fileHandler = new FileHandler();
            long fileSize = 0;
            var uploadFlag = fileHandler.SaveFile(file, fileType, _module, out fileName, ref fileSize, ref message);
            var url = string.Format("/{0}/{1}/{2}", ConfigurationManager.AppSettings["storageFolder"], _module, fileName);

            if (uploadFlag)
            {
                result = new { success = true, message = message, file = fileName, fileName = fileName, url = url, fileSize = fileSize.ToString() };
            }
            else
                result = new { success = false, message = "Import Error: " + message };

            return result.ToJson();
        }

        public string Update(FormCollection collection)
        {
            var importType = collection.GetFormValue(PayloadKeys.TransactionImport.importType);
            var fileName = collection.GetFormValue(PayloadKeys.TransactionImport.fileData);

            // Read Excel
            var fileHandler = new FileHandler();
            var filePath = fileHandler.GetPhysicalPath(fileName, _module);

            var importManager = new TransactionImportManager();
            var result = importManager.Import(importType, filePath);
            var resultMsg = "";

            if (result == CommonConstant.SystemCode.normal)
                resultMsg = new { success = true, msg = "Import Complete" }.ToJson();
            else
                resultMsg = new { success = false, msg = "Import Complete with error", closeWhenFail = true }.ToJson();

            return resultMsg;
        }
    }
}
