using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

using Palmary.Loyalty.Common;
using Palmary.Loyalty.Web_backend.Modules.Administration;
using Palmary.Loyalty.Web_backend.Modules.GlobalHandlers;
using Palmary.Loyalty.Web_backend;

namespace Palmary.Loyalty.Web_backend.Controllers.Administration
{
    [Authorize]
    public class LogController : Controller
    {
        private int _id;

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

        // Edit View Form
        public string SummaryViewForm()
        {
            var log_id = _id;

            // Access BO to retrieve real data
            var logHandler = new LogHandler();
            var formTableJSON = logHandler.GetSummaryForm(log_id);
            return formTableJSON;
        }

        // Edit View Form
        public string DetailViewForm()
        {
            var log_id = _id;

            // Access BO to retrieve real data
            var logHandler = new LogHandler();
            var formTableJSON = logHandler.GetDetailForm(log_id);
            return formTableJSON;
        }
    }
}
