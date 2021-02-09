using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Palmary.Loyalty.Web_backend;
using System.Web.Routing;

using Palmary.Loyalty.Common;

using Palmary.Loyalty.BO.Database;
using Palmary.Loyalty.BO.Modules.Administration.Role;
using Palmary.Loyalty.BO.Modules.Administration.Security;
using Palmary.Loyalty.BO.DataTransferObjects.Security;
using Palmary.Loyalty.BO.DataTransferObjects.Role;

using Palmary.Loyalty.Web_backend.Modules.GlobalHandlers;
using Palmary.Loyalty.Web_backend.Modules.Utility;
using Palmary.Loyalty.Web_backend.Modules.ExtJS;
using Palmary.Loyalty.Web_backend.Modules.Administration;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.ExtJsEntity;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.PayoadKey;
using System.Text.RegularExpressions;


namespace Palmary.Loyalty.Web_backend.Controllers.Administration
{
    [Authorize]
    public class RoleController : Controller
    {
        private RoleManager _roleManager;
        private int _role_id;

        public RoleController()
        {
            _roleManager = new RoleManager();
        }

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            var id = RouteData.Values["id"] ?? "";
            if (!string.IsNullOrEmpty(id.ToString()))
            {
                _role_id = int.Parse(id.ToString());
            }
            SystemHandler.InitSystemParameters(this.HttpContext.ApplicationInstance as MvcApplication);
        }

        // Create New Form
        public string InsertForm()
        {
            var handler = new RoleHandler();
            var formTableJSON = handler.GetFormByModule(new RoleObject());
            return formTableJSON;
        }

        // Edit View Form
        public string EditViewForm()
        {
            var systemCode = CommonConstant.SystemCode.undefine;
            var role = _roleManager.GetDetail(_role_id, ref systemCode);

            var handler = new RoleHandler();
            var formTableJSON = handler.GetFormByModule(role);
            return formTableJSON;
        }

        public string EditView_ToolbarData()
        {
            var toolData = new List<ExtJsButton>();

            toolData.Add(new ExtJsButton("button", "view") { text = "View", hidden = true, iconUrl = "iconView" });
            toolData.Add(new ExtJsButton("button", "edit") { text = "Edit", iconUrl = "iconRole16" });

            var result = new { toolData = toolData }.ToJson();
            // remove double quotation for herf:function
            result = result.Replace(@"""href"":""f", @"""href"":f");
            result = result.Replace(@"""}]}", @"}]}");

            return result;
        }

        // Create or Update
        [ValidateInput(false)]
        public string Update(FormCollection collection)
        {
            var role_id = collection.GetFormValue(PayloadKeys.Role.role_id);
            var name = collection.GetFormValue(PayloadKeys.Role.name);
            var status = collection.GetFormValue(PayloadKeys.Role.status);

            // result for ExtJS
            var result = false;
            var msg = "";
            var msg_prefix = role_id == 0 ? "Create" : "Update";
            var msg_prefix_result = "";
            var msg_content = "";
            var url = "";

            var systemCode = CommonConstant.SystemCode.undefine;
            
            var roleObject = new RoleObject()
            {
                role_id = role_id,
                name = name,
                status = status
            };

            // Regex check
            var regNormalText = new Regex(CommonConstant.RegexStr.normalText); 

            // check data
            if (name.Length > 80)
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
                // Add new object
                if (roleObject.role_id == 0)
                {
                    systemCode = _roleManager.Create(roleObject);
                }
                else if (roleObject.role_id > 0)
                {
                    // update object
                    systemCode = _roleManager.Update(roleObject);
                }

                // common result check
                if (systemCode == CommonConstant.SystemCode.normal)
                    result = true;
                else if (systemCode == CommonConstant.SystemCode.no_permission)
                    msg_content = "No Permission";
                else if (systemCode == CommonConstant.SystemCode.err_roleName_exist)
                    msg_content = "Name is duplicate";
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

                    // request relative BO Manager to perform DELETE
                    var roleManager = new RoleManager();
                    var systemCode = roleManager.SoftDelete(rec_id, ref sql_remark);

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
    }
}
