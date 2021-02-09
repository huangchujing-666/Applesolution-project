using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Palmary.Loyalty.BO.Database;
using Palmary.Loyalty.BO.DataTransferObjects.Security;
using Palmary.Loyalty.BO.DataTransferObjects.Wifi;
using Palmary.Loyalty.BO.Modules.Wifi;

using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.ExtJsEntity;
using Palmary.Loyalty.Web_backend.Modules.GlobalHandlers;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.PayoadKey;
using Palmary.Loyalty.Web_backend.Modules.Utility;
using Palmary.Loyalty.Web_backend.Modules.ExtJS;
using Palmary.Loyalty.Web_backend;
using Palmary.Loyalty.Common;
using Palmary.Loyalty.Common.Languages;
using System.Web.Routing;
using Palmary.Loyalty.Web_backend.Modules.Wifi;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.TableData;


namespace Palmary.Loyalty.Web_backend.Controllers.Wifi
{
    [Authorize]
    public class WifiLocationController : Controller
    {
        private int _id;
        private WifiLocationManager _wifiLocationManager;

        public WifiLocationController()
        {
            _wifiLocationManager = new WifiLocationManager();
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

        // Create new
        public string Insert()
        {
            var wifiLocationHandler = new WifiLocationHandler();
            var formTableJSON = wifiLocationHandler.GetFormByModule(new WifiLocationObject());
            return formTableJSON;
        }

        public string Update(FormCollection collection)
        {
            var location_id = collection.GetFormValue(PayloadKeys.WifiLocation.location_id);
            var location_no = collection.GetFormValue(PayloadKeys.WifiLocation.location_no);
            var name = collection.GetFormValue(PayloadKeys.WifiLocation.name);
            var mac_address = collection.GetFormValue(PayloadKeys.WifiLocation.mac_address);
            var point = collection.GetFormValue(PayloadKeys.WifiLocation.point);
            var status = collection.GetFormValue(PayloadKeys.WifiLocation.status);
           
            var wifiLocationObject = new WifiLocationObject()
            {
                location_id = location_id,
                location_no = location_no,
                name = name,
                mac_address = mac_address,
                point = point,
                status = status
            };

            // Category
            var mList = collection.GetFormValue(PayloadKeys.WifiLocation.member_level).Split(',');
            var member_level_list = new List<WifiLocationPrivilegeObject>();

            foreach (var ml in mList)
            {
                var obj = new WifiLocationPrivilegeObject
                {
                    member_level_id = int.Parse(ml),
                };

                member_level_list.Add(obj);
            }

            wifiLocationObject.privilege_list = member_level_list;

            var sql_remark = "";
            if (location_id == 0)
            {
                var systemCode = _wifiLocationManager.Create(wifiLocationObject);

                return systemCode == CommonConstant.SystemCode.normal ? "{success:true,url:'',msg:'Add Success'}" : "{success:true,url:'',msg:'Add Failed: " + sql_remark + "'}";
            }
            else if (location_id > 0)
            {
                var systemCode = _wifiLocationManager.Update(
                    wifiLocationObject);

                return systemCode == CommonConstant.SystemCode.normal ? "{success:true,url:'',msg:'Saved Success'}" : "{success:true,url:'',msg:'Saved Failed: " + sql_remark + "'}";
            }
            else
                return "{success:true,url:'',msg:'Saved Failed: Invalid Data'}";
        }

        // Edit View Form
        public string EditView()
        {
            var systemCode = CommonConstant.SystemCode.undefine;
            var wifiLocationObj = _wifiLocationManager.GetDetail(_id, false, ref systemCode);

            var wifiLocationHandler = new WifiLocationHandler();
            var formTableJSON = wifiLocationHandler.GetFormByModule(wifiLocationObj);
            return formTableJSON;
        }

        public string Test(FormCollection collection)
        {
            var select_value = collection["select_value"];
            var updateText = "000";

            if (select_value == "1")
                updateText = "1111";
            else if (select_value == "2")
                updateText = "2222";

            return new { success = true, updateText = updateText }.ToJson();
        }

        public string TestAjaxThree(FormCollection collection)
        {
            var select_value = collection["select_value"];
            var updateText1 = "";
            var updateText2 = "";
            var updateText3 = "";

            if (select_value == "1")
            {
                updateText1 = "1111";
                updateText2 = "2222";
                updateText3 = "3333";
            }
            else if (select_value == "2")
            {
                updateText1 = "aaaa";
                updateText2 = "bbbb";
                updateText3 = "cccc";
            }

            return new { success = true, updateText1 = updateText1, updateText2 = updateText2, updateText3 = updateText3 }.ToJson();
        }

        public string TestAjaxFour(FormCollection collection)
        {
            var select_value = collection["select_value"];
            var updateText1 = "";
            var updateText2 = "";
            var updateText3 = "";
            var updateText4 = "";

            if (select_value == "1")
            {
                updateText1 = "1111";
                updateText2 = "2222";
                updateText3 = "3333";
                updateText4 = "4444";
            }
            else if (select_value == "2")
            {
                updateText1 = "aaaa";
                updateText2 = "bbbb";
                updateText3 = "cccc";
                updateText4 = "dddd";
            }

            return new { success = true, updateText1 = updateText1, updateText2 = updateText2, updateText3 = updateText3, updateText4 = updateText4 }.ToJson();
        }

        public string EditView_ToolbarData()
        {
            var toolData = new List<ExtJsButton>();

            toolData.Add(new ExtJsButton("button", "view") { text = "View", hidden = true, iconUrl = "iconView" });
            toolData.Add(new ExtJsButton("button", "edit") { text = "Edit", iconUrl = "iconRole16" });

            var result = new { toolData = toolData }.ToJson();
            // remove double quotation for herf:function
            result = result.Replace(@"""href"":""f", @"""href"":f");
            result = result.Replace(@"""}]}", @"}]}");

            return result;
        }

        public string ChartConfig()
        {
            var config = new
            {
                success = true,
                fields = new List<ChartField>() { 
                    new ChartField(){name = "month", type= "string"}, 
                    new ChartField(){name = "AP01", type= "int"},
                    new ChartField(){name = "AP02", type= "int"},
                    new ChartField(){name = "AP03", type= "int"},
                    new ChartField(){name = "AP04", type= "int"} 
                },
                url = "../WifiLocation/ChartData", //"../UIContent/modules/portal/portlet/salesPortlet_data.js",
                series = new List<ChartSeries>() { 
                    new ChartSeries(){dataIndex = "AP01", name= "AP01"}, 
                    new ChartSeries(){dataIndex = "AP02", name= "AP02"},
                    new ChartSeries(){dataIndex = "AP03", name= "AP03"},
                    new ChartSeries(){dataIndex = "AP04", name= "AP04"}
                },
                xField = "month",
                chartConfig = new
                { 
                    chart = new {
                    },
                    title = new
                    {
                        text = "Location Presence Statistic"
                    },
                    xAxis = new
                    {
                        title = new
                        {
                            text = ""
                        }
                    },
                    yAxis = new
                    {
                        min = 0,
                        title = new
                        {
                            text = "No of people",
                            align = "high"
                        },
                        labels = new
                        {
                            overflow = "justify"
                        },
                        plotLines = new List<ChartPlotLines> { 
                            new ChartPlotLines(){
                                value = 0,
                                width = 1,
                                color = "#808080"
                            }
                        }
                    },
                    tooltip = new {
                        valueSuffix = ""
                    },
                    plotOptions = new {
                        line = new {
                            connectNulls = false
                        }
                    },
                    credits = new {
                        text = "TrueLoyalty",
                        href = "#"
                    }
                }
            };

            return config.ToJson();
        }

        public string ChartData()
        {
            var data = new
            {
                totalCount = 6,
                items = new List<ChartData>()
                {
                    new ChartData { month = "Jan 13", AP01 = 37190, AP02 = 25980, AP03 = 40560, AP04 = 12000},
                    new ChartData { month = "Feb", AP01 = 29182, AP02 = 16490, AP03 = 23004, AP04 = 13400},
                    new ChartData { month = "Mar", AP01 = 59280, AP02 = 47200, AP03 = 76103, AP04 = 10900},
                    new ChartData { month = "Apr", AP01 = 10290, AP02 = 67500, AP03 = 45020, AP04 = 50029},
                    new ChartData { month = "May", AP01 = 12, AP02 = 60, AP03 = 90, AP04 = 200},
                    new ChartData { month = "Jun", AP01 = 20090, AP02 = 50, AP03 = 20, AP04 = 20500},
                    new ChartData { month = "Jul", AP01 = 10900, AP02 = 50, AP03 = 20, AP04 = 20500},
                    new ChartData { month = "Aug", AP01 = 50990, AP02 = 50, AP03 = 20, AP04 = 20500},
                    new ChartData { month = "Sep", AP01 = 20920, AP02 = 50, AP03 = 20, AP04 = 20500},
                    new ChartData { month = "Oct", AP01 = 30920, AP02 = 10050, AP03 = 20200, AP04 = 20500},
                    new ChartData { month = "Nov", AP01 = 50920, AP02 = 50, AP03 = 20, AP04 = 50500},
                    new ChartData { month = "Dec", AP01 = 120920, AP02 = 12500, AP03 = 17000, AP04 = 20500},

                    new ChartData { month = "Jan 14", AP01 = 37190, AP02 = 25980, AP03 = 40560, AP04 = 12000},
                    new ChartData { month = "Feb", AP01 = 29182, AP02 = 16490, AP03 = 23004, AP04 = 13400},
                    new ChartData { month = "Mar", AP01 = 59280, AP02 = 47200, AP03 = 76103, AP04 = 10900},
                    new ChartData { month = "Apr", AP01 = 10290, AP02 = 67500, AP03 = 55020, AP04 = 50029},
                    new ChartData { month = "May", AP01 = 12, AP02 = 60, AP03 = 90, AP04 = 200},
                    new ChartData { month = "Jun", AP01 = 20090, AP02 = 50, AP03 = 20, AP04 = 20500},
                    new ChartData { month = "Jul", AP01 = 10900, AP02 = 50, AP03 = 20, AP04 = 20500},
                    new ChartData { month = "Aug", AP01 = 50990, AP02 = 21500, AP03 = 20, AP04 = 20500},
                    new ChartData { month = "Sep", AP01 = 80920, AP02 = 50, AP03 = 20, AP04 = 20500}
                }
            };

            return data.ToJson();
        }

        public string PromotePopupForm()
        {
            var promoteID = _id; //should be redemption_id
            var sql_result = false;
            var resultCode = CommonConstant.SystemCode.undefine;

            FileHandler _fileHandler = new FileHandler();
            
            // Fields
            List<ExtJSField> fieldList = new List<ExtJSField>();
            fieldList.Add(new ExtJSField
            {
                name = "location_no",
                fieldLabel = "Location No",
                type = "input",
                colspan = 2,
                tabIndex = "1",
                value = "AP01",
                display_value = "",
                readOnly = true
            });

            fieldList.Add(new ExtJSField
            {
                name = "mac_address",
                fieldLabel = "Mac Address",
                type = "input",
                colspan = 2,
                tabIndex = "1",
                value = "00:18:0a:26:91:c2",
                display_value = "",
                readOnly = true
            });

            fieldList.Add(new ExtJSField
            {
                name = "promoteid",
                fieldLabel = "Promote ID",
                type = "input",
                colspan = 2,
                tabIndex = "1",
                value = promoteID.ToString(),
                display_value = "",
                readOnly = true
            });

            var level_value = "";
            if (promoteID == 1)
                level_value = "Normal, Silver, Gold, Platinum";
            else
                level_value = "Platinum";

            fieldList.Add(new ExtJSField
            {
                name = "member_level",
                fieldLabel = "Member Level",
                type = "input",
                colspan = 2,
                tabIndex = "1",
                value = level_value,
                display_value = "",
                readOnly = true
            });

            var cat_value = "";
            if (promoteID == 1)
                cat_value = "Normal";
            else
                cat_value = "Normal";

            fieldList.Add(new ExtJSField
            {
                name = "member_category",
                fieldLabel = "Member Category",
                type = "input",
                colspan = 2,
                tabIndex = "1",
                value = cat_value,
                display_value = "",
                readOnly = true
            });

            fieldList.Add(new ExtJSField
            {
                name = "image",
                fieldLabel = "Pop Up Image",
                type = "input",
                colspan = 2,
                tabIndex = "1",
                value = "<img src='" + _fileHandler.GetImagePath("Coupon_0"+promoteID.ToString(), ".jpg", (string)CommonConstant.Module.promote) + "' width='20%' height='20%'>",
                display_value = "",
                readOnly = true
            });

            var pt_value = "";
            if (promoteID == 1)
                pt_value = "0";
            else
                pt_value = "1";

            fieldList.Add(new ExtJSField
            {
                name = "point",
                fieldLabel = "Earn Point",
                type = "input",
                colspan = 2,
                tabIndex = "1",
                value = pt_value,
                display_value = "",
                readOnly = true
            });

            var gift_value = "";
            if (promoteID == 1)
                gift_value = "CH01";
            else
                gift_value = "BW01";

            fieldList.Add(new ExtJSField
            {
                name = "gift",
                fieldLabel = "Earn Gift",
                type = "input",
                colspan = 2,
                tabIndex = "1",
                value = gift_value,
                display_value = "",
                readOnly = true
            });

            //fieldList.Add(new ExtJSField
            //{
            //    name = "redemption_code",
            //    fieldLabel = "Redemption Code",
            //    type = "input",
            //    colspan = 2,
            //    tabIndex = "1",
            //    value = redeem_record.redemption_code,
            //    display_value = redeem_record.redemption_code,
            //    readOnly = true
            //});

            //fieldList.Add(new ExtJSField
            //{
            //    name = "member_code",
            //    fieldLabel = "Member Code",
            //    type = "date",
            //    colspan = 2,
            //    tabIndex = "2",
            //    value = member.member_no,
            //    readOnly = true
            //});

            //fieldList.Add(new ExtJSField
            //{
            //    name = "member_name",
            //    fieldLabel = "Member Name",
            //    type = "date",
            //    colspan = 2,
            //    tabIndex = "2",
            //    value = member.GetFullname(),
            //    readOnly = true
            //});

            //fieldList.Add(new ExtJSField
            //{
            //    name = "gift_name",
            //    fieldLabel = "Gift Name",
            //    type = "date",
            //    colspan = 2,
            //    tabIndex = "2",
            //    value = gift_name,
            //    readOnly = true
            //});

            //fieldList.Add(new ExtJSField
            //{
            //    name = "quantity",
            //    fieldLabel = "Quantity",
            //    type = "date",
            //    colspan = 2,
            //    tabIndex = "2",
            //    value = redeem_record.quantity.ToString(),
            //    readOnly = true
            //});

            //fieldList.Add(new ExtJSField
            //{
            //    name = "void_reason",
            //    fieldLabel = "Void Reason",
            //    type = "input",
            //    colspan = 2,
            //    tabIndex = "2",
            //    value = ""

            //});

            //// Hidden Fields
            //List<ExtJSField_hidden> hiddenList = new List<ExtJSField_hidden>();
            //hiddenList.Add(new ExtJSField_hidden
            //{
            //    type = "hidden",
            //    name = "redemption_id",
            //    value = redemption_id.ToString()
            //});
            //hiddenList.Add(new ExtJSField_hidden
            //{
            //    type = "hidden",
            //    name = "status",
            //    value = ((int)CommonConstant.GiftRedeemStatus.voided).ToString()
            //});

            var formTableJSON = new
            {
                row = fieldList,
                rowhidden = "",

                column = 2,
                post_url = "",  //<-
                post_header = "", //<-
                title = "Detail Form",
                icon = "iconRemarkList",
                post_params = "",//Url.Action("CollectOrCancel_perform"),

                button_text = "OK",
                button_icon = "iconSave",
                value_changes = true
            }.ToJson();

            return formTableJSON;
        }
    }

    public class ChartField
    {
        public string name;
        public string type;
    }

    public class ChartSeries
    {
        public string dataIndex;
        public string name;
    }
    public class ChartPlotLines
    {
        public int value;
        public int width;
        public string color;
    }
    public class ChartData
    {
        public string month;
        public int AP01;
        public int AP02;
        public int AP03;
        public int AP04;
    }
}