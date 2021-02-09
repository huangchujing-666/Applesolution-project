using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Palmary.Loyalty.BO.Database;
using System.Data.Objects;
using Palmary.Loyalty.BO.Modules.Product;
using Palmary.Loyalty.BO.DataTransferObjects.Gift;

using Palmary.Loyalty.Common;

using Palmary.Loyalty.BO.Modules.Administration.Security;
using Palmary.Loyalty.BO.Modules.Administration.SystemControl;
using Palmary.Loyalty.BO.Modules.Utility;
using Palmary.Loyalty.BO.DataTransferObjects.Role;
using Palmary.Loyalty.BO.DataTransferObjects.DB;
using Palmary.Loyalty.BO.DataTransferObjects.Security;
using Palmary.Loyalty.BO.DataTransferObjects.Form;
using Palmary.Loyalty.BO.DataTransferObjects.Utility;

using Palmary.Loyalty.BO.Modules.Member;
using Palmary.Loyalty.BO.DataTransferObjects.Media;

namespace Palmary.Loyalty.BO.Modules.Gift
{
    public class GiftManager
    {
        private static LoyaltyDBDataContext db = new LoyaltyDBDataContext();
        private AccessManager _accessManager;
        private string _module = CommonConstant.Module.gift;
        private RolePrivilegeObject _privilege;
        private static LogManager _logManager;
        private AccessObject _accessObject;
        


        // For backend, using BO Session to access
        
        public GiftManager()
        {
            // access object
            _accessManager = new AccessManager();
            _accessObject = _accessManager.GetAccessObjectBySession();

            // privilege and log
            _privilege = _accessManager.AccessModule(_module);
            _logManager = new LogManager();
        }

        // For Webservices or other not using BO Session to access
        public GiftManager(AccessObject accessObject)
        {
            // access object
            _accessManager = new AccessManager();
            _accessObject = accessObject;

            // privilege and log
            _privilege = _accessManager.AccessModule(_module, _accessObject.type, _accessObject.id);
            _logManager = new LogManager(_accessObject);
        }

        // this function will depreciate in future
        public IEnumerable<sp_GetGiftListsResult> GetGiftLists_sp(int user_id, long rowIndexStart, int rowLimit, string searchParams)
        {
            int? get_sql_result = 0;
            var sql_remark = "";

            IEnumerable<sp_GetGiftListsResult> result = null;

            try
            {
                result = db.sp_GetGiftLists(user_id, ref get_sql_result, ref sql_remark); //rowIndexStart, rowLimit, searchParams, get_sql_result, get_sql_remark);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString(), "get gift error");
            }

            return result;
        }

        
        // Get List with paging, dynamic search, dynamic sorting
        public List<GiftObject> GetList(int rowIndexStart, int rowLimit, List<SearchParmObject> searchParmList, string sortColumn, CommonConstant.SortOrder sortOrder, ref CommonConstant.SystemCode systemCode, ref int totalRow)
        {
            List<GiftObject> resultList;

            if (_privilege.read_status == 1)
            {
                var query = (from g in db.gifts
                             join g_l in db.gift_langs on g.gift_id equals g_l.gift_id
                             join li in db.listing_items on g.status equals li.value
                             join l in db.listings on li.list_id equals l.list_id

                             join gp in db.gift_photos on g.gift_id equals gp.gift_id into gp_table
                             from gp in gp_table.DefaultIfEmpty()
                             where (gp.display_order == 1) // left outer join
                             where (
                                g.record_status != (int)CommonConstant.RecordStatus.deleted
                                && l.code == "Status"
                                && g_l.lang_id == _accessObject.languageID
                            )
                             select new GiftObject
                             {
                                 gift_id = g.gift_id,
                                 gift_no = g.gift_no,
                                 point = g.point,
                                 alert_level = g.alert_level,
                                 cost = g.cost,
                                 discount = g.discount,
                                 discount_point = g.discount_point,
                                 discount_active_date = g.discount_active_date,
                                 discount_expiry_date = g.discount_expiry_date,
                                 hot_item = g.hot_item,
                                 hot_item_active_date = g.hot_item_active_date,
                                 hot_item_expiry_date = g.hot_item_expiry_date,
                                 hot_item_display_order = g.hot_item_display_order,
                                 display_public = g.display_public,
                                 display_active_date = g.display_active_date,
                                 display_expiry_date = g.display_expiry_date,
                                 redeem_active_date = g.redeem_active_date,
                                 redeem_expiry_date = g.redeem_expiry_date,
                                 status = g.status,
                                 available_stock = g.available_stock,

                                 //-- additional info
                                 name = g_l.name,
                                 file_name = gp.file_name,
                                 file_extension = gp.file_extension,
                                 status_name = li.name
                             });

                // dynamic search
                foreach (var f in searchParmList)
                {
                    if (f.property == "gift_no")
                    {
                        query = query.Where(x => x.gift_no.Contains(f.value));
                    }
                    else if (f.property == "name")
                    {
                        query = query.Where(x => x.name.ToLower().Contains(f.value.ToLower()));
                    }
                    else if (f.property == "point_larger")
                    {
                        if (!String.IsNullOrEmpty(f.value))
                            query = query.Where(x => x.point >= double.Parse(f.value));
                    }
                    else if (f.property == "point_lower")
                    {
                        if (!String.IsNullOrEmpty(f.value))
                            query = query.Where(x => x.point <= double.Parse(f.value));
                    }
                    else if (f.property == "available_stock_larger")
                    {
                        if (!String.IsNullOrEmpty(f.value))
                            query = query.Where(x => x.available_stock >= int.Parse(f.value));
                    }
                    else if (f.property == "available_stock_lower")
                    {
                        if (!String.IsNullOrEmpty(f.value))
                            query = query.Where(x => x.available_stock <= int.Parse(f.value));
                    }
                    else if (f.property == "alert_level_larger")
                    {
                        if (!String.IsNullOrEmpty(f.value))
                            query = query.Where(x => x.alert_level >= int.Parse(f.value));
                    }
                    else if (f.property == "alert_level_lower")
                    {
                        if (!String.IsNullOrEmpty(f.value))
                            query = query.Where(x => x.alert_level <= int.Parse(f.value));
                    }
                    else if (f.property == "redeem_active_date_larger")
                    {
                        if (!String.IsNullOrEmpty(f.value))
                            query = query.Where(x => x.redeem_active_date >= Convert.ToDateTime(f.value));
                    }
                    else if (f.property == "redeem_active_date_lower")
                    {
                        if (!String.IsNullOrEmpty(f.value))
                            query = query.Where(x => x.redeem_active_date <= Convert.ToDateTime(f.value + " 23:59:59.999"));
                    }
                    else if (f.property == "redeem_expiry_date_larger")
                    {
                        if (!String.IsNullOrEmpty(f.value))
                            query = query.Where(x => x.redeem_expiry_date >= Convert.ToDateTime(f.value));
                    }
                    else if (f.property == "redeem_expiry_date_lower")
                    {
                        if (!String.IsNullOrEmpty(f.value))
                            query = query.Where(x => x.redeem_expiry_date <= Convert.ToDateTime(f.value + " 23:59:59.999"));
                    }
                    else if (f.property == "hot_item")
                    {
                        if (!String.IsNullOrEmpty(f.value))
                            query = query.Where(x => x.hot_item == (int.Parse(f.value) == 1 ? true : false));
                    }
                    else if (f.property == "discount")
                    {
                        if (!String.IsNullOrEmpty(f.value))
                            query = query.Where(x => x.discount == (int.Parse(f.value) == 1 ? true : false));
                    }
                    else if (f.property == "display_public")
                    {
                        if (!String.IsNullOrEmpty(f.value))
                            query = query.Where(x => x.display_public == (int.Parse(f.value) == 1 ? true : false));
                    }
                    else if (f.property == "status_name")
                    {
                        if (!String.IsNullOrEmpty(f.value))
                            query = query.Where(x => x.status == int.Parse(f.value));
                    }
                }

                // dynamic sort
                var orderByColumn = "";
                if (sortColumn == "gift_no"
                    || sortColumn == "name"
                    || sortColumn == "point"
                    || sortColumn == "redeem_active_date"
                    || sortColumn == "redeem_expiry_date"
                    || sortColumn == "available_stock"
                    || sortColumn == "alert_level"
                    || sortColumn == "status_name"
                    )
                    orderByColumn = sortColumn;
                else
                { //default
                    sortOrder = CommonConstant.SortOrder.asc;
                    orderByColumn = "gift_no";
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
                resultList = new List<GiftObject>();
                systemCode = CommonConstant.SystemCode.no_permission;
            }

            return resultList;
        }

        public List<GiftObject> GetListByCategory(int category_id, int rowIndexStart, int rowLimit, bool rootAccess, ref CommonConstant.SystemCode systemCode, ref int totalRow)
        {
            List<GiftObject> resultList;

            if (_privilege.read_status == 1 || rootAccess)
            {
                var query = (from g in db.gifts
                             join gl in db.gift_langs on g.gift_id equals gl.gift_id
                             join gck in db.gift_category_links on g.gift_id equals gck.gift_id

                             join li in db.listing_items on g.status equals li.value
                             join l in db.listings on li.list_id equals l.list_id

                             join gp in db.gift_photos on g.gift_id equals gp.gift_id into gp_table
                             from gp in gp_table.DefaultIfEmpty()
                             where (gp.display_order == 1) // left outer join
                             where (
                                    g.record_status != (int)CommonConstant.RecordStatus.deleted
                                && gl.lang_id == (int)CommonConstant.LangCode.en
                                && l.code == "Status"
                                && (category_id != 0 && gck.category_id == category_id) // not allow cat id = 0 as multi cat
                            )
                             select new GiftObject
                             {
                                 gift_id = g.gift_id,
                                 gift_no = g.gift_no,
                                 point = g.point,
                                 alert_level = g.alert_level,
                                 cost = g.cost,
                                 discount = g.discount,
                                 discount_point = g.discount_point,
                                 discount_active_date = g.discount_active_date,
                                 discount_expiry_date = g.discount_expiry_date,
                                 hot_item = g.hot_item,
                                 hot_item_active_date = g.hot_item_active_date,
                                 hot_item_expiry_date = g.hot_item_expiry_date,
                                 hot_item_display_order = g.hot_item_display_order,
                                 display_public = g.display_public,
                                 display_active_date = g.display_active_date,
                                 display_expiry_date = g.display_expiry_date,
                                 redeem_active_date = g.redeem_active_date,
                                 redeem_expiry_date = g.redeem_expiry_date,
                                 status = g.status,
                                 available_stock = g.available_stock,

                                 //-- additional info
                                 name = gl.name,
                                 file_name = gp.file_name,
                                 file_extension = gp.file_extension,
                                 status_name = li.name
                             });

                // row total
                totalRow = query.Count();
                resultList = query.OrderBy(x => x.name).Skip(rowIndexStart).Take(rowLimit).ToList();

                systemCode = CommonConstant.SystemCode.normal;
            }
            else
            {
                resultList = new List<GiftObject>();
                systemCode = CommonConstant.SystemCode.no_permission;
            }

            return resultList;
        }

        // Get whole List (limited data)
        public List<GiftObject> GetList(ref CommonConstant.SystemCode systemCode)
        {
            List<GiftObject> resultList;

            if (_privilege.read_status == 1)
            {
                var query = (from g in db.gifts
                             join g_l in db.gift_langs on g.gift_id equals g_l.gift_id

                             where (
                                g.record_status != (int)CommonConstant.RecordStatus.deleted
                                && g_l.lang_id == _accessObject.languageID
                            )
                             select new GiftObject
                             {
                                 gift_id = g.gift_id,
                                 gift_no = g.gift_no,
                                 point = g.point,
                                 alert_level = g.alert_level,
                                 cost = g.cost,
                                 discount = g.discount,
                                 discount_point = g.discount_point,
                                 discount_active_date = g.discount_active_date,
                                 discount_expiry_date = g.discount_expiry_date,
                                 hot_item = g.hot_item,
                                 hot_item_active_date = g.hot_item_active_date,
                                 hot_item_expiry_date = g.hot_item_expiry_date,
                                 hot_item_display_order = g.hot_item_display_order,
                                 display_public = g.display_public,
                                 display_active_date = g.display_active_date,
                                 display_expiry_date = g.display_expiry_date,
                                 redeem_active_date = g.redeem_active_date,
                                 redeem_expiry_date = g.redeem_expiry_date,
                                 status = g.status,
                                 photo_list = db.gift_photos.Where(s => s.gift_id.Equals(g.gift_id)).
                                    Select(s => new PhotoObject
                                    {
                                        photo_id = s.gift_photo_id,
                                        file_name = s.file_name,
                                        file_extension = s.file_extension,
                                        display_order = s.display_order,
                                        name = s.name,
                                        caption = s.caption,
                                        status = s.status
                                    }).ToList(),
                                 //-- additional info
                                 name = g_l.name
                             });

                resultList = query.OrderBy(x => x.gift_no).ToList();

                systemCode = CommonConstant.SystemCode.normal;
            }
            else
            {
                resultList = new List<GiftObject>();
                systemCode = CommonConstant.SystemCode.no_permission;
            }

            return resultList;
        }



        // Get whole List (limited data) allow redeem gift list by specific member
        public List<GiftObject> GetListByMember(int member_id, ref CommonConstant.SystemCode systemCode)
        {
            List<GiftObject> resultList;

            if (_privilege.read_status == 1)
            {
                var query = (from g in db.gifts
                             join gl in db.gift_langs on g.gift_id equals gl.gift_id
                             join gp in db.gift_member_privileges on g.gift_id equals gp.gift_id
                             join m in db.member_profiles on gp.member_level_id equals m.member_level_id
                             where (
                                g.record_status != (int)CommonConstant.RecordStatus.deleted
                                && gl.lang_id == (int)CommonConstant.LangCode.en
                                && gp.allow_redeem == 1
                                && m.member_id == member_id
                            )
                             select new GiftObject
                             {
                                 gift_id = g.gift_id,
                                 gift_no = g.gift_no,
                                 point = g.point,
                                 alert_level = g.alert_level,
                                 cost = g.cost,
                                 discount = g.discount,
                                 discount_point = g.discount_point,
                                 discount_active_date = g.discount_active_date,
                                 discount_expiry_date = g.discount_expiry_date,
                                 hot_item = g.hot_item,
                                 hot_item_active_date = g.hot_item_active_date,
                                 hot_item_expiry_date = g.hot_item_expiry_date,
                                 hot_item_display_order = g.hot_item_display_order,
                                 display_public = g.display_public,
                                 display_active_date = g.display_active_date,
                                 display_expiry_date = g.display_expiry_date,
                                 redeem_active_date = g.redeem_active_date,
                                 redeem_expiry_date = g.redeem_expiry_date,
                                 status = g.status,

                                 //-- additional info
                                 name = gl.name,
                             });

                resultList = query.OrderBy(x => x.gift_no).ToList();

                systemCode = CommonConstant.SystemCode.normal;
            }
            else
            {
                resultList = new List<GiftObject>();
                systemCode = CommonConstant.SystemCode.no_permission;
            }

            return resultList;
        }

        public List<GiftObjectByCategory> GetGiftListsByCategory(int category_id)
        {
            var query = (from g in db.gifts
                         join g_l in db.gift_langs on g.gift_id equals g_l.gift_id
                         join li in db.listing_items on g.status equals li.value
                         join l in db.listings on li.list_id equals l.list_id
                         join gcl in db.gift_category_links on g.gift_id equals gcl.gift_id

                         where (
                            l.code == "Status"
                            && gcl.category_id == category_id
                            && g_l.lang_id == _accessObject.languageID
                         )
                         select new GiftObjectByCategory
                         {
                             category_id = gcl.category_id,
                             display_order = gcl.display_order,
                             gift_id = g.gift_id,
                             gift_no = g.gift_no,
                             point = g.point,
                             alert_level = g.alert_level,
                             cost = g.cost,
                             discount = g.discount,
                             discount_point = g.discount_point,
                             discount_active_date = g.discount_active_date,
                             discount_expiry_date = g.discount_expiry_date,
                             hot_item = g.hot_item,
                             hot_item_active_date = g.hot_item_active_date,
                             hot_item_expiry_date = g.hot_item_expiry_date,
                             hot_item_display_order = g.hot_item_display_order,
                             display_public = g.display_public,
                             display_active_date = g.display_active_date,
                             display_expiry_date = g.display_expiry_date,
                             redeem_active_date = g.redeem_active_date,
                             redeem_expiry_date = g.redeem_expiry_date,
                             status = g.status,

                             name = g_l.name,
                             status_name = l.name
                         }).OrderBy(x => x.display_order);

            return query.ToList();
        }

        public List<GiftObject> GetGiftLists_isHotItem()
        {
            var query = (from g in db.gifts
                         join g_l in db.gift_langs on g.gift_id equals g_l.gift_id
                         join li in db.listing_items on g.status equals li.value
                         join l in db.listings on li.list_id equals l.list_id

                         where (
                            l.code == "Status"
                            && g.hot_item == true
                            && g_l.lang_id == _accessObject.languageID)
                         select new GiftObject
                         {
                             gift_id = g.gift_id,
                             gift_no = g.gift_no,
                             point = g.point,
                             alert_level = g.alert_level,
                             cost = g.cost,
                             discount = g.discount,
                             discount_point = g.discount_point,
                             discount_active_date = g.discount_active_date,
                             discount_expiry_date = g.discount_expiry_date,
                             hot_item = g.hot_item,
                             hot_item_active_date = g.hot_item_active_date,
                             hot_item_expiry_date = g.hot_item_expiry_date,
                             hot_item_display_order = g.hot_item_display_order,
                             display_public = g.display_public,
                             display_active_date = g.display_active_date,
                             display_expiry_date = g.display_expiry_date,
                             redeem_active_date = g.redeem_active_date,
                             redeem_expiry_date = g.redeem_expiry_date,
                             status = g.status,

                             name = g_l.name,
                             status_name = l.name
                         }).OrderBy(x => x.hot_item_display_order);

            return query.ToList();
        }

        public bool Create(
            GiftObject giftObject,

            ref string sql_remark,
            ref int gift_id
        )
        {
            var sql_result = false;
            sql_remark = "";
            int? get_sql_result = 0;

            // [START] Create object into object table
            //SystemObjectManager systemObjectManager = new SystemObjectManager();
            //var new_object_id = 0;
            //var new_object_sql_remark = "";

            //var object_name = "";
            //var object_power_search_list = new List<string>();
            //if (!String.IsNullOrWhiteSpace(giftObject.gift_no)) object_power_search_list.Add(giftObject.gift_no);

            //foreach (var theLang in giftObject.lang_list)
            //{
            //    if (theLang.lang_id == (int)CommonConstant.LangCode.en) // ENG name as Object Name
            //        object_name = theLang.name;

            //    if (!String.IsNullOrWhiteSpace(theLang.name)) object_power_search_list.Add(theLang.name);
            //    if (!String.IsNullOrWhiteSpace(theLang.description)) object_power_search_list.Add(theLang.description);
            //}

            //var power_search = String.Join(" ", object_power_search_list.ToArray());

            //var create_result = systemObjectManager.Create(
            //   SessionManager.Current.obj_id,

            //   Common.CommonConstant.ObjectType.gift,
            //   object_name,
            //   giftObject.status,
            //   power_search,
            //   ref new_object_id,
            //   ref new_object_sql_remark
            // );
            // [END] Create Object

            // Create Gift
            int? new_obj_id = 0;

            var result = db.sp_CreateGift(
_accessObject.id,
_accessObject.type,
                giftObject.gift_no,
                giftObject.point,
                giftObject.alert_level,
                giftObject.cost,

                giftObject.discount,
                giftObject.discount_point,
                giftObject.discount_active_date,
                giftObject.discount_expiry_date,

                giftObject.hot_item,
                giftObject.hot_item_active_date,
                giftObject.hot_item_expiry_date,
                giftObject.hot_item_display_order,


                giftObject.display_public,
                giftObject.display_active_date,
                giftObject.display_expiry_date,
                giftObject.redeem_active_date,
                giftObject.redeem_expiry_date,

                giftObject.status,
                giftObject.available_stock,

                ref new_obj_id,
                ref get_sql_result, ref sql_remark
            );

            var create_result = (get_sql_result.Value == 1 ? true : false);

            if (create_result)
            {
                gift_id = new_obj_id.Value;
                giftObject.gift_id = new_obj_id.Value;

                // Update gift id in lang object list
                foreach (var theLang in giftObject.lang_list)
                {
                    theLang.gift_id = gift_id;
                }

                // Create Category
                var categoryManager = new GiftCategoryLinkManager();
                foreach (var c in giftObject.category_list)
                {
                    categoryManager.Create(gift_id, c.category_id);
                }

                // Create Location
                GiftLocationManager giftLocationManager = new GiftLocationManager();
                foreach (var theLocation in giftObject.location_list)
                {
                    create_result = giftLocationManager.Create(SessionManager.Current.obj_id, giftObject.gift_id, theLocation.location_id, theLocation.status, ref sql_remark);
                }

                // Create Member Privilege
                GiftMemberPrivilegeManager giftMemberPrivilegeManager = new GiftMemberPrivilegeManager();
                System.Diagnostics.Debug.WriteLine(giftObject.member_privilege_list.Count(), " giftObject.member_privilege_list.count");
                foreach (var thePrivilege in giftObject.member_privilege_list)
                {
                    System.Diagnostics.Debug.WriteLine(thePrivilege.member_level_id, " thePrivilege.member_level_id");
                    create_result = giftMemberPrivilegeManager.Create(giftObject.gift_id, thePrivilege.member_level_id, 1, CommonConstant.Status.active, ref sql_remark);
                }

                // Create Lang
                GiftLangManager giftLangManager = new GiftLangManager();
                create_result = giftLangManager.Create(giftObject.lang_list, ref sql_remark);

                // Create Photo
                GiftPhotoManager giftPhotoManager = new GiftPhotoManager();
                foreach (var photo in giftObject.photo_list)
                {
                    create_result = giftPhotoManager.Create(
                         SessionManager.Current.obj_id,
                          giftObject.gift_id,
                          photo.file_name,
                          photo.file_extension,
                          photo.name,
                          photo.caption,
                          photo.display_order, // display_order
                          photo.status, // status

                          ref sql_remark
                        );
                }

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

            return sql_result;
        }

        // this function will depreciate in future
        public sp_GetGiftDetailByResult GetGiftDetailBy(int user_id, int gift_id, ref bool sql_result)
        {
            int? get_sql_result = 0;
            var sql_remark = "";

            var result = db.sp_GetGiftDetailBy(SessionManager.Current.obj_id, gift_id, ref get_sql_result, ref sql_remark);

            // convert from IEnumerable List to Generic List
            var result_list = result.ToList();

            sql_result = false;
            if (result_list.Count() > 0)
                sql_result = true;

            return result_list.FirstOrDefault() ?? new sp_GetGiftDetailByResult();
        }

        public GiftObject GetDetail(int gift_id)
        {
            var query = (from g in db.gifts
                         join gl in db.gift_langs on g.gift_id equals gl.gift_id
                         join li in db.listing_items on g.status equals li.value
                         join l in db.listings on li.list_id equals l.list_id

                         where (
                            gl.lang_id == (int)CommonConstant.LangCode.en
                         && l.code == "Status"
                         && g.gift_id == gift_id

                         )
                         select new GiftObject
                         {
                             gift_id = g.gift_id,
                             gift_no = g.gift_no,
                             point = g.point,
                             alert_level = g.alert_level,
                             cost = g.cost,
                             discount = g.discount,
                             discount_point = g.discount_point,
                             discount_active_date = g.discount_active_date,
                             discount_expiry_date = g.discount_expiry_date,
                             hot_item = g.hot_item,
                             hot_item_active_date = g.hot_item_active_date,
                             hot_item_expiry_date = g.hot_item_expiry_date,
                             hot_item_display_order = g.hot_item_display_order,
                             display_public = g.display_public,
                             display_active_date = g.display_active_date,
                             display_expiry_date = g.display_expiry_date,
                             redeem_active_date = g.redeem_active_date,
                             redeem_expiry_date = g.redeem_expiry_date,
                             status = g.status,
                             available_stock = g.available_stock,
                             name = gl.name,
                             status_name = l.name,
                             photo_list = db.gift_photos.Where(s => s.gift_id.Equals(g.gift_id)).
                                   Select(s => new PhotoObject
                                   {
                                       photo_id = s.gift_photo_id,
                                       file_name = s.file_name,
                                       file_extension = s.file_extension,
                                       display_order = s.display_order,
                                       name = s.name,
                                       caption = s.caption,
                                       status = s.status
                                   }).ToList()
                         }).OrderBy(x => x.gift_no);

            return query.FirstOrDefault();
        }

        public bool Update(
            GiftObject giftObject,
            ChangedField[] changedFields,

            ref string sql_remark
        )
        {
            int? get_sql_result = 0;

            var result = db.sp_UpdateGift(
               _accessObject.id, 
               _accessObject.type, 

                giftObject.gift_id,
                giftObject.gift_no,

                giftObject.point,
                giftObject.alert_level,
                giftObject.cost,

                giftObject.discount,
                giftObject.discount_point,
                giftObject.discount_active_date,
                giftObject.discount_expiry_date,

                giftObject.hot_item,
                giftObject.hot_item_active_date,
                giftObject.hot_item_expiry_date,
                giftObject.hot_item_display_order,

                giftObject.display_public,
                giftObject.display_active_date,
                giftObject.display_expiry_date,
                giftObject.redeem_active_date,
                giftObject.redeem_expiry_date,

                giftObject.status,
                giftObject.available_stock,

                ref get_sql_result, ref sql_remark);

            var sql_result = get_sql_result == 1 ? true : false;
            if (sql_result)
            {
                // [START] Update object into object table
                //SystemObjectManager systemObjectManager = new SystemObjectManager();

                //var object_name = "";
                //var object_power_search_list = new List<string>();
                //if (!String.IsNullOrWhiteSpace(giftObject.gift_no)) object_power_search_list.Add(giftObject.gift_no);

                //foreach (var theLang in giftObject.lang_list)
                //{
                //    if (theLang.lang_id == (int)CommonConstant.LangCode.en) // ENG name as Object Name
                //        object_name = theLang.name;

                //    if (!String.IsNullOrWhiteSpace(theLang.name)) object_power_search_list.Add(theLang.name);
                //    if (!String.IsNullOrWhiteSpace(theLang.description)) object_power_search_list.Add(theLang.description);
                //}

                //var power_search = String.Join(" ", object_power_search_list.ToArray());

                //sql_result = systemObjectManager.Update(
                //   SessionManager.Current.obj_id,

                //   giftObject.gift_id,
                //   object_name,
                //   giftObject.status,
                //   power_search,
                //   ref sql_remark
                // );
                // [END] Update Object

                //Update Category
                var categoryLinkManager = new GiftCategoryLinkManager();
                categoryLinkManager.DeleteOwnedList(giftObject.gift_id);
                foreach (var c in giftObject.category_list)
                {
                    var cat_result = categoryLinkManager.Create(giftObject.gift_id, c.category_id);
                }

                // Update Lang
                GiftLangManager giftLangManager = new GiftLangManager();
                foreach (var theLang in giftObject.lang_list)
                {
                    giftLangManager.Update(SessionManager.Current.obj_id, theLang.gift_id, theLang.lang_id, theLang.name, theLang.description, theLang.status, ref sql_remark);
                }

                // Update Location
                GiftLocationManager giftLocationManager = new GiftLocationManager();
                sql_result = giftLocationManager.Delete(SessionManager.Current.obj_id, giftObject.gift_id, ref sql_remark);
                foreach (var theLocation in giftObject.location_list)
                {
                    sql_result = giftLocationManager.Create(SessionManager.Current.obj_id, giftObject.gift_id, theLocation.location_id, theLocation.status, ref sql_remark);
                }

                // Update Member Privilege
                GiftMemberPrivilegeManager giftMemberPrivilegeManager = new GiftMemberPrivilegeManager();
                sql_result = giftMemberPrivilegeManager.DeleteOwnedList(giftObject.gift_id, ref sql_remark); //remove

                foreach (var thePrivilege in giftObject.member_privilege_list)
                {
                    System.Diagnostics.Debug.WriteLine(thePrivilege.member_level_id, " thePrivilege.member_level_id");
                    sql_result = giftMemberPrivilegeManager.Create(giftObject.gift_id, thePrivilege.member_level_id, 1, CommonConstant.Status.active, ref sql_remark);
                }

                // Update Gift Photo
                GiftPhotoManager giftPhotoManager = new GiftPhotoManager();
                //var current_photo_list = giftPhotoManager.GetGiftPhotoListBy(SessionManager.Current.user_id, giftObject.gift_id, ref sql_result).ToList();

                foreach (var photo in giftObject.photo_list)
                {
                    //var find_object = current_photo_list.Where(x => x.gift_photo_id == photo.photo_id).FirstOrDefault();

                    if (photo.photo_id == 0)
                    {
                        sql_result = giftPhotoManager.Create(
                             SessionManager.Current.obj_id,
                              giftObject.gift_id,
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

                        sql_result = giftPhotoManager.Update(

                            photo.photo_id,
                            giftObject.gift_id,
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
                //    target_obj_id = giftObject.gift_id,
                //    target_obj_type_id = null,
                //    target_obj_name = null,
                //    action_detail = null
                //};
                //logManager.Create(theLog);
            }
            return sql_result;
        }

        public bool Update_directCore(
            GiftObject giftObject,
            ref string sql_remark
       )
        {
            int? get_sql_result = 0;

            var result = db.sp_UpdateGift(
                               _accessObject.id,
                _accessObject.type, 
                giftObject.gift_id,
                giftObject.gift_no,

                giftObject.point,
                giftObject.alert_level,
                giftObject.cost,

                giftObject.discount,
                giftObject.discount_point,
                giftObject.discount_active_date,
                giftObject.discount_expiry_date,

                giftObject.hot_item,
                giftObject.hot_item_active_date,
                giftObject.hot_item_expiry_date,
                giftObject.hot_item_display_order,

                giftObject.display_public,
                giftObject.display_active_date,
                giftObject.display_expiry_date,
                giftObject.redeem_active_date,
                giftObject.redeem_expiry_date,

                giftObject.status,
                giftObject.available_stock,

                ref get_sql_result, ref sql_remark);

            var sql_result = get_sql_result == 1 ? true : false;

            return sql_result;
        }

        public CommonConstant.SystemCode UpdateHotItemDisplayOrder(GiftObject targetObject)
        {
            var linkList = GetGiftLists_isHotItem();

            var updateObject = new OrderObject<GiftObject>
            {
                id = targetObject.gift_id,
                display_order = targetObject.hot_item_display_order,
                data_object = targetObject
            };

            var orderObjectList = new List<OrderObject<GiftObject>>();

            foreach (var x in linkList)
            {
                var orderObject = new OrderObject<GiftObject>
                {
                    id = x.gift_id,
                    display_order = x.hot_item_display_order,
                    data_object = x,
                };

                orderObjectList.Add(orderObject);
            }

            var updatedList = OrderManager.Reorder(orderObjectList, updateObject);

            var sql_remark = "";
            foreach (var x in updatedList)
            {
                UpdateHotItemDisplayOrder(x.data_object.gift_id, x.display_order);
            }

            //System.Diagnostics.Debug.WriteLine(updatedList.ToJson());
            return CommonConstant.SystemCode.normal;
        }

        public CommonConstant.SystemCode UpdateHotItemDisplayOrder(int gift_id, int hotItem_displayOrder)
        {
            var sql_remark = "";
            int? get_sql_result = 0;

            var sql_run = db.sp_UpdateGiftHotItemDisplayOrder(
                _accessObject.id, 
                _accessObject.type, 
                gift_id,
                hotItem_displayOrder,
                ref get_sql_result
                );

            return CommonConstant.SystemCode.normal;
        }


        public bool CheckDuplicateGiftNo(string gift_no)
        {
            int? get_duplicate = 0;
            int? get_sql_result = 0;
            var sql_remark = "";

            var result = db.sp_CheckDuplicate_giftNo(
                SessionManager.Current.obj_id,

                gift_no,
                ref get_duplicate,
                ref get_sql_result, ref sql_remark);

            return (get_duplicate == 1 ? true : false);
        }


        // Delete object
        // With permission check and log
        // LINQ to Store Procedures
        public CommonConstant.SystemCode SoftDelete(int gift_id, ref string sql_remark)
        {
            var systemCode = CommonConstant.SystemCode.undefine;

            if (_privilege.delete_status == 1)
            {
                int? sql_result = 0;

                var obj = GetDetail(gift_id);
                var delete_result = false;

                if (obj.gift_id > 0)
                {
                    db.sp_SoftDeleteByModule(_accessObject.id, _accessObject.type, CommonConstant.Module.gift, obj.gift_id, ref sql_result, ref sql_remark);
                    delete_result = (int)sql_result.Value == 1 ? true : false;


                }

                if (delete_result)
                {
                    systemCode = CommonConstant.SystemCode.normal;

                    // take log
                    _logManager.LogObject(CommonConstant.ActionType.delete, CommonConstant.ObjectType.gift, obj.gift_id, obj.name);
                }
                else
                    systemCode = CommonConstant.SystemCode.record_invalid;
            }
            else
                systemCode = CommonConstant.SystemCode.no_permission;

            return systemCode;
        }
    
    }
}

