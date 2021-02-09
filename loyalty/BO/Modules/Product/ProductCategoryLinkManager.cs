using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Palmary.Loyalty.BO.Database;
using Palmary.Loyalty.BO.DataTransferObjects.Role;
using Palmary.Loyalty.BO.DataTransferObjects.Security;
using Palmary.Loyalty.BO.DataTransferObjects.Product;
using Palmary.Loyalty.BO.DataTransferObjects.Utility;
using Palmary.Loyalty.BO.Modules.Administration.Security;
using Palmary.Loyalty.BO.Modules.Utility;
using Palmary.Loyalty.Common;

namespace Palmary.Loyalty.BO.Modules.Product
{
    public class ProductCategoryLinkManager
    {
        private static LoyaltyDBDataContext db = new LoyaltyDBDataContext();
        private string _module = CommonConstant.Module.productCategory;
        private AccessManager _accessManager;
        private RolePrivilegeObject _privilege;
        private static LogManager _logManager;
        private AccessObject _accessObject;

        public ProductCategoryLinkManager(){
            // access object
            _accessManager = new AccessManager();
            _accessObject = _accessManager.GetAccessObjectBySession();

            // privilege and log
            _privilege = _accessManager.AccessModule(_module);
            _logManager = new LogManager();
        }

        public CommonConstant.SystemCode Create(
            int product_id,
            int category_id
        )
        {
            int? sql_result = 0;
            var system_code = CommonConstant.SystemCode.undefine;

            if (_privilege.insert_status == 1)
            {
                var result = db.sp_CreateProductCategoryLink(
                    _accessObject.id,
                _accessObject.type, 

                    product_id,
                    category_id,
                    0, //display_order,

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

        public List<ProductCategoryLinkObject> GetLinkListByCategory(int category_id)
        {
            var query = (from l in db.product_category_links
                         where (l.category_id == category_id)
                         select new ProductCategoryLinkObject
                         {
                             product_id = l.product_id,
                             category_id = l.category_id,
                             display_order = l.display_order
                         }).OrderBy(x=>x.display_order);

            return query.ToList();
        }

        public List<ProductCategoryLinkObject> GetProductCategory_ownedList(int product_id)
        {
            var query = (from l in db.product_category_links
                         join pc_l in db.product_category_langs on l.category_id equals pc_l.category_id
                         where (pc_l.lang_id == (int)CommonConstant.LangCode.en && l.product_id == product_id)
                         select new ProductCategoryLinkObject
                         {
                             category_id = l.category_id,
                             product_id = l.product_id,
                             category_name = pc_l.name
                         });

            return query.ToList();
        }

        public ProductCategoryLinkObject GetDetail(int product_id, int product_category_id)
        {
            var query = (from link in db.product_category_links
                         join pc_l in db.product_category_langs on link.category_id equals pc_l.category_id
                         join p_l in db.product_langs on link.product_id equals p_l.product_id
                         where (
                                 pc_l.lang_id == (int)CommonConstant.LangCode.en
                                 && p_l.lang_id == (int)CommonConstant.LangCode.en
                                 && link.product_id == product_id
                                 && link.category_id == product_category_id
                                )
                         select new ProductCategoryLinkObject
                         {
                             category_id = link.category_id,
                             product_id = link.product_id,
                             category_name = pc_l.name,
                             product_name = p_l.name,
                             display_order = link.display_order
                         });

            var item = query.FirstOrDefault();

            return item;
        }

        public CommonConstant.SystemCode Update(ProductCategoryLinkObject dataObject)
        {
            int? sql_result = 0;
            var system_code = CommonConstant.SystemCode.undefine;

            if (_privilege.update_status == 1)
            {
                var result = db.sp_UpdateProductCategoryLink(
                  _accessObject.id,
                _accessObject.type, 


                    dataObject.product_id,
                    dataObject.category_id,
                    dataObject.display_order,
                    
                    ref sql_result);

                system_code = (CommonConstant.SystemCode)sql_result.Value;
            }
            else
            {
                system_code = CommonConstant.SystemCode.no_permission;
            }

            return system_code;
        }

        public CommonConstant.SystemCode UpdateDisplayOrder(ProductCategoryLinkObject targetObject)
        { 
            var linkList = GetLinkListByCategory(targetObject.category_id);
            
            var updateObject = new OrderObject<ProductCategoryLinkObject>
            {
                id = targetObject.product_id,
                display_order = targetObject.display_order,
                data_object = targetObject
            };

            var orderObjectList = new List<OrderObject<ProductCategoryLinkObject>>();

            foreach(var x in linkList)
            {
                var orderObject = new OrderObject<ProductCategoryLinkObject>
                {
                    id = x.product_id,
                    display_order = x.display_order,
                    data_object = x,
                };

                orderObjectList.Add(orderObject);
            }

           var updatedList = OrderManager.Reorder(orderObjectList, updateObject);

           
           foreach (var x in updatedList)
           {
               x.data_object.display_order = x.display_order;
               Update(x.data_object);
           }


           //System.Diagnostics.Debug.WriteLine(updatedList.ToJson());
           return CommonConstant.SystemCode.normal;
        }

        public CommonConstant.SystemCode DeleteOwnedList(int product_id)
        {
            int? sql_result = 0;
            var system_code = CommonConstant.SystemCode.undefine;

            if (_privilege.delete_status == 1)
            {
                var result = db.sp_DeleteProductCategoryLink(
                    SessionManager.Current.obj_id,
                    product_id,
                    ref sql_result);

                system_code = (CommonConstant.SystemCode)sql_result.Value;
            }
            else
            {
                system_code = CommonConstant.SystemCode.no_permission;
            }

            return system_code;
        }
    }
}
