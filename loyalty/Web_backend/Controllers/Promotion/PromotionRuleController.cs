using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Palmary.Loyalty.Common;
using Palmary.Loyalty.Web_backend.Modules.GlobalHandlers;
using Palmary.Loyalty.BO.DataTransferObjects.Member;
using Palmary.Loyalty.BO.DataTransferObjects.Security;
using Palmary.Loyalty.BO.DataTransferObjects.PromotionRule;
using Palmary.Loyalty.BO.Modules.PromotionRule;
using Palmary.Loyalty.Web_backend.Modules.Utility;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.TableData;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.ExtJsEntity;
using System.Web.Routing;

using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.PayoadKey;

namespace Palmary.Loyalty.Web_backend.Controllers.Promotion
{
    [Authorize]
    public class PromotionRuleController : Controller
    {
        public int _id;

        public PromotionRuleController()
        {
        }

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

        public string CreateForm()
        {
            var reault = new
            {
                post_url = "../common/listData/role",
                post_header = "",
                post_params = "", //Url.Action("SearchParams", new { Module = _module }),
                title = "Create Promotion Rule",
                icon = "iconAward_star_gold_1",
                sub_title = "Advance Search",
                isType = true,
                column = 2,
                row = new[]{
                            new {fieldLabel = "Name", type = "input", name = "name", tabIndex = 1, colspan=1, datasource=""},
                            new {fieldLabel = "Rule Type", type = "select_input", name = "ruleType", tabIndex = 1, colspan=1 , datasource="../PromotionRule/GetRuleType"},
                            new {fieldLabel = "Active Date Range", type = "dateTimeRange", name = "activeDateRange", tabIndex = 2, colspan=1,datasource="" }
                        },

                rowhidden = new[] {
                    new {type="hidden", name="uid"}
                    },
                button_text = "Save",
                button_icon = "iconSave",
                value_changes = true
            };
            return reault.ToJson();
        }

        public string GetRuleType()
        { 
            var itemList = new List<ExtJsDataRowListItem> { };

            //itemList.Add(new ExtJsDataRowListItem { id = 4, value = "Service Payment" });
            itemList.Add(new ExtJsDataRowListItem { id = 1, value = "Product Purchase" });
            itemList.Add(new ExtJsDataRowListItem { id = 2, value = "Redemption" });
            itemList.Add(new ExtJsDataRowListItem { id = 3, value = "Complementary" });

            return itemList.ToJson();
        }

        public string GetTransactionType()
        {
            var itemList = new List<ExtJsDataRowListItem> { };

            itemList.Add(new ExtJsDataRowListItem { id = 1, value = "single transaction" });
            itemList.Add(new ExtJsDataRowListItem { id = 2, value = "multi-transactions" });
            
            return itemList.ToJson();
        }

        public string GetEarnTarget()
        {
            var itemList = new List<ExtJsDataRowListItem> { };

            itemList.Add(new ExtJsDataRowListItem { id = 1, value = "Referrer" });
            itemList.Add(new ExtJsDataRowListItem { id = 2, value = "Referee" });
            itemList.Add(new ExtJsDataRowListItem { id = 3, value = "Both" });

            return itemList.ToJson();
        }

        public string GetBirthdayType()
        {
            var itemList = new List<ExtJsDataRowListItem> { };

            itemList.Add(new ExtJsDataRowListItem { id = 1, value = "Birthday Day" });
            itemList.Add(new ExtJsDataRowListItem { id = 2, value = "Birthday Month" });

            return itemList.ToJson();
        }

        public string GetEarnPointType()
        {
            var itemList = new List<ExtJsDataRowListItem> { };

            itemList.Add(new ExtJsDataRowListItem { id = (int)CommonConstant.PromotionRuleEarnPointType.discrete, value = "Discrete" });
            itemList.Add(new ExtJsDataRowListItem { id = (int)CommonConstant.PromotionRuleEarnPointType.bonus_percent, value = "Bonus Point %" });

            return itemList.ToJson();
        }


        public string GetPurchaseProductType()
        {
            var itemList = new List<ExtJsDataRowListItem> { };

            itemList.Add(new ExtJsDataRowListItem { id = 1, value = "Product Category" });
            itemList.Add(new ExtJsDataRowListItem { id = 2, value = "Product" });
            itemList.Add(new ExtJsDataRowListItem { id = 3, value = "Any" });

            return itemList.ToJson();
        }

        public string GetPurchaseProductCriteria()
        {
            var itemList = new List<ExtJsDataRowListItem> { };

            itemList.Add(new ExtJsDataRowListItem { id = 1, value = "Quantity" });
            itemList.Add(new ExtJsDataRowListItem { id = 2, value = "Point" });
            
            return itemList.ToJson();
        }

        public string GetServicePaymentCriteria()
        {
            var itemList = new List<ExtJsDataRowListItem> { };

            itemList.Add(new ExtJsDataRowListItem { id = 1, value = "Point" });
            itemList.Add(new ExtJsDataRowListItem { id = 2, value = "Payment(MOP)" });

            return itemList.ToJson();
        }

        public string GetRowHead()
        {
            var itemList = new List<ExtJsDataRowListItem> { };

            itemList.Add(new ExtJsDataRowListItem { id = 1, value = "　 " });
            itemList.Add(new ExtJsDataRowListItem { id = 2, value = "(" });
            itemList.Add(new ExtJsDataRowListItem { id = 3, value = "((" });
            itemList.Add(new ExtJsDataRowListItem { id = 4, value = "(((" });

            return itemList.ToJson();
        }

        public string GetBracketEnd()
        {
            var itemList = new List<ExtJsDataRowListItem> { };

            itemList.Add(new ExtJsDataRowListItem { id = 1, value = "　" });
            itemList.Add(new ExtJsDataRowListItem { id = 2, value = ")" });
            itemList.Add(new ExtJsDataRowListItem { id = 3, value = "))" });
            itemList.Add(new ExtJsDataRowListItem { id = 4, value = ")))" });
            
            return itemList.ToJson();
        }

        public string GetRowEnd()
        {
            var itemList = new List<ExtJsDataRowListItem> { };

            itemList.Add(new ExtJsDataRowListItem { id = 0, value = "　" });
            itemList.Add(new ExtJsDataRowListItem { id = 1, value = "AND" });
            itemList.Add(new ExtJsDataRowListItem { id = 2, value = "OR" });
            
            return itemList.ToJson();
        }

        public string GetValueConnector_intfloat()
        {
            var itemList = new List<ExtJsDataRowListItem> { };

            itemList.Add(new ExtJsDataRowListItem { id = 1, value = "=" });
            itemList.Add(new ExtJsDataRowListItem { id = 2, value = ">" });
            itemList.Add(new ExtJsDataRowListItem { id = 3, value = ">=" });
            itemList.Add(new ExtJsDataRowListItem { id = 4, value = "<" });
            itemList.Add(new ExtJsDataRowListItem { id = 5, value = "<=" });
            
            return itemList.ToJson();
        }

        public string GetValueConnector_str()
        {
            var itemList = new List<ExtJsDataRowListItem> { };

            itemList.Add(new ExtJsDataRowListItem { id = 1, value = "=" });
            itemList.Add(new ExtJsDataRowListItem { id = 2, value = "Like" });

            return itemList.ToJson();
        }

        public string GetValueConnector_select()
        {
            var itemList = new List<ExtJsDataRowListItem> { };

            itemList.Add(new ExtJsDataRowListItem { id = 1, value = "=" });

            return itemList.ToJson();
        }

        public string GetMemberColumn()
        {
            var itemList = new List<ExtJsDataRowListItem> { };

            itemList.Add(new ExtJsDataRowListItem { id = 1, value = "birth_day" });
            itemList.Add(new ExtJsDataRowListItem { id = 2, value = "birth_month" });
            itemList.Add(new ExtJsDataRowListItem { id = 3, value = "birth_year" });
            itemList.Add(new ExtJsDataRowListItem { id = 4, value = "CTM_date" });
            itemList.Add(new ExtJsDataRowListItem { id = 5, value = "email" });
            itemList.Add(new ExtJsDataRowListItem { id = 6, value = "fbemail" });
            itemList.Add(new ExtJsDataRowListItem { id = 7, value = "firstname" });
            itemList.Add(new ExtJsDataRowListItem { id = 8, value = "gender" });
            itemList.Add(new ExtJsDataRowListItem { id = 9, value = "lastname" });
            itemList.Add(new ExtJsDataRowListItem { id = 10, value = "Loyalty_date" });
            itemList.Add(new ExtJsDataRowListItem { id = 11, value = "Loyalty_int" });
            itemList.Add(new ExtJsDataRowListItem { id = 12, value = "Loyalty_string" });
            itemList.Add(new ExtJsDataRowListItem { id = 13, value = "member_code" });
            itemList.Add(new ExtJsDataRowListItem { id = 14, value = "middlename" });
            itemList.Add(new ExtJsDataRowListItem { id = 15, value = "mobile_no" });
           // itemList.Add(new ExtJsDataRowListItem { id = 16, value = "password" });
            itemList.Add(new ExtJsDataRowListItem { id = 17, value = "salutation" });

            return itemList.ToJson();
        }

        public string GetTransactionColumn()
        {
            var itemList = new List<ExtJsDataRowListItem> { };

            itemList.Add(new ExtJsDataRowListItem { id = 1, value = "Transaction ID" });
            itemList.Add(new ExtJsDataRowListItem { id = 2, value = "Member No" });
            itemList.Add(new ExtJsDataRowListItem { id = 3, value = "Member Service No" });
            itemList.Add(new ExtJsDataRowListItem { id = 4, value = "CTM_counter" });
            itemList.Add(new ExtJsDataRowListItem { id = 5, value = "Service Plan No" });
            itemList.Add(new ExtJsDataRowListItem { id = 6, value = "Amount" });
            itemList.Add(new ExtJsDataRowListItem { id = 7, value = "Paid Amount" });
            itemList.Add(new ExtJsDataRowListItem { id = 8, value = "Payment Status" });
            itemList.Add(new ExtJsDataRowListItem { id = 9, value = "Payment Method" });
            itemList.Add(new ExtJsDataRowListItem { id = 10, value = "Payment Date" });
            itemList.Add(new ExtJsDataRowListItem { id = 11, value = "Transaction Date" });

            return itemList.ToJson();
        }

        public string GetImportJobType()
        {
            var itemList = new List<ExtJsDataRowListItem> { };

            itemList.Add(new ExtJsDataRowListItem { id = 1, value = "Transaction" });
            itemList.Add(new ExtJsDataRowListItem { id = 2, value = "Member" });
           
            return itemList.ToJson();
        }

        public string GetScheduleInterval()
        {
            var itemList = new List<ExtJsDataRowListItem> { };

            itemList.Add(new ExtJsDataRowListItem { id = 1, value = "Every Day" });
            itemList.Add(new ExtJsDataRowListItem { id = 2, value = "Every Week" });
            itemList.Add(new ExtJsDataRowListItem { id = 3, value = "Every Month" });
           
            return itemList.ToJson();
        }

        public string GetFileType()
        {
            var itemList = new List<ExtJsDataRowListItem> { };

            itemList.Add(new ExtJsDataRowListItem { id = 1, value = ".xls" });
            itemList.Add(new ExtJsDataRowListItem { id = 2, value = ".xlsx" });
            itemList.Add(new ExtJsDataRowListItem { id = 3, value = ".csv" });
            itemList.Add(new ExtJsDataRowListItem { id = 4, value = ".xml" });

            return itemList.ToJson();
        }

        public string Update(FormCollection collection)
        {
            var name = collection["name"];
            var start_date_str = collection["start_date"];
            var end_date_str = collection["end_date"];
            var status_str = collection["status"];
            var type_str = collection["type"];
            var transaction_criteria_str = collection["transaction_criteria"];
            var conjunction_str = collection["conjunction"];
            var member_level_str = collection["member_level"];
            var member_category_str = collection["member_category"];
            var special_criteria_type_str = collection["special_criteria_type"];
            var redeem_discount_str = collection["redeem_discount"];
            var purchase_earn_point_str = collection["purchase_earn_point"];
            var purchase_earn_point_value_str = collection["purchase_earn_point_value"];
            var comp_earn_point_str = collection["comp_earn_point"];
            var gift_id_str = collection["gift"];
            var gift_quantity_str = collection["gift_quantity"];
            var purchase_target_type_str = collection["purchase_target_type"];
            var purchase_target_id_str = collection["purchase_target_id"];
            var purchase_target_criteria_str = collection["purchase_target_criteria"];
            var purchase_target_value_str = collection["purchase_target_value"];

            var service_target_id_str = collection["service_target_id"];
            var service_target_criteria_str = collection["service_target_criteria"];
            var service_target_value_str = collection["service_target_value"];

            DateTime? start_date = null;
            DateTime? end_date = null;
            if (start_date_str.Length == 16)
                start_date = DateTime.ParseExact(start_date_str, "yyyy-MM-dd HH:mm", null);
            if (end_date_str.Length == 16)
                end_date = DateTime.ParseExact(end_date_str, "yyyy-MM-dd HH:mm", null);

            int type = 0;
            if (!String.IsNullOrEmpty(type_str))
                type = int.Parse(type_str);

            int conjunction = 0;
            if (!String.IsNullOrEmpty(conjunction_str))
                conjunction = int.Parse(conjunction_str);

            int? transaction_criteria = null;
            if (!String.IsNullOrEmpty(transaction_criteria_str))
                transaction_criteria = int.Parse(transaction_criteria_str);

            int? special_criteria_type = null;
            if (!String.IsNullOrEmpty(special_criteria_type_str))
                special_criteria_type = int.Parse(special_criteria_type_str);

            int? special_criteria_value = null;
            if (special_criteria_type >= 1 && special_criteria_type <= 3)
            { 
                if (!String.IsNullOrEmpty(special_criteria_type_str))
                    special_criteria_value = int.Parse(special_criteria_type_str);
            }

            int? earn_point_type = null;
            int? earn_point_value = null;

            if (type == (int)CommonConstant.PromotionRuleType.purchase)
            {
                if (!String.IsNullOrEmpty(purchase_earn_point_str)) 
                {
                    //avoid extjs passing in the irrelevant string
                    try
                    {
                        earn_point_type = int.Parse(purchase_earn_point_str);
                    }
                    catch (Exception e)
                    {
                        earn_point_type = null;
                    }
                }
                
                if (!String.IsNullOrEmpty(purchase_earn_point_value_str))
                {
                    //avoid extjs passing in the irrelevant string
                    try
                    {
                        earn_point_value = int.Parse(purchase_earn_point_value_str);
                    }
                    catch (Exception e)
                    {
                        earn_point_value = null;
                    }
                }
            }
            else if (type == (int)CommonConstant.PromotionRuleType.complementary)
            {

                if (!String.IsNullOrEmpty(comp_earn_point_str))
                {
                    //avoid extjs passing in the irrelevant string
                    try
                    {
                        earn_point_value = int.Parse(comp_earn_point_str);
                    }
                    catch (Exception e)
                    {
                        earn_point_value = null;
                    }
                }
            }
            else if (type == (int)CommonConstant.PromotionRuleType.servicePayment)
            {
                int num = 0;
                int.TryParse(purchase_earn_point_value_str, out num);
                earn_point_value = num;
            }
      

            int? earn_gift_id = null;
            int? earn_gift_quantity = null;
            int? redeem_discount = null;
            if (type == (int)CommonConstant.PromotionRuleType.purchase || type == (int)CommonConstant.PromotionRuleType.complementary)
            {
                if (!String.IsNullOrEmpty(gift_id_str) && gift_id_str!= "Please Select")
                    earn_gift_id = int.Parse(gift_id_str);

                if (!String.IsNullOrEmpty(gift_quantity_str) && gift_quantity_str != "quantity")
                    earn_gift_quantity = int.Parse(gift_quantity_str);
            }
            else if (type == (int)CommonConstant.PromotionRuleType.redeem)
            {
                if (!String.IsNullOrEmpty(redeem_discount_str))
                    redeem_discount = int.Parse(redeem_discount_str);
            }

            int status = CommonConstant.Status.inactive;
            if (!String.IsNullOrEmpty(status_str))
                status = int.Parse(status_str);

            // member_level
            var member_level_list = new List<MemberLevelObject>();
            var member_level_arr = member_level_str.Split(',');

            foreach (var x in member_level_arr)
            {
                var member_level = new MemberLevelObject()
                {
                    level_id = int.Parse(x)
                };

                member_level_list.Add(member_level);
            }

            // member_category
            var member_category_list = new List<MemberCategoryObject>();
            if (!String.IsNullOrEmpty(member_category_str))
            {
                var member_category_arr = member_category_str.Split(',');
                
                foreach (var x in member_category_arr)
                {
                    var member_category = new MemberCategoryObject()
                    {
                        category_id = int.Parse(x)
                    };

                    member_category_list.Add(member_category);
                }
            }

            // purchase criteria
            var pruchaseCriteriaList = new List<PromotionRulePurchaseCriteriaObject>();

            if (!String.IsNullOrEmpty(purchase_target_type_str))
            {
                var purchase_target_type_arr = purchase_target_type_str.Split(',');
                var purchase_target_id_arr = purchase_target_id_str.Split(',');
                var purchase_target_criteria_arr = purchase_target_criteria_str.Split(',');
                var purchase_target_value_arr = purchase_target_value_str.Split(',');

                for (int i = 0; i < purchase_target_type_arr.Count(); i++)
                {
                    var target_id = 0;
                    if (purchase_target_id_arr[i] != "Any Product")
                        target_id = int.Parse(purchase_target_id_arr[i]);

                    var criteria = int.Parse(purchase_target_criteria_arr[i]);
                    double? point = null;
                    int? quantity = null;

                    if (criteria == (int)CommonConstant.PromotionRulePurchaseProductCriteriaType.point)
                        point = double.Parse(purchase_target_value_arr[i]);
                    else // quantity
                        quantity = int.Parse(purchase_target_value_arr[i]);

                    var criteriaObject = new PromotionRulePurchaseCriteriaObject()
                    {
                        target_type = int.Parse(purchase_target_type_arr[i]),
                        target_id = target_id,
                        criteria = criteria,
                        point = point,
                        quantity = quantity
                    };

                    pruchaseCriteriaList.Add(criteriaObject);
                }
            }

            // service 
            var serviceCriteriaList = new List<PromotionRuleServiceCriteriaObject>();

            if (!String.IsNullOrEmpty(service_target_id_str))
            {
                var service_target_id_arr = service_target_id_str.Split(',');
                var service_target_criteria_arr = service_target_criteria_str.Split(',');
                var service_target_value_arr = service_target_value_str.Split(',');

                for (int i = 0; i < service_target_id_arr.Count(); i++)
                {
                    var criteriaObject = new PromotionRuleServiceCriteriaObject()
                    {

                        service_category_id = int.Parse(service_target_id_arr[i]),
                        criteria_type = int.Parse(service_target_criteria_arr[i]),
                        criteria_value = double.Parse(service_target_value_arr[i])
                    };

                    serviceCriteriaList.Add(criteriaObject);
                }
            }

            var ruleObject = new PromotionRuleObject()
            {
                name = name,
                start_date = start_date,
                end_date = end_date,
                type = type,
                conjunction = conjunction,
                transaction_criteria = transaction_criteria,
                special_criteria_type = special_criteria_type,
                special_criteria_value = special_criteria_value,
                earn_point_type = earn_point_type,
                earn_point_value = earn_point_value,
                earn_gift_id = earn_gift_id,
                earn_gift_quantity = earn_gift_quantity,
                redeem_discount = redeem_discount,
                status = status,
                member_level_list = member_level_list,
                member_category_list = member_category_list,
                purchase_criteria_list = pruchaseCriteriaList,
                service_criteria_list = serviceCriteriaList,
                
            };

            var result = false;
            var msg = "";

            if ((earn_point_type == null || earn_point_value == null)
                && (earn_gift_id == null || earn_gift_quantity == null)
                )
            {
                msg = "Add Failed: Please input earn point or gift";
            }
            else
            {
                var ruleManger = new PromotionRuleManager();
                var systemCode = ruleManger.Create(ruleObject);

                if (systemCode == CommonConstant.SystemCode.normal)
                {
                    result = true;
                    msg = "Add Success";
                }
                else if (systemCode == CommonConstant.SystemCode.no_permission)
                    msg = "Add Failed: No Permission";
                else
                    msg = "Add Failed";
            }
           
            return new {success = result, msg = msg }.ToJson();
        }

        public string ViewBasicData()
        {
            var rule_id = _id;

            var systemCode = CommonConstant.SystemCode.undefine;
            int current_stock = 0;
            int redeem_count = 0;

            var promotionRuleManager = new PromotionRuleManager();
            var r = promotionRuleManager.GetDetail(rule_id);

            //basic table data
            var extTable = new ExtJsTable
            {
                column = 2,
                post_url = "",
                post_header = "",
                title = "Detail",
                icon = "iconRemarkList",
                //post_params = Url.Action("Perform"),
                isType = true,
                //button_text = "Save",     //no neet button
                //button_icon = "iconSave",
                value_changes = true,
            };

            var rowFieldInput_str = new ExtJsFieldLabelInput<string>(PayloadKeys.Dummy, r.rule_id.ToString())
            {
                fieldLabel = "Rule ID",
                type = "input",
                colspan = 2,
                tabIndex = 1,
                allowBlank = false,
                readOnly = true
            };
            extTable.AddFieldLabelToRow(rowFieldInput_str);

            rowFieldInput_str = new ExtJsFieldLabelInput<string>(PayloadKeys.Dummy, r.name)
            {
                fieldLabel = "Rule Name",
                type = "input",
                colspan = 2,
                tabIndex = 1,
                allowBlank = false,
                readOnly = true
            };
            extTable.AddFieldLabelToRow(rowFieldInput_str);

            rowFieldInput_str = new ExtJsFieldLabelInput<string>(PayloadKeys.Dummy, r.type_name)
            {
                fieldLabel = "Rule Type",
                type = "input",
                colspan = 2,
                tabIndex = 1,
                allowBlank = false,
                readOnly = true
            };
            extTable.AddFieldLabelToRow(rowFieldInput_str);

            var conjunction_name = "";
            if (r.conjunction == 1)
                conjunction_name = "Yes";
            else
                conjunction_name = "No";

            rowFieldInput_str = new ExtJsFieldLabelInput<string>(PayloadKeys.Dummy, r.start_date == null ? "forever" : r.start_date.Value.ToString("yyyy-MM-dd HH:mm:ss"))
            {
                fieldLabel = "Start Date",
                type = "input",
                colspan = 2,
                tabIndex = 1,
                allowBlank = false,
                readOnly = true
            };
            extTable.AddFieldLabelToRow(rowFieldInput_str);

            rowFieldInput_str = new ExtJsFieldLabelInput<string>(PayloadKeys.Dummy, r.end_date == null ? "forever" :  r.end_date.Value.ToString("yyyy-MM-dd HH:mm:ss"))
            {
                fieldLabel = "End Date",
                type = "input",
                colspan = 2,
                tabIndex = 1,
                allowBlank = false,
                readOnly = true
            };
            extTable.AddFieldLabelToRow(rowFieldInput_str);

            if (r.type == (int)CommonConstant.PromotionRuleType.purchase)
            {
                var transactionCriteriaName = "";
                if (r.transaction_criteria == (int)CommonConstant.PromotionRuleTransactionCriteria.singleTransaction)
                    transactionCriteriaName = "Single Transaction";
                else if (r.transaction_criteria == (int)CommonConstant.PromotionRuleTransactionCriteria.multiTransaction)
                    transactionCriteriaName = "Multi Transaction";

                rowFieldInput_str = new ExtJsFieldLabelInput<string>(PayloadKeys.Dummy, transactionCriteriaName)
                {
                    fieldLabel = "Transaction Criteria",
                    type = "input",
                    colspan = 2,
                    tabIndex = 1,
                    allowBlank = false,
                    readOnly = true
                };
                extTable.AddFieldLabelToRow(rowFieldInput_str);
            }

            rowFieldInput_str = new ExtJsFieldLabelInput<string>(PayloadKeys.Dummy, conjunction_name)
            {
                fieldLabel = "Hit in conjunction with other rules",
                type = "input",
                colspan = 2,
                tabIndex = 1,
                allowBlank = false,
                readOnly = true
            };
            extTable.AddFieldLabelToRow(rowFieldInput_str);

            var promotionRuleMemberLevelManager = new PromotionRuleMemberLevelManager();
            var level_list = promotionRuleMemberLevelManager.GetList(r.rule_id);
            var all_level_name = new List<string> { };

            foreach (var l in level_list)
            {
                all_level_name.Add(l.member_level_name);
            }

            rowFieldInput_str = new ExtJsFieldLabelInput<string>(PayloadKeys.Dummy, string.Join(", ", all_level_name.ToArray()))
            {
                fieldLabel = "Member Level",
                type = "input",
                colspan = 2,
                tabIndex = 1,
                allowBlank = false,
                readOnly = true
            };
            extTable.AddFieldLabelToRow(rowFieldInput_str);

            var promotionRuleMemberCategoryManager = new PromotionRuleMemberCategoryManager();
            var cat_list = promotionRuleMemberCategoryManager.GetList(r.rule_id);
            var all_cat_name = new List<string> { };
            foreach (var l in cat_list)
            {
                all_cat_name.Add(l.member_category_name);
            }

            rowFieldInput_str = new ExtJsFieldLabelInput<string>(PayloadKeys.Dummy, string.Join(", ", all_cat_name.ToArray()))
            {
                fieldLabel = "Member Category",
                type = "input",
                colspan = 2,
                tabIndex = 1,
                allowBlank = false,
                readOnly = true
            };
            extTable.AddFieldLabelToRow(rowFieldInput_str);

         

            var b_str = "NO";
            if (r.rule_id == 3)
                b_str = "Yes (Birthday Month)";

            rowFieldInput_str = new ExtJsFieldLabelInput<string>(PayloadKeys.Dummy, b_str)
            {
                fieldLabel = "Special Criteria: Birthday",
                type = "input",
                colspan = 2,
                tabIndex = 1,
                allowBlank = false,
                readOnly = true
            };
            extTable.AddFieldLabelToRow(rowFieldInput_str);

            if (r.rule_id == 3)
            {
                rowFieldInput_str = new ExtJsFieldLabelInput<string>(PayloadKeys.Dummy, "Any Product (Quantity:1)")
                {
                    fieldLabel = "Purchase",
                    type = "input",
                    colspan = 2,
                    tabIndex = 1,
                    allowBlank = false,
                    readOnly = true
                };
                extTable.AddFieldLabelToRow(rowFieldInput_str);
            }

            var earn_point_type_name = "";
            var earn_point_value = "";
            if (r.earn_point_type == (int)CommonConstant.PromotionRuleEarnPointType.discrete)
            {
                earn_point_type_name = "Discrete";
                earn_point_value = r.earn_point_value.ToString() + " points";
            }
            else // bonus percent
            {
                earn_point_type_name = "Bonus Point Percent";
                earn_point_value = r.earn_point_value.ToString() + "%";
            }

            rowFieldInput_str = new ExtJsFieldLabelInput<string>(PayloadKeys.Dummy, earn_point_type_name)
            {
                fieldLabel = "Earn Point Type",
                type = "input",
                colspan = 2,
                tabIndex = 1,
                allowBlank = false,
                readOnly = true
            };
            extTable.AddFieldLabelToRow(rowFieldInput_str);

            rowFieldInput_str = new ExtJsFieldLabelInput<string>(PayloadKeys.Dummy, earn_point_value)
            {
                fieldLabel = "Earn Point Value",
                type = "input",
                colspan = 2,
                tabIndex = 1,
                allowBlank = false,
                readOnly = true
            };
            extTable.AddFieldLabelToRow(rowFieldInput_str);

            rowFieldInput_str = new ExtJsFieldLabelInput<string>(PayloadKeys.Dummy, r.status_name)
            {
                fieldLabel = "Status",
                type = "input",
                colspan = 2,
                tabIndex = 1,
                allowBlank = false,
                readOnly = true
            };
            extTable.AddFieldLabelToRow(rowFieldInput_str);

            return extTable.ToJson();
        }

        public string ServiceHeader()
        {
            var title = "";
            var columns = new List<HeaderColumn>();
            string[] fields = new string[] { };


            title = "Service Criteria";
            columns.Add(new HeaderColumn() { header = "Service Type", dataIndex = "type_name", width = 140, type = "displayfield", allowBlank = true });
            columns.Add(new HeaderColumn() { header = "Rule Criteria", dataIndex = "criteria_name", width = 140, type = "displayfield", allowBlank = true });
            columns.Add(new HeaderColumn() { header = "Amount", dataIndex = "amount", width = 80, type = "displayfield", allowBlank = true });

            fields = new string[] { "id", "type_name", "criteria_name", "amount"};
          
            var result = new
            {
                success = true,
                columns = columns,
                fields = fields,
                title = title,
                pageSize = 20,
                delete_url = "",
                update_url = "../BasicRule/Update",
                icon = "iconPayment",
                add_hidden = true,
                delete_hidden = true
            };

            return result.ToJson();
        }

        public string ServiceData()
        {
            var rule_id = _id;

            object items;
            var promotionRuleServiceCriteriaManager = new PromotionRuleServiceCriteriaManager();
            int totalCount = 0;

            var list = promotionRuleServiceCriteriaManager.GetList(rule_id);

            var resultDataList = list.Select(
                x => new
                {
                    id = x.criteria_id,
                    type_name = x.service_category_name,
                    criteria_name = x.criteria_type_name,
                    amount = x.criteria_value
                }
            );

            totalCount = list.Count();

            items = resultDataList;
          
            var result = new
            {
                success = true,
                items = items,
                totalCount = totalCount
            };

            return result.ToJson();
        }

        public string PurchaseHeader()
        {
            var title = "";
            var columns = new List<HeaderColumn>();
            string[] fields = new string[] { };

            title = "Purchase Criteria";
            columns.Add(new HeaderColumn() { header = "Product", dataIndex = "target_name", width = 140, type = "displayfield", allowBlank = true });
            columns.Add(new HeaderColumn() { header = "Rule Criteria", dataIndex = "criteria_name", width = 140, type = "displayfield", allowBlank = true });
            columns.Add(new HeaderColumn() { header = "Amount", dataIndex = "amount", width = 80, type = "displayfield", allowBlank = true });

            fields = new string[] { "id", "target_name", "criteria_name", "amount" };

            var result = new
            {
                success = true,
                columns = columns,
                fields = fields,
                title = title,
                pageSize = 20,
                delete_url = "",
                update_url = "../BasicRule/Update",
                icon = "iconPayment",
                add_hidden = true,
                delete_hidden = true
            };

            return result.ToJson();
        }

        public string PurchaseData()
        {
            var rule_id = _id;

            object items;
            var promotionRulePurchaseProductCriteriaManager = new PromotionRulePurchaseProductCriteriaManager();
            int totalCount = 0;

            var list = promotionRulePurchaseProductCriteriaManager.GetList(rule_id);

            var resultDataList = list.Select(
                x => new
                {
                    id = x.rec_id,
                    target_name = x.target_name,
                    criteria_name = (x.criteria == (int)CommonConstant.PromotionRulePurchaseProductCriteriaType.point? "Point" : "Quantity"),
                    amount = (x.quantity?? x.point)
                }
            );

            totalCount = list.Count();

            items = resultDataList;

            var result = new
            {
                success = true,
                items = items,
                totalCount = totalCount
            };

            return result.ToJson();
        }      
    }

     


    public class HeaderColumn
    {
        public string header;
        public string dataIndex;
        public int width;
        public string type;
        public bool allowBlank;
        public string url;
    }
}