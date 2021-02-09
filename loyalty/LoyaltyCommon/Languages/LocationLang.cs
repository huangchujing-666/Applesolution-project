using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Palmary.Loyalty.Common.Languages
{
    public class LocationLang
    {
        public LocationLang(int _lang_id, int _location_id, string _name, string _description, string _operation_info, string _address_unit, string _address_building, string _address_street)
        {
            lang_id = _lang_id;
            location_id = _location_id;

            name = _name;
            description = description;
            operation_info = operation_info;
            address_unit = address_unit;
            address_building = _address_building;
            address_street = _address_street;
        }

        public int lang_id { get; set; }
        public int location_id { get; set; }
        
        public string name { get; set; }
        public string description { get; set; }
        public string operation_info { get; set; }
        public string address_unit { get; set; }
        public string address_building { get; set; }
        public string address_street { get; set; }
    }
}