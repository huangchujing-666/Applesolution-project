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
    public class PromotionRuleMemberCategoryManager
    {
        private static LoyaltyDBDataContext db = new LoyaltyDBDataContext();
        private AccessManager _accessManager;
        private string _module = CommonConstant.Module.promotionRuleMemberCategory;
        private RolePrivilegeObject _privilege;

        public PromotionRuleMemberCategoryManager()
        {
            _accessManager = new AccessManager();
            _privilege = _accessManager.AccessModule(_module);
        }

        public CommonConstant.SystemCode Create(
           int rule_id, int category_id
       )
        {
            int? sql_result = 0;
            var system_code = CommonConstant.SystemCode.undefine;

            if (_privilege.insert_status == 1)
            {
                var result = db.sp_CrtPromotionRuleMemberCat(
                    SessionManager.Current.obj_id,

                    rule_id,
                    category_id,
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

        public bool CheckExist(int rule_id, int member_category_id)
        {
            var query = (from mc in db.promotion_rule_member_categories
                            where (
                            mc.record_status != (int)CommonConstant.RecordStatus.deleted
                            && mc.rule_id == rule_id
                            && mc.member_category_id == member_category_id
                        )
                            select new PromotionRuleMemberCategoryObject
                            {
                                rec_id = mc.rec_id,
                            });

            if (query.Count() > 0)
                return true;
            else
                return false;
        }

        public List<PromotionRuleMemberCategoryObject> GetList(int rule_id)
        {
            var query = (from mc in db.promotion_rule_member_categories
                         join l in db.member_category_langs on mc.member_category_id equals l.category_id
                         where (
                         mc.record_status != (int)CommonConstant.RecordStatus.deleted
                         && mc.rule_id == rule_id
                         && l.lang_id == (int) CommonConstant.LangCode.en
                     )
                         select new PromotionRuleMemberCategoryObject
                         {
                             rec_id = mc.rec_id,
                             member_category_name = l.name
                         });

            return query.ToList();
        }
    }
}
