using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Palmary.Loyalty.Common;
using Palmary.Loyalty.BO.DataTransferObjects.Member;
using Palmary.Loyalty.BO.Modules.Member;
using Palmary.Loyalty.BO.DataTransferObjects.Security;
using Palmary.Loyalty.BO.Modules.PointEngine;
using Palmary.Loyalty.BO.DataTransferObjects.CombineRedemption;
using Palmary.Loyalty.BO.Modules.CombineRedemption;
using Palmary.Loyalty.BO.Modules.Transaction;
using Palmary.Loyalty.BO.Modules.Utility;
using Palmary.Loyalty.BO.Modules.Administration.Security;
using Newtonsoft.Json;
using System.Net;
using System.ServiceModel.Channels;
using Palmary.Loyalty.BO.DataTransferObjects.Gift;
using Palmary.Loyalty.BO.Modules.Gift;
using System.Configuration;
using System.Reflection;
using Palmary.Loyalty.BO.DataTransferObjects.DB;
using Palmary.Loyalty.BO.Modules.Product;

namespace Palmary.Loyalty.API
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "MobileWebService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select MobileWebService.svc or MobileWebService.svc.cs at the Solution Explorer and start debugging.
    public class MobileWebService : IMobileWebService
    {
        public AccessObject _accessObject;

        public MobileWebService()
        {
            _accessObject = new AccessObject
            {
                type = CommonConstant.ObjectType.system,
                id = CommonConstant.SystemObject.cms_bo,
                actionChannel = CommonConstant.ActionChannel.mobile_app_frontend,
                ip = "::1"
            };
        }

        public void UpdateAccessObject(int objectType, int objectID, string objectIP)
        {
            _accessObject.type = objectType;
            _accessObject.id = objectID;
            _accessObject.ip = objectIP;
        }

        public string HelloWorld()
        {
            return "Hello World Loyalty";
        }

        public MemberObject GetMemberDetail()
        {
            var accessObject = new AccessObject
            {
                type = CommonConstant.ObjectType.member,
                id = 213,
                actionChannel = CommonConstant.ActionChannel.mobile_app_frontend,
                ip = "::1"
            };

            var memberManager = new MemberManager(accessObject);
            var systemCode = CommonConstant.SystemCode.undefine;

            var member = memberManager.GetDetail(213, true, ref systemCode);
            return member;
        }

        public void NewGetMemberDetail(string eContent, out int resultCode, out string resultContent)
        {

            var dataInvalid = false;
            resultContent = "";
            resultCode = (int)CommonConstant.SystemCode.undefine;
            eContent = CryptographyManager.DecryptAES256_CBC(CommonConstant.Cryptography.tl_key, CommonConstant.Cryptography.tl_iv, eContent);
            if (!String.IsNullOrEmpty(eContent))
            {
                dynamic theJson = JsonConvert.DeserializeObject(eContent);

                // check and bind input data
                // required fields
                if (string.IsNullOrEmpty((string)theJson.memberID))
                    dataInvalid = true;
                else
                {
                    var processDateTime = DateTime.Now;
                    var requestTimestamp = DateTime.ParseExact((string)theJson.requestTimestamp, "yyyyMMddHHmmssfff", System.Globalization.CultureInfo.InvariantCulture);

                    if ((processDateTime - requestTimestamp).TotalMinutes > 5)
                    {
                        resultCode = (int)CommonConstant.SystemCode.dateTime_invalid;
                    }
                    else
                    {

                        var memberID = (string)theJson.memberID ?? "";
                        int memberIDd = -1;

                        int.TryParse(memberID, out memberIDd);

                        if (string.IsNullOrEmpty(memberID) || memberIDd == 0)
                        {
                            resultCode = (int)CommonConstant.SystemCode.data_invalid;
                        }
                        else
                        {


                            var accessObject = new AccessObject
                  {
                      type = CommonConstant.ObjectType.member,
                      id = memberIDd,
                      actionChannel = CommonConstant.ActionChannel.mobile_app_frontend,
                      ip = "::1"
                  };

                            var memberManager = new MemberManager(accessObject);
                            var systemCode = CommonConstant.SystemCode.undefine;

                            var member = memberManager.GetDetail(memberIDd, true, ref systemCode);


                            if (member != null)
                            {

                                resultCode = (int)systemCode;

                                resultContent = new
                                {
                                    memberID = member.member_id,
                                    memberCode = member.member_no,
                                    email = member.email,
                                    fbMainEmail = member.fbemail,
                                    fbid = member.fbid,
                                    name = member.GetFullname(),
                                    firstName = member.firstname,
                                    middleName = member.middlename,
                                    lastName = member.lastname,
                                    gender = member.gender,
                                    birthYear = member.birth_year,
                                    birthMonth = member.birth_month,
                                    birthDay = member.birth_day,
                                    point = member.available_point,

                                    processTimestamp = processDateTime.ToString("yyyyMMddHHmmssfff"),
                                    resultTimestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff")
                                }.ToJson();
                                resultContent = CryptographyManager.EncryptAES256_CBC(CommonConstant.Cryptography.tl_key, CommonConstant.Cryptography.tl_iv, resultContent);
                            }
                            else
                            {
                                resultCode = (int)CommonConstant.SystemCode.record_invalid;
                            }
                        }
                    }
                }
            }
            else
            {
                dataInvalid = true;
            }

            if (dataInvalid)
                resultCode = (int)CommonConstant.SystemCode.data_invalid;
        }
        public void GetGiftDetail(string eContent, out int resultCode, out string resultContent)
        {
            var dataInvalid = false;
            resultContent = "";
            resultCode = (int)CommonConstant.SystemCode.undefine;
            eContent = CryptographyManager.DecryptAES256_CBC(CommonConstant.Cryptography.tl_key, CommonConstant.Cryptography.tl_iv, eContent);
            if (!String.IsNullOrEmpty(eContent))
            {
                dynamic theJson = JsonConvert.DeserializeObject(eContent);

                // check and bind input data
                // required fields
                if (string.IsNullOrEmpty((string)theJson.requestTimestamp) || string.IsNullOrEmpty((string)theJson.giftID))
                    dataInvalid = true;
                else
                {
                    var processDateTime = DateTime.Now;
                    var requestTimestamp = DateTime.ParseExact((string)theJson.requestTimestamp, "yyyyMMddHHmmssfff", System.Globalization.CultureInfo.InvariantCulture);
                    var gift_id = (string)theJson.giftID ?? "";
                    int gift_idd = -1;
                    int.TryParse(gift_id, out gift_idd);
                    if ((processDateTime - requestTimestamp).TotalMinutes > 5)
                    {
                        resultCode = (int)CommonConstant.SystemCode.dateTime_invalid;
                    }
                    else
                    {
                        var accessObject = new AccessObject
                        {
                            type = CommonConstant.ObjectType.system,
                            id = CommonConstant.SystemObject.cms_bo,
                            actionChannel = CommonConstant.ActionChannel.mobile_app_frontend,
                            ip = "::1"
                        };
                         //var systemCode = CommonConstant.SystemCode.undefine;
                        resultCode = (int)CommonConstant.SystemCode.undefine;
                        var giftManager = new GiftManager(accessObject);
                        List<string> list = new List<string>();
                        var m = giftManager.GetDetail(gift_idd);
                    
                        if (m != null)
                        {

                            var tempPicList = m.photo_list.OrderBy(s => s.display_order).Select(s => s.file_name + "_middle" + s.file_extension);

                        foreach (var t in tempPicList)
                        {
                            list.Add(ConfigurationManager.AppSettings["PhotoStoragePath"] + @"Gift/" + t);
                        }

                            GetGiftDetailResult gift = new GetGiftDetailResult()
                                {
                                    gift_id = m.gift_id,
                                    gift_no = m.gift_no,
                                    point = m.point,
                                    discount = m.discount,
                                    discount_point = m.discount_point,
                                    hot_item = m.hot_item,
                                    hot_item_display_order = m.hot_item_display_order,
                                    status = m.status,
                                    available_stock = m.available_stock,
                                    record_status = m.record_status,
                                    name = m.name,
                                    status_name = m.status_name,
                                    photo_url= list
                                };
                            resultCode = (int)CommonConstant.SystemCode.normal;

                            resultContent = new
                            {
                                resultGiftObject = gift,
                                processTimestamp = processDateTime.ToString("yyyyMMddHHmmssfff"),
                                resultTimestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff")

                            }.ToJson();
                            resultContent = CryptographyManager.EncryptAES256_CBC(CommonConstant.Cryptography.tl_key, CommonConstant.Cryptography.tl_iv, resultContent);
                        }
                        else
                        {
                            resultCode = (int)CommonConstant.SystemCode.record_invalid;
                        }

                    }
                }
            }
            else
            {
                dataInvalid = true;
            }

            if (dataInvalid)
                resultCode = (int)CommonConstant.SystemCode.data_invalid;

        }
        public void GetProductList(string eContent, out int resultCode, out string resultContent)
        {
            var dataInvalid = false;
            resultContent = "";
            resultCode = (int)CommonConstant.SystemCode.undefine;
           // eContent = CryptographyManager.DecryptAES256_CBC(CommonConstant.Cryptography.tl_key, CommonConstant.Cryptography.tl_iv, eContent);
            if (!String.IsNullOrEmpty(eContent))
            {
                dynamic theJson = JsonConvert.DeserializeObject(eContent);

                // check and bind input data
                // required fields
                if (string.IsNullOrEmpty((string)theJson.requestTimestamp) || string.IsNullOrEmpty((string)theJson.rowIndexStart)||  string.IsNullOrEmpty((string)theJson.rowLimit)|| (string)theJson.sortColumn == null || string.IsNullOrEmpty((string)theJson.sortOrder))
                    dataInvalid = true;
                else
                {
                    var processDateTime = DateTime.Now;
                    var requestTimestamp = DateTime.ParseExact((string)theJson.requestTimestamp, "yyyyMMddHHmmssfff", System.Globalization.CultureInfo.InvariantCulture);
                    if ((processDateTime - requestTimestamp).TotalMinutes > 5)
                    {
                        resultCode = (int)CommonConstant.SystemCode.dateTime_invalid;
                    }
                    else
                    {
                        var searchparmobject = theJson.searchParmList;
                        //var theJsontwo = JsonConvert.DeserializeObject(Convert.ToString(searchparmobject));
                        List<SearchParmObject> lists= JsonConvert.DeserializeObject<List<SearchParmObject>>(Convert.ToString(searchparmobject));
                         //string zhi=(string)theJsontwo.property ?? "";
                        List<SearchParmObject> searchparm = new List<SearchParmObject>();
                        foreach (var m in lists)
                        {
                            searchparm.Add(new SearchParmObject
                            {
                                property = m.property ?? "",
                                value = m.value ?? ""
                            });
                        }
                        var rowIndexStart = (string)theJson.rowIndexStart ?? "";
                        var rowLimit = (string)theJson.rowLimit ?? "";
                        var sortColumn = (string)theJson.sortColumn ?? "";
                        var sortOrder = (string)theJson.sortOrder ?? "";

                        int rowIndexStartd = -1;
                        int rowLimitd = -1;
                        CommonConstant.SortOrder sortOrderc = sortOrder.ToUpper() == "DESC" ? CommonConstant.SortOrder.desc : CommonConstant.SortOrder.asc;



                        int.TryParse(rowIndexStart, out rowIndexStartd);
                        int.TryParse(rowLimit, out rowLimitd);

                        if ((rowIndexStartd <= 0 && rowIndexStart != "0") || (rowLimitd <= 0 && rowLimit != "0"))
                        {
                            resultCode = (int)CommonConstant.SystemCode.data_invalid;
                        }
                        else
                        {
                            var accessObject = new AccessObject
                            {
                                type = CommonConstant.ObjectType.system,
                                id = CommonConstant.SystemObject.cms_bo,
                                actionChannel = CommonConstant.ActionChannel.mobile_app_frontend,
                                ip = "::1"
                            };
                            //var systemCode = CommonConstant.SystemCode.undefine;
                            resultCode = (int)CommonConstant.SystemCode.undefine;
                            var systemCode = CommonConstant.SystemCode.undefine;

                            var totalRow = 0;
                            var giftManager = new ProductManager(accessObject);
                            //List<string> list = new List<string>();
                            var product = giftManager.GetList(rowIndexStartd, rowLimitd, searchparm,sortColumn,sortOrderc,ref systemCode,ref totalRow);

                            if (product != null)
                            {

                  
                                var list = new List<ProductObjectResult>();
                                foreach (var p in product) {
                                    list.Add(new ProductObjectResult {
                                        product_id=p.product_id,
                                        product_no=p.product_no,
                                        price=p.price,
                                        point = p.point,
                                        name=p.name,
                                        icon_photo_url = (string.IsNullOrEmpty(p.file_name) || string.IsNullOrEmpty(p.file_extension)) ? "" : ConfigurationManager.AppSettings["PhotoStoragePath"] + @"Product/" + p.file_name + "_thumb" + p.file_extension
                                    });
                                }
                                resultCode = (int)CommonConstant.SystemCode.normal;

                                resultContent = new
                                {
                                    resultProductList = list,
                                    processTimestamp = processDateTime.ToString("yyyyMMddHHmmssfff"),
                                    resultTimestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff")

                                }.ToJson();
                         
                            }

                            else
                            {
                                resultCode = (int)CommonConstant.SystemCode.record_invalid;
                            }
                        }
                    }
                }
            }
            else
            {
                dataInvalid = true;
            }

            if (dataInvalid)
                resultCode = (int)CommonConstant.SystemCode.data_invalid;
        }
        public void GetGiftList(string eContent, out int resultCode, out string resultContent) {
            var dataInvalid = false;
            resultContent = "";
            resultCode = (int)CommonConstant.SystemCode.undefine;
            var a = ConfigurationManager.AppSettings["PhotoStoragePath"];
            eContent = CryptographyManager.DecryptAES256_CBC(CommonConstant.Cryptography.tl_key, CommonConstant.Cryptography.tl_iv, eContent);
            if (!String.IsNullOrEmpty(eContent))
            {
                dynamic theJson = JsonConvert.DeserializeObject(eContent);

                // check and bind input data
                // required fields
                if (string.IsNullOrEmpty((string)theJson.requestTimestamp) )
                    dataInvalid = true;
                else
                {
                    var processDateTime = DateTime.Now;
                    var requestTimestamp = DateTime.ParseExact((string)theJson.requestTimestamp, "yyyyMMddHHmmssfff", System.Globalization.CultureInfo.InvariantCulture);

                    if ((processDateTime - requestTimestamp).TotalMinutes > 5)
                    {
                        resultCode = (int)CommonConstant.SystemCode.dateTime_invalid;
                    }
                    else
                    {
                        var accessObject = new AccessObject
                        {
                            type = CommonConstant.ObjectType.system,
                            id = CommonConstant.SystemObject.cms_bo,
                            actionChannel = CommonConstant.ActionChannel.mobile_app_frontend,
                            ip = "::1",
                            languageID = 1
                        };
                        var systemCode = CommonConstant.SystemCode.undefine;
                        resultCode = (int)CommonConstant.SystemCode.undefine;
                        var giftManager = new GiftManager(accessObject);
                        var giftPointHistoryList = giftManager.GetList(ref systemCode);
                        if (giftPointHistoryList != null)
                        {

                            var tempgiftPointHistoryList = new List<GetGiftListResult>();
                           
                            foreach (var m in giftPointHistoryList)
                            {
                                tempgiftPointHistoryList.Add(
                                    new GetGiftListResult()
                                    {
                                        gift_id = m.gift_id,
                                        gift_no = m.gift_no,
                                        point = m.point,
                                        discount = m.discount,
                                        discount_point = m.discount_point,
                                        hot_item = m.hot_item,
                                        hot_item_display_order = m.hot_item_display_order,
                                        status = m.status,
                                        available_stock = m.available_stock,
                                        record_status = m.record_status,
                                        name = m.name,
                                        status_name = m.status_name,
                                        icon_photo_url = m.photo_list.OrderBy(s => s.display_order).Select(s => ConfigurationManager.AppSettings["PhotoStoragePath"] + @"Gift/" + s.file_name + "_thumb" + s.file_extension).FirstOrDefault()
                                    }
                                    );
                            }
                            resultCode = (int)systemCode;

                            resultContent = new
                            {
                                resultGiftObjectList = tempgiftPointHistoryList,
                                processTimestamp = processDateTime.ToString("yyyyMMddHHmmssfff"),
                                resultTimestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff")
                                
                            }.ToJson();
                            resultContent = CryptographyManager.EncryptAES256_CBC(CommonConstant.Cryptography.tl_key, CommonConstant.Cryptography.tl_iv, resultContent);
                        }
                        else
                        {
                            resultCode = (int)CommonConstant.SystemCode.record_invalid;
                        }

                    }
                }
            }
            else
            {
                dataInvalid = true;
            }

            if (dataInvalid)
                resultCode = (int)CommonConstant.SystemCode.data_invalid;

        }

        public void GetMemberPointHistoryList(string eContent, out int resultCode, out string resultContent)
        {

            var dataInvalid = false;
            resultContent = "";
            resultCode = (int)CommonConstant.SystemCode.undefine;
            eContent = CryptographyManager.DecryptAES256_CBC(CommonConstant.Cryptography.tl_key, CommonConstant.Cryptography.tl_iv, eContent);
            if (!String.IsNullOrEmpty(eContent))
            {
                dynamic theJson = JsonConvert.DeserializeObject(eContent);

                // check and bind input data
                // required fields
                if (string.IsNullOrEmpty((string)theJson.memberID) || string.IsNullOrEmpty((string)theJson.rowIndexStart) || string.IsNullOrEmpty((string)theJson.rowLimit) || (string)theJson.sortColumn == null || string.IsNullOrEmpty((string)theJson.sortOrder))
                    dataInvalid = true;
                else
                {
                    var processDateTime = DateTime.Now;
                    var requestTimestamp = DateTime.ParseExact((string)theJson.requestTimestamp, "yyyyMMddHHmmssfff", System.Globalization.CultureInfo.InvariantCulture);

                    if ((processDateTime - requestTimestamp).TotalMinutes > 5)
                    {
                        resultCode = (int)CommonConstant.SystemCode.dateTime_invalid;
                    }
                    else
                    {

                        var memberID = (string)theJson.memberID ?? "";
                        var rowIndexStart = (string)theJson.rowIndexStart ?? "";
                        var rowLimit = (string)theJson.rowLimit ?? "";
                        var sortColumn = (string)theJson.sortColumn ?? "";
                        var sortOrder = (string)theJson.sortOrder ?? "";

                        int memberIDd = -1;
                        int rowIndexStartd = -1;
                        int rowLimitd = -1;

                        CommonConstant.SortOrder sortOrderc = sortOrder.ToUpper() == "DESC" ? CommonConstant.SortOrder.desc : CommonConstant.SortOrder.asc;

                        int.TryParse(memberID, out memberIDd);
                        int.TryParse(rowIndexStart, out rowIndexStartd);
                        int.TryParse(rowLimit, out rowLimitd);

                        if (string.IsNullOrEmpty(memberID) || memberIDd == 0 || (rowIndexStartd <= 0 && rowIndexStart != "0") || (rowLimitd <= 0 && rowLimit != "0"))
                        {
                            resultCode = (int)CommonConstant.SystemCode.data_invalid;
                        }
                        else
                        {


                            var accessObject = new AccessObject
                            {
                                type = CommonConstant.ObjectType.member,
                                id = memberIDd,
                                actionChannel = CommonConstant.ActionChannel.mobile_app_frontend,
                                ip = "::1"
                            };

                            var transactionManager = new TransactionManager(accessObject);
                            var systemCode = CommonConstant.SystemCode.undefine;

                            var totalRow = 0;
                            var memberPointHistoryList = transactionManager.GetList(memberIDd, rowIndexStartd, rowLimitd, null, sortColumn, sortOrderc, ref systemCode, ref totalRow);
                            
                        

                            if (memberPointHistoryList != null)
                            {

                                var tempMemberPointHistoryList = new List<GetMemberPointHistoryListResult>();
                                foreach (var m in memberPointHistoryList)
                                {
                                    tempMemberPointHistoryList.Add(
                                        new GetMemberPointHistoryListResult()
                                        {
                                            pointChange = m.point_change,
                                            crt_date = m.crt_date,
                                            type_name = m.type_name,
                                            member_name = m.member_name,
                                            member_no = m.member_no,
                                            point_status_name = m.point_status_name,
                                            status_name = m.status_name,
                                            crt_by_name = m.crt_by_name,
                                            gift_name = m.gift_name
                                        }
                                        );
                                }
                                resultCode = (int)systemCode;

                                resultContent = new
                                {
                                    MemberPointHistoryList = tempMemberPointHistoryList,
                                    processTimestamp = processDateTime.ToString("yyyyMMddHHmmssfff"),
                                    resultTimestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff")
                                }.ToJson();
                                resultContent = CryptographyManager.EncryptAES256_CBC(CommonConstant.Cryptography.tl_key, CommonConstant.Cryptography.tl_iv, resultContent);
                            }
                            else
                            {
                                resultCode = (int)CommonConstant.SystemCode.record_invalid;
                            }
                        }
                    }
                }
            }
            else
            {
                dataInvalid = true;
            }

            if (dataInvalid)
                resultCode = (int)CommonConstant.SystemCode.data_invalid;
        }

       public class GetMemberPointHistoryListResult {
           public double pointChange;
           public DateTime crt_date;
           public string type_name;
          public string member_name;
          public string member_no;
          public string point_status_name;
          public string status_name;
          public string crt_by_name;
          public string gift_name;
       }
       public class ProductObjectResult{
           public int product_id;
           public string product_no ;
           public double price;
           public double point;
           public string name;
           public string icon_photo_url ;
       }
       public class GetGiftListResult
       { 
          public  int gift_id;
          public string gift_no;
          public double point;
          public bool discount; 
          public double? discount_point;
          public bool hot_item;
          public int hot_item_display_order; 
          public int status; 
          public  int available_stock; 
          public int record_status; 
          public string name; 
          public string status_name;
          public string icon_photo_url;
       }
       public class GetGiftDetailResult
       {
           public int gift_id;
           public string gift_no;
           public double point;
           public bool discount;
           public double? discount_point;
           public bool hot_item;
           public int hot_item_display_order;
           public int status;
           public int available_stock;
           public int record_status;
           public string name;
           public string status_name;
           public List<string> photo_url;
       }
        // encryptedLoginToken = yyyyMMddHHmmss + user login token + 8 random char (number or letter)
        // encryptedPassword = yyyyMMddHHmmss + user password + 8 random char (number or letter)
        public void MemberLogin(string encryptedLoginToken, string encryptedPassword, out int resultCode, out string resultContent)
        {
            resultContent = "";
            if (string.IsNullOrEmpty(encryptedLoginToken) || string.IsNullOrEmpty(encryptedPassword))
            {
                resultCode = (int)CommonConstant.SystemCode.data_invalid;
            }
            else
            {
                var loginToken = CryptographyManager.DecryptAES256_CBC(CommonConstant.Cryptography.tl_key, CommonConstant.Cryptography.tl_iv, encryptedLoginToken);
                var password = CryptographyManager.DecryptAES256_CBC(CommonConstant.Cryptography.tl_key, CommonConstant.Cryptography.tl_iv, encryptedPassword);

                if (loginToken.Length <= (14 + 8) || password.Length <= (14 + 8))
                {
                    resultCode = (int)CommonConstant.SystemCode.data_invalid;
                }
                else
                {
                    var loginTokenDate = DateTime.ParseExact(loginToken.Substring(0, 14), "yyyyMMddHHmmss",
                                       System.Globalization.CultureInfo.InvariantCulture);
                    var passwordDate = DateTime.ParseExact(password.Substring(0, 14), "yyyyMMddHHmmss",
                                       System.Globalization.CultureInfo.InvariantCulture);

                    var currentDate = DateTime.Now;

                    if ((currentDate - loginTokenDate).TotalMinutes > 5 || (currentDate - passwordDate).TotalMinutes > 5)
                    {
                        resultCode = (int)CommonConstant.SystemCode.dateTime_invalid;
                    }
                    else
                    {
                        loginToken = loginToken.Substring(14, loginToken.Length - 14 - 8);
                        password = password.Substring(14, password.Length - 14 - 8);

                        var accessObject = new AccessObject
                        {
                            type = CommonConstant.ObjectType.system,
                            id = CommonConstant.SystemObject.cms_bo,
                            actionChannel = CommonConstant.ActionChannel.mobile_app_frontend,
                            ip = "::1"
                        };

                        var systemCode = CommonConstant.SystemCode.undefine;
                        var accessManager = new AccessManager();
                        var memberManager = new MemberManager(accessObject);

                        var memberID = 0;
                        var session = "";
                        var validLogin = accessManager.MemberLogin(accessObject, loginToken, password, ref memberID, ref session);

                        if (validLogin)
                        {
                            // Create Token
                            var accessTokenObject = new AccessTokenObject();
                            accessManager.CreateAndGenerateToken(CommonConstant.ObjectType.member, memberID, ref accessTokenObject);

                            var encryptedToken = CryptographyManager.EncryptAES256_CBC(CommonConstant.Cryptography.tl_key, CommonConstant.Cryptography.tl_iv, accessTokenObject.token);

                            // Get Member
                            var member = memberManager.GetDetail(memberID, true, ref systemCode);
                            resultCode = (int)systemCode;

                            resultContent = new { eToken = encryptedToken, member = member }.ToJson();
                        }
                        else
                        {
                            resultCode = (int)CommonConstant.SystemCode.record_invalid;
                        }
                    }
                }
            }
        }

        public void MemberNormalLogin(string eContent, out int resultCode, out string resultContent)
        {

            var dataInvalid = false;
            resultContent = "";
            resultCode = (int)CommonConstant.SystemCode.undefine;

            if (!String.IsNullOrEmpty(eContent))
            {
                dynamic theJson = JsonConvert.DeserializeObject(eContent);

                // check and bind input data
                // required fields
                if (string.IsNullOrEmpty((string)theJson.encryptedLoginToken)
                     || string.IsNullOrEmpty((string)theJson.encryptedPassword))
                    dataInvalid = true;
                else
                {
                     var processDateTime = DateTime.Now;
                    var requestTimestamp = DateTime.ParseExact((string)theJson.requestTimestamp, "yyyyMMddHHmmssfff", System.Globalization.CultureInfo.InvariantCulture);

                    if ((processDateTime - requestTimestamp).TotalMinutes > 5)
                    {
                        resultCode = (int)CommonConstant.SystemCode.dateTime_invalid;
                    }
                    else
                    {
                    var encryptedLoginToken = (string)theJson.encryptedLoginToken ?? "";
                    var encryptedPassword = (string)theJson.encryptedPassword ?? "";

                    if (string.IsNullOrEmpty(encryptedLoginToken) || string.IsNullOrEmpty(encryptedPassword))
                    {
                        resultCode = (int)CommonConstant.SystemCode.data_invalid;
                    }
                    else
                    {
                        var loginToken = CryptographyManager.DecryptAES256_CBC(CommonConstant.Cryptography.tl_key, CommonConstant.Cryptography.tl_iv, encryptedLoginToken);
                        var password = CryptographyManager.DecryptAES256_CBC(CommonConstant.Cryptography.tl_key, CommonConstant.Cryptography.tl_iv, encryptedPassword);

                        if (loginToken.Length <= (14 + 8) || password.Length <= (14 + 8))
                        {
                            resultCode = (int)CommonConstant.SystemCode.data_invalid;
                        }
                        else
                        {
                            var loginTokenDate = DateTime.ParseExact(loginToken.Substring(0, 14), "yyyyMMddHHmmss",
                                               System.Globalization.CultureInfo.InvariantCulture);
                            var passwordDate = DateTime.ParseExact(password.Substring(0, 14), "yyyyMMddHHmmss",
                                               System.Globalization.CultureInfo.InvariantCulture);

                            var currentDate = DateTime.Now;

                            if ((currentDate - loginTokenDate).TotalMinutes > 5 || (currentDate - passwordDate).TotalMinutes > 5)
                            {
                                resultCode = (int)CommonConstant.SystemCode.dateTime_invalid;
                            }
                            else
                            {
                                loginToken = loginToken.Substring(14, loginToken.Length - 14 - 8);
                                password = password.Substring(14, password.Length - 14 - 8);

                                var accessObject = new AccessObject
                                {
                                    type = CommonConstant.ObjectType.system,
                                    id = CommonConstant.SystemObject.cms_bo,
                                    actionChannel = CommonConstant.ActionChannel.mobile_app_frontend,
                                    ip = NetworkManager.GetClientIP()
                                };

                                var systemCode = CommonConstant.SystemCode.undefine;
                                var accessManager = new AccessManager();
                                var memberManager = new MemberManager(accessObject);

                                var memberID = 0;
                                var session = "";
                                var validLogin = accessManager.MemberLogin(accessObject, loginToken, password, ref memberID, ref session);

                                if (validLogin)
                                {
                                    // Create Token
                                    var accessTokenObject = new AccessTokenObject();
                                    accessManager.CreateAndGenerateToken(CommonConstant.ObjectType.member, memberID, ref accessTokenObject);

                                    var encryptedToken = CryptographyManager.EncryptAES256_CBC(CommonConstant.Cryptography.tl_key, CommonConstant.Cryptography.tl_iv, accessTokenObject.token);

                                    // Get Member
                                    var member = memberManager.GetDetail(memberID, true, ref systemCode);
                                    resultCode = (int)systemCode;

                                    resultContent = new
                                    {
                                        resultToken = encryptedToken,
                                        memberID = member.member_id.ToString(),
                                        memberCode =  member.member_no,
                                        email = member.email,
                                        fbMainEmail = member.fbemail,
                                        fbid = member.fbid,
                                        name =  member.GetFullname(),
                                        firstName = member.firstname,
                                        middleName =member.middlename,
                                        lastName = member.lastname,
                                        gender = member.gender.ToString(),
                                        birthYear = member.birth_year.ToString(),
                                        birthMonth = member.birth_month.ToString(),
                                        birthDay =member.birth_day.ToString(),
                                        point =member.available_point.ToString(),

                                        processTimestamp = processDateTime.ToString("yyyyMMddHHmmssfff"),
                                        resultTimestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff")
                                    }.ToJson();
                                    resultContent = CryptographyManager.EncryptAES256_CBC(CommonConstant.Cryptography.tl_key, CommonConstant.Cryptography.tl_iv, resultContent);
                                }
                                else
                                {
                                    resultCode = (int)CommonConstant.SystemCode.record_invalid;
                                }
                            }
                        }
                    }
                    }
                }
            }
            else
            {
                dataInvalid = true;
            }

            if (dataInvalid)
                resultCode = (int)CommonConstant.SystemCode.data_invalid;
        }


        //test
        public void TestGetLogin(out string eloginToken, out string epassword)
        {
            var loginToken = DateTime.Now.ToString("yyyyMMddHHmmss") + "MC0001" + "12345678";
            var password = DateTime.Now.ToString("yyyyMMddHHmmss") + "123456" + "12345678";

            eloginToken = CryptographyManager.EncryptAES256_CBC(CommonConstant.Cryptography.tl_key, CommonConstant.Cryptography.tl_iv, loginToken);
            epassword = CryptographyManager.EncryptAES256_CBC(CommonConstant.Cryptography.tl_key, CommonConstant.Cryptography.tl_iv, password);
        }

        //test
        public void TestEncrypt(string message, out string eMessage)
        {
            eMessage = CryptographyManager.EncryptAES256_CBC(CommonConstant.Cryptography.tl_key, CommonConstant.Cryptography.tl_iv, message);
        }

        //test
        public void TestDecrypt(string eMessage, out string message)
        {
            message = CryptographyManager.DecryptAES256_CBC(CommonConstant.Cryptography.tl_key, CommonConstant.Cryptography.tl_iv, eMessage);
        }

        // gender: CommonConstant.Gender
        //public void MemberLoginFB(string fbid, string email, 
        //    string firstName, string middleName, string lastName, int gender, int year, int month, int day,
        //    out int resultCode, out MemberObject member)
        //{
        //    var accessObject = new AccessObject
        //    {
        //        type = CommonConstant.ObjectType.system,
        //        id = CommonConstant.SystemObject.cms_bo,
        //        actionChannel = CommonConstant.ActionChannel.mobile_app_frontend,
        //        ip = "::1"
        //    };

        //    resultCode = (int)CommonConstant.SystemCode.undefine;
        //    var memberManager = new MemberManager(accessObject);

        //    var salutation = (gender == CommonConstant.Gender.male) ? CommonConstant.Salutation.mr : CommonConstant.Salutation.miss;

        //    var memberObj = new MemberObject()
        //    {
        //        fbid = fbid,
        //        fbemail = email,
        //        email = email,
        //        firstname = firstName,
        //        middlename = middleName,
        //        lastname = lastName,
        //        birth_year = year,
        //        birth_month = month,
        //        birth_day = day,
        //        gender = gender,
        //        salutation = salutation,

        //        // default
        //        mobile_no = "",
        //        hkid = "",
        //        reg_ip = "",
        //        activate_key = "",
        //        hash_key = "",
        //        status = (int)CommonConstant.Status.active,
        //        member_level_id = 1,
        //        member_category_id = 7
        //    };

        //    var valid = memberManager.ConnectWithFB(ref memberObj);

        //    if (valid)
        //    {
        //        member = memberObj;
        //        resultCode = (int)CommonConstant.SystemCode.normal;
        //    }
        //    else
        //    {
        //        member = new MemberObject();
        //        resultCode = (int)CommonConstant.SystemCode.record_invalid;
        //    }
        //}

        public void MemberLoginFB(string eContent, out int resultCode, out string resultContent)
        {
            var processDateTime = DateTime.Now;

            var plainContent = CryptographyManager.DecryptAES256_CBC(CommonConstant.Cryptography.tl_key, CommonConstant.Cryptography.tl_iv, eContent);

            var dataInvalid = false;
            resultCode = (int)CommonConstant.SystemCode.undefine;
            resultContent = "";


            if (!String.IsNullOrEmpty(plainContent))
            {
                dynamic theJson = JsonConvert.DeserializeObject(plainContent);

                // check and bind input data
                // required fields
                if (string.IsNullOrEmpty((string)theJson.requestTimestamp)
                    || string.IsNullOrEmpty((string)theJson.fbid)
                    || string.IsNullOrEmpty((string)theJson.fbMainEmail)
                    || string.IsNullOrEmpty((string)theJson.requestSignature))
                    dataInvalid = true;
                else
                {
                    var requestTimestamp = DateTime.ParseExact((string)theJson.requestTimestamp, "yyyyMMddHHmmssfff", System.Globalization.CultureInfo.InvariantCulture);

                    if ((processDateTime - requestTimestamp).TotalMinutes > 5)
                    {
                        resultCode = (int)CommonConstant.SystemCode.dateTime_invalid;
                    }
                    else
                    {
                        var fbid = (string)theJson.fbid ?? "";
                        var fbMainEmail = (string)theJson.fbMainEmail ?? "";
                        var firstName = (string)theJson.firstName ?? "";
                        var middleName = (string)theJson.middleName ?? "";
                        var lastName = (string)theJson.lastName ?? "";
                        var genderStr = (string)theJson.gender ?? "";
                        int gender = 0;
                        if (genderStr.ToUpper() == "M")
                            gender = CommonConstant.Gender.male;
                        else if (genderStr.ToUpper() == "F")
                            gender = CommonConstant.Gender.female;

                        var birthYear = (int)(theJson.birthYear ?? 0);
                        var birthMonth = (int)(theJson.birthMonth ?? 0);
                        var birthDay = (int)(theJson.birthDay ?? 0);
                        // var eSignature = (string)theJson.eSignature ?? "";

                        var accessObject = new AccessObject
                        {
                            type = CommonConstant.ObjectType.system,
                            id = CommonConstant.SystemObject.cms_bo,
                            actionChannel = CommonConstant.ActionChannel.mobile_app_frontend,
                            ip = "::1"
                        };

                        resultCode = (int)CommonConstant.SystemCode.undefine;
                        var memberManager = new MemberManager(accessObject);

                        var salutation = (gender == CommonConstant.Gender.male) ? CommonConstant.Salutation.mr : CommonConstant.Salutation.miss;

                        var memberObj = new MemberObject()
                        {
                            fbid = fbid,
                            fbemail = fbMainEmail,
                            email = fbMainEmail,
                            firstname = firstName,
                            middlename = middleName,
                            lastname = lastName,
                            birth_year = birthYear,
                            birth_month = birthMonth,
                            birth_day = birthDay,
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

                        var valid = memberManager.ConnectWithFB(ref memberObj);

                        if (valid)
                        {
                            if (memberObj.member_id > 0)
                            {
                                // Create Token
                                var accessManager = new AccessManager();
                                var accessTokenObject = new AccessTokenObject();
                                accessManager.CreateAndGenerateToken(CommonConstant.ObjectType.member, memberObj.member_id, ref accessTokenObject);

                                var resultObj = new
                                {
                                    processTimestamp = processDateTime.ToString("yyyyMMddHHmmssfff"),
                                    resultTimestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff"),
                                    resultToken = accessTokenObject.token,
                                    memberID = memberObj.member_id,
                                    memberCode = memberObj.member_no,
                                    email = memberObj.email,
                                    fbMainEmail = memberObj.fbemail,
                                    fbid = memberObj.fbid,
                                    name = memberObj.GetFullname(),
                                    firstName = memberObj.firstname,
                                    middleName = memberObj.middlename,
                                    lastName = memberObj.lastname,
                                    gender = memberObj.gender,
                                    birthYear = memberObj.birth_year,
                                    birthMonth = memberObj.birth_month,
                                    birthDay = memberObj.birth_day,
                                    point = Convert.ToInt32(memberObj.available_point),
                                    resultSignature = RandomManager.GenerateNumberAndLetter(6)
                                };

                                resultContent = CryptographyManager.EncryptAES256_CBC(CommonConstant.Cryptography.tl_key, CommonConstant.Cryptography.tl_iv, resultObj.ToJson());

                                resultCode = (int)CommonConstant.SystemCode.normal;
                            }
                            else
                                resultCode = (int)CommonConstant.SystemCode.record_invalid;
                        }
                        else
                        {
                            //member = new MemberObject();
                            resultCode = (int)CommonConstant.SystemCode.record_invalid;
                        }
                    }
                }
            }
            else
            {
                dataInvalid = true;
            }

            if (dataInvalid)
                resultCode = (int)CommonConstant.SystemCode.data_invalid;
        }


        public void MemberRegister(string eContent, out int resultCode, out string resultContent)
        {
            var processDateTime = DateTime.Now;
            eContent = CryptographyManager.DecryptAES256_CBC(CommonConstant.Cryptography.tl_key, CommonConstant.Cryptography.tl_iv, eContent);

            //var plainContent = CryptographyManager.DecryptAES256_CBC(CommonConstant.Cryptography.tl_key, CommonConstant.Cryptography.tl_iv, eContent);
            var plainContent = eContent;

            var dataInvalid = false;
            resultCode = (int)CommonConstant.SystemCode.undefine;
            resultContent = "";

            try
            {
                if (!String.IsNullOrEmpty(plainContent))
                {
                    dynamic theJson = JsonConvert.DeserializeObject(plainContent);

                    // check and bind input data
                    // required fields
                    if (string.IsNullOrEmpty((string)theJson.requestTimestamp)
                        || string.IsNullOrEmpty((string)theJson.firstName)
                        || string.IsNullOrEmpty((string)theJson.lastName)
                        || string.IsNullOrEmpty((string)theJson.gender)
                        || string.IsNullOrEmpty((string)theJson.email)
                        || string.IsNullOrEmpty((string)theJson.password)
                        || string.IsNullOrEmpty((string)theJson.confirmPassword)
                        || string.IsNullOrEmpty((string)theJson.requestSignature))
                        dataInvalid = true;
                    else
                    {
         
                        var requestTimestamp = DateTime.ParseExact((string)theJson.requestTimestamp, "yyyyMMddHHmmssfff", System.Globalization.CultureInfo.InvariantCulture);

                        if ((processDateTime - requestTimestamp).TotalMinutes > 5)
                        {
                            resultCode = (int)CommonConstant.SystemCode.dateTime_invalid;
                        }
                        else if (!((string)theJson.password).Equals((string)theJson.confirmPassword))
                        {
                            resultCode = (int)CommonConstant.SystemCode.err_member_password_invaild;
                        }
                        else
                        {
                            var firstName = (string)theJson.firstName ?? "";
                            var lastName = (string)theJson.lastName ?? "";
                            var genderStr = (string)theJson.gender ?? "";
                            var email = (string)theJson.email ?? "";
                            var password = (string)theJson.password ?? "";
                            var confirmPassword = (string)theJson.confirmPassword ?? "";
                            var eSignature = (string)theJson.requestSignature ?? "";

                            var accessObject = new AccessObject
                            {
                                type = CommonConstant.ObjectType.system,
                                id = CommonConstant.SystemObject.cms_bo,
                                actionChannel = CommonConstant.ActionChannel.mobile_app_frontend,
                                ip = "::1"
                            };

                            resultCode = (int)CommonConstant.SystemCode.undefine;
                            var memberManager = new MemberManager(accessObject);

                            int gender = 0;
                            if (genderStr.ToUpper() == "M")
                                gender = CommonConstant.Gender.male;
                            else if (genderStr.ToUpper() == "F")
                                gender = CommonConstant.Gender.female;


                            var memberObj = new MemberObject()
                            {
                                member_no = null,
                                password = password,
                                email = email,
                                fbid = "",
                                fbemail = "",
                                mobile_no = "",
                                salutation = 0,
                                firstname = firstName,
                                middlename = "",
                                lastname = lastName,
                                birth_year = 0,
                                birth_month = 0,
                                birth_day = 0,
                                gender = gender,
                                hkid = "",
                                address1 = "",
                                address2 = "",
                                address3 = "",
                                district = 0,
                                region = 0,
                                reg_source = 0,
                                referrer = 0,
                                reg_status = 0,
                                reg_ip = "",
                                activate_key = "",
                                hash_key = "",
                                session = "",
                                status = CommonConstant.Status.active,
                                opt_in = 0,
                                member_level_id = 1,
                                member_category_id = 1
                            };



                            resultCode = (int)CommonConstant.SystemCode.undefine;
                            var m = new MemberManager(accessObject);

                            string sql_remark = "";
                            int new_member_id = 0;
                            m.Create(memberObj, ref sql_remark, ref new_member_id);

                            if (memberObj.member_id > 0)
                            {
                                // Create Token
                                var accessManager = new AccessManager();
                                var accessTokenObject = new AccessTokenObject();
                                accessManager.CreateAndGenerateToken(CommonConstant.ObjectType.member, memberObj.member_id, ref accessTokenObject);

                                var resultObj = new
                                {
                                    processTimestamp = processDateTime.ToString("yyyyMMddHHmmssfff"),
                                    resultTimestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff"),
                                    resultToken = accessTokenObject.token,
                                    memberID = memberObj.member_id,
                                    memberCode = memberObj.member_no,
                                    email = memberObj.email,
                                    fbMainEmail = memberObj.fbemail,
                                    fbid = memberObj.fbid,
                                    name = memberObj.GetFullname(),
                                    firstName = memberObj.firstname,
                                    middleName = memberObj.middlename,
                                    lastName = memberObj.lastname,
                                    gender = memberObj.gender,
                                    birthYear = memberObj.birth_year,
                                    birthMonth = memberObj.birth_month,
                                    birthDay = memberObj.birth_day,
                                    point = Convert.ToInt32(memberObj.available_point),
                                    resultSignature = RandomManager.GenerateNumberAndLetter(6)
                                };

                                //resultContent = CryptographyManager.EncryptAES256_CBC(CommonConstant.Cryptography.tl_key, CommonConstant.Cryptography.tl_iv, resultObj.ToJson());
                                resultContent = resultObj.ToJson();
                                resultContent = CryptographyManager.EncryptAES256_CBC(CommonConstant.Cryptography.tl_key, CommonConstant.Cryptography.tl_iv, resultContent);

                                resultCode = (int)CommonConstant.SystemCode.normal;
                            }


                        }
                    }
                }
                else
                {
                    dataInvalid = true;
                }
            }
            catch (Exception e)
            {
                dataInvalid = true;
            }

            if (dataInvalid)
                resultCode = (int)CommonConstant.SystemCode.data_invalid;
        }



        //public void MemberCreate(string email, string password, out int resultCode, out MemberObject newMember)
        //{
        //    var member = new MemberObject()
        //    {
        //        member_no = "",
        //        password = password,
        //        email = email,
        //        fbid = "",
        //        fbemail = "",
        //        mobile_no = "",
        //        salutation = 1,
        //        firstname = "",
        //        middlename = "",
        //        lastname = "",
        //        birth_year = 1900,
        //        birth_month = 1,
        //        birth_day = 1,
        //        gender = 1,
        //        hkid = "",
        //        address1 = "",
        //        address2 = "",
        //        address3 = "",
        //        district = 1,
        //        region = 1,
        //        reg_source = 1,
        //        referrer = 0,
        //        reg_status = 0,
        //        reg_ip = "",
        //        activate_key = "",
        //        hash_key = "",
        //        session = "",
        //        status = CommonConstant.Status.active,
        //        opt_in = 1,
        //        member_level_id = 1,
        //        member_category_id = 7,
        //    };

        //    var sql_remark = "";
        //    var memberManager = new MemberManager(_accessObject);
        //    int new_member_id = 0;
        //    var systemCode = memberManager.Create(member, ref sql_remark, ref new_member_id);

        //    if (systemCode == CommonConstant.SystemCode.normal)
        //    {
        //        newMember = memberManager.GetDetail(new_member_id, false, ref systemCode);
        //    }
        //    else
        //        newMember = new MemberObject();

        //    resultCode = (int)systemCode;
        //}

        public void MissionIBeacon(int member_id, out int resultCode)
        {
            //UpdateAccessObject(CommonConstant.ObjectType.member, member_id, "::1");

            var pointEngineManager = new PointEngineManager(_accessObject);
            pointEngineManager.IBeaconLocationPresence(member_id);

            resultCode = (int)CommonConstant.SystemCode.normal;
        }

        public void MissionGetPoint(int member_id, out int resultCode)
        {
            //UpdateAccessObject(CommonConstant.ObjectType.member, member_id, "::1");

            var pointEngineManager = new PointEngineManager(_accessObject);
            pointEngineManager.MissionGetPoint(member_id);

            resultCode = (int)CommonConstant.SystemCode.normal;
        }

        public void CreateCombineRedemption(int member_id, int coupon_id, int no_of_ppl, out int resultCode, out string combineRedemptionCode, out int new_combine_id)
        {
            var obj = new CombineRedemptionObject()
            {
                member_id = member_id,
                coupon_id = coupon_id,
                position = 1,
                no_of_ppl = no_of_ppl,
                join_combine_id = 0,
                notified_host = 0,
                status = (int)CommonConstant.CombineRedemptionStatus.connecting
            };

            var combineRedemptionManager = new CombineRedemptionManager(_accessObject);
            new_combine_id = 0;
            var systemCode = combineRedemptionManager.Create(obj, ref new_combine_id);

            combineRedemptionCode = "CR" + new_combine_id.ToString("D10");
            resultCode = (int)CommonConstant.SystemCode.normal;
        }

        public void JoinCombineRedemption(int member_id, string combineRedemptionCode, out int resultCode, out int newCombineID, out int coupon_id, out int noOfPpl, out int position, out int pointRequire)
        {
            var combineIDStr = combineRedemptionCode.Substring(2, 10);

            var combineID = int.Parse(combineIDStr);

            var combineRedemptionManager = new CombineRedemptionManager(_accessObject);
            newCombineID = 0;
            position = 0;
            coupon_id = 0;
            noOfPpl = 0;
            pointRequire = 0;
            var systemCode = combineRedemptionManager.JoinCombineRedemption(member_id, combineID, ref newCombineID, ref coupon_id, ref noOfPpl, ref position, ref pointRequire);

            resultCode = (int)systemCode;
        }

        public void CancelJoinCombineRedemption(int member_id, int combine_id, out int resultCode)
        {
            var combineRedemptionManager = new CombineRedemptionManager(_accessObject);
            var systemCode = combineRedemptionManager.CancelJoinCombineRedemption(member_id, combine_id);

            resultCode = (int)systemCode;
        }

        public void NotifyHostCombineRedemption(int member_id, int combine_id, out int resultCode, out int totalJoined, out int new_position, out int new_status)
        {
            var combineRedemptionManager = new CombineRedemptionManager(_accessObject);
            totalJoined = 0;
            new_position = 0;
            new_status = 0;
            var systemCode = combineRedemptionManager.NotifyHostCombineRedemption(member_id, combine_id, ref totalJoined, ref new_position, ref new_status);

            resultCode = (int)systemCode;
        }

        public void ConfirmCombineRedemption(int member_id, int combine_id, out int resultCode)
        {
            var combineRedemptionManager = new CombineRedemptionManager(_accessObject);

            var systemCode = combineRedemptionManager.ConfirmCombineRedemption_fujixerox(member_id, combine_id);

            resultCode = (int)systemCode;
        }

        // temp for demo
        public void PointAdjust(int member_id, int pointAdjust, out int resultCode)
        {
            var pointAdjustManager = new PointAdjustManager(_accessObject);

            var location_id = 0;
            var remark = "";
            var systemCode = pointAdjustManager.Adjust(location_id, member_id, pointAdjust, remark);

            resultCode = (int)systemCode;
        }

        public void GetCombineRedemptionStatus(int combine_id, out int resultCode, out int status)
        {
            var systemCode = CommonConstant.SystemCode.undefine;
            var combineRedemptionManager = new CombineRedemptionManager(_accessObject);
            var combineObj = combineRedemptionManager.GetDetail(combine_id, true, ref systemCode);
            status = combineObj.status;
            resultCode = (int)systemCode;
        }

        // normal login
        public void MemberLoginJson(string loginToken, string password, out int resultCode, out string content)
        {
            var accessObject = new AccessObject
            {
                type = CommonConstant.ObjectType.system,
                id = CommonConstant.SystemObject.cms_bo,
                actionChannel = CommonConstant.ActionChannel.mobile_app_frontend,
                ip = "::1"
            };

            var systemCode = CommonConstant.SystemCode.undefine;
            var accessManager = new AccessManager();
            var memberManager = new MemberManager(accessObject);

            var memberID = 0;
            var session = "";
            var validLogin = accessManager.MemberLogin(accessObject, loginToken, password, ref memberID, ref session);

            var member = new MemberObject();
            if (validLogin)
            {
                member = memberManager.GetDetail(memberID, true, ref systemCode);
                resultCode = (int)systemCode;
            }
            else
            {
                member = new MemberObject();
                resultCode = (int)CommonConstant.SystemCode.record_invalid;
            }

            content = member.ToJson();
        }



    }
}
