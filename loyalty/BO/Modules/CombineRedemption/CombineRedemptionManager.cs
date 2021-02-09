using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Palmary.Loyalty.BO.Database;
using Palmary.Loyalty.BO.DataTransferObjects.Security;
using System.Data.Objects;
using Palmary.Loyalty.BO.Modules.Administration.Security;
using Palmary.Loyalty.Common;
using Palmary.Loyalty.BO.DataTransferObjects.Role;
using Palmary.Loyalty.BO.DataTransferObjects.CombineRedemption;
using Palmary.Loyalty.BO.Modules.Member;
using Palmary.Loyalty.BO.Modules.Transaction;

namespace Palmary.Loyalty.BO.Modules.CombineRedemption
{
    public class CombineRedemptionManager
    {
        private static LoyaltyDBDataContext db = new LoyaltyDBDataContext();
        private AccessManager _accessManager;
        private string _module = CommonConstant.Module.combineRedemption;

        private RolePrivilegeObject _privilege;
        private static LogManager _logManager;
        private AccessObject _accessObject;

          // For backend, using BO Session to access
        public CombineRedemptionManager()
        {
            // access object from session
            _accessManager = new AccessManager();
            _accessObject = _accessManager.GetAccessObjectBySession();

            // privilege and log
            _privilege = _accessManager.AccessModule(_module);
            _logManager = new LogManager();
        }

        // For Webservices or other not using BO Session to access
        public CombineRedemptionManager(AccessObject accessObject)
        {
            // access object from passing in
            _accessManager = new AccessManager();
            _accessObject = accessObject;

            // privilege and log
            _privilege = _accessManager.AccessModule(_module, _accessObject.type, _accessObject.id);
            _logManager = new LogManager(_accessObject);
        }

        public CommonConstant.SystemCode Create(
          CombineRedemptionObject obj,

          ref int new_obj_id)
        {
            // LINQ to Store Procedures
            var systemCode = CommonConstant.SystemCode.undefine;
            int? sql_result = 0;
            int? get_new_obj_id = 0;

            if (_privilege.insert_status == 1)
            {
                var result = db.sp_CreateCombineRedemption(
                    _accessObject.id,
                        _accessObject.type,
                    obj.member_id,
                    obj.coupon_id,
                    obj.position,
                    obj.no_of_ppl,
                    obj.join_combine_id,
                    obj.notified_host,
                    obj.status,

                    ref get_new_obj_id,
                    ref sql_result
                    );

                systemCode = (CommonConstant.SystemCode)sql_result.Value;
                new_obj_id = get_new_obj_id.Value;
            }
            else
            {
                systemCode = CommonConstant.SystemCode.no_permission;
            }

            return systemCode;
        }

        public CommonConstant.SystemCode Update(CombineRedemptionObject obj)
        {
            int? sql_result = 0;
            var system_code = CommonConstant.SystemCode.undefine;

            if (_privilege.update_status == 1)
            {

                var result = db.sp_UpdateCombineRedemption(
                    _accessObject.id,
                    _accessObject.type,

                    obj.combine_id,
                    obj.member_id,
                    obj.coupon_id,
                    obj.position,
                    obj.no_of_ppl,
                    obj.join_combine_id,
                    obj.notified_host,
                    obj.status,
                  
                    ref sql_result);

                system_code = (CommonConstant.SystemCode)sql_result.Value;
            }
            else
            {
                system_code = CommonConstant.SystemCode.no_permission;
            }

            return system_code;
        }

        public CombineRedemptionObject GetDetail(int combine_id, bool root_access, ref CommonConstant.SystemCode systemCode)
        {
            CombineRedemptionObject resultObject;

            if (_privilege.read_status == 1 || root_access)
            {
                var query = (from c in db.combine_redemptions
                             
                             where (
                                c.record_status != (int)CommonConstant.RecordStatus.deleted
                                && c.combine_id == combine_id
                            )
                             select new CombineRedemptionObject
                             {
                                 combine_id = c.combine_id,
                                 member_id = c.member_id,
                                 coupon_id = c.coupon_id,
                                 position = c.position,
                                 no_of_ppl = c.no_of_ppl,
                                 join_combine_id = c.join_combine_id,
                                 notified_host = c.notified_host,
                                 status = c.status,
                                 crt_date = c.crt_date,
                                 crt_by_type = c.crt_by_type,
                                 crt_by = c.crt_by,
                                 upd_date = c.upd_date,
                                 upd_by_type = c.upd_by_type,
                                 upd_by = c.upd_by,
                                 record_status = c.record_status

                                 // additional info
                             });

                if (query.Count() > 0)
                {  
                    systemCode = CommonConstant.SystemCode.normal;
                    resultObject = query.FirstOrDefault();
                }
                else
                {
                    systemCode = CommonConstant.SystemCode.data_invalid;
                    resultObject = new CombineRedemptionObject();
                }
            }
            else
            {
                resultObject = new CombineRedemptionObject();
                systemCode = CommonConstant.SystemCode.no_permission;
            }

            return resultObject;
        }

        // get joiner list but not including the host
        public List<CombineRedemptionObject> GetActiveJoinedList(int combine_id, bool root_access, ref CommonConstant.SystemCode systemCode)
        {
            var resultList = new List<CombineRedemptionObject>();

            if (_privilege.read_status == 1 || root_access)
            {
                var query = (from c in db.combine_redemptions

                             where (
                                c.record_status != (int)CommonConstant.RecordStatus.deleted
                                && c.join_combine_id == combine_id
                                && c.status == (int)CommonConstant.CombineRedemptionStatus.connecting
                            )
                             select new CombineRedemptionObject
                             {
                                 combine_id = c.combine_id,
                                 member_id = c.member_id,
                                 coupon_id = c.coupon_id,
                                 position = c.position,
                                 no_of_ppl = c.no_of_ppl,
                                 join_combine_id = c.join_combine_id,
                                 notified_host = c.notified_host,
                                 status = c.status,
                                 crt_date = c.crt_date,
                                 crt_by_type = c.crt_by_type,
                                 crt_by = c.crt_by,
                                 upd_date = c.upd_date,
                                 upd_by_type = c.upd_by_type,
                                 upd_by = c.upd_by,
                                 record_status = c.record_status
                             });

                resultList = query.ToList(); 

                systemCode = CommonConstant.SystemCode.normal;
            }
            else
            {
                resultList = new List<CombineRedemptionObject>();
                systemCode = CommonConstant.SystemCode.no_permission;
            }

            return resultList;
        }

        public CommonConstant.SystemCode JoinCombineRedemption(int member_id, int combine_id, ref int new_combine_id, ref int coupon_id, ref int noOfPpl, ref int nextPosition, ref int pointRequire)
        {
            var systemCode = CommonConstant.SystemCode.undefine;
            var combineObj = GetDetail(combine_id, true, ref systemCode);

            if (combineObj.combine_id > 0 && combineObj.status == (int)CommonConstant.CombineRedemptionStatus.connecting)
            {
                coupon_id = combineObj.coupon_id;
                noOfPpl = combineObj.no_of_ppl;

                // temp for demo
                var pointOfCoupon = coupon_id - 1;
                pointRequire = pointOfCoupon / noOfPpl;
                var memberManager = new MemberManager(_accessObject);

                var memberObj = memberManager.GetDetail(member_id, false, ref systemCode);
                if (memberObj.available_point < pointRequire)
                {
                    systemCode = CommonConstant.SystemCode.err_redeem_notEnoughPoint;
                }
                else
                {

                    var joinedList = GetActiveJoinedList(combine_id, true, ref systemCode);

                    var noOfjoined = joinedList.Count + 1; // also count the host

                    if (combineObj.no_of_ppl > joinedList.Count)
                    {
                        // has space to join

                        // calculate next position
                        nextPosition = 0;
                        var connectedBefore = false;

                        // check whether connectedBefore 
                        foreach (var joinedObj in joinedList)
                        {
                            if (member_id == joinedObj.member_id)
                            {
                                connectedBefore = true;
                                new_combine_id = joinedObj.combine_id;
                                nextPosition = joinedObj.position;

                                break;
                            }
                        }

                        if (!connectedBefore)
                        {
                            for (int i = 2; i <= combineObj.no_of_ppl; i++)
                            {
                                var occupied = false;
                                foreach (var joinedObj in joinedList)
                                {
                                    if (i == joinedObj.position)
                                    {
                                        occupied = true;
                                        break;
                                    }
                                }

                                if (!occupied)
                                {
                                    nextPosition = i;

                                    break;
                                }
                            }
                        }

                        if (connectedBefore)
                        {
                            systemCode = CommonConstant.SystemCode.normal;

                        }
                        else if (nextPosition != 0 && !connectedBefore)
                        {

                            // create join record
                            var obj = new CombineRedemptionObject()
                            {
                                member_id = member_id,
                                coupon_id = 0,
                                position = nextPosition,
                                no_of_ppl = 0,
                                join_combine_id = combine_id,
                                notified_host = 0,
                                status = (int)CommonConstant.CombineRedemptionStatus.connecting
                            };

                            systemCode = Create(obj, ref new_combine_id);
                        }
                        else
                        {
                            systemCode = CommonConstant.SystemCode.record_invalid;
                        }
                    }
                    else
                    {
                        systemCode = CommonConstant.SystemCode.data_invalid;
                    }
                }
            }
            else
            {
                systemCode = CommonConstant.SystemCode.data_invalid;
            }

            return systemCode;
        }

        public CommonConstant.SystemCode CancelJoinCombineRedemption(int member_id, int combine_id)
        {
            var systemCode = CommonConstant.SystemCode.undefine;
            var combineObj = GetDetail(combine_id, true, ref systemCode);

            if (combineObj.combine_id > 0 && combineObj.member_id == member_id)
            { 
                if (combineObj.position == 1) // is host, also need to cancel all other joiner
                {
                    // cancel the host
                    combineObj.status = (int)CommonConstant.CombineRedemptionStatus.voided;
                    combineObj.notified_host = 1;
                    systemCode = Update(combineObj);

                    // also need to cancel all other joiner
                    var joinedList = GetActiveJoinedList(combine_id, true, ref systemCode);
                    foreach (var joinerObj in joinedList)
                    {
                        joinerObj.status = (int)CommonConstant.CombineRedemptionStatus.voided;
                        joinerObj.notified_host = 1;
                        systemCode = Update(joinerObj);
                    }
                }
                else // is joiner
                {
                    var joinedList = GetActiveJoinedList(combineObj.join_combine_id, true, ref systemCode);
                    var totalJoin = joinedList.Count() + 1; // also count the host
                    var hostCombineObj = GetDetail(combineObj.join_combine_id, true, ref systemCode);

                    if (hostCombineObj.no_of_ppl == totalJoin) // not allow join to cancel as the host is doing redemption
                    {
                        systemCode = CommonConstant.SystemCode.err_redeem_hostIsRedeeming;
                    }
                    else
                    {
                        combineObj.status = (int)CommonConstant.CombineRedemptionStatus.voided;
                        combineObj.notified_host = 0;

                        systemCode = Update(combineObj);
                    }
                }
            }
            else
            {
                systemCode = CommonConstant.SystemCode.record_invalid;
            }

            return systemCode;
        }

        public CombineRedemptionObject GetDetail_oldNotNotifyHost(int join_combine_id, bool root_access, ref CommonConstant.SystemCode systemCode)
        {
            systemCode = CommonConstant.SystemCode.undefine;
            var resultObject = new CombineRedemptionObject();

            if (_privilege.read_status == 1 || root_access)
            {
                var query = (from c in db.combine_redemptions

                             where (
                                c.record_status != (int)CommonConstant.RecordStatus.deleted
                                && c.join_combine_id == join_combine_id
                                && c.notified_host == 0
                            )
                             select new CombineRedemptionObject
                             {
                                 combine_id = c.combine_id,
                                 member_id = c.member_id,
                                 coupon_id = c.coupon_id,
                                 position = c.position,
                                 no_of_ppl = c.no_of_ppl,
                                 join_combine_id = c.join_combine_id,
                                 notified_host = c.notified_host,
                                 status = c.status,
                                 crt_date = c.crt_date,
                                 crt_by_type = c.crt_by_type,
                                 crt_by = c.crt_by,
                                 upd_date = c.upd_date,
                                 upd_by_type = c.upd_by_type,
                                 upd_by = c.upd_by,
                                 record_status = c.record_status

                                 // additional info
                             });

                resultObject = query.OrderBy(x => x.crt_date).FirstOrDefault();

                if (resultObject == null)
                    resultObject = new CombineRedemptionObject();

                systemCode = CommonConstant.SystemCode.normal;
            }
            else
            {
                resultObject = new CombineRedemptionObject();
                systemCode = CommonConstant.SystemCode.no_permission;
            }

            return resultObject;
        }

        public CommonConstant.SystemCode NotifyHostCombineRedemption(int member_id, int combine_id, ref int totalJoined, ref int new_position, ref int new_status)
        {
            var systemCode = CommonConstant.SystemCode.undefine;

            var obj = GetDetail_oldNotNotifyHost(combine_id, true, ref systemCode);

            if (obj.combine_id>0)
            {
                obj.notified_host = 1;

                systemCode = Update(obj);

                new_position = obj.position;
                var joinedList = GetActiveJoinedList(combine_id, true, ref systemCode);
                totalJoined = joinedList.Count + 1; //also count the hosts

                new_status = obj.status;

            }
            else
            {
                systemCode = CommonConstant.SystemCode.record_invalid;
            }

            return systemCode;
        }

        // for demo
        public CommonConstant.SystemCode ConfirmCombineRedemption(int member_id, int combine_id)
        {
            var systemCode = CommonConstant.SystemCode.undefine;

            var combineObj = GetDetail(combine_id, true, ref systemCode);
            var joinedList = GetActiveJoinedList(combine_id, true, ref systemCode);

            if (combineObj.status == (int)CommonConstant.CombineRedemptionStatus.connecting 
                && combineObj.no_of_ppl == joinedList.Count + 1)
            {
                var pointAdjustManager = new PointAdjustManager(_accessObject);
                var location_id = 0;
                var remark = "";
                
                // for demo
                var couponPoint = combineObj.coupon_id - 1;
                var pointAdjust = 0 - couponPoint / combineObj.no_of_ppl;

                // host uses point
                systemCode = pointAdjustManager.Adjust(location_id, combineObj.member_id, pointAdjust, remark);
                combineObj.status = (int)CommonConstant.CombineRedemptionStatus.completed;
                
                systemCode = Update(combineObj);

                // joiner uses point
                foreach (var obj in joinedList)
                {
                    systemCode = pointAdjustManager.Adjust(location_id, obj.member_id, pointAdjust, remark);

                    obj.status = (int)CommonConstant.CombineRedemptionStatus.completed;
                    systemCode = Update(obj);
                }
            }
            else
            {
                systemCode = CommonConstant.SystemCode.record_invalid;
            }

            return systemCode;
        }

        // for demo (Fujixerox ver)s
        public CommonConstant.SystemCode ConfirmCombineRedemption_fujixerox(int member_id, int combine_id)
        {
            var systemCode = CommonConstant.SystemCode.undefine;

            var combineObj = GetDetail(combine_id, true, ref systemCode);
            var joinedList = GetActiveJoinedList(combine_id, true, ref systemCode);

            if (combineObj.status == (int)CommonConstant.CombineRedemptionStatus.connecting
                && combineObj.no_of_ppl == joinedList.Count + 1)
            {
                var pointAdjustManager = new PointAdjustManager(_accessObject);
                var location_id = 0;
                var remark = "";

                // for demo
                var couponPoint = 0;

                if (combineObj.coupon_id == 1 || combineObj.coupon_id == 2)
                    couponPoint = 0;
                else if (combineObj.coupon_id == 3)
                    couponPoint = 10;
                else if (combineObj.coupon_id == 4)
                    couponPoint = 15;
                else if (combineObj.coupon_id == 5)
                    couponPoint = 20;
                else if (combineObj.coupon_id == 6)
                    couponPoint = 25;
                else if (combineObj.coupon_id == 7)
                    couponPoint = 30;
                else if (combineObj.coupon_id == 8)
                    couponPoint = 35;

                var pointAdjust = 0 - couponPoint / combineObj.no_of_ppl;

                // host uses point
                systemCode = pointAdjustManager.Adjust(location_id, combineObj.member_id, pointAdjust, remark);
                combineObj.status = (int)CommonConstant.CombineRedemptionStatus.completed;

                systemCode = Update(combineObj);

                // joiner uses point
                foreach (var obj in joinedList)
                {
                    systemCode = pointAdjustManager.Adjust(location_id, obj.member_id, pointAdjust, remark);

                    obj.status = (int)CommonConstant.CombineRedemptionStatus.completed;
                    systemCode = Update(obj);
                }
            }
            else
            {
                systemCode = CommonConstant.SystemCode.record_invalid;
            }

            return systemCode;
        }

    }
}
