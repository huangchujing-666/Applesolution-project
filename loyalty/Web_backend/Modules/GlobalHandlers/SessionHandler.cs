using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Palmary.Loyalty.Web_backend.Modules.GlobalHandlers
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

        // **** add your session properties here:
    }
}