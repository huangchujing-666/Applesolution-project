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
    public class PromotionRuleServiceCriteriaManager
    {
        private static LoyaltyDBDataContext db = new LoyaltyDBDataContext();
        private AccessManager _accessManager;
        private string _module = CommonConstant.Module.promotionRuleServiceCriteria;
        private RolePrivilegeObject _privilege;

        public PromotionRuleServiceCriteriaManager()
        {
            _accessManager = new AccessManager();
            _privilege = _accessManager.AccessModule(_module);
        }

        // Manager Object (DAO)  CREATE (LINQ version)
        public CommonConstant.SystemCode Create(
            PromotionRuleServiceCriteriaObject dataObject
        )
        {
            int? sql_result = 0;
            var system_code = CommonConstant.SystemCode.undefine;

            if (_privilege.insert_status == 1)
            {
                var result = db.sp_CrtPromotionRuleServiceCrite(
                    SessionManager.Current.obj_id,

                    dataObject.rule_id,
                    dataObject.service_category_id,
                    dataObject.criteria_type,
                    dataObject.criteria_value,
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

        public bool CheckExist(int rule_id, int service_category_id)
        {
            var query = (from s in db.promotion_rule_service_criterias
                         where (
                         s.record_status != (int)CommonConstant.RecordStatus.deleted
                         && s.rule_id == rule_id
                         && s.service_category_id == service_category_id
                     )
                         select new PromotionRuleServiceCriteriaObject
                         {
                             criteria_id = s.criteria_id,
                         });

            if (query.Count() > 0)
                return true;
            else
                return false;
        }

        public PromotionRuleServiceCriteriaObject GetDetail(int rule_id, int service_category_id)
        {
            PromotionRuleServiceCriteriaObject resultObject;

            var query = (from s in db.promotion_rule_service_criterias
                         where (
                         s.record_status != (int)CommonConstant.RecordStatus.deleted
                         && s.rule_id == rule_id
                         && s.service_category_id == service_category_id
                     )
                         select new PromotionRuleServiceCriteriaObject
                         {
                             criteria_id = s.criteria_id,
                             rule_id = s.rule_id,
                             service_category_id = s.service_category_id,
                             criteria_type = s.criteria_type,
                             criteria_value = s.criteria_value,
                             status = s.status,
                             crt_date = s.crt_date,
                             crt_by_type = s.crt_by_type,
                             crt_by = s.crt_by,
                             upd_date = s.upd_date,
                             upd_by_type = s.upd_by_type,
                             upd_by = s.upd_by,
                             record_status = s.record_status,
                         });

            resultObject = query.FirstOrDefault();

            return resultObject;
        }

        public List<PromotionRuleServiceCriteriaObject> GetList(int rule_id)
        {
            var query = (from s in db.promotion_rule_service_criterias
                         join c in db.service_categories on s.service_category_id equals c.category_id

                         join li in db.listing_items on s.criteria_type equals li.value
                         join l in db.listings on li.list_id equals l.list_id

                         where (
                         s.record_status != (int)CommonConstant.RecordStatus.deleted
                         && s.rule_id == rule_id
                         && l.code == "ServicePaymentCriteriaType"
                     )
                         select new PromotionRuleServiceCriteriaObject
                         {
                             criteria_id = s.criteria_id,
                             service_category_name = c.name,
                             criteria_type_name = li.name,
                             criteria_value = s.criteria_value
                         });

            return query.ToList();
        }

    }
}
