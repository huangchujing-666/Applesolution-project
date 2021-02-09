using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palmary.Loyalty.BO.DataTransferObjects.Product
{
    public class ProductLangObject
    {
        public int product_lang_id { get; set; }

        public int lang_id { get; set; }
        public int product_id { get; set; }

        public string name { get; set; }
        public string description { get; set; }
        public int status { get; set; }
    }
}
