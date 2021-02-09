using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.Objects;
using Palmary.Loyalty.Common;
using Palmary.Loyalty.BO.Database;
using Palmary.Loyalty.BO.DataTransferObjects.DB;
using Palmary.Loyalty.BO.DataTransferObjects.Security;
using Palmary.Loyalty.BO.Modules.Administration.Security;
using Palmary.Loyalty.BO.Modules.Administration.SystemControl;
using Palmary.Loyalty.BO.Modules.Transaction;
using Palmary.Loyalty.BO.Modules.Member;
using Palmary.Loyalty.BO.DataTransferObjects.Form;
using Palmary.Loyalty.BO.DataTransferObjects.Member;
using Palmary.Loyalty.BO.DataTransferObjects.Transaction;
using Palmary.Loyalty.BO.DataTransferObjects.Gift;
using Palmary.Loyalty.BO.DataTransferObjects.Role;
using Palmary.Loyalty.BO.DataTransferObjects.CSVExport;
using Palmary.Loyalty.BO.Modules.Gift;

using System.IO;
using CsvHelper;

namespace Palmary.Loyalty.BO.Modules.Export
{
    public class ExportManager
    {
        private static LoyaltyDBDataContext db = new LoyaltyDBDataContext();

        public CommonConstant.SystemCode ExportMember(string writeFullPath, List<SearchParmObject> searchParmList)
        {
            var systemCode = CommonConstant.SystemCode.undefine;

            // init csv writer
            var streamWriter = new StreamWriter(writeFullPath, false, Encoding.UTF8);
            var csvWriter = new CsvWriter(streamWriter);

            // write header
            csvWriter.WriteRecord(new MemberExportObject.Header());

            // write data
            var memberManager = new MemberManager();
            var dataList = memberManager.GetListAll(searchParmList, ref systemCode);

            foreach (var x in dataList)
            {
                csvWriter.WriteRecord(x);
            }

            // release writer
            csvWriter.Dispose();

            return CommonConstant.SystemCode.normal;
        }

        public CommonConstant.SystemCode ExportTransactionHistory(string writeFullPath, List<SearchParmObject> searchParmList)
        {
            var systemCode = CommonConstant.SystemCode.undefine;

            // init csv writer
            var streamWriter = new StreamWriter(writeFullPath, false, Encoding.UTF8);
            var csvWriter = new CsvWriter(streamWriter);

            // write header
            csvWriter.WriteRecord(new TransactionExportObject.Header());

            // write data
            var transactionManager = new TransactionManager();
            var dataList = transactionManager.GetListAll(searchParmList, ref systemCode);

            foreach (var x in dataList)
            {
                csvWriter.WriteRecord(x);
            }

            // release writer
            csvWriter.Dispose();

            return CommonConstant.SystemCode.normal;
        }

        public CommonConstant.SystemCode ExportGiftRedemptionHistory(string writeFullPath, List<SearchParmObject> searchParmList)
        {
            var systemCode = CommonConstant.SystemCode.undefine;

            // init csv writer
            var streamWriter = new StreamWriter(writeFullPath, false, Encoding.UTF8);
            var csvWriter = new CsvWriter(streamWriter);

            // write header
            csvWriter.WriteRecord(new GiftRedemptionExportObject.Header());

            // write data
            var giftRedemptionManager = new GiftRedemptionManager();
            var dataList = giftRedemptionManager.GetList_whole(searchParmList, ref systemCode);

            foreach (var x in dataList)
            {
                csvWriter.WriteRecord(x);
            }

            // release writer
            csvWriter.Dispose();

            return CommonConstant.SystemCode.normal;
        }

        public CommonConstant.SystemCode ExportMemberLevelChange(string writeFullPath, List<SearchParmObject> searchParmList)
        {
            var systemCode = CommonConstant.SystemCode.undefine;

            // init csv writer
            var streamWriter = new StreamWriter(writeFullPath, false, Encoding.UTF8);
            var csvWriter = new CsvWriter(streamWriter);

            // write header
            csvWriter.WriteRecord(new MemberLevelChangeExportObject.Header());

            // write data
            var memberLevelChangeManager = new MemberLevelChangeManager();
            var dataList = memberLevelChangeManager.GetListExport(true, ref systemCode);

            foreach (var x in dataList)
            {
                csvWriter.WriteRecord(x);
            }

            // release writer
            csvWriter.Dispose();

            return CommonConstant.SystemCode.normal;
        }
    }
}
