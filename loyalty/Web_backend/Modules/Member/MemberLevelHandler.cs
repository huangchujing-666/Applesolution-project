using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Palmary.Loyalty.BO.Database;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.ExtJsEntity;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.PayoadKey;
using Palmary.Loyalty.Web_backend.Modules.Utility;
using Palmary.Loyalty.Web_backend.Modules.GlobalHandlers;

using Palmary.Loyalty.BO.Modules.Member;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.TableData;
using Palmary.Loyalty.Web_backend.ObjectModels;
using Palmary.Loyalty.Common;
using Palmary.Loyalty.BO.DataTransferObjects.DB;

namespace Palmary.Loyalty.Web_backend.Modules.Member
{
    public class MemberLevelHandler : IHandler
    {
        public string LoadListDataToExtJSJson(int prefix_id, int startRowIndex, int rowLimit, List<SearchParmObject> searchParmList, string sortColumn, CommonConstant.SortOrder sortOrder, ref int rowTotal)
        {
            var memberLevelManager = new MemberLevelManager();
            var systemCode = CommonConstant.SystemCode.undefine;
            var resultList = memberLevelManager.GetList(startRowIndex, rowLimit, searchParmList, sortColumn, sortOrder, ref systemCode, ref rowTotal);

            var resultDataList = resultList.Select(
              x => new
              {
                  id = x.level_id,
                  level_id = x.level_id,
                  name = x.name,
                  point_required = x.point_required,
                  redeem_discount = x.redeem_discount.ToString() + "%",
                  status = x.status_name,
                  href = "new com.embraiz.tag().openNewTag('EDIT_ML_UID:" + x.level_id.ToString() + "','Member Level EDIT:" + x.name + "','com.palmary.memberProfile.js.edit','iconRole16','iconRole16','iconRole16','1')"
              }
            );

            return resultDataList.ToJson();
        }
    }
}