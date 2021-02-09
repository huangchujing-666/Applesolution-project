using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Palmary.Loyalty.Web_backend.Controllers.Administration
{
    [Authorize]
    public class FunctionController : Controller
    {
        //
        // GET: /Function/

        public ActionResult Index()
        {
            return View();
        }

    }
}
