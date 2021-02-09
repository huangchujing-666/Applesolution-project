using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palmary.Loyalty.BO.DataTransferObjects.PromotionRule
{
    public class PromotionRuleMemberCategoryObject
    {
        public int rec_id { get; set; }
        public int rule_id { get; set; }
        public int member_category_id { get; set; }
        public int status { get; set; }
        public DateTime crt_date { get; set; }
        public int crt_by_type { get; set; }
        public int crt_by { get; set; }
        public DateTime upd_date { get; set; }
        public int upd_by_type { get; set; }
        public int upd_by { get; set; }
        public int record_status { get; set; }


        public string member_category_name { get; set; }
    }
}
