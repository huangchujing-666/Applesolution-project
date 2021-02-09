using System;

namespace Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.PayoadKey
{
    public class PayloadKey<T>
    {
        private string _name;
        private string key;

        public PayloadKey(string name)
        {
            _name = name;
        }

        public PayloadKey(string name, string lang_code)
        {
            _name = name + "_" + lang_code;
        }

        public override string ToString()
        {
            return _name;
        }

        public PayloadKey<T> Add(string name)
        {
            return new PayloadKey<T>(_name + name);
        }
    }

    // Payload keys for general objects
    public static class PayloadKeys
    {
        public static PayloadKey<int> Id = new PayloadKey<int>("Id");
        public static PayloadKey<string> test = new PayloadKey<string>("test");
        public static PayloadKey<string> change_fields = new PayloadKey<string>("change_fields");

        // User
        public static PayloadKey<string> login_id = new PayloadKey<string>("login_id");

        // Member PayloadKey
        public static PayloadKey<int> Member_id = new PayloadKey<int>("member_id");
        public static PayloadKey<string> Member_no = new PayloadKey<string>("member_no");
        public static PayloadKey<string> Email = new PayloadKey<string>("Email");
        public static PayloadKey<string> fbid = new PayloadKey<string>("fbid");
        public static PayloadKey<string> fbemail = new PayloadKey<string>("fbemail");

        public static PayloadKey<int> Salutation = new PayloadKey<int>("Salutation");
        public static PayloadKey<string> firstname = new PayloadKey<string>("firstname");
        public static PayloadKey<string> middlename = new PayloadKey<string>("middlename");
        public static PayloadKey<string> lastname = new PayloadKey<string>("lastname");

        public static PayloadKey<string> Mobile_no = new PayloadKey<string>("Mobile_no");

        public static PayloadKey<DateTime> Birthday = new PayloadKey<DateTime>("Birthday");
        public static PayloadKey<int> birth_year = new PayloadKey<int>("Birth Year");
        public static PayloadKey<int> birth_month = new PayloadKey<int>("birth_month");
        public static PayloadKey<int> birth_day = new PayloadKey<int>("birth_day");

        public static PayloadKey<int> Gender = new PayloadKey<int>("Gender");
        public static PayloadKey<string> HKID = new PayloadKey<string>("HKID");
        public static PayloadKey<string> Address1 = new PayloadKey<string>("Address1");
        public static PayloadKey<string> Address2 = new PayloadKey<string>("Address2");
        public static PayloadKey<string> Address3 = new PayloadKey<string>("Address3");
        public static PayloadKey<int> District = new PayloadKey<int>("District");
        public static PayloadKey<int> Region = new PayloadKey<int>("Region");
                
        public static PayloadKey<int> Reg_source = new PayloadKey<int>("Reg_source");
        public static PayloadKey<int> Reg_status = new PayloadKey<int>("Reg_status");
        public static PayloadKey<string> Activate_key = new PayloadKey<string>("Activate_key");
        public static PayloadKey<string> Reg_ip = new PayloadKey<string>("Reg_ip");
        public static PayloadKey<string> Reg_date = new PayloadKey<string>("Reg_date");
        
        public static PayloadKey<int> Status = new PayloadKey<int>("status");
        public static PayloadKey<string> cP_ID = new PayloadKey<string>("CP_ID");
        public static PayloadKey<string> refferal_CP_ID = new PayloadKey<string>("refferal_CP_ID");
        public static PayloadKey<DateTime?> weddingDate = new PayloadKey<DateTime?>("weddingDate");
        public static PayloadKey<int> Opt_in = new PayloadKey<int>("Opt_in");
        public static PayloadKey<int> Member_level_id = new PayloadKey<int>("Member_level_id");
        public static PayloadKey<string> Member_level_name = new PayloadKey<string>("Member_level_name");
        public static PayloadKey<int> member_category_id = new PayloadKey<int>("member_category_id");

        public static PayloadKey<string> referrer_member_no = new PayloadKey<string>("referrer_member_no");

        public static PayloadKey<string> Password = new PayloadKey<string>("Password");
        public static PayloadKey<string> ConfirmPassword = new PayloadKey<string>("ConfirmPassword");

        public static PayloadKey<string> No = new PayloadKey<string>("No");
        public static PayloadKey<string> Filter = new PayloadKey<string>("filter");
        public static PayloadKey<string> Value = new PayloadKey<string>("value");
        public static PayloadKey<int> SettingType = new PayloadKey<int>("SettingType");
        public static PayloadKey<string> Code= new PayloadKey<string>("Code");
        
        public static PayloadKey<string> Description = new PayloadKey<string>("Description");

        public static PayloadKey<string> OperationInfo = new PayloadKey<string>("OperationInfo");
        public static PayloadKey<int> locationId=new PayloadKey<int>("locationId");
        public static PayloadKey<int> giftId = new PayloadKey<int>("giftId");
 
        public static PayloadKey<float> PointRequired = new PayloadKey<float>("PointRequired");
        public static PayloadKey<float> RedeemDiscount = new PayloadKey<float>("RedeemDiscount");
        public static PayloadKey<int> Display_order = new PayloadKey<int>("Display_order");
        public static PayloadKey<string> Photo=new PayloadKey<string>("Photo");
        public static PayloadKey<string> Phone=new PayloadKey<string>("Phone");
  
        public static PayloadKey<float> Latitude=new PayloadKey<float>("Latitude");
        public static PayloadKey<float> Longitude=new PayloadKey<float>("Longitude");
        public static PayloadKey<string> Fax=new PayloadKey<string>("Fax");
        public static PayloadKey<int> Catergory = new PayloadKey<int>("Catergory");
        public static PayloadKey<double> Point=new PayloadKey<double>("Point");
        
        public static PayloadKey<int> DiscountPoint = new PayloadKey<int>("DiscountPoint");
        public static PayloadKey<DateTime> DiscountActiveDate = new PayloadKey<DateTime>("DiscountActiveDate");
        public static PayloadKey<DateTime> DiscountExpiryDate = new PayloadKey<DateTime>("DiscountExpiryDate");
       
        public static PayloadKey<DateTime> HotItemActiveDate = new PayloadKey<DateTime>("HotItemActiveDate");
        public static PayloadKey<DateTime> HotItemExpiryDate = new PayloadKey<DateTime>("HotItemExpiryDate");
        public static PayloadKey<int> AlertLevel = new PayloadKey<int>("AlertLevel");

        public static PayloadKey<string> Dummy = new PayloadKey<string>("Dummy");

        // Passcode
        public static PayloadKey<DateTime> Active_date = new PayloadKey<DateTime>("active_date");
        public static PayloadKey<DateTime> Expiry_date = new PayloadKey<DateTime>("expiry_date");
        public static PayloadKey<float> Price=new PayloadKey<float>("Price");

        public static PayloadKey<long> passcode_id = new PayloadKey<long>("passcode_id");
        public static PayloadKey<string> pin_value = new PayloadKey<string>("pin_value");
        public static PayloadKey<string> passcode_format = new PayloadKey<string>("passcode_format");
        public static PayloadKey<string> passcode_prefix = new PayloadKey<string>("passcode_prefix");
        public static PayloadKey<string> prefix_value = new PayloadKey<string>("prefix_value");

        public static PayloadKey<int> format_id  = new PayloadKey<int>("format_id");
        public static PayloadKey<double> point_range_lower = new PayloadKey<double>("point_range_lower");
        public static PayloadKey<double> point_range_upper = new PayloadKey<double>("point_range_upper");

        public static PayloadKey<string> Oldpwd = new PayloadKey<string>("oldpwd");
        public static PayloadKey<string> Newpwd = new PayloadKey<string>("newpwd");
        public static PayloadKey<string> Confirmpwd = new PayloadKey<string>("confirmpwd");


        public static PayloadKey<string> MenuId = new PayloadKey<string>("menuId");
        public static PayloadKey<string> CheckRightValue = new PayloadKey<string>("checkRightValue");
        public static PayloadKey<string> RoleId = new PayloadKey<string>("roleId");

        public static PayloadKey<string> RoleName = new PayloadKey<string>("RoleName");

        public static PayloadKey<int> LevelId = new PayloadKey<int>("LevelId");
        public static PayloadKey<int> RedeemStatus = new PayloadKey<int>("RedeemStatus");

        // SystemConfig
        public static class SystemConfig
        {
            public static PayloadKey<int> config_id = new PayloadKey<int>("config_id");
            public static PayloadKey<string> value = new PayloadKey<string>("value");
        }

        // Product
        public static class Product
        {
            public static PayloadKey<int> product_id = new PayloadKey<int>("product_id");
            public static PayloadKey<string> category = new PayloadKey<string>("category");
            
            public static PayloadKey<int> Consumption_period = new PayloadKey<int>("Consumption_period");
            public static PayloadKey<int> Lost_customer_period = new PayloadKey<int>("Lost_customer_period");
            public static PayloadKey<int> quantity = new PayloadKey<int>("quantity");

            public static PayloadKey<string> photos = new PayloadKey<string>("photos");
        }

        // Product
        public static class ProductPurchase
        {
            public static PayloadKey<int> member_id = new PayloadKey<int>("member_id");
            public static PayloadKey<int> product_id = new PayloadKey<int>("product_id");
            public static PayloadKey<int> quantity = new PayloadKey<int>("quantity");

            public static PayloadKey<int> product_id_2 = new PayloadKey<int>("product_id_2");
            public static PayloadKey<int> quantity_2 = new PayloadKey<int>("quantity_2");

            public static PayloadKey<int> product_id_3 = new PayloadKey<int>("product_id_3");
            public static PayloadKey<int> quantity_3 = new PayloadKey<int>("quantity_3");

            public static PayloadKey<int> product_id_4 = new PayloadKey<int>("product_id_4");
            public static PayloadKey<int> quantity_4 = new PayloadKey<int>("quantity_4");

            public static PayloadKey<int> product_id_5 = new PayloadKey<int>("product_id_5");
            public static PayloadKey<int> quantity_5 = new PayloadKey<int>("quantity_5");

          
        }

        // payloadKey product_category
        public static class ProductCategory
        {
            public static PayloadKey<int> category_id = new PayloadKey<int>("category_id");
            public static PayloadKey<int> parent_id = new PayloadKey<int>("parent_id");
            public static PayloadKey<int> leaf = new PayloadKey<int>("leaf");
            public static PayloadKey<string> photo_file_name = new PayloadKey<string>("photo_file_name");
            public static PayloadKey<string> photo_file_extension = new PayloadKey<string>("photo_file_extension");
            public static PayloadKey<int> display_order = new PayloadKey<int>("display_order");
            public static PayloadKey<int> status = new PayloadKey<int>("status");
            public static PayloadKey<DateTime> crt_date = new PayloadKey<DateTime>("crt_date");
            public static PayloadKey<int> crt_by_type = new PayloadKey<int>("crt_by_type");
            public static PayloadKey<int> crt_by = new PayloadKey<int>("crt_by");
            public static PayloadKey<DateTime> upd_date = new PayloadKey<DateTime>("upd_date");
            public static PayloadKey<int> upd_by_type = new PayloadKey<int>("upd_by_type");
            public static PayloadKey<int> upd_by = new PayloadKey<int>("upd_by");
            public static PayloadKey<int> record_status = new PayloadKey<int>("record_status");

            public static PayloadKey<string> parent_id_name = new PayloadKey<string>("parent_id_name");
        }

        // payloadKey service category
        public static class ServiceCategory
        {
            public static PayloadKey<int> category_id = new PayloadKey<int>("category_id");
            public static PayloadKey<int> parent_id = new PayloadKey<int>("parent_id");
            public static PayloadKey<int> leaf = new PayloadKey<int>("leaf");
            public static PayloadKey<int> display_order = new PayloadKey<int>("display_order");
            public static PayloadKey<string> name = new PayloadKey<string>("name");
            public static PayloadKey<string> description = new PayloadKey<string>("description");
            public static PayloadKey<int> status = new PayloadKey<int>("status");
            public static PayloadKey<DateTime> crt_date = new PayloadKey<DateTime>("crt_date");
            public static PayloadKey<int> crt_by_type = new PayloadKey<int>("crt_by_type");
            public static PayloadKey<int> crt_by = new PayloadKey<int>("crt_by");
            public static PayloadKey<DateTime> upd_date = new PayloadKey<DateTime>("upd_date");
            public static PayloadKey<int> upd_by_type = new PayloadKey<int>("upd_by_type");
            public static PayloadKey<int> upd_by = new PayloadKey<int>("upd_by");
            public static PayloadKey<int> record_status = new PayloadKey<int>("record_status");

            // additional
            public static PayloadKey<string> parent_name = new PayloadKey<string>("parent_name");
        }

        // Gift
        public static class Gift
        {
            public static PayloadKey<int> gift_id = new PayloadKey<int>("gift_id");
            public static PayloadKey<string> gift_no = new PayloadKey<string>("gift_no");
            public static PayloadKey<int> category_id = new PayloadKey<int>("category_id");
            public static PayloadKey<double> point = new PayloadKey<double>("point");
            public static PayloadKey<int> alert_level = new PayloadKey<int>("alert_level");
            public static PayloadKey<double> cost = new PayloadKey<double>("cost");

            public static PayloadKey<string> name = new PayloadKey<string>("name");
            public static PayloadKey<string> description = new PayloadKey<string>("description");

            public static PayloadKey<bool> discount = new PayloadKey<bool>("discount");
            public static PayloadKey<double> discount_point = new PayloadKey<double>("discount_point");
            public static PayloadKey<DateTime> discount_date_range = new PayloadKey<DateTime>("discount_date_range");
            public static PayloadKey<DateTime> discount_date_range_from = new PayloadKey<DateTime>("discount_date_range_from");
            public static PayloadKey<DateTime> discount_date_range_to = new PayloadKey<DateTime>("discount_date_range_to");

            public static PayloadKey<DateTime> discount_date_range_time_from = new PayloadKey<DateTime>("discount_date_range_time_from");
            public static PayloadKey<DateTime> discount_date_range_time_to = new PayloadKey<DateTime>("discount_date_range_time_to");

            public static PayloadKey<bool> hotItem = new PayloadKey<bool>("hotItem");
            public static PayloadKey<DateTime> hotItem_date_range = new PayloadKey<DateTime>("hotItem_date_range");
            public static PayloadKey<DateTime> hotItem_date_range_from = new PayloadKey<DateTime>("hotItem_date_range_from");
            public static PayloadKey<DateTime> hotItem_date_range_to = new PayloadKey<DateTime>("hotItem_date_range_to");
            public static PayloadKey<DateTime> hotItem_date_range_time_from = new PayloadKey<DateTime>("hotItem_date_range_time_from");
            public static PayloadKey<DateTime> hotItem_date_range_time_to = new PayloadKey<DateTime>("hotItem_date_range_time_to");

            public static PayloadKey<int> hotItem_display_order = new PayloadKey<int>("hotItem_display_order");

            public static PayloadKey<bool> display_public = new PayloadKey<bool>("display_public");
            
            public static PayloadKey<DateTime> display_date_range = new PayloadKey<DateTime>("display_date_range");
            public static PayloadKey<DateTime> display_date_range_from = new PayloadKey<DateTime>("display_date_range_from");
            public static PayloadKey<DateTime> display_date_range_to = new PayloadKey<DateTime>("display_date_range_to");
            public static PayloadKey<DateTime> display_date_range_time_from = new PayloadKey<DateTime>("display_date_range_time_from");
            public static PayloadKey<DateTime> display_date_range_time_to = new PayloadKey<DateTime>("display_date_range_time_to");

            public static PayloadKey<DateTime> redeem_date_range = new PayloadKey<DateTime>("redeem_date_range");
            public static PayloadKey<DateTime> redeem_date_range_from = new PayloadKey<DateTime>("redeem_date_range_from");
            public static PayloadKey<DateTime> redeem_date_range_to = new PayloadKey<DateTime>("redeem_date_range_to");
            public static PayloadKey<DateTime> redeem_date_range_time_from = new PayloadKey<DateTime>("redeem_date_range_time_from");
            public static PayloadKey<DateTime> redeem_date_range_time_to = new PayloadKey<DateTime>("redeem_date_range_time_to");
            
            public static PayloadKey<int> status = new PayloadKey<int>("status");

            public static PayloadKey<string> location = new PayloadKey<string>("location");
            public static PayloadKey<string> member_privilege = new PayloadKey<string>("member_privilege");
            public static PayloadKey<string> photos = new PayloadKey<string>("Photos");

            public static PayloadKey<string> current_stock = new PayloadKey<string>("current_stock");
            public static PayloadKey<string> redeem_count = new PayloadKey<string>("redeem_count");
        }

        // GiftCategory
        public static class GiftCategory
        {
            public static PayloadKey<int> category_id = new PayloadKey<int>("category_id");
            public static PayloadKey<int> parent_id = new PayloadKey<int>("parent_id");
            public static PayloadKey<int> leaf = new PayloadKey<int>("leaf");
            public static PayloadKey<string> parent_id_name = new PayloadKey<string>("parent_id_name");
            public static PayloadKey<string> photo_file_name = new PayloadKey<string>("photo_file_name");
            public static PayloadKey<string> photo_file_extension = new PayloadKey<string>("photo_file_extension");
            public static PayloadKey<int> display_order = new PayloadKey<int>("display_order");
            public static PayloadKey<int> status = new PayloadKey<int>("status");
            public static PayloadKey<DateTime> crt_date = new PayloadKey<DateTime>("crt_date");
            public static PayloadKey<int> crt_by_type = new PayloadKey<int>("crt_by_type");
            public static PayloadKey<int> crt_by = new PayloadKey<int>("crt_by");
            public static PayloadKey<DateTime> upd_date = new PayloadKey<DateTime>("upd_date");
            public static PayloadKey<int> upd_by_type = new PayloadKey<int>("upd_by_type");
            public static PayloadKey<int> upd_by = new PayloadKey<int>("upd_by");
            public static PayloadKey<int> record_status = new PayloadKey<int>("record_status");
        }

        // Member
        public static class Member
        {
            public static PayloadKey<double> point_current = new PayloadKey<double>("point_current");
            public static PayloadKey<double> point_current_unrealized = new PayloadKey<double>("point_current_unrealized");
            public static PayloadKey<double> point_earned = new PayloadKey<double>("point_earned");
            public static PayloadKey<double> point_used = new PayloadKey<double>("point_used");
            public static PayloadKey<double> point_expired = new PayloadKey<double>("point_expired");
            public static PayloadKey<double> point_expiring_2month = new PayloadKey<double>("point_expiring_2month");
        }

        // MemberCategory
        public static class MemberCategory
        {
            public static PayloadKey<int> category_id = new PayloadKey<int>("category_id");
            public static PayloadKey<int> parent_id = new PayloadKey<int>("parent_id");
            public static PayloadKey<int> leaf = new PayloadKey<int>("leaf");
            public static PayloadKey<string> parent_id_name = new PayloadKey<string>("parent_id_name");
            
            public static PayloadKey<int> display_order = new PayloadKey<int>("display_order");
            public static PayloadKey<int> status = new PayloadKey<int>("status");
            public static PayloadKey<DateTime> crt_date = new PayloadKey<DateTime>("crt_date");
            public static PayloadKey<int> crt_by_type = new PayloadKey<int>("crt_by_type");
            public static PayloadKey<int> crt_by = new PayloadKey<int>("crt_by");
            public static PayloadKey<DateTime> upd_date = new PayloadKey<DateTime>("upd_date");
            public static PayloadKey<int> upd_by_type = new PayloadKey<int>("upd_by_type");
            public static PayloadKey<int> upd_by = new PayloadKey<int>("upd_by");
            public static PayloadKey<int> record_status = new PayloadKey<int>("record_status");
        }

        // Location
        public static class Location
        {
            public static PayloadKey<int> location_id = new PayloadKey<int>("location_id");
            public static PayloadKey<string> location_no = new PayloadKey<string>("location_no");
            public static PayloadKey<string> photo_file_name = new PayloadKey<string>("photo_file_name");
            public static PayloadKey<string> photo_file_extension = new PayloadKey<string>("photo_file_extension");
            public static PayloadKey<double> latitude = new PayloadKey<double>("latitude");
            public static PayloadKey<double> longitude = new PayloadKey<double>("longitude");
            public static PayloadKey<string> phone = new PayloadKey<string>("phone");
            public static PayloadKey<string> fax = new PayloadKey<string>("fax");
            public static PayloadKey<int> address_district = new PayloadKey<int>("address_district");
            public static PayloadKey<int> address_region = new PayloadKey<int>("address_region");
            public static PayloadKey<int> display_order = new PayloadKey<int>("display_order");
            public static PayloadKey<int> status = new PayloadKey<int>("status");
            public static PayloadKey<DateTime> crt_date = new PayloadKey<DateTime>("crt_date");
            public static PayloadKey<int> crt_by_type = new PayloadKey<int>("crt_by_type");
            public static PayloadKey<int> crt_by = new PayloadKey<int>("crt_by");
            public static PayloadKey<DateTime> upd_date = new PayloadKey<DateTime>("upd_date");
            public static PayloadKey<int> upd_by_type = new PayloadKey<int>("upd_by_type");
            public static PayloadKey<int> upd_by = new PayloadKey<int>("upd_by");
            public static PayloadKey<int> record_status = new PayloadKey<int>("record_status");  
        }

        // Transaction_history
        public static class TransactionEarn
        {
            public static PayloadKey<int> transaction_id = new PayloadKey<int>("transaction_id");
            public static PayloadKey<int> transaction_type = new PayloadKey<int>("transaction_type");
            public static PayloadKey<long> earn_source_id = new PayloadKey<long>("earn_source_id");

            public static PayloadKey<int> member_id = new PayloadKey<int>("member_id");
            public static PayloadKey<double> point_earn = new PayloadKey<double>("point_earn");
            public static PayloadKey<double> point_used = new PayloadKey<double>("point_used");
            public static PayloadKey<int> status = new PayloadKey<int>("status");
            public static PayloadKey<DateTime> void_date = new PayloadKey<DateTime>("void_date");
            public static PayloadKey<string> void_reason = new PayloadKey<string>("void_reason");
            public static PayloadKey<bool> display = new PayloadKey<bool>("display");
            public static PayloadKey<DateTime> crt_date = new PayloadKey<DateTime>("crt_date");
            public static PayloadKey<int> crt_by_type = new PayloadKey<int>("crt_by_type");
            public static PayloadKey<int> crt_by = new PayloadKey<int>("crt_by");
            public static PayloadKey<DateTime> upd_date = new PayloadKey<DateTime>("upd_date");
            public static PayloadKey<int> upd_by_type = new PayloadKey<int>("upd_by_type");
            public static PayloadKey<int> upd_by = new PayloadKey<int>("upd_by");
            public static PayloadKey<int> record_status = new PayloadKey<int>("record_status");
            //---
            public static PayloadKey<string> pin_value = new PayloadKey<string>("pin_value");
        }

        public static class GiftRedemption
        {
            public static PayloadKey<int> redemption_id = new PayloadKey<int>("redemption_id");
            public static PayloadKey<int> member_id = new PayloadKey<int>("member_id");
         
            public static PayloadKey<double> point_used = new PayloadKey<double>("point_used");
            public static PayloadKey<long> transaction_use_id = new PayloadKey<long>("transaction_use_id");
            public static PayloadKey<int> redemption_status = new PayloadKey<int>("redemption_status");
            public static PayloadKey<int> redemption_source = new PayloadKey<int>("redemption_source");
            public static PayloadKey<DateTime> collect_date = new PayloadKey<DateTime>("collect_date");
            
            public static PayloadKey<DateTime> void_date = new PayloadKey<DateTime>("void_date");
            public static PayloadKey<int> void_reason = new PayloadKey<int>("void_reason");
            public static PayloadKey<int> void_staff = new PayloadKey<int>("void_staff");
            public static PayloadKey<int> status = new PayloadKey<int>("status");
            public static PayloadKey<DateTime> crt_date = new PayloadKey<DateTime>("crt_date");
            public static PayloadKey<int> crt_by_type = new PayloadKey<int>("crt_by_type");
            public static PayloadKey<int> crt_by = new PayloadKey<int>("crt_by");
            public static PayloadKey<DateTime> upd_date = new PayloadKey<DateTime>("upd_date");
            public static PayloadKey<int> upd_by_type = new PayloadKey<int>("upd_by_type");
            public static PayloadKey<int> upd_by = new PayloadKey<int>("upd_by");
            public static PayloadKey<int> record_status = new PayloadKey<int>("record_status");

            public static PayloadKey<int> gift_id = new PayloadKey<int>("gift_id");
            public static PayloadKey<int> quantity = new PayloadKey<int>("quantity");
            public static PayloadKey<int> location_id = new PayloadKey<int>("location_id");

            public static PayloadKey<int> gift_id_2 = new PayloadKey<int>("gift_id_2");
            public static PayloadKey<int> quantity_2 = new PayloadKey<int>("quantity_2");
            public static PayloadKey<int> location_id_2 = new PayloadKey<int>("location_id_2");

            public static PayloadKey<int> gift_id_3 = new PayloadKey<int>("gift_id_3");
            public static PayloadKey<int> quantity_3 = new PayloadKey<int>("quantity_3");
            public static PayloadKey<int> location_id_3 = new PayloadKey<int>("location_id_3");

            public static PayloadKey<int> gift_id_4 = new PayloadKey<int>("gift_id_4");
            public static PayloadKey<int> quantity_4 = new PayloadKey<int>("quantity_4");
            public static PayloadKey<int> location_id_4 = new PayloadKey<int>("location_id_4");

            public static PayloadKey<int> gift_id_5 = new PayloadKey<int>("gift_id_5");
            public static PayloadKey<int> quantity_5 = new PayloadKey<int>("quantity_5");
            public static PayloadKey<int> location_id_5 = new PayloadKey<int>("location_id_5");
        }

        // user_profile
        public static class UserProfile
        {
            public static PayloadKey<int> user_id = new PayloadKey<int>("user_id");
            public static PayloadKey<string> login_id = new PayloadKey<string>("login_id");
            public static PayloadKey<string> password = new PayloadKey<string>("password");
            public static PayloadKey<string> name = new PayloadKey<string>("name");
            public static PayloadKey<string> email = new PayloadKey<string>("email");
            public static PayloadKey<int> status = new PayloadKey<int>("status");
            public static PayloadKey<string> action_ip = new PayloadKey<string>("action_ip");
            public static PayloadKey<DateTime> action_date = new PayloadKey<DateTime>("action_date");
            public static PayloadKey<DateTime> crt_date = new PayloadKey<DateTime>("crt_date");
            public static PayloadKey<DateTime> upd_date = new PayloadKey<DateTime>("upd_date");
            public static PayloadKey<int> crt_by = new PayloadKey<int>("crt_by");
            public static PayloadKey<int> upd_by = new PayloadKey<int>("upd_by");
            public static PayloadKey<int> record_status = new PayloadKey<int>("record_status");

            public static PayloadKey<string> role = new PayloadKey<string>("role");
        }

        // Role
        public static class Role
        {
            public static PayloadKey<int> role_id = new PayloadKey<int>("role_id");
            public static PayloadKey<string> name = new PayloadKey<string>("name");
            public static PayloadKey<int> status = new PayloadKey<int>("status");
            public static PayloadKey<DateTime> crt_date = new PayloadKey<DateTime>("crt_date");
            public static PayloadKey<int> crt_by_type = new PayloadKey<int>("crt_by_type");
            public static PayloadKey<int> crt_by = new PayloadKey<int>("crt_by");
            public static PayloadKey<DateTime> upd_date = new PayloadKey<DateTime>("upd_date");
            public static PayloadKey<int> upd_by_type = new PayloadKey<int>("upd_by_type");
            public static PayloadKey<int> upd_by = new PayloadKey<int>("upd_by");
            public static PayloadKey<int> record_status = new PayloadKey<int>("record_status");
        }

        // Role Privilege
        public static class RolePrivilege
        {
            public static PayloadKey<int> roleId = new PayloadKey<int>("roleId");
            public static PayloadKey<string> checkRightValue = new PayloadKey<string>("checkRightValue");
            public static PayloadKey<string> checkRightName = new PayloadKey<string>("checkRightName");
        }

        // var with lang
        public static PayloadKey<string> varWithLang(string var_name, string lang_code)
        {
            return new PayloadKey<string>(var_name, lang_code);
        }

        // payloadKey gift_inventory
        public static class Gift_inventory
        {
            public static PayloadKey<int> gift_inventory_id = new PayloadKey<int>("gift_inventory_id");
            public static PayloadKey<int> gift_id = new PayloadKey<int>("gift_id");
            public static PayloadKey<int> stock_change = new PayloadKey<int>("stock_change");
            public static PayloadKey<int> type_id = new PayloadKey<int>("type_id");
            public static PayloadKey<string> remark = new PayloadKey<string>("remark");
            public static PayloadKey<int> status = new PayloadKey<int>("status");
            public static PayloadKey<DateTime> crt_date = new PayloadKey<DateTime>("crt_date");
            public static PayloadKey<int> crt_by_type = new PayloadKey<int>("crt_by_type");
            public static PayloadKey<int> crt_by = new PayloadKey<int>("crt_by");
            public static PayloadKey<DateTime> upd_date = new PayloadKey<DateTime>("upd_date");
            public static PayloadKey<int> upd_by_type = new PayloadKey<int>("upd_by_type");
            public static PayloadKey<int> upd_by = new PayloadKey<int>("upd_by");
            public static PayloadKey<int> record_status = new PayloadKey<int>("record_status");

            public static PayloadKey<string> gift_name = new PayloadKey<string>("gift_name");
        }

        // payloadKey point adjustment
        public static class PointAdjustment
        {
            public static PayloadKey<double> point = new PayloadKey<double>("point");
            public static PayloadKey<string> remark = new PayloadKey<string>("remark");
            public static PayloadKey<int> member_id = new PayloadKey<int>("member_id");
        }
		
		// payloadKey point transfer
        public static class PointTransfer
        {
            public static PayloadKey<double> point = new PayloadKey<double>("point");
            public static PayloadKey<string> remark = new PayloadKey<string>("remark");
            public static PayloadKey<int> member_id = new PayloadKey<int>("member_id");
            public static PayloadKey<string> to_member_no = new PayloadKey<string>("to_member_no");
        }

        // service
        public static class ServicePlan
        {
            public static PayloadKey<int> plan_id = new PayloadKey<int>("plan_id");
            public static PayloadKey<string> plan_no = new PayloadKey<string>("plan_no");
            public static PayloadKey<int> type = new PayloadKey<int>("type");
            public static PayloadKey<string> name = new PayloadKey<string>("name");
            public static PayloadKey<string> description = new PayloadKey<string>("description");
            public static PayloadKey<double> fee = new PayloadKey<double>("fee");
            public static PayloadKey<int> point_expiry_month = new PayloadKey<int>("point_expiry_month");
            public static PayloadKey<double> ratio_payment = new PayloadKey<double>("ratio_payment");
            public static PayloadKey<double> ratio_point = new PayloadKey<double>("ratio_point");

            public static PayloadKey<int> status = new PayloadKey<int>("status");
            public static PayloadKey<DateTime> crt_date = new PayloadKey<DateTime>("crt_date");
            public static PayloadKey<int> crt_by_type = new PayloadKey<int>("crt_by_type");
            public static PayloadKey<int> crt_by = new PayloadKey<int>("crt_by");
            public static PayloadKey<DateTime> upd_date = new PayloadKey<DateTime>("upd_date");
            public static PayloadKey<int> upd_by_type = new PayloadKey<int>("upd_by_type");
            public static PayloadKey<int> upd_by = new PayloadKey<int>("upd_by");
            public static PayloadKey<int> record_status = new PayloadKey<int>("record_status");
        }

        // member import
        public static class MemberImport
        {
            public static PayloadKey<string> template = new PayloadKey<string>("template");
            public static PayloadKey<string> fileData = new PayloadKey<string>("fileData");

            public static PayloadKey<int> importType = new PayloadKey<int>("importType");
        }

        // transaction import
        public static class TransactionImport
        {
            public static PayloadKey<string> template = new PayloadKey<string>("template");
            public static PayloadKey<string> fileData = new PayloadKey<string>("fileData");

            public static PayloadKey<int> importType = new PayloadKey<int>("importType");
        }

        public static class WifiLocation
        {
            public static PayloadKey<int> location_id = new PayloadKey<int>("location_id");
            public static PayloadKey<string> location_no = new PayloadKey<string>("location_no");
            public static PayloadKey<string> name = new PayloadKey<string>("name");
            public static PayloadKey<string> mac_address = new PayloadKey<string>("mac_address");
            public static PayloadKey<double> point = new PayloadKey<double>("point");
            public static PayloadKey<int> status = new PayloadKey<int>("status");
            public static PayloadKey<DateTime> crt_date = new PayloadKey<DateTime>("crt_date");
            public static PayloadKey<int> crt_by_type = new PayloadKey<int>("crt_by_type");
            public static PayloadKey<int> crt_by = new PayloadKey<int>("crt_by");
            public static PayloadKey<DateTime> upd_date = new PayloadKey<DateTime>("upd_date");
            public static PayloadKey<int> upd_by_type = new PayloadKey<int>("upd_by_type");
            public static PayloadKey<int> upd_by = new PayloadKey<int>("upd_by");
            public static PayloadKey<int> record_status = new PayloadKey<int>("record_status");

            public static PayloadKey<string> member_level = new PayloadKey<string>("member_level");

            
        }

        // ExtJsDataRow
        public static class MemberCard
        {
            public static PayloadKey<int> card_id = new PayloadKey<int>("card_id");
            public static PayloadKey<int> member_id = new PayloadKey<int>("member_id");
            public static PayloadKey<string> card_no = new PayloadKey<string>("card_no");
            public static PayloadKey<int> card_version = new PayloadKey<int>("card_version");
            public static PayloadKey<int> card_type = new PayloadKey<int>("card_type");
            public static PayloadKey<int> card_status = new PayloadKey<int>("card_status");
            public static PayloadKey<DateTime?> issue_date = new PayloadKey<DateTime?>("issue_date");
            public static PayloadKey<int> old_card_id = new PayloadKey<int>("old_card_id");
            public static PayloadKey<string> remark = new PayloadKey<string>("remark");
            public static PayloadKey<int> status = new PayloadKey<int>("status");
            public static PayloadKey<DateTime> crt_date = new PayloadKey<DateTime>("crt_date");
            public static PayloadKey<int> crt_by_type = new PayloadKey<int>("crt_by_type");
            public static PayloadKey<int> crt_by = new PayloadKey<int>("crt_by");
            public static PayloadKey<DateTime> upd_date = new PayloadKey<DateTime>("upd_date");
            public static PayloadKey<int> upd_by_type = new PayloadKey<int>("upd_by_type");
            public static PayloadKey<int> upd_by = new PayloadKey<int>("upd_by");
            public static PayloadKey<int> record_status = new PayloadKey<int>("record_status");

            public static PayloadKey<string> member_no = new PayloadKey<string>("member_no");
        }

        // ExtJsDataRow
        public static class Demo1
        {
            public static PayloadKey<int> id = new PayloadKey<int>("id");
            public static PayloadKey<int> demo_id = new PayloadKey<int>("demo_id");
            public static PayloadKey<string> demo_name = new PayloadKey<string>("demo_name");
            public static PayloadKey<float> demo_float = new PayloadKey<float>("demo_float");
            public static PayloadKey<int> demo_select = new PayloadKey<int>("demo_select");
            public static PayloadKey<DateTime> demo_date = new PayloadKey<DateTime>("demo_date");
            public static PayloadKey<DateTime> demo_datetime = new PayloadKey<DateTime>("demo_datetime");
            public static PayloadKey<string> demo_remark = new PayloadKey<string>("demo_remark");
            public static PayloadKey<string> demo_remark2 = new PayloadKey<string>("demo_remark2");

            // test
            public static PayloadKey<int> test = new PayloadKey<int>("test");

            public static PayloadKey<string> testAjax1 = new PayloadKey<string>("testAjax1");
            public static PayloadKey<string> testAjax2 = new PayloadKey<string>("testAjax2");
            public static PayloadKey<string> testAjax3 = new PayloadKey<string>("testAjax3");
            public static PayloadKey<string> testAjax4 = new PayloadKey<string>("testAjax4");

            public static PayloadKey<string> test_time = new PayloadKey<string>("test_time");
        }

         // ExtJsDataRow
        public static class ReminderEngine
        {
            public static PayloadKey<int> reminder_engine_id = new PayloadKey<int>("reminder_engine_id");
            public static PayloadKey<string> name = new PayloadKey<string>("name");
            public static PayloadKey<int> target_type = new PayloadKey<int>("target_type");
            public static PayloadKey<int> target_id = new PayloadKey<int>("target_id");
            public static PayloadKey<int> status = new PayloadKey<int>("status");

            public static PayloadKey<DateTime> crt_date = new PayloadKey<DateTime>("crt_date");
            public static PayloadKey<int> crt_by_type = new PayloadKey<int>("crt_by_type");
            public static PayloadKey<int> crt_by = new PayloadKey<int>("crt_by");
            public static PayloadKey<DateTime> upd_date = new PayloadKey<DateTime>("upd_date");
            public static PayloadKey<int> upd_by_type = new PayloadKey<int>("upd_by_type");
            public static PayloadKey<int> upd_by = new PayloadKey<int>("upd_by");
            public static PayloadKey<int> record_status = new PayloadKey<int>("record_status");

            //additional
            public static PayloadKey<int> target_value_purchase = new PayloadKey<int>("target_value_purchase");
            public static PayloadKey<int> target_value_redeem = new PayloadKey<int>("target_value_redeem");
            public static PayloadKey<int> target_value_locationVisit = new PayloadKey<int>("target_value_locationVisit");
            public static PayloadKey<int> target_value_memberInactive = new PayloadKey<int>("target_value_memberInactive");

            public static PayloadKey<int> day = new PayloadKey<int>("day");
            public static PayloadKey<int> template_type = new PayloadKey<int>("template_type");
            public static PayloadKey<int> template_id = new PayloadKey<int>("template_id");
            public static PayloadKey<int> day1 = new PayloadKey<int>("day1");
            public static PayloadKey<int> template_type1 = new PayloadKey<int>("template_type1");
            public static PayloadKey<int> template_id1 = new PayloadKey<int>("template_id1");
            public static PayloadKey<int> day2 = new PayloadKey<int>("day2");
            public static PayloadKey<int> template_type2 = new PayloadKey<int>("template_type2");
            public static PayloadKey<int> template_id2 = new PayloadKey<int>("template_id2");
            public static PayloadKey<int> day3 = new PayloadKey<int>("day3");
            public static PayloadKey<int> template_type3 = new PayloadKey<int>("template_type3");
            public static PayloadKey<int> template_id3 = new PayloadKey<int>("template_id3");
            public static PayloadKey<int> day4 = new PayloadKey<int>("day4");
            public static PayloadKey<int> template_type4 = new PayloadKey<int>("template_type4");
            public static PayloadKey<int> template_id4 = new PayloadKey<int>("template_id4");
            public static PayloadKey<int> day5 = new PayloadKey<int>("day5");
            public static PayloadKey<int> template_type5 = new PayloadKey<int>("template_type5");
            public static PayloadKey<int> template_id5 = new PayloadKey<int>("template_id5");
            public static PayloadKey<int> day6 = new PayloadKey<int>("day6");
            public static PayloadKey<int> template_type6 = new PayloadKey<int>("template_type6");
            public static PayloadKey<int> template_id6 = new PayloadKey<int>("template_id6");
            public static PayloadKey<int> day7 = new PayloadKey<int>("day7");
            public static PayloadKey<int> template_type7 = new PayloadKey<int>("template_type7");
            public static PayloadKey<int> template_id7 = new PayloadKey<int>("template_id7");
            public static PayloadKey<int> day8 = new PayloadKey<int>("day8");
            public static PayloadKey<int> template_type8 = new PayloadKey<int>("template_type8");
            public static PayloadKey<int> template_id8 = new PayloadKey<int>("template_id8");
            public static PayloadKey<int> day9 = new PayloadKey<int>("day9");
            public static PayloadKey<int> template_type9 = new PayloadKey<int>("template_type9");
            public static PayloadKey<int> template_id9 = new PayloadKey<int>("template_id9");
        }

        // ExtJsDataRow
        public static class ReminderTemplate
        {
            public static PayloadKey<int> reminder_template_id = new PayloadKey<int>("reminder_template_id");
            public static PayloadKey<string> name = new PayloadKey<string>("name");
            public static PayloadKey<string> sms_template = new PayloadKey<string>("sms_template");
            public static PayloadKey<string> email_template = new PayloadKey<string>("email_template");
            public static PayloadKey<int> status = new PayloadKey<int>("status");
            public static PayloadKey<DateTime> crt_date = new PayloadKey<DateTime>("crt_date");
            public static PayloadKey<int> crt_by_type = new PayloadKey<int>("crt_by_type");
            public static PayloadKey<int> crt_by = new PayloadKey<int>("crt_by");
            public static PayloadKey<DateTime> upd_date = new PayloadKey<DateTime>("upd_date");
            public static PayloadKey<int> upd_by_type = new PayloadKey<int>("upd_by_type");
            public static PayloadKey<int> upd_by = new PayloadKey<int>("upd_by");
            public static PayloadKey<int> record_status = new PayloadKey<int>("record_status");
        }
    }
}