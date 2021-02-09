using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palmary.Loyalty.BO.DataTransferObjects.Member
{
    public class MemberLevelChangeObject
    {
        public int change_id { get; set; }
        public int member_id { get; set; }
        public int source_type { get; set; }
        public int change_type { get; set; }
        public int reason_type { get; set; }
        public int old_member_level_id { get; set; }
        public int new_member_level_id { get; set; }
        public string remark { get; set; }
        public DateTime crt_date { get; set; }
        public int crt_by_type { get; set; }
        public int crt_by { get; set; }
        public DateTime upd_date { get; set; }
        public int upd_by_type { get; set; }
        public int upd_by { get; set; }
        public int record_status { get; set; }

        // additional
        public string member_no { get; set; }
        public string source_type_name { get; set; }
        public string change_type_name { get; set; }

        public string reason_type_name { get; set; }
        public string old_member_level_name { get; set; }
        public string new_member_level_name { get; set; }
    }
}
