using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Collections.Specialized;
using System.Text;
using Newtonsoft.Json;
using System.Configuration;

namespace Palmary.Loyalty.Web_frontend
{
    public partial class WebForm : System.Web.UI.Page
    {
        public string apiURL = ConfigurationManager.AppSettings["APIServerURL"];
        public string fronEndStartURL = ConfigurationManager.AppSettings["fronEndStartURL"];
        public string fronEndURL = ConfigurationManager.AppSettings["fronEndURL"];
        

        protected void Page_Load(object sender, EventArgs e)
        {

            System.Diagnostics.Debug.WriteLine(apiURL, "apiURL");
        }

        public void scriptPrint(string message)
        {
            System.Diagnostics.Debug.WriteLine(message, "scriptPrint");
        }



        protected void btn_login_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("btn_login_Click");


            string apiURL_login = apiURL + "/MemberLogin.ashx";

            WebClient webClient = new WebClient();

            NameValueCollection formData = new NameValueCollection();
            formData["login_type"] = "normal";
            formData["login_id"] = login_id.Value;
            formData["password"] = password.Value;
            System.Diagnostics.Debug.WriteLine(login_id.Value);

            byte[] responseBytes = webClient.UploadValues(apiURL_login, "POST", formData);
            string resultJSON = Encoding.UTF8.GetString(responseBytes);
            System.Diagnostics.Debug.WriteLine(resultJSON);

            MemberLoginAPIresult apiResult = JsonConvert.DeserializeObject<MemberLoginAPIresult>(resultJSON);
            Response.Write(@"<script language='javascript'>alert('"+resultJSON+"');</script>");
            session.Value = apiResult.session;
        }


        public class MemberLoginAPIresult
        {
            public bool success { get; set; }
            public int member_id { get; set; }
            public string message { get; set; }
            public string session { get; set; }
        }
    }
}