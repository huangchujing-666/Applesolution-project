using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Palmary.Loyalty.Common;
using Palmary.Loyalty.BO.DataTransferObjects.DB;
using Palmary.Loyalty.Web_backend.Modules.GlobalHandlers;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.ExtJsEntity;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.PayoadKey;
using Palmary.Loyalty.BO.DataTransferObjects.Role;
using Palmary.Loyalty.BO.Modules.Administration.Role;

namespace Palmary.Loyalty.Web_backend.Modules.Administration
{
    public class RoleHandler : IHandler
    {
        public string LoadListDataToExtJSJson(int prefix_id, int startRowIndex, int rowLimit, List<SearchParmObject> searchParmList, string sortColumn, CommonConstant.SortOrder sortOrder, ref int rowTotal)
        {
            // Request BO Method
            var roleManager = new RoleManager();
            var resultCode = CommonConstant.SystemCode.undefine;
            var dataList = roleManager.GetList(startRowIndex, rowLimit, searchParmList, sortColumn, sortOrder, ref resultCode, ref rowTotal).ToList();

            var resultList = dataList.Select(x => new
            {
                id = x.role_id.ToString(),
                href = "new com.embraiz.tag().openNewTag('EDIT_R:" + x.role_id + "','Role EDIT:" + x.name + "','com.palmary.role.js.edit','iconRole16','iconRole16','iconRole16','" + x.role_id + "')",
                name = x.name,
                status_name = x.status_name
            });

            return resultList.ToJson();
        }

        public string GetFormByModule(RoleObject role)
        {
            // basic table data
            var extTable = new ExtJsTable
            {
                post_url = "../Table/ListData",
                post_header = "../Table/GridHeader",
                title = role.role_id == 0 ? "Create Role" : "Edit Role",
                icon = "iconRole16",
                post_params = "../Role/Update",
                isType = true,
                button_text = "Save",
                button_icon = "iconSave",
                value_changes = true,
            };

            //add general row into the table
            ExtJsFieldLabelInput<string> field_label_str;
            ExtJsFieldLabelSelect<int> field_select;


            field_label_str = new ExtJsFieldLabelInput<string>(PayloadKeys.Role.name, role.name)
            {
                fieldLabel = "Name",
                group = "General"
            };
            extTable.AddFieldLabelToRow(field_label_str);

            field_select = new ExtJsFieldLabelSelect<int>(PayloadKeys.Role.status, role.status.ToString())
            {
                fieldLabel = "Status",
                datasource = "../Table/GetListItems/status", //Url.Action("GetListItems/Status", "Table"),
                display_value = role.status_name
            };
            extTable.AddFieldLabelToRow(field_select);

            // hidden table data =================================
            var hiddenLabel = new ExtJsFieldLabelHidden<int>(PayloadKeys.Role.role_id, role.role_id.ToString());
            extTable.AddFieldLabelToHiddenRow(hiddenLabel);

            return extTable.ToJson();
        }
    }
}