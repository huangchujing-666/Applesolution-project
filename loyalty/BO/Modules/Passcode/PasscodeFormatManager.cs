using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Palmary.Loyalty.BO.Database;
using Palmary.Loyalty.BO.DataTransferObjects.Security;
using System.Data.Objects;
using Palmary.Loyalty.BO.Modules.Administration.Security;

namespace Palmary.Loyalty.BO.Modules.Passcode
{
    public class PasscodeFormatManager
    {
        private static LoyaltyDBDataContext db = new LoyaltyDBDataContext();

        public bool Create(
            int user_id,

            int format_id,
            string passcode_format,

            double safetyLimit_precent,

            int expiry_month,
            int status,

            ref string sql_remark
        )
        {
            int noOfNumber = passcode_format.Count(c => c == '#'); // #: 0-9
            int noOfLetterAndNumber = passcode_format.Count(c => c == '$'); // $: A-Z, 0-9

            long number_combination = (long)Math.Pow(36, noOfLetterAndNumber) * (long)Math.Pow(10, noOfNumber);
            long maximum_generate = Convert.ToInt64(number_combination * 0.5 / 100);

            int? get_sql_result = 0;
        
            var result = db.sp_CreatePasscodeFormat(
                SessionManager.Current.obj_id,

                format_id,
                passcode_format,
                number_combination,
                safetyLimit_precent,
                maximum_generate,
                expiry_month,
                status,

                ref get_sql_result, ref sql_remark);

            return get_sql_result == 1 ? true : false;
        }

        public sp_GetPasscodeFormatDetailByResult GetPasscodeFormatDetailBy(int user_id, int passcode_id, ref bool sql_result)
        {
            int? get_sql_result = 0;
            var sql_remark = "";

            var result = db.sp_GetPasscodeFormatDetailBy(user_id, passcode_id, ref get_sql_result, ref sql_remark);

            return result.FirstOrDefault() ?? new sp_GetPasscodeFormatDetailByResult();
        }
    }
}