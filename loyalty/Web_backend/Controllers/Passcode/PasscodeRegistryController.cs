using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Palmary.Loyalty.BO.Modules.Transaction;
using Palmary.Loyalty.BO.Modules.Passcode;
using Palmary.Loyalty.BO.Modules.Administration.Security;
using Palmary.Loyalty.BO.DataTransferObjects.Passcode;
using Palmary.Loyalty.BO.Database;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.ExtJsEntity;
using Palmary.Loyalty.Web_backend.Modules.GlobalHandlers;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.PayoadKey;
using Palmary.Loyalty.Web_backend.Modules.Utility;
using Palmary.Loyalty.Web_backend.Modules.ExtJS;
using Palmary.Loyalty.Web_backend;
using Palmary.Loyalty.Common;
using Palmary.Loyalty.Common.Languages;
using System.Web.Routing;
using Palmary.Loyalty.BO.DataTransferObjects.Security;

namespace Palmary.Loyalty.Web_backend.Controllers
{
    [Authorize]
    public class PasscodeRegistryController : Controller
    {
        private TransactionManager _transactionManager;
        private PasscodeManager _passcodeManager;
        private int _member_id;

        public PasscodeRegistryController()
        {
            _transactionManager = new TransactionManager();
            _passcodeManager = new PasscodeManager();
        }

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            var id = RouteData.Values["id"] ?? "";
            if (!string.IsNullOrEmpty(id.ToString()))
            {
                _member_id = int.Parse(id.ToString());
            }
            SystemHandler.InitSystemParameters(this.HttpContext.ApplicationInstance as MvcApplication);
        }

        // Create new
        public string Insert()
        {
            var passcodeRegistry = new PasscodeRegistryObject();
            passcodeRegistry.member_id = _member_id;

            var formTableJSON = TableFormHandler.GetFormByModule(passcodeRegistry);
            return formTableJSON;
        }

        public string Update(FormCollection collection)
        {
            var transaction_id = collection.GetFormValue(PayloadKeys.TransactionEarn.transaction_id);
            var pin_value = collection.GetFormValue(PayloadKeys.TransactionEarn.pin_value);
            var member_id = collection.GetFormValue(PayloadKeys.TransactionEarn.member_id);
            
            if (transaction_id == 0)
            {
                var result = false;
                var msg = "";

                var systemCode = _passcodeManager.Register(member_id, pin_value);

                if (systemCode == CommonConstant.SystemCode.normal)
                {
                    msg = "Passcode Register Success";
                }
                else
                {
                    msg = "Register Failed: Passcode Invalid";
                }

                return new { success = result, url = "com.palmary.ProductPurchase.js.insert_popupform", msg = msg }.ToJson();
            }
            else if (transaction_id > 0)
            {
                var updateFlag = false; // not allow to edit

                return updateFlag ? "{success:true,url:'',msg:'Saved Success'}" : "{success:true,url:'',msg:'Register Failed'}";
            }
            else
                return "{success:true,url:'',msg:'Saved Failed: Invalid Data'}";
        }
    }
}
