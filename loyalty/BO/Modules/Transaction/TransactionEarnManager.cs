using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.Objects;

using Palmary.Loyalty.Common;
using Palmary.Loyalty.BO.Database;
using Palmary.Loyalty.BO.DataTransferObjects.Security;
using Palmary.Loyalty.BO.DataTransferObjects.Transaction;
using Palmary.Loyalty.BO.DataTransferObjects.Role;
using Palmary.Loyalty.BO.Modules.Administration.Security;

namespace Palmary.Loyalty.BO.Modules.Transaction
{
    public class TransactionEarnManager
    {
        private static LoyaltyDBDataContext db = new LoyaltyDBDataContext();
        private string _module = CommonConstant.Module.transaction;
        private AccessManager _accessManager;
        private RolePrivilegeObject _privilege;
        private AccessObject _accessObject;
        private static LogManager _logManager;

        // For backend, using BO Session to access
        public TransactionEarnManager()
        {
            // access object
            _accessManager = new AccessManager();
            _accessObject = _accessManager.GetAccessObjectBySession();

            // privilege and log
            _privilege = _accessManager.AccessModule(_module);
            _logManager = new LogManager();
        }

        // For Webservices or other not using BO Session to access
        public TransactionEarnManager(AccessObject accessObject)
        {
            // access object
            _accessManager = new AccessManager();
            _accessObject = accessObject;

            // privilege and log
            _privilege = _accessManager.AccessModule(_module, _accessObject.type, _accessObject.id);
            _logManager = new LogManager(_accessObject);
        }

        public CommonConstant.SystemCode Create(
            TransactionEarnObject dataObject
        )
        {
            int? sql_result = 0;
            
            var system_code = CommonConstant.SystemCode.undefine;

            if (_privilege.insert_status == 1)
            {
                var result = db.sp_CreateTransactionEarn(
                    _accessObject.id,
                     _accessObject.type,
                    dataObject.transaction_id,
                    dataObject.source_type,
                    dataObject.source_id,
                    dataObject.point_earn,
                    dataObject.point_status,
                    dataObject.point_expiry_date,
                    dataObject.point_used,
                    
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

        public CommonConstant.SystemCode Update(TransactionEarnObject dataObject)
        {
            int? sql_result = 0;
            var system_code = CommonConstant.SystemCode.undefine;

            if (_privilege.update_status == 1)
            {
                var result = db.sp_UpdateTransactionEarn(
                    _accessObject.id,
                          _accessObject.type,
                    dataObject.earn_id,
                    dataObject.transaction_id,
                    dataObject.source_type,
                    dataObject.source_id,
                    dataObject.point_earn,
                    dataObject.point_status,
                    dataObject.point_expiry_date,
                    dataObject.point_used,
                    
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

        // Get whole list (limited data) earn=active(not used up), not expired, realized
        public List<TransactionEarnObject> GetList_memberAvailablePoint(int member_id, ref CommonConstant.SystemCode systemCode)
        {
            List<TransactionEarnObject> resultList;

            if (_privilege.read_status == 1)
            {
                var query = (from te in db.transaction_earns
                             join t in db.transactions on te.transaction_id equals t.transaction_id
                             where (
                                te.record_status != (int)CommonConstant.RecordStatus.deleted
                                && te.point_expiry_date >= DateTime.Now // not expired date
                                && te.point_status == (int)CommonConstant.PointStauts.realized
                                && t.member_id == member_id
                                && te.status == CommonConstant.TransactionStatus.active // point is not used up
                            )
                             select new TransactionEarnObject
                             {
                                 earn_id = te.earn_id,
                                 transaction_id = te.transaction_id,
                                 source_id = te.source_id,
                                 point_earn = te.point_earn,
                                 point_status = te.point_status,
                                 point_used = te.point_used,
                                 point_expiry_date = te.point_expiry_date,
                                 status = te.status,
                                 crt_date = te.crt_date,
                                 crt_by_type = te.crt_by_type,
                                 crt_by = te.crt_by,
                                 upd_date = te.upd_date,
                                 upd_by_type = te.upd_by_type,
                                 upd_by = te.upd_by,
                                 record_status = te.record_status
                             });

                resultList = query.OrderBy(x => x.point_expiry_date).ToList();

                systemCode = CommonConstant.SystemCode.normal;
            }
            else
            {
                resultList = new List<TransactionEarnObject>();
                systemCode = CommonConstant.SystemCode.no_permission;
            }

            return resultList;
        }

        // Get whole list (limited data) earn=active(not used up), not expired, realized, from input date to today
        public List<TransactionEarnObject> GetList_memberAvailablePoint_fromDate(int member_id, DateTime from_date, ref CommonConstant.SystemCode systemCode)
        {
            List<TransactionEarnObject> resultList;

            if (_privilege.read_status == 1)
            {
                var query = (from te in db.transaction_earns
                             join t in db.transactions on te.transaction_id equals t.transaction_id
                             where (
                                te.record_status != (int)CommonConstant.RecordStatus.deleted
                              //  && te.point_expiry_date >= DateTime.Now // not expired date
                                && te.point_status == (int)CommonConstant.PointStauts.realized
                                && t.member_id == member_id
                                && te.status == CommonConstant.TransactionStatus.active // point is not used up
                                && t.crt_date >= from_date
                            )
                             select new TransactionEarnObject
                             {
                                 earn_id = te.earn_id,
                                 transaction_id = te.transaction_id,
                                 source_id = te.source_id,
                                 point_earn = te.point_earn,
                                 point_status = te.point_status,
                                 point_used = te.point_used,
                                 point_expiry_date = te.point_expiry_date,
                                 status = te.status,
                                 crt_date = te.crt_date,
                                 crt_by_type = te.crt_by_type,
                                 crt_by = te.crt_by,
                                 upd_date = te.upd_date,
                                 upd_by_type = te.upd_by_type,
                                 upd_by = te.upd_by,
                                 record_status = te.record_status
                             });

                resultList = query.OrderBy(x => x.point_expiry_date).ToList();

                systemCode = CommonConstant.SystemCode.normal;
            }
            else
            {
                resultList = new List<TransactionEarnObject>();
                systemCode = CommonConstant.SystemCode.no_permission;
            }

            return resultList;
        }

        // Get whole list (limited data) 
        public List<TransactionEarnObject> GetList_member(int member_id, ref CommonConstant.SystemCode systemCode)
        {
            List<TransactionEarnObject> resultList;

            if (_privilege.read_status == 1)
            {
                var query = (from te in db.transaction_earns
                             join t in db.transactions on te.transaction_id equals t.transaction_id
                             where (
                                te.record_status != (int)CommonConstant.RecordStatus.deleted
                                && t.member_id == member_id
                            )
                             select new TransactionEarnObject
                             {
                                 earn_id = te.earn_id,
                                 transaction_id = te.transaction_id,
                                 source_id = te.source_id,
                                 point_earn = te.point_earn,
                                 point_status = te.point_status,
                                 point_used = te.point_used,
                                 point_expiry_date = te.point_expiry_date,
                                 status = te.status,
                                 crt_date = te.crt_date,
                                 crt_by_type = te.crt_by_type,
                                 crt_by = te.crt_by,
                                 upd_date = te.upd_date,
                                 upd_by_type = te.upd_by_type,
                                 upd_by = te.upd_by,
                                 record_status = te.record_status
                             });

                resultList = query.OrderByDescending(x => x.point_expiry_date).ToList();

                systemCode = CommonConstant.SystemCode.normal;
            }
            else
            {
                resultList = new List<TransactionEarnObject>();
                systemCode = CommonConstant.SystemCode.no_permission;
            }

            return resultList;
        }

        // Get Detail
        public TransactionEarnObject GetDetail(int transaction_id, int source_id, ref CommonConstant.SystemCode systemCode)
        {
            TransactionEarnObject resultObj;

            if (_privilege.read_status == 1)
            {
                var query = (from te in db.transaction_earns
                            
                             where (
                                te.record_status != (int)CommonConstant.RecordStatus.deleted
                                && te.transaction_id == transaction_id
                                && te.source_id == source_id
                            )
                             select new TransactionEarnObject
                             {
                                 earn_id = te.earn_id,
                                 transaction_id = te.transaction_id,
                                 source_id = te.source_id,
                                 point_earn = te.point_earn,
                                 point_status = te.point_status,
                                 point_expiry_date = te.point_expiry_date,
                                 point_used = te.point_used,
                                 status = te.status,
                                 crt_date = te.crt_date,
                                 crt_by_type = te.crt_by_type,
                                 crt_by = te.crt_by,
                                 upd_date = te.upd_date,
                                 upd_by_type = te.upd_by_type,
                                 upd_by = te.upd_by,
                                 record_status = te.record_status,
                             });

                resultObj = query.FirstOrDefault();
                systemCode = CommonConstant.SystemCode.normal;
            }
            else
            {
                resultObj = new TransactionEarnObject();
                systemCode = CommonConstant.SystemCode.no_permission;
            }

            return resultObj;
        }

        // Get Detail For temporary use (frontend demo use)
        public TransactionEarnObject GetDetailTemp(int transaction_id, ref CommonConstant.SystemCode systemCode)
        {
            TransactionEarnObject resultObj;

            var query = (from te in db.transaction_earns

                            where (
                            te.record_status != (int)CommonConstant.RecordStatus.deleted
                            && te.transaction_id == transaction_id
                        )
                            select new TransactionEarnObject
                            {
                                earn_id = te.earn_id,
                                transaction_id = te.transaction_id,
                                source_id = te.source_id,
                                point_earn = te.point_earn,
                                point_status = te.point_status,
                                point_expiry_date = te.point_expiry_date,
                                point_used = te.point_used,
                                status = te.status,
                                crt_date = te.crt_date,
                                crt_by_type = te.crt_by_type,
                                crt_by = te.crt_by,
                                upd_date = te.upd_date,
                                upd_by_type = te.upd_by_type,
                                upd_by = te.upd_by,
                                record_status = te.record_status,
                            });

            resultObj = query.FirstOrDefault();
            systemCode = CommonConstant.SystemCode.normal;
          

            return resultObj;
        }

    }
}
