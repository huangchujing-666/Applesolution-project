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

using Palmary.Loyalty.BO.Modules.Member;
using Palmary.Loyalty.BO.Modules.Utility;

namespace Palmary.Loyalty.Web_backend.Controllers.Member
{
    [Authorize]
    public class MemberImportController : Controller
    {
        private string _module = CommonConstant.Module.memberImport;
        private int _id;

        public MemberImportController()
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
            var template_label = "";
            var template_filename = "";
            var button_name = "";

            if (_id == 0 || _id == (int)CommonConstant.MemberImportType.insert)
            {
                template_label = "Insert Template";
                template_filename = "member_import_template.xlsx";
                button_name = "Import";

                _id = (int)CommonConstant.MemberImportType.insert;
            }
            else if (_id == (int)CommonConstant.MemberImportType.update)
            {
                template_label = "Update Template";
                template_filename = "member_update_template.xlsx";
                button_name = "Update";
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
                button_text = button_name,
                button_icon = "iconSave",
                value_changes = true,
            };

            var templateURL = PathHandler.GetStorage_relativePath(_module, template_filename);
            var rowFieldInput = new ExtJsFieldLabelInput<string>(PayloadKeys.MemberImport.template, "")
            {
                fieldLabel = template_label,
                colspan = 2,
                value = "<a href='" + templateURL + "' target='_blank'>Download</a>",
                readOnly = true
            };
            extTable.AddFieldLabelToRow(rowFieldInput);

            var rowFieldUpload = new ExtJsFieldLabelUpload<string>(PayloadKeys.MemberImport.fileData, "")
            {
                fieldLabel = "Upload",
                upType = "file",
                upload_url = "../MemberImport/Upload",
                fileName = "",
                colspan = 2,
                allowBlank = false
            };
            extTable.AddFieldLabelToRow(rowFieldUpload);

            var hiddenLabel = new ExtJsFieldLabelHidden<int>(PayloadKeys.MemberImport.importType, _id.ToString());
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
            var importType = collection.GetFormValue(PayloadKeys.MemberImport.importType);
            var fileName = collection.GetFormValue(PayloadKeys.MemberImport.fileData);
       
            // Read Excel
            var fileHandler = new FileHandler();
            var filePath = fileHandler.GetPhysicalPath(fileName, _module);

            var importManager = new MemberImportManager();
            var result = importManager.Import(filePath, importType);
            var resultMsg = "";
            
            if (result ==  CommonConstant.SystemCode.normal)
                resultMsg = new { success = true, msg = "Import Success" }.ToJson();
            else
                resultMsg = new { success = false, msg = "Import Failed", closeWhenFail = true}.ToJson();
            
            return resultMsg;
        }
    }
}