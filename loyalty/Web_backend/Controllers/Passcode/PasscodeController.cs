using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Palmary.Loyalty.BO.Modules.Passcode;
using Palmary.Loyalty.BO.Modules.Administration.Security;
using Palmary.Loyalty.BO.Database;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.ExtJsEntity;
using Palmary.Loyalty.Web_backend.Modules.GlobalHandlers;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.PayoadKey;
using Palmary.Loyalty.Web_backend.Modules.Utility;
using System.Web.Routing;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.TableData;
using Palmary.Loyalty.Web_backend;
using Palmary.Loyalty.BO.DataTransferObjects.Security;

using Palmary.Loyalty.Common;

namespace Palmary.Loyalty.Web_backend.Controllers
{
    [Authorize]
    public class PasscodeController : Controller
    {
        //
        // GET: /Passcode/
        private int _prefix_id;
        private PasscodeManager _passcodeManager;

        public PasscodeController()
        {
            _passcodeManager = new PasscodeManager();
        }

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            var id = RouteData.Values["id"] ?? "";
            if (!string.IsNullOrEmpty(id.ToString()))
            {
                _prefix_id = int.Parse(id.ToString());
            }
            SystemHandler.InitSystemParameters(this.HttpContext.ApplicationInstance as MvcApplication);
        }

        public string GenerateWithPrefix()
        {
            
            // Fields
           List<ExtJSField> fieldList = new List<ExtJSField>();
            fieldList.Add(new ExtJSField {
                name = "noToGenerate",
                fieldLabel = "Number to generate",
                type = "input",
                colspan = 2,
                tabIndex = "1"
            });
            //fieldList.Add(new ExtJSField
            //{
            //    name = "point",
            //    fieldLabel = "Point",
            //    type = "input",
            //    colspan = 2,
            //    tabIndex = "2"
            //});
            fieldList.Add(new ExtJSField
            {
                name = "active_date",
                fieldLabel = "Active Date",
                type = "date",
                colspan = 2,
                tabIndex = "2"
            });

            // Hidden Fields
            List<ExtJSField_hidden> hiddenList = new List<ExtJSField_hidden>();
            hiddenList.Add(new ExtJSField_hidden
            {
                type = "hidden",
                name = "prefix_id",
                value = _prefix_id.ToString()
            });

            var formTableJSON = new {
                row = fieldList,
                rowhidden = hiddenList, 

                column = 2, 
                post_url = "modules/owner/list_data.jsp",  //<-
                post_header = "modules/owner/grid_header.jsp", //<-
                title = "Config", 
                icon = "iconRemarkList", 
                post_params = Url.Action("Generate"),

                button_text = "Save", 
                button_icon = "iconSave", 
                value_changes = true 
            }.ToJson();

            return formTableJSON;
        }

        public string Generate(FormCollection collection)
        {
            _prefix_id = Convert.ToInt32(collection["prefix_id"].Trim());

            var noToGenerate =  Convert.ToInt64(collection["noToGenerate"].Trim());

            var active_date_str = collection["active_date"].Trim();
            var theYear = int.Parse(active_date_str.Substring(0, 4));
            var theMonth = int.Parse(active_date_str.Substring(5, 2));
            var theDay = int.Parse(active_date_str.Substring(8, 2));

            DateTime active_date = new DateTime(theYear, theMonth, theDay);

            var sql_result = _passcodeManager.Generate(
                  SessionManager.Current.obj_id,
                   _prefix_id,
                   noToGenerate,
                   active_date
                   );

            var result = "";
            if (sql_result)
                result = new { success = true, msg = "Generate Complete" }.ToJson();
            else
                result = new { success = false, msg = "Generate Fail" }.ToJson();

            return result;
        }
    }
}