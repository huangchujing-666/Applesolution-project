using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data.Objects;
using Palmary.Loyalty.BO.Modules.Administration.Security;
using System.Web.Security;
using Palmary.Loyalty.Web_backend.Modules;
using Palmary.Loyalty.Web_backend.Modules.GlobalHandlers;
using Palmary.Loyalty.Common;

namespace Palmary.Loyalty.Web_backend.Modules.Security
{
    public class AccessHandler
    {
        private AccessManager _accessManager;

        public AccessHandler()
        {
            _accessManager = new AccessManager();
        }

        public CommonConstant.SystemCode Login(string login_id, string password, string ip)
        {
            var user_id = 0;
            var name = "";
            var systemCode = _accessManager.CMSLogin(login_id, password, ip, ref user_id, ref name);

            return systemCode;
        }

        public bool Logout()
        {
            FormsAuthentication.SignOut();
            SessionManager.Destroy();
            _accessManager.CMSLogout(SessionManager.Current.obj_id);
            return true;
        }

        public bool ChangePassword()
        {
            FormsAuthentication.SignOut();
            SessionManager.Destroy();
            _accessManager.CMSLogout(SessionManager.Current.obj_id);
            return true;
        }
    }
}