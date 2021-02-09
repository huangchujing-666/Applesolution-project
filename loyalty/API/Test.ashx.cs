using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Net;
using System.Xml;
using System.IO;
using Palmary.Loyalty.BO.Modules.Utility;
using Palmary.Loyalty.Common;
using Palmary.Loyalty.BO.Modules.Administration.Security;

namespace Palmary.Loyalty.API
{
    /// <summary>
    /// Summary description for Test
    /// </summary>
    public class Test : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            var org_string = "member@MC0001.com";
            var test = "zAKo4mRIx6UrnpRuh5BQgCerebliSm8f0FKjqByoMdU%3D";

            //var encrypted = CryptographyManager.EncryptRJ256_CBC(CommonConstant.Cryptography.tl_Key, CommonConstant.Cryptography.tl_IV, org_string);
            // var member_token = CryptographyManager.DecryptRJ256_CBC(CommonConstant.Cryptography.tl_Key, CommonConstant.Cryptography.tl_IV, test);

           // var encrypted = CryptographyManager.EncryptRJ256(CommonConstant.Cryptography.tl_Key, CommonConstant.Cryptography.tl_IV, org_string);

            //var member_token = CryptographyManager.DecryptRJ256(CommonConstant.Cryptography.tl_Key, CommonConstant.Cryptography.tl_IV, e_member_token);
            //var member_password = CryptographyManager.DecryptRJ256(CommonConstant.Cryptography.tl_Key, CommonConstant.Cryptography.tl_IV, e_member_password);
            
            //CallWebService();

            //DebugManager.LogFileWriteln("OK leo test");

            context.Response.Write("Hello World <br/> encrypted: "); // + encrypted);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        // test
        public static void CallWebService()
        {
            var _url = "http://localhost:65418/MobileWebService.svc?wsdl";
            //var _url = "http://dev13.palmary.hk/loyalty/api/MobileWebService.svc?wsdl";
            //var _action = "http://dev13.palmary.hk/loyalty/api/IMobileWebService/GetMemberDetail";
            var _action = "http://tempuri.org/IMobileWebService/MemberLogin";

            XmlDocument soapEnvelopeXml = CreateSoapEnvelope();
            HttpWebRequest webRequest = CreateWebRequest(_url, _action);
            InsertSoapEnvelopeIntoWebRequest(soapEnvelopeXml, webRequest);

            // begin async call to web request.
            IAsyncResult asyncResult = webRequest.BeginGetResponse(null, null);

            // suspend this thread until call is complete. You might want to
            // do something usefull here like update your UI.
            asyncResult.AsyncWaitHandle.WaitOne();

            // get the response from the completed web request.
            string soapResult;
            using (WebResponse webResponse = webRequest.EndGetResponse(asyncResult))
            {
                using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
                {
                    soapResult = rd.ReadToEnd();
                }
                System.Diagnostics.Debug.WriteLine("soapResult: "+ soapResult);
                Console.Write(soapResult);
            }
        }

        // request API [start]
        private static XmlDocument CreateSoapEnvelope()
        {
            XmlDocument soapEnvelop = new XmlDocument();
            soapEnvelop.LoadXml(@"<?xml version='1.0' encoding='UTF-8'?><soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' xmlns:tem='http://tempuri.org/'><soapenv:Header/><soapenv:Body>
                <tem:MemberLogin>
                    <tem:loginToken>MC0001</tem:loginToken>
                    <tem:password>123456</tem:password>
                </tem:MemberLogin>
                </soapenv:Body></soapenv:Envelope>");
            return soapEnvelop;
        }

        private static HttpWebRequest CreateWebRequest(string url, string action)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.Headers.Add("SOAPAction", action);
            webRequest.ContentType = "text/xml;charset=\"utf-8\"";
            webRequest.Accept = "text/xml";
            webRequest.Method = "POST";
            return webRequest;
        }

        private static void InsertSoapEnvelopeIntoWebRequest(XmlDocument soapEnvelopeXml, HttpWebRequest webRequest)
        {
            using (Stream stream = webRequest.GetRequestStream())
            {
                soapEnvelopeXml.Save(stream);
            }
        }

        // request API [END]
    }
}