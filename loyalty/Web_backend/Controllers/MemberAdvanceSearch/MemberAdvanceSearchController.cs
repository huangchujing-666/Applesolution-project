using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Palmary.Loyalty.Common;
using Palmary.Loyalty.Web_backend.Modules.GlobalHandlers;
using Palmary.Loyalty.BO.DataTransferObjects.Security;
using Palmary.Loyalty.BO.Modules.PromotionRule;
using Palmary.Loyalty.Web_backend.Modules.Utility;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.TableData;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.ExtJsEntity;
using Palmary.Loyalty.BO.DataTransferObjects.MemberAdvanceSearch;
using Palmary.Loyalty.BO.Modules.MemberAdvanceSearch;

using System.Web.Routing;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.PayoadKey;

namespace Palmary.Loyalty.Web_backend.Controllers.MemberAdvanceSearch
{
    [Authorize]
    public class MemberAdvanceSearchController : Controller
    {
        public int _id;

        //
        // GET: /MemberAdvanceSearch/

        //public ActionResult Index()
        //{
        //    return View();
        //}

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            var id = RouteData.Values["id"] ?? "";
            if (!string.IsNullOrEmpty(id.ToString()))
            {
                _id = int.Parse(id.ToString());
            }
            SystemHandler.InitSystemParameters(this.HttpContext.ApplicationInstance as MvcApplication);
        }

        public string Create(FormCollection collection)
        {
            var systemCode = CommonConstant.SystemCode.undefine;

            var searchObj = new MemberAdvanceSearchObject()
            {
                name = collection["name"],
                status = CommonConstant.Status.active
            };

            var ruleList = new List<MemberAdvanceSearchRuleObject>();
            var rowTargetFieldKeyList = collection.AllKeys.Where(x => x.Contains("target_field"));
            
            foreach (var x in rowTargetFieldKeyList)
            {
                var itemID = x.Substring(0, 6);
                var groupID = int.Parse(x.Substring(1, 2));
                var rowID = int.Parse(x.Substring(4, 2));

                var target_field = int.Parse(collection[x]);
                var target_condition = int.Parse(collection[itemID + "_target_condition"]);
                var target_value = collection[itemID + "v1_target_value"];

                ruleList.Add(new MemberAdvanceSearchRuleObject()
                {
                    search_id = 0,
                    group_id = groupID,
                    row_id = rowID,
                    target_field = target_field,
                    target_condition = target_condition,
                    target_value = target_value
                });
            }
            
            searchObj.ruleList = ruleList;

            var result = false;
            var msg = "";
            var msg_prefix = "Create";
            var msg_prefix_result = "";
            var msg_content = "";
            
            var memberAdvanceSearchManager = new MemberAdvanceSearchManager();

            var new_obj_id = 0;

            var resultCode = memberAdvanceSearchManager.CreateWithRule(
                   searchObj,
                   ref new_obj_id
               );

            if (resultCode == CommonConstant.SystemCode.normal)
                result = true;
            else if (systemCode == CommonConstant.SystemCode.no_permission)
                msg_content = "No Permission";

            // output json
            if (result)
                msg_prefix_result = "Success";
            else
                msg_prefix_result = "Fail";

            if (!String.IsNullOrEmpty(msg_content))
                msg = msg_prefix + " " + msg_prefix_result + ": <br/>" + msg_content;
            else
                msg = msg_prefix + " " + msg_prefix_result;

            return new { success = result, msg = msg }.ToJson();
        }

        public string ViewDetail()
        {
            var systemCode = CommonConstant.SystemCode.undefine;
            int current_stock = 0;
            int redeem_count = 0;

            var memberAdvanceSearchManager = new MemberAdvanceSearchManager();
            var obj = memberAdvanceSearchManager.GetDetail(_id, false, ref systemCode);

            //basic table data
            var extTable = new ExtJsTable
            {
                column = 2,
                post_url = "",
                post_header = "",
                title = "Member Advance Search Detail",
                icon = "iconRemarkList",
                //post_params = Url.Action("Perform"),
                isType = true,
                //button_text = "Save",     //no neet button
                //button_icon = "iconSave",
                value_changes = true,
            };

            var rowFieldInput_str = new ExtJsFieldLabelInput<string>(PayloadKeys.Dummy, obj.name)
            {
                fieldLabel = "Name",
                type = "input",
                colspan = 2,
                tabIndex = 1,
                allowBlank = false,
                readOnly = true
            };
            extTable.AddFieldLabelToRow(rowFieldInput_str);

            var group_id = 0;
            foreach (var r in obj.ruleList)
            {
                var newGroup = false;
                if (group_id == 0 || group_id != r.group_id)
                    newGroup = true;

                group_id = r.group_id;

                var displayValue = "";
                if (r.target_condition == (int)CommonConstant.CompareCondition.like)
                    displayValue = r.target_condition_name + ": " + r.target_value_name;
                else
                    displayValue = r.target_condition_name + " " + r.target_value_name;

                if (newGroup)
                {
                    rowFieldInput_str = new ExtJsFieldLabelInput<string>(PayloadKeys.Dummy, displayValue)
                    {
                        fieldLabel = r.target_field_name,
                        type = "input",
                        colspan = 2,
                        tabIndex = 1,
                        allowBlank = false,
                        readOnly = true,
                        group = "Or Group"
                    };
                }
                else
                {
                    rowFieldInput_str = new ExtJsFieldLabelInput<string>(PayloadKeys.Dummy, displayValue)
                    {
                        fieldLabel = r.target_field_name,
                        type = "input",
                        colspan = 2,
                        tabIndex = 1,
                        allowBlank = false,
                        readOnly = true
                    };
                }

                extTable.AddFieldLabelToRow(rowFieldInput_str);
            }

            //rowFieldInput_str = new ExtJsFieldLabelInput<string>(PayloadKeys.Dummy, r.name)
            //{
            //    fieldLabel = "Rule Name",
            //    type = "input",
            //    colspan = 2,
            //    tabIndex = 1,
            //    allowBlank = false,
            //    readOnly = true
            //};
            //extTable.AddFieldLabelToRow(rowFieldInput_str);

            //rowFieldInput_str = new ExtJsFieldLabelInput<string>(PayloadKeys.Dummy, r.type_name)
            //{
            //    fieldLabel = "Rule Type",
            //    type = "input",
            //    colspan = 2,
            //    tabIndex = 1,
            //    allowBlank = false,
            //    readOnly = true
            //};
            //extTable.AddFieldLabelToRow(rowFieldInput_str);

            //var conjunction_name = "";
            //if (r.conjunction == 1)
            //    conjunction_name = "Yes";
            //else
            //    conjunction_name = "No";

            //rowFieldInput_str = new ExtJsFieldLabelInput<string>(PayloadKeys.Dummy, r.start_date == null ? "forever" : r.start_date.Value.ToString("yyyy-MM-dd HH:mm:ss"))
            //{
            //    fieldLabel = "Start Date",
            //    type = "input",
            //    colspan = 2,
            //    tabIndex = 1,
            //    allowBlank = false,
            //    readOnly = true
            //};
            //extTable.AddFieldLabelToRow(rowFieldInput_str);

            //rowFieldInput_str = new ExtJsFieldLabelInput<string>(PayloadKeys.Dummy, r.end_date == null ? "forever" : r.end_date.Value.ToString("yyyy-MM-dd HH:mm:ss"))
            //{
            //    fieldLabel = "End Date",
            //    type = "input",
            //    colspan = 2,
            //    tabIndex = 1,
            //    allowBlank = false,
            //    readOnly = true
            //};
            //extTable.AddFieldLabelToRow(rowFieldInput_str);

            //if (r.type == (int)CommonConstant.PromotionRuleType.purchase)
            //{
            //    var transactionCriteriaName = "";
            //    if (r.transaction_criteria == (int)CommonConstant.PromotionRuleTransactionCriteria.singleTransaction)
            //        transactionCriteriaName = "Single Transaction";
            //    else if (r.transaction_criteria == (int)CommonConstant.PromotionRuleTransactionCriteria.multiTransaction)
            //        transactionCriteriaName = "Multi Transaction";

            //    rowFieldInput_str = new ExtJsFieldLabelInput<string>(PayloadKeys.Dummy, transactionCriteriaName)
            //    {
            //        fieldLabel = "Transaction Criteria",
            //        type = "input",
            //        colspan = 2,
            //        tabIndex = 1,
            //        allowBlank = false,
            //        readOnly = true
            //    };
            //    extTable.AddFieldLabelToRow(rowFieldInput_str);
            //}

            //rowFieldInput_str = new ExtJsFieldLabelInput<string>(PayloadKeys.Dummy, conjunction_name)
            //{
            //    fieldLabel = "Hit in conjunction with other rules",
            //    type = "input",
            //    colspan = 2,
            //    tabIndex = 1,
            //    allowBlank = false,
            //    readOnly = true
            //};
            //extTable.AddFieldLabelToRow(rowFieldInput_str);

            //var promotionRuleMemberLevelManager = new PromotionRuleMemberLevelManager();
            //var level_list = promotionRuleMemberLevelManager.GetList(r.rule_id);
            //var all_level_name = new List<string> { };

            //foreach (var l in level_list)
            //{
            //    all_level_name.Add(l.member_level_name);
            //}

            //rowFieldInput_str = new ExtJsFieldLabelInput<string>(PayloadKeys.Dummy, string.Join(", ", all_level_name.ToArray()))
            //{
            //    fieldLabel = "Member Level",
            //    type = "input",
            //    colspan = 2,
            //    tabIndex = 1,
            //    allowBlank = false,
            //    readOnly = true
            //};
            //extTable.AddFieldLabelToRow(rowFieldInput_str);

            //var promotionRuleMemberCategoryManager = new PromotionRuleMemberCategoryManager();
            //var cat_list = promotionRuleMemberCategoryManager.GetList(r.rule_id);
            //var all_cat_name = new List<string> { };
            //foreach (var l in cat_list)
            //{
            //    all_cat_name.Add(l.member_category_name);
            //}

            //rowFieldInput_str = new ExtJsFieldLabelInput<string>(PayloadKeys.Dummy, string.Join(", ", all_cat_name.ToArray()))
            //{
            //    fieldLabel = "Member Category",
            //    type = "input",
            //    colspan = 2,
            //    tabIndex = 1,
            //    allowBlank = false,
            //    readOnly = true
            //};
            //extTable.AddFieldLabelToRow(rowFieldInput_str);



            //var b_str = "NO";
            //if (r.rule_id == 3)
            //    b_str = "Yes (Birthday Month)";

            //rowFieldInput_str = new ExtJsFieldLabelInput<string>(PayloadKeys.Dummy, b_str)
            //{
            //    fieldLabel = "Special Criteria: Birthday",
            //    type = "input",
            //    colspan = 2,
            //    tabIndex = 1,
            //    allowBlank = false,
            //    readOnly = true
            //};
            //extTable.AddFieldLabelToRow(rowFieldInput_str);

            //if (r.rule_id == 3)
            //{
            //    rowFieldInput_str = new ExtJsFieldLabelInput<string>(PayloadKeys.Dummy, "Any Product (Quantity:1)")
            //    {
            //        fieldLabel = "Purchase",
            //        type = "input",
            //        colspan = 2,
            //        tabIndex = 1,
            //        allowBlank = false,
            //        readOnly = true
            //    };
            //    extTable.AddFieldLabelToRow(rowFieldInput_str);
            //}

            //var earn_point_type_name = "";
            //var earn_point_value = "";
            //if (r.earn_point_type == (int)CommonConstant.PromotionRuleEarnPointType.discrete)
            //{
            //    earn_point_type_name = "Discrete";
            //    earn_point_value = r.earn_point_value.ToString() + " points";
            //}
            //else // bonus percent
            //{
            //    earn_point_type_name = "Bonus Point Percent";
            //    earn_point_value = r.earn_point_value.ToString() + "%";
            //}

            //rowFieldInput_str = new ExtJsFieldLabelInput<string>(PayloadKeys.Dummy, earn_point_type_name)
            //{
            //    fieldLabel = "Earn Point Type",
            //    type = "input",
            //    colspan = 2,
            //    tabIndex = 1,
            //    allowBlank = false,
            //    readOnly = true
            //};
            //extTable.AddFieldLabelToRow(rowFieldInput_str);

            //rowFieldInput_str = new ExtJsFieldLabelInput<string>(PayloadKeys.Dummy, earn_point_value)
            //{
            //    fieldLabel = "Earn Point Value",
            //    type = "input",
            //    colspan = 2,
            //    tabIndex = 1,
            //    allowBlank = false,
            //    readOnly = true
            //};
            //extTable.AddFieldLabelToRow(rowFieldInput_str);

            //rowFieldInput_str = new ExtJsFieldLabelInput<string>(PayloadKeys.Dummy, r.status_name)
            //{
            //    fieldLabel = "Status",
            //    type = "input",
            //    colspan = 2,
            //    tabIndex = 1,
            //    allowBlank = false,
            //    readOnly = true
            //};
            //extTable.AddFieldLabelToRow(rowFieldInput_str);

            return extTable.ToJson();
        }
    }
}
