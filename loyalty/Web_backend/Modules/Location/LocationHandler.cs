using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Palmary.Loyalty.Web_backend.Modules.GlobalHandlers;
using Palmary.Loyalty.Common;

using Palmary.Loyalty.BO.Modules.Location;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.TableData;
using Palmary.Loyalty.Web_backend.Modules.Utility;
using Palmary.Loyalty.Web_backend.ObjectModels;

using Palmary.Loyalty.BO.DataTransferObjects.DB;
using Palmary.Loyalty.BO.Modules.Administration.Security;

namespace Palmary.Loyalty.Web_backend.Modules.Location
{
    public class LocationHandler : IHandler
    {
        public string LoadListDataToExtJSJson(int prefix_id, int startRowIndex, int rowLimit, List<SearchParmObject> searchParmList, string sortColumn, CommonConstant.SortOrder sortOrder, ref int rowTotal)
        {
            LocationManager _locationManager = new LocationManager();
            FileHandler _fileHandler = new FileHandler();

            var resultList = _locationManager.GetLocationLists(SessionManager.Current.obj_id, startRowIndex, rowLimit, "").ToList();
            var resultDataList = resultList.Select(
                x => new
                {
                    name = x.name,
                    location_no = x.location_no,
                    location_id = x.location_id, 
                    phone = x.phone,
                    status_name = x.status_name,

                    photo = _fileHandler.GetImagePath(x.photo_file_name, x.photo_file_extension, (string)CommonConstant.Module.location, (int)CommonConstant.ImageSizeType.thumb),
                    href = "new com.embraiz.tag().openNewTag('EDIT_L:" + x.location_id + "','Location: " + x.location_no + "','com.palmary.location.js.edit','iconRole16','iconRole16','iconRole16','" + x.location_id + "')"
                }
            );

            return resultDataList.ToJson();
        }
    }
}