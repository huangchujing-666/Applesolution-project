using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Palmary.Loyalty.BO.Modules.Gift;
using Palmary.Loyalty.BO.Modules.Transaction;
using Palmary.Loyalty.BO.Modules;
using Palmary.Loyalty.BO.Database;
using Palmary.Loyalty.BO.DataTransferObjects.Transaction;

using Palmary.Loyalty.Common;
using Palmary.Loyalty.Common.Languages;
using Palmary.Loyalty.BO.Modules.Administration.Security;
using Palmary.Loyalty.BO.DataTransferObjects.Security;

namespace Palmary.Loyalty.BO.Modules.Transaction
{
    public class PointDeductActor
    {
        private TransactionManager _transactionManager;
        private TransactionEarnManager _transactionEarnManager;
        private TransactionUseManager _transactionUseManager;

        private AccessManager _accessManager;
        private AccessObject _accessObject;
        private static LogManager _logManager;

        // For backend, using BO Session to access
        public PointDeductActor()
        {
            // access object
            _accessManager = new AccessManager();
            _accessObject = _accessManager.GetAccessObjectBySession();

            // privilege and log
            //_privilege = _accessManager.AccessModule(_module);
            _logManager = new LogManager();

            InitOtherManager();
        }

        // For Webservices or other not using BO Session to access
        public PointDeductActor(AccessObject accessObject)
        {
            // access object
            _accessManager = new AccessManager();
            _accessObject = accessObject;

            // privilege and log
           // _privilege = _accessManager.AccessModule(_module, _accessObject.type, _accessObject.id);
            _logManager = new LogManager(_accessObject);

            InitOtherManager();
        }

        public void InitOtherManager()
        {
            _transactionManager = new TransactionManager(_accessObject);
            _transactionEarnManager = new TransactionEarnManager(_accessObject);
            _transactionUseManager = new TransactionUseManager(_accessObject);
        }

        public bool DeductPoint(int location_id, int member_id, double point_required, int transactionType, int source_id, string remark, ref int? new_transaction_id)
        {
            var systemCode = CommonConstant.SystemCode.undefine;
            var availableEarnList = _transactionEarnManager.GetList_memberAvailablePoint(member_id, ref systemCode);
            var result = false;

            point_required = point_required > 0 ? point_required : (0 - point_required); // turn back positive value
            double point_can_use = 0;

            foreach (var x in availableEarnList)
            {
                var remain_point = (x.point_earn - x.point_used);
                point_can_use += remain_point;
            }

            if (point_can_use >= point_required)
            {
                decimal need_to_consume_point = (decimal)point_required;

                // Create main transaction (use point record)
                var transactionObject = new TransactionObject()
                {
                    type = transactionType,
                    source_id = source_id,
                    location_id = location_id,
                    member_id = member_id,
                    point_change = (0 - (double)need_to_consume_point),
                    point_status = (int)CommonConstant.PointStauts.realized,
                    display = true,
                    void_date = null,
                    remark = remark,
                    status = CommonConstant.Status.active
                };

                _transactionManager.Create(transactionObject, ref new_transaction_id);

                foreach (var x in availableEarnList)
                {
                    decimal remain_point = (decimal)(x.point_earn - x.point_used);
                    decimal consumed_point = 0;

                    if (remain_point >= need_to_consume_point)
                        consumed_point = (decimal)need_to_consume_point;
                    else
                        consumed_point = (decimal)remain_point;

                    // Update transaction earn list
                    x.point_used = (double)((decimal)x.point_used + (decimal)consumed_point);

                    if (x.point_used == x.point_earn)
                    {
                        x.status = CommonConstant.TransactionStatus.all_point_used;
                    }

                    var updateTransactionFlag = _transactionEarnManager.Update(x);

                    // Create use detail
                    var transactionUseObject = new TransactionUseObject()
                    {
                        transaction_id = new_transaction_id.Value,
                        earn_id = x.earn_id,
                        point_used = (double)consumed_point,
                        status = CommonConstant.Status.active
                    };

                    _transactionUseManager.Create(transactionUseObject);

                    // minus need_to_consume_point
                    need_to_consume_point -= consumed_point;

                    // loop checking
                    if (need_to_consume_point == 0)
                    {
                        result = true;
                        break;
                    }
                }
            }
            return result;
        }
    }
}
