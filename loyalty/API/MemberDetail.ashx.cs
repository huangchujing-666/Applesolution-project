using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Palmary.Loyalty.Common;
using Palmary.Loyalty.BO.Modules.Member;
using Palmary.Loyalty.BO.Database;

namespace Palmary.Loyalty.API
{
    /// <summary>
    /// Summary description for MemberLogin
    /// </summary>
    public class MemberDetail : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            
            System.Diagnostics.Debug.WriteLine("[API] MemberDetail");

            System.Diagnostics.Debug.WriteLine("member_id: " + context.Request.Form["member_id"]);
            
            if (String.IsNullOrWhiteSpace(context.Request.Form["member_id"]) ||
                String.IsNullOrWhiteSpace(context.Request.Form["session"]))
            {
                context.Response.Write("{\"success\":false,\"msg\":\"Null or empty value\"}");
            }
            else
            {
                //By POST
                #region Parameters Binding/Checking
                var member_id = int.Parse(context.Request.Form["member_id"]);
                var session = context.Request.Form["session"];
                #endregion

                System.Diagnostics.Debug.WriteLine("member_id: " + member_id + " session: " + session);

                var _memberManager = new MemberManager();
                
                var user_id = 0;
                var resultCode = CommonConstant.SystemCode.undefine;
                var member = _memberManager.GetDetail(member_id, false, ref resultCode);

                var resultJSON = "";

              
                if (member.member_id>0)
                    resultJSON = member.ToJson();
                else
                    resultJSON = "{\"success\":false,\"msg\":\"Invalid Data\"}";

                context.Response.Write(resultJSON);
            }
        }

        //public static string GetFormByModule2(EM_MemberDetail member)
        //{
        //    //basic table data
        //    var extTable = new ExtJsTable
        //    {
        //        post_url = "/Table/ListData", //Url.Action("ListData", "Common"),
        //        post_header = "/Table/GridHeader", //Url.Action("GridHeader", "Common"),
        //        title = "Member Detail",
        //        icon = "iconRole16",
        //        post_params = "/Member/Update", // Url.Action("Update"), //Update action
        //        isType = true,
        //        button_text = "Save",
        //        button_icon = "iconSave",
        //        value_changes = true,
        //    };
            
        //    //add row into the table
        //    var rowFieldLabelInt = new ExtJsFieldLabelInput<int>(PayloadKeys.Id, member.member_id.ToString())
        //    {
        //        fieldLabel = "Member ID",
        //        group = "General",
        //        readOnly = true
        //    };
        //    extTable.AddFieldLabelToRow(rowFieldLabelInt);


        //    if (member.member_id > 0)
        //    {
        //        var rowFieldLabelString_no = new ExtJsFieldLabelInput<string>(PayloadKeys.Member_no, member.member_no)
        //        {
        //            fieldLabel = "Member No",
        //            readOnly = true
        //        };
        //        extTable.AddFieldLabelToRow(rowFieldLabelString_no);
        //    }
        //    else
        //    {
        //        var rowFieldLabelString_no = new ExtJsFieldLabelInput<string>(PayloadKeys.Member_no, member.member_no)
        //        {
        //            fieldLabel = "Member No",
        //        };
        //        extTable.AddFieldLabelToRow(rowFieldLabelString_no);
        //    }

        //    var rowFieldLabelString = new ExtJsFieldLabelInput<string>(PayloadKeys.Email, member.email)
        //    {
        //        fieldLabel = "Email",

        //    };
        //    extTable.AddFieldLabelToRow(rowFieldLabelString);

        //    rowFieldLabelString = new ExtJsFieldLabelInput<string>(PayloadKeys.HKID, member.hkid)
        //    {
        //        fieldLabel = "HKID",

        //    };
        //    extTable.AddFieldLabelToRow(rowFieldLabelString);


        //    var birth_year = member.birth_year ?? default(int);
        //    var birth_month = member.birth_month ?? default(int);
        //    var birth_day = member.birth_day ?? default(int);

        //    if (birth_year == 0)
        //    {
        //        birth_year = 1950;
        //        birth_month = 1;
        //        birth_day = 1;
        //    }

        //    DateTime birthday = new DateTime(birth_year, birth_month, birth_day);
        //    var rowFieldLabelDate = new ExtJsFieldLabelDate<DateTime>(PayloadKeys.Birthday, birthday)
        //    {
        //        fieldLabel = "Birthday",

        //    };
        //    extTable.AddFieldLabelToRow(rowFieldLabelDate);

        //    var rowFieldSelect = new ExtJsFieldLabelSelect<int>(PayloadKeys.Gender, member.gender.ToString())
        //    {
        //        fieldLabel = "Gender",
        //        datasource = "/Table/GetListItems/Gender", //Url.Action("GetListItems/Gender", "Table"),
        //        display_value = member.gender.ToString()
        //    };
        //    extTable.AddFieldLabelToRow(rowFieldSelect);

        //    rowFieldSelect = new ExtJsFieldLabelSelect<int>(PayloadKeys.Salutation, member.salutation.ToString())
        //    {
        //        fieldLabel = "Salutation",
        //        datasource = "/Table/GetListItems/Salutation", //Url.Action("GetListItems/Salutation", "Table"),
        //        display_value = member.salutation.ToString()  //Extend.ToItemName("Salutation", member.salutation)
        //    };
        //    extTable.AddFieldLabelToRow(rowFieldSelect);

        //    rowFieldLabelString = new ExtJsFieldLabelInput<string>(PayloadKeys.Mobile_no, member.mobile_no)
        //    {
        //        fieldLabel = "Mobile No",

        //    };
        //    extTable.AddFieldLabelToRow(rowFieldLabelString);

        //    rowFieldSelect = new ExtJsFieldLabelSelect<int>(PayloadKeys.District, member.district.ToString())
        //    {
        //        fieldLabel = "District",
        //        datasource = "/Table/GetListItems/District", //Url.Action("GetListItems/District", "Table"),
        //        display_value = member.district.ToString()
        //    };
        //    extTable.AddFieldLabelToRow(rowFieldSelect);

        //    rowFieldSelect = new ExtJsFieldLabelSelect<int>(PayloadKeys.Reg_source, member.reg_source.ToString())
        //    {
        //        fieldLabel = "reg_source",
        //        datasource = "/Table/GetListItems/RegSource", //Url.Action("GetListItems/RegSource", "Table"),
        //        display_value = member.reg_source.ToString()
        //    };
        //    extTable.AddFieldLabelToRow(rowFieldSelect);


        //    rowFieldSelect = new ExtJsFieldLabelSelect<int>(PayloadKeys.Reg_status, member.reg_status.ToString())
        //    {
        //        fieldLabel = "reg_status",
        //        datasource = "/Table/GetListItems/Status", //Url.Action("GetListItems/Status", "Table"),
        //        display_value = member.reg_status.ToString()
        //    };
        //    extTable.AddFieldLabelToRow(rowFieldSelect);



        //    rowFieldLabelString = new ExtJsFieldLabelInput<string>(PayloadKeys.Activate_key, member.activate_key)
        //    {
        //        fieldLabel = "activate_key",

        //    };
        //    extTable.AddFieldLabelToRow(rowFieldLabelString);

        //    rowFieldLabelString = new ExtJsFieldLabelInput<string>(PayloadKeys.Reg_ip, member.reg_ip)
        //    {
        //        fieldLabel = "reg_ip",

        //    };
        //    extTable.AddFieldLabelToRow(rowFieldLabelString);

        //    rowFieldLabelInt = new ExtJsFieldLabelInput<int>(PayloadKeys.Referrer, member.referrer.ToString())
        //    {
        //        fieldLabel = "Referrer",

        //    };
        //    extTable.AddFieldLabelToRow(rowFieldLabelInt);

        //    rowFieldSelect = new ExtJsFieldLabelSelect<int>(PayloadKeys.Status, member.status.ToString())
        //    {
        //        fieldLabel = "Status",
        //        datasource = "/Table/GetListItems/Status", //Url.Action("GetListItems/Status", "Table"),
        //        display_value = member.status.ToString()
        //    };
        //    extTable.AddFieldLabelToRow(rowFieldSelect);

        //    rowFieldSelect = new ExtJsFieldLabelSelect<int>(PayloadKeys.Opt_in, member.opt_in.ToString())
        //    {
        //        fieldLabel = "opt_in",
        //        datasource = "/Table/GetListItems/YesNo", //Url.Action("GetListItems/YesNo", "Table"),
        //        display_value = member.opt_in.ToString()
        //    };
        //    extTable.AddFieldLabelToRow(rowFieldSelect);

        //    rowFieldSelect = new ExtJsFieldLabelSelect<int>(PayloadKeys.Member_level_id, member.member_level_id.ToString())
        //    {
        //        fieldLabel = "Member_level",
        //        datasource = "/Table/GetListItems/MemberLevel", //Url.Action("GetListItems/MemberLevel", "Table"),
        //        display_value = member.member_level_name
        //    };
        //    extTable.AddFieldLabelToRow(rowFieldSelect);


        //    // English
        //    rowFieldLabelString = new ExtJsFieldLabelInput<string>(PayloadKeys.Name, member.name)
        //    {
        //        fieldLabel = "Name",
        //        group = "English"
        //    };
        //    extTable.AddFieldLabelToRow(rowFieldLabelString);

        //    rowFieldLabelString = new ExtJsFieldLabelInput<string>(PayloadKeys.Surname, member.surname)
        //    {
        //        fieldLabel = "Surname",

        //    };
        //    extTable.AddFieldLabelToRow(rowFieldLabelString);

        //    rowFieldLabelString = new ExtJsFieldLabelInput<string>(PayloadKeys.Givenname, member.givenname)
        //    {
        //        fieldLabel = "Givenname",

        //    };
        //    extTable.AddFieldLabelToRow(rowFieldLabelString);

        //    rowFieldLabelString = new ExtJsFieldLabelInput<string>(PayloadKeys.Address1, member.address1)
        //    {
        //        fieldLabel = "Address1",

        //    };
        //    extTable.AddFieldLabelToRow(rowFieldLabelString);

        //    rowFieldLabelString = new ExtJsFieldLabelInput<string>(PayloadKeys.Address2, member.address2)
        //    {
        //        fieldLabel = "Address2",

        //    };
        //    extTable.AddFieldLabelToRow(rowFieldLabelString);

        //    rowFieldLabelString = new ExtJsFieldLabelInput<string>(PayloadKeys.Address3, member.address3)
        //    {
        //        fieldLabel = "Address3",

        //    };
        //    extTable.AddFieldLabelToRow(rowFieldLabelString);

        //    rowFieldLabelString = new ExtJsFieldLabelInput<string>(PayloadKeys.Address4, member.address4)
        //    {
        //        fieldLabel = "Address4",

        //    };
        //    extTable.AddFieldLabelToRow(rowFieldLabelString);
            
        //    //hidden table data
        //    var hiddenLabel = new ExtJsFieldLabelHidden<int>(PayloadKeys.Id, member.member_id.ToString());
        //    extTable.AddFieldLabelToHiddenRow(hiddenLabel);

        //    return extTable.ToJson();
        //}

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}