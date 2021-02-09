using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data;

namespace Palmary.Loyalty.Web_backend.Modules.Utility
{
    public class jsonFormatter
    {
        public String extJSFormat(DataSet inputDataSet)
        {
            String resultJson;

            resultJson = "{'success':true,'items':[";
            for (int i = 0; i < inputDataSet.Tables[0].Rows.Count; i++)
            {
                for (int j = 0; j < inputDataSet.Tables[0].Rows[i].ItemArray.Length; j++)
                {

                    switch (j)
                    {
                        case 0:
                            resultJson += "{'username':";
                            resultJson += "'" + inputDataSet.Tables[0].Rows[i].ItemArray[j].ToString() + "',";
                            break;
                        case 1:
                            resultJson += "'email':";
                            resultJson += "'" + inputDataSet.Tables[0].Rows[i].ItemArray[j].ToString() + "',";
                            break;

                        case 2:
                            resultJson += "'mobile_no':";
                            resultJson += "'" + inputDataSet.Tables[0].Rows[i].ItemArray[j].ToString() + "',";
                            break;
                        case 3:
                            resultJson += "'hkid':";
                            resultJson += "'" + inputDataSet.Tables[0].Rows[i].ItemArray[j].ToString() + "',";
                            break;
                        case 4:
                            resultJson += "'chi_name':";
                            resultJson += "'" + inputDataSet.Tables[0].Rows[i].ItemArray[j].ToString() + "',";
                            break;
                        case 5:
                            resultJson += "'href':\"new com.embraiz.tag().openNewTag('Customer Profile:1','Customer Profile:test','com.palmary.customerProfile.js.edit','iconRole16','iconRole16','iconRole16','8')\",";
                            resultJson += "'id':";
                            resultJson += inputDataSet.Tables[0].Rows[i].ItemArray[j].ToString();
                            if (i < inputDataSet.Tables[0].Rows.Count - 1)
                                resultJson += "},";
                            else
                                resultJson += "}],'totalCount':0}";

                            break;
                        default:
                            resultJson += "'unknown':";
                            break;
                    }
                }
            }
            return resultJson;
        }
    }
}