using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.Objects;
using Palmary.Loyalty.Common;
using Palmary.Loyalty.BO.Database;
using Palmary.Loyalty.BO.DataTransferObjects.Member;
using Palmary.Loyalty.BO.DataTransferObjects.Role;
using Palmary.Loyalty.BO.DataTransferObjects.Security;
using Palmary.Loyalty.BO.Modules.Administration.Security;

namespace Palmary.Loyalty.BO.Modules.Member
{
    public class MemberCategoryManager
    {
        private static LoyaltyDBDataContext db = new LoyaltyDBDataContext();
        private AccessManager _accessManager;
        private string _module = CommonConstant.Module.memberCategory;
        private RolePrivilegeObject _privilege;
        private static LogManager _logManager;
        private AccessObject _accessObject;

        public MemberCategoryManager()
        {
            // access object
            _accessManager = new AccessManager();
            _accessObject = _accessManager.GetAccessObjectBySession();

            // privilege and log
            _privilege = _accessManager.AccessModule(_module);
            _logManager = new LogManager();
        }

        public CommonConstant.SystemCode Create(
            MemberCategoryObject dataObject
        )
        {
            int? get_sql_result = 0;
            int? new_cat_id = 0;
            var system_code = CommonConstant.SystemCode.undefine;

            if (_privilege.insert_status == 1)
            {
                var result = db.sp_CreateMemberCategory(
                  _accessObject.id,
                _accessObject.type, 


                    dataObject.parent_id,
                    dataObject.leaf,
                    dataObject.display_order,
                    dataObject.status,

                    ref new_cat_id,
                    ref get_sql_result
                    );

                system_code = (CommonConstant.SystemCode)get_sql_result.Value;

                if (system_code == CommonConstant.SystemCode.normal)
                {
                    var memberCategoryLangMember = new MemberCategoryLangManager();

                    //Create Lang
                    foreach (var langObject in dataObject.lang_list)
                    {
                        langObject.category_id = new_cat_id.Value;
                        system_code = memberCategoryLangMember.Create(langObject);
                    }
                }
              
            }else
            {
                system_code = CommonConstant.SystemCode.no_permission;
            }

            return system_code;
        }

        public List<MemberCategoryObject> GetMemberCategory_list(ref CommonConstant.SystemCode systemCode)
        {
            List<MemberCategoryObject> resultList;

            if (_privilege.read_status == 1)
            {
                 var query = (from mc in db.member_categories
                              join mcl in db.member_category_langs on mc.category_id equals mcl.category_id
                              where (mcl.lang_id == (int)CommonConstant.LangCode.en
                                    && mcl.record_status != (int)CommonConstant.RecordStatus.deleted)
                             select new MemberCategoryObject
                             {
                                 category_id = mc.category_id,
                                 parent_id = mc.parent_id,
                                 leaf = mc.leaf,
                                 display_order = mc.display_order,
                                 status = mc.status,
                                 crt_date = mc.crt_date,
                                 crt_by_type = mc.crt_by_type,
                                 crt_by = mc.crt_by,
                                 upd_date = mc.upd_date,
                                 upd_by_type = mc.upd_by_type,
                                 upd_by = mc.upd_by,
                                 record_status = mc.record_status,
                                 name = mcl.name
                             });

                 resultList = query.ToList();
                 systemCode = CommonConstant.SystemCode.normal;
            }
            else
            {
                resultList = new List<MemberCategoryObject>();
                systemCode = CommonConstant.SystemCode.no_permission;
            }

            return resultList;
        }

        public MemberCategoryObject GetMemberCategoryDetail_withLang(int memberCategory_id, ref CommonConstant.SystemCode systemCode)
        {
            MemberCategoryObject member_category;
            
            if (_privilege.read_status == 1)
            {
                var query = (from mc in db.member_categories
                             where (mc.category_id == memberCategory_id)
                             select new MemberCategoryObject
                             {
                                 category_id = mc.category_id,
                                 parent_id = mc.parent_id,
                                 leaf = mc.leaf,
                                 display_order = mc.display_order,
                                 status = mc.status,
                                 crt_date = mc.crt_date,
                                 crt_by_type = mc.crt_by_type,
                                 crt_by = mc.crt_by,
                                 upd_date = mc.upd_date,
                                 upd_by_type = mc.upd_by_type,
                                 upd_by = mc.upd_by,
                                 record_status = mc.record_status
                             });

                member_category = query.FirstOrDefault() ?? new MemberCategoryObject();

                if (member_category.category_id > 0)
                {
                    // load lang list
                    var memberCategoryLangManager = new MemberCategoryLangManager();
                    var lang_list = memberCategoryLangManager.GetMemberCategoryLang_ownedList(memberCategory_id, ref systemCode);
                    member_category.lang_list = lang_list;
                }
            }
            else
            {
                member_category = new MemberCategoryObject();
                systemCode = CommonConstant.SystemCode.no_permission;
            }

            return member_category;
        }

        // Manager Object (DAO) UPDATE
        public CommonConstant.SystemCode Update(MemberCategoryObject dataObject)
        {
            int? sql_result = 0;
            var system_code = CommonConstant.SystemCode.undefine;

            if (_privilege.update_status == 1)
            {
                var result = db.sp_UpdateMemberCategory(
                    _accessObject.id,
                _accessObject.type, 

                    dataObject.category_id,
                    dataObject.parent_id,
                    dataObject.leaf,
                    dataObject.display_order,
                    dataObject.status,

                    ref sql_result);

                system_code = (CommonConstant.SystemCode)sql_result.Value;

                if (system_code == CommonConstant.SystemCode.normal)
                {
                    var memberCategoryLangManager = new MemberCategoryLangManager();
                    // Update Lang
                    foreach (var langObject in dataObject.lang_list)
                    {
                        system_code = memberCategoryLangManager.Update(langObject);
                    }
                }
            }
            else
            {
                system_code = CommonConstant.SystemCode.no_permission;
            }

            return system_code;
        }  
    }
}