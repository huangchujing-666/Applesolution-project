using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palmary.Loyalty.BO.DataTransferObjects.Wifi
{
   public  class WifiAccessReportObject
    {
       public class ByLocation
       {
           public int location_id { get; set; }
           public string location_no { get; set; }
           public string location_name { get; set; }

           public int count { get; set; }
       }

       public class ByMemberLevel
       {
           public int level_id { get; set; }
           public string level_name { get; set; }
           public int display_order { get; set; }
           public int count { get; set; }
       }
    }
}
