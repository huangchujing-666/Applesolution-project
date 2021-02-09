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
    public class MemberCategoryLangManager
    {
        private static LoyaltyDBDataContext db = new LoyaltyDBDataContext();
        private AccessManager _accessManager;
        private string _module = CommonConstant.Module.memberCategory;
        private RolePrivilegeObject _privilege;
        private static LogManager _logManager;
        private AccessObject _accessObject;

        public MemberCategoryLangManager()
        {
            // access object
            _accessManager = new AccessManager();
            _accessObject = _accessManager.GetAccessObjectBySession();

            // privilege and log
            _privilege = _accessManager.AccessModule(_module);
            _logManager = new LogManager();
        }

        public CommonConstant.SystemCode Create(MemberCategoryLangObject dataObject)
        {
            int? sql_result = 0;
            var system_code = CommonConstant.SystemCode.undefine;

            if (_privilege.insert_status == 1)
            {
                var result = db.sp_CreateMemberCategoryLang(
                    _accessObject.id,
                _accessObject.type, 

                    dataObject.category_id,
                    dataObject.lang_id,
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

        public List<MemberCategoryLangObject> GetMemberCategoryLang_ownedList(int memberCategory_id, ref CommonConstant.SystemCode system_code)
        {
            List<MemberCategoryLangObject> resultList;

            if (_privilege.read_status == 1)
            {
                var query = from mcl in db.member_category_langs
                            where (
                                mcl.category_id == memberCategory_id
                            )
                            select new MemberCategoryLangObject
                            {
                                category_lang_id = mcl.category_lang_id,
                                category_id = mcl.category_id,
                                lang_id = mcl.lang_id,
                                name = mcl.name,
                                description = mcl.description,
                                status = mcl.status,
                                crt_date = mcl.crt_date,
                                crt_by_type = mcl.crt_by_type,
                                crt_by = mcl.crt_by,
                                upd_date = mcl.upd_date,
                                upd_by_type = mcl.upd_by_type,
                                upd_by = mcl.upd_by,
                                record_status = mcl.record_status
                            };

                resultList = query.ToList();
                system_code = CommonConstant.SystemCode.normal;
            }
            else
            {
                resultList = new List<MemberCategoryLangObject>();
                system_code = CommonConstant.SystemCode.no_permission;
            }

            return resultList;
        }

        public CommonConstant.SystemCode Update(MemberCategoryLangObject dataObject)
        {
            int? sql_result = 0;
            var system_code = CommonConstant.SystemCode.undefine;

            if (_privilege.update_status == 1)
            {
                var result = db.sp_UpdateMemberCategoryLang(
         _accessObject.id,
                _accessObject.type, 

                    dataObject.category_id,
                    dataObject.lang_id,
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


    }
}
