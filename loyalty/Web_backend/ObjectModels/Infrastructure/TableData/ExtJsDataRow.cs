using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;



namespace Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.TableData
{
    public class ExtJsDataRowListItem
    {
        public int id { get; set; }
        public string value { get; set; }
    }

    public class ExtJsDataRowFunction
    {
        public string CSS { get; set; }
        public string name { get; set; }
        public int delete_status { get; set; }
        public int insert_status { get; set; }
        public int read_status { get; set; }
        public int update_status { get; set; }

        public string status { get; set; }
        public int id { get; set; }
        public string href { get; set; }
    }

    public class ExtJsDataRowUser
    {
        public int id { get; set; }
        public string email { get; set; }
        public string name { get; set; }
        public string status { get; set; }
        public string action_ip { get; set; }
        public int status_id { get; set; }
        public string login_id { get; set; }
        public string href { get; set; }
    }

    public class ExtJsDataRowRole
    {
        public int id { get; set; }
        public string name { get; set; }
        public string status { get; set; }
        public string href { get; set; }
    }

    public class ExtJsDataRowRole_DropDownList
    {
        public int role_Id { get; set; }
        public string role_name { get; set; }
    }

    public class ExtJsDataRowRoleDetail
    {
        public int menuId { get; set; }
        public string menuName { get; set; }
        public int menuLevel { get; set; }
        public string rightR { get; set; }
        public string rightU { get; set; }
        public string rightD { get; set; }
        public string rightI { get; set; }
        public string rightLog { get; set; }
        public string leaf { get; set; }
    }

    public class ExtJsDataRowMember
    {
        public int member_id { get; set; }
        public string member_no { get; set; }
        public string email { get; set; }
        public string mobile_no { get; set; }
        public string hkid { get; set; }
        public string name { get; set; }
        public string available_point { get; set; }
        public string member_level_name { get; set; }
        
        //for ExtJS
        public int id { get; set; }
        public string href { get; set; }
    }

    public class ExtJsDataRowMemberLevel
    {     
        public int level_id { get; set; }
        public string name { get; set; }
        public double point_required { get; set; }
        public string redeem_discount { get; set; }
        public string status { get; set; } 

        public string href { get; set; }
    }

    public class ExtJsDataRowProduct
    {
        public string product_no { get; set; }
        public string name { get; set; }
        public string category { get; set; }
        public string photo { get; set; }
        public string status { get; set; }
        public int display_order { get; set; }
        
        public int id { get; set; }
        public string href { get; set; }
    }

    public class ExtJsDataRowProductCategory
    {
        public string name { get; set; }
        public string photo { get; set; }
        public string status { get; set; }

        public int id { get; set; }
        public string href { get; set; }
    }

    public class ExtJsDataRowSetting
    {
        public string setting_name { get; set; }
        public string setting_value { get; set; }
        
        public int id { get; set; }
        public string href { get; set; }
    }

    public class ExtJsDataRowPasscode_prefix
    {
        public long prefix_id { get; set; }
        public int product_id { get; set; }
        public string product_name { get; set; }
        public long format_id { get; set; }
        public string passcode_format { get; set; }
        public string prefix_value { get; set; }

        public long current_generated { get; set; }
        public long maximum_generate { get; set; }
        
        public string usage_precent { get; set; }
        public string status { get; set; }
        public DateTime crt_date { get; set; }
        public DateTime upd_date { get; set; }
        public int crt_by { get; set; }
        public int upd_by { get; set; }
        public int record_status { get; set; }

        public string action { get; set; }
        public long id { get; set; }
        public string href { get; set; }
    }

    public class ExtJSField
    {
        public string name { get; set; }
        public string fieldLabel { get; set; }
        public string type  { get; set; }
        public int colspan { get; set; }
        public string tabIndex { get; set; }
        public bool readOnly { get; set; }
        public string value { get; set; }
        public string display_value { get; set; }
    }

    public class ExtJSField_hidden
    {
        public string type { get; set; }
        public string name { get; set; }
        public string value { get; set; }
    }

    // ExtJsDataRow
    public class ExtJsDataRowPasscode
    {
        public int product_id { get; set; }
        public int passcode_prefix_id { get; set; }
        public long passcode_id { get; set; }
        public string pin_value { get; set; }
        public string product_no { get; set; }
        public int category_id { get; set; }
        public string product_name { get; set; }
        public string description { get; set; }
        public double price { get; set; }
        public double point { get; set; }
        public string registered_name { get; set; }
        public string member_no { get; set; }
        public int consumption_period { get; set; }
        public int lost_customer_period { get; set; }
        public int display_order { get; set; }
        public int status { get; set; }

        public string active_date_str { get; set; }
        public string expiry_date_str { get; set; }

        public int crt_by { get; set; }
        public int upd_by { get; set; }
        public int record_status { get; set; }

        public long id { get; set; }
        public string href { get; set; } // for grid_type = title
        public string href1 { get; set; } // for grid_type = title1
    }
    
    // ExtJsDataRow
    public class ExtJsDataRowPasscode_generate 
    {
        public int generate_id { get; set; }
        public long noToGenerate { get; set; }
        public long generateCompleteCounter { get; set; }
        public long insertErrorCounter { get; set; }
        public string error_messgae { get; set; }
        public string generate_status_name { get; set; }
        public string crt_date_str { get; set; }
        public string upd_date_str { get; set; }
        
        public string crt_by_name { get; set; }

        public int id { get; set; }
        public string href { get; set; } // for grid_type = title
        public string href1 { get; set; } // for grid_type = title1
    }

    // ExtJsDataRow
    public class ExtJsDataRow_passcodeGenerateSummary 
    {
        public int product_id { get; set; }
        public string product_no { get; set; }
        public string product_name { get; set; }
        public long no_of_imported { get; set; }
        public long no_of_registered { get; set; }

        public int id { get; set; }
        public string href { get; set; } // for grid_type = title
    }

    public class ExtJsDataRow_product_custom_info
    {
        public string field_name { get; set; }
        public string field_value { get; set; }

        public int id { get; set; }
        public string href { get; set; }
    }

    // ExtJsDataRow
    public class ExtJsDataRowGift
    {
        public int gift_id { get; set; }

        public string gift_no { get; set; }
        public int category_id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public double point { get; set; }
        public int alert_level { get; set; }
        public double cost { get; set; }

        public bool discount { get; set; }
        public double discount_point { get; set; }
        public string discount_active_date { get; set; }
        public string discount_expiry_date { get; set; }

        public string redeem_active_date { get; set; }
        public string redeem_expiry_date { get; set; }

        public bool hot_item { get; set; }
        public DateTime hot_item_active_date { get; set; }
        public DateTime hot_item_expiry_date { get; set; }
        public int hot_item_display_order { get; set; }

        
        public bool display_public { get; set; }
        public string display_active_date { get; set; }
        public string display_expiry_date { get; set; }
        
        public int status { get; set; }
        public string status_name { get; set; }
        public int available_stock { get; set; }

        public DateTime crt_date { get; set; }
        public int crt_by_type { get; set; }
        public int crt_by { get; set; }
        public DateTime upd_date { get; set; }
        public int upd_by_type { get; set; }
        public int upd_by { get; set; }
        public int record_status { get; set; }

        public string photo { get; set; }
        public int id { get; set; }
        public string href { get; set; }
    }

    // ExtJsDataRow
    public class ExtJsDataRow_systemConfig
    {
        public int config_id { get; set; }
        public string name { get; set; }
        public string value { get; set; }
        public string action { get; set; }

        public int id { get; set; }
        public string href { get; set; }
    }

    // ExtJsDataRow: language
    public class ExtJsDataRow_language
    {
        public int lang_id { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public int display_order { get; set; }
        public int status { get; set; }
        //public DateTime crt_date { get; set; }
        //public int crt_by_type { get; set; }
        //public int crt_by { get; set; }
        //public DateTime upd_date { get; set; }
        //public int upd_by_type { get; set; }
        //public int upd_by { get; set; }
        //public int record_status { get; set; }

        public int id { get; set; }
        public string href { get; set; }
    }

    // ExtJsDataRow: log
    public class ExtJsDataRow_log
    {
        public long id;
        public string href;
        public string href1;

        public string action;
        public string crt_by_type_name;
        public string crt_by_name;
        public int access_obj;
        public string action_channel_name;
        public string action_type_name;
        public string target_obj_type_name;
        public string target_obj_name;
        public long target_obj;
        public string action_ip;
        public string log_date;

    }

    // ExtJsDataRow: transaction
    public class ExtJsDataRow_transaction
    {
        public int id;
        public int transaction_id;
        public string type_name;
        public string member_name;
        public string point_change;
        public string point_status;
        public string point_expiry_date;
        public string status_name;
        public string crt_date;
        public string href;
    }

    // ExtJsDataRow: transaction history
    public class ExtJsDataRow_transactionHistory
    {
        public int id;
        public int transaction_id;
        public string type_name;
        public string member_no;
        public string member_name;
        public string point_change;
        public string remark;
        public string crt_date;
        public string href;
        public string href1;
    }

    

    // ExtJsDataRow: member field
    public class ExtJsDataRow_memberField
    {
        public int id;

        public int column_id { get; set; }
        public string udd_column_name { get; set; }
        public int udd_column_id { get; set; }
        public string datatype_name { get; set; }
        public string datalength { get; set; }
        public string display_name { get; set; }
        public string remark { get; set; }
        public string crt_date { get; set; }
        public string crt_by_type { get; set; }
        public string crt_by { get; set; }
        public string upd_date { get; set; }
        public string upd_by_type { get; set; }
        public string upd_by { get; set; }
        public string record_status { get; set; }

        public string href { get; set; }
    }

    // ExtJsDataRow: member group
    public class ExtJsDataRow_memberGroup
    {
        public int id;

        public string group_name { get; set; }
        public string description { get; set; }

        public string crt_date { get; set; }
        public string crt_by_type { get; set; }
        public string crt_by { get; set; }
        public string upd_date { get; set; }
        public string upd_by_type { get; set; }
        public string upd_by { get; set; }
        public string record_status { get; set; }

        public string href { get; set; }
    }

    // ExtJsDataRow: file import job
    public class ExtJsDataRow_fileimportjob
    {
        public int id;

        public int job_id { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string schedule_date { get; set; }
        public string last_executed_date { get; set; }
        public string crt_date { get; set; }
        
        public string href { get; set; }
    }

    public class ExtJsDataRow_report
    {
        public int id;

        public string name { get; set; }
        public string type { get; set; }

        public string href { get; set; }
    }
}