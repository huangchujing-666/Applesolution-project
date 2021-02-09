using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Palmary.Loyalty.BO.Database;
using System.Data.Objects;

namespace Palmary.Loyalty.BO.Modules.Administration.Table
{
    public class MenuManager
    {
        private static LoyaltyDBDataContext db = new LoyaltyDBDataContext();

        public MenuManager()
        {
        }

        public IEnumerable<sp_GetUserSectionsResult> GetMenuTree(int userId, int parentId, int lang_id)
        {
            var ip = "127.0.0.1"; // CommonService.GetIPAddress(HttpContext.Current.Request);
            int? status = 0;
            var remark = "";
            int? total = 0;
            var result = db.sp_GetUserSections(userId, ip, parentId, lang_id, ref status, ref remark, ref total);

            return result.ToList();
        }
    }
}
