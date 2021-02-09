using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palmary.Loyalty.BO.DataTransferObjects.Service
{
    public class ServicePaymentObject
    {
        public int payment_id { get; set; }
        public int transaction_id { get; set; }
        public string invoice_no { get; set; }
        public int member_id { get; set; }
        public int member_service_id { get; set; }
        public int plan_id { get; set; }
        public DateTime? service_start_date { get; set; }
        public DateTime? service_end_date { get; set; }
        public double amount { get; set; }
        public double paid_amount { get; set; }
        public DateTime payment_date { get; set; }
        public int payment_method { get; set; }
        public int status { get; set; }
        public DateTime crt_date { get; set; }
        public int crt_by_type { get; set; }
        public int crt_by { get; set; }
        public DateTime upd_date { get; set; }
        public int upd_by_type { get; set; }
        public int upd_by { get; set; }
        public int record_status { get; set; }

        // additional info 
        public string member_service_no { get; set; }
        public string member_service_name { get; set; }
        
        public string service_plan_no { get; set; }
        public string payment_method_name { get; set; }
        public string member_no { get; set; }
    }
}
