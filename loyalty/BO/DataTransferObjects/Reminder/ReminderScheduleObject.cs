using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palmary.Loyalty.BO.DataTransferObjects.Reminder
{
    public class ReminderScheduleObject
    {
        public int reminder_schedule_id { get; set; }

        public int reminder_engine_id { get; set; }
        public int day { get; set; }
        public int template_type { get; set; }
        public int template_id { get; set; }
        
        // additional
        public string template_type_name { get; set; }
        public string template_name { get; set; }
    }
}
