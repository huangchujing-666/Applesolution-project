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

namespace Palmary.Loyalty.BO.Modules.PromotionRule
{
    public class PromotionRulePurchaseProductCriteriaManager
    {
        private static LoyaltyDBDataContext db = new LoyaltyDBDataContext();
        private AccessManager _accessManager;
        private string _module = CommonConstant.Module.promotionRulePurchaseProductCriteria;
        private RolePrivilegeObject _privilege;
        private static LogManager _logManager;
        private AccessObject _accessObject;

         // For backend, using BO Session to access
        public PromotionRulePurchaseProductCriteriaManager()
        {
            // access object
            _accessManager = new AccessManager();
            _accessObject = _accessManager.GetAccessObjectBySession();

            // privilege and log
            _privilege = _accessManager.AccessModule(_module);
            _logManager = new LogManager();
        }

        // For Webservices or other not using BO Session to access
        public PromotionRulePurchaseProductCriteriaManager(AccessObject accessObject)
        {
            // access object
            _accessManager = new AccessManager();
            _accessObject = accessObject;

            // privilege and log
            _privilege = _accessManager.AccessModule(_module, _accessObject.type, _accessObject.id);
            _logManager = new LogManager(_accessObject);
        }


        public CommonConstant.SystemCode Create(
            int rule_id,
            PromotionRulePurchaseCriteriaObject dataObject
        )
        {
            int? sql_result = 0;
            var system_code = CommonConstant.SystemCode.undefine;

            if (_privilege.insert_status == 1)
            {
                var result = db.sp_CrtPromotionRulePurchaseCrite(
                    SessionManager.Current.obj_id,

                    rule_id,
                    dataObject.target_type,
                    dataObject.target_id,
                    dataObject.criteria,
                    dataObject.quantity,
                    dataObject.point,
                    dataObject.status,
             
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

        public List<PromotionRulePurchaseCriteriaObject> GetList(int rule_id)
        {
            var query = (from r_pc in db.promotion_rule_purchase_criterias
                         join pl in db.product_langs on r_pc.target_id equals pl.product_id

                         where (
                         r_pc.record_status != (int)CommonConstant.RecordStatus.deleted
                         && r_pc.rule_id == rule_id
                         && pl.lang_id == _accessObject.languageID
                         && r_pc.target_type == (int)CommonConstant.PromotionRulePurchaseProductTargetType.product
                     )
                         select new PromotionRulePurchaseCriteriaObject
                         {
                             rec_id = r_pc.rec_id,
                             rule_id = r_pc.rule_id,
                             target_type = r_pc.target_type,
                             target_id = r_pc.target_id,
                             criteria = r_pc.criteria,
                             quantity = r_pc.quantity,
                             point = r_pc.point,
                             status = r_pc.status,
                             crt_date = r_pc.crt_date,
                             crt_by_type = r_pc.crt_by_type,
                             crt_by = r_pc.crt_by,
                             upd_date = r_pc.upd_date,
                             upd_by_type = r_pc.upd_by_type,
                             upd_by = r_pc.upd_by,
                             record_status = r_pc.record_status,

                             // additional
                             target_name = pl.name
                         });

            return query.ToList();
        }
    }
}
