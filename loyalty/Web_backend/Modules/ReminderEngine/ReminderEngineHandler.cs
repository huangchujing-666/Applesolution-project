using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Palmary.Loyalty.BO.DataTransferObjects.DB;
using Palmary.Loyalty.BO.DataTransferObjects.Reminder;
using Palmary.Loyalty.BO.Modules.Gift;
using Palmary.Loyalty.BO.Modules.Product;
using Palmary.Loyalty.BO.Modules.Reminder;
using Palmary.Loyalty.BO.Modules.Wifi;
using Palmary.Loyalty.Common;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.ExtJsEntity;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.PayoadKey;
using Palmary.Loyalty.Web_backend.Modules.Utility;

namespace Palmary.Loyalty.Web_backend.Modules.Reminder
{
    public class ReminderEngineHandler : IHandler
    {
        public string LoadListDataToExtJSJson(int prefix_id, int startRowIndex, int rowLimit, List<SearchParmObject> searchParmList, string sortColumn, CommonConstant.SortOrder sortOrder, ref int rowTotal)
        {
            var reminderEngineManager = new ReminderEngineManager();

            var resultCode = CommonConstant.SystemCode.undefine;
            var resultList = reminderEngineManager.GetList(startRowIndex, rowLimit, searchParmList, sortColumn, sortOrder, ref resultCode, ref rowTotal).ToList();
            var resultDataList = resultList.Select(
                x => new
                {
                    id = x.reminder_engine_id,
                    name = x.name,
                    type_name = x.target_type_name,
                    status_name = x.status_name,
                    href = "new com.embraiz.tag().openNewTag('EDIT_RE:" + x.reminder_engine_id.ToString() + "','Reminder Engine: " + x.name + "','com.palmary.reminderengine.js.edit','iconRole16','iconRole16','iconRole16', '" + x.reminder_engine_id + "')"
                }
            );

            return resultDataList.ToJson();
        }

        public string GetFormByModule(ReminderEngineObject obj)
        {
            var extTable = new ExtJsTable
            {
                column = 2,
                post_url = "../Table/ListData",
                post_header = "../Table/GridHeader",
                title = obj.reminder_engine_id == 0 ? "Create New Reminder" : "Reminder Detail",
                icon = "iconRole16",
                post_params = "../ReminderEngine/Update",
                //将要返回来修改的action

                isType = true,
                button_text = "Save",
                button_icon = "iconSave",
                value_changes = true,
                confirmSave = false
            };

            var rowFieldLabel_input = new ExtJsFieldLabelInput<string>(PayloadKeys.ReminderEngine.name, obj.name)
            {
                fieldLabel = "Name",
                tabIndex = 1,
                group = "General"
            };
            extTable.AddFieldLabelToRow(rowFieldLabel_input);

            var rowFieldSelect = new ExtJsFieldLabelSelect<int>(PayloadKeys.ReminderEngine.target_type, obj.target_type.ToString())
            {
                fieldLabel = "Type",
                datasource = "../Table/GetListItems/ReminderEngineType",
                display_value = obj.target_type_name
            };
            extTable.AddFieldLabelToRow(rowFieldSelect);

            rowFieldSelect = new ExtJsFieldLabelSelect<int>(PayloadKeys.ReminderEngine.status, obj.status.ToString())
            {
                fieldLabel = "Status",
                datasource = "../Table/GetListItems/Status",
                display_value = obj.status_name
            };
            extTable.AddFieldLabelToRow(rowFieldSelect);
            
            if (obj.target_type == (int)CommonConstant.ReminderEngineType.ProductPurchase && obj.reminder_engine_id>0)
            {
                var productManager = new ProductManager();
                var systemCode = CommonConstant.SystemCode.undefine;
                var theProduct = productManager.GetDetail(int.Parse(obj.target_value), true, ref systemCode);

                var theName = theProduct.name;
                rowFieldSelect = new ExtJsFieldLabelSelect<int>(PayloadKeys.ReminderEngine.target_value_purchase, obj.target_value.ToString())
                {
                    fieldLabel = "Product Purchase",
                    datasource = "../Table/GetListItems/product",
                    display_value = theName,
                    allowBlank = true
                };
                extTable.AddFieldLabelToRow(rowFieldSelect);
            }
            else if (obj.reminder_engine_id == 0)
            {
                rowFieldSelect = new ExtJsFieldLabelSelect<int>(PayloadKeys.ReminderEngine.target_value_purchase, "")
                {
                    fieldLabel = "Product Purchase",
                    datasource = "../Table/GetListItems/product",
                    display_value = "",
                    allowBlank = true
                };
                extTable.AddFieldLabelToRow(rowFieldSelect);

                var pruchase_valueList = new List<string> { };
                
                var field_multiSelect_intList = new ExtJsFieldLabelMultiSelect<string>(PayloadKeys.Gift.member_privilege, "")
                {
                    fieldLabel = "Member Level",
                    datasource = "../Table/GetListItems/MemberLevel",
                    display_value = "",
                    value = pruchase_valueList.ToArray(),
                    readOnly = false
                };
                extTable.AddFieldLabelToRow(field_multiSelect_intList);
            }

            if (obj.target_type == (int)CommonConstant.ReminderEngineType.GiftRedeem && obj.reminder_engine_id > 0)
            {
                var giftManager = new GiftManager();
                var theGift = giftManager.GetDetail(int.Parse(obj.target_value));

                var theName = theGift.name;
                rowFieldSelect = new ExtJsFieldLabelSelect<int>(PayloadKeys.ReminderEngine.target_value_redeem, obj.target_value.ToString())
                {
                    fieldLabel = "Gift Redeem",
                    datasource = "../Table/GetListItems/gift",
                    display_value = theName,
                    allowBlank = true
                };
                extTable.AddFieldLabelToRow(rowFieldSelect);
            }
            else if (obj.reminder_engine_id == 0)
            {
                rowFieldSelect = new ExtJsFieldLabelSelect<int>(PayloadKeys.ReminderEngine.target_value_redeem, obj.target_value.ToString())
                {
                    fieldLabel = "Gift Redeem",
                    datasource = "../Table/GetListItems/gift",
                    display_value = obj.target_type_name,
                    allowBlank = true
                };
                extTable.AddFieldLabelToRow(rowFieldSelect);
            }

            if (obj.target_type == (int)CommonConstant.ReminderEngineType.LocationVisit && obj.reminder_engine_id > 0)
            {
                var wifiLocationManager = new WifiLocationManager();
                var systemCode = CommonConstant.SystemCode.undefine;
                var theLocation = wifiLocationManager.GetDetail(int.Parse(obj.target_value), true, ref systemCode);

                var theName = theLocation.name;
                rowFieldSelect = new ExtJsFieldLabelSelect<int>(PayloadKeys.ReminderEngine.target_value_locationVisit, obj.target_value.ToString())
                {
                    fieldLabel = "Location Visit",
                    datasource = "../Table/GetListItems/location",
                    display_value = theName,
                    allowBlank = true
                };
                extTable.AddFieldLabelToRow(rowFieldSelect);
            }
            else if (obj.reminder_engine_id == 0)
            {
                rowFieldSelect = new ExtJsFieldLabelSelect<int>(PayloadKeys.ReminderEngine.target_value_locationVisit, obj.target_value.ToString())
                {
                    fieldLabel = "Location Visit",
                    datasource = "../Table/GetListItems/location",
                    display_value = obj.target_type_name,
                    allowBlank = true
                };
                extTable.AddFieldLabelToRow(rowFieldSelect);
            }

            if (obj.target_type == (int)CommonConstant.ReminderEngineType.MemberInactive && obj.reminder_engine_id > 0)
            {
                var theIntValue = int.Parse(obj.target_value);
                rowFieldSelect = new ExtJsFieldLabelSelect<int>(PayloadKeys.ReminderEngine.target_value_memberInactive, obj.target_value.ToString())
                {
                    fieldLabel = "Member Inactive",
                    datasource = "../Table/GetListItems/MemberInactiveType",
                    display_value = theIntValue.ToItemName("MemberInactiveType"),
                    allowBlank = true
                };
                extTable.AddFieldLabelToRow(rowFieldSelect);
            }
            else if (obj.reminder_engine_id == 0)
            {
                rowFieldSelect = new ExtJsFieldLabelSelect<int>(PayloadKeys.ReminderEngine.target_value_memberInactive, obj.target_value.ToString())
                {
                    fieldLabel = "Member Inactive",
                    datasource = "../Table/GetListItems/MemberInactiveType",
                    display_value = obj.target_type_name,
                    allowBlank = true
                };
                extTable.AddFieldLabelToRow(rowFieldSelect);
            }


            // schedule list----------------
            var scheduleList = new List<ReminderScheduleObject>();
            if (obj.reminder_engine_id > 0)
            {
                var reminderScheduleManager = new ReminderScheduleManager();
                var systemCode = CommonConstant.SystemCode.undefine;
                scheduleList = reminderScheduleManager.GetListByReminder(obj.reminder_engine_id, ref systemCode);

                int needAdd = 10 - scheduleList.Count();
                for (int i = 0; i < needAdd; i++)
                {
                    scheduleList.Add(new ReminderScheduleObject());
                }
            }
            else
            {
                for (int i = 0; i < 10; i++)
                {
                    scheduleList.Add(new ReminderScheduleObject());
                }
            }

            var template_typeStr = scheduleList[0].template_type == 0 ? "" : scheduleList[0].template_type.ToString();
            var template_idStr = scheduleList[0].template_id == 0 ? "" : scheduleList[0].template_id.ToString();
            var dayStr = template_typeStr == "" ? "" : scheduleList[0].day.ToString();
            var rowFieldLabel_input_int = new ExtJsFieldLabelInput<int>(PayloadKeys.ReminderEngine.day, dayStr)
            {
                fieldLabel = "After Day",
                tabIndex = 1,
                allowBlank = false,
                group = "Remind Schedule"
            };
            extTable.AddFieldLabelToRow(rowFieldLabel_input_int);

            var rowFieldSelectSelect = new ExtJsFieldLabelSelectSelect<int>(PayloadKeys.ReminderEngine.template_type, template_typeStr)
            {
                fieldLabel = "Template",
                datasource = "../Table/GetListItems/ReminderTemplateType",
                display_value = scheduleList[0].template_type_name,
                allowBlank = false,

                //second select
                selectName = PayloadKeys.ReminderEngine.template_id.ToString(),
                selectFieldLabel = "",
                display_value2 = scheduleList[0].template_name,
                selectDatasource = "../Table/GetListItems/remindertemplate",
                selectValue = template_idStr
            };
            extTable.AddFieldLabelToRow(rowFieldSelectSelect);

            template_typeStr = scheduleList[1].template_type == 0 ? "" : scheduleList[1].template_type.ToString();
            template_idStr = scheduleList[1].template_id == 0 ? "" : scheduleList[1].template_id.ToString();
            dayStr = template_typeStr == "" ? "" : scheduleList[1].day.ToString();
            extTable.AddFieldLabelToRow(new ExtJsFieldLabelInput<int>(PayloadKeys.ReminderEngine.day1, dayStr)
            {
                fieldLabel = "After Day",
                tabIndex = 1,
                allowBlank = true
            });

            extTable.AddFieldLabelToRow(new ExtJsFieldLabelSelectSelect<int>(PayloadKeys.ReminderEngine.template_type1, template_typeStr)
            {
                fieldLabel = "Template",
                datasource = "../Table/GetListItems/ReminderTemplateType",
                display_value = scheduleList[1].template_type_name,
                allowBlank = true,

                //second select
                selectName = PayloadKeys.ReminderEngine.template_id1.ToString(),
                selectFieldLabel = "",
                display_value2 = scheduleList[1].template_name,
                selectDatasource = "../Table/GetListItems/remindertemplate",
                selectValue = template_idStr
            });

            template_typeStr = scheduleList[2].template_type == 0 ? "" : scheduleList[2].template_type.ToString();
            template_idStr = scheduleList[2].template_id == 0 ? "" : scheduleList[2].template_id.ToString();
            dayStr = template_typeStr == "" ? "" : scheduleList[2].day.ToString();
            extTable.AddFieldLabelToRow(new ExtJsFieldLabelInput<int>(PayloadKeys.ReminderEngine.day2, dayStr)
            {
                fieldLabel = "After Day",
                tabIndex = 1,
                allowBlank = true
            });

            extTable.AddFieldLabelToRow(new ExtJsFieldLabelSelectSelect<int>(PayloadKeys.ReminderEngine.template_type2, template_typeStr)
            {
                fieldLabel = "Template",
                datasource = "../Table/GetListItems/ReminderTemplateType",
                display_value = scheduleList[2].template_type_name,
                allowBlank = true,

                //second select
                selectName = PayloadKeys.ReminderEngine.template_id2.ToString(),
                selectFieldLabel = "",
                display_value2 = scheduleList[2].template_name,
                selectDatasource = "../Table/GetListItems/remindertemplate",
                selectValue = template_idStr
            });

            template_typeStr = scheduleList[3].template_type == 0 ? "" : scheduleList[3].template_type.ToString();
            template_idStr = scheduleList[3].template_id == 0 ? "" : scheduleList[3].template_id.ToString();
            dayStr = template_typeStr == "" ? "" : scheduleList[3].day.ToString();
            extTable.AddFieldLabelToRow(new ExtJsFieldLabelInput<int>(PayloadKeys.ReminderEngine.day3, dayStr)
            {
                fieldLabel = "After Day",
                tabIndex = 1,
                allowBlank = true
            });

            extTable.AddFieldLabelToRow(new ExtJsFieldLabelSelectSelect<int>(PayloadKeys.ReminderEngine.template_type3, template_typeStr)
            {
                fieldLabel = "Template",
                datasource = "../Table/GetListItems/ReminderTemplateType",
                display_value = scheduleList[3].template_type_name,
                allowBlank = true,

                //second select
                selectName = PayloadKeys.ReminderEngine.template_id3.ToString(),
                selectFieldLabel = "",
                display_value2 = scheduleList[3].template_name,
                selectDatasource = "../Table/GetListItems/remindertemplate",
                selectValue = template_idStr
            });

            template_typeStr = scheduleList[4].template_type == 0 ? "" : scheduleList[4].template_type.ToString();
            template_idStr = scheduleList[4].template_id == 0 ? "" : scheduleList[4].template_id.ToString();
            dayStr = template_typeStr == "" ? "" : scheduleList[4].day.ToString();
            extTable.AddFieldLabelToRow(new ExtJsFieldLabelInput<int>(PayloadKeys.ReminderEngine.day4, dayStr)
            {
                fieldLabel = "After Day",
                tabIndex = 1,
                allowBlank = true
            });

            extTable.AddFieldLabelToRow(new ExtJsFieldLabelSelectSelect<int>(PayloadKeys.ReminderEngine.template_type4, template_typeStr)
            {
                fieldLabel = "Template",
                datasource = "../Table/GetListItems/ReminderTemplateType",
                display_value = scheduleList[4].template_type_name,
                allowBlank = true,

                //second select
                selectName = PayloadKeys.ReminderEngine.template_id4.ToString(),
                selectFieldLabel = "",
                display_value2 = scheduleList[4].template_name,
                selectDatasource = "../Table/GetListItems/remindertemplate",
                selectValue = template_idStr
            });

            template_typeStr = scheduleList[5].template_type == 0 ? "" : scheduleList[5].template_type.ToString();
            template_idStr = scheduleList[5].template_id == 0 ? "" : scheduleList[5].template_id.ToString();
            dayStr = template_typeStr == "" ? "" : scheduleList[5].day.ToString();
            extTable.AddFieldLabelToRow(new ExtJsFieldLabelInput<int>(PayloadKeys.ReminderEngine.day5, dayStr)
            {
                fieldLabel = "After Day",
                tabIndex = 1,
                allowBlank = true
            });

            extTable.AddFieldLabelToRow(new ExtJsFieldLabelSelectSelect<int>(PayloadKeys.ReminderEngine.template_type5, template_typeStr)
            {
                fieldLabel = "Template",
                datasource = "../Table/GetListItems/ReminderTemplateType",
                display_value = scheduleList[5].template_type_name,
                allowBlank = true,

                //second select
                selectName = PayloadKeys.ReminderEngine.template_id5.ToString(),
                selectFieldLabel = "",
                display_value2 = scheduleList[5].template_name,
                selectDatasource = "../Table/GetListItems/remindertemplate",
                selectValue = template_idStr
            });

            template_typeStr = scheduleList[6].template_type == 0 ? "" : scheduleList[6].template_type.ToString();
            template_idStr = scheduleList[6].template_id == 0 ? "" : scheduleList[6].template_id.ToString();
            dayStr = template_typeStr == "" ? "" : scheduleList[6].day.ToString();
            extTable.AddFieldLabelToRow(new ExtJsFieldLabelInput<int>(PayloadKeys.ReminderEngine.day6, dayStr)
            {
                fieldLabel = "After Day",
                tabIndex = 1,
                allowBlank = true
            });

            extTable.AddFieldLabelToRow(new ExtJsFieldLabelSelectSelect<int>(PayloadKeys.ReminderEngine.template_type6, template_typeStr)
            {
                fieldLabel = "Template",
                datasource = "../Table/GetListItems/ReminderTemplateType",
                display_value = scheduleList[6].template_type_name,
                allowBlank = true,

                //second select
                selectName = PayloadKeys.ReminderEngine.template_id6.ToString(),
                selectFieldLabel = "",
                display_value2 = scheduleList[6].template_name,
                selectDatasource = "../Table/GetListItems/remindertemplate",
                selectValue = template_idStr
            });

            template_typeStr = scheduleList[7].template_type == 0 ? "" : scheduleList[7].template_type.ToString();
            template_idStr = scheduleList[7].template_id == 0 ? "" : scheduleList[7].template_id.ToString();
            dayStr = template_typeStr == "" ? "" : scheduleList[7].day.ToString();
            extTable.AddFieldLabelToRow(new ExtJsFieldLabelInput<int>(PayloadKeys.ReminderEngine.day7, dayStr)
            {
                fieldLabel = "Before Day",
                tabIndex = 1,
                allowBlank = true
            });

            extTable.AddFieldLabelToRow(new ExtJsFieldLabelSelectSelect<int>(PayloadKeys.ReminderEngine.template_type7, template_typeStr)
            {
                fieldLabel = "Template",
                datasource = "../Table/GetListItems/ReminderTemplateType",
                display_value = scheduleList[7].template_type_name,
                allowBlank = true,

                //second select
                selectName = PayloadKeys.ReminderEngine.template_id7.ToString(),
                selectFieldLabel = "",
                display_value2 = scheduleList[7].template_name,
                selectDatasource = "../Table/GetListItems/remindertemplate",
                selectValue = template_idStr
            });

            template_typeStr = scheduleList[8].template_type == 0 ? "" : scheduleList[8].template_type.ToString();
            template_idStr = scheduleList[8].template_id == 0 ? "" : scheduleList[8].template_id.ToString();
            dayStr = template_typeStr == "" ? "" : scheduleList[8].day.ToString();
            extTable.AddFieldLabelToRow(new ExtJsFieldLabelInput<int>(PayloadKeys.ReminderEngine.day8, dayStr)
            {
                fieldLabel = "Before Day",
                tabIndex = 1,
                allowBlank = true
            });

            extTable.AddFieldLabelToRow(new ExtJsFieldLabelSelectSelect<int>(PayloadKeys.ReminderEngine.template_type8, template_typeStr)
            {
                fieldLabel = "Template",
                datasource = "../Table/GetListItems/ReminderTemplateType",
                display_value = scheduleList[8].template_type_name,
                allowBlank = true,

                //second select
                selectName = PayloadKeys.ReminderEngine.template_id8.ToString(),
                selectFieldLabel = "",
                display_value2 = scheduleList[8].template_name,
                selectDatasource = "../Table/GetListItems/remindertemplate",
                selectValue = template_idStr
            });

            template_typeStr = scheduleList[9].template_type == 0 ? "" : scheduleList[9].template_type.ToString();
            template_idStr = scheduleList[9].template_id == 0 ? "" : scheduleList[9].template_id.ToString();
            dayStr = template_typeStr == "" ? "" : scheduleList[9].day.ToString();
            extTable.AddFieldLabelToRow(new ExtJsFieldLabelInput<int>(PayloadKeys.ReminderEngine.day9, dayStr)
            {
                fieldLabel = "Before Day",
                tabIndex = 1,
                allowBlank = true
            });

            extTable.AddFieldLabelToRow(new ExtJsFieldLabelSelectSelect<int>(PayloadKeys.ReminderEngine.template_type9, template_typeStr)
            {
                fieldLabel = "Template",
                datasource = "../Table/GetListItems/ReminderTemplateType",
                display_value = scheduleList[9].template_type_name,
                allowBlank = true,

                //second select
                selectName = PayloadKeys.ReminderEngine.template_id9.ToString(),
                selectFieldLabel = "",
                display_value2 = scheduleList[9].template_name,
                selectDatasource = "../Table/GetListItems/remindertemplate",
                selectValue = template_idStr
            });

            var hiddenLabel = new ExtJsFieldLabelHidden<int>(PayloadKeys.Id, obj.reminder_engine_id.ToString());
            extTable.AddFieldLabelToHiddenRow(hiddenLabel);

            return extTable.ToJson();
        }
    }
}