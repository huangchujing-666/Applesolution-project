using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Palmary.Loyalty.BO.Database;
using System.Data.Objects;
using Palmary.Loyalty.BO.Modules.Administration.SystemControl;
using Palmary.Loyalty.BO.DataTransferObjects.Security;
using Palmary.Loyalty.BO.Modules.Administration.Security;
using Palmary.Loyalty.BO.DataTransferObjects.Product;
using Palmary.Loyalty.Common;
using Palmary.Loyalty.BO.DataTransferObjects.Role;

namespace Palmary.Loyalty.BO.Modules.Product
{
    public class ProductPhotoManager
    {
        private static LoyaltyDBDataContext db = new LoyaltyDBDataContext();

            private AccessManager _accessManager;
        private string _module = CommonConstant.Module.productPhoto;
        private RolePrivilegeObject _privilege;
        private static LogManager _logManager;
        private AccessObject _accessObject;

        // For backend, using BO Session to access
        public ProductPhotoManager()
        {
            // access object
            _accessManager = new AccessManager();
            _accessObject = _accessManager.GetAccessObjectBySession();

            // privilege and log
            _privilege = _accessManager.AccessModule(_module);
            _logManager = new LogManager();
        }

        public bool Create(
            
            int product_id,
            string file_name,
            string file_extension,
            string name,
            string caption,
            int display_order,
            int status,
            
            ref string sql_remark)
        {
            int? get_sql_result = 0;

            var result = db.sp_CreateProductPhoto(
                _accessObject.id,
                _accessObject.type, 

                product_id,
                file_name,
                file_extension,
                name,
                caption,
                display_order,
                status,

                ref get_sql_result, ref sql_remark);

            return get_sql_result.Value == 1 ? true : false;
        }

        public List<ProductPhotoObject> GetList(int product_id, ref CommonConstant.SystemCode systemCode)
        {
            List<ProductPhotoObject> resultList;
            var query = (from p in db.product_photos
                         where (
                             p.record_status != (int)CommonConstant.RecordStatus.deleted
                             && p.product_id == product_id
                         )
                         select new ProductPhotoObject
                         {
                             photo_id = p.photo_id,
                             product_id = p.product_id,
                             file_name = p.file_name,
                             file_extension = p.file_extension,
                             name = p.name,
                             caption = p.caption,
                             display_order = p.display_order,
                             status = p.status,
                             crt_date = p.crt_date,
                             crt_by_type = p.crt_by_type,
                             crt_by = p.crt_by,
                             upd_date = p.upd_date,
                             upd_by_type = p.upd_by_type,
                             upd_by = p.upd_by,
                             record_status = p.record_status
                         });

            resultList = query.OrderBy(x => x.display_order).ToList();

            systemCode = CommonConstant.SystemCode.normal;

            return resultList;
        }

        // UPDATE
        public bool Update(

            int photo_id,
            int product_id,
            string file_name,
            string file_extension,
            string name,
            string caption,
            int display_order,
            int status,

            ref string sql_remark
        )
        {
            int? get_sql_result = 0;
            
            var result = db.sp_UpdateProductPhoto(
             _accessObject.id,
                _accessObject.type, 

                photo_id,
                product_id,
                file_name,
                file_extension,
                name,
                caption,
                display_order,
                status,

                ref get_sql_result, ref sql_remark);

            return get_sql_result.Value == 1 ? true : false;
        }  
    }
}
