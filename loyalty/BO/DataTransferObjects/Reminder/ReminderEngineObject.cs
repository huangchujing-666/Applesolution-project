using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palmary.Loyalty.BO.DataTransferObjects.Reminder
{
    public class ReminderEngineObject
    {
        public int reminder_engine_id { get; set; }
        public string name { get; set; }
        public int target_type { get; set; }
        public string target_value { get; set; }
        public int status { get; set; }

        public DateTime crt_date { get; set; }
        public int crt_by_type { get; set; }
        public int crt_by { get; set; }
        public DateTime upd_date { get; set; }
        public int upd_by_type { get; set; }
        public int upd_by { get; set; }
        public int record_status { get; set; }

        // additional
        public string target_type_name { get; set; }
        public List<ReminderScheduleObject> scheduleList { get; set; }
        public string status_name { get; set; }

        public ReminderEngineObject()
        {
            // defalut value
            name = "";
            target_value = "";

            // defalut value for additional
            target_type_name = "";
        }
    }
}
