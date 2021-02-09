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
using Palmary.Loyalty.BO.DataTransferObjects.Role;
using Palmary.Loyalty.BO.DataTransferObjects.MemberAdvanceSearch;
using Palmary.Loyalty.BO.DataTransferObjects.Member;

using Palmary.Loyalty.BO.Modules.Utility;
using Palmary.Loyalty.BO.Modules.Member;

namespace Palmary.Loyalty.BO.Modules.MemberAdvanceSearch
{
    public class MemberAdvanceSearchListMemberManager
    {
        private static LoyaltyDBDataContext db = new LoyaltyDBDataContext();
        private AccessManager _accessManager;
        private string _module = CommonConstant.Module.memberAdvanceSearch;

        private RolePrivilegeObject _privilege;
        private static LogManager _logManager;
        private AccessObject _accessObject;

         // For backend, using BO Session to access
        public MemberAdvanceSearchListMemberManager()
        {
            // access object from session
            _accessManager = new AccessManager();
            _accessObject = _accessManager.GetAccessObjectBySession();

            // privilege and log
            _privilege = _accessManager.AccessModule(_module);
            _logManager = new LogManager();
        }

        // For Webservices or other not using BO Session to access
        public MemberAdvanceSearchListMemberManager(AccessObject accessObject)
        {
            // access object from passing in
            _accessManager = new AccessManager();
            _accessObject = accessObject;

            // privilege and log
            _privilege = _accessManager.AccessModule(_module, _accessObject.type, _accessObject.id);
            _logManager = new LogManager(_accessObject);
        }

        public List<MemberObject> GetListBySearch(int search_id, int rowIndexStart, int rowLimit, List<SearchParmObject> searchParmList, string sortColumn, CommonConstant.SortOrder sortOrder, ref CommonConstant.SystemCode systemCode, ref int totalRow)
        {
            // LINQ to SQL
            systemCode = CommonConstant.SystemCode.undefine;
            var resultList = new List<MemberObject>();

            if (_privilege.read_status == 1)
            {
                int? result_int = 0;
                var resultlist = db.sp_GetMemberAdvanceSearchListMember(_accessObject.id, rowIndexStart, rowLimit ,search_id, ref result_int);

                foreach(sp_GetMemberAdvanceSearchListMemberResult obj in resultlist)
                {
                    resultList.Add(new MemberObject()
                        {
                            member_id = obj.member_id.Value,
                            member_no = obj.member_no,
                            email = obj.email,
                            firstname = obj.firstname,
                            middlename = obj.middlename,
                            lastname = obj.lastname
                        }
                    );
                }

                // row total
                totalRow = resultList.Count();

                resultList = resultList.Skip(rowIndexStart).Take(rowLimit).ToList();

                //var query = (from m in db.member_profiles
                             
                //             where (
                //                m.record_status != (int)CommonConstant.RecordStatus.deleted
                //                )
                //             select new MemberObject
                //             {
                //                 member_id = m.member_id,
                //                 member_no = m.member_no,
                //                 fullname = m.firstname
                //             });

                //// dynamic search
                ////foreach (var f in searchParmList)
                ////{
                ////    if (!String.IsNullOrEmpty(f.value))
                ////    {
                ////        if (f.property == "name")
                ////        {
                ////            query = query.Where(x => x.name.ToLower().Contains(f.value.ToLower()));
                ////        }
                        
                ////    }
                ////}

                //// dynamic sort
                //var orderByColumn = "";
                //if (sortColumn == "member_no")
                //    orderByColumn = sortColumn;
                //else
                //{ //default
                //    sortOrder = CommonConstant.SortOrder.asc;
                //    orderByColumn = "member_no";
                //}

                //// row total
                //totalRow = query.Count();

                //if (sortOrder == CommonConstant.SortOrder.desc)
                //    resultList = query.OrderBy(orderByColumn, true).Skip(rowIndexStart).Take(rowLimit).ToList();
                //else
                //    resultList = query.OrderBy(orderByColumn, false).Skip(rowIndexStart).Take(rowLimit).ToList();

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
