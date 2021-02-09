using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.ExtJsEntity;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.PayoadKey;
using Palmary.Loyalty.Web_backend.Modules.Utility;
using Palmary.Loyalty.Web_backend;
using System.Web.Routing;
using Palmary.Loyalty.Web_backend.Modules.GlobalHandlers;

namespace Palmary.Loyalty.Web_backend.Controllers
{
    [Authorize]
    public partial class MemberLevelController : Controller
    {
        //
        // GET: /Memberlevel/

        private int _userId = 0;
        private int _memberLevelId = 0;

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            SystemHandler.InitSystemParameters(this.HttpContext.ApplicationInstance as MvcApplication);
        }

        //private readonly MemberLevelService _memberLevelService;

        //public MemberLevelController()
        //{
        //    _languageService = new LanguageService();
        //    _memberLevelService = new MemberLevelService();
        //}

        //public string GetModule()
        //{
        //    var languages = _languageService.GetAllLanguage();
        //    var extTable = new ExtJsTable
        //    {
        //        post_url = Url.Action("ListData", "Common"),
        //        post_header = Url.Action("GridHeader", "Common"),
        //        title = "Member Level Detail",
        //        icon = "iconRole16",
        //        post_params = Url.Action("Update"), //将要返回来修改的action
        //        isType = true,
        //        button_text = "Save",
        //        button_icon = "iconSave",
        //        value_changes = true,
        //    };
        //    var memberLevel = _memberLevelService.GetMemberLevel(_userId, _memberLevelId, 0);
        //    GetGeneralExtFiles(memberLevel, ref extTable);

        //    foreach (var language in languages)
        //    {
        //        memberLevel = _memberLevelService.GetMemberLevel(_userId, _memberLevelId, language.lang_id);
        //        GetExtFilesByLang(memberLevel, ref extTable, language.name);
        //    }
        //    _memberLevelId = 1;
        //    var row1FieldLabel3 = new ExtJsFieldLabelHidden<int>(PayloadKeys.Id, _memberLevelId.ToString());
        //    extTable.AddFieldLabelToHiddenRow(row1FieldLabel3);
        //    return extTable.ToJson();
        //}

        //private void GetGeneralExtFiles(EMemberLevel memberLevel, ref ExtJsTable extTable)
        //{
        //    var rowFieldLabelString = new ExtJsFieldLabelInput<string>(PayloadKeys.Name, memberLevel.name)
        //    {
        //        fieldLabel = "Name",
        //        group = "General",
        //        readOnly = true
        //    };
        //    extTable.AddFieldLabelToRow(rowFieldLabelString);

        //    var rowFieldLabelCurrency = new ExtJsFieldLabelNumber(PayloadKeys.PointRequired, memberLevel.point_required.ToString())
        //    {
        //        fieldLabel = "Point Required",
        //        allowBlank = false
        //    };
        //    extTable.AddFieldLabelToRow(rowFieldLabelCurrency);
        //    rowFieldLabelCurrency = new ExtJsFieldLabelNumber(PayloadKeys.RedeemDiscount, memberLevel.redeem_discount.ToString())
        //    {
        //        fieldLabel = "Redeem Discount",
        //        allowBlank = false
        //    };
        //    extTable.AddFieldLabelToRow(rowFieldLabelCurrency);
        //    var rowFieldLabelCurrencyInt = new ExtJsFieldLabelInt(PayloadKeys.DisplayOrder, memberLevel.display_order.ToString())
        //    {
        //        fieldLabel = "Display Order",
        //        allowBlank = false
        //    };
        //    extTable.AddFieldLabelToRow(rowFieldLabelCurrencyInt);
        //    var rowFieldSelect = new ExtJsFieldLabelSelect<int>(PayloadKeys.Status, memberLevel.status.ToString())
        //    {
        //        fieldLabel = "Status",
        //        datasource = Url.Action("GetStatus", "Common"),
        //        display_value = memberLevel.status.ToItemName("Status")
        //    };
        //    extTable.AddFieldLabelToRow(rowFieldSelect);
        //}

        //private static void GetExtFilesByLang(EMemberLevel memberLevel, ref ExtJsTable extTable, string groupName)
        //{
        //    var row1FieldLabel1 = new ExtJsFieldLabelInput<string>(PayloadKeys.Name.Add(groupName), memberLevel.name)
        //    {
        //        fieldLabel = "Name",
        //        group = groupName,
        //        allowBlank = false,
        //    };

        //    extTable.AddFieldLabelToRow(row1FieldLabel1);
        //}

        //public string ToolbarData()
        //{
        //    var toolData = new List<ExtJsButton>();
        //    toolData.Add(new ExtJsButton("button", "view") { text = "View", hidden = true, iconUrl = "iconView" });
        //    toolData.Add(new ExtJsButton("button", "edit") { text = "Edit", iconUrl = "iconRole16" });
        //    return new { toolData = toolData }.ToJson();
        //}

        //public string Update(FormCollection collection)
        //{
        //    var languages = _languageService.GetAllLanguage();
        //    var levelId = collection.GetFormValue(PayloadKeys.Id);
        //    System.Diagnostics.Debug.WriteLine("PayloadKeys.Id: " + PayloadKeys.Id.ToString());
        //    System.Diagnostics.Debug.WriteLine("collection.GetFormValue(PayloadKeys.Id): " + levelId);
        //    var addFlag = levelId == 0;
        //    var updateFlag = true;
        //    var defaultLanguage = languages.First(x => x.lang_id == 2);
        //    var name = collection.GetFormValue(PayloadKeys.Name.Add(defaultLanguage.name));
        //    var pointRequired = collection.GetFormValue(PayloadKeys.PointRequired);
        //    var displayOrder = collection.GetFormValue(PayloadKeys.DisplayOrder);
        //    var status = collection.GetFormValue(PayloadKeys.Status);
        //    if (addFlag)
        //        levelId = _memberLevelService.Add(_userId, name);
        //    else
        //        updateFlag &= _memberLevelService.UpdateMemberLevel(_userId, levelId, name, pointRequired, displayOrder, status);

        //    foreach (var language in languages)
        //    {
        //        name = collection.GetFormValue(PayloadKeys.Name.Add(language.name));
        //        _memberLevelService.UpdateMemberLevelLang(language.lang_id, _userId, levelId, name);
        //    }

        //    if (addFlag)
        //    {
        //        return
        //            string.Format(
        //                "{{success:true, id:'MemberLevel:{0}',title:'MemberLevel:{1}', url:'com.palmary.memberLevel.js.edit', icon:'iconRole16',msg:'Add Success' }} ",
        //                levelId, name); //傳值 id,及 標籤title
        //    }
        //    else
        //    {
        //        return updateFlag ? "{success:true,url:'',msg:'Saved Success'}" : "{success:false,url:'',msg:'Saved Failed'}";
        //    }
        //}

    }
}
