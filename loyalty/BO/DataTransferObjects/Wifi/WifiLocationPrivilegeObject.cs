using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palmary.Loyalty.BO.DataTransferObjects.Wifi
{
    public class WifiLocationPrivilegeObject
    {
        public int privilege_id { get; set; }
        public int location_id { get; set; }
        public int member_level_id { get; set; }
        public int status { get; set; }
        public DateTime crt_date { get; set; }
        public int crt_by_type { get; set; }
        public int crt_by { get; set; }
        public DateTime upd_date { get; set; }
        public int upd_by_type { get; set; }
        public int upd_by { get; set; }
        public int record_status { get; set; }

        // additional
        public int member_level_display_order { get; set; }
        public string member_level_name { get; set; }
    }
}
