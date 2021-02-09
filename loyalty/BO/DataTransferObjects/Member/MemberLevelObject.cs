using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palmary.Loyalty.BO.DataTransferObjects.Member
{
    public class MemberLevelObject
    {
        public int level_id { get; set; }
        public string name { get; set; }
        public double point_required { get; set; }
        public double redeem_discount { get; set; }
        public int display_order { get; set; }
        public int status { get; set; }
        public DateTime crt_date { get; set; }
        public DateTime upd_date { get; set; }
        public int crt_by { get; set; }
        public int upd_by { get; set; }
    
        // additional
        public string status_name { get; set; }
        
    }
}
