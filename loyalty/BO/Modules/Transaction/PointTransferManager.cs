using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.Objects;
using Palmary.Loyalty.Common;
using Palmary.Loyalty.BO.Database;
using Palmary.Loyalty.BO.DataTransferObjects.Product;
using Palmary.Loyalty.BO.DataTransferObjects.Role;
using Palmary.Loyalty.BO.DataTransferObjects.Security;
using Palmary.Loyalty.BO.Modules.Administration.Security;
using Palmary.Loyalty.BO.Modules.Transaction;
using Palmary.Loyalty.BO.Modules.Administration.SystemControl;
using Palmary.Loyalty.BO.DataTransferObjects.Member;


namespace Palmary.Loyalty.BO.Modules.Transaction
{
    public class PointTransferManager
    {
        private static LoyaltyDBDataContext db = new LoyaltyDBDataContext();

        public CommonConstant.SystemCode Transfer(int location_id, int member_id, double point_change, string remark, string to_member_no)
        {
            var transactionManager = new TransactionManager();
            var result = false;

            if (point_change > 0)
            {
                MemberObject resultObject;

                // TODO:: Add Row Data Format Validation

                var query = (from m in db.member_profiles
                             join so in db.system_objects on m.member_id equals so.object_id

                             where (
                                m.record_status != (int)CommonConstant.RecordStatus.deleted
                                && m.member_no == to_member_no
                            )
                             select new MemberObject
                             {
                                 member_id = m.member_id,
                                 member_no = m.member_no,
                                 password = m.password,
                                 email = m.email,
                                 fbid = m.fbid,
                                 fbemail = m.fbemail,
                                 mobile_no = m.mobile_no,
                                 salutation = m.salutation,
                                 firstname = m.firstname,
                                 middlename = m.middlename,
                                 lastname = m.lastname,
                                 fullname = m.fullname, 
                                 birth_year = m.birth_year,
                                 birth_month = m.birth_month,
                                 birth_day = m.birth_day,
                                 gender = m.gender,
                                 hkid = m.hkid,
                                 address1 = m.address1,
                                 address2 = m.address2,
                                 address3 = m.address3,
                                 district = m.district,
                                 region = m.region,
                                 reg_source = m.reg_source,
                                 referrer = m.referrer,
                                 reg_status = m.reg_status,
                                 reg_ip = m.reg_ip,
                                 activate_key = m.activate_key,
                                 hash_key = m.hash_key,
                                 session = m.session,
                                 status = m.status,
                                 opt_in = m.opt_in,
                                 member_level_id = m.member_level_id,
                                 member_category_id = m.member_category_id,
                                 available_point = m.available_point,
                                 crt_date = m.crt_date,
                                 upd_date = m.upd_date,
                                 crt_by_type = m.crt_by_type,
                                 crt_by = m.crt_by,
                                 upd_by_type = m.upd_by_type,
                                 upd_by = m.upd_by,
                                 record_status = m.record_status,

                             });

                resultObject = query.FirstOrDefault();

                if (resultObject != null)
                {
                    int? new_use_transaction_id = 0;
                    
                    result = transactionManager.UsePoint(location_id, member_id, -1 * point_change, (int)CommonConstant.TransactionType.point_transfer, SessionManager.Current.obj_id, remark, ref new_use_transaction_id);

                    if (result)
                    {
                        int? new_add_transaction_id = 0;
                        var point_status = (int)CommonConstant.PointStauts.realized;
                        var systemConfigManager = new SystemConfigManager();

                        var bo_remark = "";
                        var point_expiry_month = int.Parse(systemConfigManager.GetSystemConfigValue("point_expiry_month", ref bo_remark));
                        var point_expiry_date = DateTime.Now.AddMonths(point_expiry_month);
                        var source_id = new_use_transaction_id.Value;

                        result = transactionManager.AddPoint(location_id, resultObject.member_id, point_change, point_status, point_expiry_date, (int)CommonConstant.TransactionType.point_transfer, source_id, remark, ref new_add_transaction_id);
                    }
                }
            }

            if (result)
                return CommonConstant.SystemCode.normal;
            else
                return CommonConstant.SystemCode.record_invalid;
        }
    }
}
