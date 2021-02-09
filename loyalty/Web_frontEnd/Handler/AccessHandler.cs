using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.Security;

namespace Palmary.Loyalty.Web_frontend.Handler
{
    public class AccessHandler
    {
        public bool MemberLogin(int member_id, string member_name)
        {
           // FormsAuthentication.SetAuthCookie(member_name, false);    //conflict with backend
            SessionHandler.Current.member_id = member_id;
            SessionHandler.Current.member_name = member_name;

            return true;
        }

        public bool MemberLogout()
        {
            return true;
        }
    }
}