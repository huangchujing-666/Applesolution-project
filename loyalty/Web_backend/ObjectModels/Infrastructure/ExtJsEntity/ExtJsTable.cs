using System.Collections.Generic;


namespace Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.ExtJsEntity
{
    public class ExtJsTable : IExtJs
    {
        private IList<ExtJsFieldLabel> rowfiledLabels;
        private IList<ExtJsFieldLabel> hiddenRowfiledLabels;

        public ExtJsTable()
        {
            this.rowfiledLabels = new List<ExtJsFieldLabel>();
            this.hiddenRowfiledLabels = new List<ExtJsFieldLabel>();
        }

        public int column { get; set; }
        public string post_url { get; set; }
        public string post_header { get; set; }
        public string title { get; set; }
        public string icon { get; set; }
        public string post_params { get; set; }
        public string button_text { get; set; }
        public string button_icon { get; set; }
        public bool value_changes { get; set; }
        public string sub_title { get; set; }
        public bool isType { get; set; }  // true: fields can display by group
        public bool noToolBar { get; set; } // true: position of form will be at the top
        public bool confirmSave { get; set; } // true: pop up confirm message box before save

        public IEnumerable<ExtJsFieldLabel> row
        {
            get { return rowfiledLabels; }
        }
        public IEnumerable<ExtJsFieldLabel> rowhidden
        {
            get { return hiddenRowfiledLabels; }
        }

        public void AddFieldLabelToRow(ExtJsFieldLabel label)
        {
            rowfiledLabels.Add(label);
        }
        public void AddFieldLabelToHiddenRow(ExtJsFieldLabel label)
        {
            hiddenRowfiledLabels.Add(label);
        }
    }
}