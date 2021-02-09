using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palmary.Loyalty.BO.DataTransferObjects.Role
{
    public class RolePrivilegeObject
    {
        public int privilege_id { get; set; }
        public int object_type { get; set; }
        public int object_id { get; set; }
        public int section_id { get; set; }
        public int read_status { get; set; }
        public int insert_status { get; set; }
        public int update_status { get; set; }
        public int delete_status { get; set; }
        public int status { get; set; }
        public DateTime crt_date { get; set; }
        public DateTime upd_date { get; set; }
        public int crt_by { get; set; }
        public int upd_by { get; set; }
    }
}