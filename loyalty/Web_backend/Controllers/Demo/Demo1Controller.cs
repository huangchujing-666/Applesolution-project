using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Web.Routing;
using Palmary.Loyalty.Common;
using Palmary.Loyalty.BO.Database;
using Palmary.Loyalty.BO.DataTransferObjects.Security;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.ExtJsEntity;
using Palmary.Loyalty.Web_backend.Modules.GlobalHandlers;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.PayoadKey;
using Palmary.Loyalty.Web_backend.Modules.Utility;
using Palmary.Loyalty.Web_backend.Modules.ExtJS;
using Palmary.Loyalty.Web_backend;

using Palmary.Loyalty.Web_backend.Modules.Demo;
using Palmary.Loyalty.BO.Modules.Administration.Security;

namespace Palmary.Loyalty.Web_backend.Controllers.Demo
{
    [Authorize]
    public class Demo1Controller : Controller
    {
        private int _id;

        public Demo1Controller()
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

        // Create New Form
        public string InsertForm()
        {
            var dataObject = new Demo1Object();
            var demo1Handler = new Demo1Handler();
            var formTableJSON = demo1Handler.GetFormByModule(dataObject);
            return formTableJSON;
        }

        // Edit View Form
        public string EditViewForm()
        {
            var demo_id = _id;

            // Access BO to retrieve real data
            var theDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 12, 45, 0);

            var fileHandler = new FileHandler();
            var obj = new Demo1Object()
            {
                id = 1,

                demo_id = 1,
                demo_image = fileHandler.GetImagePath("20140212143257_0765", ".jpg", (string)CommonConstant.Module.demo1, (int)CommonConstant.ImageSizeType.thumb),
                demo_name = "my name",
                demo_float = (float)1.23,
                demo_select = 1,
                demo_select_name = "my option",
                demo_date = DateTime.Now,
                demo_datetime = theDateTime,
                demo_remark = "my remark",
                demo_remark2 = @"
<ol style='background-color: rgb(255, 255, 255);'><li><font face='helvetica, arial, verdana, sans-serif' size='2'>test1</font></li><li><font face='helvetica, arial, verdana, sans-serif' size='2'>test2</font></li></ol><font face='helvetica, arial, verdana, sans-serif' style='background-color: rgb(255, 255, 255);'><font color='#ff0000'><div style='text-align: center; font-size: small;'><font face='helvetica, arial, verdana, sans-serif' size='2'><font color='#ff0000'>TEST</font><b><u> <font color='#0000ff'>&nbsp;</font></u></b></font><b><u><font color='#0000ff'>TEST &nbsp;</font><i> <font color='#ffff00' style='background-color: rgb(192, 192, 192);'>TEST&nbsp;</font></i></u></b></div><div><font size='7'>TEST</font></div><div style='font-size: small;'><b><u><i><font color='#ffff00' style='background-color: rgb(192, 192, 192);'><br></font></i></u></b></div><div style='font-size: small;'><b><u><i><font color='#ffff00' style='background-color: rgb(192, 192, 192);'><br></font></i></u></b></div><div style='font-size: small;'><b><u><i><font color='#ffff00' style='background-color: rgb(192, 192, 192);'><br></font></i></u></b></div><div style='font-size: small;'><b><u><i><font color='#ffff00' style='background-color: rgb(192, 192, 192);'><br></font></i></u></b></div><div style='font-size: small;'><b><u><i><font color='#ffff00' style='background-color: rgb(192, 192, 192);'><br></font></i></u></b></div></font></font><div><font color='#ffff00' face='helvetica, arial, verdana, sans-serif' size='2'><b><i><u><br></u></i></b></font><div><font color='#ffff00' face='helvetica, arial, verdana, sans-serif' size='2'><b><i><u><br></u></i></b></font></div><div><font color='#ffff00' face='helvetica, arial, verdana, sans-serif' size='2'><b><i><u><br></u></i></b></font><div><font color='#ffff00' face='helvetica, arial, verdana, sans-serif' size='2'><b><i><u><br></u></i></b></font><div><span style='font-family: helvetica, arial, verdana, sans-serif; font-size: small;'><b><u><i><font color='#ffff00' style='background-color: rgb(192, 192, 192);'><br></font></i></u></b></span></div></div></div></div>
",
                photo_file_name = "20140212143257_0765",
                photo_file_extension = ".jpg"
            };

            var demo1Handler = new Demo1Handler();
            var formTableJSON = demo1Handler.GetFormByModule(obj);
            return formTableJSON;
        }

        // toolbar for Edit View Form
        public string ToolbarData()
        {
            var toolData = new List<ExtJsButton>();

            toolData.Add(new ExtJsButton("button", "view") { text = "View", hidden = true, iconUrl = "iconView" });
            toolData.Add(new ExtJsButton("button", "edit") { text = "Edit", iconUrl = "iconRole16" });

            toolData.Add(new ExtJsButton("button", "demobutton1")
            {
                text = "open new tag",
                iconUrl = "iconRole16",

                newTag_method = "open_new_tag",
                newTag_id = "V_GIS:" + 1,
                newTag_title = "open_new_tag:" + 1,
                newTag_url = "com.embraiz.user.js.index",
                newTag_iconCls = "iconRole16",
                newTag_iconClsC = "iconRole16",
                newTag_iconClsE = "iconRole16",
                newTag_itemId = "1"
            });
            toolData.Add(new ExtJsButton("button", "demobutton2")
            {
                text = "open pop up",
                iconUrl = "iconRole16",

                newTag_method = "open_pop_up",
                newTag_id = "V_GIS:" + 1,
                newTag_title = "open_pop_up:" + 1,
                newTag_url = "com.embraiz.user.js.index",
                newTag_iconCls = "iconRole16",
                newTag_iconClsC = "iconRole16",
                newTag_iconClsE = "iconRole16",
                newTag_itemId = "1"
            });

            var result = new { toolData = toolData }.ToJson();

            return result;
        }

        // Create or Update
        public string Update(FormCollection collection)
        {
            var demo_id = collection.GetFormValue(PayloadKeys.Demo1.demo_id);
            var demo_name = collection.GetFormValue(PayloadKeys.Demo1.demo_name);
            var demo_remark = collection.GetFormValue(PayloadKeys.Demo1.demo_remark);
            var demo_select = collection.GetFormValue(PayloadKeys.Demo1.demo_select);
            var demo_float = collection.GetFormValue(PayloadKeys.Demo1.demo_float);
            var demo_date = collection.GetFormValue(PayloadKeys.Demo1.demo_date);
            var demo_datetime = collection.GetFormValue(PayloadKeys.Demo1.demo_datetime);

            var obj = new Demo1Object()
            {
                demo_id = demo_id,
                demo_name = demo_name,
                demo_remark = demo_remark,
                demo_select = demo_select,
                demo_float = demo_float,
                demo_date = demo_date,
                demo_datetime = demo_datetime
            };

            // Add new object
            if (obj.demo_id == 0)
            {
                var addFlag = false;
                var msg = "";
                var url = "";

                if (addFlag)
                    msg = "Add Success";
                else
                    msg = "Add Failed: Not connect db";

                return new { success = addFlag, url = url, msg = msg }.ToJson();
            }
            else
            {   // update object
                var updateFlag = false;
                var msg = "";
                var url = "";

                if (updateFlag)
                    msg = "Update Success";
                else
                    msg = "Update Failed: Not connect db";

                return new { success = updateFlag, url = url, msg = msg }.ToJson();
            }
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
                    var systemCode = CommonConstant.SystemCode.record_invalid;  //var systemCode = Manager.Delete(rec_id, ref sql_remark);

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
