using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Palmary.Loyalty.Common;

using Palmary.Loyalty.BO.Modules.Wifi;
using Palmary.Loyalty.BO.Modules.Transaction;
using Palmary.Loyalty.BO.Modules.Administration.SystemControl;
using Palmary.Loyalty.BO.DataTransferObjects.Security;
using Palmary.Loyalty.BO.Modules.Administration.Security;

namespace Palmary.Loyalty.BO.Modules.PointEngine
{
    // all point change request should be perform through Point Engine Manager
    public class PointEngineManager
    {
        private AccessManager _accessManager;
        private AccessObject _accessObject;
        private static LogManager _logManager;

        // For backend, using BO Session to access
        public PointEngineManager()
        {
            // access object
            _accessManager = new AccessManager();
            _accessObject = _accessManager.GetAccessObjectBySession();

            // privilege and log
            //_privilege = _accessManager.AccessModule(_module);
            _logManager = new LogManager();
        }

        // For Webservices or other not using BO Session to access
        public PointEngineManager(AccessObject accessObject)
        {
            // access object
            _accessManager = new AccessManager();
            _accessObject = accessObject;

            // privilege and log
           // _privilege = _accessManager.AccessModule(_module, _accessObject.type, _accessObject.id);
            _logManager = new LogManager(_accessObject);
        }

        public CommonConstant.SystemCode IBeaconLocationPresence(int member_id)
        {
            var systemCode = CommonConstant.SystemCode.undefine;

            var transactionManager = new TransactionManager(_accessObject);
            var location_id = 1;
            var location_point = 1;

            int? new_transaction_id = 0;
            var point_expiry_date = GetDefaultPointExpiryDate();
            var source_id = 0;
            var remark = "";
            transactionManager.AddPoint(location_id, member_id, location_point,
                (int)CommonConstant.PointStauts.realized,
                point_expiry_date,
                (int)CommonConstant.TransactionType.location_presence,
                source_id,
                remark,
                ref new_transaction_id);

            systemCode = CommonConstant.SystemCode.normal;

            return systemCode;
        }

        public CommonConstant.SystemCode MissionGetPoint(int member_id)
        {
            var systemCode = CommonConstant.SystemCode.undefine;

            var transactionManager = new TransactionManager(_accessObject);
            var location_id = 1;
            var location_point = 1;

            int? new_transaction_id = 0;
            var point_expiry_date = GetDefaultPointExpiryDate();
            var source_id = 0;
            var remark = "";
            transactionManager.AddPoint(location_id, member_id, location_point,
                (int)CommonConstant.PointStauts.realized,
                point_expiry_date,
                (int)CommonConstant.TransactionType.mission,
                source_id,
                remark,
                ref new_transaction_id);

            systemCode = CommonConstant.SystemCode.normal;

            return systemCode;
        }


        public CommonConstant.SystemCode WifiLocationPresence(int location_id, int member_id)
        {
            var systemCode = CommonConstant.SystemCode.undefine;

            var wifiLocationManager = new WifiLocationManager();
            
            var location = wifiLocationManager.GetDetail(location_id, true, ref systemCode);

            if (location.location_id > 0)
            {
                var transactionManager = new TransactionManager();

                int? new_transaction_id = 0;
                var point_expiry_date = GetDefaultPointExpiryDate();
                var source_id = 0;
                var remark = "";
                transactionManager.AddPoint(location_id, member_id, location.point,
                    (int)CommonConstant.PointStauts.realized,
                    point_expiry_date,
                    (int)CommonConstant.TransactionType.location_presence,
                    source_id,
                    remark,
                    ref new_transaction_id);
            }
            else
            {
                systemCode = CommonConstant.SystemCode.data_invalid;
            }
            return systemCode;
        }

        public DateTime GetDefaultPointExpiryDate()
        {
            var systemConfigManager = new SystemConfigManager(_accessObject);
            var sqlremark = "";
            var point_expiry_month = int.Parse(systemConfigManager.GetSystemConfigValue("point_expiry_month", ref sqlremark));

            DateTime point_expiry_date = DateTime.Now.AddMonths(point_expiry_month);

            return point_expiry_date;
        }
    }
}