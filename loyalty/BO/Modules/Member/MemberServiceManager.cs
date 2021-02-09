using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.Objects;
using Palmary.Loyalty.Common;
using Palmary.Loyalty.BO.Database;
using Palmary.Loyalty.BO.DataTransferObjects.DB;
using Palmary.Loyalty.BO.DataTransferObjects.Security;
using Palmary.Loyalty.BO.Modules.Administration.Security;
using Palmary.Loyalty.BO.Modules.Administration.SystemControl;
using Palmary.Loyalty.BO.Modules.Transaction;
using Palmary.Loyalty.BO.DataTransferObjects.Form;
using Palmary.Loyalty.BO.DataTransferObjects.Member;
using Palmary.Loyalty.BO.DataTransferObjects.Role;

namespace Palmary.Loyalty.BO.Modules.Member
{
    public class MemberServiceManager
    {
        private static LoyaltyDBDataContext db = new LoyaltyDBDataContext();
        private AccessManager _accessManager;
        private string _module = CommonConstant.Module.memberService;
        private RolePrivilegeObject _privilege;
        private static LogManager _logManager;
        private AccessObject _accessObject;

        public MemberServiceManager()
        {  
            // access object
            _accessManager = new AccessManager();
            _accessObject = _accessManager.GetAccessObjectBySession();

            // privilege and log
            _privilege = _accessManager.AccessModule(_module);
            _logManager = new LogManager();
        }

        public CommonConstant.SystemCode Create(
            MemberServiceObject dataObject,
            ref int? new_service_id
        )
        {
            int? sql_result = 0;
            var system_code = CommonConstant.SystemCode.undefine;

            if (_privilege.insert_status == 1)
            {
                var result = db.sp_CreateMemberService(
                    _accessObject.id,
                _accessObject.type, 


                    dataObject.member_id,
                    dataObject.service_no,
                    dataObject.plan_id,
                    dataObject.point,
                    dataObject.start_date,
                    dataObject.end_date,
                    dataObject.status,
                    ref new_service_id,
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

        public CommonConstant.SystemCode Update(MemberServiceObject dataObject)
        {
            int? sql_result = 0;
            var system_code = CommonConstant.SystemCode.undefine;

            if (_privilege.update_status == 1)
            {
                var result = db.sp_UpdateMemberService(
                    _accessObject.id,
                _accessObject.type, 

                    dataObject.member_service_id,
                    dataObject.member_id,
                    dataObject.service_no,
                    dataObject.plan_id,
                    dataObject.point,
                    dataObject.start_date,
                    dataObject.end_date,
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

        // Get Detail
        public MemberServiceObject GetDetail(string member_service_no, ref CommonConstant.SystemCode systemCode)
        {
            MemberServiceObject resultObj;

            if (_privilege.read_status == 1)
            {
                var query = (from ms in db.member_services
                             where (
                                ms.record_status != (int)CommonConstant.RecordStatus.deleted
                            )
                            select new MemberServiceObject
                            {
                                member_service_id = ms.member_service_id,
                                member_id = ms.member_id,
                                service_no = ms.service_no,
                                plan_id = ms.plan_id,
                                point = ms.point,
                                start_date = ms.start_date,
                                end_date = ms.end_date,
                                status = ms.status,
                                crt_date = ms.crt_date,
                                crt_by_type = ms.crt_by_type,
                                crt_by = ms.crt_by,
                                upd_date = ms.upd_date,
                                upd_by_type = ms.upd_by_type,
                                upd_by = ms.upd_by,
                                record_status = ms.record_status
                            });

                resultObj = query.FirstOrDefault();
                systemCode = CommonConstant.SystemCode.normal;
            }
            else
            {
                resultObj = new MemberServiceObject();
                systemCode = CommonConstant.SystemCode.no_permission;
            }

            return resultObj;
        }
    }
}