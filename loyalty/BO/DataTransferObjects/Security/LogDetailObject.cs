using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palmary.Loyalty.BO.DataTransferObjects.Security
{
    [Serializable]
    public class LogDetailObject
    {
        public long log_detail_id { get; set; }
        public int log_id { get; set; }
        public string field_name { get; set; }
        public string old_value { get; set; }
        public string new_value { get; set; }
        public DateTime crt_date { get; set; }
        public int crt_by_type { get; set; }
        public int crt_by { get; set; }
        public DateTime upd_date { get; set; }
        public int upd_by_type { get; set; }
        public int upd_by { get; set; }
        public int record_status { get; set; }
    }
}
