using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.Objects;
using Palmary.Loyalty.Common;
using Palmary.Loyalty.BO.Database;
using Palmary.Loyalty.BO.Modules.Service;
using Palmary.Loyalty.BO.DataTransferObjects.Rule;

using Palmary.Loyalty.BO.Modules.Administration.Security;
using Palmary.Loyalty.BO.Modules.Administration.SystemControl;
using Palmary.Loyalty.BO.Modules.Utility;
using Palmary.Loyalty.BO.DataTransferObjects.Role;
using Palmary.Loyalty.BO.DataTransferObjects.DB;
using Palmary.Loyalty.BO.DataTransferObjects.Security;
using Palmary.Loyalty.BO.DataTransferObjects.Form;
using Palmary.Loyalty.BO.DataTransferObjects.Utility;

namespace Palmary.Loyalty.BO.Modules.Rule
{
    public class BasicRuleManager
    {
        private static LoyaltyDBDataContext db = new LoyaltyDBDataContext();
        private AccessManager _accessManager;
        private string _module = CommonConstant.Module.basicRule;
        private RolePrivilegeObject _privilege;
        private static LogManager _logManager;
        private AccessObject _accessObject;


        public BasicRuleManager()
        {
            // access object
            _accessManager = new AccessManager();
            _accessObject = _accessManager.GetAccessObjectBySession();

            // privilege and log
            _privilege = _accessManager.AccessModule(_module);
            _logManager = new LogManager();
        }

        // Get List of PostPaidService
        public List<BasicRuleObject> GetList(int rule_type, ref CommonConstant.SystemCode system_code)
        {
            List<BasicRuleObject> resultList;

            if (_privilege.read_status == 1 && rule_type == (int)CommonConstant.BasicRuleType.RetailPurchase)
            {
                var query = (from br in db.basic_rules
                             join ml in db.member_levels on br.member_level_id equals ml.level_id
                           //  join mc in db.member_categories on br.memebr_category_id equals mc.category_id
                           //  join mcl in db.member_category_langs on br.memebr_category_id equals mcl.category_id
                             //join p in db.products on br.target_id equals p.product_id


                             join li in db.listing_items on br.status equals li.value
                             join l in db.listings on li.list_id equals l.list_id

                             join li_t in db.listing_items on br.type equals li_t.value
                             join l_t in db.listings on li_t.list_id equals l_t.list_id

                             where (
                             br.record_status != (int)CommonConstant.RecordStatus.deleted
                             && l.code == "Status"
                             && l_t.code == "BasicRuleType"
                             && br.type == rule_type
                          //   && mcl.lang_id == (int)CommonConstant.LangCode.en
                         )
                             select new BasicRuleObject
                             {
                                 basic_rule_id = br.basic_rule_id,
                                 type = br.type,
                                 target_id = br.target_id,
                                 member_level_id = br.member_level_id,
                                 memebr_category_id = br.memebr_category_id,
                                 ratio_payment = br.ratio_payment,
                                 ratio_point = br.ratio_point,
                                 point = br.point,
                                 point_expiry_month = br.point_expiry_month,
                                 remark = br.remark,
                                 status = br.status,
                                 crt_date = br.crt_date,
                                 crt_by_type = br.crt_by_type,
                                 crt_by = br.crt_by,
                                 upd_date = br.upd_date,
                                 upd_by_type = br.upd_by_type,
                                 upd_by = br.upd_by,
                                 record_status = br.record_status,

                                 // additional
                                 target_no = "Any Product",//p.product_no,
                                 member_level_name = ml.name,
                                 status_name = li.name,
                                 member_category_name = "",//mcl.name,
                                 type_name = li_t.name,
                                 member_level_display_order = ml.display_order,
                                 category_display_order = 0//mc.display_order
                             });

                resultList = query.OrderBy(x => x.category_display_order).ToList();
                system_code = CommonConstant.SystemCode.normal;
            }
            else if (_privilege.read_status == 1 && rule_type == (int)CommonConstant.BasicRuleType.PostPaidService)
            {
                var query = (from br in db.basic_rules
                                join ml in db.member_levels on br.member_level_id equals ml.level_id
                                join mc in db.member_categories on br.memebr_category_id equals mc.category_id
                                join mcl in db.member_category_langs on br.memebr_category_id equals mcl.category_id
                                join sp in db.service_plans on br.target_id equals sp.plan_id


                                join li in db.listing_items on br.status equals li.value
                                join l in db.listings on li.list_id equals l.list_id

                                join li_t in db.listing_items on br.type equals li_t.value
                                join l_t in db.listings on li_t.list_id equals l_t.list_id

                                where (
                                br.record_status != (int)CommonConstant.RecordStatus.deleted
                                && l.code == "Status"
                                && l_t.code == "BasicRuleType"
                                && br.type == rule_type
                                && mcl.lang_id == (int)CommonConstant.LangCode.tc
                            )
                                select new BasicRuleObject
                                {
                                    basic_rule_id = br.basic_rule_id,
                                    type = br.type,
                                    target_id = br.target_id,
                                    member_level_id = br.member_level_id,
                                    memebr_category_id = br.memebr_category_id,
                                    ratio_payment = br.ratio_payment,
                                    ratio_point = br.ratio_point,
                                    point = br.point,
                                    point_expiry_month = br.point_expiry_month,
                                    remark = br.remark,
                                    status = br.status,
                                    crt_date = br.crt_date,
                                    crt_by_type = br.crt_by_type,
                                    crt_by = br.crt_by,
                                    upd_date = br.upd_date,
                                    upd_by_type = br.upd_by_type,
                                    upd_by = br.upd_by,
                                    record_status = br.record_status,

                                    // additional
                                    target_no = sp.plan_no,
                                    
                                    status_name = li.name,
                                    member_category_name = mcl.name,
                                    type_name = li_t.name,
                                    
                                    category_display_order = mc.display_order
                                });

                resultList = query.OrderBy(x => x.category_display_order).ToList();
                system_code = CommonConstant.SystemCode.normal;
            }
           
            else
            {
                resultList = new List<BasicRuleObject>();
                system_code = CommonConstant.SystemCode.no_permission;
            }

            return resultList;
        }

        // Get Detail
        public BasicRuleObject GetDetail(int rule_id, ref CommonConstant.SystemCode system_code)
        {
            BasicRuleObject resultObj;

            if (_privilege.read_status == 1)
            {
                var query = (from br in db.basic_rules
                             where (
                                br.record_status != (int)CommonConstant.RecordStatus.deleted
                                && br.basic_rule_id == rule_id
                             )
                             select new BasicRuleObject
                             {
                                 basic_rule_id = br.basic_rule_id,
                                 type = br.type,
                                 target_id = br.target_id,
                                 member_level_id = br.member_level_id,
                                 memebr_category_id = br.memebr_category_id,
                                 ratio_payment = br.ratio_payment,
                                 ratio_point = br.ratio_point,

                                 point = br.point,
                                 point_expiry_month = br.point_expiry_month,
                                 remark = br.remark,
                                 status = br.status,

                                 crt_date = br.crt_date,
                                 crt_by_type = br.crt_by_type,
                                 crt_by = br.crt_by,
                                 upd_date = br.upd_date,
                                 upd_by_type = br.upd_by_type,
                                 upd_by = br.upd_by,
                                 record_status = br.record_status,
                             });

                resultObj = query.FirstOrDefault();

                system_code = CommonConstant.SystemCode.normal;
            }
            else
            {
                resultObj = new BasicRuleObject();
                system_code = CommonConstant.SystemCode.no_permission;
            }

            return resultObj;
        }

        // Manager Object (DAO)  CREATE (LINQ version)
        public CommonConstant.SystemCode Create(
            BasicRuleObject dataObject
        )
        {
            int? sql_result = 0;
            var system_code = CommonConstant.SystemCode.undefine;

            if (_privilege.insert_status == 1)
            {
                var result = db.sp_CreateBasicRule(
                                  _accessObject.id,
                _accessObject.type, 

                    dataObject.type,
                    dataObject.target_id,
                    dataObject.member_level_id,
                    dataObject.memebr_category_id,
                    dataObject.ratio_payment,
                    dataObject.ratio_point,
                    dataObject.point,
                    dataObject.point_expiry_month,
                    dataObject.remark,
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

        // Get Detail
        //public BasicRuleObject GetDetail(int rule_type, int service_category_id,  int member_level_id, ref CommonConstant.SystemCode system_code)
        //{
        //    BasicRuleObject resultObj;

        //    if (_privilege.read_status == 1)
        //    {
        //        var query = (from br in db.basic_rules
        //                     where (
        //                        br.record_status != (int)CommonConstant.RecordStatus.deleted
        //                        && br.type == rule_type
        //                        && br.service_category_id == service_category_id 
        //                        && br.member_level_id == member_level_id
        //                     )
        //                     select new BasicRuleObject
        //                     {
        //                         basic_rule_id = br.basic_rule_id,
        //                         type = br.type,
        //                         service_category_id = br.service_category_id,
        //                         member_level_id = br.member_level_id,
        //                         payment = br.payment,
        //                         point = br.point,
        //                         point_expiry_month = br.point_expiry_month,
        //                         remark = br.remark,
        //                         status = br.status,
        //                         crt_date = br.crt_date,
        //                         crt_by_type = br.crt_by_type,
        //                         crt_by = br.crt_by,
        //                         upd_date = br.upd_date,
        //                         upd_by_type = br.upd_by_type,
        //                         upd_by = br.upd_by,
        //                         record_status = br.record_status,
        //                     });

        //        resultObj = query.FirstOrDefault();

        //        system_code = CommonConstant.SystemCode.normal;
        //    }
        //    else
        //    {
        //        resultObj = new BasicRuleObject();
        //        system_code = CommonConstant.SystemCode.no_permission;
        //    }

        //    return resultObj;
        //}

        public CommonConstant.SystemCode Update(BasicRuleObject dataObject)
        {
            int? sql_result = 0;
            var system_code = CommonConstant.SystemCode.undefine;

            if (_privilege.update_status == 1)
            {
                var result = db.sp_UpdateBasicRule(
                   _accessObject.id, 
                   _accessObject.type, 

                    dataObject.basic_rule_id,
                    dataObject.type,
                    dataObject.target_id,
                    dataObject.member_level_id,
                    dataObject.memebr_category_id,
                    dataObject.ratio_payment,
                    dataObject.ratio_point,
                    dataObject.point,
                    dataObject.point_expiry_month,
                    dataObject.remark,
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
