using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palmary.Loyalty.BO.DataTransferObjects.CSVExport
{
    public class MemberLevelChangeExportObject
    {
        // additional info 
        public string member_no { get; set; }
        public string source_type_name { get; set; }
        public string change_type_name { get; set; }
        public string reason_type_name { get; set; }
        public string old_member_level_name { get; set; }
        public string new_member_level_name { get; set; }
        public string crt_date { get; set; }
        public string remark { get; set; }

        public class Header // should match with TransactionObject
        {
            // CsvHelper Library needs getter, setter
            

            // additional info 
            public string member_no { get; set; }
            public string source_type_name { get; set; }
            public string change_type_name { get; set; }
            public string reason_type_name { get; set; }
            public string old_member_level_name { get; set; }
            public string new_member_level_name { get; set; }
            public string crt_date { get; set; }

            public string remark { get; set; }

            public Header()
            {
                // additional info 
                member_no = "Member No";
                source_type_name = "Source Type";
                change_type_name = "Change Type";
                reason_type_name = "Reason Type";
                old_member_level_name = "Old Member Level";
                new_member_level_name = "New Member Level";
                crt_date = "Date";
                remark = "Remark";
            }
        }
    }
}
