using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Palmary.Loyalty.Common;
using Palmary.Loyalty.BO.Database;
using Palmary.Loyalty.BO.DataTransferObjects.PromotionRule;
using Palmary.Loyalty.BO.Modules.Administration.Security;
using Palmary.Loyalty.BO.DataTransferObjects.Security;
using Palmary.Loyalty.BO.DataTransferObjects.Role;
using Palmary.Loyalty.BO.Modules.PromotionRule;
using Palmary.Loyalty.BO.Modules.Member;
using Palmary.Loyalty.BO.Modules.Product;
using Palmary.Loyalty.BO.Modules.Transaction;
using Palmary.Loyalty.BO.Modules.Administration.SystemControl;

namespace Palmary.Loyalty.BO.Modules.PromotionRule
{
    public class PromotionRuleManager
    {
        private static LoyaltyDBDataContext db = new LoyaltyDBDataContext();
        private AccessManager _accessManager;
        private string _module = CommonConstant.Module.promotionRule;
        private RolePrivilegeObject _privilege;
        private static LogManager _logManager;
        private AccessObject _accessObject;

        // For backend, using BO Session to access
        public PromotionRuleManager()
        {
            // access object
            _accessManager = new AccessManager();
            _accessObject = _accessManager.GetAccessObjectBySession();

            // privilege and log
            _privilege = _accessManager.AccessModule(_module);
            _logManager = new LogManager();
        }

        // For Webservices or other not using BO Session to access
        public PromotionRuleManager(AccessObject accessObject)
        {
            // access object
            _accessManager = new AccessManager();
            _accessObject = accessObject;

            // privilege and log
            _privilege = _accessManager.AccessModule(_module, _accessObject.type, _accessObject.id);
            _logManager = new LogManager(_accessObject);
        }

        public CommonConstant.SystemCode Create(
            PromotionRuleObject dataObject
        )
        {
            int? sql_result = 0;
            int? new_rule_id = 0;
            var systemCode = CommonConstant.SystemCode.undefine;

            if (_privilege.insert_status == 1)
            {
                var result = db.sp_CreatePromotionRule(
                              _accessObject.id,
                _accessObject.type, 
                    dataObject.name,
                    dataObject.start_date,
                    dataObject.end_date,
                    dataObject.type,
                    dataObject.conjunction,
                    dataObject.transaction_criteria,
                    dataObject.special_criteria_type,
                    dataObject.special_criteria_value,
                    dataObject.earn_point_type,
                    dataObject.earn_point_value,
                    dataObject.earn_gift_id,
                    dataObject.earn_gift_quantity,
                    dataObject.redeem_discount,
                    dataObject.status,

                    ref new_rule_id,
                    ref sql_result
                );

                systemCode = (CommonConstant.SystemCode)sql_result.Value;
                var rule_id = new_rule_id ?? 0;

                if (systemCode == CommonConstant.SystemCode.normal && rule_id > 0)
                {
                    var memberLevelManager = new PromotionRuleMemberLevelManager();
                    
                    foreach (var x in dataObject.member_level_list)
                    {
                        memberLevelManager.Create(rule_id, x.level_id);
                    }

                    var memberCategoryManager = new PromotionRuleMemberCategoryManager();

                    foreach (var x in dataObject.member_category_list)
                    {
                        memberCategoryManager.Create(rule_id, x.category_id);
                    }

                    var purchaseCriteriaManager = new PromotionRulePurchaseProductCriteriaManager();

                    foreach (var x in dataObject.purchase_criteria_list)
                    {
                        purchaseCriteriaManager.Create(rule_id, x);
                    }

                    var promotionRuleServiceCriteriaManager = new PromotionRuleServiceCriteriaManager();

                    foreach (var x in dataObject.service_criteria_list)
                    {
                        x.rule_id = rule_id;
                        promotionRuleServiceCriteriaManager.Create(x);
                    }
                }
            }
            else
            {
                systemCode = CommonConstant.SystemCode.no_permission;
            }

            return systemCode;
        }

        public List<PromotionRuleObject> GetList(int rowIndexStart, int rowLimit, string searchParams, ref int totalRow)
        {
            var query = (from r in db.promotion_rules
  
                         // for status name 
                         join li_s in db.listing_items on r.status equals li_s.value
                         join l_s in db.listings on li_s.list_id equals l_s.list_id

                         // for rule type name 
                         join li_rt in db.listing_items on r.type equals li_rt.value
                         join l_rt in db.listings on li_rt.list_id equals l_rt.list_id

                         where (
                            r.record_status != CommonConstant.RecordStatus.deleted
                            && l_s.code == "Status"
                            && l_rt.code == "PromotionRuleType"
                         )
                         select new PromotionRuleObject
                         {
                             rule_id = r.rule_id,
                             name = r.name,
                             start_date = r.start_date,
                             end_date = r.end_date,
                             type = r.type,
                             type_name = li_rt.name,
                             transaction_criteria = r.transaction_criteria,
                             special_criteria_type = r.special_criteria_type,
                             special_criteria_value = r.special_criteria_value,
                             earn_point_type = r.earn_point_type,
                             earn_point_value = r.earn_point_value,
                             earn_gift_id = r.earn_gift_id,
                             earn_gift_quantity = r.earn_gift_quantity,
                             redeem_discount = r.redeem_discount,
                             status = r.status,
                             crt_date = r.crt_date,
                             crt_by_type = r.crt_by_type,
                             crt_by = r.crt_by,
                             upd_date = r.upd_date,
                             upd_by_type = r.upd_by_type,
                             upd_by = r.upd_by
                         });

            totalRow = query.Count();
            var limitedList = query.OrderByDescending(x => x.crt_date).Skip(rowIndexStart).Take(rowLimit).ToList();

            return limitedList;
        }

        public PromotionRuleObject GetDetail(int rule_id)
        {
            var query = (from r in db.promotion_rules

                         // for status name 
                         join li_s in db.listing_items on r.status equals li_s.value
                         join l_s in db.listings on li_s.list_id equals l_s.list_id

                         // for rule type name 
                         join li_rt in db.listing_items on r.type equals li_rt.value
                         join l_rt in db.listings on li_rt.list_id equals l_rt.list_id

                         where (
                            r.record_status != CommonConstant.RecordStatus.deleted
                            && r.rule_id == rule_id
                            && l_s.code == "Status"
                            && l_rt.code == "PromotionRuleType"
                         )
                         select new PromotionRuleObject
                         {
                             rule_id = r.rule_id,
                             name = r.name,
                             start_date = r.start_date,
                             end_date = r.end_date,
                             type = r.type,
                             conjunction = r.conjunction,
                             type_name = li_rt.name,
                             transaction_criteria = r.transaction_criteria,
                             special_criteria_type = r.special_criteria_type,
                             special_criteria_value = r.special_criteria_value,
                             earn_point_type = r.earn_point_type,
                             earn_point_value = r.earn_point_value,
                             earn_gift_id = r.earn_gift_id,
                             earn_gift_quantity = r.earn_gift_quantity,
                             redeem_discount = r.redeem_discount,
                             status = r.status,
                             crt_date = r.crt_date,
                             crt_by_type = r.crt_by_type,
                             crt_by = r.crt_by,
                             upd_date = r.upd_date,
                             upd_by_type = r.upd_by_type,
                             upd_by = r.upd_by,

                             status_name = li_s.name
                         });

            return query.FirstOrDefault();
        }

        public List<PromotionRuleObject> GetList_currentActive()
        {
            var query = (from r in db.promotion_rules

                         where (
                            r.record_status != CommonConstant.RecordStatus.deleted
                            && r.status == CommonConstant.Status.active
                            && (r.start_date < DateTime.Now || r.start_date == null)
                            && (r.end_date > DateTime.Now || r.end_date == null)
                         )
                         select new PromotionRuleObject
                         {
                             rule_id = r.rule_id,
                             name = r.name,
                             start_date = r.start_date,
                             end_date = r.end_date,
                             type = r.type,
                             transaction_criteria = r.transaction_criteria,
                             special_criteria_type = r.special_criteria_type,
                             special_criteria_value = r.special_criteria_value,
                             earn_point_type = r.earn_point_type,
                             earn_point_value = r.earn_point_value,
                             earn_gift_id = r.earn_gift_id,
                             earn_gift_quantity = r.earn_gift_quantity,
                             redeem_discount = r.redeem_discount,
                             status = r.status,
                             crt_date = r.crt_date,
                             crt_by_type = r.crt_by_type,
                             crt_by = r.crt_by,
                             upd_date = r.upd_date,
                             upd_by_type = r.upd_by_type,
                             upd_by = r.upd_by
                         });

            var limitedList = query.OrderBy(x => x.start_date).ToList();

            return limitedList;
        }

        // Not allow to hit in conjunction with other rules
        public void GiveBonusForMemberPurchase(int member_id, int purchase_id)
        {
            var systemCode = CommonConstant.SystemCode.undefine;
            var memberManager = new MemberManager(_accessObject);
            var productPurchaseManager = new ProductPurchaseManager(_accessObject);
            var transactionManager = new TransactionManager(_accessObject);
            var member = memberManager.GetDetail(member_id, true, ref systemCode);
            
            var dateTimeNow = DateTime.Now;
             
            // select valid purchase rule
            var query = (from r in db.promotion_rules
                         join r_ml in db.promotion_rule_member_levels on r.rule_id equals r_ml.rule_id
                         join r_mc in db.promotion_rule_member_categories on r.rule_id equals r_mc.rule_id

                         where (
                            r.record_status != CommonConstant.RecordStatus.deleted
                            && r.status == CommonConstant.Status.active
                            && r.type == (int)CommonConstant.PromotionRuleType.purchase
                            && r_ml.member_level_id == member.member_level_id
                            && r_mc.member_category_id == member.member_category_id
                            && ((r.start_date == null && r.end_date == null)
                                || (r.start_date < dateTimeNow && r.end_date == null)
                                || (r.start_date == null && dateTimeNow < r.end_date)
                                || (r.start_date < dateTimeNow && dateTimeNow < r.end_date)
                                )
                         )
                         select new PromotionRuleObject
                         {
                             rule_id = r.rule_id,
                             name = r.name,
                             start_date = r.start_date,
                             end_date = r.end_date,
                             type = r.type,
                             transaction_criteria = r.transaction_criteria,
                             special_criteria_type = r.special_criteria_type,
                             special_criteria_value = r.special_criteria_value,
                             earn_point_type = r.earn_point_type,
                             earn_point_value = r.earn_point_value,
                             earn_gift_id = r.earn_gift_id,
                             earn_gift_quantity = r.earn_gift_quantity,
                             redeem_discount = r.redeem_discount,
                             status = r.status,
                             crt_date = r.crt_date,
                             crt_by_type = r.crt_by_type,
                             crt_by = r.crt_by,
                             upd_date = r.upd_date,
                             upd_by_type = r.upd_by_type,
                             upd_by = r.upd_by
                         });

            var selectList = query.OrderBy(x => x.start_date).ToList();
            var hitList = new List<PromotionRuleObject>();

            // check hit product purchase and calculate total earn point
            var purchase = productPurchaseManager.GetDetail(purchase_id, ref systemCode);

            foreach (var r in selectList)
            {
                var hit = false;
                r.hit_purchase_list = new List<int>();

                List<PromotionRulePurchaseCriteriaObject> purchaseCriteriaList;

                var purchaseCriteriaQuery = (from r_pc in db.promotion_rule_purchase_criterias
                                             join r_ml in db.promotion_rule_member_levels on r.rule_id equals r_ml.rule_id
                                             where (
                                                   r_pc.record_status != CommonConstant.RecordStatus.deleted
                                                   && r_pc.rule_id == r.rule_id
                                                   && r_pc.target_id == purchase.product_id
                                                   && r_ml.member_level_id == member.member_level_id
                                                   )
                                             select new PromotionRulePurchaseCriteriaObject
                                             {
                                                 rec_id = r_pc.rec_id,
                                                 rule_id = r_pc.rule_id,
                                                 target_type = r_pc.target_type,
                                                 target_id = r_pc.target_id,
                                                 criteria = r_pc.criteria,
                                                 quantity = r_pc.quantity,
                                                 point = r_pc.point,
                                                 status = r_pc.status,
                                                 crt_date = r_pc.crt_date,
                                                 crt_by_type = r_pc.crt_by_type,
                                                 crt_by = r_pc.crt_by,
                                                 upd_date = r_pc.upd_date,
                                                 upd_by_type = r_pc.upd_by_type,
                                                 upd_by = r_pc.upd_by,
                                                 record_status = r_pc.record_status
                                             });

                purchaseCriteriaList = purchaseCriteriaQuery.ToList();

                if (r.transaction_criteria == (int)CommonConstant.PromotionRuleTransactionCriteria.singleTransaction)
                {
                    if (purchaseCriteriaList.Count() == 1)
                    {
                        var theCriteria = purchaseCriteriaList[0];
                        if (theCriteria.criteria == (int)CommonConstant.PromotionRulePurchaseProductCriteriaType.point)
                        {
                            if (purchase.point_earned >= theCriteria.point.Value)
                                hit = true;
                             
                        }
                        else // quantity
                        {
                            if (purchase.quantity >= theCriteria.quantity.Value)
                                hit = true;
                        }
                    }
                    else
                        hit = false; // multi purchase critera, now false because currenly do not have order concept

                    if (hit)
                    {
                        if (r.earn_point_type == (int)CommonConstant.PromotionRuleEarnPointType.discrete)
                        {
                            r.total_earn_point = r.earn_point_value.Value;
                        }
                        else // bonus point percent
                        {
                            r.total_earn_point = purchase.point_earned * r.earn_point_value.Value / 100;
                        }

                        r.hit_purchase_list.Add(purchase_id);

                        hitList.Add(r);
                    }
                }
                else // multi transaction
                {
                    double total_purchase_point_earned = 0;
                    int total_purchase_quantity = 0;

                    if (purchaseCriteriaList.Count() == 1)
                    {
                        // get past purchase records
                        DateTime ruleStartTime;
                        DateTime ruleEndTime;

                        if (r.start_date == null)
                            ruleStartTime = DateTime.Parse("1900-01-01 00:00");
                        else
                            ruleStartTime = r.start_date.Value;

                        ruleEndTime = dateTimeNow;

                        var purchaseList = productPurchaseManager.GetListByTime(member_id, ruleStartTime, ruleEndTime, ref systemCode);
                        foreach(var p in purchaseList)
                        {
                            var continueCheck = false;

                            if (p.promotion_transaction_id == 0)
                                continueCheck = true;
                            else
                            {
                                var theTransaction = transactionManager.GetDetail(p.promotion_transaction_id, true, ref systemCode);
                                if (theTransaction.source_id != r.rule_id)
                                    continueCheck = true;
                            }

                            if (continueCheck)
                            {
                                foreach (var pc in purchaseCriteriaList)
                                {
                                    if (p.product_id == pc.target_id && pc.target_type == (int)CommonConstant.PromotionRulePurchaseProductTargetType.product)
                                    {
                                        total_purchase_point_earned += p.point_earned;
                                        total_purchase_quantity += p.quantity;

                                        r.hit_purchase_list.Add(p.purchase_id);
                                    }
                                }
                            }
                        }

                        var theCriteria = purchaseCriteriaList[0];
                        if (theCriteria.criteria == (int)CommonConstant.PromotionRulePurchaseProductCriteriaType.point)
                        {
                            if (total_purchase_point_earned >= theCriteria.point.Value)
                                hit = true;

                        }
                        else // quantity
                        {
                            if (total_purchase_quantity >= theCriteria.quantity.Value)
                                hit = true;
                        }
                    }
                    else
                        hit = false; // multi purchase critera, now false because currenly do not have order concept

                    if (hit)
                    {
                        if (r.earn_point_type == (int)CommonConstant.PromotionRuleEarnPointType.discrete)
                        {
                            r.total_earn_point = r.earn_point_value.Value;
                        }
                        else // bonus point percent
                        {
                            r.total_earn_point = total_purchase_point_earned * r.earn_point_value.Value / 100;
                        }

                        hitList.Add(r);
                    }
                }

                
            }
            
            if (hitList.Count() > 0)
            {
                // git the rule that has highest earn point
                hitList = hitList.OrderByDescending(x => x.total_earn_point).ToList();

                foreach(var hitRule in hitList)
                {
                    if (hitRule.conjunction == 1)
                    {
                        // always not conjunction

                        //foreach (var r in hitList)
                        //{
                        //    // give extra bonus point
                        //    var extra_remark = "";
                        //    var location_id = 0;
                        //    var source_rule_id = r.rule_id;
                        //    int? extra_new_transaction_id = 0;
                        //    var extra_point_earn = r.total_earn_point;
                        //    var point_status = (int)CommonConstant.PointStauts.realized;

                        //    var remark = "";
                        //    var systemConfigManager = new SystemConfigManager();
                        //    var point_expiry_month = int.Parse(systemConfigManager.GetSystemConfigValue("point_expiry_month", ref remark));
                        //    var point_expiry_date = DateTime.Now.AddMonths(point_expiry_month);

                        //    var transactionManager = new TransactionManager();
                        //    var extra_result = transactionManager.AddPoint(
                        //            location_id, member_id, extra_point_earn, point_status, point_expiry_date,
                        //            (int)CommonConstant.TransactionType.promotion_rule, source_rule_id, extra_remark, ref extra_new_transaction_id);
                        //}
                    }
                    else // not hit in conjunction with other rules
                    {
                        // give extra bonus point
                        var extra_remark = "";
                        var location_id = 0;
                        var source_rule_id = hitRule.rule_id;
                        int? extra_new_transaction_id = 0;
                        var extra_point_earn = hitRule.total_earn_point;
                        var point_status = (int)CommonConstant.PointStauts.realized;

                        var remark = "";
                        var systemConfigManager = new SystemConfigManager(_accessObject);
                        var point_expiry_month = int.Parse(systemConfigManager.GetSystemConfigValue("point_expiry_month", ref remark));
                        var point_expiry_date = DateTime.Now.AddMonths(point_expiry_month);

                        // 1. check whether vaild purchase, deduct bonus point from previous promotion rule
                        var checkedTransactionList = new List<int>();
                        var ignoreHitedPurchase = new List<int>();

                        foreach (var p_id in hitRule.hit_purchase_list)
                        {
                            var thePurchase = productPurchaseManager.GetDetail(p_id, ref systemCode);

                            if (thePurchase.promotion_transaction_id != 0)
                            {
                                var needCheck = true;
                                if (checkedTransactionList.Count() > 0)
                                {
                                    var theID = checkedTransactionList.Select(x => x = thePurchase.promotion_transaction_id).First();
                                    if (theID > 0)
                                    {
                                        needCheck = false;
                                        ignoreHitedPurchase.Add(p_id);
                                    }
                                }

                                if (needCheck)
                                {
                                    var theTransaction = transactionManager.GetDetail(thePurchase.promotion_transaction_id, true, ref systemCode);

                                    if (theTransaction.point_change >= hitRule.total_earn_point || theTransaction.source_id == hitRule.rule_id)
                                    { //previous hit more than current hit
                                        ignoreHitedPurchase.Add(p_id);
                                    }
                                    else
                                    {  // deduct bonus point from previous promotion rule
                                        var extra_result = transactionManager.UsePoint(
                                            location_id, member_id, theTransaction.point_change,
                                            (int)CommonConstant.TransactionType.promotion_rule, theTransaction.source_id, extra_remark, ref extra_new_transaction_id);
                                    }
                                    

                                    checkedTransactionList.Add(thePurchase.promotion_transaction_id);
                                }
                            }
                        }

                        // 2. remove purchase that is not valid hit
                        foreach (var p_id in ignoreHitedPurchase)
                        {
                            hitRule.hit_purchase_list.Remove(p_id);
                        }

                        // 3. confirm hit new rule, give point
                        if (hitRule.hit_purchase_list.Count() > 0)
                        {
                            var extra_result = transactionManager.AddPoint(
                                location_id, member_id, extra_point_earn, point_status, point_expiry_date,
                                (int)CommonConstant.TransactionType.promotion_rule, source_rule_id, extra_remark, ref extra_new_transaction_id);

                            // update purchase record with new promote transaction id
                            foreach (var p_id in hitRule.hit_purchase_list)
                            {
                                var thePurchase = productPurchaseManager.GetDetail(p_id, ref systemCode);
                                thePurchase.promotion_transaction_id = extra_new_transaction_id.Value;
                                systemCode = productPurchaseManager.Update(thePurchase, true);
                            }

                            //break for loop highest hit rule
                            break;
                        }
                    }
                } // for loop highest hit rule
            }
        }
    }
}
