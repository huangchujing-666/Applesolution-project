using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palmary.Loyalty.BO.DataTransferObjects.Passcode
{
    public class PasscodePrefixObject
    {
        public int prefix_id { get; set; }
        public int product_id { get; set; }
        public long format_id { get; set; }
        public string prefix_value { get; set; }
        public long current_generated { get; set; }
        public double usage_precent { get; set; }
        public int status { get; set; }
        public DateTime crt_date { get; set; }
        public DateTime upd_date { get; set; }
        public int crt_by { get; set; }
        public int upd_by { get; set; }
        public int record_status { get; set; }

        // additional
        public string passcode_format { get; set; }
        public long maximum_generate { get; set; }
        public string product_name { get; set; }
    }
}
