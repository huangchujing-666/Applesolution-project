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
using Palmary.Loyalty.BO.DataTransferObjects.Form;
using Palmary.Loyalty.BO.DataTransferObjects.Member;
using Palmary.Loyalty.BO.DataTransferObjects.Role;

using Palmary.Loyalty.BO.Modules.Utility;

namespace Palmary.Loyalty.BO.Modules.Member
{
    public class MemberColumnManager
    {
        private static LoyaltyDBDataContext db = new LoyaltyDBDataContext();

        public List<MemberColumnObject> GetList(int rowIndexStart, int rowLimit, List<SearchParmObject> searchParmList, string sortColumn, CommonConstant.SortOrder sortOrder, ref CommonConstant.SystemCode systemCode, ref int totalRow)
        {
            var resultList = new List<MemberColumnObject>();

            //if (_privilege.read_status == 1)
            //{
                var query = (from mc in db.member_columns

                             join li_dt in db.listing_items on mc.datatype equals li_dt.value
                             join l_dt in db.listings on li_dt.list_id equals l_dt.list_id

                             join li_udd in db.listing_items on mc.udd_column equals li_udd.value
                             join l_udd in db.listings on li_udd.list_id equals l_udd.list_id

                             where (
                                mc.record_status != (int)CommonConstant.RecordStatus.deleted
                                && l_dt.code == "ColumnDataType"
                                && l_udd.code == "YesNo"

                             )
                             select new MemberColumnObject
                             {
                                 column_id = mc.column_id,
                                 udd_column = mc.udd_column,
                                 udd_column_id = mc.udd_column_id,
                                 datatype = mc.datatype,
                                 datalength = mc.datalength,
                                 display_name = mc.display_name,
                                 remark = mc.remark,
                                 crt_date = mc.crt_date,
                                 crt_by_type = mc.crt_by_type,
                                 crt_by = mc.crt_by,
                                 upd_date = mc.upd_date,
                                 upd_by_type = mc.upd_by_type,
                                 upd_by = mc.upd_by,
                                 record_status = mc.record_status,

                                 // additional
                                 udd_column_name = li_udd.name,
                                 datatype_name = li_dt.name
                             });

                // dynamic search
                foreach (var f in searchParmList)
                {
                    //if (f.property == "member_no")
                    //{
                    //    query = query.Where(x => x.member_no.Contains(f.value));
                    //}
                    //else if (f.property == "status")
                    //{
                    //    if (!String.IsNullOrEmpty(f.value))
                    //        query = query.Where(x => x.status == int.Parse(f.value));
                    //}
                }

                // dynamic sort
                Func<MemberColumnObject, Object> orderByFunc = null;
                if (sortColumn == "display_name")
                    orderByFunc = x => x.display_name;
                else
                {
                    sortOrder = CommonConstant.SortOrder.asc;
                    orderByFunc = x => x.display_name;
                }

                // row total
                totalRow = query.Count();

                if (sortOrder == CommonConstant.SortOrder.desc)
                    resultList = query.OrderByDescending(orderByFunc).Skip(rowIndexStart).Take(rowLimit).ToList();
                else
                    resultList = query.OrderBy(orderByFunc).Skip(rowIndexStart).Take(rowLimit).ToList();

                systemCode = CommonConstant.SystemCode.normal;
            //}
            //else
            //{
            //    resultList = new List<MemberCardObject>();
            //    systemCode = CommonConstant.SystemCode.no_permission;
            //}

            return resultList;
        }
    }
}
