using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palmary.Loyalty.BO.DataTransferObjects.Member
{
    public class MemberServiceObject
    {
        public int member_service_id { get; set; }
        public int member_id { get; set; }
        public string service_no { get; set; }
        public int plan_id { get; set; }
        public double point { get; set; }
        public DateTime start_date { get; set; }
        public DateTime end_date { get; set; }
        public int status { get; set; }
        public DateTime crt_date { get; set; }
        public int crt_by_type { get; set; }
        public int crt_by { get; set; }
        public DateTime upd_date { get; set; }
        public int upd_by_type { get; set; }
        public int upd_by { get; set; }
        public int record_status { get; set; }
    }
}
