using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Palmary.Loyalty.BO.DataTransferObjects.DB;
using Palmary.Loyalty.Common;
namespace Palmary.Loyalty.Web_backend.Modules
{
    public interface IHandler
    {
        string LoadListDataToExtJSJson(int prefix_id, int startRowIndex, int rowLimit, List<SearchParmObject> searchParmList, string sortColumn, CommonConstant.SortOrder sortOrder, ref int rowTotal);
    }
}
