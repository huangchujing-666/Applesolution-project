using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Palmary.Loyalty.BO.Database;
using Palmary.Loyalty.BO.DataTransferObjects.DB;
using Palmary.Loyalty.BO.DataTransferObjects.Member;
using Palmary.Loyalty.BO.DataTransferObjects.Role;
using Palmary.Loyalty.BO.Modules.Administration.Security;
using Palmary.Loyalty.Common;
using Palmary.Loyalty.BO.Modules.Utility;
using Palmary.Loyalty.BO.DataTransferObjects.CSVExport;

namespace Palmary.Loyalty.BO.Modules.Member
{
    public class MemberLevelChangeManager
    {
        private static LoyaltyDBDataContext db = new LoyaltyDBDataContext();
        private AccessManager _accessManager;
        private string _module = CommonConstant.Module.memberLevelChange;
        private RolePrivilegeObject _privilege;

        public MemberLevelChangeManager()
        {
            _accessManager = new AccessManager();
            _privilege = _accessManager.AccessModule(_module);
        }

        public List<MemberLevelChangeObject> GetList(int rowIndexStart, int rowLimit, List<SearchParmObject> searchParmList, string sortColumn, CommonConstant.SortOrder sortOrder, ref CommonConstant.SystemCode systemCode, ref int totalRow)
        {
            // LINQ to SQL
            systemCode = CommonConstant.SystemCode.undefine;
            var resultList = new List<MemberLevelChangeObject>();

            if (_privilege.read_status == 1)
            {
                var query = (from mlc in db.member_level_changes
                             join m in db.member_profiles on mlc.member_id equals m.member_id
                             join li_st in db.listing_items on mlc.source_type equals li_st.value
                             join li_ct in db.listing_items on mlc.change_type equals li_ct.value
                             join li_rt in db.listing_items on mlc.reason_type equals li_rt.value

                             join ml_ol  in db.member_levels on mlc.old_member_level_id equals ml_ol.level_id
                             join ml_nl in db.member_levels on mlc.new_member_level_id equals ml_nl.level_id

                             where (
                                mlc.record_status != (int)CommonConstant.RecordStatus.deleted
                                && li_st.list_id == (int)CommonConstant.ListingType.MemberLevelChangeSourceType
                                && li_ct.list_id == (int)CommonConstant.ListingType.MemberLevelChangeChangeType
                                && li_rt.list_id == (int)CommonConstant.ListingType.MemberLevelChangeReasonType
                             )
                             select new MemberLevelChangeObject
                             {
                                 change_id = mlc.change_id,
                                 member_id = mlc.member_id,
                                 source_type = mlc.source_type,
                                 change_type = mlc.change_type,
                                 reason_type = mlc.reason_type,
                                 old_member_level_id = mlc.old_member_level_id,
                                 new_member_level_id = mlc.new_member_level_id,
                                 remark = mlc.remark,
                                 crt_date = mlc.crt_date,
                                 crt_by_type = mlc.crt_by_type,
                                 crt_by = mlc.crt_by,
                                 upd_date = mlc.upd_date,
                                 upd_by_type = mlc.upd_by_type,
                                 upd_by = mlc.upd_by,
                                 record_status = mlc.record_status,

                                 // additional
                                 member_no = m.member_no,
                                 source_type_name = li_st.name,
                                 change_type_name = li_ct.name,
                                 reason_type_name = li_rt.name,

                                 old_member_level_name = ml_ol.name,
                                 new_member_level_name = ml_nl.name
                             });

                // dynamic search
                //foreach (var f in searchParmList)
                //{
                //    if (!String.IsNullOrEmpty(f.value))
                //    {
                //        if (f.property == "change_type_name")
                //        {
                //            query = query.Where(x => x == x.change_type_name.ToLower());
                //        }
                //    }
                //}

                // dynamic sort
                var orderByColumn = "";
                if (string.IsNullOrEmpty(sortColumn))
                {
                    sortOrder = CommonConstant.SortOrder.desc;
                    orderByColumn = "crt_date";
                }
                //else if (sortColumn == "name")
                //    orderByColumn = "name";
                //else if (sortColumn == "status_name")
                //    orderByColumn = "status_name";
                else
                    orderByColumn = sortColumn;

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

        public List<MemberLevelChangeExportObject> GetListExport(bool rootAccess, ref CommonConstant.SystemCode systemCode)
        {
            // LINQ to SQL
            systemCode = CommonConstant.SystemCode.undefine;
            var resultList = new List<MemberLevelChangeExportObject>();

            if (_privilege.read_status == 1 || rootAccess)
            {
                var query = (from mlc in db.member_level_changes
                             join m in db.member_profiles on mlc.member_id equals m.member_id
                             join li_st in db.listing_items on mlc.source_type equals li_st.value
                             join li_ct in db.listing_items on mlc.change_type equals li_ct.value
                             join li_rt in db.listing_items on mlc.reason_type equals li_rt.value

                             join ml_ol in db.member_levels on mlc.old_member_level_id equals ml_ol.level_id
                             join ml_nl in db.member_levels on mlc.new_member_level_id equals ml_nl.level_id

                             where (
                                mlc.record_status != (int)CommonConstant.RecordStatus.deleted
                                && li_st.list_id == (int)CommonConstant.ListingType.MemberLevelChangeSourceType
                                && li_ct.list_id == (int)CommonConstant.ListingType.MemberLevelChangeChangeType
                                && li_rt.list_id == (int)CommonConstant.ListingType.MemberLevelChangeReasonType
                             )
                             select new MemberLevelChangeObject
                             {
                                 change_id = mlc.change_id,
                                 member_id = mlc.member_id,
                                 source_type = mlc.source_type,
                                 change_type = mlc.change_type,
                                 reason_type = mlc.reason_type,
                                 old_member_level_id = mlc.old_member_level_id,
                                 new_member_level_id = mlc.new_member_level_id,
                                 remark = mlc.remark,
                                 crt_date = mlc.crt_date,
                               
                                 // additional
                                 member_no = m.member_no,
                                 source_type_name = li_st.name,
                                 change_type_name = li_ct.name,
                                 reason_type_name = li_rt.name,

                                 old_member_level_name = ml_ol.name,
                                 new_member_level_name = ml_nl.name
                             });

                var objList = query.OrderByDescending(x => x.crt_date).ToList();
                foreach (var x in objList)
                {
                    resultList.Add(new MemberLevelChangeExportObject() {
                    
                        remark = x.remark,
                        crt_date = x.crt_date.ToString("yyyy-MM-dd HH:mm:ss"),

                        // additional
                        member_no = x.member_no,
                        source_type_name = x.source_type_name,
                        change_type_name = x.change_type_name,
                        reason_type_name = x.reason_type_name,

                        old_member_level_name = x.old_member_level_name,
                        new_member_level_name = x.new_member_level_name
                    }
                    );
                }

                systemCode = CommonConstant.SystemCode.normal;
            }
            else
            {
                systemCode = CommonConstant.SystemCode.no_permission;
            }

            return resultList;
        }
    }
}
