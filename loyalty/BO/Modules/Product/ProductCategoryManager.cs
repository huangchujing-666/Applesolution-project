using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Palmary.Loyalty.BO.Database;
using Palmary.Loyalty.BO.DataTransferObjects.Security;
using Palmary.Loyalty.BO.DataTransferObjects.Product;
using System.Data.Objects;
using Palmary.Loyalty.BO.Modules.Administration.Security;
using Palmary.Loyalty.Common;
using Palmary.Loyalty.BO.DataTransferObjects.Role;

namespace Palmary.Loyalty.BO.Modules.Product
{
    public class ProductCategoryManager
    {
        private static LoyaltyDBDataContext db = new LoyaltyDBDataContext();
        private AccessManager _accessManager;
        private string _module = CommonConstant.Module.productCategory;
        private RolePrivilegeObject _privilege;
        private static LogManager _logManager;
        private AccessObject _accessObject;

        // For backend, using BO Session to access
        public ProductCategoryManager()
        {
            // access object
            _accessManager = new AccessManager();
            _accessObject = _accessManager.GetAccessObjectBySession();

            // privilege and log
            _privilege = _accessManager.AccessModule(_module);
            _logManager = new LogManager();
        }

        // For Webservices or other not using BO Session to access
        public ProductCategoryManager(AccessObject accessObject)
        {
            // access object
            _accessManager = new AccessManager();
            _accessObject = accessObject;

            // privilege and log
            _privilege = _accessManager.AccessModule(_module, _accessObject.type, _accessObject.id);
            _logManager = new LogManager(_accessObject);
        }

        public List<ProductCategoryObject> GetListAll(bool rootAccess, ref CommonConstant.SystemCode systemCode)
        {
            List<ProductCategoryObject> resultList;

            if (_privilege.read_status == 1 || rootAccess)
            {
                var query = (from pc in db.product_categories
                             join pcl in db.product_category_langs on pc.category_id equals pcl.category_id

                             where (
                                 pc.record_status != (int)CommonConstant.RecordStatus.deleted
                                 && pc.status == CommonConstant.Status.active
                                 && pcl.lang_id == _accessObject.languageID
                             )
                             select new ProductCategoryObject
                             {
                                 category_id = pc.category_id,
                                 parent_id = pc.parent_id,
                                 leaf = pc.leaf,
                                 photo_file_name = pc.photo_file_name,
                                 photo_file_extension = pc.photo_file_extension,
                                 display_order = pc.display_order,
                                 status = pc.status,
                                 crt_date = pc.crt_date,
                                 crt_by_type = pc.crt_by_type,
                                 crt_by = pc.crt_by,
                                 upd_date = pc.upd_date,
                                 upd_by_type = pc.upd_by_type,
                                 upd_by = pc.upd_by,
                                 record_status = pc.record_status,

                                 //-- additional info
                                 name = pcl.name
                             });

                resultList = query.ToList();

                systemCode = CommonConstant.SystemCode.normal;
            }
            else
            {
                resultList = new List<ProductCategoryObject>();
                systemCode = CommonConstant.SystemCode.no_permission;
            }

            return resultList;
        }

        public IEnumerable<sp_GetProductCategoryListsResult> GetProductCategoryLists()
        {
            int? get_sql_result = 0;
            var sql_remark = "";

            var result = db.sp_GetProductCategoryLists(SessionManager.Current.obj_id, ref get_sql_result, ref sql_remark);

            return result;
        }

        public sp_GetProductCategoryDetailByResult GetProductCategoryDetailBy( int category_id, ref bool sql_result)
        {
            int? get_sql_result = 0;
            var sql_remark = "";

            var result = db.sp_GetProductCategoryDetailBy(SessionManager.Current.obj_id, category_id, ref get_sql_result, ref sql_remark);

            return result.FirstOrDefault() ?? new sp_GetProductCategoryDetailByResult();
        }

        public bool Create(
            int parent_id,
            string photo_file_name,
            string photo_file_extension,
            int display_order,
            int status,
        
            ref int new_category_id,
            ref string sql_remark
        )
        {
            int? get_category_id = 0;
            int? get_sql_result = 0;
            

            var result = db.sp_CreateProductCategory(
                  _accessObject.id,
                _accessObject.type, 

                parent_id,
                1, //leaf , new add should be leaf
                photo_file_name,
                photo_file_extension,
                display_order,
                status,
           
                ref get_category_id,
                ref get_sql_result, ref sql_remark);

            var create_result = get_sql_result.Value == 1 ? true : false;
            new_category_id = get_category_id.Value;

            if (create_result) //Update parent cats leaf values
            {
                var sql_result = false;
                var parentCat = GetProductCategoryDetailBy(parent_id, ref sql_result);
                
                if (parentCat.leaf == 1)
                {
                    sql_result = Update(
                        
                        parentCat.parent_id,
                        0, //leaf,  parent's leaf value should be changed to 0
                        parentCat.category_id,
                        parentCat.photo_file_name,
                        parentCat.photo_file_extension,
                        parentCat.display_order,
                        parentCat.status_id,

                        ref sql_remark
                    );
                }
            }
            return create_result;
        }

        public bool Update(

            int parent_id,
            int leaf,
            int category_id,
            string photo_file_name,
            string photo_file_extension,
            int display_order,
            int status,

            ref string sql_remark
        )
        {
            int? get_sql_result = 0;
            
            var result = db.sp_UpdateProductCategory(
                            _accessObject.id,
                _accessObject.type, 
                parent_id,
                leaf,
                category_id,
                photo_file_name,
                photo_file_extension,
                display_order,
                status,

                ref get_sql_result, ref sql_remark);

            return get_sql_result.Value == 1 ? true : false;
        }

        public int CountProduct(int product_category_id)
        {
            var query = (from pcl in db.product_category_links

                        where (pcl.category_id == product_category_id)
                        select new ProductCategoryObject{
                            category_id = pcl.category_id
                        });

            var count = query.Count();

            return count;
        }

    }
}
