using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Palmary.Loyalty.BO.Database;
using System.Data.Objects;
using Palmary.Loyalty.Common.Languages;
using Palmary.Loyalty.BO.DataTransferObjects.Security;
using Palmary.Loyalty.BO.DataTransferObjects.Product;
using Palmary.Loyalty.BO.Modules.Administration.Security;
using Palmary.Loyalty.Common;
using Palmary.Loyalty.BO.DataTransferObjects.Role;

namespace Palmary.Loyalty.BO.Modules.Product
{
    public class ProductLangManager
    {
        private static LoyaltyDBDataContext db = new LoyaltyDBDataContext();
            private AccessManager _accessManager;
        private string _module = CommonConstant.Module.product;
        private RolePrivilegeObject _privilege;
        private static LogManager _logManager;
        private AccessObject _accessObject;

        // For backend, using BO Session to access
        public ProductLangManager()
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
           int lang_id,
           string name,
           string description,
           int status,

           ref string sql_remark
       )
        {
            int? get_sql_result = 0;
            
            var result = db.sp_CreateProductLang(
      _accessObject.id,
                _accessObject.type, 

                product_id,
                lang_id,
                name,
                description,
                status,

                ref get_sql_result, ref sql_remark);

            return get_sql_result.Value == 1 ? true : false;
        }

        public bool Create(
           List<ProductLangObject> product_lang_list,
           ref string sql_remark
       )
        {
            int? get_sql_result = 0;
            
            var sql_result = false;

            foreach (var theLang in product_lang_list)
            {
                var result = db.sp_CreateProductLang(
                _accessObject.id, 
                _accessObject.type, 

                theLang.product_id,
                theLang.lang_id,
                theLang.name,
                theLang.description,
                theLang.status,

                ref get_sql_result, ref sql_remark);

                sql_result = get_sql_result.Value == 1 ? true : false;
            }

            return sql_result;
        }

        public IEnumerable<sp_GetProductLang_ownedListResult> GetProductLang_ownedList(int product_id, ref bool sql_result)
        {
            int? get_sql_result = 0;
            var sql_remark = "";

            var result = db.sp_GetProductLang_ownedList(SessionManager.Current.obj_id, product_id, ref get_sql_result, ref sql_remark);
            return result;
        }

        public bool Update(

           int product_id,
           int lang_id,
           string name,
           string description,
           int status,

           ref string sql_remark
       )
        {
            int? get_sql_result = 0;
            
            var sql_run = db.sp_UpdateProductLang(
               _accessObject.id,
                _accessObject.type, 


                product_id,
                lang_id,
                name,
                description,
                status,

                ref get_sql_result, ref sql_remark);

            var update_result = get_sql_result.Value == 1 ? true : false;

            return update_result;
        }  
    }
}