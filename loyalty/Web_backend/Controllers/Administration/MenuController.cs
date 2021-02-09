using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.IO;
//using Palmary.Loyalty.Web_backend.Infrastructure;
using Palmary.Loyalty.BO.Modules.Administration.Table;
using Palmary.Loyalty.BO.Modules.Administration.Security;
using Palmary.Loyalty.BO.DataTransferObjects.Security;
using Palmary.Loyalty.Web_backend.Modules.Utility;
using Palmary.Loyalty.Web_backend.Modules.GlobalHandlers;
using Palmary.Loyalty.Web_backend;

using Palmary.Loyalty.Common;

namespace Palmary.Loyalty.Web_backend.Controllers
{
    [Authorize]
    public class MenuController : Controller
    {
        //
        // GET: /Menu/

        private MenuManager _menuManger;
        private string _ip;

        public MenuController()
        {
            _menuManger = new MenuManager();

            // Keep Access Object Data
      
        }

        public String MenuTree()
        {
            var parentID = 0;

            if (!string.IsNullOrEmpty(Request.QueryString["node"]) && Request.QueryString["node"] != "src")
            {
                parentID = int.Parse(Request.QueryString["node"]);
            }

            var menuTree = _menuManger.GetMenuTree(SessionManager.Current.obj_id, parentID, SessionManager.Current.obj_language_id).ToArray();

            var list = menuTree.Select(x => new
            {
                id = x.id,
                text = x.text,
                iconCls = x.iconCls,
                leaf = x.leaf,
                url = x.url,
                modulue = x.module,
                expanded = true
            });

            var result = list.ToJson();
            
            return result.ToString();
        }
    }
}