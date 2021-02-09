using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

using Palmary.Loyalty.Common;

namespace Palmary.Loyalty.BO.Modules.Administration.Security
{
    public class SessionManager
    {
        // private constructor
        private SessionManager()
        {
            //set default value of session prperties
            obj_id = CommonConstant.SystemObject.cms_bo;
            obj_action_channel = CommonConstant.ActionChannel.cms_bo;
        }

        // Gets the current session.
        public static SessionManager Current
        {
            get
            {
                SessionManager session = (SessionManager)HttpContext.Current.Session["__SessionManager__"];
                if (session == null)
                {
                    session = new SessionManager();
                    HttpContext.Current.Session["__SessionManager__"] = session; // put the SessionManager() object into Session["__SessionManager__"]
                }
                return session;
            }
        }

        public static void Destroy()
        {
            HttpContext.Current.Session.Abandon();
        }

        // check valid user session
        public static bool CheckValidUser(string mvc_identity_name)
        {
            var result = false;

            if (SessionManager.Current.obj_id > 0 && SessionManager.Current.obj_type == CommonConstant.ObjectType.user
                //&& !String.IsNullOrEmpty(mvc_identity_name) 
                )
            {
                result = true;
            }
            else
            {
                SessionManager.Current.obj_name = ""; //clear session
            } 

            return result;
        }

        // **** add your session properties here: 
        public int obj_type { get; set; }
        public int obj_id { get; set; }
        public string obj_name { get; set; }
        public int obj_action_channel { get; set; }
        public DateTime obj_loginTime { get; set; }
        public string obj_ip { get; set; }
        public int obj_language_id { get; set; }
        
        // additional properties
    }
}
