using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palmary.Loyalty.BO.DataTransferObjects.Wifi
{
    public class WifiAccessHistoryObject
    {
        public int history_id { get; set; }
        public int location_id { get; set; }
        public int member_id { get; set; }
        public string client_ip { get; set; }
        public string client_mac_address { get; set; }
        public int status { get; set; }
        public DateTime crt_date { get; set; }
        public int crt_by_type { get; set; }
        public int crt_by { get; set; }
        public DateTime upd_date { get; set; }
        public int upd_by_type { get; set; }
        public int upd_by { get; set; }
        public int record_status { get; set; }

        // additional
        public string member_no { get; set; }
        public string location_no { get; set; }
        public string location_name { get; set; }
    }
}
