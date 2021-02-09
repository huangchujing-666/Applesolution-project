using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Palmary.Loyalty.BO.DataTransferObjects.Media;

namespace Palmary.Loyalty.BO.DataTransferObjects.Gift
{
    public class GiftObject
    {
        public int gift_id { get; set; }
        public string gift_no { get; set; }
        public List<GiftCategoryObject> category_list { get; set; }

        public double point { get; set; }
        public int alert_level { get; set; }
        public double cost { get; set; }

        public bool discount { get; set; }
        public double? discount_point { get; set; }
        public DateTime? discount_active_date { get; set; }
        public DateTime? discount_expiry_date { get; set; }

        public bool hot_item { get; set; }
        public DateTime? hot_item_active_date { get; set; }
        public DateTime? hot_item_expiry_date { get; set; }
        public int hot_item_display_order { get; set; }

        public bool display_public { get; set; }
        public DateTime display_active_date { get; set; }
        public DateTime display_expiry_date { get; set; }
        public DateTime redeem_active_date { get; set; }
        public DateTime redeem_expiry_date { get; set; }

        public int status { get; set; }
        public int available_stock { get; set; }

        public int record_status { get; set; }

        public List<GiftLocationObject> location_list { get; set; }
        public List<GiftMemberPrivilegeObject> member_privilege_list { get; set; }
        public List<GiftLangObject> lang_list { get; set; }

        public List<PhotoObject> photo_list { get; set; }

        public string name { get; set; }
        public string status_name { get; set; }
        public string file_name { get; set; }
        public string file_extension { get; set; }
    }

    public class GiftObjectByCategory : GiftObject
    {
        public int category_id { get; set; }
        public int display_order { get; set; }
    }
}