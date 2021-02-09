using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palmary.Loyalty.BO.DataTransferObjects.Gift
{
    public class GiftRedemptionObject
    {
        public int redemption_id { get; set; }
        public int transaction_id { get; set; }
        public string redemption_code { get; set; }
        public int redemption_channel { get; set; }
        public int member_id { get; set; }
        public int gift_id { get; set; }
        public int quantity { get; set; }
        public double point_used { get; set; }
        public int redemption_status { get; set; }
        public DateTime? collect_date { get; set; }
        public int collect_location_id { get; set; }
        public DateTime? void_date { get; set; }
        public int? void_user_id { get; set; }
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
        public string gift_no { get; set; }
        public string gift_name { get; set; }
        public string location_name { get; set; }
        public string redemption_status_name { get; set; }
        public string member_no { get; set; }
        public string member_name { get; set; }
        public string crt_by_name { get; set; }
        public string upd_by_name { get; set; }
    }
}
