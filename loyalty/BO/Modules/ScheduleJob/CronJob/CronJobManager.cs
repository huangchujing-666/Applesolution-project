using Palmary.Loyalty.BO.Database;
using Palmary.Loyalty.BO.DataTransferObjects;
using Palmary.Loyalty.BO.DataTransferObjects.Cronjob;
using Palmary.Loyalty.BO.DataTransferObjects.DB;
using Palmary.Loyalty.BO.DataTransferObjects.Role;
using Palmary.Loyalty.BO.DataTransferObjects.Security;
using Palmary.Loyalty.BO.Modules.Administration.Security;
using Palmary.Loyalty.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Palmary.Loyalty.BO.Modules.Utility;

namespace Palmary.Loyalty.BO.Modules.ScheduleJob.CronJob
{
    public class CronJobManager
    {
        private static LoyaltyDBDataContext db = new LoyaltyDBDataContext();
        private AccessManager _accessManager;
        private string _module = CommonConstant.Module.cronjob;
        private RolePrivilegeObject _privilege;
        private static LogManager _logManager;
        private AccessObject _accessObject;

        public CronJobManager()
        {
            // access object
            _accessManager = new AccessManager();
            _accessObject = _accessManager.GetAccessObjectBySession();

            // privilege and log
            _privilege = _accessManager.AccessModule(_module);
            _logManager = new LogManager();
        }

        public CronJobManager(AccessObject accessObject)
        {
            // access object
            _accessManager = new AccessManager();
            _accessObject = accessObject;

            // privilege and log
            _privilege = _accessManager.AccessModule(_module, _accessObject.type, _accessObject.id);
            _logManager = new LogManager(_accessObject);
        }

        /// <summary>
        /// 獲取CronjobObject對象集合  
        /// </summary>
        /// <param name="rowIndexStart">開始行</param>
        /// <param name="rowLimit">獲取行數</param>
        /// <param name="searchParmList">搜索參數</param>
        /// <param name="sortColumn">排序的列</param>
        /// <param name="sortOrder">排序方式</param>
        /// <param name="systemCode"></param>
        /// <param name="totalRow">總行數</param>
        /// <returns></returns>
        public List<CronjobObject> GetList(int rowIndexStart, int rowLimit, List<SearchParmObject> searchParmList, string sortColumn, CommonConstant.SortOrder sortOrder, ref CommonConstant.SystemCode systemCode, ref int totalRow)
        {
            systemCode = CommonConstant.SystemCode.undefine;
            var resultList=new List<CronjobObject>();
            //可讀權限
            if (_privilege.read_status == 1)
            {
                var query = (from c in db.cronjob
                             join li in db.listing_items on c.status equals li.value
                             join l in db.listing_items on c.execute_result equals l.value
                             where (c.status != (int)CommonConstant.RecordStatus.deleted
                               && li.list_id == (int)CommonConstant.ListingType.Status && l.list_id == (int)CommonConstant.ListingType.CronjobResult
                             )

                             select new CronjobObject
                             {
                                 cronjob_id = c.cronjob_id,
                                 name = c.name,
                                 execute_message = c.execute_message,
                                 execute_result = l.name ,
                                 no_of_fail = c.no_of_fail,
                                 no_of_success = c.no_of_success,
                                 no_of_processd = c.no_of_processd,
                                 record_status = c.record_status,
                                 execute_date = c.execute_date,
                                 complete_date = c.complete_date,
                                 crt_by = c.crt_by,
                                 crt_by_type = c.crt_by_type,
                                 crt_date = c.crt_date,
                                 status = c.status,
                                 upd_by = c.upd_by,
                                 upd_by_type = c.upd_by_type,
                                 upd_date = c.upd_date,
                                 status_name = li.name
                             });


                                
                //模糊搜索
                //foreach (var f in searchParmList)
                //{
                //    if (!string.IsNullOrWhiteSpace(f.value))
                //    {
                //        if (f.property=="cronjob_id")
                //        {
                //            query = query.Where(x=>x.cronjob_id.ToString().Contains(f.value));
                //        }
                //        else if (f.property == "name")
                //        {
                //            query = query.Where(x=>x.name.Contains(f.value));
                //        }
                //        else if (f.property=="execute_message")
                //        {
                //            query = query.Where(x=>x.execute_message.Contains(f.value));
                //        }
                //        else if (f.property == "status")
                //        {
                //            query = query.Where(x => x.status == int.Parse(f.value));
                //        }

                //    }
                //}

                var orderByColumn = "";
                if (sortColumn == "cronjob_id" || sortColumn == "name" || sortColumn == "execute_date" || sortColumn == "complete_date" || sortColumn == "status" || sortColumn == "execute_message")
                {
                    orderByColumn = sortColumn;
                }
                else
                {
                    //default
                    sortOrder = CommonConstant.SortOrder.asc;
                    //sortOrder = CommonConstant.SortOrder.desc;
                    orderByColumn = "cronjob_id";
                }

                // row total
                totalRow = query.Count();

                if (sortOrder == CommonConstant.SortOrder.desc)
                {
                    resultList =query.OrderBy(orderByColumn, true).Skip(rowIndexStart).Take(rowLimit).ToList();
                }
                else
                {
                    resultList = query.OrderBy(orderByColumn, false).Skip(rowIndexStart).Take(rowLimit).ToList();
                }
                   systemCode = CommonConstant.SystemCode.normal;

            }
            else
            {
                resultList = new List<CronjobObject>();
                systemCode = CommonConstant.SystemCode.no_permission;
            }
            return resultList;
        }
    }
}
