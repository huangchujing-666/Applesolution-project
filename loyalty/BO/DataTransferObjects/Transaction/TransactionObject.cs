using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Palmary.Loyalty.Common;

namespace Palmary.Loyalty.BO.DataTransferObjects.Transaction
{
    public class TransactionObject
    {
        public int transaction_id { get; set; }
        public int type { get; set; } // CommonConstant.TransactionType
        public int source_id { get; set; } // source id to induce point earn/use
        public int location_id { get; set; }
        public int member_id { get; set; }
        public double point_change { get; set; }
        public int point_status { get; set; }
        public DateTime? point_expiry_date { get; set; }
        
        public bool display { get; set; }
        public DateTime? void_date { get; set; }
        public string remark { get; set; }
        public int status { get; set; }
        public DateTime crt_date { get; set; }
        public int crt_by_type { get; set; }
        public int crt_by { get; set; }
        public DateTime upd_date { get; set; }
        public int upd_by_type { get; set; }
        public int upd_by { get; set; }
        public int record_status { get; set; }

        // additional info 
        public string type_name { get; set; }
        public string member_name { get; set; }
        public string member_no { get; set; }
        public string point_status_name { get; set; }
        public string status_name { get; set; }
        public string crt_by_name { get; set; }
        public string gift_name { get; set; }
    }
}
