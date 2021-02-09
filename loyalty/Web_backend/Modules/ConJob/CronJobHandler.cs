using Palmary.Loyalty.BO.Modules.ScheduleJob.CronJob;
using Palmary.Loyalty.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Palmary.Loyalty.Web_backend.Modules.ConJob
{
    public class CronJobHandler : IHandler
    {
        /// <summary>
        ///  获得列表页面json字符串
        /// </summary>
        /// <param name="prefix_id"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="rowLimit"></param>
        /// <param name="searchParmList"></param>
        /// <param name="sortColumn"></param>
        /// <param name="sortOrder"></param>
        /// <param name="rowTotal"></param>
        /// <returns>返回json 字符串</returns>
        public string LoadListDataToExtJSJson(int prefix_id, int startRowIndex, int rowLimit, List<BO.DataTransferObjects.DB.SearchParmObject> searchParmList, string sortColumn, Common.CommonConstant.SortOrder sortOrder, ref int rowTotal)
        {
            CronJobManager cronManager = new CronJobManager();

            var resultCode = CommonConstant.SystemCode.undefine;
            var resultList = cronManager.GetList(startRowIndex, rowLimit, searchParmList, sortColumn, sortOrder, ref resultCode, ref rowTotal);
      
            var resultDataList = resultList.Select(x => new
            {
                //id, Name, Status, Execute Date, Complete Date, Result, Processed, Success, Fail, Message
                cronjob_id = x.cronjob_id,
                name = x.name,
                status = x.status,
                execute_date = x.execute_date,// == null ? "" : x.execute_date.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                complete_date = x.complete_date,// == null ? "" : x.complete_date.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"), 
                execute_result = x.execute_result,
                no_of_processd = x.no_of_processd.ToString(),
                no_of_success = x.no_of_success.ToString(),
                no_of_fail = x.no_of_fail.ToString(),
                execute_message = x.execute_message

            });
            return resultList.ToJson();
        }
    }
}