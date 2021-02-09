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
    public class ServicePlanManager
    {
        private static LoyaltyDBDataContext db = new LoyaltyDBDataContext();
        private AccessManager _accessManager;
        private string _module = CommonConstant.Module.servicePlan;
        private RolePrivilegeObject _privilege;
        private static LogManager _logManager;
        private AccessObject _accessObject;

        public ServicePlanManager()
        {
            // access object
            _accessManager = new AccessManager();
            _accessObject = _accessManager.GetAccessObjectBySession();

            // privilege and log
            _privilege = _accessManager.AccessModule(_module);
            _logManager = new LogManager();
        }

        public CommonConstant.SystemCode Create(
            ServicePlanObject dataObject
        )
        {
            int? sql_result = 0;
            var system_code = CommonConstant.SystemCode.undefine;

            if (_privilege.insert_status == 1)
            {
                var result = db.sp_CreateServicePlan(
                   _accessObject.id, 
                   _accessObject.type, 

                    dataObject.plan_no,
                    dataObject.type,
                    dataObject.name,
                    dataObject.description,
                    dataObject.fee,
                    dataObject.point_expiry_month,
                    dataObject.ratio_payment,
                    dataObject.ratio_point,
                    dataObject.status,
                    
                    ref sql_result
                );
                system_code = (CommonConstant.SystemCode)sql_result.Value;
            }
            else
            {
                system_code = CommonConstant.SystemCode.no_permission;
            }
            return system_code;
        }

        public CommonConstant.SystemCode Update(ServicePlanObject dataObject)
        {
            int? sql_result = 0;
            var system_code = CommonConstant.SystemCode.undefine;

            if (_privilege.update_status == 1)
            {
                var result = db.sp_UpdateServicePlan(
                                    _accessObject.id,
                  _accessObject.type, 

                    dataObject.plan_id,
                    dataObject.plan_no,
                    dataObject.type,
                    dataObject.name,
                    dataObject.description,
                    dataObject.fee,
                    dataObject.point_expiry_month,
                    dataObject.ratio_payment,
                    dataObject.ratio_point,
                    dataObject.status,
                    ref sql_result);

                system_code = (CommonConstant.SystemCode)sql_result.Value;
            }
            else
            {
                system_code = CommonConstant.SystemCode.no_permission;
            }

            return system_code;
        }

        // Get List with paging, dynamic search, dynamic sorting
        public List<ServicePlanObject> GetList(int rowIndexStart, int rowLimit, List<SearchParmObject> searchParmList, string sortColumn, CommonConstant.SortOrder sortOrder, ref CommonConstant.SystemCode systemCode, ref int totalRow)
        {
            List<ServicePlanObject> resultList;

            if (_privilege.read_status == 1)
            {
                var query = (from sp in db.service_plans
                             join sc in db.service_categories on sp.type equals sc.category_id
                             join li in db.listing_items on sp.status equals li.value
                             join l in db.listings on li.list_id equals l.list_id

                             where (
                                sp.record_status != (int)CommonConstant.RecordStatus.deleted
                                && l.code == "Status"
                            )
                             select new ServicePlanObject
                             {
                                 plan_id = sp.plan_id,
                                 plan_no = sp.plan_no,
                                 type = sp.type,
                                 name = sp.name,
                                 description = sp.description,
                                 fee = sp.fee,
                                 point_expiry_month = sp.point_expiry_month,
                                 status = sp.status,
                                 crt_date = sp.crt_date,
                                 crt_by_type = sp.crt_by_type,
                                 crt_by = sp.crt_by,
                                 upd_date = sp.upd_date,
                                 upd_by_type = sp.upd_by_type,
                                 upd_by = sp.upd_by,
                                 record_status = sp.record_status,

                                 //-- additional info
                                 status_name = li.name,
                                 type_name = sc.name
                             });

                // dynamic search
                foreach (var f in searchParmList)
                {
                    if (f.property == "plan_no")
                    {
                        query = query.Where(x => x.plan_no.Contains(f.value));
                    }
                    else if (f.property == "name")
                    {
                        query = query.Where(x => x.name.ToLower().Contains(f.value.ToLower()));
                    }
                }

                // dynamic sort
                var orderByColumn = "";
                if (sortColumn == "plan_no"
                    || sortColumn == "name"
                    || sortColumn == "status_name"
                    || sortColumn == "type_name"
                    )
                    orderByColumn = sortColumn;
                else
                { //default
                    sortOrder = CommonConstant.SortOrder.asc;
                    orderByColumn = "plan_no";
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
                resultList = new List<ServicePlanObject>();
                systemCode = CommonConstant.SystemCode.no_permission;
            }

            return resultList;
        }

        // Get Whole List  (limited data)
        public List<ServicePlanObject> GetList(ref CommonConstant.SystemCode system_code)
        {
            List<ServicePlanObject> resultList;
            
            if (_privilege.read_status == 1)
            {
                var query = (from s in db.service_plans
                             select new ServicePlanObject
                             {
                                 plan_id = s.plan_id,
                                 plan_no = s.plan_no,
                                 type = s.type,
                                 name = s.name,
                                 description = s.description,
                                 fee = s.fee,
                                 point_expiry_month = s.point_expiry_month,
                                 status = s.status,
                                 crt_date = s.crt_date,
                                 crt_by_type = s.crt_by_type,
                                 crt_by = s.crt_by,
                                 upd_date = s.upd_date,
                                 upd_by_type = s.upd_by_type,
                                 upd_by = s.upd_by,
                                 record_status = s.record_status,
                             });

                resultList = query.OrderBy(x=> x.plan_no).ToList();

                system_code = CommonConstant.SystemCode.normal;
            }
            else
            {
                resultList = new List<ServicePlanObject>();
                system_code = CommonConstant.SystemCode.no_permission;
            }

            return resultList;
        }

        // Get Detail
        public ServicePlanObject GetDetail(int plan_id, ref CommonConstant.SystemCode systemCode)
        {
            ServicePlanObject resultObj;

            if (_privilege.read_status == 1)
            {
                var query = (from sp in db.service_plans
                             join sc in db.service_categories on sp.type equals sc.category_id
                             join li in db.listing_items on sp.status equals li.value
                             join l in db.listings on li.list_id equals l.list_id

                             where (
                                sp.record_status != (int)CommonConstant.RecordStatus.deleted
                                && l.code == "Status"
                                && sp.plan_id == plan_id
                            )
                             select new ServicePlanObject
                             {
                                 plan_id = sp.plan_id,
                                 plan_no = sp.plan_no,
                                 type = sp.type,
                                 name = sp.name,
                                 description = sp.description,
                                 fee = sp.fee,
                                 point_expiry_month = sp.point_expiry_month,
                                 ratio_payment = sp.ratio_payment,
                                 ratio_point = sp.ratio_point,
                                 status = sp.status,
                                 crt_date = sp.crt_date,
                                 crt_by_type = sp.crt_by_type,
                                 crt_by = sp.crt_by,
                                 upd_date = sp.upd_date,
                                 upd_by_type = sp.upd_by_type,
                                 upd_by = sp.upd_by,
                                 record_status = sp.record_status,

                                 //-- additional info
                                 status_name = li.name,
                                 type_name = sc.name
                             });

                resultObj = query.FirstOrDefault();

                systemCode = CommonConstant.SystemCode.normal;
            }
            else
            {
                resultObj = new ServicePlanObject();
                systemCode = CommonConstant.SystemCode.no_permission;
            }

            return resultObj;
        }

        // Get Detail
        public ServicePlanObject GetDetail_byServicePlanNo(string plan_no, ref CommonConstant.SystemCode systemCode)
        {
            ServicePlanObject resultObj;

            if (_privilege.read_status == 1)
            {
                var query = (from sp in db.service_plans
                             join sc in db.service_categories on sp.type equals sc.category_id
                             
                             where (
                                sp.record_status != (int)CommonConstant.RecordStatus.deleted
                                && sp.plan_no == plan_no
                            )
                             select new ServicePlanObject
                             {
                                 plan_id = sp.plan_id,
                                 plan_no = sp.plan_no,
                                 type = sp.type,
                                 name = sp.name,
                                 description = sp.description,
                                 fee = sp.fee,
                                 point_expiry_month = sp.point_expiry_month,
                                 ratio_payment = sp.ratio_payment,
                                 ratio_point = sp.ratio_point,
                                 status = sp.status,
                                 crt_date = sp.crt_date,
                                 crt_by_type = sp.crt_by_type,
                                 crt_by = sp.crt_by,
                                 upd_date = sp.upd_date,
                                 upd_by_type = sp.upd_by_type,
                                 upd_by = sp.upd_by,
                                 record_status = sp.record_status,

                                 //-- additional info
                                 type_name = sc.name
                             });

                resultObj = query.FirstOrDefault();
                if (resultObj != null)
                    systemCode = CommonConstant.SystemCode.normal;
                else
                    systemCode = CommonConstant.SystemCode.record_invalid;
            }
            else
            {
                resultObj = new ServicePlanObject();
                systemCode = CommonConstant.SystemCode.no_permission;
            }

            return resultObj;
        }
    }
}
