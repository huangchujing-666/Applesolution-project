using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palmary.Loyalty.BO.DataTransferObjects.Member
{
    public class MemberColumnObject
    {
        public int column_id { get; set; }
        public int udd_column { get; set; }
        public int udd_column_id { get; set; }
        public int datatype { get; set; }
        public int datalength { get; set; }
        public string display_name { get; set; }
        public string remark { get; set; }
        public DateTime crt_date { get; set; }
        public int crt_by_type { get; set; }
        public int crt_by { get; set; }
        public DateTime upd_date { get; set; }
        public int upd_by_type { get; set; }
        public int upd_by { get; set; }
        public int record_status { get; set; }

        // additional information
        public string udd_column_name { get; set; }
        public string datatype_name { get; set; }

    }
}
