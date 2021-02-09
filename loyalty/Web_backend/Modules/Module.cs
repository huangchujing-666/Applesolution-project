using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data.Objects;
using Palmary.Loyalty.BO.Database;
using Palmary.Loyalty.BO.DataTransferObjects.DB;
using Palmary.Loyalty.Web_backend.Modules.Product;
using Palmary.Loyalty.Web_backend.Modules.Gift;
using Palmary.Loyalty.Web_backend.Modules.Administration;
using Palmary.Loyalty.Web_backend.Modules.Location;
using Palmary.Loyalty.Web_backend.Modules.Transaction;
using Palmary.Loyalty.Web_backend.Modules.Member;
using Palmary.Loyalty.Web_backend.Modules.PromotionRule;
using Palmary.Loyalty.Web_backend.Modules.Service;
using Palmary.Loyalty.Web_backend.Modules.Wifi;
using Palmary.Loyalty.Web_backend.Modules.ConJob;
using Palmary.Loyalty.Web_backend.Modules.Demo;
using Palmary.Loyalty.Web_backend.ObjectModels;
using Palmary.Loyalty.Common;
using Palmary.Loyalty.Web_backend.Modules.Reminder;
using Palmary.Loyalty.Web_backend.Modules.Report;
using Palmary.Loyalty.Web_backend.Modules.ReminderEngine;
using Palmary.Loyalty.Web_backend.Modules.Passcode;
using Palmary.Loyalty.Web_backend.Modules.MemberAdvanceSearch;

namespace Palmary.Loyalty.Web_backend.Modules
{
    public delegate string LoadListDataToExtJSJson(int object_id, int startRowIndex, int rowLimit, List<SearchParmObject> searchParmList, string sort, CommonConstant.SortOrder sortOrder, ref int rowTotal);
    
    public class Module
    {
        private readonly string _name;
        public LoadListDataToExtJSJson LoadListDataToExtJSJson;

        public Module(string module_name) //: this()
        {
            _name = module_name.ToLower();
            initFunctions();
        }

        private void initFunctions()
        {
            IHandler handler = null;
            
            // core
            if (_name == CommonConstant.Module.user.ToLower())
                handler = new UserHandler();
            else if (_name == CommonConstant.Module.Role.ToLower())
                handler = new RoleHandler();
            else if (_name == CommonConstant.Module.systemConfig.ToLower())
                handler = new SystemConfigHandler();
            else if (_name == CommonConstant.Module.language.ToLower())
                handler = new LanguageHandler();
            else if (_name == CommonConstant.Module.log.ToLower())
                handler = new LogHandler();
            // demo
            else if (_name == CommonConstant.Module.demo1.ToLower())
                handler = new Demo1Handler();
            // other
            else if (_name == CommonConstant.Module.passcode.ToLower())
                handler = new PasscodeHandler();
            else if (_name == CommonConstant.Module.passcodeGenerate.ToLower())
                handler = new PasscodeGenerateHandler();
            else if (_name == CommonConstant.Module.passcodeRegistry.ToLower())
                handler = new PasscodeRegistryHandler();
            else if (_name == CommonConstant.Module.passcodePrefix.ToLower())
                handler = new PasscodePrefixHandler();

            else if (_name == CommonConstant.Module.gift.ToLower())
                handler = new GiftHandler();
            else if (_name == CommonConstant.Module.giftCategory.ToLower())
                handler = new GiftCategoryHandler();
            else if (_name == CommonConstant.Module.giftInventory.ToLower())
                handler = new GiftInventoryHandler();
            else if (_name == CommonConstant.Module.giftRedemption.ToLower())
                handler = new GiftRedemptionHandler();
            else if (_name == CommonConstant.Module.giftInventorySummary.ToLower())
                handler = new GiftInventorySummaryHandler();
            else if (_name == CommonConstant.Module.giftRedemptionHistory.ToLower())
                handler = new GiftRedemptionHistroyHandler();
            else if (_name == CommonConstant.Module.location.ToLower())
                handler = new LocationHandler();

            else if (_name == CommonConstant.Module.transaction.ToLower())
                handler = new TransactionHandler();
            else if (_name == CommonConstant.Module.transactionDetailRedemption.ToLower())
                handler = new TransactionDetailRedemptionHandler();
            else if (_name == CommonConstant.Module.transactionHistory.ToLower())
                handler = new TransactionHistoryHandler();
            else if (_name == CommonConstant.Module.transactionImport.ToLower())
                handler = new TransactionImportHandler();
            else if (_name == CommonConstant.Module.transactionField.ToLower())
                handler = new TransactionFieldHandler();
            else if (_name == CommonConstant.Module.transactionGroup.ToLower())
                handler = new TransactionGroupHandler();

            else if (_name == CommonConstant.Module.member.ToLower())
                handler = new MemberHandler();
            else if (_name == CommonConstant.Module.memberLevel.ToLower())
                handler = new MemberLevelHandler();
            else if (_name == CommonConstant.Module.memberImport.ToLower())
                handler = new MemberImportHandler();
            else if (_name == CommonConstant.Module.memberCard.ToLower())
                handler = new MemberCardHandler();
            else if (_name == CommonConstant.Module.memberCardHistory.ToLower())
                handler = new MemberCardHistoryHandler();
            else if (_name == CommonConstant.Module.memberField.ToLower())
                handler = new MemberFieldHandler();
            else if (_name == CommonConstant.Module.memberGroup.ToLower())
                handler = new MemberGroupHandler();

            else if (_name == CommonConstant.Module.product.ToLower())
                handler = new ProductHandler();
    
            else if (_name == CommonConstant.Module.promotionRule.ToLower())
                handler = new PromotionRuleHandler();
            else if (_name == CommonConstant.Module.productPurchase.ToLower())
                handler = new ProductPurchaseHandler();
            else if (_name == CommonConstant.Module.productPurchaseDetail.ToLower())
                handler = new ProductPurchaseDetailHandler();
            else if (_name == CommonConstant.Module.servicePlan.ToLower())
                handler = new ServicePlanHandler();
 
            else if (_name == CommonConstant.Module.serviceContract.ToLower())
                handler = new ServiceContractHandler();
            else if (_name == CommonConstant.Module.servicePayment.ToLower())
                handler = new ServicePaymentHandler();
            else if (_name == CommonConstant.Module.servicePaymentDetail.ToLower())
                handler = new ServicePaymentDetailHandler();
            else if (_name == CommonConstant.Module.servicePaymentDetailExtra.ToLower())
                handler = new ServicePaymentDetailExtraHandler();
            else if (_name == CommonConstant.Module.wifiLocation.ToLower())
                handler = new WifiLocationHandler();
            else if (_name == CommonConstant.Module.wifiAccessHistory.ToLower())
                handler = new WifiAccessHistoryHandler();
            else if (_name == CommonConstant.Module.wifiAccessReportByLocation.ToLower())
                handler = new WifiAccessReportByLocationHandler();
            else if (_name == CommonConstant.Module.wifiAccessReportByMemberLevel.ToLower())
                handler = new WifiAccessReportByMemberLevelHandler();
            else if (_name == CommonConstant.Module.fileImportJob.ToLower())
                handler = new FileImportJobHandler();

            else if (_name == CommonConstant.Module.reminderEngine.ToLower())
                handler = new ReminderEngineHandler();
            else if (_name == CommonConstant.Module.reminderTemplate.ToLower())
                handler = new ReminderTemplateHandler();

            else if (_name == CommonConstant.Module.report.ToLower())
                handler = new ReportHandler();
            else if (_name == CommonConstant.Module.memberLevelChange.ToLower())
                handler = new MemberLevelChangeHandler();
            else if (_name == CommonConstant.Module.wifiLocationPromote.ToLower())
                handler = new WifiLocationPromoteHandler();
            else if (_name == CommonConstant.Module.memberAdvanceSearch.ToLower())
                handler = new MemberAdvanceSearchHandler();
            else if (_name == CommonConstant.Module.memberAdvanceSearchListMember.ToLower())
                handler = new MemberAdvanceSearchListMemberHandler();
            else if (_name==CommonConstant.Module.cronjob.ToLower())
            {
                handler = new CronJobHandler();
            }
            if (handler != null)
                LoadListDataToExtJSJson = handler.LoadListDataToExtJSJson;

        }

        public override string ToString()
        {
            return _name;
        }
    }
}