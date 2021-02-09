using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Palmary.Loyalty.BO.Database;
using System.Data.Objects;
using Palmary.Loyalty.BO.Modules.Administration.Security;
using Palmary.Loyalty.Common;
using Palmary.Loyalty.BO.DataTransferObjects.DB;
using Palmary.Loyalty.BO.DataTransferObjects.Role;
using Palmary.Loyalty.BO.DataTransferObjects.Language;
using Palmary.Loyalty.BO.Modules.Utility;

namespace Palmary.Loyalty.BO.Modules.Administration.Section
{
    public class LanguageManager
    {
        private static LoyaltyDBDataContext db = new LoyaltyDBDataContext();
        
        private AccessManager _accessManager;
        private RolePrivilegeObject _privilege;
        private string _module = CommonConstant.Module.language;
        private static LogManager _logManager;

        // init and load privilege
        public LanguageManager()
        {
            _accessManager = new AccessManager();
            _privilege = _accessManager.AccessModule(_module);
            _logManager = new LogManager();
        }

        // Get List with dyncmic search, paging, ordering
        // With permission check and log 
        // LINQ to SQL directly
        public List<LanguageObject> GetList(int rowIndexStart, int rowLimit, List<SearchParmObject> searchParmList, string sortColumn, CommonConstant.SortOrder sortOrder, ref CommonConstant.SystemCode systemCode, ref int totalRow)
        {
            systemCode = CommonConstant.SystemCode.undefine;
            var resultList = new List<LanguageObject>();

            if (_privilege.read_status == 1)
            {
                var query = (from l in db.languages

                             where (
                                l.record_status != (int)CommonConstant.RecordStatus.deleted
                                )
                             select new LanguageObject
                             {
                                 lang_id = l.lang_id,
                                 code = l.code,
                                 name = l.name,
                                 display_order = l.display_order,
                                 status = l.status,
                                 crt_date = l.crt_date,
                                 crt_by_type = l.crt_by_type,
                                 crt_by = l.crt_by,
                                 upd_date = l.upd_date,
                                 upd_by_type = l.upd_by_type,
                                 upd_by = l.upd_by,
                                 record_status = l.record_status
                             });

                // dynamic search
                foreach (var f in searchParmList)
                {
                    if (f.property == "name")
                    {
                        query = query.Where(x => x.name.Contains(f.value));
                    }
                }

                // dynamic sort
                var orderByColumn = "";
                if (sortColumn == "name")
                    orderByColumn = "name";
                else if (sortColumn == "lang_id")
                    orderByColumn = "lang_id";
                else if (sortColumn == "code")
                    orderByColumn = "code";
                else
                { //default
                    sortOrder = CommonConstant.SortOrder.asc;
                    orderByColumn = "lang_id";
                }

                // row total
                totalRow = query.Count();

                if (sortOrder == CommonConstant.SortOrder.desc)
                    resultList = query.OrderBy(orderByColumn, true).Skip(rowIndexStart).Take(rowLimit).ToList();
                else
                    resultList = query.OrderBy(orderByColumn, false).Skip(rowIndexStart).Take(rowLimit).ToList();

                systemCode = CommonConstant.SystemCode.normal;
            }
            else
            {
                systemCode = CommonConstant.SystemCode.no_permission;
            }

            return resultList;
        }

        public List<LanguageObject> GetListAll(bool rootAccess, ref CommonConstant.SystemCode systemCode)
        {
            systemCode = CommonConstant.SystemCode.undefine;
            var resultList = new List<LanguageObject>();

            if (_privilege.read_status == 1 || rootAccess)
            {
                var query = (from l in db.languages

                             where (
                                l.record_status != (int)CommonConstant.RecordStatus.deleted

                                )
                             select new LanguageObject
                             {
                                 lang_id = l.lang_id,
                                 code = l.code,
                                 name = l.name,
                                 display_order = l.display_order,
                                 status = l.status,
                                 crt_date = l.crt_date,
                                 crt_by_type = l.crt_by_type,
                                 crt_by = l.crt_by,
                                 upd_date = l.upd_date,
                                 upd_by_type = l.upd_by_type,
                                 upd_by = l.upd_by,
                                 record_status = l.record_status
                             });

                resultList = query.OrderBy(x=> x.lang_id).ToList();

                systemCode = CommonConstant.SystemCode.normal;
            }
            else
            {
                systemCode = CommonConstant.SystemCode.no_permission;
            }

            return resultList;
        }


        //public IEnumerable<sp_GetLanguageListResult> GetLanguageList()
        //{
        //    AccessManager accessManager = new AccessManager();
        //    var sql_remark = "";
        //    var access_result = accessManager.AccessModule("language", CommonConstant.ActionType.update, ref sql_remark);
        //    IEnumerable<sp_GetLanguageListResult> result = null;

        //    if (access_result)
        //    {
        //        int? get_sql_result = 0;

        //        try
        //        {
        //            result = db.sp_GetLanguageList(SessionManager.Current.user_id, ref get_sql_result, ref sql_remark);
        //        }
        //        catch (Exception ex)
        //        {
        //            result = new List<sp_GetLanguageListResult>();
        //        }
        //    }

        //    return result;
        //}
    }
}