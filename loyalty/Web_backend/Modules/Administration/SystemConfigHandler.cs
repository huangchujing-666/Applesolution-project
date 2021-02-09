using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Palmary.Loyalty.Common;
using Palmary.Loyalty.BO.DataTransferObjects.DB;
using Palmary.Loyalty.Web_backend.Modules.GlobalHandlers;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.ExtJsEntity;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.PayoadKey;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.TableData;

using Palmary.Loyalty.BO.DataTransferObjects.SystemConfig;
using Palmary.Loyalty.BO.Modules.Administration.SystemControl;


namespace Palmary.Loyalty.Web_backend.Modules.Administration
{
    public class SystemConfigHandler : IHandler
    {
        public string LoadListDataToExtJSJson(int prefix_id, int startRowIndex, int rowLimit, List<SearchParmObject> searchParmList, string sortColumn, CommonConstant.SortOrder sortOrder, ref int rowTotal)
        {
            var systemConfigManager = new SystemConfigManager();
            var resultCode = CommonConstant.SystemCode.undefine;

            var resultList = systemConfigManager.GetList(startRowIndex, rowLimit, searchParmList, sortColumn, sortOrder, ref resultCode, ref rowTotal).ToList();

            var list = new List<ExtJsDataRow_systemConfig> { };

            foreach (var x in resultList)
            {
                var action = "";

                if (x.edit)
                    action = "<button type='button' style=\"display:\" onclick=\"new com.embraiz.tag().open_pop_up('Edit_config:" + x.config_id + "','System Config: edit','com.palmary.systemconfig.js.edit','iconRemarkList','iconRemarkList','iconRemarkList',9282,'owner','')\">Edit</button>";

                list.Add(new ExtJsDataRow_systemConfig
                {
                    id = x.config_id,
                    name = x.name,
                    value = x.value,
                    action = action
                });

            }

            return list.ToJson();
        }

        public string GetPopForm(SystemConfigObject obj)
        {
            // basic table data
            var extTable = new ExtJsTable
            {
                post_url = "../Table/ListData",
                post_header = "../Table/GridHeader",
                title = obj.config_id == 0 ? "Create System Config" : "Edit System Config",
                icon = "iconRole16",
                post_params = "../SystemConfig/Update",
                isType = true,
                button_text = "Save",
                button_icon = "iconSave",
                value_changes = true,
            };

            //add general row into the table
            var field_label_str = new ExtJsFieldLabelInput<string>(PayloadKeys.Dummy, obj.name)
            {
                fieldLabel = "Name",
                // group = "General",
                colspan = 2,
                readOnly = true
            };
            extTable.AddFieldLabelToRow(field_label_str);

            field_label_str = new ExtJsFieldLabelInput<string>(PayloadKeys.SystemConfig.value, obj.value)
            {
                fieldLabel = "Value",
                regex = obj.regex,
                regexText = obj.regex_text,
                colspan = 2
            };
            extTable.AddFieldLabelToRow(field_label_str);


            // hidden table data =================================
            var hiddenLabel = new ExtJsFieldLabelHidden<int>(PayloadKeys.SystemConfig.config_id, obj.config_id.ToString());
            extTable.AddFieldLabelToHiddenRow(hiddenLabel);

            return extTable.ToJson();
        }
    }
}