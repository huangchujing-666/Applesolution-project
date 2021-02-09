using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data.Objects;

using Palmary.Loyalty.Common;
using Palmary.Loyalty.BO.Modules.Administration.Security;
using Palmary.Loyalty.BO.Modules.Administration.SystemControl;
using Palmary.Loyalty.BO.Database;
using Palmary.Loyalty.BO.DataTransferObjects.DB;
using Palmary.Loyalty.BO.DataTransferObjects.Security;
using Palmary.Loyalty.BO.DataTransferObjects.Form;
using Palmary.Loyalty.BO.DataTransferObjects.Product;
using Palmary.Loyalty.BO.DataTransferObjects.Role;

using Palmary.Loyalty.BO.Modules.Member;
using Palmary.Loyalty.BO.Modules.Utility;

namespace Palmary.Loyalty.BO.Modules.Product
{
    public class ProductManager
    {
        private static LoyaltyDBDataContext db = new LoyaltyDBDataContext();
        private AccessManager _accessManager;
        private string _module = CommonConstant.Module.product;
        private RolePrivilegeObject _privilege;
        private static LogManager _logManager;
        private AccessObject _accessObject;

         // For backend, using BO Session to access
        public ProductManager()
        {
            // access object
            _accessManager = new AccessManager();
            _accessObject = _accessManager.GetAccessObjectBySession();

            // privilege and log
            _privilege = _accessManager.AccessModule(_module);
            _logManager = new LogManager();
        }

        // For Webservices or other not using BO Session to access
        public ProductManager(AccessObject accessObject)
        {
            // access object
            _accessManager = new AccessManager();
            _accessObject = accessObject;

            // privilege and log
            _privilege = _accessManager.AccessModule(_module, _accessObject.type, _accessObject.id);
            _logManager = new LogManager(_accessObject);
        }

        // this function will depreciate in future
        public IEnumerable<sp_GetProductListsResult> GetProductLists_sp(int user_id, long rowIndexStart, int rowLimit, string searchParams)
        {
            int? get_sql_result = 0;
            var sql_remark = "";

            var result = db.sp_GetProductLists(user_id, rowIndexStart, rowLimit, searchParams, ref get_sql_result, ref sql_remark);
            return result;
        }

        public ProductObject GetDetail(int product_id, bool rootAccess, ref CommonConstant.SystemCode systemCode)
        {
            ProductObject resultObj;

            if (_privilege.read_status == 1 || rootAccess)
            {
                var query = (from p in db.products
                             join p_l in db.product_langs on p.product_id equals p_l.product_id

                             join li in db.listing_items on p.status equals li.value
                             join l in db.listings on li.list_id equals l.list_id

                             join pp in db.product_photos on p.product_id equals pp.product_id into tt_table
                             from pp in tt_table.DefaultIfEmpty()
                             where (pp.display_order == 1) // left outer join
                             where (
                                 p.record_status != (int)CommonConstant.RecordStatus.deleted
                                 && l.code == "Status"
                                 && p.product_id == product_id
                             )
                             select new ProductObject
                             {
                                 product_id = p.product_id,
                                 product_no = p.product_no,
                                 price = p.price,
                                 point = p.point,
                                 consumption_period = p.consumption_period,
                                 lost_customer_period = p.lost_customer_period,
                                 status = p.status,
                                 crt_date = p.crt_date,
                                 crt_by_type = p.crt_by_type,
                                 crt_by = p.crt_by,
                                 upd_date = p.upd_date,
                                 upd_by_type = p.upd_by_type,
                                 upd_by = p.upd_by,
                                 record_status = p.record_status,

                                 //-- additional info
                                 name = p_l.name,
                                 status_name = li.name,
                                 file_name = pp.file_name,
                                 file_extension = pp.file_extension
                             });

                resultObj = query.FirstOrDefault();

                systemCode = CommonConstant.SystemCode.normal;
            }
            else
            {
                resultObj = new ProductObject();
                systemCode = CommonConstant.SystemCode.no_permission;
            }

            return resultObj;
        }

        public ProductObject GetDetailByProductNo(string product_no, bool rootAccess, ref CommonConstant.SystemCode systemCode)
        {
            ProductObject resultObj;

            if (_privilege.read_status == 1 || rootAccess)
            {
                var query = (from p in db.products
                             join p_l in db.product_langs on p.product_id equals p_l.product_id

                             join li in db.listing_items on p.status equals li.value
                             join l in db.listings on li.list_id equals l.list_id

                             //join pp in db.product_photos on p.product_id equals pp.product_id into tt_table
                             //from pp in tt_table.DefaultIfEmpty()
                             //where (pp.display_order == 1) // left outer join
                             where (
                                 p.record_status != (int)CommonConstant.RecordStatus.deleted
                                 && l.code == "Status"
                                 && p.product_no == product_no
                                 && p_l.lang_id == (int)CommonConstant.LangCode.en
                             )
                             select new ProductObject
                             {
                                 product_id = p.product_id,
                                 product_no = p.product_no,
                                 price = p.price,
                                 point = p.point,
                                 consumption_period = p.consumption_period,
                                 lost_customer_period = p.lost_customer_period,
                                 status = p.status,
                                 crt_date = p.crt_date,
                                 crt_by_type = p.crt_by_type,
                                 crt_by = p.crt_by,
                                 upd_date = p.upd_date,
                                 upd_by_type = p.upd_by_type,
                                 upd_by = p.upd_by,
                                 record_status = p.record_status,

                                 //-- additional info
                                 name = p_l.name,
                                 status_name = li.name,
                                 //file_name = pp.file_name,
                                 //file_extension = pp.file_extension
                             });

                systemCode = CommonConstant.SystemCode.normal;
                if (query.Count() == 1)
                {
                    resultObj = query.FirstOrDefault();
                    systemCode = CommonConstant.SystemCode.normal;
                }
                else
                {
                    resultObj = new ProductObject();
                    systemCode = CommonConstant.SystemCode.err_product_not_exist;
                }
            }
            else
            {
                resultObj = new ProductObject();
                systemCode = CommonConstant.SystemCode.no_permission;
            }

            return resultObj;
        }

        // Get List with paging, dynamic search, dynamic sorting
        public List<ProductObject> GetList(int rowIndexStart, int rowLimit, List<SearchParmObject> searchParmList, string sortColumn, CommonConstant.SortOrder sortOrder, ref CommonConstant.SystemCode systemCode, ref int totalRow)
        {
            List<ProductObject> resultList;

            if (_privilege.read_status == 1)
            {
                var query = (from p in db.products
                             join p_l in db.product_langs on p.product_id equals p_l.product_id

                             join li in db.listing_items on p.status equals li.value
                             join l in db.listings on li.list_id equals l.list_id

                             join pp in db.product_photos on p.product_id equals pp.product_id into tt_table
                             from pp in tt_table.DefaultIfEmpty()
                             where (pp.display_order == 1) // left outer join
                             where (
                                 p.record_status != (int)CommonConstant.RecordStatus.deleted
                                 && p_l.lang_id == (int)CommonConstant.LangCode.en
                                 && l.code == "Status"
                             )
                             select new ProductObject
                             {
                                 product_id = p.product_id,
                                 product_no = p.product_no,
                                 price = p.price,
                                 point = p.point,
                                 consumption_period = p.consumption_period,
                                 lost_customer_period = p.lost_customer_period,
                                 status = p.status,
                                 crt_date = p.crt_date,
                                 crt_by_type = p.crt_by_type,
                                 crt_by = p.crt_by,
                                 upd_date = p.upd_date,
                                 upd_by_type = p.upd_by_type,
                                 upd_by = p.upd_by,
                                 record_status = p.record_status,

                                 //-- additional info
                                 name = p_l.name,
                                 status_name = li.name,
                                 file_name = pp.file_name,
                                 file_extension = pp.file_extension
                             });

                // dynamic search
                foreach (var f in searchParmList)
                {
                    if (f.property == "name")
                    {
                        query = query.Where(x => x.name.ToLower().Contains(f.value.ToLower()));
                    }
                    else if (f.property == "product_no")
                    {
                        query = query.Where(x => x.product_no.Contains(f.value));
                    }
                }

                // dynamic sort
                var orderByColumn = "";
                if (sortColumn == "product_no"
                    || sortColumn == "name"
                    || sortColumn == "status_name"
                    )
                    orderByColumn = sortColumn;
                else
                { //default
                    sortOrder = CommonConstant.SortOrder.asc;
                    orderByColumn = "product_no";
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
                resultList = new List<ProductObject>();
                systemCode = CommonConstant.SystemCode.no_permission;
            }

            return resultList;
        }

        // Get whole list (limited data)
        public List<ProductObject> GetList(ref CommonConstant.SystemCode systemCode)
        {
            List<ProductObject> resultList;

            if (_privilege.read_status == 1)
            {
                var query = (from p in db.products
                             join p_l in db.product_langs on p.product_id equals p_l.product_id

                             join li in db.listing_items on p.status equals li.value
                             join l in db.listings on li.list_id equals l.list_id

                             where (
                                p.record_status != (int)CommonConstant.RecordStatus.deleted
                                && p_l.lang_id == (int)CommonConstant.LangCode.en
                                && l.code == "Status"
                            )
                             select new ProductObject
                             {
                                 product_id = p.product_id,
                                 product_no = p.product_no,
                                 price = p.price,
                                 point = p.point,
                                 consumption_period = p.consumption_period,
                                 lost_customer_period = p.lost_customer_period,
                                 status = p.status,
                                 crt_date = p.crt_date,
                                 crt_by_type = p.crt_by_type,
                                 crt_by = p.crt_by,
                                 upd_date = p.upd_date,
                                 upd_by_type = p.upd_by_type,
                                 upd_by = p.upd_by,
                                 record_status = p.record_status,

                                 //-- additional info
                                 name = p_l.name,
                                 status_name = li.name,
                             });

                resultList = query.OrderBy(x => x.product_no).ToList();

                systemCode = CommonConstant.SystemCode.normal;
            }
            else
            {
                resultList = new List<ProductObject>();
                systemCode = CommonConstant.SystemCode.no_permission;
            }

            return resultList;
        }

        // Get whole list (limited data)
        public List<ProductObject> GetListByCategory(int category_id, int rowIndexStart, int rowLimit, bool rootAccess, ref CommonConstant.SystemCode systemCode, ref int totalRow)
        {
            List<ProductObject> resultList;

            if (_privilege.read_status == 1 || rootAccess)
            {
                var query = (from pcl in db.product_category_links
                             join p in db.products on pcl.product_id equals p.product_id
                             join pl in db.product_langs on p.product_id equals pl.product_id

                             join pc_lang in db.product_category_langs on pcl.category_id equals pc_lang.category_id
                             

                             join li in db.listing_items on p.status equals li.value
                             join l in db.listings on li.list_id equals l.list_id

                             join pp in db.product_photos on p.product_id equals pp.product_id into tt_table
                             from pp in tt_table.DefaultIfEmpty()
                             where (pp.display_order == 1) // left outer join

                             where (
                                p.record_status != (int)CommonConstant.RecordStatus.deleted
                                && pc_lang.lang_id == (int)CommonConstant.LangCode.en
                                && pl.lang_id == (int)CommonConstant.LangCode.en
                                && l.code == "Status"
                                && (category_id != 0 && pcl.category_id == category_id) // not allow cat id = 0 as multi cat
                            )
                             select new ProductObject
                             {
                                 product_id = p.product_id,
                                 product_no = p.product_no,
                                 price = p.price,
                                 point = p.point,
                                 consumption_period = p.consumption_period,
                                 lost_customer_period = p.lost_customer_period,
                                 status = p.status,
                                 crt_date = p.crt_date,
                                 crt_by_type = p.crt_by_type,
                                 crt_by = p.crt_by,
                                 upd_date = p.upd_date,
                                 upd_by_type = p.upd_by_type,
                                 upd_by = p.upd_by,
                                 record_status = p.record_status,

                                 //-- additional info
                                 name = pl.name,
                                 status_name = li.name,
                                 category_name = pc_lang.name,
                                 category_id = pcl.category_id,
                                 file_name = pp.file_name,
                                 file_extension = pp.file_extension
                             });

                // row total
                totalRow = query.Count();

                resultList = query.OrderBy(x => x.name).Skip(rowIndexStart).Take(rowLimit).ToList();

                systemCode = CommonConstant.SystemCode.normal;
            }
            else
            {
                resultList = new List<ProductObject>();
                systemCode = CommonConstant.SystemCode.no_permission;
            }

            return resultList;
        }


        public List<ProductObjectByCategory> GetProductListsByCategory(int category_id)
        {
            var query = (from p in db.products
                         join p_l in db.product_langs on p.product_id equals p_l.product_id
                         join li in db.listing_items on p.status equals li.value
                         join l in db.listings on li.list_id equals l.list_id
                         join pcl in db.product_category_links on p.product_id equals pcl.product_id

                         where (
                            l.code == "Status" 
                            && pcl.category_id == category_id
                            && p_l.lang_id == _accessObject.languageID
                        )
                         select new ProductObjectByCategory
                         {
                             category_id = pcl.category_id,
                             display_order = pcl.display_order,
                             product_id = p.product_id,
                             product_no = p.product_no,
                             price = p.price,
                             point = p.point,
                             consumption_period = p.consumption_period,
                             lost_customer_period = p.lost_customer_period,

                             status = p.status,
                             crt_date = p.crt_date,
                             crt_by_type = p.crt_by_type,
                             crt_by = p.crt_by,
                             upd_date = p.upd_date,
                             upd_by_type = p.upd_by_type,
                             upd_by = p.upd_by,
                             record_status = p.record_status,

                             name = p_l.name,
                             status_name = l.name

                         }).OrderBy(x => x.display_order);

            return query.ToList();
        }

        //public IEnumerable<sp_GetProductCustomInfoLists_Result> GetProductCustomInfoLists(int user_id, int product_id)
        //{
        //    var get_sql_result = new ObjectParameter("sql_result", typeof(int));
        //    var get_sql_remark = new ObjectParameter("sql_remark", typeof(string));

        //    var result = _entities.sp_GetProductCustomInfoLists(user_id, product_id, get_sql_result, get_sql_remark);
        //    return result;
        //}

        public sp_GetProductDetailByResult GetProductDetailBy(int user_id, int? product_id, string product_no, ref bool sql_result)
        {
            int? get_sql_result = 0;
            var sql_remark = "";

            var result = db.sp_GetProductDetailBy(user_id, product_id, product_no, ref get_sql_result, ref sql_remark);

            return result.FirstOrDefault() ?? new sp_GetProductDetailByResult();
        }

        public bool Update(
            ProductObject product,

            ChangedField[] changedFields,
            ref string sql_remark)
        {
            AccessManager accessManager = new AccessManager();
            var privilege = accessManager.AccessModule("product");
            var update_result = false;

            if (privilege.insert_status == 1)
            {
                int? get_sql_result = 0;

                var result = db.sp_UpdateProduct(
                    SessionManager.Current.obj_id,
                    product.product_id,
                    product.product_no,

                    product.price,
                    product.point,
                    product.consumption_period,
                    product.lost_customer_period,

                    product.status,
                    ref get_sql_result, ref sql_remark);

                update_result = get_sql_result.Value == 1 ? true : false;

                if (update_result)
                {
                    // Update object table
                    //SystemObjectManager systemObjectManager = new SystemObjectManager();

                    //var object_name = "";
                    //var object_power_search_list = new List<string>();
                    //if (!String.IsNullOrWhiteSpace(product.product_no)) object_power_search_list.Add(product.product_no);

                    //foreach (var theLang in product.lang_list)
                    //{
                    //    if (theLang.lang_id == (int)CommonConstant.LangCode.en) // ENG name as Object Name
                    //        object_name = theLang.name;

                    //    if (!String.IsNullOrWhiteSpace(theLang.name)) object_power_search_list.Add(theLang.name);
                    //    if (!String.IsNullOrWhiteSpace(theLang.description)) object_power_search_list.Add(theLang.description);
                    //}

                    //var power_search = String.Join(" ", object_power_search_list.ToArray());

                    //var sql_update_obj_remark = "";
                    //var update_flag = systemObjectManager.Update(
                    //    SessionManager.Current.obj_id, //access_user_id
                    //    product.product_id,        //object_id
                    //    object_name,
                    //    product.status,         //status
                    //    power_search,   //power_search
                    //    ref sql_update_obj_remark
                    //);

                    //update_result = update_flag;

                    //Update Category
                    var categoryLinkManager = new ProductCategoryLinkManager();
                    categoryLinkManager.DeleteOwnedList(product.product_id);
                    foreach (var c in product.category_list)
                    {
                        var cat_result = categoryLinkManager.Create(product.product_id, c.category_id);
                    }

                    // Update Photo
                    var productPhotoManager = new ProductPhotoManager();

                    foreach (var photo in product.photo_list)
                    {
                        if (photo.photo_id == 0)
                        {
                            update_result = productPhotoManager.Create(

                                  product.product_id,
                                  photo.file_name,
                                  photo.file_extension,
                                  photo.name,
                                  photo.caption,
                                  photo.display_order, // display_order
                                  photo.status, // status

                                  ref sql_remark
                                );
                        }
                        else if (photo.photo_id >= 0)
                        {

                            update_result = productPhotoManager.Update(

                                  photo.photo_id,
                                  product.product_id,
                                  photo.file_name,
                                  photo.file_extension,
                                  photo.name,
                                  photo.caption,
                                  photo.display_order, // display_order
                                  photo.status, // status

                                  ref sql_remark
                                );
                        }
                    }

                    // Take Log
                    //LogManager logManager = new LogManager();
                    //var theLog = new LogObject()
                    //{
                    //    action_ip = AccessObject.ip,
                    //    action_channel = AccessObject.action_channel,
                    //    action_type = Common.CommonConstant.ActionType.update,
                    //    target_obj_id = product.product_id,
                    //    target_obj_type_id = null,
                    //    target_obj_name = null,
                    //    action_detail = null
                    //};
                    //logManager.Create(theLog);
                }
            }
            else
            {
                sql_remark = "No Access";
            }

            return update_result;
        }

        public bool Create(
            ProductObject product,

            ref string sql_remark
        )
        {
            var create_result = false;
            sql_remark = "";

            AccessManager accessManager = new AccessManager();
            var privilege = accessManager.AccessModule("product");

            if (privilege.insert_status == 1)
            {
                int? get_sql_result = 0;

                // [START] Create object into object table
                //SystemObjectManager systemObjectManager = new SystemObjectManager();
                //var new_object_id = 0;
                //var new_object_sql_remark = "";

                //var object_name = "";
                //var object_power_search_list = new List<string>();
                //if (!String.IsNullOrWhiteSpace(product.product_no)) object_power_search_list.Add(product.product_no);

                //foreach (var theLang in product.lang_list)
                //{
                //    if (theLang.lang_id == (int)CommonConstant.LangCode.en) // ENG name as Object Name
                //        object_name = theLang.name;

                //    if (!String.IsNullOrWhiteSpace(theLang.name)) object_power_search_list.Add(theLang.name);
                //    if (!String.IsNullOrWhiteSpace(theLang.description)) object_power_search_list.Add(theLang.description);
                //}

                //var power_search = String.Join(" ", object_power_search_list.ToArray());

                //create_result = systemObjectManager.Create(
                //   SessionManager.Current.obj_id,

                //   Common.CommonConstant.ObjectType.product,
                //   object_name,
                //   product.status,
                //   power_search,
                //   ref new_object_id,
                //   ref new_object_sql_remark
                // );
                // [END] Create Object

                int? new_object_id = 0;
                var result = db.sp_CreateProduct(
                                  _accessObject.id,
                _accessObject.type, 
                    product.product_no,

                    product.price,
                    product.point,
                    product.consumption_period,
                    product.lost_customer_period,
                    product.status,
                    ref new_object_id,
                    ref get_sql_result, ref sql_remark
                );

                create_result = (int.Parse(get_sql_result.Value.ToString()) == 1 ? true : false);

                if (create_result)
                {
                    // Update product id in lang object list
                    product.product_id = new_object_id.Value;
                    foreach (var theLang in product.lang_list)
                    {
                        theLang.product_id = new_object_id.Value;
                    }

                    // Create Category
                    var categoryManager = new ProductCategoryLinkManager();
                    foreach (var c in product.category_list)
                    {
                        categoryManager.Create(new_object_id.Value, c.category_id);
                    }

                    // Create Photo
                    ProductPhotoManager productPhotoManager = new ProductPhotoManager();
                    foreach (var photo in product.photo_list)
                    {
                        create_result = productPhotoManager.Create(

                              new_object_id.Value,
                              photo.file_name,
                              photo.file_extension,
                              photo.name,
                              photo.caption,
                              photo.display_order, // display_order
                              photo.status, // status

                              ref sql_remark
                            );
                    }

                    // Create Lang
                    ProductLangManager productLangManager = new ProductLangManager();
                    create_result = productLangManager.Create(product.lang_list, ref sql_remark);

                    // Take Log
                    //LogManager logManager = new LogManager();
                    //var theLog = new LogObject()
                    //{
                    //    action_ip = AccessObject.ip,
                    //    action_channel = AccessObject.action_channel,
                    //    action_type = Common.CommonConstant.ActionType.create,
                    //    target_obj_id = new_object_id,
                    //    target_obj_type_id = null,
                    //    target_obj_name = null,
                    //    action_detail = null
                    //};
                    //logManager.Create(theLog);

                }
                else
                {   // remove object if fail to create
                    //systemObjectManager.Delete(SessionManager.Current.obj_id, new_object_id);
                }
            }
             
            return create_result;
        }


        public bool GetProductID(
           int user_id,

           string product_no,
           ref int product_id)
        {
            int? get_product_id = 0;
            int? get_sql_result = 0;
            var sql_remark = "";

            var result = db.sp_GetProductID(
                user_id,

                product_no,
                ref get_product_id,
                ref get_sql_result, ref sql_remark);

            product_id = get_product_id.Value;
            return get_sql_result.Value == 1 ? true : false;
        }

        public bool CheckDuplicateProductNo(
            string product_no)
        {
            int? get_duplicate = 0;
            int? get_sql_result = 0;
            var sql_remark = "";

            var result = db.sp_CheckDuplicate_productNo(
                SessionManager.Current.obj_id,

                product_no,
                ref get_duplicate,
                ref get_sql_result, ref sql_remark);

            var duplicate = get_duplicate.Value;
            return (duplicate == 1 ? true : false);
        }
    }
}