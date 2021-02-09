using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palmary.Loyalty.BO.DataTransferObjects.Gift
{
    public class GiftInventoryObject
    {
        public int inventory_id { get; set; }
        public int source_id { get; set; }
        public int location_id { get; set; }
        public int gift_id { get; set; }
        public int stock_change_type { get; set; }
        public int stock_change { get; set; }
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
        public string stock_change_type_name { get; set; }
        
    }
}
