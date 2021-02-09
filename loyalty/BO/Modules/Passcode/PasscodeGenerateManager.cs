using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Palmary.Loyalty.BO.Database;
using Palmary.Loyalty.BO.DataTransferObjects.Security;
using System.Data.Objects;
using Palmary.Loyalty.BO.Modules.Administration.Security;
using Palmary.Loyalty.BO.DataTransferObjects.Role;
using Palmary.Loyalty.Common;

namespace Palmary.Loyalty.BO.Modules.Passcode
{
    public class PasscodeGenerateManager
    {
        private static LoyaltyDBDataContext db = new LoyaltyDBDataContext();

        private AccessManager _accessManager;

        private RolePrivilegeObject _privilege;
        private static LogManager _logManager;

        private AccessObject _accessObject;
        private string _module = CommonConstant.Module.passcodeGenerate;

        // For backend, using BO Session to access
        public PasscodeGenerateManager()
        {
            // access object from session
            _accessManager = new AccessManager();
            _accessObject = _accessManager.GetAccessObjectBySession();

            // privilege and log
            _privilege = _accessManager.AccessModule(_module);
            _logManager = new LogManager();
        }




        public bool Create(
            int user_id,

            long noToGenerate,
            int generate_status,

            ref int generate_id,
            ref string sql_remark
        )
        {
            int? get_generate_id = 0;
            int? get_sql_result = 0;

            var result = db.sp_CreatePasscodeGenerate(
                _accessObject.id,
               _accessObject.type,

                noToGenerate,
                generate_status,

                ref get_generate_id,
                ref get_sql_result, ref sql_remark);

            generate_id = get_generate_id.Value;

            return get_sql_result == 1 ? true : false;
        }

        public IEnumerable<sp_GetPasscodeGenerateListsResult> GetPasscodeGenerateLists(int user_id, int rowIndexStart, int rowLimit)
        {
            int? get_sql_result = 0;
            var sql_remark = "";

            var result = db.sp_GetPasscodeGenerateLists(SessionManager.Current.obj_id, rowIndexStart, rowLimit, ref get_sql_result, ref sql_remark);

            return result;
        }

        public bool Update(
            int? user_id,
            int? generate_id,

            long? noToGenerate,
            long? generateCompleteCounter,
            long? insertErrorCounter,
            string error_messgae,
            int? generate_status,

            ref string sql_remark
        )
        {
            int? get_sql_result = 0;

            // Avoid SQL injection
            if (error_messgae != null)
                error_messgae = error_messgae.Replace("'", "''");

            System.Diagnostics.Debug.WriteLine(user_id, "user_id");
            System.Diagnostics.Debug.WriteLine(generate_id, "generate_id");
            System.Diagnostics.Debug.WriteLine(noToGenerate, "noToGenerate");
            System.Diagnostics.Debug.WriteLine(generateCompleteCounter, "generateCompleteCounter");
            System.Diagnostics.Debug.WriteLine(insertErrorCounter, "insertErrorCounter");
            System.Diagnostics.Debug.WriteLine(error_messgae, "error_messgae");
            System.Diagnostics.Debug.WriteLine(generate_status, "generate_status");

            // Run store procedure
            var result = db.sp_UpdatePasscodeGenerate(

                _accessObject.id,
                _accessObject.type,
                generate_id,
                noToGenerate,
                generateCompleteCounter,
                insertErrorCounter,
                error_messgae,
                generate_status,

                ref get_sql_result, ref sql_remark);

            return get_sql_result == 1 ? true : false;
        }
    }
}