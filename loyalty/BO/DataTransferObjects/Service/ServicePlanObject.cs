using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palmary.Loyalty.BO.DataTransferObjects.Service
{
    public class ServicePlanObject
    {
        public int plan_id { get; set; }
        public string plan_no { get; set; }
        public int type { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public double fee { get; set; }
        public int point_expiry_month { get; set; }
        public double ratio_payment { get; set; }
        public double ratio_point { get; set; }
        public int status { get; set; }
        public DateTime crt_date { get; set; }
        public int crt_by_type { get; set; }
        public int crt_by { get; set; }
        public DateTime upd_date { get; set; }
        public int upd_by_type { get; set; }
        public int upd_by { get; set; }
        public int record_status { get; set; }
        
        // additional
        public string status_name { get; set; }
        public string type_name { get; set; }
    }
}
