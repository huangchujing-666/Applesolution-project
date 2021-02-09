using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Palmary.Loyalty.BO.DataTransferObjects.Media;

namespace Palmary.Loyalty.BO.DataTransferObjects.Product
{
    public class ProductObject
    {
        public int product_id { get; set; }

        public string product_no { get; set; }
        public List<ProductCategoryObject> category_list { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public double price { get; set; }
        public double point { get; set; }
        public int consumption_period { get; set; }
        public int lost_customer_period { get; set; }
        
        public int status { get; set; }
        public string status_name { get; set; }

        public DateTime crt_date { get; set; }
        public DateTime upd_date { get; set; }
        public int crt_by_type { get; set; }
        public int crt_by { get; set; }
        public int upd_by_type { get; set; }
        public int upd_by { get; set; }
        public int record_status { get; set; }

        public List<ProductLangObject> lang_list { get; set; }
        public List<PhotoObject> photo_list { get; set; }
        public string file_name { get; set; }
        public string file_extension { get; set; }

        // additional
        // for break down
        public string category_name { get; set; }
        public int category_id { get; set; }
    }


    public class ProductObjectByCategory : ProductObject
    {
        public int category_id { get; set; }
        public int display_order { get; set; }
    }
}
