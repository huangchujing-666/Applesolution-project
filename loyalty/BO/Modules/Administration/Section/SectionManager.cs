using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Palmary.Loyalty.BO.Database;
using System.Data.Objects;
using Palmary.Loyalty.BO.Modules.Administration.Security;

namespace Palmary.Loyalty.BO.Modules.Administration.Section
{
    public class SectionManager
    {
        //private static LoyaltyDBDataContext db = new LoyaltyDBDataContext();

        //public bool Create(
        //    int user_id,

        //    int section_id,
        //    int parent,
        //    string name,
        //    int icon,
        //    string link,
        //    int display,
        //    int display_order,
        //    string module,
        //    int status,
        //    int leaf,

        //    ref string sql_remark
        //)
        //{
        //    var create_result = false;
        //    int? get_sql_result = 0;
            
        //    var result = db.sp_CreateSection(
        //        user_id,

        //        section_id,
        //        parent,
        //        name,
        //        icon,
        //        link,
        //        display,
        //        display_order,
        //        module,
        //        status,

        //        leaf,
        //        ref get_sql_result, ref sql_remark);

        //    create_result = get_sql_result == 1 ? true : false;
           
        //    return create_result;
        //}
    }
}