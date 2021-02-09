using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Palmary.Loyalty.BO.Modules;
using Palmary.Loyalty.BO.Modules.Passcode;
using Palmary.Loyalty.BO.Modules.Administration.Security;
using Palmary.Loyalty.BO.Database;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.ExtJsEntity;
using Palmary.Loyalty.Web_backend.Modules.GlobalHandlers;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.PayoadKey;
using Palmary.Loyalty.Web_backend.Modules.Utility;
using System.Web.Routing;
using Palmary.Loyalty.Web_backend.Modules.ExtJS;
using Palmary.Loyalty.Web_backend;
using Palmary.Loyalty.BO.DataTransferObjects.Security;

namespace Palmary.Loyalty.Web_backend.Controllers
{
    [Authorize]
    public class PasscodePrefixController : Controller
    {
        //
        // GET: /PasscodePrefix/
        private PasscodePrefixManager _passcodePrefixManager;

        public PasscodePrefixController()
        {
            _passcodePrefixManager = new PasscodePrefixManager();
        }

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            SystemHandler.InitSystemParameters(this.HttpContext.ApplicationInstance as MvcApplication);
        }

        public string Insert()
        {
            var formTableJSON = TableFormHandler.GetFormByModule(new sp_GetPasscodePrefixDetailByResult());
            return formTableJSON;
        }

        public string Update(FormCollection collection)
        {
            var prefix_id = collection.GetFormValue(PayloadKeys.Id);
            var product_id = collection.GetFormValue(PayloadKeys.Product.product_id);
            var format_id = 1; // collection.GetFormValue(PayloadKeys.format_id);
            var prefix_value = collection.GetFormValue(PayloadKeys.prefix_value);
            var status = 1; // collection.GetFormValue(PayloadKeys.Status);
          
            var sql_remark = "";
            if (prefix_id == 0)
            {
                var addFlag = _passcodePrefixManager.Create(
                    SessionManager.Current.obj_id,

                    product_id,
                    format_id,
                    prefix_value,
                    status,

                    ref prefix_id,
                    ref sql_remark);

                return addFlag ? "{success:true,url:'',msg:'Add Success'}" : "{success:true,url:'',msg:'Add Failed: " + sql_remark + "'}";
            }
            else if (prefix_id > 0)
            {
                var updateFlag = false;
                //var updateFlag = _passcode_prefixManager.Update(
                //    SessionManager.Current.user_id,

                //    prefix_id,
                //    product_id,
                //    format_id,
                //    prefix_value,
                //    current_generated,
                //    usage_precent,
                //    status,
                //    crt_date,
                //    upd_date,
                //    crt_by,
                //    upd_by,
                //    record_status,

                //    ref sql_remark);

                return updateFlag ? "{success:true,url:'',msg:'Saved Success'}" : "{success:true,url:'',msg:'Saved Failed: " + sql_remark + "'}";
            }
            else
                return "{success:true,url:'',msg:'Saved Failed: Invalid Data'}";
        }
    }
}
