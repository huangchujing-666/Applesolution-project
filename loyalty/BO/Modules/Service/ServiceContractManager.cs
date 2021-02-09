using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.Objects;
using Palmary.Loyalty.Common;
using Palmary.Loyalty.BO.Database;
using Palmary.Loyalty.BO.Modules.Service;
using Palmary.Loyalty.BO.DataTransferObjects.Service;

using Palmary.Loyalty.BO.Modules.Administration.Security;
using Palmary.Loyalty.BO.Modules.Administration.SystemControl;
using Palmary.Loyalty.BO.Modules.Utility;
using Palmary.Loyalty.BO.DataTransferObjects.Role;
using Palmary.Loyalty.BO.DataTransferObjects.DB;
using Palmary.Loyalty.BO.DataTransferObjects.Security;
using Palmary.Loyalty.BO.DataTransferObjects.Form;
using Palmary.Loyalty.BO.DataTransferObjects.Utility;

namespace Palmary.Loyalty.BO.Modules.Service
{
    public class ServiceContractManager
    {
        private static LoyaltyDBDataContext db = new LoyaltyDBDataContext();
        private AccessManager _accessManager;
        private string _module = CommonConstant.Module.serviceContract;
        private RolePrivilegeObject _privilege;

        public ServiceContractManager()
        {
            _accessManager = new AccessManager();
            _privilege = _accessManager.AccessModule(_module);
        }

        // Get List with paging, dynamic search, dynamic sorting
        public List<ServiceContractObject> GetList(int member_id, int rowIndexStart, int rowLimit, List<SearchParmObject> searchParmList, string sortColumn, CommonConstant.SortOrder sortOrder, ref CommonConstant.SystemCode systemCode, ref int totalRow)
        {
            List<ServiceContractObject> resultList;

            if (_privilege.read_status == 1)
            {
                //var query = (from sc in db.service_contracts
                //             join s in db.services on sc.service_id equals s.service_id
                //             where (
                //                s.record_status != (int)CommonConstant.RecordStatus.deleted
                //                && sc.member_id == member_id
                //            )
                //             select new ServiceContractObject
                //             {
                //                 contract_id = sc.contract_id,
                //                 contract_no = sc.contract_no,
                //                 member_id = sc.member_id,
                //                 service_id = sc.service_id,
                //                 fee = sc.fee,
                //                 start_date = sc.start_date,
                //                 end_date = sc.end_date,
                //                 status = sc.status,
                //                 crt_date = sc.crt_date,
                //                 crt_by_type = sc.crt_by_type,
                //                 crt_by = sc.crt_by,
                //                 upd_date = sc.upd_date,
                //                 upd_by_type = sc.upd_by_type,
                //                 upd_by = sc.upd_by,
                //                 record_status = sc.record_status,

                //                 // additional info
                //                 service_name = s.name,
                //                 service_no = s.service_no,
                //             });

                //// dynamic sort
                //Func<ServiceContractObject, Object> orderByFunc = null;
                //if (sortColumn == "service_no")
                //    orderByFunc = x => x.service_no;
                //if (sortColumn == "contract_no")
                //    orderByFunc = x => x.contract_no;
                //if (sortColumn == "service_name")
                //    orderByFunc = x => x.service_name;
                //if (sortColumn == "fee")
                //    orderByFunc = x => x.fee;
                //if (sortColumn == "start_date")
                //    orderByFunc = x => x.start_date;
                //if (sortColumn == "end_date")
                //    orderByFunc = x => x.end_date;
                //else
                //{
                //    sortOrder = CommonConstant.SortOrder.desc;
                //    orderByFunc = x => x.crt_date;
                //}

                //// row total
                //totalRow = query.Count();

                //if (sortOrder == CommonConstant.SortOrder.desc)
                //    resultList = query.OrderByDescending(orderByFunc).Skip(rowIndexStart).Take(rowLimit).ToList();
                //else
                //    resultList = query.OrderBy(orderByFunc).Skip(rowIndexStart).Take(rowLimit).ToList();
                resultList = new List<ServiceContractObject>();
                systemCode = CommonConstant.SystemCode.normal;
            }
            else
            {
                resultList = new List<ServiceContractObject>();
                systemCode = CommonConstant.SystemCode.no_permission;
            }
            return resultList;
        }

         // Get Deatil
        public ServiceContractObject GetDetail_byServiceNo(int member_id, string service_no, ref CommonConstant.SystemCode systemCode)
        {
            ServiceContractObject resultObj;
            if (_privilege.read_status == 1)
            {
                //var query = (from sc in db.service_contracts
                //             join s in db.services on sc.service_id equals s.service_id
                //             where (
                //             s.record_status != (int)CommonConstant.RecordStatus.deleted
                //             && sc.member_id == member_id
                //             && s.service_no == service_no
                //         )
                //             select new ServiceContractObject
                //             {
                //                 contract_id = sc.contract_id,
                //                 contract_no = sc.contract_no,
                //                 member_id = sc.member_id,
                //                 service_id = sc.service_id,
                //                 fee = sc.fee,
                //                 start_date = sc.start_date,
                //                 end_date = sc.end_date,
                //                 status = sc.status,
                //                 crt_date = sc.crt_date,
                //                 crt_by_type = sc.crt_by_type,
                //                 crt_by = sc.crt_by,
                //                 upd_date = sc.upd_date,
                //                 upd_by_type = sc.upd_by_type,
                //                 upd_by = sc.upd_by,
                //                 record_status = sc.record_status,

                //                 // additional info
                //                 service_name = s.name,
                //                 service_no = s.service_no,
                //             });
                //resultObj = query.FirstOrDefault();
                resultObj = new ServiceContractObject();

                systemCode = CommonConstant.SystemCode.normal;
            }
            else
            {
                resultObj = new ServiceContractObject();
                systemCode = CommonConstant.SystemCode.no_permission;
            }
            return resultObj;
        }
    }
}