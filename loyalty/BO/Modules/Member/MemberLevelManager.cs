using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.Objects;

using Palmary.Loyalty.BO.Database;
using Palmary.Loyalty.BO.DataTransferObjects.Security;
using Palmary.Loyalty.BO.Modules.Administration.Security;
using Palmary.Loyalty.BO.Modules.Administration.SystemControl;
using Palmary.Loyalty.BO.DataTransferObjects.Form;
using Palmary.Loyalty.Common;
using Palmary.Loyalty.BO.DataTransferObjects.Role;
using Palmary.Loyalty.BO.DataTransferObjects.Member;
using Palmary.Loyalty.BO.DataTransferObjects.DB;
using Palmary.Loyalty.BO.Modules.Utility;

namespace Palmary.Loyalty.BO.Modules.Member
{
    public class MemberLevelManager
    {
        private static LoyaltyDBDataContext db = new LoyaltyDBDataContext();
        private AccessManager _accessManager;
        private string _module = CommonConstant.Module.memberLevel;
        private RolePrivilegeObject _privilege;
        private static LogManager _logManager;
        private AccessObject _accessObject;

        // For backend, using BO Session to access
        public MemberLevelManager()
        {
            // access object
            _accessManager = new AccessManager();
            _accessObject = _accessManager.GetAccessObjectBySession();

            // privilege and log
            _privilege = _accessManager.AccessModule(_module);
            _logManager = new LogManager();
        }

        // For Webservices or other not using BO Session to access
        public MemberLevelManager(AccessObject accessObject)
        {
            // access object
            _accessManager = new AccessManager();
            _accessObject = accessObject;

            // privilege and log
            _privilege = _accessManager.AccessModule(_module, _accessObject.type, _accessObject.id);
            _logManager = new LogManager(_accessObject);
        }

        //public IEnumerable<sp_GetMemberLevelListsResult> GetMemberLevelList()
        //{
        //    int? get_sql_result = 0;
        //    var sql_remark = "";

        //    var result = db.sp_GetMemberLevelLists(SessionManager.Current.user_id, ref get_sql_result, ref sql_remark);

        //    return result;
        //}

        public List<MemberLevelObject> GetList(int rowIndexStart, int rowLimit, List<SearchParmObject> searchParmList, string sortColumn, CommonConstant.SortOrder sortOrder, ref CommonConstant.SystemCode systemCode, ref int totalRow)
        {
            systemCode = CommonConstant.SystemCode.undefine;
            List<MemberLevelObject> resultList;

            if (_privilege.read_status == 1)
            {
                var query = (from ml in db.member_levels
                             join ml_l in db.member_level_langs on ml.level_id equals ml_l.level_id
                             join li_s in db.listing_items on ml.status equals li_s.value

                             where (
                                ml_l.lang_id == (int)CommonConstant.LangCode.en
                                && li_s.list_id == (int)CommonConstant.ListingType.Status
                            )
                             select new MemberLevelObject
                             {
                                 level_id = ml.level_id,
                                 name = ml_l.name,
                                 point_required = ml.point_required,
                                 redeem_discount = ml.redeem_discount,
                                 display_order = ml.display_order,
                                 status = ml.status,
                                 crt_date = ml.crt_date,
                                 upd_date = ml.upd_date,
                                 crt_by = ml.crt_by,
                                 upd_by = ml.upd_by,

                                 // additional info 
                                 status_name = li_s.name
                             });

                // dynamic search
                //foreach (var f in searchParmList)
                //{
                //}

                // dynamic sort
                var orderByColumn = "";
                if (sortColumn == "name"
                    || sortColumn == "point_required"
                    || sortColumn == "redeem_discount"
                    || sortColumn == "status_name")
                    orderByColumn = sortColumn;
                else
                { //default
                    sortOrder = CommonConstant.SortOrder.asc;
                    sortOrder = CommonConstant.SortOrder.desc;
                    orderByColumn = "point_required";
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
                resultList = new List<MemberLevelObject>();
                systemCode = CommonConstant.SystemCode.no_permission;
            }

            return resultList;
        }

        public List<MemberLevelObject> GetListAll(bool rootAccess, ref CommonConstant.SystemCode systemCode)
        {
            systemCode = CommonConstant.SystemCode.undefine;
            List<MemberLevelObject> resultList;

            if (_privilege.read_status == 1 || rootAccess)
            {
                var query = (from ml in db.member_levels
                             join ml_l in db.member_level_langs on ml.level_id equals ml_l.level_id
                             join li_s in db.listing_items on ml.status equals li_s.value

                             where (
                                ml_l.lang_id == (int)CommonConstant.LangCode.en
                                && li_s.list_id == (int)CommonConstant.ListingType.Status
                            )
                             select new MemberLevelObject
                             {
                                 level_id = ml.level_id,
                                 name = ml_l.name,
                                 point_required = ml.point_required,
                                 redeem_discount = ml.redeem_discount,
                                 display_order = ml.display_order,
                                 status = ml.status,
                                 crt_date = ml.crt_date,
                                 upd_date = ml.upd_date,
                                 crt_by = ml.crt_by,
                                 upd_by = ml.upd_by,

                                 // additional info 
                                 status_name = li_s.name
                             });
             
                resultList = query.OrderBy(x => x.point_required).ToList();
              
                systemCode = CommonConstant.SystemCode.normal;
            }
            else
            {
                resultList = new List<MemberLevelObject>();
                systemCode = CommonConstant.SystemCode.no_permission;
            }

            return resultList;
        }

        public int GetMemberLevelIDFromPoint(double point)
        { 
            var systemCode = CommonConstant.SystemCode.undefine;
            var levelList = GetListAll(true, ref systemCode);
            var targetMemberLevel = levelList.Where(x => x.point_required <= point).OrderByDescending(x => x.point_required).FirstOrDefault();
            return targetMemberLevel.level_id;
        }

        // public List<MemberLevelObject> Test(bool rootAccess, ref CommonConstant.SystemCode systemCode)
        //{
        //    systemCode = CommonConstant.SystemCode.undefine;
        //    List<MemberLevelObject> resultList;

          
        //        var query = (from ml_l in db.member_level_langs

        //                     where (
        //                        ml_l.lang_id == _accessObject.languageID
                            
        //                    )
        //                     select new MemberLevelObject
        //                     {
        //                         level_id = ml_l.level_id,
        //                         name = ml_l.name,
                                
        //                     });
             
        //        resultList = query.ToList();
              
        //        systemCode = CommonConstant.SystemCode.normal;
           
        //    return resultList;
        //}
    }
}
