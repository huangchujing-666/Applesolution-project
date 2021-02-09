using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Palmary.Loyalty.Web_backend;


namespace Palmary.Loyalty.Web_backend.Modules.GlobalHandlers
{
    public static class NetworkHandler
    {
        private static MvcApplication _mvcApplication;
        private static HttpRequest _request;

        public static MvcApplication GetMVCApplication()
        {
            return _mvcApplication;
        }

        public static void SetMVCApplication(MvcApplication theApp)
        {
            _mvcApplication = theApp;
        }

        public static void SetRequest(HttpRequest request)
        {
            _request = request;
        }

        public static string GetIP()
        {
            var ip = _mvcApplication.Request.ServerVariables["REMOTE_ADDR"];
            return ip;
        }

        public static string GetIP(HttpRequest request)
        {
            var ip = request.ServerVariables["REMOTE_ADDR"];
            System.Diagnostics.Debug.WriteLine("NetworkHandler IP: " + ip);
      
            return ip;
        }

        public static string GetIP(MvcApplication theApp)
        {
            var ip = theApp.Request.ServerVariables["REMOTE_ADDR"];
            System.Diagnostics.Debug.WriteLine("NetworkHandler IP: " + ip);

            return ip;
        }
    }
}