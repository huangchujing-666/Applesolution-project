using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Palmary.Loyalty.Web_frontend.Controllers
{
    public class LoyaltyController : Controller
    {
        //
        // GET: /Loyalty/
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        public string Login()
        {
           string email=  Request["loginformname"];
           string pwd = Request["loginformpassword"];
           return "1";
        }

        #region Cookie
        /// <summary>
        /// 获取cookie
        /// </summary>
        /// <param name="Key">键</param>
        /// <returns></returns>
        public string getCookie(string Key)
        {
            HttpCookie memberID = Request.Cookies[Key];
            if (memberID != null)
            {
                return memberID.Value;
            }
            return "";
        }
        /// <summary>
        /// 设置cookie
        /// </summary>
        /// <param name="Key">键</param>
        /// <param name="Value">值</param>
        /// <param name="isSetDay">是否设置过期时间</param>
        /// <param name="day">过期天数</param>
        public void setCookie(string Key,string Value,bool isSetDay,int day=0)
        {
            HttpCookie cookie = new HttpCookie(Key, Value);
            if (isSetDay)
            {
                cookie.Expires = DateTime.Now.AddDays(day);
            }
            Response.Cookies.Add(cookie);
        }


        #endregion

        /// <summary>
        /// 會員資料  
        /// </summary>
        /// <returns></returns>
        public ActionResult Member()
        {
            return View();
        }

        /// <summary>
        /// 換領紀錄 
        /// </summary>
        /// <returns></returns>
        public ActionResult Redeemed()
        {
            return View();
        }

        /// <summary>
        /// 積分記錄
        /// </summary>
        /// <returns></returns>
        public ActionResult Transaction()
        {
            return View();
        }

        /// <summary>
        /// 禮品專區
        /// </summary>
        /// <returns></returns>
        public ActionResult Gift()
        {
            return View();
        }

    }
}
