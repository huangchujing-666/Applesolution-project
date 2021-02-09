using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Web.Routing;

using Palmary.Loyalty.Common;
using Palmary.Loyalty.BO.Database;
using Palmary.Loyalty.BO.Modules.Transaction;
using Palmary.Loyalty.BO.DataTransferObjects.Security;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.ExtJsEntity;
using Palmary.Loyalty.Web_backend.Modules.GlobalHandlers;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.PayoadKey;
using Palmary.Loyalty.Web_backend.Modules.Utility;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.TableData;

namespace Palmary.Loyalty.Web_backend.Controllers
{
    [Authorize]
    public class PointTransferController : Controller
    {
        private int _id;

        public PointTransferController()
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

        public string GenerateForm()
        {
            var member_id = _id;

            //basic table data
            var extTable = new ExtJsTable
            {
                column = 2,
                post_url = "",
                post_header = "",
                title = "Config",
                icon = "iconRemarkList",
                post_params = Url.Action("Perform"),
                isType = true,
                button_text = "Save",
                button_icon = "iconSave",
                value_changes = true,
            };

            var rowFieldInput_int = new ExtJsFieldLabelInput<double>(PayloadKeys.PointTransfer.point, "")
            {
                fieldLabel = "Point Transfer",
                type = "input",
                colspan = 2,
                tabIndex = 1,
                allowBlank = false
            };
            extTable.AddFieldLabelToRow(rowFieldInput_int);

            var rowFieldInput_string = new ExtJsFieldLabelInput<string>(PayloadKeys.PointTransfer.to_member_no, "")
            {
                fieldLabel = "To (Member Code)",
                type = "input",
                colspan = 2,
                tabIndex = 1,
                allowBlank = false
            };
            extTable.AddFieldLabelToRow(rowFieldInput_string);

            var rowFieldTextArea = new ExtJsFieldLabelInput<string>(PayloadKeys.PointTransfer.remark, "")
            {   
                fieldLabel = "Remark",
                type = "textarea",
                colspan = 2,
                tabIndex = 1,
                allowBlank = true
            };
            extTable.AddFieldLabelToRow(rowFieldTextArea);

            // Hidden Fields
            var hiddenLabel = new ExtJsFieldLabelHidden<int>(PayloadKeys.PointTransfer.member_id, member_id.ToString());
            extTable.AddFieldLabelToHiddenRow(hiddenLabel);

            return extTable.ToJson();
        }

        public string ViewForm()
        {
            var transaction_id = _id;

            var systemCode = CommonConstant.SystemCode.undefine;
            var transactionManager = new TransactionManager();
            var transaction = transactionManager.GetDetail(transaction_id, true, ref systemCode);

            var from_member_no = "";
            var from_member_name = "";

            var to_member_no = "";
            var to_member_name = "";

            if (transaction.point_change > 0)
            {
                var from_transaction = transactionManager.GetDetail(transaction.source_id, true, ref systemCode);

                from_member_no = from_transaction.member_no;
                from_member_name = from_transaction.member_name;

                to_member_no = transaction.member_no;
                to_member_name = transaction.member_name;
            }
            else
            {
                var to_transaction = transactionManager.GetDetail(transaction.source_id, true, ref systemCode);

                from_member_no = transaction.member_no;
                from_member_name = transaction.member_name;

                to_member_no = to_transaction.member_no;
                to_member_name = to_transaction.member_name;
            }

            //basic table data
            var extTable = new ExtJsTable
            {
                column = 2,
                post_url = "",
                post_header = "",
                title = "Detail",
                icon = "iconRemarkList",
                //post_params = Url.Action("Perform"),
                isType = true,
                //button_text = "Save",     //no neet button
                //button_icon = "iconSave",
                value_changes = true,
            };

            var rowFieldInput_str = new ExtJsFieldLabelInput<string>(PayloadKeys.Dummy, from_member_no)
            {
                fieldLabel = "Member No (From)",
                type = "input",
                colspan = 2,
                tabIndex = 1,
                allowBlank = false,
                readOnly = true
            };
            extTable.AddFieldLabelToRow(rowFieldInput_str);

            rowFieldInput_str = new ExtJsFieldLabelInput<string>(PayloadKeys.Dummy, from_member_name)
            {
                fieldLabel = "Member Name (From)",
                type = "input",
                colspan = 2,
                tabIndex = 1,
                allowBlank = false,
                readOnly = true
            };
            extTable.AddFieldLabelToRow(rowFieldInput_str);

            rowFieldInput_str = new ExtJsFieldLabelInput<string>(PayloadKeys.Dummy, to_member_no)
            {
                fieldLabel = "Member No (To)",
                type = "input",
                colspan = 2,
                tabIndex = 1,
                allowBlank = false,
                readOnly = true
            };
            extTable.AddFieldLabelToRow(rowFieldInput_str);

            rowFieldInput_str = new ExtJsFieldLabelInput<string>(PayloadKeys.Dummy, to_member_name)
            {
                fieldLabel = "Member Name (To)",
                type = "input",
                colspan = 2,
                tabIndex = 1,
                allowBlank = false,
                readOnly = true
            };
            extTable.AddFieldLabelToRow(rowFieldInput_str);

            var change_value = transaction.point_change > 0 ? "+" + transaction.point_change : transaction.point_change.ToString();

            rowFieldInput_str = new ExtJsFieldLabelInput<string>(PayloadKeys.Dummy, change_value)
            {
                fieldLabel = "Point Change",
                type = "input",
                colspan = 2,
                tabIndex = 1,
                allowBlank = false,
                readOnly = true
            };
            extTable.AddFieldLabelToRow(rowFieldInput_str);

            rowFieldInput_str = new ExtJsFieldLabelInput<string>(PayloadKeys.Dummy, transaction.point_status_name)
            {
                fieldLabel = "Point Status",
                type = "input",
                colspan = 2,
                tabIndex = 1,
                allowBlank = false,
                readOnly = true
            };
            extTable.AddFieldLabelToRow(rowFieldInput_str);

            rowFieldInput_str = new ExtJsFieldLabelInput<string>(PayloadKeys.Dummy, transaction.point_expiry_date == null ? "" : transaction.point_expiry_date.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"))
            {
                fieldLabel = "Point Expiry Date",
                type = "input",
                colspan = 2,
                tabIndex = 1,
                allowBlank = false,
                readOnly = true
            };
            extTable.AddFieldLabelToRow(rowFieldInput_str);

            var rowFieldTextArea = new ExtJsFieldLabelInput<string>(PayloadKeys.PointAdjustment.remark, transaction.remark)
            {
                fieldLabel = "Remark",
                type = "textarea",
                colspan = 2,
                tabIndex = 1,
                allowBlank = true,
                readOnly = true
            };
            extTable.AddFieldLabelToRow(rowFieldTextArea);

            rowFieldInput_str = new ExtJsFieldLabelInput<string>(PayloadKeys.Dummy, transaction.crt_by_name)
            {
                fieldLabel = "Action By",
                type = "input",
                colspan = 2,
                tabIndex = 1,
                allowBlank = false,
                readOnly = true
            };
            extTable.AddFieldLabelToRow(rowFieldInput_str);

            rowFieldInput_str = new ExtJsFieldLabelInput<string>(PayloadKeys.Dummy, transaction.crt_date.ToString("yyyy-MM-dd HH:mm:ss.fff"))
            {
                fieldLabel = "Action Date",
                type = "input",
                colspan = 2,
                tabIndex = 1,
                allowBlank = false,
                readOnly = true
            };
            extTable.AddFieldLabelToRow(rowFieldInput_str);

            // Hidden Fields
            //var hiddenLabel = new ExtJsFieldLabelHidden<int>(PayloadKeys.PointAdjustment.member_id, member_id.ToString());
            //extTable.AddFieldLabelToHiddenRow(hiddenLabel);

            return extTable.ToJson();
        }

        public string Perform(FormCollection collection)
        {
            var result = "";
            
            var member_id = collection.GetFormValue(PayloadKeys.PointTransfer.member_id);
            var point_adjust = collection.GetFormValue(PayloadKeys.PointTransfer.point);
            var remark = collection.GetFormValue(PayloadKeys.PointTransfer.remark);
            var to_member_no = collection.GetFormValue(PayloadKeys.PointTransfer.to_member_no);
            var location_id = 0;

            var pointTransferManager = new PointTransferManager();
            var systemCode = pointTransferManager.Transfer(location_id, member_id, point_adjust, remark, to_member_no);

            if (systemCode == CommonConstant.SystemCode.normal)
                result = new { success = true, msg = "Point Transfer Complete" }.ToJson();
            else
                result = new { success = false, msg = "Point Transfer Fail" }.ToJson();

            return result;
        }
    }
}