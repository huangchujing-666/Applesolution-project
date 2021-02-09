using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Palmary.Loyalty.Common;
using Palmary.Loyalty.BO.Database;
using Palmary.Loyalty.BO.Modules.Transaction;
using Palmary.Loyalty.BO.DataTransferObjects.DB;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.ExtJsEntity;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.PayoadKey;
using Palmary.Loyalty.Web_backend.Modules.Utility;
using Palmary.Loyalty.Web_backend.Modules.GlobalHandlers;
using Palmary.Loyalty.BO.Modules.Utility;

namespace Palmary.Loyalty.Web_backend.Modules.Transaction
{
    public class TransactionImportHandler : IHandler
    {
        public string LoadListDataToExtJSJson(int prefix_id, int startRowIndex, int rowLimit, List<SearchParmObject> searchParmList, string sortColumn, CommonConstant.SortOrder sortOrder, ref int rowTotal)
        {
            var transactionImportManager = new TransactionImportManager();
            FileHandler _fileHandler = new FileHandler();

            var resultCode = CommonConstant.SystemCode.undefine;
            var resultList = transactionImportManager.GetList(startRowIndex, rowLimit, searchParmList, sortColumn, sortOrder, ref resultCode, ref rowTotal).ToList();
            var resultDataList = resultList.Select(
                x => new
                {
                    transaction_type_name = x.transaction_type_name,
                    import_file = "<a href='" + PathHandler.GetStorage_relativePath(CommonConstant.Module.transactionImport, x.file_name) + "'>" + x.file_name + "</a>",
                    no_of_data_row = x.no_of_dataRow,
                    no_of_imported = x.no_of_imported,
                    no_of_fail_record = x.no_of_failRecord,
                    remark = x.remark,
                    crt_date = x.crt_date.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                    crt_by_name = x.crt_by_name,
                    
                    fail_report = x.no_of_failRecord > 0 ? "<a href='" + PathHandler.GetStorage_relativePath_changeExt(CommonConstant.Module.transactionImport, x.file_name, ".csv") + "'>download</a>" : "",
                    href = "new com.embraiz.tag().openNewTag('EDIT_UID:" + x.crt_by + "','User: " + x.crt_by_name + "','com.palmary.user.js.edit','iconRole16','iconRole16','iconRole16', '" + x.crt_by + "')"
                }
            );

            return resultDataList.ToJson();
        }
    }
}