using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Palmary.Loyalty.Common
{
    public static class CommonConstant
    {
        public static class Module
        {
            // string value should not contain space character

            // framework module
            public const string user = "User";
            public const string Role = "Role";
            public const string log = "Log";
            public const string logDetail = "LogDetail";
            public const string roleAccess = "RoleAccess";
            public const string systemConfig = "SystemConfig";
            public const string language = "Language";

            // demo mobule
            public static string demo1 = "Demo1";

            // other module
            public static string gift = "Gift";
            public static string giftLocation = "GiftLocation";
            public static string promote = "Promote";
            public static string giftRedemptionHistory = "GiftRedemptionHistory";
            public static string product = "Product";
            public static string location = "Location";
            public static string giftCategory = "GiftCategory";
            public static string productCategory = "ProductCategory";
            public static string memberCategory = "MemberCategory";
            public static string promotionRule = "PromotionRule";

            public static string productPurchase = "ProductPurchase";
            public static string productPurchaseDetail = "ProductPurchaseDetail";
            public static string passcode = "Passcode";
            public static string passcodeGenerate = "PasscodeGenerate";
            public static string passcodeRegistry = "PasscodeRegistry";
            public static string passcodePrefix = "PasscodePrefix";
            public static string passcodeImport = "PasscodeImport";

            public static string transaction = "Transaction";
            public static string giftRedemption = "GiftRedemption";
            public static string giftInventory = "GiftInventory";
            public static string giftInventorySummary = "GiftInventorySummary";

            public static string servicePlan = "ServicePlan";
            public static string serviceCategory = "ServiceCategory";

            public static string basicRule = "BasicRule";

            public static string serviceContract = "ServiceContract";
            public static string servicePayment = "ServicePayment";
            public static string servicePaymentDetail = "ServicePaymentDetail";
            public static string servicePaymentDetailExtra = "ServicePaymentDetailExtra";


            public static string transactionHistory = "TransactionHistory";
            public static string transactionImport = "TransactionImport";
            public static string transactionDetailRedemption = "TransactionDetailRedemption";
            public static string transactionField = "TransactionField";
            public static string transactionGroup = "TransactionGroup";

            public static string memberLevel = "MemberLevel";
            public static string memberService = "MemberService";
            public static string memberCard = "MemberCard";
            public static string memberCardHistory = "MemberCardHistory";
            public static string memberField = "MemberField";
            public static string memberGroup = "MemberGroup";
            public static string memberImport = "MemberImport";
            public static string memberLevelChange = "MemberLevelChange";
            public static string memberAdvanceSearch = "MemberAdvanceSearch";
            public static string memberAdvanceSearchListMember = "MemberAdvanceSearchListMember";

            public static string wifiLocation = "WifiLocation";
            public static string wifiLocationPromote = "WifiLocationPromote";
            public static string wifiAccessHistory = "WifiAccessHistory";
            public static string wifiAccessReportByLocation = "WifiAccessReportByLocation";
            public static string wifiAccessReportByMemberLevel = "WifiAccessReportByMemberLevel";

            public static string fileImportJob = "FileImportJob";

            public static string reminderEngine = "ReminderEngine";
            public static string reminderTemplate = "ReminderTemplate";

            public static string rolePrivilege = "RolePrivilege";
            public static string giftMemberPrivileg = "GiftMemberPrivileg";
            public static string giftPhoto = "GiftPhoto";
            public static string member = "Member";
            public static string productPhoto = "ProductPhoto";

            public static string promotionRulePurchaseCriteria = "PromotionRulePurchaseCriteria";
            public static string promotionRuleMemberCategory = "PromotionRuleMemberCategory";
            public static string promotionRuleMemberLevel = "PromotionRuleMemberLevel";
            public static string promotionRulePurchaseProductCriteria = "PromotionRulePurchaseProductCriteria";
            public static string promotionRuleServiceCriteria = "PromotionRuleServiceCriteria";
            public static string reminderSchedule = "ReminderSchedule";
            public static string transactionEarn = "TransactionEarn";

            public static string transactionUse = "TransactionUse";
            public static string wifiLocationPrivilege = "WifiLocationPrivilege";


            public static string bo = "BO";
         

            public static string report = "Report";

            public static string combineRedemption = "CombineRedemption";

            public static string cronjob = "Cronjob";
        }

        public static class ObjectType
        {// should be same as table listing_item where code = ObjectType, list_id = 67
            // framework object
            public static int system = -1000;   // access object
            public static int user = 1;         // access object
            public static int user_role = 2;
            public static int section = 3;
            public static int section_privilege = 4;
            public static int system_config = 5;

            // other object
            public static int member = 10;  // access object
            public static int member_level = 11;
            public static int member_category = 12;
            public static int member_card_history = 13;
            public static int member_card = 14;

            public static int product = 20;
            public static int product_category = 21;

            public static int gift = 31;
            public static int gift_category = 32;
            public static int location = 33;

            public static int report = 34;
        }

        public static class SystemObject
        {// should be same as table system_object
            public static int cms_bo = 1;
            public static int con_job = 2;
            public static int store_procedure = 3;
        }

        public enum ListingType : int
        {// should be same as table listing
            CronjobResult=94,
            Status = 2,
            Gender = 62,
            ObjectType = 67,
            ReminderEngineType = 80,
            ReminderTemplateType = 83,
            MemberLevelChangeSourceType = 84,
            MemberLevelChangeChangeType = 87,
            MemberLevelChangeReasonType = 88,
            MemberAdvanceSearchField = 91,
            CompareCondition = 92,
            Month = 93
        }

        public static class ActionChannel
        {// should be same as table listing_item where code = ActionChannel
            public static int cms_backend = 1;
            public static int Web_frontend = 2;
            public static int mobile_Web_frontend = 3;
            public static int mobile_app_frontend = 4;
            public static int kiosk_frontend = 5;

            public static int cms_bo = 1000;
            public static int store_procedure = 1001;
        }

        public static class ActionType
        {// should be same as listing_item where code = ActionType
            public static int create = 1;
            public static int read = 2;
            public static int update = 3;
            public static int delete = 4;
            public static int login = 5;
            public static int logout = 6;
        }

        public static class Status
        {
            public static int active = 1;
            public static int inactive = 2;
        }

        public static class RecordStatus
        {
            public static int normal = 0;
            public static int deleted = -1;
        }

        public static class Gender
        {
            public static int unknown = 0;
            public static int male = 1;
            public static int female = 2;
        }

        public static class Salutation
        {
            public static int mr = 1;
            public static int miss = 2;
            public static int mrs = 3;
            public static int dr = 4;
            public static int prof = 5;

        }

        public enum TransactionType : int // list id =45 in table Listing
        {
            passcode_registration = 1,
            promotion_rule = 2,
            point_adjustment = 3,  // add or minus point
            purchase_product = 4,

            postpaidservice = 5,
            prepaidservice = 6,

            point_transfer = 7, // point transfer
            member_referral = 8, //
            redemption = 50, // minus point
            redemption_cancel = 51,

            location_presence = 60,
            mission = 70,

            other = 100
        }

        public static class TransactionStatus
        {
            public static int cancel = -1;
            public static int active = 1;
            public static int all_point_used = 2;  // all point used
        }

        public enum LangCode : int
        {
            en = 1,
            tc = 2,
            sc = 3
        }

        public static class RegexStr
        {
            public const string normalText = @"^(([\w\-()][\w\-\s()]*[\w\-()])|[\w\-()]?)$";  //allow empty, alphanumeric,-,_,space,chinese, prefix/suffix no space
            public const string normalCode = @"^[a-zA-Z0-9_-]*$"; // no space
            public const string normalPhone = @"^[0-9-]*$"; // no space
            public const string normalEmail = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";
            public const string normalNumber = @"^[\d]";
            public const string normalHKID = @"^[a-zA-Z0-9-()]*$";
        }

        public enum ImageSizeType : int
        {
            orginial = 0, large = 1, middle = 2, thumb = 3
        }

        public static string[] ImageSizeName_postfix = { "_original", "_large", "_middle", "_thumb" };


        public static class ImageSize
        {
            public static class Large
            {
                public static int width = 800;
                public static int height = 800;
            }

            public static class Middle
            {
                public static int width = 500;
                public static int height = 500;
            }

            public static class Thumb
            {
                public static int width = 100;
                public static int height = 100;
            }
        }

        public static class ConfigName
        {
            public static string point_expiry_month = "point_expiry_month";
            public static string point_mode = "point_mode";
            public static string passcode_expiry_month = "passcode_expiry_month";
            public static string point_cut_off_date = "point_cut_off_date";
        }

        public static class PointMode
        {
            public static string continuous = "continuous";
            public static string cut_off = "cut_off";
        }

        public static class Table
        {
            public static string system_object = "system_object";

        }

        public enum StockChangeType : int
        {
            stock_adjustment = 1,
            redemption = 2,
            redemption_cancel = 3
        }

        public enum SystemCode : int
        {
            // core framework system code
            undefine = 0,
            database_error = 1,
            normal = 100,

            no_permission = 1111,
            data_invalid = 1112,
            dateTime_invalid = 1113,
            record_invalid = 1114,

            err_loginId_exist = 10101,
            err_email_exist = 10102,
            err_roleName_exist = 102001,

            // Other system code
            // error no assign pattern:
            // {section_no}+{000}

            err_not_enough_stcok = 90101,

            err_location_not_exist = 90201,

            err_member_not_exist = 90301,
            err_memberNo_exist = 90302,
            
            err_member_id = 90304,
            err_member_card_existusing = 90304,
            err_member_cardno_duplicate = 90304,
            err_member_password_invaild = 90305,

            err_product_not_exist = 301001,

            err_passcodeInvalid = 303001,
            err_passcodeQuantityInvalid = 303002,

            err_redeem_notEnoughPoint = 950001,
            err_redeem_hostIsRedeeming = 950002

        };

        public enum ReminderEngineType : int
        {
            ProductPurchase = 1,
            GiftRedeem = 2,
            Birthday = 3,
            LocationVisit = 4,
            MemberInactive = 5
        }

        public static Dictionary<int, string> SystemWord = new Dictionary<int, string>()
        {
            { 0, "undefine"},
            { 100, "normal"},
            { 1111, "You do not have permission."},
            { 1112, "Record Invalid"},
        };

        public enum SortOrder
        {
            asc, desc
        }

        public enum LocationType : int
        {
            productShop = 1,
            giftShop = 2,
            serviceCenter = 3
        }

        public enum PromotionRuleType : int
        {
            purchase = 1, redeem = 2, complementary = 3, servicePayment = 4
        }

        public enum PromotionRuleTransactionCriteria : int
        {
            singleTransaction = 1,
            multiTransaction = 2
        }

        public enum PromotionRulePurchaseProductTargetType : int
        {
            productCategory = 1,
            product = 2,
            any = 3
        }

        public enum PromotionRulePurchaseProductCriteriaType : int
        {
            quantity = 1,
            point = 2
        }

        public enum PromotionRuleEarnPointType : int
        {
            discrete = 1,
            bonus_percent = 2
        }

        public enum GiftRedeemStatus : int
        {
            voided = -1,
            waiting_collect = 1,
            collected = 2
            // confirmed = 3   //?
        }

        public enum GiftRedeemChannel : int
        {
            web = 1,
            mobile = 2,
            cms = 3
        }

        public enum BasicRuleType : int
        {
            RetailPurchase = 1,
            PostPaidService = 2,
            PrePaidService = 3
        }

        public static class Default
        {
            public static DateTime point_expiry_date = new DateTime(2099, 12, 31, 12, 0, 0);
        }

        public enum PointStauts : int
        {
            realized = 1,
            unrealized = 2
        }

        public enum PaymentStauts : int
        {
            complete = 1,
            incomplete = 2
        }

        public enum MemberImportType : int
        {
            insert = 1,
            update = 2
        }

        public enum PromotionRuleServicePaymentCriteria : int
        {
            point = 1,
            payment = 2
        }

        public enum MemberCardStatus : int
        {
            waiting_issue = 1,
            issued = 2,
            voided = 3
        }

        public enum CombineRedemptionStatus : int
        {
            connecting = 1,
            voided = 2,
            completed = 3
        }

        public static class Cryptography
        {
            public const string tl_key = "k*UOlPaq091IU)^brm910*@23EW42ast"; //32 chr shared ascii string (32 * 8 = 256 bit)
            public const string tl_iv = "Yn230*pkdj)64qom";  //16 chr shared ascii string (16 * 8 = 128 bit)
        }

        public enum MemberAdvanceSearchField : int
        {
            availablePoint = 1,
            birthdayMonth = 2,
            email = 3,
            gender = 4,
            hkid = 5,
            lastPurchaseDate = 6,
            locationVisit = 7,
            locationVisitDate = 8,
            memberCode = 9,
            memberLevel = 10,
            missionDate = 11,
            mobileNo = 12,
            name = 13,
            purchaseAmount = 14,
            purchaseCategory = 15,
            purchaseItem = 16,
            purchaseItemCount = 17,
            purchaseDate = 18,
            registrationDate = 19,
            redeemCategory = 20,
            redeemDate = 21,
            redeemItem = 22,
            redeemItemCount = 23,
            status = 24
        }

        public enum CompareCondition : int
        {
            is_ = 1,
            like = 2,
            lessThan = 3,
            lessOrEqual = 4,
            equal = 5,
            largerOrEqual = 6,
            largerThan = 7
        }
    }
}