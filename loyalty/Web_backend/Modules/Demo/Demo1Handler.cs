using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Palmary.Loyalty.Common;
using Palmary.Loyalty.BO.DataTransferObjects.DB;
using Palmary.Loyalty.Web_backend.Modules.GlobalHandlers;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.ExtJsEntity;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.PayoadKey;

namespace Palmary.Loyalty.Web_backend.Modules.Demo
{
    public class Demo1Handler : IHandler
    {
        public string LoadListDataToExtJSJson(int prefix_id, int startRowIndex, int rowLimit, List<SearchParmObject> searchParmList, string sortColumn, CommonConstant.SortOrder sortOrder, ref int rowTotal)
        {
            // Request BO Method
            //var demo1Manager = new Demo1Manager();
            //var resultCode = CommonConstant.SystemCode.undefine;
            //var resultList = demo1Manager.GetUserList(access_object_id, startRowIndex, rowLimit, searchParmList, sortColumn, sortOrder, ref resultCode, ref rowTotal).ToList();

            var resultList = new List<ExtJsDemo1>();

            var fileHandler = new FileHandler();

            resultList.Add(new ExtJsDemo1()
            {
                id = 1,
                href = "new com.embraiz.tag().openNewTag('EDIT_D:" + 1 + "','Demo1: my name','com.palmary.demo1.js.edit','iconRole16','iconRole16','iconRole16','" + 1 + "')",
                demo_id = "1",
                demo_image = fileHandler.GetImagePath("20140212143257_0765", ".jpg", (string)CommonConstant.Module.demo1, (int)CommonConstant.ImageSizeType.thumb),
                demo_name = "my name",
                demo_float = "1.23",
                demo_select_name = "my option",
                demo_date = DateTime.Now.ToString("yyyy-MM-dd"),

                // demo_datetime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff"),
                //demo_remark = "my remark"
            }
            );

            resultList.Add(new ExtJsDemo1()
            {
                id = 2,
                href = "new com.embraiz.tag().openNewTag('EDIT_D:" + 1 + "','Demo1: my name','com.palmary.demo1.js.edit','iconRole16','iconRole16','iconRole16','" + 1 + "')",
                demo_id = "2",
                demo_image = fileHandler.GetImagePath("20140212143257_0765", ".jpg", (string)CommonConstant.Module.demo1, (int)CommonConstant.ImageSizeType.thumb),
                demo_name = "my name",
                demo_float = "1.23",
                demo_select_name = "my option",
                demo_date = DateTime.Now.ToString("yyyy-MM-dd"),
                demo_datetime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff"),
                demo_remark = "my remark"
            }
            );

            resultList.Add(new ExtJsDemo1()
            {
                id = 3,
                href = "new com.embraiz.tag().openNewTag('EDIT_D:" + 1 + "','Demo1: my name','com.palmary.demo1.js.edit','iconRole16','iconRole16','iconRole16','" + 1 + "')",
                demo_id = "3",
                demo_image = fileHandler.GetImagePath("20140212143257_0765", ".jpg", (string)CommonConstant.Module.demo1, (int)CommonConstant.ImageSizeType.thumb),
                demo_name = "my name",
                demo_float = "1.23",
                demo_select_name = "my option",
                demo_date = DateTime.Now.ToString("yyyy-MM-dd"),
                demo_datetime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff"),
                demo_remark = "my remark"
            }
            );

            return resultList.ToJson();
        }

        public string GetFormByModule(Demo1Object obj)
        {
            // basic table data
            var extTable = new ExtJsTable
            {
                post_url = "../Table/ListData",
                post_header = "../Table/GridHeader",
                title = obj.demo_id == 0 ? "Create Demo" : "Edit Demo",
                icon = "iconRole16",
                post_params = "../Role/Update",
                isType = true,
                button_text = "Save",
                button_icon = "iconSave",
                value_changes = true,
            };

            //add general row into the table
            var field_label_str = new ExtJsFieldLabelInput<string>(PayloadKeys.Dummy, obj.demo_id.ToString())
            {
                fieldLabel = "Demo ID",
                group = "General",
                readOnly = true
            };
            extTable.AddFieldLabelToRow(field_label_str);

            field_label_str = new ExtJsFieldLabelInput<string>(PayloadKeys.Demo1.demo_name, obj.demo_name)
            {
                fieldLabel = "Demo Name",
            };
            extTable.AddFieldLabelToRow(field_label_str);

            var field_label_float = new ExtJsFieldLabelInput<float>(PayloadKeys.Demo1.demo_float, obj.demo_float.ToString())
            {
                fieldLabel = "Demo Float",
            };
            extTable.AddFieldLabelToRow(field_label_float);

            var field_select = new ExtJsFieldLabelSelect<int>(PayloadKeys.Demo1.demo_select, obj.demo_select.ToString())
            {
                fieldLabel = "Demo Select",
                datasource = "../Table/GetListItems/status",
                display_value = obj.demo_select_name,
            };
            extTable.AddFieldLabelToRow(field_select);

            var field_date = new ExtJsFieldLabelDate<DateTime>(PayloadKeys.Demo1.demo_date, obj.demo_date)
            {
                fieldLabel = "Demo Date",
                colspan = 2
            };
            extTable.AddFieldLabelToRow(field_date);

            var field_datetime_noSec = new ExtJsField_dateTime_noSec<DateTime>(PayloadKeys.Demo1.demo_datetime, obj.demo_datetime)
            {
                fieldLabel = "Demo DateTime",
            };
            extTable.AddFieldLabelToRow(field_datetime_noSec);

            var photoFileName_withPath = "";

            if (!String.IsNullOrWhiteSpace(obj.photo_file_name))
            {
                FileHandler _fileHandler = new FileHandler();
                photoFileName_withPath = _fileHandler.GetImagePath(obj.photo_file_name, obj.photo_file_extension, (string)CommonConstant.Module.demo1, (int)CommonConstant.ImageSizeType.thumb);
            }
            var field_uplaod = new ExtJsFieldLabelUpload<string>(PayloadKeys.Location.photo_file_name, photoFileName_withPath)
            {
                fieldLabel = "Photo",
                upType = "img",  //img or file
                name = "fileData", //Post name
                //value = photoFileName_withPath, //location.photo_file_name + location.photo_file_extension,
                upload_url = "/DemoPhoto/Upload",
                allowBlank = false,
                group = "More",
            };
            extTable.AddFieldLabelToRow(field_uplaod);

            var field_textarea = new ExtJsFieldLabelTextarea<string>(PayloadKeys.Demo1.demo_remark, obj.demo_remark)
            {
                fieldLabel = "Demo Remark",
                allowBlank = true
            };
            extTable.AddFieldLabelToRow(field_textarea);

            var field_textEditor = new ExtJsField_textEditor<string>(PayloadKeys.Demo1.demo_remark2, obj.demo_remark2)
            {
                fieldLabel = "remark2"

            };
            extTable.AddFieldLabelToRow(field_textEditor);

            // start test
            var test = new ExtJsFieldLabelSelectAjaxText<int>(PayloadKeys.Demo1.test, "1")
            {
                fieldLabel = "Test",
                datasource = "../Table/GetListItems/status",
                inputFieldName = "point",
                ajaxURL = "../WifiLocation/Test",
                allowBlank = false
            };
            extTable.AddFieldLabelToRow(test);

            var testAjaxThree = new ExtJsFieldLabelSelectAjaxFourText<int>(PayloadKeys.Demo1.test, "1")
            {
                fieldLabel = "Select Ajax 4 Test",
                datasource = "../Table/GetListItems/status",
                inputFieldName1 = PayloadKeys.Demo1.testAjax1.ToString(),
                inputFieldName2 = PayloadKeys.Demo1.testAjax2.ToString(),
                inputFieldName3 = PayloadKeys.Demo1.testAjax3.ToString(),
                inputFieldName4 = PayloadKeys.Demo1.testAjax4.ToString(),
                ajaxURL = "../WifiLocation/TestAjaxFour",
                allowBlank = false
            };
            extTable.AddFieldLabelToRow(testAjaxThree);

            extTable.AddFieldLabelToRow(new ExtJsFieldLabelInput<string>(PayloadKeys.Demo1.testAjax1, "")
            {
                fieldLabel = "Ajax1",
                allowBlank = false
            });

            extTable.AddFieldLabelToRow(new ExtJsFieldLabelInput<string>(PayloadKeys.Demo1.testAjax2, "")
            {
                fieldLabel = "Ajax2",
                allowBlank = false
            });

            extTable.AddFieldLabelToRow(new ExtJsFieldLabelInput<string>(PayloadKeys.Demo1.testAjax3, "")
            {
                fieldLabel = "Ajax3",
                allowBlank = false
            });
            extTable.AddFieldLabelToRow(new ExtJsFieldLabelInput<string>(PayloadKeys.Demo1.testAjax4, "")
            {
                fieldLabel = "Ajax4",
                allowBlank = false
            });

            DateTime current = DateTime.Now;
            var field_time = new ExtJsField_time<string>(PayloadKeys.Demo1.test_time, "01:39")
            {
                fieldLabel = "Time Only",
                allowBlank = false
            };
            extTable.AddFieldLabelToRow(field_time);

            field_label_str = new ExtJsFieldLabelInput<string>(PayloadKeys.Dummy, "123456789")
            {
                fieldLabel = "Read Only",
                readOnly = true
            };
            extTable.AddFieldLabelToRow(field_label_str);

            field_label_str = new ExtJsFieldLabelInput<string>(PayloadKeys.Dummy, "abcdefghijkl")
            {
                fieldLabel = "Can edit"
            };
            extTable.AddFieldLabelToRow(field_label_str);
            // [END] test
            // hidden table data =================================
            var hiddenLabel = new ExtJsFieldLabelHidden<int>(PayloadKeys.Demo1.demo_id, obj.demo_id.ToString());
            extTable.AddFieldLabelToHiddenRow(hiddenLabel);

            return extTable.ToJson();
        }
    }

    public class ExtJsDemo1
    {
        public int id { get; set; }
        public string href { get; set; }
        public string demo_image { get; set; }
        public string demo_id { get; set; }
        public string demo_name { get; set; }
        public string demo_float { get; set; }
        public string demo_select_name { get; set; }
        public string demo_date { get; set; }
        public string demo_datetime { get; set; }
        public string demo_remark { get; set; }
    }

    public class ExtJsLocker
    {
        public int id { get; set; }
        public string href { get; set; }

        public string locker_id { get; set; }
        public string school_id { get; set; }
        public string locker_no { get; set; }
        public string rental_type_name { get; set; }
        public string remark { get; set; }
        public string status_name { get; set; }
    }

    public class Demo1Object
    {
        public int id { get; set; }

        public string demo_image { get; set; }
        public int demo_id { get; set; }
        public string demo_name { get; set; }
        public float demo_float { get; set; }
        public int demo_select { get; set; }
        public string demo_select_name { get; set; }
        public DateTime demo_date { get; set; }
        public DateTime demo_datetime { get; set; }
        public string demo_remark { get; set; }
        public string demo_remark2 { get; set; }

        public string photo_file_name { get; set; }
        public string photo_file_extension { get; set; }
    }

}