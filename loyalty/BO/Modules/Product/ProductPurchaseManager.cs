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
using Palmary.Loyalty.BO.DataTransferObjects.Transaction;
using Palmary.Loyalty.BO.DataTransferObjects.Member;
using Palmary.Loyalty.BO.Modules.Administration.Security;
using Palmary.Loyalty.BO.Modules.Transaction;
using Palmary.Loyalty.BO.Modules.Administration.SystemControl;
using Palmary.Loyalty.BO.Modules.Member;
using Palmary.Loyalty.BO.Modules.Passcode;
using Palmary.Loyalty.BO.Modules.PromotionRule;
using Palmary.Loyalty.BO.Modules.Utility;
using Palmary.Loyalty.BO.DataTransferObjects.DB;
using Palmary.Loyalty.BO.Modules.Rule;

namespace Palmary.Loyalty.BO.Modules.Product
{
    public class ProductPurchaseManager
    {
        private static LoyaltyDBDataContext db = new LoyaltyDBDataContext();
        private AccessManager _accessManager;
        private string _module = CommonConstant.Module.productPurchase;
        private RolePrivilegeObject _privilege;
        private static LogManager _logManager;
        private AccessObject _accessObject;

         // For backend, using BO Session to access
        public ProductPurchaseManager()
        {
            // access object
            _accessManager = new AccessManager();
            _accessObject = _accessManager.GetAccessObjectBySession();

            // privilege and log
            _privilege = _accessManager.AccessModule(_module);
            _logManager = new LogManager();
        }

        // For Webservices or other not using BO Session to access
        public ProductPurchaseManager(AccessObject accessObject)
        {
            // access object
            _accessManager = new AccessManager();
            _accessObject = accessObject;

            // privilege and log
            _privilege = _accessManager.AccessModule(_module, _accessObject.type, _accessObject.id);
            _logManager = new LogManager(_accessObject);
        }

        // purchase controller should call Purchase(), not Create()
        private CommonConstant.SystemCode Create(
            ProductPurchaseObject purchaseObject,
            ref int? new_purchase_id
        )
        {
            int? sql_result = 0;
            var system_code = CommonConstant.SystemCode.undefine;

            if (_privilege.insert_status == 1 )
            {

                if (purchaseObject.purchase_date.Year == 1) //default datetime year value is 1
                {
                    purchaseObject.purchase_date = DateTime.Now;
                }
                
                db.sp_CreateProductPurchase(
                    _accessObject.id,
                    _accessObject.type, 
                    purchaseObject.order_id,
                    purchaseObject.transaction_id,
                    purchaseObject.point,
                    purchaseObject.promotion_transaction_id,
                    purchaseObject.member_id,
                    purchaseObject.product_id,
                    purchaseObject.quantity,
                    purchaseObject.total_amount,
                    purchaseObject.purchase_date,

                    purchaseObject.status,

                    ref new_purchase_id,
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

        public CommonConstant.SystemCode Purchase(
          string memberNo, string productNo, double totalAmount, DateTime purchaseDate)
        { 
          var systemCode = CommonConstant.SystemCode.undefine;

          if (_privilege.insert_status == 1)
          {
              var memberManager = new MemberManager(_accessObject);
              var member = memberManager.GetDetail_byMemberNo(memberNo, ref systemCode);

              if (systemCode == CommonConstant.SystemCode.normal)
              {
                  var productManager = new ProductManager(_accessObject);
                  var product = productManager.GetDetailByProductNo(productNo, false, ref systemCode);

                  if (member.member_id > 0 && product.product_id > 0)
                  {
                      var purchaseList = new List<ProductPurchaseObject>();

                      var productPurchaseObject = new ProductPurchaseObject()
                      {
                          member_id = member.member_id,
                          product_id = product.product_id,
                          quantity = 1,
                          status = CommonConstant.Status.active,
                          total_amount = totalAmount
                      };

                      purchaseList.Add(productPurchaseObject);

                      Purchase(purchaseList);
                  }
              }
              
          }
          return systemCode;
        }

        public CommonConstant.SystemCode Purchase( // purchase list from same member id
            List<ProductPurchaseObject> purchaseList
        )
        {
            var system_code = CommonConstant.SystemCode.undefine;

            if (_privilege.insert_status == 1 && purchaseList.Count>0)
            {
                int? new_purchase_id = 0;

                double point_earn = 0;
                
                var productManager = new ProductManager(_accessObject);
                var memberManager = new MemberManager(_accessObject);
                var member = memberManager.GetDetail(purchaseList[0].member_id, true, ref system_code);
                var sql_result_bool = false;

                // get basic rule data
                var systemCode = CommonConstant.SystemCode.undefine;
                var basicRuleManager = new BasicRuleManager();
                var list = basicRuleManager.GetList((int)CommonConstant.BasicRuleType.RetailPurchase, ref systemCode);
                var basicRule = list.Select(
                   x => new
                   {
                       id = x.basic_rule_id,
                       type_name = x.type_name,
                       target_id = x.target_id,
                       target_no = x.target_no,
                       member_level_id = x.member_level_id,
                       ratio_payment = x.ratio_payment,
                       ratio_point = x.ratio_point,
                       point_expiry_month = x.point_expiry_month,
                       status = x.status,
                       remark = x.remark,
                       type = x.type
                   }
               ).Where(t => t.member_level_id == member.member_level_id).FirstOrDefault();

                // calculate earn point
                foreach (var x in purchaseList)
                {
                    var product = productManager.GetProductDetailBy(SessionManager.Current.obj_id, x.product_id, null, ref sql_result_bool);

                    if (product.point > 0) // use product point
                    {
                        x.point_earned = product.point * x.quantity;
                    }
                    else // use basic rule
                    {
                        if (x.total_amount == 0)
                            x.total_amount = product.price * x.quantity;
                        x.point_earned = x.total_amount / basicRule.ratio_payment * basicRule.ratio_point;
                    }
                    
                    x.point = x.point_earned;
                    point_earn += x.point_earned;
                }

                var remark = "";
                var systemConfigManager = new SystemConfigManager(_accessObject);
                var point_expiry_month = int.Parse(systemConfigManager.GetSystemConfigValue("point_expiry_month", ref remark));
                var point_expiry_date = DateTime.Now.AddMonths(point_expiry_month);
                
                // create parent transaction - add point
                var transactionManager = new TransactionManager(_accessObject);
                
                var purchase_location_id = 0;
                var source_id = 0;
                int? new_transaction_id = 0;
                var point_status = (int)CommonConstant.PointStauts.realized;

                var result = transactionManager.AddPoint(purchase_location_id, purchaseList[0].member_id, point_earn, point_status, point_expiry_date, (int)CommonConstant.TransactionType.purchase_product, source_id, remark, ref new_transaction_id);

                // create purchase records and earn point detail
                if (result)
                {
                    foreach (var x in purchaseList)
                    {
                        // create purchase records
                        x.transaction_id = new_transaction_id.Value;
                        system_code = Create(x, ref new_purchase_id);

                        if (system_code == CommonConstant.SystemCode.normal)
                        {
                            // earn point detail
                            var transactionEarnObject = new TransactionEarnObject()
                            {
                                transaction_id = new_transaction_id.Value,
                                source_id = new_purchase_id.Value,

                                point_earn = x.point_earned,
                                point_status = point_status,
                                point_expiry_date = point_expiry_date,
                                point_used = 0,

                                status = CommonConstant.TransactionStatus.active,
                            };

                            var transactionEarnManager = new TransactionEarnManager(_accessObject);
                            system_code = transactionEarnManager.Create(transactionEarnObject);

                            // check Promotion Rule to earn extra bonus point
                            // Garbo case no needs Promotion Rule
                            //var promotionRuleManager = new PromotionRuleManager(_accessObject);
                            //promotionRuleManager.GiveBonusForMemberPurchase(member.member_id, new_purchase_id.Value); 
                        }
                    }

                    // update member point cache and member level
                    transactionManager.UpgradeLevelAndAvailablePoint(member.member_id);
                    
                }
            }
            else
            {
                system_code = CommonConstant.SystemCode.no_permission;
            }
            return system_code;
        }

        public CommonConstant.SystemCode PurchaseByPasscode( 
            int member_id, string pin_value, int quantity
        )
        {
            // TODO1: need to void passcode in future development
            // for demo no need to void the passcode for easy demo repeatly
            // TODO2: quantity should not >1 in real case

            var systemCode = CommonConstant.SystemCode.undefine;

            var passcodeManager = new PasscodeManager(_accessObject);
            var thePasscode = passcodeManager.GetDetail(pin_value, true, ref systemCode);

            if (thePasscode.passcode_id > 0)
            {
                var purchaseList = new List<ProductPurchaseObject>();

                purchaseList.Add(new ProductPurchaseObject()
                {
                    member_id = member_id,
                    product_id = thePasscode.product_id,
                    quantity = quantity,
                    status = CommonConstant.Status.active
                });

                systemCode = Purchase(purchaseList);
            }
            else
                systemCode = CommonConstant.SystemCode.err_passcodeInvalid;

            return systemCode;
        }

        //public CommonConstant.SystemCode CreateByPasscode(
        //    ProductPurchaseObject dataObject
        //)
        //{
        //    int? sql_result = 0;
        //    var system_code = CommonConstant.SystemCode.undefine;

        //    if (_privilege.insert_status == 1)
        //    {
        //        int? new_purchase_id = 0;
        //        var result = db.sp_CreateProductPurchase(
        //            SessionManager.Current.user_id,

        //            dataObject.order_id,
        //            dataObject.transaction_id,
        //            dataObject.point,
        //            dataObject.promotion_rule_id,
        //            dataObject.member_id,
        //            dataObject.product_id,
        //            dataObject.quantity,
        //            dataObject.status,

        //            ref new_purchase_id,
        //            ref sql_result
        //            );

        //        system_code = (CommonConstant.SystemCode)sql_result.Value;
        //    }
        //    else
        //    {
        //        system_code = CommonConstant.SystemCode.no_permission;
        //    }

        //    return system_code;
        //}

        public CommonConstant.SystemCode Update(ProductPurchaseObject obj, bool rootAccess)
        {
            int? sql_result = 0;
            var systemCode = CommonConstant.SystemCode.undefine;

            if (_privilege.update_status == 1 || rootAccess)
            {
                
                var result = db.sp_UpdateProductPurchase(
           
                    _accessObject.id, 
                    _accessObject.type, 
                    obj.purchase_id,
                    obj.order_id,
                    obj.transaction_id,
                    obj.point,
                    obj.promotion_transaction_id,
                    obj.member_id,
                    obj.product_id,
                    obj.quantity,
                    obj.total_amount,
                    obj.purchase_date,
                    obj.status,
                    
                    ref sql_result);

                systemCode = (CommonConstant.SystemCode)sql_result.Value;
            }
            else
            {
                systemCode = CommonConstant.SystemCode.no_permission;
            }

            return systemCode;
        }

        public List<ProductPurchaseObject>GetList(int member_id, int rowIndexStart, int rowLimit, List<SearchParmObject> searchParmList, string sortColumn, CommonConstant.SortOrder sortOrder, ref CommonConstant.SystemCode systemCode, ref int totalRow)
        {
            systemCode = CommonConstant.SystemCode.undefine;
            List<ProductPurchaseObject> resultList;

            if (_privilege.read_status == 1)
            {
                var query = (from pp in db.product_purchases
                             join p_l in db.product_langs on pp.product_id equals p_l.product_id
                             join p in db.products on pp.product_id equals p.product_id
                             where (
                                pp.record_status != CommonConstant.RecordStatus.deleted
                                && (pp.member_id == member_id)
                                && p_l.lang_id == _accessObject.languageID
                             )
                             select new ProductPurchaseObject
                             {
                                 purchase_id = pp.purchase_id,
                                 order_id = pp.order_id,
                                 transaction_id = pp.transaction_id,
                                 point = pp.point,
                                 promotion_transaction_id = pp.promotion_transaction_id,
                                 member_id = pp.member_id,
                                 product_id = pp.product_id,
                                 quantity = pp.quantity,
                                 status = pp.status,
                                 crt_date = pp.crt_date,
                                 crt_by_type = pp.crt_by_type,
                                 crt_by = pp.crt_by,
                                 upd_date = pp.upd_date,
                                 upd_by_type = pp.upd_by_type,
                                 upd_by = pp.upd_by,
                                 record_status = pp.record_status,

                                 product_name = p_l.name,
                                 product_no = p.product_no
                             });

                // dynamic sort
                var orderByColumn = "";
                if (sortColumn == "product_no"
                    || sortColumn == "product_name"
                    || sortColumn == "quantity"
                    || sortColumn == "crt_date"
                    )
                    orderByColumn = sortColumn;
                else
                { //default
                    sortOrder = CommonConstant.SortOrder.desc;
                    orderByColumn = "crt_date";
                }

                // row total
                totalRow = query.Count();

                if (sortOrder == CommonConstant.SortOrder.desc)
                    resultList = query.OrderBy(orderByColumn, true).Skip(rowIndexStart).Take(rowLimit).ToList();
                else
                    resultList = query.OrderBy(orderByColumn, false).Skip(rowIndexStart).Take(rowLimit).ToList();

                systemCode = CommonConstant.SystemCode.normal;
            }
            else
            {
                resultList = new List<ProductPurchaseObject>();
                systemCode = CommonConstant.SystemCode.no_permission;
            }

            return resultList;
        }

        public List<ProductPurchaseObject> GetListByTransaction(int transaction_id, int rowIndexStart, int rowLimit, string searchParams, ref CommonConstant.SystemCode systemCode, ref int totalRow)
        {
            var query = (from pp in db.product_purchases
                         join p_l in db.product_langs on pp.product_id equals p_l.product_id
                         join p in db.products on pp.product_id equals p.product_id
                         join te in db.transaction_earns on pp.purchase_id equals te.source_id

                         join li_ps in db.listing_items on te.point_status equals li_ps.value
                         join l_ps in db.listings on li_ps.list_id equals l_ps.list_id

                         where (
                            pp.record_status != CommonConstant.RecordStatus.deleted
                            && pp.transaction_id == transaction_id
                            && l_ps.code == "PointStatus"
                            && p_l.lang_id == _accessObject.languageID
                         )
                         select new ProductPurchaseObject
                         {
                             purchase_id = pp.purchase_id,
                             order_id = pp.order_id,
                             transaction_id = pp.transaction_id,
                             point = pp.point,
                             promotion_transaction_id = pp.promotion_transaction_id,
                             member_id = pp.member_id,
                             product_id = pp.product_id,
                             quantity = pp.quantity,
                            
                             status = pp.status,
                             crt_date = pp.crt_date,
                             crt_by_type = pp.crt_by_type,
                             crt_by = pp.crt_by,
                             upd_date = pp.upd_date,
                             upd_by_type = pp.upd_by_type,
                             upd_by = pp.upd_by,
                             record_status = pp.record_status,

                             //-- Additional Info
                             product_name = p_l.name,
                             product_no = p.product_no,
                             point_earned = te.point_earn,
                             point_status_name = li_ps.name,
                             point_expiry_date = te.point_expiry_date
                         });

            totalRow = query.Count();
            var limitedList = query.OrderByDescending(x => x.crt_date).Skip(rowIndexStart).Take(rowLimit).ToList();

            return limitedList;
        }

        public List<ProductPurchaseObject> GetListByTime(int memberID, DateTime startTime, DateTime endTime, ref CommonConstant.SystemCode systemCode)
        {
            var query = (from pp in db.product_purchases
                         join p_l in db.product_langs on pp.product_id equals p_l.product_id
                         join p in db.products on pp.product_id equals p.product_id
                         join te in db.transaction_earns on pp.purchase_id equals te.source_id

                         join li_ps in db.listing_items on te.point_status equals li_ps.value
                         join l_ps in db.listings on li_ps.list_id equals l_ps.list_id

                         where (
                            pp.record_status != CommonConstant.RecordStatus.deleted
                            && pp.member_id == memberID
                            && p_l.lang_id == (int)CommonConstant.LangCode.en
                            && l_ps.code == "PointStatus"
                            && (startTime <= pp.crt_date && pp.crt_date <= endTime)
                         )
                         select new ProductPurchaseObject
                         {
                             purchase_id = pp.purchase_id,
                             order_id = pp.order_id,
                             transaction_id = pp.transaction_id,
                             point = pp.point,
                             promotion_transaction_id = pp.promotion_transaction_id,
                             member_id = pp.member_id,
                             product_id = pp.product_id,
                             quantity = pp.quantity,

                             status = pp.status,
                             crt_date = pp.crt_date,
                             crt_by_type = pp.crt_by_type,
                             crt_by = pp.crt_by,
                             upd_date = pp.upd_date,
                             upd_by_type = pp.upd_by_type,
                             upd_by = pp.upd_by,
                             record_status = pp.record_status,

                             //-- Additional Info
                             product_name = p_l.name,
                             product_no = p.product_no,
                             point_earned = te.point_earn,
                             point_status_name = li_ps.name,
                             point_expiry_date = te.point_expiry_date
                         });

            var limitedList = query.OrderByDescending(x => x.crt_date).ToList();

            return limitedList;
        }

        public ProductPurchaseObject GetDetail(int purchase_id, ref CommonConstant.SystemCode systemCode)
        {
            ProductPurchaseObject resultObj;

            var query = (from pp in db.product_purchases
                         
                         join p in db.products on pp.product_id equals p.product_id
                         join p_l in db.product_langs on p.product_id equals p_l.product_id
                         join te in db.transaction_earns on pp.purchase_id equals te.source_id

                         join li_ps in db.listing_items on te.point_status equals li_ps.value
                         join l_ps in db.listings on li_ps.list_id equals l_ps.list_id

                         where (
                            pp.record_status != CommonConstant.RecordStatus.deleted
                            && pp.purchase_id == purchase_id
                            && l_ps.code == "PointStatus"
                            && p_l.lang_id == _accessObject.languageID
                         )
                         select new ProductPurchaseObject
                         {
                             purchase_id = pp.purchase_id,
                             order_id = pp.order_id,
                             transaction_id = pp.transaction_id,
                             point = pp.point,
                             promotion_transaction_id = pp.promotion_transaction_id,
                             member_id = pp.member_id,
                             product_id = pp.product_id,
                             quantity = pp.quantity,

                             status = pp.status,
                             crt_date = pp.crt_date,
                             crt_by_type = pp.crt_by_type,
                             crt_by = pp.crt_by,
                             upd_date = pp.upd_date,
                             upd_by_type = pp.upd_by_type,
                             upd_by = pp.upd_by,
                             record_status = pp.record_status,

                             //-- Additional Info
                             product_name = p_l.name,
                             product_no = p.product_no,
                             point_earned = te.point_earn,
                             point_status_name = li_ps.name,
                             point_expiry_date = te.point_expiry_date
                         });

            resultObj = query.FirstOrDefault();

            return resultObj;
        }

        public ProductPurchaseObject GetDetailByTransaction(int transaction_id, ref CommonConstant.SystemCode systemCode)
        {
            ProductPurchaseObject resultObj;

            var query = (from pp in db.product_purchases
                         
                         join p in db.products on pp.product_id equals p.product_id
                         join p_l in db.product_langs on p.product_id equals p_l.product_id
                         join te in db.transaction_earns on pp.purchase_id equals te.source_id

                         join li_ps in db.listing_items on te.point_status equals li_ps.value
                         join l_ps in db.listings on li_ps.list_id equals l_ps.list_id

                         where (
                            pp.record_status != CommonConstant.RecordStatus.deleted
                            && pp.transaction_id == transaction_id
                            && l_ps.code == "PointStatus"
                            && p_l.lang_id == _accessObject.languageID
                         )
                         select new ProductPurchaseObject
                         {
                             purchase_id = pp.purchase_id,
                             order_id = pp.order_id,
                             transaction_id = pp.transaction_id,
                             point = pp.point,
                             promotion_transaction_id = pp.promotion_transaction_id,
                             member_id = pp.member_id,
                             product_id = pp.product_id,
                             quantity = pp.quantity,

                             status = pp.status,
                             crt_date = pp.crt_date,
                             crt_by_type = pp.crt_by_type,
                             crt_by = pp.crt_by,
                             upd_date = pp.upd_date,
                             upd_by_type = pp.upd_by_type,
                             upd_by = pp.upd_by,
                             record_status = pp.record_status,

                             //-- Additional Info
                             product_name = p_l.name,
                             product_no = p.product_no,
                             point_earned = te.point_earn,
                             point_status_name = li_ps.name,
                             point_expiry_date = te.point_expiry_date
                         });

            resultObj = query.FirstOrDefault();

            return resultObj;
        }
    }
}