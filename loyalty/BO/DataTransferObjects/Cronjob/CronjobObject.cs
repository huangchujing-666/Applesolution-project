using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palmary.Loyalty.BO.DataTransferObjects.Cronjob
{
    public class CronjobObject
    {
        public int cronjob_id { get; set; }
        public string name { get; set; }
        public DateTime? execute_date { get; set; }
        public DateTime? complete_date { get; set; }
        public string execute_result { get; set; }

        public string execute_message { get; set; }
        public int no_of_processd { get; set; }
        public int no_of_success { get; set; }
        public int no_of_fail { get; set; }

        public int status { get; set; }
        public DateTime crt_date { get; set; }
        public int crt_by_type { get; set; }
        public int crt_by { get; set; }
        public DateTime upd_date { get; set; }
        public int upd_by_type { get; set; }
        public int upd_by { get; set; }
        public int record_status { get; set; }

        public string status_name { get; set; }
    }
}
