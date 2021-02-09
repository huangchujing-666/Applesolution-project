using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Palmary.Loyalty.BO.DataTransferObjects.Role;

namespace Palmary.Loyalty.BO.DataTransferObjects.User
{
    [Serializable]
    public class UserObject
    {
        [DisplayName("User ID")]
        public int user_id { get; set; }
        [DisplayName("Login ID")]
        public string login_id { get; set; }
        [DisplayName("Name")]
        public string name { get; set; }
        [DisplayName("Password")]
        public string password { get; set; }
        [DisplayName("Email")]
        public string email { get; set; }
        [DisplayName("Action IP")]
        public string action_ip { get; set; }
        [DisplayName("Action Date")]
        public DateTime? action_date { get; set; }
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
        [DisplayName("Role")]
        public List<RoleObject> role_list { get; set; }

        public UserObject()
        {
            // default value 
            user_id = 0;
            login_id = "";
            name = "";
            password = "";
            email = "";
            action_ip = "";
            action_date = null;
            status = 0;

            // default value for additional
            status_name = "";
            role_list = new List<RoleObject>();
        }
    }
}
