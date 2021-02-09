using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Web.Routing;
using Palmary.Loyalty.Web_backend.Modules.GlobalHandlers;
using Palmary.Loyalty.BO.Modules.Administration.Security;

namespace Palmary.Loyalty.Web_backend.Controllers.Administration
{
    [Authorize]
    public class LanguageController : Controller
    {
        //
        // GET: /Language/

        private int _language_id;

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            var id = RouteData.Values["id"] ?? "";
            if (!string.IsNullOrEmpty(id.ToString()))
            {
                _language_id = int.Parse(id.ToString());
            }
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Change()
        {
            SessionManager.Current.obj_language_id = _language_id;
        
            return RedirectToAction("Index", "Home");
        }

    }
}
