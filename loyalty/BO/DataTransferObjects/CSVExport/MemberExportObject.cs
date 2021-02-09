using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palmary.Loyalty.BO.DataTransferObjects.CSVExport
{
    public class MemberExportObject
    {
        public class Header // should match with MemberObject
        {
            // CsvHelper Library needs getter, setter
            public string member_id { get; set; }
            public string member_no { get; set; }
            public string password { get; set; }
            public string email { get; set; }
            public string fbid { get; set; }
            public string fbemail { get; set; }
            public string mobile_no { get; set; }
            public string salutation { get; set; }
            public string firstname { get; set; }
            public string middlename { get; set; }
            public string lastname { get; set; }
            public string birth_year { get; set; }
            public string birth_month { get; set; }
            public string birth_day { get; set; }
            public string gender { get; set; }
            public string hkid { get; set; }
            public string address1 { get; set; }
            public string address2 { get; set; }
            public string address3 { get; set; }
            public string district { get; set; }
            public string region { get; set; }
            public string reg_source { get; set; }
            public string referrer { get; set; }
            public string reg_status { get; set; }
            public string reg_ip { get; set; }
            public string activate_key { get; set; }
            public string hash_key { get; set; }
            public string session { get; set; }
            public string status { get; set; }
            public string opt_in { get; set; }
            public string member_level_id { get; set; }
            public string member_category_id { get; set; }
            public string available_point { get; set; }
            public string crt_date { get; set; }
            public string upd_date { get; set; }
            public string crt_by_type { get; set; }
            public string crt_by { get; set; }
            public string upd_by_type { get; set; }
            public string upd_by { get; set; }
            public string record_status { get; set; }

            // additional info
            public string fullname { get; set; }
            public string member_level_name { get; set; }

            public Header()
            {
                member_id = "member_id";
                member_no = "member_no";
                password = "password";
                email = "email";
                fbid = "fbid";
                fbemail = "fbemail";
                mobile_no = "mobile_no";
                salutation = "salutation";
                firstname = "firstname";
                middlename = "middlename";
                lastname = "lastname";
                birth_year = "birth_year";
                birth_month = "birth_month";
                birth_day = "birth_day";
                gender = "gender";
                hkid = "hkid";
                address1 = "address1";
                address2 = "address2";
                address3 = "address3";
                district = "district";
                region = "region";
                reg_source = "reg_source";
                referrer = "referrer";
                reg_status = "reg_status";
                reg_ip = "reg_ip";
                activate_key = "activate_key";
                hash_key = "hash_key";
                session = "session";
                status = "status";
                opt_in = "opt_in";
                member_level_id = "member_level_id";
                member_category_id = "member_category_id";
                available_point = "available_point";
                crt_date = "crt_date";
                upd_date = "upd_date";
                crt_by_type = "crt_by_type";
                crt_by = "crt_by";
                upd_by_type = "upd_by_type";
                upd_by = "upd_by";
                record_status = "record_status";

                //additional
                fullname = "fullname";
                member_level_name = "member_level_name";

            }
        }

    }
}
