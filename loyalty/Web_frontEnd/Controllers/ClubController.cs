using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Palmary.Loyalty.Common;

using Palmary.Loyalty.BO.DataTransferObjects.Member;
using Palmary.Loyalty.BO.DataTransferObjects.Transaction;
using Palmary.Loyalty.BO.DataTransferObjects.Product;
using Palmary.Loyalty.BO.DataTransferObjects.Gift;
using Palmary.Loyalty.BO.Modules.Member;
using Palmary.Loyalty.BO.Modules.Transaction;
using Palmary.Loyalty.BO.Modules.Product;
using Palmary.Loyalty.BO.Modules.Gift;
using Palmary.Loyalty.BO.Modules.PromotionRule;

using Palmary.Loyalty.Web_frontend.Models;
using Palmary.Loyalty.Web_frontend.Handler;
using Palmary.Loyalty.BO.DataTransferObjects.DB;
using System.Globalization;
using Palmary.Loyalty.BO.Modules.Administration.Security;
using Palmary.Loyalty.BO.Modules.Passcode;
using Palmary.Loyalty.BO.DataTransferObjects.Security;

namespace Palmary.Loyalty.Web_frontend.Controllers
{
    public class ClubController : Controller
    {
        //
        // GET: /Club/
        private LoyaltyModels _loyaltyModel = new LoyaltyModels();

        public ActionResult Index()
        {
            _loyaltyModel.showHeaderSlider = true;
            _loyaltyModel.showIndexInformation = true;
            if (SessionHandler.Current.member_id > 0)
                _loyaltyModel.HasLogin = true;

            return View(_loyaltyModel);
        }

        public ActionResult Register(String Referrer = "")
        {
            return View(_loyaltyModel);
        }

        [HttpPost]
        public string Login(FormCollection collection)
        {
            /*
             * invalidCode
             * 1 - Login success
             * -1 - Invalid data
             * -2 - Login fail
             * -3 - Account not activate
             */
            Boolean formValid = true;
            int invalidCode = 0;

            string loginname = collection["loginname"].ToString().Trim();
            string loginpassword = collection["loginpassword"].ToString().Trim();

            // Perform Form Data Checking
            if (loginname.Length > 0)
                loginname = Server.HtmlEncode(loginname);
            else
            {
                formValid = false;
                invalidCode = invalidCode == 0 ? -1 : invalidCode;
            }

            if (loginpassword.Length > 0)
            //loginpassword = _des.Encryption(Server.HtmlEncode(loginpassword));
            //loginpassword = Server.HtmlEncode(loginpassword);
            { }
            else
            {
                formValid = false;
                invalidCode = invalidCode == 0 ? -1 : invalidCode;
            }

            if (formValid)
            {
                var accessObject = new AccessObject
                {
                    type = CommonConstant.ObjectType.system,
                    id = CommonConstant.SystemObject.cms_bo,
                    actionChannel = CommonConstant.ActionChannel.Web_frontend,
                    ip = "::1",
                    languageID = (int)CommonConstant.LangCode.en

                };

                var accessManager = new AccessManager();
              
                var member_id = 0;
                var session = "";

                var loginResult = accessManager.MemberLogin(accessObject, loginname, loginpassword, ref member_id, ref session);

                if (loginResult)
                {
                    // from bo object to member object
                    accessObject.type = CommonConstant.ObjectType.member;
                    accessObject.id = member_id;

                    var memberManager = new MemberManager(accessObject);

                    invalidCode = 1; //OK
                    var systemCode = CommonConstant.SystemCode.undefine;
                    var member = memberManager.GetDetail(member_id, true, ref systemCode);
                    var loginHandler = new AccessHandler();
                    loginHandler.MemberLogin(member.member_id, member.firstname + " " + member.middlename + " " + member.lastname);

                    SessionManager.Current.obj_language_id = (int)CommonConstant.LangCode.tc;
                }
                else
                    invalidCode = -2;

                //ObjectParameter loginStatus = new ObjectParameter("loginStatus", typeof(Int32));
                //ObjectParameter loginID = new ObjectParameter("loginID", typeof(Int32));

                //_db.MemberLogin(loginname, loginpassword, _lib.GetIPAddress(Request), loginStatus, loginID);

                //invalidCode = Convert.ToInt32(loginStatus.Value);

                //if (invalidCode == 1)
                //{
                //    Session["HasLogin"] = true;
                //    Session["MemberID"] = Convert.ToInt32(loginID.Value);

                //    _oenobiol.GetMemberInfoByID(Convert.ToInt32(loginID.Value));

                //    Session["MemberName"] = String.Format("{0} {1}", _oenobiol.MemberInfo.lastname, _oenobiol.MemberInfo.firstname);
                //}
            }

            return Convert.ToString(invalidCode);
        }

        [HttpPost]
        public string FBLogin(FormCollection collection)
        {
            var fbid = collection["fbid"];
            var email = collection["email"];
            var first_name = "";
            var middle_name = "";
            var last_name ="";
            
            if (collection["first_name"]!= null)
                first_name = collection["first_name"];

            if (collection["middle_name"] != null)
                middle_name = collection["middle_name"];

            if (collection["last_name"] != null)
                last_name = collection["last_name"];
        
            DateTime birthday = new DateTime(1900, 1, 1);
            if (collection["birthday"]!=null)
            {
                var bday_str = collection["birthday"];
                var format = "d";
                CultureInfo provider = CultureInfo.InvariantCulture;

                birthday = DateTime.ParseExact(bday_str, format, provider);
            }
            
            var gender = (collection["gender"] == "male") ? (int)CommonConstant.Gender.male : CommonConstant.Gender.female;
            var salutation = (collection["gender"] == "male") ? (int)CommonConstant.Salutation.mr : CommonConstant.Salutation.miss;
            
            var memberObj = new MemberObject()
            {
                fbid = fbid,
                fbemail = email,
                email = email,
                firstname = first_name,
                middlename = middle_name,
                lastname = last_name,
                birth_year = birthday.Year,
                birth_month = birthday.Month,
                birth_day = birthday.Day,
                gender = gender,
                salutation = salutation,

                // default
                mobile_no = "",
                hkid = "",
                reg_ip = "",
                activate_key = "",
                hash_key = "",
                status = (int)CommonConstant.Status.active,
                member_level_id = 1,
                member_category_id = 7
            };

            var accessObject = new AccessObject
            {
                type = CommonConstant.ObjectType.system,
                id = CommonConstant.SystemObject.cms_bo,
                actionChannel = CommonConstant.ActionChannel.Web_frontend,
                ip = "::1",
                languageID = (int)CommonConstant.LangCode.en
            };
            var memberManager = new MemberManager(accessObject);

            var valid = memberManager.ConnectWithFB(ref memberObj);

            if (valid)
            {
                 var loginHandler = new AccessHandler();
                 loginHandler.MemberLogin(memberObj.member_id, memberObj.GetFullname());
            }
            
            var resultMsg = new {result = true};
            return resultMsg.ToJson();
        }

        public ActionResult Logout()
        {
            SessionHandler.Destroy();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public string SessionExist()
        {
            var sesssionExist = false;

            if (SessionHandler.Current.member_id > 0)
                sesssionExist = true;

            var resultMsg = new { result = sesssionExist };

            return resultMsg.ToJson();
        }

        public void LoadBasicData()
        {
            if (SessionHandler.Current.member_id > 0)
            {
                var accessObject = new AccessObject
                {
                    type = CommonConstant.ObjectType.member,
                    id = SessionHandler.Current.member_id,
                    actionChannel = CommonConstant.ActionChannel.Web_frontend,
                    ip = "::1",
                    languageID = (int)CommonConstant.LangCode.en
                };

                var memberManager = new MemberManager(accessObject);
                var systemCode = CommonConstant.SystemCode.undefine;
                var member = memberManager.GetDetail(SessionHandler.Current.member_id, true, ref systemCode);
                _loyaltyModel.MemberAvailablePoint = member.available_point;
                _loyaltyModel.MemberInfo.memberlevel = member.member_level_name;
                _loyaltyModel.MemberInfo.member_no = member.member_no;
                _loyaltyModel.MemberName = SessionHandler.Current.member_name;

                if (member.birth_day != 0 && member.birth_month != 0 && member.birth_year != 0)
                    _loyaltyModel.MemberInfo.age = new DateTime(member.birth_year, member.birth_month, member.birth_day);

                _loyaltyModel.HasLogin = true;
            }
        }

        public ActionResult Member(int? page)
        {
            LoadBasicData();

            //if (SessionHandler.Current.member_id>0)
            //{
            //    if (page != null && page > 0)
            //        _loyaltyModel.Pager.page = page.Value;
            //    else
            //        _loyaltyModel.Pager.page = 1;

            //    var _transactionManager = new TransactionManager();
            //    var resultCode = CommonConstant.SystemCode.undefine;
            //    var rowTotal = 0;
            //    var searchParmList = new List<SearchParmObject>();

            //    var startRowIndex = 0;
            //    var itemPerPage = 20;
            //    if (_loyaltyModel.Pager.page > 1)
            //        startRowIndex = (_loyaltyModel.Pager.page -1) * itemPerPage;

            //    var dataList = _transactionManager.GetListWithName(SessionHandler.Current.member_id, startRowIndex, itemPerPage, searchParmList, "", CommonConstant.SortOrder.asc, ref resultCode, ref rowTotal).ToList();
            //    if (rowTotal <= itemPerPage)
            //        _loyaltyModel.Pager.totalpage = 1;
            //    else
            //    {
            //        _loyaltyModel.Pager.totalpage = rowTotal / itemPerPage;
            //        if (rowTotal % itemPerPage != 0)
            //            _loyaltyModel.Pager.totalpage++;
            //    }

            //    var resultList = new List<TransactionInfo>();

            //    foreach (var x in dataList)
            //    {
            //        var itemName = "";

            //        if (x.type == (int)CommonConstant.TransactionType.redemption)
            //            itemName = x.type_name + ": " + x.gift_name;
            //        else if (x.type == (int)CommonConstant.TransactionType.promotion_rule)
            //        {
            //            var promotionRuleManager = new PromotionRuleManager();
            //            var r = promotionRuleManager.GetDetail(x.source_id);

            //            itemName = x.type_name + ": " + r.name;
            //        }
            //        else if (x.type == (int)CommonConstant.TransactionType.purchase_product)
            //        {
            //            itemName = x.type_name + " (Order ID: " + x.transaction_id + ")";
            //        }
            //        else
            //            itemName = x.type_name;
                    
            //        var t = new TransactionInfo()
            //        {
            //            Type = "Bonus",
            //            TransactionDate = x.crt_date,
            //            ItemName = itemName,
            //            Point = x.point_change
            //        };

            //        resultList.Add(t);
            //    }
                
            //    _loyaltyModel.Transactions = resultList;
            //}

            if (SessionHandler.Current.member_id > 0)
            {
                return View(_loyaltyModel);
            }
            else
                return RedirectToAction("Index");
        }

        public ActionResult Warranty(int? page)
        {
            LoadBasicData();


            if (SessionHandler.Current.member_id > 0)
            {
                return View(_loyaltyModel);
            }
            else
                return RedirectToAction("Index");
        }


        // p for passcode
        public ActionResult ScanPasscode(string p)
        {
            LoadBasicData();
            _loyaltyModel.passcode = p;

            return View(_loyaltyModel);  
        }

        public ActionResult Product(int? category, int? page)
        {
            LoadBasicData();

            if (category == null)
                category = 0;

            if (SessionHandler.Current.member_id > 0)
            {
                var accessObject = new AccessObject
                {
                    type = CommonConstant.ObjectType.member,
                    id = SessionHandler.Current.member_id,
                    actionChannel = CommonConstant.ActionChannel.Web_frontend,
                    ip = "::1",
                    languageID = (int)CommonConstant.LangCode.en
                };

                if (page != null && page > 0)
                    _loyaltyModel.Pager.page = page.Value;
                else
                    _loyaltyModel.Pager.page = 1;

                var startRowIndex = 0;
                var itemPerPage = 18;
                if (_loyaltyModel.Pager.page > 1)
                    startRowIndex = (_loyaltyModel.Pager.page - 1) * itemPerPage;

                // category button
               
                var productCategoryManager = new ProductCategoryManager(accessObject);
                var systemCode = CommonConstant.SystemCode.undefine;
                var categoryList = productCategoryManager.GetListAll(true, ref systemCode);

                _loyaltyModel.CategoryButtons = "";
                _loyaltyModel.CategoryNum = category.Value;

                foreach (var x in categoryList)
                {
                    var className = "btn btn-category";
                    if (x.category_id == category)
                        className += " active";
                   

                    _loyaltyModel.CategoryButtons += "<a class='" + className + "' href='" + Url.Action("Product", "Club", new { category = x.category_id }) + @"'>" + x.name + "</a>";
                }

                // content
                var productManager = new ProductManager(accessObject);
                var rowTotal = 0;
                List<ProductObject> pList;
                if (category > 0)
                {
                    pList = productManager.GetListByCategory(category.Value, startRowIndex, itemPerPage, true, ref systemCode, ref rowTotal);
                }
                else
                {
                    var searchParmList = new List<SearchParmObject>();
                    pList = productManager.GetList(startRowIndex, itemPerPage, searchParmList, "name", CommonConstant.SortOrder.asc, ref systemCode, ref rowTotal);
                }

                if (rowTotal <= itemPerPage)
                    _loyaltyModel.Pager.totalpage = 1;
                else
                {
                    _loyaltyModel.Pager.totalpage = rowTotal / itemPerPage;
                    if (rowTotal % itemPerPage != 0)
                        _loyaltyModel.Pager.totalpage++;
                }

                var fileHandler = new FileHandler();
               // var productCategoryManager = new ProductCategoryManager();

                foreach (var p in pList)
                {
                    var totalInCat = pList.Where(x => x.category_name == p.category_name).Count();

                    var pp = new ProductPoints()
                    {
                        categoryname = p.category_name,
                        image = fileHandler.GetImagePathFromBackend(p.file_name, p.file_extension, (string)CommonConstant.Module.product, (int)CommonConstant.ImageSizeType.thumb),
                        productname = p.name,
                        point = p.point,
                        rec_count = totalInCat
                    };

                    _loyaltyModel.ProductPoints.Add(pp);
                }

                return View(_loyaltyModel);
            }
            else
                return RedirectToAction("Index");
        }

        public ActionResult Profile()
        {
            LoadBasicData();

            if (SessionHandler.Current.member_id > 0)
            {
                var accessObject = new AccessObject
                {
                    type = CommonConstant.ObjectType.member,
                    id = SessionHandler.Current.member_id,
                    actionChannel = CommonConstant.ActionChannel.Web_frontend,
                    ip = "::1",
                    languageID = (int)CommonConstant.LangCode.en
                };

                var memberManager = new MemberManager(accessObject);
                var systemCode = CommonConstant.SystemCode.undefine;
                var m = memberManager.GetDetail(SessionHandler.Current.member_id, true, ref systemCode);

                _loyaltyModel.MemberInfo.lastname = m.lastname;
                _loyaltyModel.MemberInfo.firstname = m.middlename + " " + m.firstname;

                if (m.gender == (int)CommonConstant.Gender.male)
                    _loyaltyModel.MemberInfo.gender = "M";
                else
                    _loyaltyModel.MemberInfo.gender = "F";

                if (m.birth_year == 1900)
                    _loyaltyModel.MemberInfo.age = null;
                else
                    _loyaltyModel.MemberInfo.age = new DateTime(m.birth_year, m.birth_month, m.birth_day);

                _loyaltyModel.MemberInfo.mobile = m.mobile_no;
                _loyaltyModel.MemberInfo.email = m.email;
                _loyaltyModel.MemberInfo.address1 = m.address1;
                _loyaltyModel.MemberInfo.address2 = m.address2;
                _loyaltyModel.MemberInfo.hkid = m.hkid;

                return View(_loyaltyModel);
            }
            else
                return RedirectToAction("Index");
        }

        public ActionResult Transaction(int? page)
        {
            LoadBasicData();

            if (SessionHandler.Current.member_id > 0)
            {
                var accessObject = new AccessObject
                {
                    type = CommonConstant.ObjectType.member,
                    id = SessionHandler.Current.member_id,
                    actionChannel = CommonConstant.ActionChannel.Web_frontend,
                    ip = "::1",
                    languageID = (int)CommonConstant.LangCode.en
                };

                if (page != null && page > 0)
                    _loyaltyModel.Pager.page = page.Value;
                else
                    _loyaltyModel.Pager.page = 1;

                var _transactionManager = new TransactionManager(accessObject);
                var resultCode = CommonConstant.SystemCode.undefine;
                var rowTotal = 0;
                var searchParmList = new List<SearchParmObject>();

                var startRowIndex = 0;
                var itemPerPage = 20;
                if (_loyaltyModel.Pager.page > 1)
                    startRowIndex = (_loyaltyModel.Pager.page - 1) * itemPerPage;

                var dataList = _transactionManager.GetListWithName(SessionHandler.Current.member_id, startRowIndex, itemPerPage, searchParmList, "", CommonConstant.SortOrder.asc, ref resultCode, ref rowTotal).ToList();
                if (rowTotal <= itemPerPage)
                    _loyaltyModel.Pager.totalpage = 1;
                else
                {
                    _loyaltyModel.Pager.totalpage = rowTotal / itemPerPage;
                    if (rowTotal % itemPerPage != 0)
                        _loyaltyModel.Pager.totalpage++;
                }

                var resultList = new List<TransactionInfo>();

                foreach (var x in dataList)
                {
                    var itemName = "";

                    if (x.type == (int)CommonConstant.TransactionType.redemption)
                        itemName = "換領禮品: " + x.gift_name;
                    else if (x.type == (int)CommonConstant.TransactionType.promotion_rule)
                    {
                        var promotionRuleManager = new PromotionRuleManager(accessObject);
                        var r = promotionRuleManager.GetDetail(x.source_id);

                        if (x.point_change>0)
                            itemName = "額外獎賞: " + r.name;
                        else
                            itemName = "扣除獎賞: " + r.name;
                    }
                    else if (x.type == (int)CommonConstant.TransactionType.purchase_product)
                    {
                        var systemCode = CommonConstant.SystemCode.undefine;
                        var productPurchaseManager = new ProductPurchaseManager(accessObject);
                        var thePurchase = productPurchaseManager.GetDetailByTransaction(x.transaction_id, ref systemCode);
                        
                        itemName = "購買產品: " + thePurchase.product_name+", 數量: "+thePurchase.quantity;
                    }
                    else
                        itemName = x.type_name;

                    var t = new TransactionInfo()
                    {
                        Type = "Bonus",
                        TransactionDate = x.crt_date,
                        ItemName = itemName,
                        Point = x.point_change
                    };

                    resultList.Add(t);
                }

                _loyaltyModel.Transactions = resultList;

                return View(_loyaltyModel);
            }
            else
                return RedirectToAction("Index");
        }

        public ActionResult WarrantyTransaction(int? page)
        {
            LoadBasicData();

            if (SessionHandler.Current.member_id > 0)
            {
                var accessObject = new AccessObject
                {
                    type = CommonConstant.ObjectType.member,
                    id = SessionHandler.Current.member_id,
                    actionChannel = CommonConstant.ActionChannel.Web_frontend,
                    ip = "::1",
                    languageID = (int)CommonConstant.LangCode.en
                };

                if (page != null && page > 0)
                    _loyaltyModel.Pager.page = page.Value;
                else
                    _loyaltyModel.Pager.page = 1;

                var _transactionManager = new TransactionManager(accessObject);
                var resultCode = CommonConstant.SystemCode.undefine;
                var rowTotal = 0;
                var searchParmList = new List<SearchParmObject>();

                var startRowIndex = 0;
                var itemPerPage = 20;
                if (_loyaltyModel.Pager.page > 1)
                    startRowIndex = (_loyaltyModel.Pager.page - 1) * itemPerPage;

                var dataList = _transactionManager.GetListWithName(SessionHandler.Current.member_id, startRowIndex, itemPerPage, searchParmList, "", CommonConstant.SortOrder.asc, ref resultCode, ref rowTotal).ToList();
                if (rowTotal <= itemPerPage)
                    _loyaltyModel.Pager.totalpage = 1;
                else
                {
                    _loyaltyModel.Pager.totalpage = rowTotal / itemPerPage;
                    if (rowTotal % itemPerPage != 0)
                        _loyaltyModel.Pager.totalpage++;
                }

                var resultList = new List<TransactionInfo>();

                foreach (var x in dataList)
                {
                    var itemName = "";

                    if (x.type == (int)CommonConstant.TransactionType.redemption)
                        itemName = "換領禮品: " + x.gift_name;
                    else if (x.type == (int)CommonConstant.TransactionType.promotion_rule)
                    {
                        var promotionRuleManager = new PromotionRuleManager(accessObject);
                        var r = promotionRuleManager.GetDetail(x.source_id);

                        if (x.point_change > 0)
                            itemName = "額外獎賞: " + r.name;
                        else
                            itemName = "扣除獎賞: " + r.name;
                    }
                    else if (x.type == (int)CommonConstant.TransactionType.purchase_product)
                    {
                        var systemCode = CommonConstant.SystemCode.undefine;
                        var productPurchaseManager = new ProductPurchaseManager(accessObject);
                        var thePurchase = productPurchaseManager.GetDetailByTransaction(x.transaction_id, ref systemCode);

                        itemName = "購買產品: " + thePurchase.product_name + ", 數量: " + thePurchase.quantity;
                    }
                    else
                        itemName = x.type_name;

                    var t = new TransactionInfo()
                    {
                        Type = "Bonus",
                        TransactionDate = x.crt_date,
                        ItemName = itemName,
                        Point = x.point_change
                    };

                    resultList.Add(t);
                }

                _loyaltyModel.Transactions = resultList;

                return View(_loyaltyModel);
            }
            else
                return RedirectToAction("Index");
        }

        public ActionResult Gift(int? page, int? category)
        {
            LoadBasicData();

            if (category == null)
                category = 0;

            if (SessionHandler.Current.member_id > 0)
            {
                var accessObject = new AccessObject
                {
                    type = CommonConstant.ObjectType.member,
                    id = SessionHandler.Current.member_id,
                    actionChannel = CommonConstant.ActionChannel.Web_frontend,
                    ip = "::1",
                    languageID = (int)CommonConstant.LangCode.en
                };

                if (page != null && page > 0)
                    _loyaltyModel.Pager.page = page.Value;
                else
                    _loyaltyModel.Pager.page = 1;

                var giftManager = new GiftManager(accessObject);

                var resultCode = CommonConstant.SystemCode.undefine;
                

                var startRowIndex = 0;
                var itemPerPage = 18;
                if (_loyaltyModel.Pager.page > 1)
                    startRowIndex = (_loyaltyModel.Pager.page - 1) * itemPerPage;

                // category button
                var giftCategoryManager = new GiftCategoryManager(accessObject);
                var systemCode = CommonConstant.SystemCode.undefine;
                var categoryList = giftCategoryManager.GetListParent(true, ref systemCode);

                _loyaltyModel.CategoryButtons = "";
                _loyaltyModel.CategoryNum = category.Value;

                foreach (var x in categoryList)
                {
                    var className = "btn btn-category";
                    if (x.category_id == category)
                        className += " active";

                    _loyaltyModel.CategoryButtons += "<a class='" + className + "' href='" + Url.Action("Gift", "Club", new { category = x.category_id }) + @"'>" + x.name + "</a>";
                }

                // content
                var rowTotal = 0;
                List<GiftObject> dataList;
                if (category > 0)
                    dataList = giftManager.GetListByCategory(category.Value, startRowIndex, itemPerPage, true, ref systemCode, ref rowTotal);
                else
                {
                    var searchParmList = new List<SearchParmObject>();
                    dataList = giftManager.GetList(startRowIndex, itemPerPage, searchParmList, "name", CommonConstant.SortOrder.asc, ref systemCode, ref rowTotal);
                }

                if (rowTotal <= itemPerPage)
                    _loyaltyModel.Pager.totalpage = 1;
                else
                {
                    _loyaltyModel.Pager.totalpage = rowTotal / itemPerPage;
                    if (rowTotal % itemPerPage != 0)
                        _loyaltyModel.Pager.totalpage++;
                }

                var fileHandler = new FileHandler();
                var resultList = dataList.Select(x => new Gift()
                {
                    id = x.gift_id,
                    image = fileHandler.GetImagePathFromBackend(x.file_name, x.file_extension, (string)CommonConstant.Module.gift, (int)CommonConstant.ImageSizeType.thumb),
                    name = x.name,
                    mark = x.point.ToString()
                }).ToList();

                _loyaltyModel.Gifts = resultList;

                return View(_loyaltyModel);
            }
            else
                return RedirectToAction("Index");
        }

        public ActionResult Referral()
        {
            return RedirectToAction("Member");
        }


        public ActionResult Promotion()
        {
            return RedirectToAction("Member");
        }

        public ActionResult Contact_Us()
        {
            return RedirectToAction("Member");
        }

        [HttpPost]
        public string Register(FormCollection collection)
        {
            var email = collection["email"];

            var first_name = collection["firstname"];
            var middle_name = "";
            var last_name = collection["lastname"];

            var birth_year = 1900;
            var birth_month = 1;
            var birth_day = 1;

            if (!String.IsNullOrEmpty(collection["age"]))
            {
                DateTime birthday = DateTime.Parse(collection["age"]);
                birth_year = birthday.Year;
                birth_month = birthday.Month;
                birth_day = birthday.Day;
            }
            
            var gender = (collection["gender"] == "M") ? (int)CommonConstant.Gender.male : CommonConstant.Gender.female;
            var salutation = (collection["gender"] == "M") ? (int)CommonConstant.Salutation.mr : CommonConstant.Salutation.miss;

            var mobile = collection["mobile"];

            var address1 = collection["address1"];
            var address2 = collection["address2"];

            var hkid = collection["hkid"];
            var password = collection["password"];

            var memberObj = new MemberObject()
            {
                fbid = "",
                fbemail = "",
                email = email,
                firstname = first_name,
                middlename = middle_name,
                lastname = last_name,
                birth_year = birth_year,
                birth_month = birth_month,
                birth_day = birth_day,
                gender = gender,
                salutation = salutation,
                mobile_no = mobile,
                hkid = hkid,
                password = password,
                address1 = address1,
                address2 = address2,

                // default
                
                reg_ip = "",
                activate_key = "",
                hash_key = "",
                status = (int)CommonConstant.Status.active,
                member_level_id = 1,
                member_category_id = 7
            };

            var accessObject = new AccessObject
            {
                type = CommonConstant.ObjectType.system,
                id = CommonConstant.SystemObject.cms_bo,
                actionChannel = CommonConstant.ActionChannel.Web_frontend,
                ip = "::1",
                languageID = (int)CommonConstant.LangCode.en
            };
            var memberManager = new MemberManager(accessObject);

            var sql_remark = "";
            var new_member_id = 0;
            var systemCode = memberManager.Create(memberObj, ref sql_remark, ref new_member_id);

            var resultCode = 0;
            if (systemCode == CommonConstant.SystemCode.normal)
                resultCode = 1;
            else if (systemCode == CommonConstant.SystemCode.err_email_exist)
                resultCode = -6;

            var resultMsg = new { result = resultCode };
            return resultMsg.ToJson();
        }

        
        public ActionResult RegisterSuccess()
        {
            return View(_loyaltyModel);
        }

        [HttpPost]
        public string RegisterPasscode(FormCollection collection)
        {
            var accessObject = new AccessObject
            {
                type = CommonConstant.ObjectType.member,
                id = SessionHandler.Current.member_id,
                actionChannel = CommonConstant.ActionChannel.Web_frontend,
                ip = "::1",
                languageID = (int)CommonConstant.LangCode.en
            };

            var pin_value = collection["pin_value"].ToUpper();
            int quantity = 0;
            var parseResult = int.TryParse(collection["quantity"], out quantity);

            var systemCode = CommonConstant.SystemCode.undefine;

            if (!parseResult)
                systemCode = CommonConstant.SystemCode.err_passcodeQuantityInvalid;
            else if (quantity <= 0)
                systemCode = CommonConstant.SystemCode.err_passcodeQuantityInvalid;
            else if (String.IsNullOrEmpty(pin_value))
                systemCode = CommonConstant.SystemCode.err_passcodeInvalid;
            else
            {
                var productPurchaseManager = new ProductPurchaseManager(accessObject);
                systemCode = productPurchaseManager.PurchaseByPasscode(SessionHandler.Current.member_id, pin_value, quantity);
            }

            var result = new { resultCode = systemCode };
            return result.ToJson();
        }

        [HttpPost]
        public string UpdateMember(FormCollection collection)
        {
            var accessObject = new AccessObject
            {
                type = CommonConstant.ObjectType.member,
                id = SessionHandler.Current.member_id,
                actionChannel = CommonConstant.ActionChannel.Web_frontend,
                ip = "::1",
                languageID = (int)CommonConstant.LangCode.en
            };

            var mobile = collection["mobile"];

            var address1 = collection["address1"];
            var address2 = collection["address2"];

            
            var password = collection["password"];

            var memberManager = new MemberManager(accessObject);
            var systemCode = CommonConstant.SystemCode.undefine;
            var m = memberManager.GetDetail(SessionHandler.Current.member_id, true, ref systemCode);

            m.mobile_no = mobile;
            m.address1 = address1;
            m.address2 = address2;

            if (!String.IsNullOrEmpty(password))
                m.password = password;

            var sql_remark = "";
            var result = memberManager.Update(m, null, ref sql_remark);

            var resultCode = 0;
            if (result)
                resultCode = 1;
            else
                resultCode = -1;

            var resultMsg = new { result = resultCode };
            return resultMsg.ToJson();
        }

        public ActionResult ProfileChangeSuccess()
        {
            return View(_loyaltyModel);
        }

        public ActionResult PasscodeSuccess()
        {
            return View(_loyaltyModel);
        }

        [HttpPost]
        public string Redeem(FormCollection collection)
        {

            var resultCode = -5;
            var message = "";

            if (SessionHandler.Current.member_id > 0 && !String.IsNullOrEmpty(collection["id"]))
            {
                var accessObject = new AccessObject
                {
                    type = CommonConstant.ObjectType.member,
                    id = SessionHandler.Current.member_id,
                    actionChannel = CommonConstant.ActionChannel.Web_frontend,
                    ip = "::1",
                    languageID = (int)CommonConstant.LangCode.en
                };

                var redeemList = new List<GiftRedemptionObject>();

                var gift_id = int.Parse(collection["id"]);
                var quantity = 1;
                var location_id = 1;

                if (gift_id > 0 && quantity > 0)
                {
                    var giftRedemption = new GiftRedemptionObject()
                    {
                        redemption_id = 0,

                        redemption_channel = (int)CommonConstant.GiftRedeemChannel.web,
                        member_id = SessionHandler.Current.member_id,
                        gift_id = gift_id,
                        quantity = quantity,

                        redemption_status = (int)CommonConstant.GiftRedeemStatus.waiting_collect,
                        collect_date = null,
                        collect_location_id = location_id,
                        void_date = null,
                        void_user_id = null,
                        remark = null,
                        status = (int)CommonConstant.Status.active
                    };
                    redeemList.Add(giftRedemption);
                }

                if (redeemList.Count() > 0)
                {
                    var giftRedemptionManager = new GiftRedemptionManager(accessObject);
                  
                    var redeemFlag = giftRedemptionManager.Redeem(redeemList, ref message);

                    if (redeemFlag)
                    {
                        var giftManager = new GiftManager(accessObject);
                        var g = giftManager.GetDetail(gift_id);

                        TempData["PointUse"] = Convert.ToString(g.point);
                        TempData["GiftName"] = Convert.ToString(g.name);

                        resultCode = 1;
                    }
                }
            }
           
            var resultMsg = new { result = resultCode, msg = message };
            return resultMsg.ToJson();
        }

        public ActionResult Gift_Redeem_Success()
        {
            LoadBasicData();

            if (SessionHandler.Current.member_id > 0)
            {
                 return View(_loyaltyModel);
            }
            else
                return RedirectToAction("Index");
        }
    }
}
