using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palmary.Loyalty.BO.DataTransferObjects.CSV
{
    public class PointAdjustmentImportCSVReportObject
    {
        public class Row
        {
            // CsvHelper Library needs getter, setter
            public int excel_row_no { get; set; }
            public string fail_reason { get; set; }

            public string member_no { get; set; }
            public double adjust_point { get; set; }
            public string adjust_reason { get; set; }
            public int member_id { get; set; }
            public int location_id { get; set; }

        }

        public class Header
        {
            // CsvHelper Library needs getter, setter
            public string excel_row_no { get; set; }
            public string fail_reason { get; set; }

            public string member_no { get; set; }
            public double adjust_point { get; set; }
            public string adjust_reason { get; set; }
            public int member_id { get; set; }
            public int location_id { get; set; }

            public Header()
            {
                excel_row_no = "Excel Row No";
                fail_reason = "Fail Reason";

                member_no = "Member Code";
                adjust_reason = "Adjust Reason";
            }
        }
    }
}
