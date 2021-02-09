using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palmary.Loyalty.BO.DataTransferObjects.Service
{
    public class ServicePaymentDetailExtraObject
    {
        public int earn_id { get; set; }
        public int transaction_id { get; set; }
        public int source_type { get; set; }
        public int source_id { get; set; }
        public double point_earn { get; set; }
        public int point_status { get; set; }
        public DateTime point_expiry_date { get; set; }
        public double point_used { get; set; }
        public int status { get; set; }
        public DateTime crt_date { get; set; }
        public int crt_by_type { get; set; }
        public int crt_by { get; set; }
        public DateTime upd_date { get; set; }
        public int upd_by_type { get; set; }
        public int upd_by { get; set; }
        public int record_status { get; set; }

        // additional info
        public int rule_id { get; set; }

        public string member_no { get; set; }
        public string point_status_name { get; set; }
        public string rule_name { get; set; }

    }
}
