using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Web.Routing;

using Palmary.Loyalty.BO.DataTransferObjects.Security;
using Palmary.Loyalty.BO.DataTransferObjects.User;
using Palmary.Loyalty.BO.Modules.Administration.User;
using Palmary.Loyalty.BO.Modules.Administration.Security;
using Palmary.Loyalty.BO.Database;

using Palmary.Loyalty.Web_backend.Modules.GlobalHandlers;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.ExtJsEntity;
using Palmary.Loyalty.Web_backend.Modules.Utility;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.PayoadKey;
using Palmary.Loyalty.Web_backend.Modules.Administration;

using HashLib;
using Palmary.Loyalty.Common;
using Palmary.Loyalty.BO.DataTransferObjects.Role;
using System.Text.RegularExpressions;

namespace Palmary.Loyalty.Web_backend.Controllers.Administration
{
    [Authorize]
    public class UserController : Controller
    {
        private int _id;

        public UserController()
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

        public string GetModule() //Edit Form
        {
            var _userManager = new UserManager();
            var systemCode = CommonConstant.SystemCode.undefine;

            UserObject theUser;
            if (_id > 0)
                theUser = _userManager.GetDetail(_id, null, null, false, ref systemCode);
            else
                theUser = new UserObject();

            UserHandler userHandler = new UserHandler();
            var extTable = userHandler.GetFormByModule(theUser);

            return extTable.ToJson();
        }

        [ValidateInput(false)]
        public string Update(FormCollection collection)
        {
            var userManager = new UserManager();

            var user_id = collection.GetFormValue(PayloadKeys.Id);
            var email = collection.GetFormValue(PayloadKeys.UserProfile.email);
            var status = collection.GetFormValue(PayloadKeys.UserProfile.status);
            var login_id = collection.GetFormValue(PayloadKeys.UserProfile.login_id);
            var name = collection.GetFormValue(PayloadKeys.UserProfile.name);
            var action_ip = collection.GetFormValue(PayloadKeys.UserProfile.action_ip);
            var action_date = collection.GetFormValue(PayloadKeys.UserProfile.action_date);
            var password = collection.GetFormValue(PayloadKeys.Password);
            var confirmPassowrd = collection.GetFormValue(PayloadKeys.ConfirmPassword);

            // change_fields
            var change_fields = collection.GetFormValue(PayloadKeys.change_fields);
            var jsonExtractHelper = new JsonExtractHelper();
            var changedFields = jsonExtractHelper.ExtJSFormChangedFields(change_fields);

            // role
            var roleListString = "";
            if (!String.IsNullOrWhiteSpace(collection.GetFormValue(PayloadKeys.UserProfile.role)))
                roleListString = collection.GetFormValue(PayloadKeys.UserProfile.role);

            var roleList = roleListString.Split(',');

            var validPassword = false;

            // result for ExtJS
            var result = false;
            var msg = "";
            var msg_prefix = user_id == 0 ? "Create" : "Update";
            var msg_prefix_result = "";
            var msg_content = "";
            var url = "";

            // Regex check
            var regNormalText = new Regex(CommonConstant.RegexStr.normalText);
            var regNormalCode = new Regex(CommonConstant.RegexStr.normalCode);

            if (user_id == 0 && confirmPassowrd == password && !String.IsNullOrWhiteSpace(password))
                validPassword = true;
            else if (user_id > 0 && String.IsNullOrWhiteSpace(password) && String.IsNullOrWhiteSpace(confirmPassowrd))
                validPassword = true;
            else if (user_id > 0 && confirmPassowrd == password && !String.IsNullOrWhiteSpace(password))
                validPassword = true;
            else
                msg_content = "Password Mismatch";

            var user_object = new UserObject()
            {
                user_id = user_id,
                login_id = login_id,
                password = password,
                name = name,
                email = email,
                status = status
            };

            // role list
            var role_list = new List<RoleObject>();
            foreach (var r in roleList)
            {
                role_list.Add(new RoleObject()
                {
                    role_id = int.Parse(r)
                }
                );
            }
            user_object.role_list = role_list;

            //check input data
            if (login_id.Trim().Length == 0 && user_id == 0)
            {
                result = false;
                msg_content = "Login ID is required";
            }
            else if (login_id.Length > 20 && user_id == 0)
            {
                result = false;
                msg_content = "Login ID cannot more than 20 characters";
            }
            else if (!regNormalCode.IsMatch(login_id))
            {
                result = false;
                msg_content = "Login ID is not in correct format";
            }
            else if (name.Trim().Length == 0)
            {
                result = false;
                msg_content = "Name is required";
            }
            else if (name.Length > 80)
            {
                result = false;
                msg_content = "Name cannot more than 80 characters";
            }
            else if (!regNormalText.IsMatch(name))
            {
                result = false;
                msg_content = "Name is not in correct format";
            }
            else
            {
                // Add new user
                if (user_id == 0)
                {
                    if (validPassword)
                    {
                        var systemCode = CommonConstant.SystemCode.undefine;
                        var sql_remark = "";
                        var new_user_id = 0;

                        systemCode = userManager.Create(
                            user_object,
                            ref new_user_id);

                        result = false;

                        if (systemCode == CommonConstant.SystemCode.normal)
                        {
                            result = true;
                        }
                        else if (systemCode == CommonConstant.SystemCode.no_permission)
                            msg_content = "No Permission";
                        else if (systemCode == CommonConstant.SystemCode.err_loginId_exist)
                            msg_content = "Login ID is duplicate";
                        else if (systemCode == CommonConstant.SystemCode.err_email_exist)
                            msg_content = "Email is duplicate";
                        else if (systemCode == CommonConstant.SystemCode.record_invalid)
                            msg_content = "db error: " + sql_remark;
                    }
                    else
                    {
                        if (String.IsNullOrEmpty(password))
                            msg_content = "Password Required";
                        else
                            msg_content = "Password Mismatch";
                    }
                }
                // Edit user
                else if (user_id > 0 && validPassword && changedFields.Count() > 0 && roleList.Count() > 0)
                {
                    var a = changedFields.Count();

                    var systemCode = userManager.Update(
                        user_object,
                        changedFields);

                    if (systemCode == CommonConstant.SystemCode.normal)
                    {
                        result = true;
                    }
                    else if (systemCode == CommonConstant.SystemCode.no_permission)
                        msg_content = "No Permission";
                    else if (systemCode == CommonConstant.SystemCode.err_email_exist)
                        msg_content = "Email is duplicate";

                }
                else if (changedFields.Count() == 0)// no change
                {
                    msg_content = "No Need to Update";
                }
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

        // multi delete from grid
        public string MultiDelete()
        {
            var ids = Request.Form["id"];
            var idArrary = ids.Split(',');

            var userManager = new UserManager();
            var sql_remark = "";
            var result = false;
            var msg = "";
            var no_permission = false;

            var need_to_delete_count = 0;
            var success_delete_count = 0;

            var fail_rec_id_list = new List<string>();

            foreach (var idStr in idArrary)
            {
                var rec_id = int.Parse(idStr);
                if (rec_id > 0)
                {
                    need_to_delete_count++;

                    var systemCode = userManager.SoftDelete(rec_id, ref sql_remark);

                    if (systemCode == CommonConstant.SystemCode.normal)
                        success_delete_count++;
                    else if (systemCode == CommonConstant.SystemCode.no_permission)
                    {
                        no_permission = true;
                        break;
                    }
                    else if (systemCode == CommonConstant.SystemCode.record_invalid)
                        fail_rec_id_list.Add(rec_id.ToString());
                }
            }

            if (no_permission)
                msg = "Failed: no permission";
            else if (need_to_delete_count > success_delete_count)
                msg = "Deleted " + success_delete_count + " row(s). <br/>Cannot delete following id: " + string.Join(",", fail_rec_id_list.ToArray());
            else if (need_to_delete_count == success_delete_count)
            {
                result = true;
                msg = "Success deleted " + success_delete_count + " row(s).";
            }

            return new { success = result, url = "", msg = msg }.ToJson();
        }

        public string ToolbarData()
        {
            var toolData = new List<ExtJsButton>();
            toolData.Add(new ExtJsButton("button", "view") { text = "View", hidden = true, iconUrl = "iconView" });
            toolData.Add(new ExtJsButton("button", "edit") { text = "Edit", iconUrl = "iconRole16" });
            //toolData.Add(new ExtJsButton("button", "reset") { text = "Reset Password", iconUrl = "iconRole16", href = "function(){new com.embraiz.tag().open_pop_up('E:1','Head Office Fee','com.palmary.user.js.edit.extend','iconHeadOffice','iconHeadOffice','iconHeadOffice','<%=id %>');}}" });

            return new { toolData = toolData }.ToJson();
        }

        public string ChangePassword(FormCollection collection)
        {
            var _userManager = new UserManager();

            var updateFlag = false;

            var oldPassword = collection.GetFormValue(PayloadKeys.Oldpwd);
            var newPassword = collection.GetFormValue(PayloadKeys.Newpwd);
            var confirmPassword = collection.GetFormValue(PayloadKeys.Confirmpwd);

            if (newPassword == confirmPassword)
            {
                updateFlag = _userManager.UpdatePassword(SessionManager.Current.obj_id, oldPassword, confirmPassword);
            }

            return updateFlag ? "{success:true,url:'',msg:'Saved Success'}" : "{success:false,url:'',msg:'Update Failed: wrong password'}";
        }

        public string Insert()
        {
            UserHandler userHandler = new UserHandler();
            var jsonStr = userHandler.GetFormByModule();
            return jsonStr;
        }

        [AllowAnonymous]
        public string SessionCheck(FormCollection collection) // API, for front-end (ExtJS) to request valid user checking
        {
            var result = "false";
            var returnURL = Url.Action("Index", "Login");

            if (SessionManager.CheckValidUser(User.Identity.Name))
                result = "true";

            return new { result = result, user_id = SessionManager.Current.obj_id, returnURL = returnURL }.ToJson();
        }

        public string CheckDuplicateLoginID(FormCollection collection)
        {
            var loginID = collection["value"];
            var userManager = new UserManager();
            var duplicate = userManager.CheckDuplicateLoginID(loginID);
            var message = "";
            if (duplicate)
                message = "Login ID is duplicate";

            return new { success = true, duplicate = duplicate, message = message }.ToJson();
        }

        public string CheckDuplicateEmail(FormCollection collection)
        {
            var email = collection["value"];
            var userManager = new UserManager();
            var duplicate = userManager.CheckDuplicateEmail(email);
            var message = "";
            if (duplicate)
                message = "Email is duplicate";

            return new { success = true, duplicate = duplicate, message = message }.ToJson();
        }
    }
}