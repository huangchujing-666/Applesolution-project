using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palmary.Loyalty.BO.DataTransferObjects.CSV
{
    public class MemberImportCSVReportObject
    {
        // CsvHelper Library needs getter, setter
        public int excel_row_no  { get; set; }
        public string member_no  { get; set; }
        public string fail_reason  { get; set; }
    }

    public class MemberImportCSVReportHeaderObject
    {
        // CsvHelper Library needs getter, setter
        public string excel_row_no  { get; set; }
        public string member_no { get; set; }
        public string fail_reason { get; set; }

        public MemberImportCSVReportHeaderObject(){
            excel_row_no = "Excel Row No";
            member_no = "Member No";
            fail_reason = "Fail Reason";
        }
    }
}
