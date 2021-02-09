using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Palmary.Loyalty.BO.Modules.Administration.Security;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.TableData;
using Palmary.Loyalty.Web_backend.Modules.Utility;
using Palmary.Loyalty.Web_backend.Modules.GlobalHandlers;
using Palmary.Loyalty.Web_backend.ObjectModels;
using Palmary.Loyalty.Common;
using Palmary.Loyalty.BO.DataTransferObjects.DB;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.ExtJsEntity;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.PayoadKey;

namespace Palmary.Loyalty.Web_backend.Modules.Administration
{
    public class LogHandler : IHandler
    {
        public string LoadListDataToExtJSJson(int prefix_id, int startRowIndex, int rowLimit, List<SearchParmObject> searchParmList, string sortColumn, CommonConstant.SortOrder sortOrder, ref int rowTotal)
        {
            LogManager _logManager = new LogManager();

            int skipRow = startRowIndex; // unchecked((int)startRowIndex);

            var resultCode = CommonConstant.SystemCode.undefine;
            var logList = _logManager.GetList(startRowIndex, rowLimit, searchParmList, sortColumn, sortOrder, ref resultCode, ref rowTotal).ToList();
            var resultList = new List<ExtJsDataRow_log>();

            foreach (var x in logList)
            {
                var tragetObj_url = "";
                var action = "";

                if (x.action_type != CommonConstant.ActionType.delete)
                {
                    if (x.target_obj_type_id == CommonConstant.ObjectType.user)
                        tragetObj_url = "new com.embraiz.tag().openNewTag('EDIT_User:" + x.target_obj_id + "','User: " + x.target_obj_name + "','com.palmary.user.js.edit','iconRole16','iconRole16','iconRole16', '" + x.target_obj_id + "')";
                    else if (x.target_obj_type_id == CommonConstant.ObjectType.user_role)
                        tragetObj_url = "new com.embraiz.tag().openNewTag('EDIT_R:" + x.target_obj_id + "','Role EDIT:" + x.target_obj_name + "','com.palmary.role.js.edit','iconRole16','iconRole16','iconRole16','" + x.target_obj_id + "')";
                    
                }

                if (x.action_type == CommonConstant.ActionType.update || x.action_type == CommonConstant.ActionType.create)
                    action = "<button type='button' style=\"display:\" onclick=\"new com.embraiz.tag().openNewTag('LogDetail:" + x.log_id + "','Log Detail: " + x.log_id + "','com.palmary.logdetail.js.index','iconRole16','iconRole16','iconRole16','" + x.log_id + "','')\">Detail</button>";
                else
                    action = "";

                resultList.Add(new ExtJsDataRow_log()
                {
                    id = x.log_id,
                    href = "new com.embraiz.tag().openNewTag('EDIT_UID:" + x.crt_by + "','User: " + x.crt_by_name + "','com.palmary.user.js.edit','iconRole16','iconRole16','iconRole16', '" + x.crt_by + "')",
                    href1 = tragetObj_url,
                    action = action,
                    crt_by_type_name = x.crt_by_type_name,
                    crt_by_name = x.crt_by_name,
                    access_obj = x.crt_by,
                    action_channel_name = x.action_channel_name,
                    action_type_name = x.action_type_name,
                    target_obj_type_name = x.target_obj_type_name ?? "",
                    target_obj_name = x.target_obj_name ?? "",
                    target_obj = x.target_obj_id ?? 0,
                    action_ip = x.action_ip,
                    log_date = x.crt_date.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                });

            }

            return resultList.ToJson();
        }

        public string GetSummaryForm(int log_id)
        {
            var result = "";

            var logManager = new LogManager();
            var systemCode = CommonConstant.SystemCode.undefine;
            var logObj = logManager.GetDetail(log_id, ref systemCode);

            if (logObj.log_id > 0)
            {
                var logDetailManager = new LogDetailManager();

                var logDetail_list = logDetailManager.GetList(log_id, ref systemCode);

                var extTable = new ExtJsTable
                {
                    column = 2,
                    post_url = "../Table/ListData",
                    post_header = "../Table/GridHeader",
                    title = "Log Detail - Summary",
                    icon = "iconRole16",
                    post_params = "../User/Update", //将要返回来修改的action
                    isType = false, // no group
                    noToolBar = true,
                    button_text = "Save",
                    button_icon = "iconSave",
                    value_changes = true,
                };

                extTable.AddFieldLabelToRow(new ExtJsFieldLabelInput<string>(PayloadKeys.Dummy, logObj.crt_date.ToString("yyyy-MM-dd HH:mm:ss.fff"))
                {
                    fieldLabel = "Log Date",
                    colspan = 2
                });

                extTable.AddFieldLabelToRow(new ExtJsFieldLabelInput<string>(PayloadKeys.Dummy, logObj.crt_by_name)
                {
                    fieldLabel = "Access Object Name",
                    colspan = 2
                });

                extTable.AddFieldLabelToRow(new ExtJsFieldLabelInput<string>(PayloadKeys.Dummy, logObj.action_channel_name)
                {
                    fieldLabel = "Action Channel",
                    colspan = 2
                });

                extTable.AddFieldLabelToRow(new ExtJsFieldLabelInput<string>(PayloadKeys.Dummy, logObj.action_type_name)
                {
                    fieldLabel = "Action Type",
                    colspan = 2

                });

                extTable.AddFieldLabelToRow(new ExtJsFieldLabelInput<string>(PayloadKeys.Dummy, logObj.target_obj_type_name)
                {
                    fieldLabel = "Target Object Type",
                    colspan = 2
                });

                extTable.AddFieldLabelToRow(new ExtJsFieldLabelInput<string>(PayloadKeys.Dummy, logObj.target_obj_name)
                {
                    fieldLabel = "Target Object Name",
                    colspan = 2
                });

                extTable.AddFieldLabelToRow(new ExtJsFieldLabelInput<string>(PayloadKeys.Dummy, logObj.action_ip)
                {
                    fieldLabel = "Action IP",
                    colspan = 2
                });

                var hiddenLabel = new ExtJsFieldLabelHidden<int>(PayloadKeys.Id, logObj.log_id.ToString());
                extTable.AddFieldLabelToHiddenRow(hiddenLabel);

                result = extTable.ToJson();
            }


            return result;
        }

        public string GetDetailForm(int log_id)
        {
            var result = "";

            var logManager = new LogManager();
            var systemCode = CommonConstant.SystemCode.undefine;
            var logObj = logManager.GetDetail(log_id, ref systemCode);

            if (logObj.log_id > 0)
            {
                var logDetailManager = new LogDetailManager();

                var logDetail_list = logDetailManager.GetList(log_id, ref systemCode);

                var extTable = new ExtJsTable
                {
                    column = 2,
                    post_url = "../Table/ListData",
                    post_header = "../Table/GridHeader",
                    title = "Log Detail - Changes",
                    icon = "iconRole16",
                    post_params = "../User/Update", //将要返回来修改的action
                    isType = true,
                    noToolBar = true,
                    button_text = "Save",
                    button_icon = "iconSave",
                    value_changes = true,
                };

                // new value group
                for (int i = 0; i < logDetail_list.Count(); i++)
                {
                    var x = logDetail_list[i];

                    if (i == 0)
                    {
                        extTable.AddFieldLabelToRow(new ExtJsFieldLabelInput<string>(PayloadKeys.Dummy, x.new_value)
                        {
                            fieldLabel = x.field_name,
                            group = "New Value"
                        });
                    }
                    else
                    {
                        extTable.AddFieldLabelToRow(new ExtJsFieldLabelInput<string>(PayloadKeys.Dummy, x.new_value)
                        {
                            fieldLabel = x.field_name
                        });
                    }
                }

                // old value group
                // only for update, no need for create
                if (logObj.action_type == CommonConstant.ActionType.update)
                {
                    for (int i = 0; i < logDetail_list.Count(); i++)
                    {
                        var x = logDetail_list[i];

                        if (i == 0)
                        {
                            extTable.AddFieldLabelToRow(new ExtJsFieldLabelInput<string>(PayloadKeys.Dummy, x.old_value)
                            {
                                fieldLabel = x.field_name,
                                group = "Old Value"
                            });
                        }
                        else
                        {
                            extTable.AddFieldLabelToRow(new ExtJsFieldLabelInput<string>(PayloadKeys.Dummy, x.old_value)
                            {
                                fieldLabel = x.field_name
                            });
                        }
                    }
                }

                result = extTable.ToJson();
            }

            return result;
        }
    }
}