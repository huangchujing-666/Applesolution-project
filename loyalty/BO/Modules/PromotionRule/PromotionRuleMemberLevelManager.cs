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
    public class PromotionRuleMemberLevelManager
    {
        private static LoyaltyDBDataContext db = new LoyaltyDBDataContext();
        private AccessManager _accessManager;
        private string _module = CommonConstant.Module.promotionRuleMemberLevel;
        private RolePrivilegeObject _privilege;

        public PromotionRuleMemberLevelManager()
        {
            _accessManager = new AccessManager();
            _privilege = _accessManager.AccessModule(_module);
        }

        public CommonConstant.SystemCode Create(
            int rule_id, int member_level_id
        )
        {
            int? sql_result = 0;
            var system_code = CommonConstant.SystemCode.undefine;

            if (_privilege.insert_status == 1)
            {
                var result = db.sp_CrtPromotionRuleMemberLevel(
                    SessionManager.Current.obj_id,
                    
                    rule_id,
                    member_level_id,
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

        public bool CheckExist(int rule_id, int member_level_id)
        {
            var query = (from ml in db.promotion_rule_member_levels
                         where (
                         ml.record_status != (int)CommonConstant.RecordStatus.deleted
                         && ml.rule_id == rule_id
                         && ml.member_level_id == member_level_id
                     )
                         select new PromotionRuleMemberLevelObject
                         {
                             rec_id = ml.rec_id,
                         });

            if (query.Count() > 0)
                return true;
            else
                return false;
        }

        public List<PromotionRuleMemberLevelObject> GetList(int rule_id)
        {
            var query = (from ml in db.promotion_rule_member_levels
                         join l in db.member_levels on ml.member_level_id equals l.level_id
                         where (
                         ml.record_status != (int)CommonConstant.RecordStatus.deleted
                         && ml.rule_id == rule_id
                     )
                         select new PromotionRuleMemberLevelObject
                         {
                             rec_id = ml.rec_id,
                             member_level_name = l.name
                         });

            return query.ToList();
        }
    }
}
