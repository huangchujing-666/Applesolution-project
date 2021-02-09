using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Palmary.Loyalty.BO.DataTransferObjects.Reminder;
using Palmary.Loyalty.Web_backend.Modules.Reminder;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.ExtJsEntity;
using Palmary.Loyalty.Common;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.TableData;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.PayoadKey;
using Palmary.Loyalty.Web_backend.Modules.Utility;
using Palmary.Loyalty.BO.Modules.Reminder;
using System.Text.RegularExpressions;

namespace Palmary.Loyalty.Web_backend.Controllers.Reminder
{
    [Authorize]
    public class ReminderEngineController : Controller
    {
        private int _id;

        public ReminderEngineController()
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

        public string Insert()
        {
            var reminderEngineHandler = new ReminderEngineHandler();

            var jsonStr = reminderEngineHandler.GetFormByModule(new ReminderEngineObject());
            return jsonStr;
        }

        public string ToolbarData()
        {
            var toolData = new List<ExtJsButton>();
            toolData.Add(new ExtJsButton("button", "view") { text = "View", hidden = true, iconUrl = "iconView" });
            toolData.Add(new ExtJsButton("button", "edit") { text = "Edit", iconUrl = "iconRole16" });
            
            return new { toolData = toolData }.ToJson();
        }

        // Edit View Form
        public string EditViewForm()
        {
            var systemCode = CommonConstant.SystemCode.undefine;
            var reminderEngineManager = new ReminderEngineManager();
            var handler = new ReminderEngineHandler();
            var obj = reminderEngineManager.GetDetail(_id, ref systemCode);

            var formTableJSON = handler.GetFormByModule(obj);
            return formTableJSON;
        }

        public string EditView_ToolbarData()
        {
            var toolData = new List<ExtJsButton>();

            //toolData.Add(new ExtJsButton("button", "view") { text = "View", hidden = true, iconUrl = "iconView" });
           // toolData.Add(new ExtJsButton("button", "edit") { text = "Edit", iconUrl = "iconRole16" });

            var result = new { toolData = toolData }.ToJson();

            return result;
        }

        // Create or Update
        [ValidateInput(false)]
        public string Update(FormCollection collection)
        {
            // load form data
            var reminder_engine_id = collection.GetFormValue(PayloadKeys.ReminderEngine.reminder_engine_id);
            var name = collection.GetFormValue(PayloadKeys.ReminderEngine.name);
            var target_type = collection.GetFormValue(PayloadKeys.ReminderEngine.target_type);
            var status = collection.GetFormValue(PayloadKeys.ReminderEngine.status);

            var target_value = "";
            if (target_type == (int)CommonConstant.ReminderEngineType.ProductPurchase)
                target_value = collection.GetFormValue(PayloadKeys.ReminderEngine.target_value_purchase).ToString();
            else if (target_type == (int)CommonConstant.ReminderEngineType.GiftRedeem)
                target_value = collection.GetFormValue(PayloadKeys.ReminderEngine.target_value_redeem).ToString();
            else if (target_type == (int)CommonConstant.ReminderEngineType.LocationVisit)
                target_value = collection.GetFormValue(PayloadKeys.ReminderEngine.target_value_locationVisit).ToString();
            else if (target_type == (int)CommonConstant.ReminderEngineType.MemberInactive)
                target_value = collection.GetFormValue(PayloadKeys.ReminderEngine.target_value_memberInactive).ToString();

            var dayListStr = new List<int>();
            var templateTypeListStr = new List<int>();
            var templateIDListStr = new List<int>();

            dayListStr.Add(collection.GetFormValue(PayloadKeys.ReminderEngine.day));
            templateTypeListStr.Add(collection.GetFormValue(PayloadKeys.ReminderEngine.template_type));
            templateIDListStr.Add(collection.GetFormValue(PayloadKeys.ReminderEngine.template_id));
            dayListStr.Add(collection.GetFormValue(PayloadKeys.ReminderEngine.day1));
            templateTypeListStr.Add(collection.GetFormValue(PayloadKeys.ReminderEngine.template_type1));
            templateIDListStr.Add(collection.GetFormValue(PayloadKeys.ReminderEngine.template_id1));
            dayListStr.Add(collection.GetFormValue(PayloadKeys.ReminderEngine.day2));
            templateTypeListStr.Add(collection.GetFormValue(PayloadKeys.ReminderEngine.template_type2));
            templateIDListStr.Add(collection.GetFormValue(PayloadKeys.ReminderEngine.template_id2));
            dayListStr.Add(collection.GetFormValue(PayloadKeys.ReminderEngine.day3));
            templateTypeListStr.Add(collection.GetFormValue(PayloadKeys.ReminderEngine.template_type3));
            templateIDListStr.Add(collection.GetFormValue(PayloadKeys.ReminderEngine.template_id3));
            dayListStr.Add(collection.GetFormValue(PayloadKeys.ReminderEngine.day4));
            templateTypeListStr.Add(collection.GetFormValue(PayloadKeys.ReminderEngine.template_type4));
            templateIDListStr.Add(collection.GetFormValue(PayloadKeys.ReminderEngine.template_id4));
            dayListStr.Add(collection.GetFormValue(PayloadKeys.ReminderEngine.day5));
            templateTypeListStr.Add(collection.GetFormValue(PayloadKeys.ReminderEngine.template_type5));
            templateIDListStr.Add(collection.GetFormValue(PayloadKeys.ReminderEngine.template_id5));
            dayListStr.Add(collection.GetFormValue(PayloadKeys.ReminderEngine.day6));
            templateTypeListStr.Add(collection.GetFormValue(PayloadKeys.ReminderEngine.template_type6));
            templateIDListStr.Add(collection.GetFormValue(PayloadKeys.ReminderEngine.template_id6));
            dayListStr.Add(collection.GetFormValue(PayloadKeys.ReminderEngine.day7));
            templateTypeListStr.Add(collection.GetFormValue(PayloadKeys.ReminderEngine.template_type7));
            templateIDListStr.Add(collection.GetFormValue(PayloadKeys.ReminderEngine.template_id7));
            dayListStr.Add(collection.GetFormValue(PayloadKeys.ReminderEngine.day8));
            templateTypeListStr.Add(collection.GetFormValue(PayloadKeys.ReminderEngine.template_type8));
            templateIDListStr.Add(collection.GetFormValue(PayloadKeys.ReminderEngine.template_id8));
            dayListStr.Add(collection.GetFormValue(PayloadKeys.ReminderEngine.day9));
            templateTypeListStr.Add(collection.GetFormValue(PayloadKeys.ReminderEngine.template_type9));
            templateIDListStr.Add(collection.GetFormValue(PayloadKeys.ReminderEngine.template_id9));

            var scheduleList = new List<ReminderScheduleObject>();

            for (int i = 0; i < 10; i++)
            {
                if (dayListStr[i]!=0 && templateTypeListStr[i]!=0 && templateIDListStr[i]!=0 )
                {
                    scheduleList.Add(new ReminderScheduleObject
                    {
                        day = dayListStr[i],
                        template_type = templateTypeListStr[i],
                        template_id = templateIDListStr[i]
                    });
                }
            }

            // result for ExtJS
            var result = false;
            var msg = "";
            var msg_prefix = reminder_engine_id == 0 ? "Create" : "Update";
            var msg_prefix_result = "";
            var msg_content = "";
            var url = "";

            var systemCode = CommonConstant.SystemCode.undefine;
       
            var obj = new ReminderEngineObject()
            {
                reminder_engine_id = reminder_engine_id,
                name = name,
                target_type = target_type,
                target_value = target_value,
                status = status,
                scheduleList = scheduleList
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
            else if (target_value == "0")
            {
                result = false;
                if (target_type == (int)CommonConstant.ReminderEngineType.ProductPurchase)
                    msg_content = "Product purchase is empty value";
                else if (target_type == (int)CommonConstant.ReminderEngineType.GiftRedeem)
                    msg_content = "Gift redeem is empty value";
                else if (target_type == (int)CommonConstant.ReminderEngineType.LocationVisit)
                    msg_content = "Location visit is empty value";
                else if (target_type == (int)CommonConstant.ReminderEngineType.MemberInactive)
                    msg_content = "Member inactive is empty value";
            }
            else
            {
                var reminderEngineManager = new ReminderEngineManager();

                // Add new object
                if (obj.reminder_engine_id == 0)
                {
                    systemCode = reminderEngineManager.Create(obj);
                }
                //else if (obj.reminder_engine_id > 0)
                //{
                //    // update object
                //    msg_prefix = "Update";
                //    systemCode = _roleManager.Update(roleObject);

                //    if (systemCode == CommonConstant.SystemCode.normal)
                //    {
                //        result = true;
                //    }
                //}

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
