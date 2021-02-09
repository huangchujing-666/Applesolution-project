using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palmary.Loyalty.BO.DataTransferObjects.CSVExport
{
    public class TransactionExportObject
    {
        public class Header // should match with TransactionObject
        {
            // CsvHelper Library needs getter, setter
            public string transaction_id { get; set; }
            public string type { get; set; } // CommonConstant.TransactionType
            public string source_id { get; set; } // source id to induce point earn/use
            public string location_id { get; set; }
            public string member_id { get; set; }
            public string point_change { get; set; }
            public string point_status { get; set; }
            public string point_expiry_date { get; set; }

            public string display { get; set; }
            public string void_date { get; set; }
            public string remark { get; set; }
            public string status { get; set; }
            public string crt_date { get; set; }
            public string crt_by_type { get; set; }
            public string crt_by { get; set; }
            public string upd_date { get; set; }
            public string upd_by_type { get; set; }
            public string upd_by { get; set; }
            public string record_status { get; set; }

            // additional info 
            public string type_name { get; set; }
            public string member_name { get; set; }
            public string member_no { get; set; }
            public string point_status_name { get; set; }
            public string status_name { get; set; }
            public string crt_by_name { get; set; }

            public Header()
            {
                transaction_id = "transaction_id";
                type = "type";
                source_id = "source_id";
                location_id = "location_id";
                member_id = "member_id";
                point_change = "point_change";
                point_status = "point_status";
                point_expiry_date = "point_expiry_date";
                display = "display";
                void_date = "void_date";
                remark = "remark";
                status = "status";
                crt_date = "crt_date";
                crt_by_type = "crt_by_type";
                crt_by = "crt_by";
                upd_date = "upd_date";
                upd_by_type = "upd_by_type";
                upd_by = "upd_by";
                record_status = "record_status";

                // additional info 
                type_name = "type_name";
                member_name = "member_name";
                member_no = "member_no";
                point_status_name = "point_status_name";
                status_name = "status_name";
                crt_by_name = "crt_by_name";

            }
        }
    }
}
