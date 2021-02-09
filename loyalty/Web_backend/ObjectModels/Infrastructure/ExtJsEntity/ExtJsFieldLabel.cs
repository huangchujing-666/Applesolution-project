using System;
using System.Collections.Generic;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.ExtJsEntity;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.PayoadKey;
using Palmary.Loyalty.BO.DataTransferObjects.Form;

namespace Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.ExtJsEntity
{
    // basic ExtJsFieldLabel class
    public class ExtJsFieldLabel : IExtJs
    {
        public string name { get; set; }
        public string type { get; set; }
        public string value { get; set; }
        public string regex { get; set; }
        public string regexText { get; set; }
        public string handler { get; set; }

        public string group { get; set; }
        public bool autoFitErrors { get; set; }
        public string fieldLabel { get; set; } //label內容
        public bool readOnly { get; set; }
        public bool allowBlank { get; set; } //是否可為空

        public int colspan { get; set; }
        public ExtJsFieldLabel(string name, FieldLabelType type, string value)
        {
            this.name = name;
            this.type = type.ToString();
            this.value = value;
        }
    }

    // ExtJsFieldLabelInput
    public class ExtJsFieldLabelInput<T> : ExtJsFieldLabel
    {
        public ExtJsFieldLabelInput(PayloadKey<T> name, string value)
            : base(name.ToString(), FieldLabelType.input, value)
        {
        }

        public int tabIndex { get; set; }
        
        public string datasource { get; set; }
        //public string inputType { get; set; }
    }

    public class ExtJsFieldLabelInputPassword<T> : ExtJsFieldLabel
    {
        public ExtJsFieldLabelInputPassword(PayloadKey<T> name, string value)
            : base(name.ToString(), FieldLabelType.passWord, value)
        {
        }

        public int tabIndex { get; set; }

    }

    public class ExtJsFieldLabelTextarea<T> : ExtJsFieldLabel
    {
        public ExtJsFieldLabelTextarea(PayloadKey<T> name, string value)
            : base(name.ToString(), FieldLabelType.textarea, value)
        {
        }

        public int tabIndex { get; set; }
        public string display_value { get; set; }
        public int height { get; set; }
    }

    public class ExtJsFieldLabelUpload<T> : ExtJsFieldLabel
    {
        public ExtJsFieldLabelUpload(PayloadKey<T> name, string value)
            : base(name.ToString(), FieldLabelType.upload, value)
        {
        }
        //name for POST name
        public string upload_url { get; set; }
        public string upType { get; set; } // img or file
        public string fileName { get; set; } // 
        public int tabIndex { get; set; }
        
        public int height { get; set; }
    }

    public class ExtJsFieldLabelNumber : ExtJsFieldLabel
    {
        public ExtJsFieldLabelNumber(PayloadKey<float> name, string value)
            : base(name.ToString(), FieldLabelType.input, value)
        {
            regex = "doubleRegex";
            regexText = "Please input number";
        }

        public string regex { get; set; }
        public string regexText { get; set; }
        public int tabIndex { get; set; }
        
    }

    public class ExtJsFieldLabelNumberStr : ExtJsFieldLabel
    {
        public ExtJsFieldLabelNumberStr(PayloadKey<string> name, string value)
            : base(name.ToString(), FieldLabelType.input, value)
        {
            regex = "doubleRegex";
            regexText = "Please input number";
        }

        public string regex { get; set; }
        public string regexText { get; set; }
        public int tabIndex { get; set; }
        
    }

    public class ExtJsFieldLabelInt : ExtJsFieldLabel
    {
        public ExtJsFieldLabelInt(PayloadKey<int> name, string value)
            : base(name.ToString(), FieldLabelType.input, value)
        {
            regex = "numberRegex";
            regexText = "Please input positive integer";
        }

        public string regex { get; set; }
        public string regexText { get; set; }
        public int tabIndex { get; set; }
        
    }

    public class ExtJsFieldLabelBool<T> : ExtJsFieldLabel
    {
        public IEnumerable<ExtJsFieldLabelBoolItem> items
        {
            get { return _items; }
        }
        private IList<ExtJsFieldLabelBoolItem> _items;
        public string display_value { get; set; }

        public ExtJsFieldLabelBool(PayloadKey<T> name, string value)
            : base(string.Format("{0}1", name), FieldLabelType.checkboxgroup, value)
        {
            _items = new List<ExtJsFieldLabelBoolItem>
                         {
                             new ExtJsFieldLabelBoolItem
                                 {
                                     name =name.ToString(),// string.Format("{0}", name),
                                     inputValue = "true",
                                     boxLabel="Y/N",
                                     @checked = value.Equals("1")
                                 }
            };
        }
    }

    public class ExtJsFieldLabelBoolItem
    {
        public string name { get; set; }
        public string inputValue { get; set; }
        public string boxLabel { get; set; }
        public bool @checked { get; set; }
    }

    public class ExtJsFieldLabelDate<T> : ExtJsFieldLabel
    {
        public ExtJsFieldLabelDate(PayloadKey<T> name, DateTime? value)
            : base(name.ToString(), FieldLabelType.date, value == null ? "" : value.Value.ToString("yyyy-MM-dd"))
        {

        }
    }

    public class ExtJsField_dateTime<T> : ExtJsFieldLabel
    {
        public ExtJsField_dateTime(PayloadKey<T> name, DateTime? value)
            : base(name.ToString(), FieldLabelType.dateTime, value == null || value.Value.ToString("yyyy") == "0001" ? "" : value.Value.ToString("yyyy-MM-dd HH:mm:ss"))
        {

        }
    }

    public class ExtJsField_time<T> : ExtJsFieldLabel
    {
        public ExtJsField_time(PayloadKey<T> name, string value)
            : base(name.ToString(), FieldLabelType.time, value)
        {

        }
    }

    public class ExtJsField_dateTime_noSec<T> : ExtJsFieldLabel
    {
        public ExtJsField_dateTime_noSec(PayloadKey<T> name, DateTime? value)
            : base(name.ToString(), FieldLabelType.dateTime, value == null || value.Value.ToString("yyyy") == "0001" ? "" : value.Value.ToString("yyyy-MM-dd HH:mm"))
        {

        }
    }

    public class ExtJsFieldLabelSelect<T> : ExtJsFieldLabel
    {
        public string datasource { get; set; }
        public string display_value { get; set; }
        public int tabIndex { get; set; }

        public ExtJsFieldLabelSelect(PayloadKey<T> name, string value)
            : base(name.ToString(), FieldLabelType.select, value)
        {
        }
    }

    public class ExtJsFieldLabelSelectSelect<T> : ExtJsFieldLabel
    {
        public string datasource { get; set; }
        public string display_value { get; set; }
        public int tabIndex { get; set; }
        
        // second select
        public string selectName { get; set; }
        public string selectFieldLabel { get; set; }
        public string display_value2 { get; set; }
        public string selectDatasource { get; set; }
        public string selectValue { get; set; }

        public ExtJsFieldLabelSelectSelect(PayloadKey<T> name, string value)
            : base(name.ToString(), FieldLabelType.select_select, value)
        {
        }
    }

    public class ExtJsFieldLabelSelectInput<T> : ExtJsFieldLabel
    {
        public string datasource { get; set; }
        public string display_value { get; set; }
        public int tabIndex { get; set; }

        public ExtJsFieldLabelSelectInput(PayloadKey<T> name, string value)
            : base(name.ToString(), FieldLabelType.select_input, value)
        {
        }
    }

    public class ExtJsFieldLabelSelectText<T> : ExtJsFieldLabel
    {
        public string datasource { get; set; }
        public string inputTextName { get; set; }
        public string textValue { get; set; }
        
        public ExtJsFieldLabelSelectText(PayloadKey<T> name, string value)
            : base(name.ToString(), FieldLabelType.select_text, value)
        {
        }
    }

    // select then query controller to change value of one specific input field
    public class ExtJsFieldLabelSelectAjaxText<T> : ExtJsFieldLabel
    {
        public string datasource { get; set; }
        public string inputFieldName { get; set; }
        public string ajaxURL { get; set; }
        
        public ExtJsFieldLabelSelectAjaxText(PayloadKey<T> name, string value)
            : base(name.ToString(), FieldLabelType.select_ajax_text, value)
        {
        }
    }

    // select then query controller to change value of three input fields
    public class ExtJsFieldLabelSelectAjaxThreeText<T> : ExtJsFieldLabel
    {
        public string datasource { get; set; }
        public string inputFieldName1 { get; set; }
        public string inputFieldName2 { get; set; }
        public string inputFieldName3 { get; set; }
        public string ajaxURL { get; set; }

        public ExtJsFieldLabelSelectAjaxThreeText(PayloadKey<T> name, string value)
            : base(name.ToString(), FieldLabelType.select_ajax_threeText, value)
        {
        }
    }

    // select then query controller to change value of four input fields
    public class ExtJsFieldLabelSelectAjaxFourText<T> : ExtJsFieldLabel
    {
	    public string datasource { get; set; }
	    public string inputFieldName1 { get; set; }
	    public string inputFieldName2 { get; set; }
	    public string inputFieldName3 { get; set; }
	    public string inputFieldName4 { get; set; }
	    public string ajaxURL { get; set; }

	    public ExtJsFieldLabelSelectAjaxFourText(PayloadKey<T> name, string value)
		    : base(name.ToString(), FieldLabelType.select_ajax_fourText, value)
	    {
	    }
    }

    public class ExtJsFieldLabelDateTime_threeInput<T> : ExtJsFieldLabel
    {
        public string datasource { get; set; }
        public string display_value { get; set; }
        public int tabIndex { get; set; }
        public string name2 { get; set; }
        public string value2 { get; set; }
        public string name3 { get; set; }
        public string value3 { get; set; }

        public ExtJsFieldLabelDateTime_threeInput(PayloadKey<T> name, string value)
            : base(name.ToString(), FieldLabelType.dateTime_threeInput, value)
        {
        }
    }

    public class ExtJsFieldLabel_inputNoDuplicate<T> : ExtJsFieldLabel
    {
        public string datasource { get; set; }
        public string display_value { get; set; }
        public int tabIndex { get; set; }
        public string check_path { get; set; }

        public ExtJsFieldLabel_inputNoDuplicate(PayloadKey<T> name, string value)
            : base(name.ToString(), FieldLabelType.InputNoDuplicate, value)
        {
        }
    }

    public class ExtJsField_dateRange<T> : ExtJsFieldLabel
    {
        public string toName { get; set; }
        public string toValue { get; set; }

        public ExtJsField_dateRange(PayloadKey<T> name, string value)
            : base(name.ToString(), FieldLabelType.dateRange, value)
        {
            this.name = name.ToString() + "_from";
            toName = name + "_to";
        }
    }

    public class ExtJsField_dateTimeRange<T> : ExtJsFieldLabel
    {
        public string toName { get; set; }
        public string toValue { get; set; }
        public string timeFromName { get; set; }
        public string timeToName { get; set; }

        public ExtJsField_dateTimeRange(PayloadKey<T> name, string value)
            : base(name.ToString(), FieldLabelType.dateTimeRange, value)
        {
            this.name = name.ToString() + "_from";
            toName = name + "_to";

            timeFromName = name + "_time_from";
            timeToName = name + "_time_to";
        }
    }

    public class ExtJsFieldLabelMultiSelect<T> : ExtJsFieldLabel
    {
        public string datasource { get; set; }
        public string display_value { get; set; }
        public int tabIndex { get; set; }
        public string[] value { get; set; }

        public ExtJsFieldLabelMultiSelect(PayloadKey<T> name, string value)
            : base(name.ToString(), FieldLabelType.multselect, value)
        {
        }
    }

    public class ExtJsFieldLabelMultiUploadDialog<T> : ExtJsFieldLabel
    {
        public string datasource { get; set; }
        public string display_value { get; set; }
        public int tabIndex { get; set; }

        public ExtJsFieldLabelMultiUploadDialog(PayloadKey<T> name, string value)
            : base(name.ToString(), FieldLabelType.multiUploadDialog, value)
        {
            images = new List<PhotoField>();
        }
        public List<PhotoField> images { get; set; }
        public string uploadUrl { get; set; }
        //:'upload.jsp',	
        public string editUrl { get; set; }
        //:'saveEdit.jsp',
        public string editDataUrl { get; set; }
        //:'editData.jsp',
        public string removeUrl { get; set; }
        //:'remove.jsp'

    }

    public class ExtJsFieldLabelHidden<T> : ExtJsFieldLabel
    {
        public ExtJsFieldLabelHidden(PayloadKey<T> name, string value)
            : base(name.ToString(), FieldLabelType.hidden, value)
        {
        }
    }


    public class ExtJsField_textEditor<T> : ExtJsFieldLabel
    {
        public ExtJsField_textEditor(PayloadKey<T> name, string value)
            : base(name.ToString(), FieldLabelType.texteditor, value)
        {

        }

        public int height { get; set; }
    }

    public class ExtJsField_monthDate<T> : ExtJsFieldLabel
    {
        public ExtJsField_monthDate(PayloadKey<T> name, string value)
            : base(name.ToString(), FieldLabelType.month_date, value)
        {

        }
    }

    public enum FieldLabelType
    {
        hidden, select, input, label, upload,
        date, dateTime, dateTime_threeInput, dateTimeRange, dateRange,
        checkboxgroup, multiUploadDialog, passWord, multselect, select_input, textarea,
        InputNoDuplicate, texteditor, select_text, select_ajax_text, select_ajax_threeText, select_ajax_fourText,
        time, month_date, select_select
    }
}