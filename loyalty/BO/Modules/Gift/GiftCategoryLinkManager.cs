using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Palmary.Loyalty.BO.Database;
using Palmary.Loyalty.BO.DataTransferObjects.Role;
using Palmary.Loyalty.BO.DataTransferObjects.Security;
using Palmary.Loyalty.BO.DataTransferObjects.Gift;
using Palmary.Loyalty.BO.Modules.Administration.Security;
using Palmary.Loyalty.BO.DataTransferObjects.Utility;
using Palmary.Loyalty.BO.Modules.Utility;
using Palmary.Loyalty.Common;

namespace Palmary.Loyalty.BO.Modules.Gift
{
    public class GiftCategoryLinkManager
    {
        private static LoyaltyDBDataContext db = new LoyaltyDBDataContext();
        private string _module = CommonConstant.Module.giftCategory;
        private AccessManager _accessManager;
        private RolePrivilegeObject _privilege;
        private static LogManager _logManager;
        private AccessObject _accessObject;


        public GiftCategoryLinkManager()
        {
            // access object
            _accessManager = new AccessManager();
            _accessObject = _accessManager.GetAccessObjectBySession();

            // privilege and log
            _privilege = _accessManager.AccessModule(_module);
            _logManager = new LogManager();
        }

        public CommonConstant.SystemCode Create(
          int gift_id,
          int category_id
        )
        {
            int? sql_result = 0;
            var system_code = CommonConstant.SystemCode.undefine;

            if (_privilege.insert_status == 1)
            {
                var result = db.sp_CreateGiftCategoryLink(
                    _accessObject.id, 
                    _accessObject.type, 
                    gift_id,
                    category_id,
                    0, // display_order
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

        public List<GiftCategoryObject> GetGiftCategory_ownedList(int gift_id)
        {
            var query = (from l in db.gift_category_links
                         join gc_l in db.gift_category_langs on l.category_id equals gc_l.category_id
                         where (gc_l.lang_id == (int)CommonConstant.LangCode.en && l.gift_id == gift_id)
                         select new GiftCategoryObject
                         {
                             category_id = l.category_id,
                             name = gc_l.name
                         });

            return query.ToList();
        }

        public List<GiftCategoryLinkObject> GetLinkListByCategory(int category_id)
        {
            var query = (from l in db.gift_category_links
                         where (l.category_id == category_id)
                         select new GiftCategoryLinkObject
                         {
                             gift_id = l.gift_id,
                             category_id = l.category_id,
                             display_order = l.display_order
                         }).OrderBy(x => x.display_order);

            return query.ToList();
        }

        public GiftCategoryLinkObject GetDetail(int gift_id, int gift_category_id)
        {
            var query = (from link in db.gift_category_links
                         join gc_l in db.gift_category_langs on link.category_id equals gc_l.category_id
                         join g_l in db.gift_langs on link.gift_id equals g_l.gift_id
                         where (
                                 gc_l.lang_id == (int)CommonConstant.LangCode.en
                                 && g_l.lang_id == (int)CommonConstant.LangCode.en
                                 && link.gift_id == gift_id
                                 && link.category_id == gift_category_id
                                )
                         select new GiftCategoryLinkObject
                         {
                             category_id = link.category_id,
                             gift_id = link.gift_id,
                             category_name = gc_l.name,
                             gift_name = g_l.name,
                             display_order = link.display_order
                         });

            var item = query.FirstOrDefault();

            return item;
        }

        public CommonConstant.SystemCode Update(GiftCategoryLinkObject dataObject)
        {
            int? sql_result = 0;
            var system_code = CommonConstant.SystemCode.undefine;

            if (_privilege.update_status == 1)
            {
                var result = db.sp_UpdateGiftCategoryLink(
                     _accessObject.id,
                _accessObject.type, 


                    dataObject.gift_id,
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

        public CommonConstant.SystemCode UpdateDisplayOrder(GiftCategoryLinkObject targetObject)
        {
            var linkList = GetLinkListByCategory(targetObject.category_id);

            var updateObject = new OrderObject<GiftCategoryLinkObject>
            {
                id = targetObject.gift_id,
                display_order = targetObject.display_order,
                data_object = targetObject
            };

            var orderObjectList = new List<OrderObject<GiftCategoryLinkObject>>();

            foreach (var x in linkList)
            {
                var orderObject = new OrderObject<GiftCategoryLinkObject>
                {
                    id = x.gift_id,
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

        public CommonConstant.SystemCode DeleteOwnedList(int gift_id)
        {
            int? sql_result = 0;
            var system_code = CommonConstant.SystemCode.undefine;

            if (_privilege.delete_status == 1)
            {
                var result = db.sp_DeleteGiftCategoryLink(
                    SessionManager.Current.obj_id,
                    gift_id,
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
