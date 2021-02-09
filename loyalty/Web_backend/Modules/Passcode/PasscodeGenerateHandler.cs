using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Palmary.Loyalty.BO.Modules.Passcode;
using Palmary.Loyalty.BO.Modules.Administration.Security;
using Palmary.Loyalty.Web_backend.Modules.GlobalHandlers;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.TableData;
using Palmary.Loyalty.Web_backend.Modules.Utility;
using Palmary.Loyalty.Web_backend.ObjectModels;
using Palmary.Loyalty.Common;
using Palmary.Loyalty.BO.DataTransferObjects.DB;

namespace Palmary.Loyalty.Web_backend.Modules.Product
{
    public class PasscodeGenerateHandler : IHandler
    {
        public string LoadListDataToExtJSJson(int prefix_id, int startRowIndex, int rowLimit, List<SearchParmObject> searchParmList, string sortColumn, CommonConstant.SortOrder sortOrder, ref int rowTotal)
        {
            PasscodeGenerateManager _passcodeGenerateManager = new PasscodeGenerateManager();
            var resultList = _passcodeGenerateManager.GetPasscodeGenerateLists(SessionManager.Current.obj_id, startRowIndex, rowLimit).ToList();

            var list = new List<ExtJsDataRowPasscode_generate> { };
            int insertCounter = 0;
            var counter = 0;
        
            foreach (var item in resultList)
            {
                counter++;
                rowTotal = item.rowTotal ?? 0;

                list.Insert(insertCounter, new ExtJsDataRowPasscode_generate
                {
                    id = item.generate_id,
                    generate_id = item.generate_id,
                    noToGenerate = item.noToGenerate,
                    generateCompleteCounter = item.generateCompleteCounter,
                    insertErrorCounter = item.insertErrorCounter,
                    error_messgae = item.error_messgae,
                    generate_status_name = item.generate_status_name,
                    crt_date_str = item.crt_date.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                    
                    crt_by_name = item.crt_by_name,

                    href = "new com.embraiz.tag().openNewTag('EDIT_UID:" + item.user_id + "','User: " + item.crt_by_name + "','com.palmary.user.js.edit.extend','iconRole16','iconRole16','iconRole16','" + item.user_id + "')",
                    href1 = "new com.embraiz.tag().openNewTag('EDIT_UID:" + item.user_id + "','User: " + item.crt_by_name + "','com.palmary.user.js.edit.extend','iconRole16','iconRole16','iconRole16','" + item.user_id + "')" 
                });
                insertCounter++;
            }
            
            return list.ToJson();
        }
    }
}