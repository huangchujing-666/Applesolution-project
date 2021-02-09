using Palmary.Loyalty.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Palmary.Loyalty.Web_frontend.Filters
{
    public class CheckLoginAttribute:ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string user = null, lang = "";
            if (HttpContext.Current.Request.RequestContext.RouteData.Values["LangeCode"]!=null)
            {
                lang = HttpContext.Current.Request.RequestContext.RouteData.Values["LangCode"] + "";
            }
            else
            {
                //lang = CommonConstant.LangCodeStr.tc;
            }
        }
    }
}