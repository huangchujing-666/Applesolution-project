using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Palmary.Loyalty.BO.DataTransferObjects.DB;
using Palmary.Loyalty.Common;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.TableData;

namespace Palmary.Loyalty.Web_backend.Modules.Report
{
    public class ReportHandler : IHandler
    {
        public string LoadListDataToExtJSJson(int prefix_id, int startRowIndex, int rowLimit, List<SearchParmObject> searchParmList, string sortColumn, CommonConstant.SortOrder sortOrder, ref int rowTotal)
        {
            //GiftManager _giftManager = new GiftManager();

            //var resultCode = CommonConstant.SystemCode.undefine;
            //var resultList = _giftManager.GetList(startRowIndex, rowLimit, searchParmList, sortColumn, sortOrder, ref resultCode, ref rowTotal).ToList();

            var list = new List<ExtJsDataRow_report> { };

            var id = 1;
            list.Add(new ExtJsDataRow_report
            {
                id = 1,
                name = "Member Level distribution",
                type = "Normal Report",
                href = "new com.embraiz.tag().openNewTag('Report:" + id + "','Report: " + "member distribution" + "','com.palmary.report.js.memberlevel','iconRole16','iconRole16','iconRole16','" + id + "')"
            });

            id = 2;
            list.Add(new ExtJsDataRow_report
            {
                id = id,
                name = "Member Upgrade",
                type = "Normal Report",
                href = "new com.embraiz.tag().openNewTag('Report:" + id + "','Report: " + "member upgrade" + "','com.palmary.report.js.memberUpgrade','iconRole16','iconRole16','iconRole16','" + id + "')"
            });

            id = 3;
            list.Add(new ExtJsDataRow_report
            {
                id = id,
                name = "Member Upgrade List",
                type = "Normal Report",
                href = "new com.embraiz.tag().openNewTag('Report:" + id + "','Report: " + "member upgrade list" + "','com.palmary.memberlevelchange.js.index','iconRole16','iconRole16','iconRole16','" + id + "')"
            });


            id = 4;
            list.Add(new ExtJsDataRow_report
            {
                id = id,
                name = "Member Profile Demographic",
                type = "Normal Report",
                href = "new com.embraiz.tag().openNewTag('Report:" + id + "','Report: " + "Member Profile Demographic" + "','com.palmary.report.js.memberProfileDistribution','iconRole16','iconRole16','iconRole16','" + id + "')"
            });

            id = 5;
            list.Add(new ExtJsDataRow_report
            {
                id = id,
                name = "Top Redeem Report",
                type = "Normal Report",
                href = "new com.embraiz.tag().openNewTag('Report:" + id + "','Report: " + "Top Redeem Report" + "','com.palmary.report.js.topRedeemReport','iconRole16','iconRole16','iconRole16','" + id + "')"
            });
            rowTotal = list.Count();

            id = 6;
            list.Add(new ExtJsDataRow_report
            {
                id = id,
                name = "Sales Statistic",
                type = "Normal Report",
                href = "new com.embraiz.tag().openNewTag('Report:" + id + "','Report: " + "Sales Statistic Report" + "','com.palmary.report.js.salesReport','iconRole16','iconRole16','iconRole16','" + id + "')"
            });
            rowTotal = list.Count();

            //-- advance
            id = 101;
            list.Add(new ExtJsDataRow_report
            {
                id = id,
                name = "Member Average Lifetime",
                type = "Advance Report",
                href = "new com.embraiz.tag().openNewTag('Report:" + id + "','Report: " + "member average lifetime" + "','com.palmary.report.js.memberaveragelifetime','iconRole16','iconRole16','iconRole16','" + id + "')"
            });

            id = 102;
            list.Add(new ExtJsDataRow_report
            {
                id = id,
                name = "Point Report",
                type = "Advance Report",
                href = "new com.embraiz.tag().openNewTag('Report:" + id + "','Report: " + "point report" + "','com.palmary.report.js.point','iconRole16','iconRole16','iconRole16','" + id + "')"
            });

            id = 103;
            list.Add(new ExtJsDataRow_report
            {
                id = id,
                name = "Member Gain Lost Report",
                type = "Advance Report",
                href = "new com.embraiz.tag().openNewTag('Report:" + id + "','Report: " + "Member Gain Lost Report" + "','com.palmary.report.js.memberGainLost','iconRole16','iconRole16','iconRole16','" + id + "')"
            });

       

            return list.ToJson();
        }
    }
}