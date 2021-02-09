using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palmary.Loyalty.BO.DataTransferObjects.PromotionRule
{
    public class PromotionRuleServiceCriteriaObject
    {
        public int criteria_id { get; set; }
        public int rule_id { get; set; }
        public int service_category_id { get; set; }
        public int criteria_type { get; set; }
        public double criteria_value { get; set; }
        public int status { get; set; }
        public DateTime crt_date { get; set; }
        public int crt_by_type { get; set; }
        public int crt_by { get; set; }
        public DateTime upd_date { get; set; }
        public int upd_by_type { get; set; }
        public int upd_by { get; set; }
        public int record_status { get; set; }

        public string service_category_name { get; set; }
        public string criteria_type_name { get; set; }
    }
}
