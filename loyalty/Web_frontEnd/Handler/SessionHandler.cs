using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Palmary.Loyalty.Web_frontend.Handler
{
    public class SessionHandler
    {
        // private constructor
        private SessionHandler()
        {
            //set default value of session prperties
        }

        // Gets the current session.

        public static SessionHandler Current
        {
            get
            {
                SessionHandler session = (SessionHandler)HttpContext.Current.Session["__SessionHandler__"];
                if (session == null)
                {
                    session = new SessionHandler();
                    HttpContext.Current.Session["__SessionHandler__"] = session; // put the SessionHandler() object into Session["__SessionHandler__"]
                }
                return session;
            }
        }

        public static void Destroy()
        {
            HttpContext.Current.Session.Abandon();
        }

        // check valid user session
        public static bool checkValidUser(string mvc_identity_name)
        {
            var result = false;

            if (!String.IsNullOrEmpty(mvc_identity_name) && SessionHandler.Current.member_id > 0)
            {
                result = true;
            }
            else
            {
                SessionHandler.Current.member_name = ""; //clear session
            }

            return result;
        }

        // **** add your session properties here:
        public int member_id { get; set; }
        public string member_name { get; set; }
        public bool forceLogout { get; set; }
    }
}