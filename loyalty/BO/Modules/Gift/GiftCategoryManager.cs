using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Palmary.Loyalty.BO.Database;
using System.Data.Objects;
using Palmary.Loyalty.BO.Modules.Administration.Security;
using Palmary.Loyalty.BO.DataTransferObjects.Security;
using Palmary.Loyalty.Common;
using Palmary.Loyalty.BO.DataTransferObjects.Role;
using Palmary.Loyalty.BO.DataTransferObjects.Gift;
using Palmary.Loyalty.BO.DataTransferObjects.User;

namespace Palmary.Loyalty.BO.Modules.Gift
{
    public class GiftCategoryManager
    {
        private static LoyaltyDBDataContext db = new LoyaltyDBDataContext();
        private AccessManager _accessManager;
        private string _module = CommonConstant.Module.giftCategory;
        private RolePrivilegeObject _privilege;
        private static LogManager _logManager;
        private AccessObject _accessObject;

        // For backend, using BO Session to access
        public GiftCategoryManager()
        {
            // access object
            _accessManager = new AccessManager();
            _accessObject = _accessManager.GetAccessObjectBySession();

            // privilege and log
            _privilege = _accessManager.AccessModule(_module);
            _logManager = new LogManager();
        }

        // For Webservices or other not using BO Session to access
        public GiftCategoryManager(AccessObject accessObject)
        {
            // access object
            _accessManager = new AccessManager();
            _accessObject = accessObject;

            // privilege and log
            _privilege = _accessManager.AccessModule(_module, _accessObject.type, _accessObject.id);
            _logManager = new LogManager(_accessObject);
        }

        // get parent category only
        public List<GiftCategoryObject> GetListParent(bool rootAccess, ref CommonConstant.SystemCode systemCode)
        {
            List<GiftCategoryObject> resultList;

            if (_privilege.read_status == 1 || rootAccess)
            {
                var query = (from gc in db.gift_categories
                             join gcl in db.gift_category_langs on gc.category_id equals gcl.category_id

                             where (
                                 gc.record_status != (int)CommonConstant.RecordStatus.deleted
                                 && gc.status == CommonConstant.Status.active
                                 && gcl.lang_id == _accessObject.languageID
                                 && gc.parent_id == 0
                             )
                             select new GiftCategoryObject
                             {
                                 category_id = gc.category_id,
                                 parent_id = gc.parent_id,
                                 leaf = gc.leaf,
                                 photo_file_name = gc.photo_file_name,
                                 photo_file_extension = gc.photo_file_extension,
                                 display_order = gc.display_order,
                                 status = gc.status,
                                 crt_date = gc.crt_date,
                                 crt_by_type = gc.crt_by_type,
                                 crt_by = gc.crt_by,
                                 upd_date = gc.upd_date,
                                 upd_by_type = gc.upd_by_type,
                                 upd_by = gc.upd_by,
                                 record_status = gc.record_status,

                                 //-- additional info
                                 name = gcl.name
                             });

                resultList = query.ToList();

                systemCode = CommonConstant.SystemCode.normal;
            }
            else
            {
                resultList = new List<GiftCategoryObject>();
                systemCode = CommonConstant.SystemCode.no_permission;
            }

            return resultList;
        }

        public bool Create(
            int user_id,

            int parent_id,
            string photo_file_name,
            string photo_file_extension,
            int display_order,
            int status,
            
            ref int new_giftCategory_id,
            ref string sql_remark
        )
        {
            bool create_result = false;
            AccessManager accessManager = new AccessManager();
            var privilege = accessManager.AccessModule("giftcategory");
            
            if (privilege.insert_status == 1)
            {
                int? get_category_id = 0;
                int? get_sql_result = 0;
                
                var result = db.sp_CreateGiftCategory(
                    _accessObject.id, 
                    _accessObject.type, 
                    parent_id,
                    0, //leaf , new add should be leaf
                    photo_file_name,
                    photo_file_extension,
                    display_order,
                    status,

                    ref get_category_id,
                    ref get_sql_result,
                    ref sql_remark);

                new_giftCategory_id = get_category_id.Value;
                
                create_result = get_sql_result == 1 ? true : false;

                if (create_result) //Update parent cats leaf values
                {
                    var sql_result = false;
                    var parentCat = GetGiftCategoryDetail(SessionManager.Current.obj_id, parent_id, ref sql_result);
                    System.Diagnostics.Debug.WriteLine(parent_id, "parent_id");
                    System.Diagnostics.Debug.WriteLine(parentCat.leaf, "leaf");

                    if (parentCat.leaf == 1)
                    {
                        sql_result = Update(
                            SessionManager.Current.obj_id,
                            parentCat.parent_id,
                            0, //leaf,  parent's leaf value should be changed to 0
                            parentCat.category_id,
                            parentCat.photo_file_name,
                            parentCat.photo_file_extension,
                            parentCat.display_order,
                            parentCat.status,

                            ref sql_remark
                        );
                    }
                }
            }
            else
            {
                sql_remark = "No Access";
            }
            return create_result;
        }

        public bool Update(
            int user_id,

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
            var update_result = false;

            AccessManager accessManager = new AccessManager();
            var privilege = accessManager.AccessModule("user");
            
            if (privilege.update_status == 1)
            {
                int? get_sql_result = 0;
                
                var result = db.sp_UpdateGiftCategory(
                    
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

                update_result = get_sql_result == 1 ? true : false;
            }
            else
            {
                sql_remark = "No Access";
            }
            return update_result;
        }
       
        public IEnumerable<sp_GetGiftCategoryListsResult> GetGiftCategoryLists(int user_id, long rowIndexStart, int rowLimit, string searchParams)
        {
            int? get_sql_result = 0;
            var sql_remark = "";

            IEnumerable<sp_GetGiftCategoryListsResult> result = null;

            try
            {
                result = db.sp_GetGiftCategoryLists(user_id, _accessObject.languageID, ref get_sql_result, ref sql_remark); //rowIndexStart, rowLimit, searchParams, get_sql_result, get_sql_remark);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString(), "get gift error");
            }

            return result;
        }

        public sp_GetGiftCategoryDetailResult GetGiftCategoryDetail(int user_id, int giftCategory_id, ref bool sql_result)
        {
            int? get_sql_result = 0;
            var sql_remark = "";

            System.Diagnostics.Debug.WriteLine(user_id, "user_id");

            var result = db.sp_GetGiftCategoryDetail(user_id, giftCategory_id, ref get_sql_result, ref sql_remark);

            return result.FirstOrDefault() ?? new sp_GetGiftCategoryDetailResult();
        }

        public IEnumerable<sp_GetGiftCategory_childListResult> GetGiftCategory_childList(int parent_id)
        {
            int? get_sql_result = 0;
            var sql_remark = "";

            IEnumerable<sp_GetGiftCategory_childListResult> result = null;

            try
            {
                result = db.sp_GetGiftCategory_childList(SessionManager.Current.obj_id, parent_id, ref get_sql_result, ref sql_remark); //rowIndexStart, rowLimit, searchParams, get_sql_result, get_sql_remark);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString(), "sp_GetGiftCategory_childList_Result error");
            }

            return result;
        }

        public CommonConstant.SystemCode DelCategory(int selected_parent_id, int user_id, ref string sql_remark)
        {
            // LINQ to Store Procedures
            var systemCode = CommonConstant.SystemCode.undefine;
            if (_privilege.delete_status==1)
            {

                 int? get_sql_result = 0;

                var userObj = GetDetail(user_id, null, null, true, ref systemCode);
                var flag = false;

                if (userObj.user_id > 0)
                {
                    db.sp_SoftDeleteByModule(_accessObject.id, _accessObject.type, CommonConstant.Module.giftCategory, selected_parent_id, ref get_sql_result, ref sql_remark);
                    flag = (int)get_sql_result.Value == 1 ? true : false;

                }
            }
            else
                systemCode = CommonConstant.SystemCode.no_permission;

            return systemCode;
        }

        public UserObject GetDetail(int? search_user_id, string search_user_login_id, string search_user_email, bool rootAccess, ref CommonConstant.SystemCode systemCode)
        {
            // LINQ to SQL
            systemCode = CommonConstant.SystemCode.undefine;
            var resultObj = new UserObject();

            if (_privilege.read_status == 1 || rootAccess)
            {
                var query = (from u in db.user_profiles
                             join li in db.listing_items on u.status equals li.value
                             join l in db.listings on li.list_id equals l.list_id
                             where (
                                u.record_status != (int)CommonConstant.RecordStatus.deleted
                                && l.code == "Status"
                                && (u.user_id == search_user_id || u.login_id == search_user_login_id || u.email == search_user_email)
                            )
                             select new UserObject
                             {
                                 user_id = u.user_id,
                                 login_id = u.login_id,
                                 name = u.name,
                                 password = u.password,
                                 email = u.email,
                                 action_ip = u.action_ip,
                                 action_date = u.action_date,
                                 status = u.status,

                                 crt_date = u.crt_date,
                                 crt_by_type = u.crt_by_type,
                                 crt_by = u.crt_by,
                                 upd_date = u.upd_date,
                                 upd_by_type = u.upd_by_type,
                                 upd_by = u.upd_by,

                                 record_status = u.record_status,

                                 // additional
                                 status_name = li.name
                             });


                if (query.Count() > 0)
                {
                    resultObj = query.FirstOrDefault();
                    systemCode = CommonConstant.SystemCode.normal;
                }
                else
                    systemCode = CommonConstant.SystemCode.record_invalid;
            }
            else
            {
                systemCode = CommonConstant.SystemCode.no_permission;
            }

            return resultObj;
        }
    }
}