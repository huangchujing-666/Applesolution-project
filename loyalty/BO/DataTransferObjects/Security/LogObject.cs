using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palmary.Loyalty.BO.DataTransferObjects.Security
{
    public class LogObject
    {
        public long log_id { get; set; }
        public string action_ip { get; set; }

        public int action_channel { get; set; }
        public string action_channel_name { get; set; }

        public int action_type { get; set; }
        public string action_type_name { get; set; }

        public long? target_obj_id { get; set; }
        public long? target_obj_type_id { get; set; }
        public string target_obj_type_name { get; set; }
        public string target_obj_name { get; set; }

        public string action_detail { get; set; }
        public DateTime crt_date { get; set; }

        public int crt_by_type { get; set; }
        public string crt_by_type_name { get; set; }

        public int crt_by { get; set; }
        public string crt_by_name { get; set; }

    }
}
