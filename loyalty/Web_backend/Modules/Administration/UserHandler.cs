using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Newtonsoft.Json;

using Palmary.Loyalty.Common;
using Palmary.Loyalty.BO.Database;
using Palmary.Loyalty.BO.Modules.Utility;
using Palmary.Loyalty.BO.Modules.Administration.User;
using Palmary.Loyalty.BO.Modules.Administration.Role;
using Palmary.Loyalty.BO.Modules.Administration.Security;
using Palmary.Loyalty.BO.DataTransferObjects.DB;
using Palmary.Loyalty.BO.DataTransferObjects.User;

using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.ExtJsEntity;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.PayoadKey;
using Palmary.Loyalty.Web_backend.Modules.Utility;
using Palmary.Loyalty.Web_backend.Modules.GlobalHandlers;

namespace Palmary.Loyalty.Web_backend.Modules.Administration
{
    public class UserHandler : IHandler
    {
        public string LoadListDataToExtJSJson(int prefix_id, int startRowIndex, int rowLimit, List<SearchParmObject> searchParmList, string sortColumn, CommonConstant.SortOrder sortOrder, ref int rowTotal)
        {
            UserManager userManager = new UserManager();

            var resultCode = CommonConstant.SystemCode.undefine;
            var resultList = userManager.GetList(startRowIndex, rowLimit, searchParmList, sortColumn, sortOrder, ref resultCode, ref rowTotal);
            var resultDataList = resultList.Select(
                x => new
                {
                    id = x.user_id,
                    user_id = x.user_id,
                    name = x.name,
                    login_id = x.login_id,
                    email = x.email,
                    action_ip = x.action_ip,
                    status_name = x.status_name,
                    href = "new com.embraiz.tag().openNewTag('EDIT_UID:" + x.user_id.ToString() + "','User: " + x.name + "','com.palmary.user.js.edit','iconRole16','iconRole16','iconRole16', '" + x.user_id + "')"
                }
            );

            return resultDataList.ToJson();
        }

        public string GetFormByModule()
        {
            return GetFormByModule(new UserObject());
        }

        public string GetFormByModule(UserObject userProfile)
        {
            var extTable = new ExtJsTable
            {
                column = 2,
                post_url = "../Table/ListData",
                post_header = "../Table/GridHeader",
                title = userProfile.user_id == 0 ? "Create New User" : "User Detail",
                icon = "iconRole16",
                post_params = "../User/Update",
                //将要返回来修改的action

                button_text = "Save",
                button_icon = "iconSave",
                value_changes = true,
                confirmSave = false,
            };

            if (userProfile.user_id == 0)
            {
                var row1FieldLabel_loginID_noDuplicate = new ExtJsFieldLabel_inputNoDuplicate<string>(PayloadKeys.UserProfile.login_id, userProfile.login_id)
                {
                    fieldLabel = "Login ID (username)",
                    tabIndex = 1,
                    check_path = "../User/CheckDuplicateLoginID",
                    display_value = userProfile.login_id,

                };
                extTable.AddFieldLabelToRow(row1FieldLabel_loginID_noDuplicate);
            }
            else if (userProfile.user_id > 0)
            {
                var rowFieldLabel_loginID = new ExtJsFieldLabelInput<string>(PayloadKeys.Dummy, userProfile.login_id)
                {
                    fieldLabel = "Login ID (username)",
                    tabIndex = 1,
                    readOnly = true,
                    value = userProfile.login_id
                };
                extTable.AddFieldLabelToRow(rowFieldLabel_loginID);
            }

            //var row1FieldLabel_noDuplicate = new ExtJsFieldLabel_inputNoDuplicate<string>(PayloadKeys.UserProfile.login_id, userProfile.login_id)
            //{
            //    fieldLabel = "Login ID (username)",
            //    tabIndex = 1,
            //    check_path = "../User/CheckDuplicateLoginID",
            //    display_value = userProfile.login_id,
            //    readOnly = true,
            //};
            //extTable.AddFieldLabelToRow(row1FieldLabel_noDuplicate);

            var row1FieldLabel1 = new ExtJsFieldLabelInput<string>(PayloadKeys.UserProfile.name, userProfile.name)
            {
                fieldLabel = "Name",
                tabIndex = 2,
                allowBlank = false,
            };
            extTable.AddFieldLabelToRow(row1FieldLabel1);

            var row1FieldLabel_noDuplicate = new ExtJsFieldLabel_inputNoDuplicate<string>(PayloadKeys.UserProfile.email, userProfile.email)
            {
                fieldLabel = "Email",
                display_value = userProfile.email,
                regex = "emailRegex",
                check_path = "../User/CheckDuplicateEmail",
                tabIndex = 3,
                allowBlank = false,
            };
            extTable.AddFieldLabelToRow(row1FieldLabel_noDuplicate);

            var rowFieldSelect = new ExtJsFieldLabelSelect<int>(PayloadKeys.UserProfile.status, userProfile.status.ToString())
            {
                fieldLabel = "Status",
                datasource = "../Table/GetListItems/Status",
                display_value = userProfile.status.ToListingItemName("Status"),
                tabIndex = 4,
                readOnly = false
            };
            extTable.AddFieldLabelToRow(rowFieldSelect);

            // user_role
            var role_nameList = new List<string> { };
            var role_valueList = new List<string> { };

            if (userProfile.user_id > 0)
            {
                var _roleAccessManager = new RoleAccessManager();

                var roleList = _roleAccessManager.GetUserOwnedRole_lists(SessionManager.Current.obj_id, userProfile.user_id);

                foreach (var role in roleList)
                {
                    role_nameList.Add(role.role_name);
                    role_valueList.Add(role.role_id.ToString());
                }
            }

            var field_multiSelect_intList = new ExtJsFieldLabelMultiSelect<string>(PayloadKeys.UserProfile.role, "")
            {
                fieldLabel = "Role",
                datasource = "../Table/GetListItems/role",
                display_value = string.Join(",", role_nameList.ToArray()),
                value = role_valueList.ToArray(),
                tabIndex = 5,
                readOnly = false,
                allowBlank = false
            };
            extTable.AddFieldLabelToRow(field_multiSelect_intList);

            var field_password = new ExtJsFieldLabelInputPassword<string>(PayloadKeys.Password, "")
            {
                fieldLabel = "New Password",
                tabIndex = 6,
                allowBlank = false,

            };
            extTable.AddFieldLabelToRow(field_password);

            field_password = new ExtJsFieldLabelInputPassword<string>(PayloadKeys.ConfirmPassword, "")
            {
                fieldLabel = "Confirm Password",
                tabIndex = 7,
                allowBlank = false,
            };
            extTable.AddFieldLabelToRow(field_password);

            if (userProfile.user_id > 0)
            {
                var action_date_str = userProfile.action_date == null ? "" : userProfile.action_date.Value.ToString("yyyy-MM-dd hh:mm:ss:fff");

                var field_date = new ExtJsFieldLabelDate<DateTime>(PayloadKeys.UserProfile.action_date, userProfile.action_date)
                {
                    fieldLabel = "Last Login Date",
                    value = action_date_str, //userProfile.action_date.Value.ToString("yyyy-MM-dd hh:mm:ss:fff"),

                    readOnly = true
                };
                extTable.AddFieldLabelToRow(field_date);

                var field_ip = new ExtJsFieldLabelInput<string>(PayloadKeys.UserProfile.action_ip, userProfile.action_ip)
                {
                    fieldLabel = "Last Login IP",
                    readOnly = true
                };
                extTable.AddFieldLabelToRow(field_ip);
            }

            //var field_select = new ExtJsFieldLabelSelect<int>(PayloadKeys.Gift.category_id, PayloadKeys.RoleId.ToString())
            //{
            //    fieldLabel = "Role",
            //    datasource = "../Table/GetListItems/role", //Url.Action("GetListItems/Status", "Table"),
            //    display_value = "aaa,bbb",
            //    readOnly = false
            //};
            //extTable.AddFieldLabelToRow(field_select);

            //var row1FieldLabel3 = new ExtJsFieldLabelHidden<int>(PayloadKeys.Id, customerProfile.customer_id.ToString());
            //extTable.AddFieldLabelToHiddenRow(row1FieldLabel3);

            var hiddenLabel = new ExtJsFieldLabelHidden<int>(PayloadKeys.Id, userProfile.user_id.ToString());
            extTable.AddFieldLabelToHiddenRow(hiddenLabel);

            if (userProfile.user_id > 0)
            {
                var hiddenLabel_str = new ExtJsFieldLabelHidden<string>(PayloadKeys.UserProfile.login_id, userProfile.login_id);
                extTable.AddFieldLabelToHiddenRow(hiddenLabel_str);
            }

            return extTable.ToJson();
        }
    }
}