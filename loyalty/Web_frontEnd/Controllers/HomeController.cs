using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Palmary.Loyalty.Web_frontend.Models;

namespace Web_frontend.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var loyaltyModel = new LoyaltyModels();


            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View(loyaltyModel);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
