using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palmary.Loyalty.BO.DataTransferObjects.CSV
{
    public class ServicePaymentImportCSVReportObject
    {
        public class Row
        {
            // CsvHelper Library needs getter, setter
            public int excel_row_no { get; set; }
            public string fail_reason { get; set; }

            public string invoice_no { get; set; }
            public string member_no { get; set; }
            public string member_service_no { get; set; }
            public string service_plan_no { get; set; }
            
        }

        public class Header
        {
            // CsvHelper Library needs getter, setter
            public string excel_row_no { get; set; }
            public string fail_reason { get; set; }

            public string invoice_no { get; set; }
            public string member_no { get; set; }
            public string member_service_no { get; set; }
            public string service_plan_no { get; set; }
            
            public Header()
            {
                excel_row_no = "Excel Row No";
                fail_reason = "Fail Reason";

                invoice_no = "Invoice No";
                member_no = "Member Code";
                member_service_no = "Member Service No";
                service_plan_no = "Service Plan No";
            }
        }
    }
}
