using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palmary.Loyalty.BO.DataTransferObjects.Product
{
    public class ProductPurchaseObject
    {
        public int purchase_id { get; set; }
        public int order_id { get; set; }
        public int transaction_id { get; set; }
        public double point { get; set; }
        public int promotion_transaction_id { get; set; }
        public int member_id { get; set; }
        public int product_id { get; set; }
        public int quantity { get; set; }
        public double total_amount { get; set; }
        public DateTime purchase_date { get; set; }
        
        public int status { get; set; }
        public DateTime crt_date { get; set; }
        public int crt_by_type { get; set; }
        public int crt_by { get; set; }
        public DateTime upd_date { get; set; }
        public int upd_by_type { get; set; }
        public int upd_by { get; set; }
        public int record_status { get; set; }

        //-- Additional Info
        public string product_name { get; set; }
        public string product_no { get; set; }
        public double point_earned { get; set; }
        public string point_status_name { get; set; }
        public DateTime point_expiry_date { get; set; }

        
    }
}
