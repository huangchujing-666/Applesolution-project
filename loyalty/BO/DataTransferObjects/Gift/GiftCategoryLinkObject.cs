using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palmary.Loyalty.BO.DataTransferObjects.Gift
{
    public class GiftCategoryLinkObject
    {
        public int gift_id { get; set; }
        public int category_id { get; set; }
        public string gift_name { get; set; }
        public string category_name { get; set; }
        public int display_order { get; set; }
    }
}
