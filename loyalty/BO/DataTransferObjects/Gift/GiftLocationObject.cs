using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palmary.Loyalty.BO.DataTransferObjects.Gift
{
    public class GiftLocationObject
    {
        public int location_id { get; set; }
        public string location_no { get; set; }
        public int type { get; set; }
        public string photo_file_name { get; set; }
        public string photo_file_extension { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public string phone { get; set; }
        public string fax { get; set; }
        public int address_district { get; set; }
        public int address_region { get; set; }
        public int display_order { get; set; }
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