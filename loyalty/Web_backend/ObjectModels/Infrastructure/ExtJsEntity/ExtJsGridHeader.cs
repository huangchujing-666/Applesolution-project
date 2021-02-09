using System.Collections.Generic;
using System.Linq;

namespace Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.ExtJsEntity
{
    public class ExtJsGridHeader
    {
        public ExtJsGridHeader()
        {
            _columns=new List<ExtJsGridColumn>();
            _fields=new List<string>();
        }

        private IList<ExtJsGridColumn> _columns;
        private List<string> _fields;
        public bool success { get; set; }
        public string title { get; set; }
        public int pageSize { get; set; }
        public string add_url { get; set; }
        public string iconSrc { get; set; }
        public bool add_hidden { get; set; }
        public bool search_text_hidden { get; set; }
        public string delete_url { get; set; }
        public bool delete_hidden { get; set; }
        public bool checkbox_hidden { get; set; }
        public IEnumerable<ExtJsGridColumn> columns { get { return _columns; } }
        public IEnumerable<string> fields { get { return _fields; } }

        public void AddColumn (ExtJsGridColumn column)
        {
            _columns.Add(column);
        }

        public void AddField(string field)
        {
            _fields.Add(field);
        }
        public void AddField(IList<string> fields)
        {
            _fields.AddRange(fields);
        }
    }

    public class ExtJsGridColumn
    {
        public string header { get; set; }
        public string dataIndex { get; set; }
        public int width { get; set; }
        public string type { get; set; }
        public string renderer { get; set; }
        public bool sortable { get; set; }
    }
}