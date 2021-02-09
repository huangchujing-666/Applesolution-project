using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palmary.Loyalty.BO.DataTransferObjects.PromotionRule
{
    public class PromotionRulePurchaseCriteriaObject
    {
        public int rec_id { get; set; }
        public int rule_id { get; set; }
        public int target_type { get; set; }
        public int target_id { get; set; }
        public int criteria { get; set; }
        public int? quantity { get; set; }
        public double? point { get; set; }
        public int status { get; set; }
        public DateTime crt_date { get; set; }
        public int crt_by_type { get; set; }
        public int crt_by { get; set; }
        public DateTime upd_date { get; set; }
        public int upd_by_type { get; set; }
        public int upd_by { get; set; }
        public int record_status { get; set; }

        // additional
        public string target_name { get; set; }
    }
}