using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Palmary.Loyalty.Web_backend; // for controllers or MvcApplication
using Palmary.Loyalty.Web_backend.Modules;

namespace Palmary.Loyalty.Web_backend.Modules.GlobalHandlers
{
    public static class SystemHandler
    {
        public static void InitSystemParameters(MvcApplication theApp)
        {
            NetworkHandler.SetMVCApplication(theApp);
        }

        public static void InitSystemParameters(HttpRequest request)
        {
            NetworkHandler.SetRequest(request);
        }
    }
}