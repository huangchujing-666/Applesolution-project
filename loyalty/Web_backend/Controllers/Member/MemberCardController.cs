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
    public class MemberCardController : Controller
    {
        private int _id;

        public MemberCardController()
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

        public string Insert()
        {
            var formTableJSON = TableFormHandler.GetFormByModule(new MemberCardObject());
            return formTableJSON;
        }

        public string Update(FormCollection collection)
        {
            var card_id = collection.GetFormValue(PayloadKeys.MemberCard.card_id);
            var member_id = collection.GetFormValue(PayloadKeys.MemberCard.member_id);
            var member_no = collection.GetFormValue(PayloadKeys.MemberCard.member_no);
            var card_no = collection.GetFormValue(PayloadKeys.MemberCard.card_no);
            var card_version = collection.GetFormValue(PayloadKeys.MemberCard.card_version);
            var card_type = collection.GetFormValue(PayloadKeys.MemberCard.card_type);
            var card_status = collection.GetFormValue(PayloadKeys.MemberCard.card_status);
            var issue_date = collection.GetFormValue(PayloadKeys.MemberCard.issue_date);
            var old_card_id = collection.GetFormValue(PayloadKeys.MemberCard.old_card_id);
            var remark = collection.GetFormValue(PayloadKeys.MemberCard.remark);
            var status = collection.GetFormValue(PayloadKeys.MemberCard.status);

            MemberCardObject cardObj;
            var memberCardManager = new MemberCardManager();

            if (card_id == 0)
            {
                cardObj = new MemberCardObject()
                {
                    card_id = card_id,
                    member_id = member_id,
                    member_no = member_no,
                    card_no = card_no,
                    card_version = card_version,
                    card_type = card_type,
                    card_status = card_status,
                    issue_date = issue_date,
                    old_card_id = old_card_id,
                    remark = remark,
                    status = status
                };
            }
            else
            {
                var systemCode = CommonConstant.SystemCode.undefine;
                cardObj = memberCardManager.GetDetail(card_id, ref systemCode);
                cardObj.remark = remark;
                cardObj.card_status = card_status;
            }

            if (card_id == 0)
            {
                var systemCode = memberCardManager.Create(cardObj);
                
                var result = false;
                var msg = "";

                if (systemCode == CommonConstant.SystemCode.normal)
                {
                    result = true;
                    msg = "Save Success";
                }
                else if (systemCode == CommonConstant.SystemCode.err_member_id)
                    msg = "Incorrect Member No";
                else if (systemCode == CommonConstant.SystemCode.err_member_card_existusing)
                    msg = "Member has active member card";
                else if (systemCode == CommonConstant.SystemCode.err_member_cardno_duplicate)
                    msg = "Duplicate card no";
                else if (systemCode == CommonConstant.SystemCode.record_invalid)
                    msg = "Data Invalid";
                else if (systemCode == CommonConstant.SystemCode.no_permission)
                    msg = "No Permission";

                return new { success = result, url = "", msg = msg }.ToJson();
            }
            else if (card_id > 0)
            {
                var systemCode = memberCardManager.Update(
                    cardObj);

                return systemCode == CommonConstant.SystemCode.normal ? "{success:true,url:'',msg:'Saved Success'}" : "{success:true,url:'',msg:'Saved Failed'}";
            }
            else
                return "{success:true,url:'',msg:'Saved Failed: Invalid Data'}";
        }

        public string PerformIssue(FormCollection collection)
        {
            var member_id = collection.GetFormValue(PayloadKeys.MemberCard.member_id);
            var remark = collection.GetFormValue(PayloadKeys.MemberCard.remark);
            
            var memberCardManager = new MemberCardManager();

            var cardObj = new MemberCardObject()
            {
                member_id = member_id,
                remark = remark
            };

            var systemCode = memberCardManager.IssueAndReissue(cardObj);

            return systemCode == CommonConstant.SystemCode.normal ? "{success:true,url:'',msg:'Saved Success'}" : "{success:true,url:'',msg:'Saved Failed'}";
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
                title = "Member Card Detail",
                icon = "iconRemarkList",
                //post_params = Url.Action("Perform"),
                isType = true,
                //button_text = "Save",     //no neet button
                //button_icon = "iconSave",
                value_changes = true,
            };

            var rowFieldInput_str = new ExtJsFieldLabelInput<string>(PayloadKeys.Dummy, member.member_no)
            {
                fieldLabel = "Member Code",
                type = "input",
                colspan = 2,
                tabIndex = 1,
                allowBlank = false,
                readOnly = true
            };
            extTable.AddFieldLabelToRow(rowFieldInput_str);

            rowFieldInput_str = new ExtJsFieldLabelInput<string>(PayloadKeys.Dummy, member.GetFullname())
            {
                fieldLabel = "Member Name",
                type = "input",
                colspan = 2,
                tabIndex = 1,
                allowBlank = false,
                readOnly = true
            };
            extTable.AddFieldLabelToRow(rowFieldInput_str);

            rowFieldInput_str = new ExtJsFieldLabelInput<string>(PayloadKeys.Dummy, currentCard.card_no)
            {
                fieldLabel = "Current Card No",
                type = "input",
                colspan = 2,
                tabIndex = 1,
                allowBlank = false,
                readOnly = true
            };
            extTable.AddFieldLabelToRow(rowFieldInput_str);

            rowFieldInput_str = new ExtJsFieldLabelInput<string>(PayloadKeys.Dummy, currentCard.card_type_name)
            {
                fieldLabel = "Current Card Type",
                type = "input",
                colspan = 2,
                tabIndex = 1,
                allowBlank = false,
                readOnly = true
            };
            extTable.AddFieldLabelToRow(rowFieldInput_str);

            rowFieldInput_str = new ExtJsFieldLabelInput<string>(PayloadKeys.Dummy, currentCard.card_status_name)
            {
                fieldLabel = "Current Card Status",
                type = "input",
                colspan = 2,
                tabIndex = 1,
                allowBlank = false,
                readOnly = true
            };
            extTable.AddFieldLabelToRow(rowFieldInput_str);

            var date_str = "";

            if (currentCard.issue_date != null)
                date_str = currentCard.issue_date.Value.ToString("yyyy-MM-dd HH:mm:ss.fff");

            rowFieldInput_str = new ExtJsFieldLabelInput<string>(PayloadKeys.Dummy, date_str)
            {
                fieldLabel = "Current Card Issue Date",
                type = "input",
                colspan = 2,
                tabIndex = 1,
                allowBlank = false,
                readOnly = true
            };
            extTable.AddFieldLabelToRow(rowFieldInput_str);

            rowFieldInput_str = new ExtJsFieldLabelInput<string>(PayloadKeys.Dummy, currentCard.remark)
            {
                fieldLabel = "Current Card Remark",
                type = "input",
                colspan = 2,
                tabIndex = 1,
                allowBlank = false,
                readOnly = true
            };
            extTable.AddFieldLabelToRow(rowFieldInput_str);

            return extTable.ToJson();
        }


        public string UpdateForm()
        {
            var card_id = _id;

            var systemCode = CommonConstant.SystemCode.undefine;
            var memberCardManager = new MemberCardManager();
            var card = memberCardManager.GetDetail(card_id, ref systemCode);

            //basic table data
            var extTable = new ExtJsTable
            {
                column = 2,
                post_url = "",
                post_header = "",
                title = "Config",
                icon = "iconRemarkList",
                post_params = Url.Action("Update"),
                isType = true,
                button_text = "Save",
                button_icon = "iconSave",
                value_changes = true,
            };

            var rowFieldInput_str = new ExtJsFieldLabelInput<string>(PayloadKeys.MemberCard.card_no, card.card_no)
            {
                fieldLabel = "Card No",
                type = "input",
                colspan = 2,
                tabIndex = 1,
                readOnly = true
            };
            extTable.AddFieldLabelToRow(rowFieldInput_str);

            rowFieldInput_str = new ExtJsFieldLabelInput<string>(PayloadKeys.MemberCard.member_no, card.member_no)
            {
                fieldLabel = "Member No",
                type = "input",
                colspan = 2,
                tabIndex = 1,
                readOnly = true
            };
            extTable.AddFieldLabelToRow(rowFieldInput_str);

            var rowFieldSelect = new ExtJsFieldLabelSelect<int>(PayloadKeys.MemberCard.card_status, card.card_status.ToString())
            {
                fieldLabel = "Card Status",
                datasource = "../Table/GetListItems/MemberCardStatus",
                colspan = 2,
                allowBlank = true
            };
            extTable.AddFieldLabelToRow(rowFieldSelect);

            var rowFieldTextArea = new ExtJsFieldLabelInput<string>(PayloadKeys.MemberCard.remark, card.remark)
            {
                fieldLabel = "Remark",
                type = "textarea",
                colspan = 2,
                tabIndex = 1,
                allowBlank = true
            };
            extTable.AddFieldLabelToRow(rowFieldTextArea);

            // Hidden Fields
            var hiddenLabel = new ExtJsFieldLabelHidden<int>(PayloadKeys.MemberCard.card_id, card.card_id.ToString());
            extTable.AddFieldLabelToHiddenRow(hiddenLabel);

            return extTable.ToJson();
        }

        public string IssueForm()
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
                title = "Config",
                icon = "iconRemarkList",
                post_params = Url.Action("PerformIssue"),
                isType = true,
                button_text = "Save",
                button_icon = "iconSave",
                value_changes = true,
            };

            var rowFieldInput_str = new ExtJsFieldLabelInput<string>(PayloadKeys.MemberCard.member_no, member.member_no)
            {
                fieldLabel = "Member No",
                type = "input",
                colspan = 2,
                tabIndex = 1,
                readOnly = true
            };
            extTable.AddFieldLabelToRow(rowFieldInput_str);

            var action = "Create a new card";
            if (currentCard.card_id > 0)
                action = "Re-issue card and void old card";

            rowFieldInput_str = new ExtJsFieldLabelInput<string>(PayloadKeys.Dummy, action)
            {
                fieldLabel = "Action",
                type = "input",
                colspan = 2,
                tabIndex = 1,
                readOnly = true
            };
            extTable.AddFieldLabelToRow(rowFieldInput_str);

            if (currentCard.card_id > 0)
            {
                rowFieldInput_str = new ExtJsFieldLabelInput<string>(PayloadKeys.Dummy, currentCard.card_no)
                {
                    fieldLabel = "Current Card No",
                    type = "input",
                    colspan = 2,
                    tabIndex = 1,
                    readOnly = true
                };
                extTable.AddFieldLabelToRow(rowFieldInput_str);
            }
            var rowFieldTextArea = new ExtJsFieldLabelInput<string>(PayloadKeys.MemberCard.remark, "")
            {
                fieldLabel = "Remark",
                type = "textarea",
                colspan = 2,
                tabIndex = 1,
                allowBlank = true
            };
            extTable.AddFieldLabelToRow(rowFieldTextArea);

            // Hidden Fields
            var hiddenLabel = new ExtJsFieldLabelHidden<int>(PayloadKeys.MemberCard.member_id, member_id.ToString());
            extTable.AddFieldLabelToHiddenRow(hiddenLabel);

            return extTable.ToJson();
        }

        public string ToolbarData()
        {
            var member_id = _id;

            var toolData = new List<ExtJsButton>();
            var button_name = "Create Card";

            var memberCardManager = new MemberCardManager();
            var systemCode = CommonConstant.SystemCode.undefine;

            var currentCard = memberCardManager.CurrentCard(member_id, ref systemCode);
            if (currentCard.card_id > 0)
                button_name = "Re-issue Card";

            toolData.Add(new ExtJsButton("button", "membercard") { text = button_name, iconUrl = "iconRole16", href = "function(){new com.embraiz.tag().open_pop_up('MemberCard:" + member_id + "','Member Card','com.palmary.membercard.js.issueform','iconRole16','iconRole16','iconRole16',9282,'owner','')}" });

            var result = new { toolData = toolData }.ToJson();

            // remove double quotation for herf:function
            result = result.Replace(@"""href"":""f", @"""href"":f");
            result = result.Replace(@"""}]}", @"}]}");
            result = result.Replace(@")}""},{", @")}},{");

            return result.ToJson();
        }
    }
}
