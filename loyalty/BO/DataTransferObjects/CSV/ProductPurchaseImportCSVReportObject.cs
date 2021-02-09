using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palmary.Loyalty.BO.DataTransferObjects.CSV
{
    public class ProductPurchaseImportCSVReportObject
    {
        public class Row
        {
            // CsvHelper Library needs getter, setter
            public int excel_row_no { get; set; }
            public string fail_reason { get; set; }

            public DateTime purchase_date { get; set; }
            public string member_no { get; set; }
            public string product_no { get; set; }
            public double total_amount { get; set; }

        }

        public class Header
        {
            // CsvHelper Library needs getter, setter
            public string excel_row_no { get; set; }
            public string fail_reason { get; set; }

            public string purchase_date { get; set; }
            public string member_no { get; set; }
            public string product_no { get; set; }
            public string total_amount { get; set; }

            public Header()
            {
                excel_row_no = "Excel Row No";
                fail_reason = "Fail Reason";

                purchase_date = "Purchase Date";
                member_no = "Member Code";
                product_no = "Product Code";
                total_amount = "Total Amount";
            }
        }
    }
}
