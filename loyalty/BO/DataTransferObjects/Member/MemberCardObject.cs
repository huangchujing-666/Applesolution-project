using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palmary.Loyalty.BO.DataTransferObjects.Member
{
    public class MemberCardObject
    {
        public int card_id { get; set; }
        public int member_id { get; set; }
        public string card_no { get; set; }
        public int card_version { get; set; }
        public int card_type { get; set; }
        public int card_status { get; set; }
        public DateTime? issue_date { get; set; }
        public int old_card_id { get; set; }
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
        public string member_no { get; set; }
        public string card_type_name { get; set; }
        public string status_name { get; set; }
        public string card_status_name { get; set; }
    }
}
