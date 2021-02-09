using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Palmary.Loyalty.Common;
using Palmary.Loyalty.BO.Modules.Member;
using Palmary.Loyalty.BO.Database;
using Palmary.Loyalty.BO.DataTransferObjects.Security;
using Palmary.Loyalty.BO.DataTransferObjects.Member;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.ExtJsEntity;
using Palmary.Loyalty.Web_backend.Modules.GlobalHandlers;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.PayoadKey;
using Palmary.Loyalty.Web_backend.Modules.Utility;
using Palmary.Loyalty.Web_backend.Modules.ExtJS;

using System.Web.Routing;

namespace Palmary.Loyalty.Web_backend.Controllers.Member
{
    [Authorize]
    public class MemberFieldController : Controller
    {
        private int _id;

        public MemberFieldController()
        {
        }

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            var id = RouteData.Values["id"] ?? "";
            if (!string.IsNullOrEmpty(id.ToString()))
            {
                _id = int.Parse(id.ToString());
            }

            SystemHandler.InitSystemParameters(this.HttpContext.ApplicationInstance as MvcApplication);
        }

        public string ViewBasicData()
        {
            var member_id = _id;

            var systemCode = CommonConstant.SystemCode.undefine;

            var memberManager = new MemberManager();
            var member = memberManager.GetDetail(member_id, true, ref systemCode);

            var memberCardManager = new MemberCardManager();
            var currentCard = memberCardManager.CurrentCard(member_id, ref systemCode);

            //basic table data
            var extTable = new ExtJsTable
            {
                column = 2,
                post_url = "",
                post_header = "",
                title = "Member Custom Field Usage",
                icon = "iconRemarkList",
                //post_params = Url.Action("Perform"),
                isType = true,
                //button_text = "Save",     //no neet button
                //button_icon = "iconSave",
                value_changes = true,
            };

            var rowFieldInput_str = new ExtJsFieldLabelInput<string>(PayloadKeys.Dummy, "Used 1 of 100")
            {
                fieldLabel = "NVARCHAR",
                type = "input",
                colspan = 2,
                tabIndex = 1,
                allowBlank = false,
                readOnly = true
            };
            extTable.AddFieldLabelToRow(rowFieldInput_str);

            rowFieldInput_str = new ExtJsFieldLabelInput<string>(PayloadKeys.Dummy, "Used 1 of 100")
            {
                fieldLabel = "DateTime",
                type = "input",
                colspan = 2,
                tabIndex = 1,
                allowBlank = false,
                readOnly = true
            };
            extTable.AddFieldLabelToRow(rowFieldInput_str);

            rowFieldInput_str = new ExtJsFieldLabelInput<string>(PayloadKeys.Dummy, "Used 1 of 100")
            {
                fieldLabel = "INT",
                type = "input",
                colspan = 2,
                tabIndex = 1,
                allowBlank = false,
                readOnly = true
            };
            extTable.AddFieldLabelToRow(rowFieldInput_str);

            rowFieldInput_str = new ExtJsFieldLabelInput<string>(PayloadKeys.Dummy, "Used 0 of 100")
            {
                fieldLabel = "Float",
                type = "input",
                colspan = 2,
                tabIndex = 1,
                allowBlank = false,
                readOnly = true
            };
            extTable.AddFieldLabelToRow(rowFieldInput_str);

            rowFieldInput_str = new ExtJsFieldLabelInput<string>(PayloadKeys.Dummy, "Used 0 of 100")
            {
                fieldLabel = "Select",
                type = "input",
                colspan = 2,
                tabIndex = 1,
                allowBlank = false,
                readOnly = true
            };
            extTable.AddFieldLabelToRow(rowFieldInput_str);

            rowFieldInput_str = new ExtJsFieldLabelInput<string>(PayloadKeys.Dummy, "Used 0 of 100")
            {
                fieldLabel = "Boolean",
                type = "input",
                colspan = 2,
                tabIndex = 1,
                allowBlank = false,
                readOnly = true
            };
            extTable.AddFieldLabelToRow(rowFieldInput_str);

            return extTable.ToJson();
        }

        public string ToolbarData()
        {
            var toolData = new List<ExtJsButton>();
            var button_name = "Add Field";

            toolData.Add(new ExtJsButton("button", "addfield") { text = button_name, iconUrl = "iconRole16", href = "function(){new com.embraiz.tag().open_pop_up('AddField:" + 0 + "','Add Field','com.palmary.memberfield.js.addform','iconRole16','iconRole16','iconRole16',9282,'owner','')}" });

            var result = new { toolData = toolData }.ToJson();

            // remove double quotation for herf:function
            result = result.Replace(@"""href"":""f", @"""href"":f");
            result = result.Replace(@"""}]}", @"}]}");
            result = result.Replace(@")}""},{", @")}},{");

            return result.ToJson();
        }

        public string AddForm()
        {
            //basic table data
            var extTable = new ExtJsTable
            {
                column = 2,
                post_url = "",
                post_header = "",
                title = "Config",
                icon = "iconRemarkList",
                post_params = Url.Action("PerformAdd"),
                isType = true,
                button_text = "Save",
                button_icon = "iconSave",
                value_changes = true,
            };

            var rowFieldInput_str = new ExtJsFieldLabelInput<string>(PayloadKeys.Dummy, "")
            {
                fieldLabel = "Display Name",
                type = "input",
                colspan = 2,
                tabIndex = 1,
               // readOnly = true
            };
            extTable.AddFieldLabelToRow(rowFieldInput_str);

            var rowFieldSelect = new ExtJsFieldLabelSelect<int>(PayloadKeys.ProductPurchase.product_id_2, "")
            {
                fieldLabel = "Data Type",
                datasource = "../Table/GetListItems/columndatatype",
                colspan = 1,
                allowBlank = true

            };
            extTable.AddFieldLabelToRow(rowFieldSelect);

            rowFieldInput_str = new ExtJsFieldLabelInput<string>(PayloadKeys.Dummy, "")
            {
                fieldLabel = "Data Length",
                type = "input",
                colspan = 2,
                tabIndex = 1,
                allowBlank = true
            };
            extTable.AddFieldLabelToRow(rowFieldInput_str);
           
            var rowFieldTextArea = new ExtJsFieldLabelInput<string>(PayloadKeys.MemberCard.remark, "")
            {
                fieldLabel = "Remark",
                type = "textarea",
                colspan = 2,
                tabIndex = 1,
                allowBlank = true
            };
            extTable.AddFieldLabelToRow(rowFieldTextArea);

         
            return extTable.ToJson();
        }

        public string PerformAdd()
        {
            return "{success:true,url:'',msg:'Record Added'}";
        }
    }
}
