using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palmary.Loyalty.BO.DataTransferObjects.Member
{
    public class MemberImportObject
    {
        public int import_id { get; set; }
        public string file_name { get; set; }
        public int no_of_dataRow { get; set; }
        public int no_of_imported { get; set; }
        public int no_of_failRecord { get; set; }
        public string remark { get; set; }
        public int status { get; set; }
        public DateTime crt_date { get; set; }
        public int crt_by_type { get; set; }
        public int crt_by { get; set; }
        public DateTime upd_date { get; set; }
        public int upd_by_type { get; set; }
        public int upd_by { get; set; }
        public int record_status { get; set; }

        // additional info
        public string crt_by_name { get; set; }
    }
}
