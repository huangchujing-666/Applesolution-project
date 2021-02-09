using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Web.Routing;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.ExtJsEntity;
using Palmary.Loyalty.Web_backend.Modules.GlobalHandlers;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.PayoadKey;
using Palmary.Loyalty.Web_backend.Modules.Utility;
using Palmary.Loyalty.Web_backend.Modules.ExtJS;

using Palmary.Loyalty.Web_backend;
using Palmary.Loyalty.Common;
using Palmary.Loyalty.BO.DataTransferObjects.Security;
using Palmary.Loyalty.BO.DataTransferObjects.Rule;
using Palmary.Loyalty.BO.Modules.Rule;
using Palmary.Loyalty.BO.Modules.Service;
using Palmary.Loyalty.BO.Modules.Product;

namespace Palmary.Loyalty.Web_backend.Controllers.Rule
{
    [Authorize]
    public class BasicRuleController : Controller
    {
        private string _module;

        public BasicRuleController()
        {
        }

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            if (RouteData.Values["Module"] != null)
            {
                _module = RouteData.Values["Module"].ToString().ToLower();
            }

            SystemHandler.InitSystemParameters(this.HttpContext.ApplicationInstance as MvcApplication);
        }

        public string GetHeader()
        {
            var title = "";
            var columns = new List<HeaderColumn>();
            string[] fields = new string[]{};
            var type = 0;

            if (_module == "purchase")
            {
                title = "Retail Purchase";
              
                columns.Add(new HeaderColumn() { header = "Member Level", dataIndex = "member_level_id", width = 110, type = "combobox", url = "../Table/GetListItems/MemberLevel", allowBlank = false, readOnly = true });
                columns.Add(new HeaderColumn() { header = "Purchase Amount (HKD)", dataIndex = "ratio_payment", width = 170, type = "textfield", allowBlank = false });
                columns.Add(new HeaderColumn() { header = "Earn Point", dataIndex = "ratio_point", width = 120, type = "textfield", allowBlank = false });
                columns.Add(new HeaderColumn() { header = "Point Expiry Month", dataIndex = "point_expiry_month", width = 140, type = "textfield", allowBlank = false });
                columns.Add(new HeaderColumn() { header = "Status", dataIndex = "status", width = 120, type = "combobox", url = "../Table/GetListItems/Status", allowBlank = false, readOnly = true });
                columns.Add(new HeaderColumn() { header = "Remark", dataIndex = "remark", width = 180, type = "textfield", allowBlank = true });

                // hidden
                //columns.Add(new HeaderColumn() { header = "type", dataIndex = "type", width = 180, hidden = true, type = "textfield", allowBlank = true });


                fields = new string[] { "id", "member_level_id", "ratio_payment", "ratio_point", "point_expiry_month", "status", "remark" };
                type = (int)CommonConstant.BasicRuleType.RetailPurchase;
            }
            else if (_module == "postpaid")
            {
                title = "Post Paid Service";
                columns.Add(new HeaderColumn() { header = "Service Plan No", dataIndex = "target_no", width = 140, type = "textfield", allowBlank = false });
                columns.Add(new HeaderColumn() { header = "Member Level", dataIndex = "member_level_id", width = 110, type = "combobox", url = "../Table/GetListItems/MemberLevel", allowBlank = false });
                columns.Add(new HeaderColumn() { header = "Member Category", dataIndex = "member_category_id", width = 140, type = "combobox", url = "../Table/GetListItems/MemberCategory", allowBlank = false });
                columns.Add(new HeaderColumn() { header = "Transfer: Payment", dataIndex = "ratio_payment", width = 140, type = "textfield", allowBlank = false });
                columns.Add(new HeaderColumn() { header = "To: Point", dataIndex = "ratio_point", width = 120, type = "textfield", allowBlank = false });
                columns.Add(new HeaderColumn() { header = "Point Expiry Month", dataIndex = "point_expiry_month", width = 140, type = "textfield", allowBlank = false });
                columns.Add(new HeaderColumn() { header = "Status", dataIndex = "status", width = 120, type = "combobox", url = "../Table/GetListItems/Status", allowBlank = false });
                columns.Add(new HeaderColumn() { header = "Remark", dataIndex = "remark", width = 180, type = "textfield", allowBlank = true });

                // hidden
               // columns.Add(new HeaderColumn() { header = "type", dataIndex = "type", width = 180, hidden = true, type = "textfield", allowBlank = true });

                fields = new string[] { "id", "target_no", "member_level_id", "member_category_id", "ratio_payment", "ratio_point", "point_expiry_month", "status", "remark", "target_id" };
                type = (int)CommonConstant.BasicRuleType.PostPaidService;
            }
            //else if (_module == "prepaid")
            //{
            //    title = "Pre Paid Service";
            //    columns.Add(new HeaderColumn() { header = "Type", dataIndex = "type_name", width = 140, type = "displayfield", allowBlank = true });
            //    columns.Add(new HeaderColumn() { header = "Service Category", dataIndex = "service_category_name", width = 140, type = "displayfield", allowBlank = true });
            //    columns.Add(new HeaderColumn() { header = "Member Level", dataIndex = "member_level_name", width = 80, type = "displayfield", allowBlank = true });
            //    columns.Add(new HeaderColumn() { header = "Usage (MOP)", dataIndex = "payment", width = 140, type = "textfield", allowBlank = false });
            //    columns.Add(new HeaderColumn() { header = "To: Point", dataIndex = "point", width = 120, type = "textfield", allowBlank = false });
            //    columns.Add(new HeaderColumn() { header = "Point Expiry Month", dataIndex = "point_expiry_month", width = 120, type = "textfield", allowBlank = false });
            //    columns.Add(new HeaderColumn() { header = "Status", dataIndex = "status", width = 120, type = "combobox", url = "../Table/GetListItems/Status", allowBlank = false });
            //    columns.Add(new HeaderColumn() { header = "Remark", dataIndex = "remark", width = 180, type = "textfield", allowBlank = true });

            //    fields = new string[] { "id", "service_category_name", "member_level_name", "type_name", "payment", "point", "point_expiry_month", "status", "remark" };
            //}
            
            var result = new
            {
                success = true,
                columns = columns,
                fields = fields,
                title = title,
                pageSize = 20,
                delete_url = "",
                update_url = "../BasicRule/Update?type=" + type,
                icon = "iconPayment",
                add_hidden = true,
                delete_hidden = true
            };

            return result.ToJson();
        }

        public string GetData()
        {
            object items;
            var basicRuleManager = new BasicRuleManager();
            int totalCount = 0;
            
            if (_module == "purchase")
            {
                var systemCode = CommonConstant.SystemCode.undefine;
                var list = basicRuleManager.GetList((int)CommonConstant.BasicRuleType.RetailPurchase, ref systemCode);

                var resultDataList = list.Select(
                   x => new
                   {
                       id = x.basic_rule_id,
                       type_name = x.type_name,
                       target_id = x.target_id,
                      // target_no = x.target_no,
                       member_level_id = x.member_level_id,
                       ratio_payment = x.ratio_payment,

                       ratio_point = x.ratio_point,
                       point_expiry_month = x.point_expiry_month,
                       status = x.status,
                       remark = x.remark,

                       type = x.type
                   }
               );

                totalCount = list.Count();

                items = resultDataList;
            }
            else if (_module == "postpaid")
            {
                var systemCode = CommonConstant.SystemCode.undefine;
                var list = basicRuleManager.GetList((int)CommonConstant.BasicRuleType.PostPaidService, ref systemCode);

                var resultDataList = list.Select(
                   x => new
                   {
                       id = x.basic_rule_id,
                       type_name = x.type_name,
                       target_id = x.target_id,
                       target_no = x.target_no,
                       member_level_id = x.member_level_id,
                       member_category_id = x.memebr_category_id,
                       
                       ratio_payment = x.ratio_payment,
                       ratio_point = x.ratio_point,
                       point = x.point,
                       point_expiry_month = x.point_expiry_month,
                       status = x.status,
                       remark = x.remark,

                       type = x.type
                   }
                );

                totalCount = list.Count();
                items = resultDataList;
            }
            //else if (_module == "prepaid")
            //{
            //    var systemCode = CommonConstant.SystemCode.undefine;
            //    var list = basicRuleManager.GetList_PrePaid(ref systemCode);

            //    var resultDataList = list.Select(
            //       x => new
            //       {
            //           id = x.basic_rule_id,
            //           type_name = x.type_name,
            //           service_category_name = x.service_category_name,
            //           member_level_name = x.member_level_name,
            //           payment = x.payment,
            //           point = x.point,
            //           point_expiry_month = x.point_expiry_month,
            //           status = x.status,
            //           remark = x.remark
            //       }
            //   );

            //    totalCount = list.Count();

            //    items = resultDataList;
            //}
            else
                items = new List<BasicRuleObject>();

            var result = new
            {
                success = true,
                items = items,
                totalCount = totalCount
            };

            return result.ToJson();
        }

        public string Update(FormCollection collection)
        {
            var id = 0;
            var type = 0;
            var target_no = "";
            int target_id = 0;
            int member_level_id = 0;
            int memebr_category_id = 0;

            double ratio_payment = 0;
            double ratio_point = 0;
            double point = 0;
            var point_expiry_month = 0;
            var status = 0;
            var remark = "";
            
            var validInput = false;
            var systemCode = CommonConstant.SystemCode.undefine;

            try 
            {
                if (collection["id"] != null)
                {
                    id = int.Parse(collection["id"].Trim());
                  
                }
                else
                {
                    id = 0;

                }

                type = int.Parse(Request.QueryString["type"]);
               // target_no = collection["target_no"].Trim();

                member_level_id = int.Parse(collection["member_level_id"].Trim());
               // memebr_category_id = int.Parse(collection["member_category_id"].Trim());

                if (type == (int)CommonConstant.BasicRuleType.RetailPurchase)
                {
                    var x = 1;
                    ratio_payment = double.Parse(collection["ratio_payment"].Trim());
                    ratio_point = double.Parse(collection["ratio_point"].Trim());


                    point_expiry_month = int.Parse(collection["point_expiry_month"].Trim());
                    status = int.Parse(collection["status"].Trim());
                    remark = collection["remark"].Trim();

                    //var productManager = new ProductManager();
                    //var p = productManager.GetProductID(1, target_no, ref target_id);

                    if (ratio_payment > 0 && ratio_point > 0 && point_expiry_month>0)
                        validInput = true;
                }
                else if (type == (int)CommonConstant.BasicRuleType.PostPaidService)
                {
                    var a = collection["ratio_payment"];
                    ratio_payment = double.Parse(collection["ratio_payment"].Trim());
                    ratio_point = double.Parse(collection["ratio_point"].Trim());

                    var servicePlanManager = new ServicePlanManager();
                    var theSystemCode = CommonConstant.SystemCode.undefine;
                    var s = servicePlanManager.GetDetail_byServicePlanNo(target_no, ref theSystemCode);
                    if (theSystemCode == CommonConstant.SystemCode.normal)
                        target_id = s.plan_id;
                }
              

                
            }
            catch(Exception e)
            {
                validInput = false;
            }

            if (validInput)
            {
                var basicRuleManager = new BasicRuleManager();

                if (id > 0)
                {
                    var rule = basicRuleManager.GetDetail(id, ref systemCode);
                    rule.target_id = target_id;
                    rule.member_level_id = member_level_id;
                    rule.memebr_category_id = memebr_category_id;
                    rule.ratio_payment = ratio_payment;
                    rule.ratio_point = ratio_point;
                    rule.point = point;
                    rule.point_expiry_month = point_expiry_month;
                    rule.status = status;
                    rule.remark = remark;

                    systemCode = basicRuleManager.Update(rule);
                }
                else
                {
                    var rule = new BasicRuleObject()
                    {
                        type = type,
                        target_id = target_id,
                        member_level_id = member_level_id,
                        memebr_category_id = memebr_category_id,
                        ratio_payment = ratio_payment,
                        ratio_point = ratio_point,
                        point = point,
                        point_expiry_month = point_expiry_month,
                        status = status,
                        remark = remark
                    };

                    systemCode = basicRuleManager.Create(rule);
                }
            }

            var result = "";

            if (systemCode == CommonConstant.SystemCode.normal)
            {
                result = new { success = true, msg = "Update Success" }.ToJson();
            }
            else
            {
                var msg = "Update Fail";

                if (!validInput)
                {
                    if (type == (int)CommonConstant.BasicRuleType.PostPaidService && target_id ==0)
                        msg = "Invalid Service Plan No";
                    else if (type == (int)CommonConstant.BasicRuleType.RetailPurchase && target_id ==0)
                        msg = "Invalid Input";
                    else
                        msg = "Invalid Input";
                }

                result = new { success = true, msg = msg }.ToJson();
            }

            return result;
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
        public bool readOnly;
        public bool hidden;
    }
}
