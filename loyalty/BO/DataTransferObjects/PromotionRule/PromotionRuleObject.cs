using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Palmary.Loyalty.BO.DataTransferObjects.Member;

namespace Palmary.Loyalty.BO.DataTransferObjects.PromotionRule
{
    public class PromotionRuleObject
    {
        public int rule_id { get; set; }
        public string name { get; set; }
        public DateTime? start_date { get; set; }
        public DateTime? end_date { get; set; }
        public int type { get; set; }
        public int conjunction { get; set; }
        public string type_name { get; set; }
        public int? transaction_criteria { get; set; }
        
        public int? special_criteria_type { get; set; }
        public int? special_criteria_value { get; set; }
        public int? earn_point_type { get; set; }
        public double? earn_point_value { get; set; }
        public int? earn_gift_id { get; set; }
        public double? earn_gift_quantity { get; set; }
        public double? redeem_discount { get; set; }
        public int status { get; set; }
        public DateTime crt_date { get; set; }
        public int crt_by_type { get; set; }
        public int crt_by { get; set; }
        public DateTime upd_date { get; set; }
        public int upd_by_type { get; set; }
        public int upd_by { get; set; }

        public string status_name { get; set; }

        public List<MemberLevelObject> member_level_list { get; set; }
        public List<MemberCategoryObject> member_category_list { get; set; }

        public List<PromotionRulePurchaseCriteriaObject> purchase_criteria_list { get; set; }
        public List<PromotionRuleServiceCriteriaObject> service_criteria_list { get; set; }

        // additional
        public double total_earn_point { get; set; }
        public List<int> hit_purchase_list { get; set; }
    }
}
