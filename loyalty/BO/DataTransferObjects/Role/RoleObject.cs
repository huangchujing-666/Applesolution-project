using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palmary.Loyalty.BO.DataTransferObjects.Role
{
    public class RoleObject
    {
        [DisplayName("Role ID")]
        public int role_id { get; set; }
        [DisplayName("Name")]
        public string name { get; set; }
        [DisplayName("Status")]
        public int status { get; set; }

        // core
        public DateTime crt_date { get; set; }
        public int crt_by_type { get; set; }
        public int crt_by { get; set; }
        public DateTime upd_date { get; set; }
        public int upd_by_type { get; set; }
        public int upd_by { get; set; }
        public int record_status { get; set; }

        // additional
        public string status_name { get; set; }

        public RoleObject()
        {
            // defalut value
            role_id = 0;
            name = "";
            status = 0;

            // defalut value for additional
            status_name = "";
        }
    }
}
