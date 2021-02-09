using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Palmary.Loyalty.BO.DataTransferObjects.Member;
using Palmary.Loyalty.BO.DataTransferObjects.Wifi;

using Palmary.Loyalty.Common;
using Palmary.Loyalty.BO.Modules.Utility;
using Palmary.Loyalty.BO.Modules.Member;
using Palmary.Loyalty.BO.Modules.Gift;
using Palmary.Loyalty.BO.Modules.Wifi;

namespace Palmary.Loyalty.API
{
    /// <summary>
    /// Summary description for LoyaltyWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class LoyaltyWebService : System.Web.Services.WebService
    {
        [WebMethod]
        public string MemberLogin(string e_member_token, string e_member_password)
        {
            var member_token = CryptographyManager.DecryptAES256_CBC(CommonConstant.Cryptography.tl_key, CommonConstant.Cryptography.tl_iv, e_member_token);
            var member_password = CryptographyManager.DecryptAES256_CBC(CommonConstant.Cryptography.tl_key, CommonConstant.Cryptography.tl_iv, e_member_password);

            var memberManager = new MemberManager();
            var member_id = 0;
            var session = "";
            var result = ""; // memberManager.Login(member_token, member_password, ref member_id, ref session);

            return new { result = result }.ToJson();
        }

        [WebMethod]
        public string MemberLogin_meraki(string e_member_token, string e_member_password, string node_mac, string client_ip, string client_mac)
        {
            var member_token = CryptographyManager.DecryptAES256_CBC(CommonConstant.Cryptography.tl_key, CommonConstant.Cryptography.tl_iv, e_member_token);
            var member_password = CryptographyManager.DecryptAES256_CBC(CommonConstant.Cryptography.tl_key, CommonConstant.Cryptography.tl_iv, e_member_password);

            var memberManager = new MemberManager();
            var member_id = 0;
            var session = "";
            var result = false; // memberManager.Login(member_token, member_password, ref member_id, ref session);
            var msg = "";
            
            if (result)
            {
                // access wifi and add point
                var systemCode = WifiLocationPresence(member_id, node_mac, client_ip, client_mac, ref msg);

                if (systemCode != CommonConstant.SystemCode.normal)
                    result = false;
            }else
                msg = "Invalid Member";

            return new { result = result, msg = msg }.ToJson();
        }

        [WebMethod]
        public string MemberCreateAndLogin_meraki(string e_member_email, string e_member_password, string node_mac, string client_ip, string client_mac)
        {
            var member_email = CryptographyManager.DecryptAES256_CBC(CommonConstant.Cryptography.tl_key, CommonConstant.Cryptography.tl_iv, e_member_email);
            var member_password = CryptographyManager.DecryptAES256_CBC(CommonConstant.Cryptography.tl_key, CommonConstant.Cryptography.tl_iv, e_member_password);

            var member = new MemberObject()
            {
                member_no = "",
                password = member_password,
                email = member_email,
                fbid = "",
                fbemail = "",
                mobile_no = "",
                salutation = 1,
                firstname = "",
                middlename = "",
                lastname = "",
                birth_year = 1900,
                birth_month = 1,
                birth_day = 1,
                gender = 1,
                hkid = "",
                address1 = "",
                address2 = "",
                address3 = "",
                district = 1,
                region = 1,
                reg_source = 1,
                referrer = 0,
                reg_status = 0,
                reg_ip = "",
                activate_key = "",
                hash_key = "",
                session = "",
                status = CommonConstant.Status.active,
                opt_in = 1,
                member_level_id = 1,
                member_category_id = 7,
            };
       

            var sql_remark = "";
            var memberManager = new MemberManager();
            int new_member_id = 0;
            var systemCode = memberManager.Create(member, ref sql_remark, ref new_member_id);

            var msg = "";
            var result = false;

            if (systemCode == CommonConstant.SystemCode.normal)
            {
                // access wifi and add point
                systemCode = WifiLocationPresence(new_member_id, node_mac, client_ip, client_mac, ref msg);

                if (systemCode == CommonConstant.SystemCode.normal)
                    result = true;
            }
            else if (systemCode == CommonConstant.SystemCode.err_email_exist)
                msg = "Registry Fail: Email Duplicate";
            else
                msg = "Registry Fail";

            return new { result = result, msg = msg }.ToJson();
        }

        private CommonConstant.SystemCode WifiLocationPresence(int member_id, string node_mac, string client_ip, string client_mac, ref string result_msg)
        {
            var wifiLocationManager = new WifiLocationManager();
            var systemCode = wifiLocationManager.MemberAccessWifi(member_id, node_mac, client_ip, client_mac);

            if (systemCode != CommonConstant.SystemCode.normal)
            {
                if (systemCode == CommonConstant.SystemCode.err_location_not_exist)
                    result_msg = "Invalid Node Mac Address";
                else if (systemCode == CommonConstant.SystemCode.no_permission)
                    result_msg = "No Permission";
            }

            return systemCode;
        }
    }
}
