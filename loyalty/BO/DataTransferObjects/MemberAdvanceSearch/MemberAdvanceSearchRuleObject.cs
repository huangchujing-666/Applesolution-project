using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palmary.Loyalty.BO.DataTransferObjects.MemberAdvanceSearch
{
    public class MemberAdvanceSearchRuleObject
    {
        public long rec_id { get; set; }
        public int search_id { get; set; }
        public int group_id { get; set; }
        public int row_id { get; set; }
        public int target_field { get; set; }
        public int target_condition { get; set; }
        public string target_value { get; set; }
        public DateTime crt_date { get; set; }
        public int crt_by_type { get; set; }
        public int crt_by { get; set; }
        public DateTime upd_date { get; set; }
        public int upd_by_type { get; set; }
        public int upd_by { get; set; }
        public int record_status { get; set; }

        public int id { get; set; }
        public string href { get; set; }

        // additional 
        public string target_field_name { get; set; }
        public string target_condition_name { get; set; }
        public string target_value_name { get; set; }
    }
}
