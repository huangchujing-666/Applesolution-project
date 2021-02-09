using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


using Palmary.Loyalty.Web_backend.Modules.Utility;
using System.Web.Security;
using Palmary.Loyalty.BO.Modules.Administration.Security;
using Palmary.Loyalty.Web_backend.Modules.GlobalHandlers;
using Palmary.Loyalty.Web_backend.Modules.Security;
using System.Web.Routing;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure;
using System.Configuration;
using Palmary.Loyalty.Web_backend;

namespace Palmary.Loyalty.Web_backend.Controllers
{
    [Authorize] // Require Premission
    public class HomeController : Controller
    {
        private AccessManager _loginManager;
        private AccessHandler _userAccess;

        public HomeController()
         
        {         
            _loginManager = new AccessManager();
            _userAccess = new AccessHandler();
        }

       //  
        protected override void Initialize(RequestContext requestContext)
      {
            base.Initialize(requestContext);
            SystemHandler.InitSystemParameters(this.HttpContext.ApplicationInstance as MvcApplication);
        }

        public ActionResult Index()
        {
            ViewBag.UserLoginTime = SessionManager.Current.obj_loginTime;
            ViewBag.LanguageChangeID = SessionManager.Current.obj_language_id == 2 ? 3 : 2;
            ViewBag.LanguageChangeName = SessionManager.Current.obj_language_id == 2 ? "简体中文" : "English";

            var homeObj = new HomeObject()
            {
                siteVersion = ConfigurationManager.AppSettings["siteVersion"]
            };

            return View(homeObj);
        }

        // for shorten path landing, then redirect to Index action
        public ActionResult Land()
        {
            return RedirectToAction("Index");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        [AllowAnonymous]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


        [AllowAnonymous]
        public ActionResult Test()
        {
            ViewBag.Message = "Your contact page.";
            //         FormsAuthentication.SetAuthCookie("leo", false);
            return View();
        }

        //[AllowAnonymous]
        //public ActionResult Login()
        //{
        //    return View("Login");
        //}
    }
}
