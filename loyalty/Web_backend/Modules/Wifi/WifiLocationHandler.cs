using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Palmary.Loyalty.Web_backend.Modules.GlobalHandlers;
using Palmary.Loyalty.Common;

using Palmary.Loyalty.BO.Modules.Wifi;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.TableData;
using Palmary.Loyalty.Web_backend.Modules.Utility;
using Palmary.Loyalty.Web_backend.ObjectModels;

using Palmary.Loyalty.BO.DataTransferObjects.DB;
using Palmary.Loyalty.BO.DataTransferObjects.Wifi;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.ExtJsEntity;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.PayoadKey;

namespace Palmary.Loyalty.Web_backend.Modules.Wifi
{
    public class WifiLocationHandler : IHandler
    {
        public string LoadListDataToExtJSJson(int prefix_id, int startRowIndex, int rowLimit, List<SearchParmObject> searchParmList, string sortColumn, CommonConstant.SortOrder sortOrder, ref int rowTotal)
        {
            var wifiLocationManager = new WifiLocationManager();

            var resultCode = CommonConstant.SystemCode.undefine;
            var resultList = wifiLocationManager.GetList(startRowIndex, rowLimit, searchParmList, sortColumn, sortOrder, ref resultCode, ref rowTotal).ToList();

            var resultDataList = resultList.Select(
                x => new
                {
                    name = x.name,
                    location_no = x.location_no,
                    id = x.location_id,
                    mac_address = x.mac_address,
                    
                    href = "new com.embraiz.tag().openNewTag('EDIT_WL:" + x.location_id + "','Location: " + x.location_no + "','com.palmary.WifiLocation.js.edit','iconRole16','iconRole16','iconRole16','" + x.location_id + "')"
                }
            );

            return resultDataList.ToJson();
        }

        
        public string GetFormByModule(WifiLocationObject wifiLocationObject)
        {
            // basic table data
            var extTable = new ExtJsTable
            {
                post_url = "../Table/ListData",
                post_header = "../Table/GridHeader",
                title = "Wifi Location Detail",
                icon = "iconRole16",
                post_params = "../WifiLocation/Update",
                isType = true,
                button_text = "Save",
                button_icon = "iconSave",
                value_changes = true,
            };

            var field_label_str = new ExtJsFieldLabelInput<string>(PayloadKeys.WifiLocation.location_no, wifiLocationObject.location_no)
            {
                fieldLabel = "Location No",
                value = wifiLocationObject.location_no,
                group = "General",
            };
            extTable.AddFieldLabelToRow(field_label_str);

            field_label_str = new ExtJsFieldLabelInput<string>(PayloadKeys.WifiLocation.name, wifiLocationObject.name)
            {
                fieldLabel = "Name"
            };
            extTable.AddFieldLabelToRow(field_label_str);

            field_label_str = new ExtJsFieldLabelInput<string>(PayloadKeys.WifiLocation.mac_address, wifiLocationObject.mac_address)
            {
                fieldLabel = "Mac Address",
                allowBlank = false
            };
            extTable.AddFieldLabelToRow(field_label_str);

            // privilege
            var privilege_nameList = new List<string> { };
            var privilege_valueList = new List<string> { };

            if (wifiLocationObject.location_id > 0)
            {
                var wifiLocationPrivilegeManager = new WifiLocationPrivilegeManager();
                var systemCode = CommonConstant.SystemCode.undefine;
                var pList = wifiLocationPrivilegeManager.GetList(wifiLocationObject.location_id, true, ref systemCode);

                foreach (var p in pList)
                {
                    privilege_nameList.Add(p.member_level_name);
                    privilege_valueList.Add(p.member_level_id.ToString());
                }
            }

            var field_multiSelect_intList = new ExtJsFieldLabelMultiSelect<string>(PayloadKeys.WifiLocation.member_level, "")
            {
                fieldLabel = "Member Level",
                datasource = "../Table/GetListItems/memberlevel", //Url.Action("GetListItems/Status", "Table"),
                display_value = string.Join(",", privilege_nameList.ToArray()),
                value = privilege_valueList.ToArray(),
                readOnly = false
            };
            extTable.AddFieldLabelToRow(field_multiSelect_intList);

            extTable.AddFieldLabelToRow(new ExtJsFieldLabelInput<double>(PayloadKeys.WifiLocation.point, wifiLocationObject.point.ToString())
            {
                fieldLabel = "Point",
                allowBlank = false
            });

            var field_select = new ExtJsFieldLabelSelect<int>(PayloadKeys.WifiLocation.status, wifiLocationObject.status.ToString())
            {
                fieldLabel = "Status",
                datasource = "../Table/GetListItems/status",
                display_value = wifiLocationObject.status_name,
                allowBlank = false
            };
            extTable.AddFieldLabelToRow(field_select);

           

            //hidden table data
            var hiddenLabel = new ExtJsFieldLabelHidden<int>(PayloadKeys.WifiLocation.location_id, wifiLocationObject.location_id.ToString());
            extTable.AddFieldLabelToHiddenRow(hiddenLabel);

            return extTable.ToJson();
        }
    }
}