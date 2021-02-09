using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palmary.Loyalty.BO.DataTransferObjects.CombineRedemption
{
    public class CombineRedemptionObject
    {
        public int combine_id { get; set; }
        public int member_id { get; set; }
        public int coupon_id { get; set; }
        public int position { get; set; }
        public int no_of_ppl { get; set; }
        public int join_combine_id { get; set; }
        public int notified_host { get; set; }
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
