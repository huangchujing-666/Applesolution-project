using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palmary.Loyalty.BO.DataTransferObjects.Passcode
{
    public class PasscodeObject
    {
        public long passcode_id { get; set; }
        public string pin_value { get; set; }
        public int generate_id { get; set; }
        public int passcode_prefix_id { get; set; }
        public DateTime active_date { get; set; }
        public DateTime expiry_date { get; set; }
        public double point { get; set; }
        public int product_id { get; set; }
        public string product_name { get; set; }
        public int status { get; set; }
        public int registered { get; set; }
        public string registered_name { get; set; }
        public DateTime? void_date { get; set; }
        public string void_reason { get; set; }
        public DateTime crt_date { get; set; }
        public int crt_by_type { get; set; }
        public int crt_by { get; set; }
        public DateTime upd_date { get; set; }
        public int upd_by_type { get; set; }
        public int upd_by { get; set; }
        public int record_status { get; set; }

        public int? member_id { get; set; }
        public string member_no { get; set; }
    }
}
