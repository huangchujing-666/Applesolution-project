using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Palmary.Loyalty.BO.DataTransferObjects.Reminder;
using Palmary.Loyalty.Web_backend.Modules.ReminderEngine;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.PayoadKey;
using Palmary.Loyalty.Web_backend.Modules.Utility;
using Palmary.Loyalty.Common;
using System.Text.RegularExpressions;
using Palmary.Loyalty.BO.Modules.Reminder;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.ExtJsEntity;

namespace Palmary.Loyalty.Web_backend.Controllers.Reminder
{
    [Authorize]
    public class ReminderTemplateController : Controller
    {
        private int _id;

        public ReminderTemplateController()
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
        }

        public string InsertForm()
        {
            var reminderTemplateHandler = new ReminderTemplateHandler();

            var jsonStr = reminderTemplateHandler.GetFormByModule(new ReminderTemplateObject());
            return jsonStr;
        }

        // Edit View Form
        public string EditViewForm()
        {
            var systemCode = CommonConstant.SystemCode.undefine;
            var reminderTemplateManager = new ReminderTemplateManager();
            var handler = new ReminderTemplateHandler();
            var obj = reminderTemplateManager.GetDetail(_id, ref systemCode);

            var formTableJSON = handler.GetFormByModule(obj);
            return formTableJSON;
        }

        public string EditView_ToolbarData()
        {
            var toolData = new List<ExtJsButton>();

            toolData.Add(new ExtJsButton("button", "view") { text = "View", hidden = true, iconUrl = "iconView" });
            toolData.Add(new ExtJsButton("button", "edit") { text = "Edit", iconUrl = "iconRole16" });

            var result = new { toolData = toolData }.ToJson();

            return result;
        }

        // Create or Update
        [ValidateInput(false)]
        public string Update(FormCollection collection)
        {
            var reminder_template_id = collection.GetFormValue(PayloadKeys.ReminderTemplate.reminder_template_id);
            var name = collection.GetFormValue(PayloadKeys.ReminderTemplate.name);
            var sms_template = collection.GetFormValue(PayloadKeys.ReminderTemplate.sms_template);
            var email_template = collection.GetFormValue(PayloadKeys.ReminderTemplate.email_template);
            var status = collection.GetFormValue(PayloadKeys.ReminderTemplate.status);
    
            // result for ExtJS
            var result = false;
            var msg = "";
            var msg_prefix = reminder_template_id == 0 ? "Create" : "Update";
            var msg_prefix_result = "";
            var msg_content = "";
            var url = "";

            var systemCode = CommonConstant.SystemCode.undefine;
            
            var obj = new ReminderTemplateObject()
            {
                reminder_template_id = reminder_template_id,
                name = name,
                sms_template = sms_template,
                email_template = email_template,
                status = status
            };

            // Regex check
            var regNormalText = new Regex(CommonConstant.RegexStr.normalText);

            // check data
            if (name.Length > 50)
            {
                result = false;
                msg_content = "Name cannot more than 50 characters";
            }
            else if (!regNormalText.IsMatch(name))
            {
                result = false;
                msg_content = "Name is not in correct format";
            }
            else
            {
                var reminderTemplateManager = new ReminderTemplateManager();

                // Add new object
                if (obj.reminder_template_id == 0)
                {
                    systemCode = reminderTemplateManager.Create(obj);
                }
                else if (obj.reminder_template_id > 0)
                {
                    // update object
                    systemCode = reminderTemplateManager.Update(obj);
                }

                // common result check
                if (systemCode == CommonConstant.SystemCode.normal)
                    result = true;
                else if (systemCode == CommonConstant.SystemCode.no_permission)
                    msg_content = "No Permission";
            }

            // output json
            if (result)
                msg_prefix_result = "Success";
            else
                msg_prefix_result = "Fail";

            if (!String.IsNullOrEmpty(msg_content))
                msg = msg_prefix + " " + msg_prefix_result + ": <br/>" + msg_content;
            else
                msg = msg_prefix + " " + msg_prefix_result;

            return new { success = result, url = url, msg = msg }.ToJson();
        }
    }
}
