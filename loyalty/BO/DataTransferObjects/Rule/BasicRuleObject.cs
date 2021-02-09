using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palmary.Loyalty.BO.DataTransferObjects.Rule
{
    public class BasicRuleObject
    {
        public int basic_rule_id { get; set; }
        public int type { get; set; }
        public int target_id { get; set; }
        public int member_level_id { get; set; }
        public int memebr_category_id { get; set; }
        public double ratio_payment { get; set; }
        public double ratio_point { get; set; }
        public double point { get; set; }
        public int point_expiry_month { get; set; }
        public string remark { get; set; }
        public int status { get; set; }
        public DateTime crt_date { get; set; }
        public int crt_by_type { get; set; }
        public int crt_by { get; set; }
        public DateTime upd_date { get; set; }
        public int upd_by_type { get; set; }
        public int upd_by { get; set; }
        public int record_status { get; set; }

        // additional
        public string target_no { get; set; }
        public string member_level_name { get; set; }
        public string status_name { get; set; }
        public string member_category_name { get; set; }
        public string type_name { get; set; }
        public int member_level_display_order { get; set; }
        public int category_display_order { get; set; }
    }
}
