using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Palmary.Loyalty.Common.Languages
{
    public class GiftLang
    {
        public GiftLang(int _lang_id, int _gift_id, string _name, string _desc, int _status)
        { 
            lang_id = _lang_id;
            gift_id = _gift_id;

            name = _name;
            description = _desc;
            status = _status;
        }

        public int lang_id { get; set; }
        public int gift_id { get; set; }

        public string name { get; set; }
        public string description { get; set; }
        public int status { get; set; }
    }
}