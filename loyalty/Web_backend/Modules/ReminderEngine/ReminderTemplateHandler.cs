using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Palmary.Loyalty.BO.DataTransferObjects.Reminder;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.ExtJsEntity;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.PayoadKey;
using Palmary.Loyalty.Common;
using Palmary.Loyalty.BO.DataTransferObjects.DB;
using Palmary.Loyalty.BO.Modules.Reminder;

namespace Palmary.Loyalty.Web_backend.Modules.ReminderEngine
{
    public class ReminderTemplateHandler : IHandler
    {
        public string LoadListDataToExtJSJson(int prefix_id, int startRowIndex, int rowLimit, List<SearchParmObject> searchParmList, string sortColumn, CommonConstant.SortOrder sortOrder, ref int rowTotal)
        {
            var reminderTemplateManager = new ReminderTemplateManager();

            var resultCode = CommonConstant.SystemCode.undefine;
            var resultList = reminderTemplateManager.GetList(startRowIndex, rowLimit, searchParmList, sortColumn, sortOrder, ref resultCode, ref rowTotal).ToList();
            var resultDataList = resultList.Select(
                x => new
                {
                    id = x.reminder_template_id,
                    name = x.name,
                    status_name = x.status_name,
                    href = "new com.embraiz.tag().openNewTag('EDIT_RTID:" + x.reminder_template_id.ToString() + "','Reminder Template: " + x.name + "','com.palmary.remindertemplate.js.edit','iconRole16','iconRole16','iconRole16', '" + x.reminder_template_id + "')"
                }
            );

            return resultDataList.ToJson();
        }

        public string GetFormByModule(ReminderTemplateObject obj)
        {
            var extTable = new ExtJsTable
            {
                column = 2,
                post_url = "../Table/ListData",
                post_header = "../Table/GridHeader",
                title = obj.reminder_template_id == 0 ? "Create New Reminder Template" : "Reminder Template Detail",
                icon = "iconRole16",
                post_params = "../ReminderTemplate/Update",
                //将要返回来修改的action

                isType = true,
                button_text = "Save",
                button_icon = "iconSave",
                value_changes = true,
                confirmSave = false,
            };

            var rowFieldLabel_input = new ExtJsFieldLabelInput<string>(PayloadKeys.ReminderTemplate.name, obj.name)
            {
                fieldLabel = "Name",
                tabIndex = 1,
                group = "General"
            };
            extTable.AddFieldLabelToRow(rowFieldLabel_input);

            var rowFieldSelect = new ExtJsFieldLabelSelect<int>(PayloadKeys.ReminderTemplate.status, obj.status.ToString())
            {
                fieldLabel = "Status",
                datasource = "../Table/GetListItems/status",
                display_value = obj.status_name
            };
            extTable.AddFieldLabelToRow(rowFieldSelect);

            var copyText = "<<member_firstname>>, <<member_lastname>>, <<member_salution>>, <<member_code>>, <<member_email>>, <<member_mobile>>, <<member_level>>, <<member_available_point>>";
            copyText = System.Web.HttpUtility.HtmlEncode(copyText);
          
            rowFieldLabel_input = new ExtJsFieldLabelInput<string>(PayloadKeys.Dummy, copyText)
            {
                fieldLabel = "Merge Field",
                colspan = 2,
                readOnly = true
            };
            extTable.AddFieldLabelToRow(rowFieldLabel_input);

            var rowFieldLabel_textArea = new ExtJsFieldLabelTextarea<string>(PayloadKeys.ReminderTemplate.sms_template, obj.sms_template)
            {
                fieldLabel = "SMS Template",
                display_value = HttpUtility.HtmlEncode(obj.sms_template),
                height = 200,
                allowBlank = true,
                colspan = 2
            };
            extTable.AddFieldLabelToRow(rowFieldLabel_textArea);

            var rowFieldLabel_textEditor = new ExtJsField_textEditor<string>(PayloadKeys.ReminderTemplate.email_template, obj.email_template)
            {
                fieldLabel = "Email Template",
                height = 300,
                allowBlank = true,
                colspan = 2
            };
            extTable.AddFieldLabelToRow(rowFieldLabel_textEditor);

            var hiddenLabel = new ExtJsFieldLabelHidden<int>(PayloadKeys.ReminderTemplate.reminder_template_id, obj.reminder_template_id.ToString());
            extTable.AddFieldLabelToHiddenRow(hiddenLabel);

            return extTable.ToJson();
        }
    }
}