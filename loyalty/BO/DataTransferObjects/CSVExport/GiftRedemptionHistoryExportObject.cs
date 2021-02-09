using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palmary.Loyalty.BO.DataTransferObjects.CSVExport
{
    public class GiftRedemptionExportObject
    {
        public class Header // should match with GiftRedemptionObject
        {
            // CsvHelper Library needs getter, setter
            public string redemption_id { get; set; }
            public string transaction_id { get; set; }
            public string redemption_code { get; set; }
            public string redemption_channel { get; set; }
            public string member_id { get; set; }
            public string gift_id { get; set; }
            public string quantity { get; set; }
            public string point_used { get; set; }
            public string redemption_status { get; set; }
            public string collect_date { get; set; }
            public string collect_location_id { get; set; }
            public string void_date { get; set; }
            public string void_user_id { get; set; }
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
            public string gift_no { get; set; }
            public string gift_name { get; set; }
            public string location_name { get; set; }
            public string redemption_status_name { get; set; }
            public string member_no { get; set; }
            public string member_name { get; set; }
            public string crt_by_name { get; set; }

            public Header()
            {
                redemption_id = "redemption_id";
                transaction_id = "transaction_id";
                redemption_code = "redemption_code";
                redemption_channel = "redemption_channel";
                member_id = "member_id";
                gift_id = "gift_id";
                quantity = "quantity";
                point_used = "point_used";
                redemption_status = "redemption_status";
                collect_date = "collect_date";
                collect_location_id = "collect_location_id";
                void_date = "void_date";
                void_user_id = "void_user_id";
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
                gift_no = "gift_no";
                gift_name = "gift_name";
                location_name = "location_name";
                redemption_status_name = "redemption_status_name";
                member_no = "member_no";
                member_name = "member_name";
                crt_by_name = "crt_by_name";
            }
        }
    }
}
