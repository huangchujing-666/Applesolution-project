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

namespace Palmary.Loyalty.BO.Modules.Transaction
{
    public class PointAdjustManager
    {
        private static LoyaltyDBDataContext db = new LoyaltyDBDataContext();
        private AccessManager _accessManager;
        private AccessObject _accessObject;
        private static LogManager _logManager;

        // For backend, using BO Session to access
        public PointAdjustManager()
        {
            // access object
            _accessManager = new AccessManager();
            _accessObject = _accessManager.GetAccessObjectBySession();

            // privilege and log
            //_privilege = _accessManager.AccessModule(_module);
            _logManager = new LogManager();
        }

        // For Webservices or other not using BO Session to access
        public PointAdjustManager(AccessObject accessObject)
        {
            // access object
            _accessManager = new AccessManager();
            _accessObject = accessObject;

            // privilege and log
           // _privilege = _accessManager.AccessModule(_module, _accessObject.type, _accessObject.id);
            _logManager = new LogManager(_accessObject);
        }

        public CommonConstant.SystemCode Adjust(int location_id, int member_id, double point_change, string remark)
        {
            var transactionManager = new TransactionManager(_accessObject);
            var result = false;

            if (point_change < 0)
            {
                int? new_transaction_id = 0;
                result = transactionManager.UsePoint(location_id, member_id, point_change, (int)CommonConstant.TransactionType.point_adjustment, _accessObject.id, remark, ref new_transaction_id);
            }
            else if (point_change > 0)
            {
                int? new_transaction_id = 0;
                var point_status = (int)CommonConstant.PointStauts.realized;
                var systemConfigManager = new SystemConfigManager(_accessObject);

                var bo_remark = "";
                var point_expiry_month = int.Parse(systemConfigManager.GetSystemConfigValue("point_expiry_month", ref bo_remark));
                var point_expiry_date = DateTime.Now.AddMonths(point_expiry_month);

                result = transactionManager.AddPoint(location_id, member_id, point_change, point_status, point_expiry_date, (int)CommonConstant.TransactionType.point_adjustment, _accessObject.id, remark, ref new_transaction_id);
            }

            if (result)
                return CommonConstant.SystemCode.normal;
            else
                return CommonConstant.SystemCode.record_invalid;
        }
    }
}
