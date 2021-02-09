using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Palmary.Loyalty.Common;
using Palmary.Loyalty.BO.Database;
using Palmary.Loyalty.BO.DataTransferObjects.PromotionRule;
using Palmary.Loyalty.BO.Modules.Administration.Security;
using Palmary.Loyalty.BO.DataTransferObjects.Security;
using Palmary.Loyalty.BO.DataTransferObjects.Role;

namespace Palmary.Loyalty.BO.Modules.Product
{
    public class PromotionRulePurchaseCriteriaManager
    {
        private static LoyaltyDBDataContext db = new LoyaltyDBDataContext();
        private AccessManager _accessManager;
        private string _module = CommonConstant.Module.promotionRulePurchaseCriteria;
        private RolePrivilegeObject _privilege;

        public PromotionRulePurchaseCriteriaManager()
        {
            _accessManager = new AccessManager();
            _privilege = _accessManager.AccessModule(_module);
        }

        public CommonConstant.SystemCode Create(
            PromotionRulePurchaseCriteriaObject dataObject
        )
        {
            int? sql_result = 0;
            var system_code = CommonConstant.SystemCode.undefine;

            if (_privilege.insert_status == 1)
            {
                var result = db.sp_CrtPromotionRulePurchaseCrite(
                    SessionManager.Current.obj_id,

                    dataObject.rule_id,
                    dataObject.target_type,
                    dataObject.target_id,
                    dataObject.criteria,
                    dataObject.quantity,
                    dataObject.point,
                    CommonConstant.Status.active,
                    
                    ref sql_result
                );
                system_code = (CommonConstant.SystemCode)sql_result.Value;
            }
            else
            {
                system_code = CommonConstant.SystemCode.no_permission;
            }

            return system_code;
        }
    }
}