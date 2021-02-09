using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palmary.Loyalty.BO.DataTransferObjects.Member
{
    public class MemberObject
    {
        public int member_id { get; set; }
        public string member_no { get; set; }
        public string password { get; set; }
        public string email { get; set; }
        public string fbid { get; set; }
        public string fbemail { get; set; }
        public string mobile_no { get; set; }
        public int salutation { get; set; }
        public string firstname { get; set; }
        public string middlename { get; set; }
        public string lastname { get; set; }
        public int birth_year { get; set; }
        public int birth_month { get; set; }
        public int birth_day { get; set; }
        public int gender { get; set; }
        public string hkid { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string address3 { get; set; }
        public int district { get; set; }
        public int region { get; set; }
        public int reg_source { get; set; }
        public int? referrer { get; set; }
        public int reg_status { get; set; }
        public string reg_ip { get; set; }
        public string activate_key { get; set; }
        public string hash_key { get; set; }
        public string session { get; set; }
        public int status { get; set; }
        public int opt_in { get; set; }
        public int member_level_id { get; set; }
        public int member_category_id { get; set; }
        public double available_point { get; set; }
        public DateTime crt_date { get; set; }
        public DateTime upd_date { get; set; }
        public int crt_by_type { get; set; }
        public int crt_by { get; set; }
        public int upd_by_type { get; set; }
        public int upd_by { get; set; }
        public int record_status { get; set; }

        // additional info
        public string fullname { get; set; }
        public string referrer_member_no { get; set; }

        public string GetFullname() {

            string[] tempList = { firstname, middlename, lastname};
            List<string> nameList = new List<string>();

            foreach (var t in tempList)
            {
                if (!string.IsNullOrEmpty(t))
                    nameList.Add(t);
            }

            fullname = String.Join(" ", nameList); 

            return fullname;
        }

        public string member_level_name { get; set; }
    }
}
