using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.Objects;
using Palmary.Loyalty.Common;
using Palmary.Loyalty.BO.Database;
using Palmary.Loyalty.BO.Modules.Service;
using Palmary.Loyalty.BO.DataTransferObjects.Service;

using Palmary.Loyalty.BO.Modules.Administration.Security;
using Palmary.Loyalty.BO.Modules.Administration.SystemControl;
using Palmary.Loyalty.BO.Modules.Utility;
using Palmary.Loyalty.BO.Modules.Rule;
using Palmary.Loyalty.BO.Modules.Member;
using Palmary.Loyalty.BO.Modules.Transaction;
using Palmary.Loyalty.BO.Modules.PromotionRule;
using Palmary.Loyalty.BO.DataTransferObjects.Role;
using Palmary.Loyalty.BO.DataTransferObjects.DB;
using Palmary.Loyalty.BO.DataTransferObjects.Security;
using Palmary.Loyalty.BO.DataTransferObjects.Form;
using Palmary.Loyalty.BO.DataTransferObjects.Utility;
using Palmary.Loyalty.BO.DataTransferObjects.Transaction;
using Palmary.Loyalty.BO.DataTransferObjects.Rule;
using Palmary.Loyalty.BO.DataTransferObjects.Member;
using Palmary.Loyalty.BO.DataTransferObjects.PromotionRule;


namespace Palmary.Loyalty.BO.Modules.Service
{
    public class ServicePaymentManager
    {
        private static LoyaltyDBDataContext db = new LoyaltyDBDataContext();
        private AccessManager _accessManager;
        private string _module = CommonConstant.Module.servicePayment;
        private RolePrivilegeObject _privilege;
        private static LogManager _logManager;
        private AccessObject _accessObject;

        public ServicePaymentManager()
        {
            // access object
            _accessManager = new AccessManager();
            _accessObject = _accessManager.GetAccessObjectBySession();

            // privilege and log
            _privilege = _accessManager.AccessModule(_module);
            _logManager = new LogManager();
        }

        private CommonConstant.SystemCode Create(
            ServicePaymentObject dataObject,
            ref int? new_payment_id
        )
        {
            int? sql_result = 0;
            var system_code = CommonConstant.SystemCode.undefine;

            if (_privilege.insert_status == 1)
            {
                var result = db.sp_CreateServicePayment(
                    _accessObject.id, 
                    _accessObject.type, 

                    dataObject.transaction_id,
                    dataObject.invoice_no,
                    dataObject.member_id,
                    dataObject.member_service_id,
                    dataObject.plan_id,
                    dataObject.service_start_date,
                    dataObject.service_end_date,
                    dataObject.amount,
                    dataObject.paid_amount,
                    dataObject.payment_date,
                    dataObject.payment_method,
                    dataObject.status,

                    ref new_payment_id,
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

        public CommonConstant.SystemCode Update(ServicePaymentObject dataObject)
        {
            int? sql_result = 0;
            var system_code = CommonConstant.SystemCode.undefine;

            if (_privilege.update_status == 1)
            {
                var result = db.sp_UpdateServicePayment(
                  _accessObject.id, 
                  _accessObject.type, 

                    dataObject.payment_id,
                    dataObject.invoice_no,
                    dataObject.member_id,
                    dataObject.member_service_id,
                    dataObject.plan_id,
                    dataObject.service_start_date,
                    dataObject.service_end_date,
                    dataObject.amount,
                    dataObject.paid_amount,
                    dataObject.payment_date,
                    dataObject.payment_method,
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

        // Get List with paging, dynamic search, dynamic sorting
        public List<ServicePaymentObject> GetList(int member_id, int rowIndexStart, int rowLimit, List<SearchParmObject> searchParmList, string sortColumn, CommonConstant.SortOrder sortOrder, ref CommonConstant.SystemCode systemCode, ref int totalRow)
        {
            List<ServicePaymentObject> resultList;

            if (_privilege.read_status == 1)
            {
                //var query = (from sp in db.service_payments
                //             join sc in db.service_contracts on sp.contract_id equals sc.contract_id
                //             join s in db.services on sc.service_id equals s.service_id

                //             join li in db.listing_items on sp.payment_method equals li.value
                //             join l in db.listings on li.list_id equals l.list_id

                //             where (
                //                s.record_status != (int)CommonConstant.RecordStatus.deleted
                //                && sc.member_id == member_id
                //                && l.code == "PaymentMethod"
                //            )
                //             select new ServicePaymentObject
                //             {
                //                 payment_id = sp.payment_id,
                //                 contract_id = sp.contract_id,
                //                 member_id = sp.member_id,
                //                 payment_method = sp.payment_method,
                //                 fee = sp.fee,
                //                 status = sp.status,
                //                 crt_date = sp.crt_date,
                //                 crt_by_type = sp.crt_by_type,
                //                 crt_by = sp.crt_by,
                //                 upd_date = sp.upd_date,
                //                 upd_by_type = sp.upd_by_type,
                //                 upd_by = sp.upd_by,
                //                 record_status = sp.record_status,

                //                 // additional info
                //                 service_id = s.service_id,
                //                 service_name = s.name,
                //                 service_no = s.service_no,
                //                 contract_no = sc.contract_no,
                //                 payment_method_name = li.name

                //             });

                //// dynamic sort
                //Func<ServicePaymentObject, Object> orderByFunc = null;
                //if (sortColumn == "service_no")
                //    orderByFunc = x => x.service_no;
                //if (sortColumn == "contract_no")
                //    orderByFunc = x => x.contract_no;
                //if (sortColumn == "service_name")
                //    orderByFunc = x => x.service_name;
                //if (sortColumn == "fee")
                //    orderByFunc = x => x.fee;
                //if (sortColumn == "payment_date")
                //    orderByFunc = x => x.crt_date;
                //if (sortColumn == "payment_method_name")
                //    orderByFunc = x => x.payment_method_name;
                //else
                //{
                //    sortOrder = CommonConstant.SortOrder.desc;
                //    orderByFunc = x => x.crt_date;
                //}

                //// row total
                //totalRow = query.Count();

                //if (sortOrder == CommonConstant.SortOrder.desc)
                //    resultList = query.OrderByDescending(orderByFunc).Skip(rowIndexStart).Take(rowLimit).ToList();
                //else
                //    resultList = query.OrderBy(orderByFunc).Skip(rowIndexStart).Take(rowLimit).ToList();
                resultList = new List<ServicePaymentObject>();
                systemCode = CommonConstant.SystemCode.normal;
            }
            else
            {
                resultList = new List<ServicePaymentObject>();
                systemCode = CommonConstant.SystemCode.no_permission;
            }
            return resultList;
        }


        // Get whole list (limited data) by transaction id
        public List<ServicePaymentDetailObject> GetList_transaction(int transaction_id, int rowIndexStart, int rowLimit, List<SearchParmObject> searchParmList, string sortColumn, CommonConstant.SortOrder sortOrder, ref CommonConstant.SystemCode systemCode, ref int totalRow)
        {
            List<ServicePaymentDetailObject> resultList;

            if (_privilege.read_status == 1)
            {
                var query = (from te in db.transaction_earns
                             join t in db.transactions on te.transaction_id equals t.transaction_id
                             join p in db.service_payments on te.source_id equals p.payment_id
                             join ms in db.member_services on p.member_service_id equals ms.member_service_id
                             join m in db.member_profiles on t.member_id equals m.member_id
                             join splan in db.service_plans on p.plan_id equals splan.plan_id

                             join li_pm in db.listing_items on p.payment_method equals li_pm.value
                             join l_pm in db.listings on li_pm.list_id equals l_pm.list_id

                             // payment status
                             join li_ps in db.listing_items on p.status equals li_ps.value
                             join l_ps in db.listings on li_ps.list_id equals l_ps.list_id

                             join li_pointStatus in db.listing_items on te.point_status equals li_pointStatus.value
                             join l_pointStatus in db.listings on li_pointStatus.list_id equals l_pointStatus.list_id

                             where (
                                te.record_status != (int)CommonConstant.RecordStatus.deleted
                                && te.transaction_id == transaction_id
                                && te.source_type != (int)CommonConstant.TransactionType.promotion_rule  //exclude
                                && l_pm.code == "PaymentMethod"
                                && l_ps.code == "PaymentStauts"
                                && l_pointStatus.code == "PointStatus"
                            )
                             select new ServicePaymentDetailObject
                             {
                                payment_id = p.payment_id,
                                transaction_id = p.transaction_id,
                                invoice_no = p.invoice_no,
                                member_id = p.member_id,
                                member_service_id = p.member_service_id,
                                plan_id = p.plan_id,
                                service_start_date = p.service_start_date,
                                service_end_date = p.service_end_date,
                                amount = p.amount,
                                paid_amount = p.paid_amount,
                                payment_date = p.payment_date,
                                payment_method = p.payment_method,
                                status = p.status,
                                crt_date = p.crt_date,
                                crt_by_type = p.crt_by_type,
                                crt_by = p.crt_by,
                                upd_date = p.upd_date,
                                upd_by_type = p.upd_by_type,
                                upd_by = p.upd_by,
                                record_status = p.record_status,

                                // transaction earn
                                point_change = te.point_earn,
                                point_expiry_date = te.point_expiry_date,
                                point_status = te.point_status,

                                // additional
                                member_service_no = ms.service_no,
                                payment_method_name = li_pm.name,
                                payment_status_name = li_ps.name,
                                point_status_name = li_pointStatus.name,
                                member_no = m.member_no,
                                service_plan_no = splan.plan_no,
                             });

                resultList = query.OrderByDescending(x => x.crt_date).ToList();

                systemCode = CommonConstant.SystemCode.normal;
            }
            else
            {
                resultList = new List<ServicePaymentDetailObject>();
                systemCode = CommonConstant.SystemCode.no_permission;
            }

            return resultList;
        }

        // Get whole list (limited data) by transaction id
        public List<ServicePaymentDetailExtraObject> GetList_transaction_extra(int transaction_id, int rowIndexStart, int rowLimit, List<SearchParmObject> searchParmList, string sortColumn, CommonConstant.SortOrder sortOrder, ref CommonConstant.SystemCode systemCode, ref int totalRow)
        {
            List<ServicePaymentDetailExtraObject> resultList;

            if (_privilege.read_status == 1)
            {
                var query = (from te in db.transaction_earns
                             join t in db.transactions on te.transaction_id equals t.transaction_id
                            
                             join m in db.member_profiles on t.member_id equals m.member_id
                             join r in db.promotion_rules on te.source_id equals r.rule_id

                             join li_pointStatus in db.listing_items on te.point_status equals li_pointStatus.value
                             join l_pointStatus in db.listings on li_pointStatus.list_id equals l_pointStatus.list_id

                             where (
                                te.record_status != (int)CommonConstant.RecordStatus.deleted
                                && te.transaction_id == transaction_id
                                && te.source_type == (int) CommonConstant.TransactionType.promotion_rule
                                && l_pointStatus.code == "PointStatus"
                            )
                             select new ServicePaymentDetailExtraObject
                             {
                                    earn_id = te.earn_id,
                                    transaction_id = te.transaction_id,
                                    source_type = te.source_type,
                                    source_id = te.source_id,
                                    point_earn = te.point_earn,
                                    point_status = te.point_status,
                                    point_expiry_date = te.point_expiry_date,
                                    point_used = te.point_used,
                                    status = te.status,
                                    crt_date = te.crt_date,
                                    crt_by_type = te.crt_by_type,
                                    crt_by = te.crt_by,
                                    upd_date = te.upd_date,
                                    upd_by_type = te.upd_by_type,
                                    upd_by = te.upd_by,
                                    record_status = te.record_status,
                               
                                    // additional
                                    point_status_name = li_pointStatus.name,
                                    member_no = m.member_no,
                                    rule_id = r.rule_id,
                                    rule_name = r.name
                             });
                resultList = query.OrderByDescending(x => x.crt_date).ToList();
                systemCode = CommonConstant.SystemCode.normal;
            }
            else
            {
                resultList = new List<ServicePaymentDetailExtraObject>();
                systemCode = CommonConstant.SystemCode.no_permission;
            }

            return resultList;
        }
        // Get Detail
        public ServicePaymentObject GetDetail(int payment_id, ref CommonConstant.SystemCode systemCode)
        {
            ServicePaymentObject resultObj;

            if (_privilege.read_status == 1)
            {
                var query = (from sp in db.service_payments

                             where (
                                sp.record_status != (int)CommonConstant.RecordStatus.deleted
                                && sp.payment_id == payment_id
                            )
                             select new ServicePaymentObject
                             {
                                 payment_id = sp.payment_id,
                                 transaction_id = sp.transaction_id,
                                 invoice_no = sp.invoice_no,
                                 member_id = sp.member_id,
                                 member_service_id = sp.member_service_id,
                                 plan_id = sp.plan_id,
                                 service_start_date = sp.service_start_date,
                                 service_end_date = sp.service_end_date,
                                 amount = sp.amount,
                                 paid_amount = sp.paid_amount,
                                 payment_date = sp.payment_date,
                                 payment_method = sp.payment_method,
                                 status = sp.status,
                                 crt_date = sp.crt_date,
                                 crt_by_type = sp.crt_by_type,
                                 crt_by = sp.crt_by,
                                 upd_date = sp.upd_date,
                                 upd_by_type = sp.upd_by_type,
                                 upd_by = sp.upd_by,
                                 record_status = sp.record_status
                             });

                resultObj = new ServicePaymentObject();
                systemCode = CommonConstant.SystemCode.normal;
            }
            else
            {
                resultObj = new ServicePaymentObject();
                systemCode = CommonConstant.SystemCode.no_permission;
            }
            return resultObj;
        }

        // Get Detail by invoice no, member id, member service id, plan id
        public ServicePaymentObject GetDetail(string invoice_no, int member_id, int member_service_id, int plan_id, ref CommonConstant.SystemCode systemCode)
        {
            ServicePaymentObject resultObj;

            if (_privilege.read_status == 1)
            {
                var query = (from sp in db.service_payments

                             where (
                                sp.record_status != (int)CommonConstant.RecordStatus.deleted
                                && sp.invoice_no == invoice_no
                                && sp.member_id == member_id
                                && sp.member_service_id == member_service_id
                                && sp.plan_id == plan_id
                            )
                             select new ServicePaymentObject
                             {
                                 payment_id = sp.payment_id,
                                 transaction_id = sp.transaction_id,
                                 invoice_no = sp.invoice_no,
                                 member_id = sp.member_id,
                                 member_service_id = sp.member_service_id,
                                 plan_id = sp.plan_id,
                                 service_start_date = sp.service_start_date,
                                 service_end_date = sp.service_end_date,
                                 amount = sp.amount,
                                 paid_amount = sp.paid_amount,
                                 payment_date = sp.payment_date,
                                 payment_method = sp.payment_method,
                                 status = sp.status,
                                 crt_date = sp.crt_date,
                                 crt_by_type = sp.crt_by_type,
                                 crt_by = sp.crt_by,
                                 upd_date = sp.upd_date,
                                 upd_by_type = sp.upd_by_type,
                                 upd_by = sp.upd_by,
                                 record_status = sp.record_status
                             });

                resultObj = query.FirstOrDefault();
                systemCode = CommonConstant.SystemCode.normal;
            }
            else
            {
                resultObj = new ServicePaymentObject();
                systemCode = CommonConstant.SystemCode.no_permission;
            }
            return resultObj;
        }

        public int GetTransactionID(string invoice_no)
        {
            var query = (from sp in db.service_payments
                             where (
                                sp.record_status != (int)CommonConstant.RecordStatus.deleted
                                && sp.invoice_no == invoice_no
                            )
                             select new ServicePaymentObject
                             {
                                 payment_id = sp.payment_id,
                                 transaction_id = sp.transaction_id
                             });

            var payment = query.FirstOrDefault();

            return (payment == null) ? 0 : payment.transaction_id;
        }

        public CommonConstant.SystemCode MakePayment(
            int transactionType,
            ServicePaymentObject payment,
            ref string result_msg
        )
        {
            var system_code = CommonConstant.SystemCode.undefine;

            if (_privilege.insert_status == 1 && transactionType == (int)CommonConstant.TransactionType.postpaidservice)
            {
                var servicePlanManager = new ServicePlanManager();
                var basicRuleManager = new BasicRuleManager();
                var memberManger = new MemberManager();

                // retrive related objects
                BasicRuleObject rule = null;
                var validMember = false;
                var validServicePlan = false;
                var validRule = false;
             
                var member = memberManger.GetDetail_byMemberNo(payment.member_no, ref system_code);
                if (member == null)
                    result_msg = "Invalid Member No";
                else if (member.status != CommonConstant.Status.active)
                    result_msg = "Inactive Member";
                else
                {
                    validMember = true;
                    if (payment.member_id == 0)
                        payment.member_id = member.member_id;
                }

                var servicePlan = servicePlanManager.GetDetail_byServicePlanNo(payment.service_plan_no, ref system_code);
                if (servicePlan == null)
                    result_msg = "Invalid Service Plan No";
                else if (servicePlan.status != CommonConstant.Status.active)
                    result_msg = "Inactive Service Plan";
                else
                {
                    validServicePlan = true;
                    if (payment.plan_id == 0)
                        payment.plan_id = servicePlan.plan_id;
                }

                // calculate points
                double point_earn = 0;
                if (validServicePlan && validMember)
                {
                    //rule = basicRuleManager.GetDetail((int)CommonConstant.BasicRuleType.PostPaidService, servicePlan.type, member.member_level_id, ref system_code);

                    //if (rule != null && rule.status == CommonConstant.Status.active)
                    //{
                    //    int div = (int)(payment.amount / rule.payment);
                    //    point_earn = div * rule.point;

                    //    validRule = true;
                    //}
                    //else
                    //    result_msg = "Invalid Basic Rule";
                }

                if (validMember && validServicePlan && validRule)
                {
                    var point_expiry_date = DateTime.Now;
                    point_expiry_date = point_expiry_date.AddMonths(rule.point_expiry_month);

                    // 1. check exist member service
                    var memberServiceManager = new MemberServiceManager();
                    var memberService = memberServiceManager.GetDetail(payment.member_service_no, ref system_code);
                    if (memberService == null || memberService.member_service_id == 0)
                    {
                        // new member service
                        memberService = new MemberServiceObject()
                        {
                            member_id = payment.member_id,
                            service_no = payment.member_service_no,
                            plan_id = servicePlan.plan_id,
                            point = 0,
                            start_date = new DateTime(2014, 1, 1),
                            end_date = new DateTime(2016, 01, 01),
                            status = CommonConstant.Status.active
                        };

                        int? new_service_id = 0;
                        system_code = memberServiceManager.Create(memberService, ref new_service_id);
                        memberService.member_service_id = new_service_id.Value;
                    }
                   
                    payment.member_service_id = memberService.member_service_id;

                    // 2. check exist payment
                    var existServicePayment = GetDetail(payment.invoice_no, member.member_id, memberService.member_service_id, servicePlan.plan_id, ref system_code);
                    var transactionManager = new TransactionManager();
                    var transactionEarnManager = new TransactionEarnManager();

                    // 3. Add Point
                    if (existServicePayment == null || existServicePayment.payment_id == 0)
                    {           
                        var point_status = (int)CommonConstant.PointStauts.unrealized;
                        if (payment.status == (int)CommonConstant.PaymentStauts.complete)
                            point_status = (int)CommonConstant.PointStauts.realized;

                        // check exist invoice no
                        payment.transaction_id = GetTransactionID(payment.invoice_no);
                        
                        // main transaction
                        if (payment.transaction_id > 0)
                        {
                            //Update transaction
                            var transaction = transactionManager.GetDetail(payment.transaction_id, true, ref system_code);
                            transaction.point_change += point_earn;
                            transaction.point_status = point_status;
                            system_code = transactionManager.Update(transaction);
                        }
                        else
                        {
                            // create transaction - add point
                            var remark = "";
                            var location_id = 0;
                            var source_id = 0;
                            int? new_transaction_id = 0;

                            var result = transactionManager.AddPoint(location_id, member.member_id, point_earn, point_status, point_expiry_date, transactionType, source_id, remark, ref new_transaction_id);

                            if (result)
                                payment.transaction_id = new_transaction_id.Value;
                        }

                        // transaction earn and payment
                        if (payment.transaction_id > 0)
                        {
                            int? new_payment_id = 0;
                            system_code = Create(payment, ref new_payment_id);

                            if (system_code == CommonConstant.SystemCode.normal)
                            {
                                // earn detail
                                var transactionEarnObject = new TransactionEarnObject()
                                {
                                    transaction_id = payment.transaction_id,
                                    source_type = (int)CommonConstant.TransactionType.postpaidservice,
                                    source_id = new_payment_id.Value,

                                    point_earn = point_earn,
                                    point_status = point_status,
                                    point_expiry_date = point_expiry_date,
                                    point_used = 0,

                                    status = CommonConstant.TransactionStatus.active,
                                };
                                system_code = transactionEarnManager.Create(transactionEarnObject);
                            }

                            // calculate extra bonus point
                            var ruleManager = new PromotionRuleManager();
                            var ruleMemberLevelManager = new PromotionRuleMemberLevelManager();
                            var ruleMemberCategoryManager = new PromotionRuleMemberCategoryManager();
                            var ruleServiceCriteriaManager = new PromotionRuleServiceCriteriaManager();

                            var currentActiveRules = ruleManager.GetList_currentActive();
                            var promotionRuleEarnList = new List<PromotionRuleEarnObject>();

                            foreach (var c in currentActiveRules)
                            {
                                var validMemberLevel = ruleMemberLevelManager.CheckExist(c.rule_id, member.member_level_id);
                                var validMemberCategory = ruleMemberCategoryManager.CheckExist(c.rule_id, member.member_category_id);
                                var validService = ruleServiceCriteriaManager.CheckExist(c.rule_id, servicePlan.type);

                                double extra_point_earn = 0;

                                if (validMemberLevel && validMemberCategory && validService && c.type == (int)CommonConstant.PromotionRuleType.servicePayment)
                                {
                                    var serviceCriteria = ruleServiceCriteriaManager.GetDetail(c.rule_id, servicePlan.type);

                                    int div = 0;
                                    if (serviceCriteria.criteria_type == (int)CommonConstant.PromotionRuleServicePaymentCriteria.point)
                                        div = (int)(point_earn / serviceCriteria.criteria_value);
                                    else if (serviceCriteria.criteria_type == (int)CommonConstant.PromotionRuleServicePaymentCriteria.payment)
                                        div = (int)(payment.amount / serviceCriteria.criteria_value);

                                    extra_point_earn = div * c.earn_point_value.Value;
                                }

                                if (extra_point_earn > 0)
                                {
                                    var promotionRuleEarn = new PromotionRuleEarnObject()
                                    {
                                        rule_id = c.rule_id,
                                        earn_point = extra_point_earn,
                                    };
                                    promotionRuleEarnList.Add(promotionRuleEarn);
                                }
                            }

                            if (promotionRuleEarnList.Count() > 0)
                            {
                                var promotionRuleEarn_max = promotionRuleEarnList.OrderByDescending(x => x.earn_point).First();

                                // create transaction - add point
                                var remark = "";
                                var location_id = 0;
                                var source_id = promotionRuleEarn_max.rule_id;
                                int? new_transaction_id = 0;

                                var result = transactionManager.AddPoint(
                                        location_id, member.member_id, point_earn, point_status, point_expiry_date, 
                                        (int)CommonConstant.TransactionType.promotion_rule, source_id, remark, ref new_transaction_id);

                                if (result)
                                    system_code = CommonConstant.SystemCode.normal;
                                else
                                    system_code = CommonConstant.SystemCode.record_invalid;

                                //// earn transaction
                                //var transactionEarnObject = new TransactionEarnObject()
                                //{
                                //    transaction_id = payment.transaction_id,
                                //    source_type = (int)CommonConstant.TransactionType.promotion_rule,
                                //    source_id = promotionRuleEarn_max.rule_id,

                                //    point_earn = promotionRuleEarn_max.earn_point,
                                //    point_status = point_status,
                                //    point_expiry_date = point_expiry_date,
                                //    point_used = 0,

                                //    status = CommonConstant.TransactionStatus.active,
                                //};
                                //system_code = transactionEarnManager.Create(transactionEarnObject);

                                //// update main transaction
                                //var transaction = transactionManager.GetDetail(payment.transaction_id, ref system_code);
                                //transaction.point_change += promotionRuleEarn_max.earn_point;
                                //transaction.point_status = point_status;
                                //system_code = transactionManager.Update(transaction);
                            }

                            // update member point cache
                            member.available_point = (double)transactionManager.GetAvailablePoint(member.member_id);
                            system_code = memberManger.Update_directCore(member);
                        }
                        else
                            system_code = CommonConstant.SystemCode.record_invalid;
                    }
                    else
                    { 
                        // update payment, transaction, transaction earn
                        if (payment.status == (int)CommonConstant.PaymentStauts.complete && existServicePayment.status == (int)CommonConstant.PaymentStauts.incomplete)
                        {
                            var transaction = transactionManager.GetDetail(existServicePayment.transaction_id, true, ref system_code);
                            var transactionEarn = transactionEarnManager.GetDetail(existServicePayment.transaction_id, existServicePayment.payment_id, ref system_code);

                            existServicePayment.status = (int)CommonConstant.PointStauts.realized;
                            transaction.point_status = (int)CommonConstant.PointStauts.realized;
                            transactionEarn.point_status = (int)CommonConstant.PointStauts.realized;

                            system_code = Update(existServicePayment);
                            system_code = transactionManager.Update(transaction);
                            system_code = transactionEarnManager.Update(transactionEarn);
                        }
                        else
                        {
                            result_msg = "No need to update";
                            system_code = CommonConstant.SystemCode.record_invalid;
                        }
                    }
                }
                else
                    system_code = CommonConstant.SystemCode.record_invalid;
            }
            else
            {
                system_code = CommonConstant.SystemCode.no_permission;
            }

            return system_code;
        }
    }
}