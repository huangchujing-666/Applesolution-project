using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Palmary.Loyalty.BO.DataTransferObjects.Form;
using Palmary.Loyalty.BO.DataTransferObjects.DB;
using Newtonsoft.Json;

namespace Palmary.Loyalty.Web_backend.Modules.Utility
{
    public class JsonExtractHelper
    {
        public class ChangedFields
        {
            public ChangedField[] ChangedFieldObject;
        }

        public ChangedField[] ExtJSFormChangedFields(string jsonValue)
        {
            //convert the embraiz extjs json into corrected json
            //var correctedJSON = "{'ChangedFieldObject':[" + jsonValue + "]}";

            //System.Diagnostics.Debug.WriteLine(correctedJSON, "correctedJSON");

            //correctedJSON = correctedJSON.Replace(",]}", "]}");
            //correctedJSON = correctedJSON.Replace("},]", "}]");

            //System.Diagnostics.Debug.WriteLine(correctedJSON);
            //var results = JsonConvert.DeserializeObject<ChangedFields>(correctedJSON);
            //return results.ChangedFieldObject;

            // temp for support for the form with multi_upload field
            var result = new ChangedField[1];
            result[0] = new ChangedField() { field_name = "must_change", newValue = "must_change", oldValue = "must_change" };

            return result;
        }

        public class MutliPhotoField
        {
            public PhotoField[] photoField_list;
        }

        public PhotoField[] ExtJSFormPhotosField(string jasonValue)
        {
            // convert the embraiz extjs json into corrected json
            var correctedJSON = "{'photoField_list':" + jasonValue + "}";
            //correctedJSON = correctedJSON.Replace(",]}", "]}");
            System.Diagnostics.Debug.WriteLine(correctedJSON);
            var results = JsonConvert.DeserializeObject<MutliPhotoField>(correctedJSON);

            return results.photoField_list;
        }

        public List<SearchParmObject> ExtJSFormSearchParm(string jasonValue)
        {
            System.Diagnostics.Debug.WriteLine(jasonValue);
            var results = JsonConvert.DeserializeObject<List<SearchParmObject>>(jasonValue);

            return results;
        }
    }
}