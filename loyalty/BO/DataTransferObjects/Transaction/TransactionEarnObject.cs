using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palmary.Loyalty.BO.DataTransferObjects.Transaction
{
    public class TransactionEarnObject
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
    }
}
