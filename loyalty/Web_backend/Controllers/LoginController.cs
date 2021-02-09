using System;
using System.Collections.Generic;
using System.Linq;
//using System.Web;
using System.Web.Mvc;

using Palmary.Loyalty.Web_backend.Modules.Utility;
using System.Web.Security;
using Palmary.Loyalty.BO.Modules.Administration.Security;
using Palmary.Loyalty.Web_backend.Modules.GlobalHandlers;
using Palmary.Loyalty.Web_backend.Modules.Security;
using System.Web.Routing;
using Palmary.Loyalty.Web_backend;

using Palmary.Loyalty.Common;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure;
using System.Configuration;

namespace Palmary.Loyalty.Web_backend.Controllers
{
    [Authorize]
    public class LoginController : Controller
    {
        //
        // GET: /Login/

        private AccessHandler _userAccess;

        
        public LoginController()
        {
            _userAccess = new AccessHandler();
        }

        
        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            SystemHandler.InitSystemParameters(this.HttpContext.ApplicationInstance as MvcApplication);
        }

        [AllowAnonymous]
        public ActionResult Index()
        {
            var homeObj = new HomeObject()
            {
                siteVersion = ConfigurationManager.AppSettings["siteVersion"]
              
            };

            return View(homeObj);
        }

        [HttpPost]  // receive POST request only
        [AllowAnonymous]
        public string Perform(FormCollection collection)
        {
            var login_id = collection["login_id"].Trim();
            var password = collection["login_password"].Trim();
            var ip = NetworkHandler.GetIP();

            var systemCode = _userAccess.Login(login_id, password, ip);

            if (systemCode == CommonConstant.SystemCode.normal)
            {
                return new { success = true }.ToJson();
            }
            else if (systemCode == CommonConstant.SystemCode.no_permission)
                return new { success = false, msg = "Incorrect Account or Password" }.ToJson();
            else
                return new { success = false, msg = "DB Connection Error / Other Error" }.ToJson();
        }

        [AllowAnonymous]
        public ActionResult Logout()
        {
            //WebSecurity.Logout();
            //FormsAuthentication.SignOut();
            //Session.Abandon(); //destroys the session
            _userAccess.Logout();

            var homeObj = new HomeObject()
            {
                siteVersion = ConfigurationManager.AppSettings["siteVersion"]

            };

            //return View("Index", homeObj);
            return RedirectToAction("Index");
        }
    }
}
