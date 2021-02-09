using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Palmary.Loyalty.BO.Modules.Passcode;
using Palmary.Loyalty.BO.Database;

using Palmary.Loyalty.Common;

namespace Palmary.Loyalty.API
{
    /// <summary>
    /// Summary description for PasscodeRegistry
    /// </summary>
    public class PasscodeRegistry : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            if (String.IsNullOrWhiteSpace(context.Request.Form["member_id"]))
            {
                context.Response.Write("{\"success\":false,\"msg\":\"Null or empty value\"}");
            }
            else
            {
                //By POST
                #region Parameters Binding/Checking
                var member_id = int.Parse(context.Request.Form["member_id"]);

                #endregion


                PasscodeManager _passcodeManager = new PasscodeManager();

                var resultList = _passcodeManager.GetPasscodeRegistryLists(1, member_id, 0, 0, "").ToList();
                var resultJSON = resultList.ToJson();

                context.Response.Write(resultJSON);

            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}