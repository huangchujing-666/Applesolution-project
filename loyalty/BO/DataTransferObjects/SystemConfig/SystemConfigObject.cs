using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palmary.Loyalty.BO.DataTransferObjects.SystemConfig
{
    public class SystemConfigObject
    {
        [DisplayName("Config ID")]
        public int config_id { get; set; }
        [DisplayName("Name")]
        public string name { get; set; }
        [DisplayName("Value")]
        public string value { get; set; }
        [DisplayName("Data Type")]
        public string data_type { get; set; }
        [DisplayName("Display")]
        public bool display { get; set; }
        [DisplayName("Display Order")]
        public int display_order { get; set; }
        [DisplayName("Edit")]
        public bool edit { get; set; }
        [DisplayName("Regex")]
        public string regex { get; set; }
        [DisplayName("Regex text")]
        public string regex_text { get; set; }

        // core
        public DateTime crt_date { get; set; }
        public int crt_by_type { get; set; }
        public int crt_by { get; set; }
        public DateTime upd_date { get; set; }
        public int upd_by_type { get; set; }
        public int upd_by { get; set; }
        public int record_status { get; set; }
    }
}
