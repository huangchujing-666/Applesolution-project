using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Web.Routing;
using Palmary.Loyalty.BO.DataTransferObjects.Security;
using Palmary.Loyalty.BO.DataTransferObjects.Role;

using Palmary.Loyalty.BO.Modules.Administration.Role;

using Palmary.Loyalty.Common;
using Palmary.Loyalty.Web_backend.Modules.GlobalHandlers;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.ExtJsEntity;
using Palmary.Loyalty.Web_backend.Modules.Utility;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.PayoadKey;

namespace Palmary.Loyalty.Web_backend.Controllers.Administration
{
    [Authorize]
    public class RolePrivilegeController : Controller
    {
        private int _target_id;

        public RolePrivilegeController()
        {

        }

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            //_userId = int.Parse(Session["userId"].ToString());   //login access id

            var id = RouteData.Values["id"] ?? "";
            if (!string.IsNullOrEmpty(id.ToString()))
            {
                _target_id = int.Parse(id.ToString());
            }
        }

        public string Update(FormCollection collection)
        {
            var role_id = collection.GetFormValue(PayloadKeys.RolePrivilege.roleId);

            var checkRightName_string = "";
            if (!String.IsNullOrWhiteSpace(collection.GetFormValue(PayloadKeys.RolePrivilege.checkRightName)))
                checkRightName_string = collection.GetFormValue(PayloadKeys.RolePrivilege.checkRightName);

            var checkRightValue_string = "";
            if (!String.IsNullOrWhiteSpace(collection.GetFormValue(PayloadKeys.RolePrivilege.checkRightValue)))
                checkRightValue_string = collection.GetFormValue(PayloadKeys.RolePrivilege.checkRightValue);

            var roleRightNameList = checkRightName_string.Split(',');
            var roleRightValueList = checkRightValue_string.Split(',');

            var privilegeDict = new Dictionary<int, RolePrivilegeObject>();

            for (int i = 0; i < roleRightNameList.Count(); i++)
            {

                var rightName = roleRightNameList[i];
                var rightValue = roleRightValueList[i];

                int section_id = int.Parse(rightName.Substring(6, rightName.Length - 6));
                string privilegeType = rightName.Substring(5, 1);

                if (privilegeType == "R")
                {
                    var theRolePrivilege = new RolePrivilegeObject();

                    theRolePrivilege.object_type = (int)CommonConstant.ObjectType.user;
                    theRolePrivilege.object_id = role_id;
                    theRolePrivilege.section_id = section_id;

                    theRolePrivilege.read_status = int.Parse(rightValue);
                    theRolePrivilege.update_status = 0;
                    theRolePrivilege.insert_status = 0;
                    theRolePrivilege.delete_status = 0;

                    theRolePrivilege.status = CommonConstant.Status.active;

                    privilegeDict.Add(section_id, theRolePrivilege);
                }
                else
                {
                    var theRolePrivilege = privilegeDict[section_id];

                    if (privilegeType == "U")
                        theRolePrivilege.update_status = int.Parse(rightValue);
                    else if (privilegeType == "I")
                        theRolePrivilege.insert_status = int.Parse(rightValue);
                    else if (privilegeType == "D")
                        theRolePrivilege.delete_status = int.Parse(rightValue);
                }
            }

            var updatedPrivilegeList = privilegeDict.Values.ToList();

            var rolePrivilegeManager = new RolePrivilegeManager();
            var message = "";
            var result = rolePrivilegeManager.Update(updatedPrivilegeList, ref message);

            return new { result = result, msg = message }.ToJson();
        }
    }
}