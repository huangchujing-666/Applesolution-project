using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Palmary.Loyalty.Common;
using Palmary.Loyalty.BO.Database;

using Palmary.Loyalty.BO.DataTransferObjects.DB;
using Palmary.Loyalty.BO.DataTransferObjects.Security;
using Palmary.Loyalty.BO.DataTransferObjects.Role;
using Palmary.Loyalty.BO.DataTransferObjects.Service;
using Palmary.Loyalty.BO.Modules.Administration.Security;


namespace Palmary.Loyalty.BO.Modules.Service
{
    public class ServiceCategoryManager
    {
        private static LoyaltyDBDataContext db = new LoyaltyDBDataContext();
        private AccessManager _accessManager;
        private string _module = CommonConstant.Module.serviceCategory;
        private RolePrivilegeObject _privilege;
        private static LogManager _logManager;
        private AccessObject _accessObject;

        public ServiceCategoryManager()
        {
            // access object
            _accessManager = new AccessManager();
            _accessObject = _accessManager.GetAccessObjectBySession();

            // privilege and log
            _privilege = _accessManager.AccessModule(_module);
            _logManager = new LogManager();
        }

        public CommonConstant.SystemCode Create(
            ServiceCategoryObject dataObject
        )
        {
            int? sql_result = 0;
            var system_code = CommonConstant.SystemCode.undefine;

            if (_privilege.insert_status == 1)
            {
                var result = db.sp_CreateServiceCategory(
                                 _accessObject.id,
                _accessObject.type, 
                    dataObject.parent_id,
                    dataObject.leaf,
                    dataObject.display_order,
                    dataObject.name,
                    dataObject.description,
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

        public CommonConstant.SystemCode Update(ServiceCategoryObject dataObject)
        {
            int? sql_result = 0;
            var system_code = CommonConstant.SystemCode.undefine;

            if (_privilege.update_status == 1)
            {
                var result = db.sp_UpdateServiceCategory(
                 _accessObject.id,
                 _accessObject.type,

                    dataObject.category_id,
                    dataObject.parent_id,
                    dataObject.leaf,
                    dataObject.display_order,
                    dataObject.name,
                    dataObject.description,
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

        // Get whole list (limited data)
        public List<ServiceCategoryObject> GetList(ref CommonConstant.SystemCode systemCode)
        {
            List<ServiceCategoryObject> resultList;

            if (_privilege.read_status == 1)
            {
                var query = (from sc in db.service_categories
                             
                             where (
                                sc.record_status != (int)CommonConstant.RecordStatus.deleted
                            )
                             select new ServiceCategoryObject
                             {
                                 category_id = sc.category_id,
                                 parent_id = sc.parent_id,
                                 leaf = sc.leaf,
                                 display_order = sc.display_order,
                                 name = sc.name,
                             });

                resultList = query.OrderBy(x => x.display_order).ToList();

                systemCode = CommonConstant.SystemCode.normal;
            }
            else
            {
                resultList = new List<ServiceCategoryObject>();
                systemCode = CommonConstant.SystemCode.no_permission;
            }

            return resultList;
        }

        // Get detail object
        public ServiceCategoryObject GetDetail(int category_id, ref CommonConstant.SystemCode systemCode)
        {
            ServiceCategoryObject resultObj;

            if (_privilege.read_status == 1)
            {
                var query = (from sc in db.service_categories
                             join li in db.listing_items on sc.status equals li.value
                             join l in db.listings on li.list_id equals l.list_id

                             where (
                                sc.record_status != (int)CommonConstant.RecordStatus.deleted
                                && sc.category_id == category_id

                            )
                             select new ServiceCategoryObject
                             {
                                 category_id = sc.category_id,
                                 parent_id = sc.parent_id,
                                 leaf = sc.leaf,
                                 display_order = sc.display_order,
                                 name = sc.name,
                                 description = sc.description,
                                 status = sc.status,
                                 crt_date = sc.crt_date,
                                 crt_by_type = sc.crt_by_type,
                                 crt_by = sc.crt_by,
                                 upd_date = sc.upd_date,
                                 upd_by_type = sc.upd_by_type,
                                 upd_by = sc.upd_by,
                                 record_status = sc.record_status,

                                 // additional
                                 status_name = li.name
                             });

                resultObj = query.FirstOrDefault();

                systemCode = CommonConstant.SystemCode.normal;
            }
            else
            {
                resultObj = new ServiceCategoryObject();
                systemCode = CommonConstant.SystemCode.no_permission;
            }
            return resultObj;
        }
    }
}
