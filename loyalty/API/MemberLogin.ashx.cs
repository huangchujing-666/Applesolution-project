using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Palmary.Loyalty.BO.Modules.Member;

namespace Palmary.Loyalty.API
{
    /// <summary>
    /// Summary description for MemberLogin
    /// </summary>
    public class MemberLogin : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            System.Diagnostics.Debug.WriteLine(":::::MemberLogin:::::::");

            if (String.IsNullOrWhiteSpace(context.Request.Form["login_type"]))
            {
                context.Response.Write("{\"success\":false,\"message\":\"Null or empty value\"}");
            }
            else
            {
                //By POST
                #region Parameters Binding/Checking
                var login_type = context.Request.Form["login_type"];
                System.Diagnostics.Debug.WriteLine(login_type, "login_type");
                var _memberManager = new MemberManager();

                if (login_type == "fb")
                {
                     System.Diagnostics.Debug.WriteLine("login with fb");
                     System.Diagnostics.Debug.WriteLine(context.Request.Form["fbid"], "fbid");
                     System.Diagnostics.Debug.WriteLine(context.Request.Form["login_id"], "login_id");
                     System.Diagnostics.Debug.WriteLine("email" + context.Request.Form["email"]);
                     System.Diagnostics.Debug.WriteLine("birthday" + context.Request.Form["birthday"]);
                     System.Diagnostics.Debug.WriteLine("gender" + context.Request.Form["gender"]);

                     var fbid = context.Request.Form["fbid"];
                     var email = context.Request.Form["email"];
                     var birthday = context.Request.Form["birthday"];
                     var gender = context.Request.Form["gender"];
                     var name = context.Request.Form["name"];
                     var middlename = context.Request.Form["middlename"];
                     var lastname = context.Request.Form["lastname"];

                     System.Diagnostics.Debug.WriteLine(fbid, "fbid");
                     System.Diagnostics.Debug.WriteLine(email, "email");
                     System.Diagnostics.Debug.WriteLine(birthday, "birthday");
                     System.Diagnostics.Debug.WriteLine(gender, "gender");


                     System.Diagnostics.Debug.WriteLine("before LoginWithFB");
                     var result = 0;// _memberManager.LoginWithFB(fbid);
                     System.Diagnostics.Debug.WriteLine("after LoginWithFB");
                    if (result == 0)
                        context.Response.Write("{\"success\":false,\"message\":\"Incorrect Login\"}");
                    else if (result == 1)
                        context.Response.Write("{\"success\":true,\"message\":\"Valid Member Login\"}");
                    else if (result == 2)
                    {
                        System.Diagnostics.Debug.WriteLine("New FB Member, need to create member record");
                        var birth_month = int.Parse(birthday.Substring(0, 2));
                        var birth_day = int.Parse(birthday.Substring(3, 2));
                        var birth_year = int.Parse(birthday.Substring(6, 4));
                        System.Diagnostics.Debug.WriteLine(birth_month + "birth_month");
                        System.Diagnostics.Debug.WriteLine(birth_day + "birth_day");
                        System.Diagnostics.Debug.WriteLine(birth_year + "birth_year");

                        var generInt = 1;
                        if (gender != "male")
                            generInt = 2;

                       
                        var sql_remark = "";
                        bool createResult = _memberManager.CreateMemberWithFB(fbid, email, name, lastname, middlename, birth_year, birth_month, birth_day, generInt, ref sql_remark);
                       if (createResult)
                           context.Response.Write("{\"success\":true,\"message\":\"New FB Member\"}");
                       else
                           context.Response.Write("{\"success\":false,\"message\":\"New FB Member: "+sql_remark+"\"}");
                    }

                }else
                {
                    //String.IsNullOrWhiteSpace(context.Request.Form["login_id"]) ||
                    //String.IsNullOrWhiteSpace(context.Request.Form["password"]))

                    var login_id = context.Request.Form["login_id"];
                    var password = context.Request.Form["password"];
                    #endregion
              
                    System.Diagnostics.Debug.WriteLine("login_id: " + login_id + ", password: " + password);

                    var member_id = 0;
                    var session = "";

                    var result = false; // _memberManager.Login(login_id, password, ref member_id, ref  session);

                    if (result)
                        context.Response.Write("{\"success\":true,\"member_id\":" + member_id + ",\"session\":\"" + session + "\"}");
                    else
                        context.Response.Write("{\"success\":false,\"message\":\"Incorrect Login\"}");
                }
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