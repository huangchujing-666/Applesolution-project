using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Palmary.Loyalty.Web_frontend.Models
{
    public class LoyaltyModels
    {
        public Boolean HasLogin { get; set; }
        public Boolean IsMobileDevice { get; set; }
        public int CategoryNum { get; set; }
        public List<Category> Categories { get; set; }

        public List<Banner> TopBanners { get; set; }
        public List<Banner> LeftBanners { get; set; }
        public List<Banner> RightBanners { get; set; }

        public List<Banner> LoginBanners { get; set; }
        public List<Banner> RollingBanners { get; set; }
        
        public List<Category> CategoryBtns { get; set; }
        public List<News> NewsRecords { get; set; }

        public List<Product> Products { get; set; }
        public string Referrer { get; set; }
        public string MemberName { get; set; }

        public double MemberAvailablePoint { get; set; }
        public double MemberAvailablePointCurYear { get; set; }
        public double MemberAvailablePointNextYear { get; set; }

        public Pager Pager { get; set; }

        public List<TransactionInfo> Transactions { get; set; }
        public List<ProductPoints> ProductPoints { get; set; }
        public Member MemberInfo { get; set; }

        public List<Gift> Gifts { get; set; }
        public string CategoryButtons { get; set; }
        public string siteVersion { get; set; }

        public Boolean showHeaderSlider { get; set; }
        public Boolean showIndexInformation { get; set; }

        public string passcode { get; set; }

        public LoyaltyModels()
        {
            HasLogin = false;
            IsMobileDevice = false;
            showHeaderSlider = false;
            CategoryNum = 0;

            TopBanners = new List<Banner>();
            LeftBanners = new List<Banner>();
            RightBanners = new List<Banner>();

            LoginBanners = new List<Banner>();
            RollingBanners = new List<Banner>();

            Categories = new List<Category>();
            CategoryBtns = new List<Category>();
            NewsRecords = new List<News>();

            Products = new List<Product>();

            Referrer = "";
            MemberName = "";
            MemberAvailablePointCurYear = 0;
            MemberAvailablePointNextYear = 0;

            Pager = new Pager();

            Transactions = new List<TransactionInfo>();
            ProductPoints = new List<ProductPoints>();

            MemberInfo = new Member();
            Gifts = new List<Gift>();

            siteVersion = ConfigurationManager.AppSettings["siteVersion"];

        }

        public bool CheckSessionExpired()
        {
            bool sessionExpired = false;

            //if (HttpContext.Current.Session != null)
            //{
            //    if (HttpContext.Current.Session.IsNewSession)
            //    {
            //        var sessionCookie = HttpContext.Current.Request.Headers["Cookie"];
            //        if ((sessionCookie != null) && (sessionCookie.IndexOf("ASP.NET_SessionId") >= 0))
            //        {
            //            // redirect to login
            //            sessionExpired = true;
            //        }
            //    }
            //}

            return sessionExpired;
        }
    }

    public class Banner
    {
        public string link { get; set; }
        public string title { get; set; }
        public string file_path { get; set; }
    }

    public class Category
    {
        public string page { get; set; }
        public string chiname { get; set; }
        public string introduction { get; set; }
        public string image { get; set; }

        public string name { get; set; }
        public string color { get; set; }
    }

    public class News
    {
        public DateTime postdate { get; set; }
        public string id { get; set; }
        public string title { get; set; }
    }

    public class Product
    {
        
        public string id { get; set; }
        public string name { get; set; }
    }

    public class Referrer
    {
        public DateTime postdate { get; set; }
        public string id { get; set; }
        public string title { get; set; }
    }

    public class Pager
    {
        public int page { get; set; }
        public int totalpage { get; set; }
        public int pageset { get; set; }
        public int total { get; set; }

        public Pager()
        {
            page = 1;
            totalpage = 1;
            pageset = 1;
            total = 0;
        }
    }

    public class TransactionInfo
    {
        public string Type { get; set; }
        public string Image { get; set; }
        public DateTime TransactionDate { get; set; }
        public string ItemName { get; set; }
        public double Point { get; set; }
    }

    public class ProductPoints
    {
        public string categoryname { get; set; }
        public int rec_count { get; set; }
        public string image { get; set; }
        public double point { get; set; }
        public string productname { get; set; }
    }

    public class Member
    {
        public string mobile { get; set; }
        public string email { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public DateTime? age { get; set; }
        public string gender { get; set; }
        public string hkid { get; set; }
        public string member_no { get; set; }

        public string memberlevel { get; set; }
    }

    public class Gift
    {
        public string name { get; set; }
        public string mark { get; set; }
        public string image { get; set; }
        public int id { get; set; }
    }
}