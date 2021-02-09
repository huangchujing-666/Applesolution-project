using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palmary.Loyalty.BO.DataTransferObjects.Security
{
    public class AccessObject
    {
        public int type { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public int actionChannel { get; set; }
        public int languageID { get; set; }
        public string ip { get; set; }
        public DateTime loginTime { get; set; }
    }
}
