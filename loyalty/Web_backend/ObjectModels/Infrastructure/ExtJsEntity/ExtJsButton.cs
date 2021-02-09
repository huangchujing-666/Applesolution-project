namespace Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.ExtJsEntity
{
    public class ExtJsButton
    {
        public ExtJsButton(string xtype, string name)
        {
            this.xtype = xtype;
            this.name = name;
        }

        public string xtype { get; set; }
        public string text { get; set; }
        public string name { get; set; }
        public bool hidden { get; set; }
        public string iconUrl { get; set; }
        public string href { get; set; }

        public string newTag_method { get; set; }

        public string newTag_id { get; set; }
        public string newTag_title { get; set; }
        public string newTag_url { get; set; }
        public string newTag_iconCls { get; set; }
        public string newTag_iconClsC { get; set; }
        public string newTag_iconClsE { get; set; }
        public string newTag_itemId { get; set; }
        public string newTag_extra { get; set; }

    }



    public class ExtJsGridButton
    {
        public ExtJsGridButton()
        { }

        public ExtJsGridButton(string xtype, string name)
        {
            this.xtype = xtype;
            this.name = name;
        }

        public string xtype { get; set; }
        public string text { get; set; }
        public string name { get; set; }
        public bool hidden { get; set; }
        public string iconCls { get; set; }
        public string handler { get; set; }
    }
}