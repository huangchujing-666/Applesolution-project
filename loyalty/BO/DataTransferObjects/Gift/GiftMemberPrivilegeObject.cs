using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palmary.Loyalty.BO.DataTransferObjects.Gift
{
    public class GiftMemberPrivilegeObject
    {
        public int id { get; set; }
        public int member_level_id { get; set; }
        public int allow_redeem { get; set; }
    }
}
