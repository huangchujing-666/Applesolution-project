/*
 * Last modified by lokshu 2012-07-17
 * Varsion 3.1
 * Fix the layout issue for Extjs 4.1
 */
Ext.Loader.setConfig({
    enabled: true
});
Ext.Loader.setPath('Ext.ux', '../UIContent/ux');

Ext.require(['Ext.grid.*', 'Ext.data.*', 'Ext.tip.QuickTipManager']);
var search_window;
var formFieldFocusIndex = 0;
Ext.define('Ext.form.SystemLink', {
    extend: 'Ext.form.field.Display',
    alias: 'widget.syslink',
    fieldSubTpl: ['<div id="{id}" class="{fieldCls}" style="color: #09F;cursor: pointer;">{value}</div>',
    {
        compiled: true,
        disableFormats: true
    }],
    listeners: {
        afterrender: function (dv, record) {
            var textField = this.inputEl;
            var me = this;
            textField.on("click", function (event, htmlElement, me) {
                Ext.decode(this.href);

            }, this);
        }
    }
});
Ext.define('Ext.form.dob', {
    extend: 'Ext.form.field.Display',
    alias: 'widget.dob',
    setValue: function (value) {
        var me = this;
        if (this.format == null) {
            this.format = 'Y-m-d';
        }
        var dt = Ext.Date.parse(value, this.format);
        var dt_new = Ext.Date.getElapsed(dt);
        var year_diff = dt_new / 1000 / 60 / 60 / 24 / 365;
        var month_diff = Math.floor((year_diff - Math.floor(year_diff)) * 12)
        me.rawValue = value + " " + Math.floor(year_diff) + " yrs, " + month_diff + " months";
        if (me.inputEl) {
            me.inputEl.dom.value = value;
        }
        return value;
    }
});
function listener_ComboboxBeforeQuery(e) {
    var combo = e.combo;
    try {
        var value = e.query;
        //combo.lastQuery = value;//把lastquery定位到自定义search inputbox
        combo.store.filterBy(function (record, id) {
            var text = record.get(combo.displayField);
            return (text.toLowerCase().indexOf(value.toLowerCase()) != -1); // ignore upper / lower case.
        });
        combo.onLoad();
        combo.expand();
        return false;
    } catch (e) {
        combo.selectedIndex = -1;
        combo.store.clearFilter();
        combo.onLoad();
        combo.expand();
        return false;
    }
}
//Ext.onReady(function() {
Ext.define('com.embraiz.component.form', {

    /*myMask : new Ext.LoadMask(Ext.getBody(), {
		    useMsg:false
		   }), */
    viewSimple: undefined,
    editSimple: undefined,
    store: undefined,
    multstore: undefined,
    win: undefined,
    grid: undefined,
    target_div: undefined,
    regexText: "Please input correct format",
    emailRegex: /^\s*\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*(\;\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)*(\;)*\s*$/,
    numberRegex: /^\d*$/,
    //doubleRegex:/^[1-9]\d+(\.\d+)?$/,
    doubleRegex: /^(-?\d+)(\.\d+)?$/,
    intRegex: /^[1-9]\d*$/,
    json_data: undefined,
    json_data_url: undefined,
    pre_tab: undefined,
    index_url: undefined,
    index_id: undefined,
    index_itemId: undefined,
    tabUrl: undefined,
    itemId: undefined,
    itemId1: undefined,
    toolbar: undefined,
    appGrid: undefined,
    approvalProcess: undefined,
    url: undefined,
    parentType: undefined,
    parentId: undefined,
    id: undefined,
    noToolBar: false,
    viewForm: function (target_div, json_data_url, pre_tab, index_url, index_id, index_itemId, params) {


        this.target_div = target_div;
        this.pre_tab = pre_tab;
        this.index_id = index_id;
        this.index_itemId = index_itemId;
        this.index_url = index_url;
        this.json_data_url = json_data_url;
        if (this.editSimple != undefined) {
            this.editSimple.hide();
        }
        if (this.viewSimple != undefined) {
            this.viewSimple.destroy();
        }
        Ext.Ajax.request({
            url: json_data_url,
            async: false,
            params: params,
            success: this.showViewData,
            scope: this
        });

    },
    showViewData: function (o) {
        var json_data = this.json_data = Ext.decode(o.responseText);
		if (json_data.noToolBar == true) {
            this.noToolBar = true;
        }
        //target_div.dom.style.margin="5px";
        var target_div = this.target_div;
        var form_content = [];
        var form_content_left = [];
        var form_content_right = [];

        var form_group = [];
        var form_items = undefined;
        var groupIndex = 0;
        var groupItem = undefined;
        function check(value) {
            if (value == undefined) {
                value = "";
            }
            return value;
        }
        for (i = 0; i < json_data.row.length; i++) {
            var temp_element = {};
            if (json_data.row[i].width) {
                temp_element.width = json_data.row[i].width;
            }
            if (json_data.row[i].height) {
                temp_element.height = json_data.row[i].height;
            }
            if (json_data.row[i].type == "currencyFormat") {
                temp_element.fieldLabel = json_data.row[i].fieldLabel;
                temp_element.xtype = "displayfield";
                temp_element.name = json_data.row[i].name;
                var sign = (json_data.row[i].sign == undefined ? "$" : json_data.row[i].sign);
                if (isNaN(Number(json_data.row[i].value))) {
                    var values = json_data.row[i].value.replace(sign, '');
                    var valuesList = values.split(',');
                    var valueInt = "";
                    for (i = 0; i < valuesList.length; i++) {
                        valueInt += valuesList[i];
                    }
                    temp_element.value = Ext.util.Format.currency(valueInt, sign);
                } else {
                    temp_element.value = Ext.util.Format.currency(json_data.row[i].value, sign);
                }
            } else if (json_data.row[i].type == "product") {
                temp_element.xtype = json_data.row[i].type;
                temp_element.field1Name = json_data.row[i].field1Name;
                temp_element.field2Name = json_data.row[i].field2Name;
                temp_element.field3Name = json_data.row[i].field3Name;
                temp_element.field4Name = json_data.row[i].field4Name;
                temp_element.field1Value = json_data.row[i].field1Value;
                temp_element.field2Value = json_data.row[i].field2Value;
                temp_element.field3Value = json_data.row[i].field3Value;
                temp_element.field4Value = json_data.row[i].field4Value;
                temp_element.datasource1 = json_data.row[i].datasource1;
                temp_element.datasource2 = json_data.row[i].datasource2;
                temp_element.datasource3 = json_data.row[i].datasource3;
                temp_element.datasource4 = json_data.row[i].datasource4;
                temp_element.tabIndex = json_data.row[i].tabIndex;
                temp_element.name = json_data.row[i].name;
            } else if (json_data.row[i].type == "TRM_REF") {
                temp_element.fieldLabel = json_data.row[i].fieldLabel;
                temp_element.xtype = "displayfield";
                temp_element.name = json_data.row[i].name;
                temp_element.value = check(json_data.row[i].value) + '<span style="font-weight:bold;margin-left:5px;margin-right:5px ">' + json_data.row[i].fieldLabel + ':</span>' + check(json_data.row[i].value1);
            } else if (json_data.row[i].type == "multiUploadDialog") {
                temp_element.xtype = 'viewMultImage';
                temp_element.name = json_data.row[i].name;
                temp_element.images = json_data.row[i].images;
            } else if (json_data.row[i].type == "checkbox_label") {
                var checkboxLabelName = "";
                if (json_data.row[i].checkboxLabel != "") {
                    checkboxLabelName = '<span style="font-weight:bold;margin-left:5px;margin-right:5px ">   ' + json_data.row[i].checkboxLabel + ':   </span>    '
                }
                temp_element.fieldLabel = json_data.row[i].fieldLabel;
                temp_element.xtype = "displayfield";
                temp_element.name = json_data.row[i].name;
                temp_element.value = check(json_data.row[i].value) + checkboxLabelName + check(json_data.row[i].checked);
            } else if (json_data.row[i].type == "firstname_input") {
                temp_element.fieldLabel = json_data.row[i].fieldLabel;
                temp_element.xtype = "displayfield";
                temp_element.name = json_data.row[i].name;
                temp_element.value = check(json_data.row[i].display_value) + '  ' + check(json_data.row[i].input_value);
            } else if (json_data.row[i].type == "dateRange") {
                temp_element.fieldLabel = json_data.row[i].fieldLabel;
                temp_element.xtype = "displayfield";
                temp_element.name = json_data.row[i].name;
                temp_element.value = check(json_data.row[i].value) + '<span style="font-weight:bold;margin-left:5px;margin-right:5px "> TO:</span>  ' + check(json_data.row[i].toValue);
            } else if (json_data.row[i].type == "dateTimeRange") {
                // this part is not in embraiz version
                temp_element.fieldLabel = json_data.row[i].fieldLabel;
                temp_element.xtype = "displayfield";
                temp_element.name = json_data.row[i].name;
                temp_element.value = check(json_data.row[i].value) + '<span style="font-weight:bold;margin-left:5px;margin-right:5px "> TO:</span>  ' + check(json_data.row[i].toValue);
            } else if (json_data.row[i].type == "upload") {
                if (json_data.row[i].value != null || json_data.row[i].value != '') {
                    if (json_data.row[i].upType == 'file') {
                        temp_element.fieldLabel = json_data.row[i].fieldLabel;
                        temp_element.xtype = "displayfield";
                        temp_element.name = json_data.row[i].name;
                        temp_element.value = "<a href='" + json_data.row[i].value + "' target='_blank'>" + json_data.row[i].fileName + "</a>";
                    } else {
                        temp_element.fieldLabel = json_data.row[i].fieldLabel;
                        temp_element.xtype = "displayfield";
                        temp_element.name = json_data.row[i].name;
                        temp_element.value = '<img src="' + check(json_data.row[i].value) + '" width="80" height="80" />';
						
                    }
                }

            } else if (json_data.row[i].type == "label") {
                temp_element.fieldLabel = json_data.row[i].fieldLabel;
                temp_element.xtype = "displayfield";
                temp_element.name = json_data.row[i].name;
                temp_element.value = check(json_data.row[i].value);
            } else if (json_data.row[i].type == "input_check" || json_data.row[i].type == "date_check") {
                var checkboxLabelName = "";
                if (json_data.row[i].checkboxLabel != "") {
                    checkboxLabelName = '<span style="font-weight:bold;margin-left:5px;margin-right:5px ">   ' + json_data.row[i].checkboxLabel + ':   </span>    '
                }
                temp_element.fieldLabel = json_data.row[i].fieldLabel;
                temp_element.xtype = "displayfield";
                temp_element.name = json_data.row[i].name;
                temp_element.value = json_data.row[i].value + checkboxLabelName + json_data.row[i].checked;
            } else if (json_data.row[i].type == "select_select") {
                temp_element.fieldLabel = json_data.row[i].fieldLabel;
                temp_element.xtype = "displayfield";
                temp_element.name = json_data.row[i].name;
                temp_element.value = check(json_data.row[i].display_value) + '<span style="font-weight:bold;margin-left:5px;margin-right:5px ">' + json_data.row[i].selectFieldLabel + ':</span>' + check(json_data.row[i].display_value2);
            } else if (json_data.row[i].type == "select_input") {
                temp_element.fieldLabel = json_data.row[i].fieldLabel;
                temp_element.xtype = "displayfield";
                temp_element.name = json_data.row[i].name;
                temp_element.value = check(json_data.row[i].display_value) + '<span style="font-weight:bold;margin-left:5px;margin-right:5px ">' + check(json_data.row[i].fieldLabel1) + (json_data.row[i].fieldLabel1 == undefined ? "" : ':') + '</span>' + check(json_data.row[i].input_value);
            } else if (json_data.row[i].type == "input_select") {
                // this part else-if is not in embraiz version
                temp_element.fieldLabel = json_data.row[i].fieldLabel;
                temp_element.xtype = "displayfield";
                temp_element.name = json_data.row[i].name;
                temp_element.value = check(json_data.row[i].input_value) + '<span style="font-weight:bold;margin-left:5px;margin-right:5px ">' + check(json_data.row[i].fieldLabel1) + (json_data.row[i].fieldLabel1 == undefined ? "" : ':') + '</span>' + check(json_data.row[i].display_value);
            }
            else if (json_data.row[i].type == "dateTime_threeInput") {
                // this part else-if is not in embraiz version
                temp_element.fieldLabel = json_data.row[i].fieldLabel;
                temp_element.xtype = "displayfield";
                temp_element.name = json_data.row[i].name;
                temp_element.value = json_data.row[i].display_value;
            }
            else if (json_data.row[i].type == "InputNoDuplicate") {
                // this part else-if is not in embraiz version
                temp_element.fieldLabel = json_data.row[i].fieldLabel;
                temp_element.xtype = "displayfield";
                temp_element.name = json_data.row[i].name;
                temp_element.value = json_data.row[i].display_value;
            }
            else if (json_data.row[i].type == "textarea") {
                // this part else-if is not in embraiz version
                temp_element.fieldLabel = json_data.row[i].fieldLabel;
                temp_element.xtype = "displayfield";
                temp_element.name = json_data.row[i].name;

                if (json_data.row[i].display_value==undefined)
                    temp_element.value = json_data.row[i].value.replace(/\n/g, "<br/>");
                else
                    temp_element.value = json_data.row[i].display_value.replace(/\n/g, "<br/>");

                temp_element.anchor = "96%";
                if (json_data.row[i].url == null) {
                    if (json_data.row[i].type == "dob") {
                        temp_element.xtype = "dob";
                    } else {
                        temp_element.xtype = "displayfield";
                    }
                } else {
                    temp_element.xtype = "syslink";
                    temp_element.href = json_data.row[i].url;

                }
            } else {
                temp_element.fieldLabel = json_data.row[i].fieldLabel;
                temp_element.name = json_data.row[i].name;
                temp_element.hidden = json_data.row[i].hidden;
                if (json_data.row[i].type == "select" || json_data.row[i].type == "select_text" || json_data.row[i].type == "multselect" || json_data.row[i].type == "checkboxgroup" || json_data.row[i].type == "radiogroup") {
                    temp_element.value = json_data.row[i].display_value;
                } else if (json_data.row[i].type == 'email') {
                    temp_element.value = '<a href="mailto:' + check(json_data.row[i].value) + '">' + check(json_data.row[i].value) + '</a>';
                } else {
                    if (json_data.row[i].currencyMat) {
                        var sign = (json_data.row[i].sign == undefined ? "$" : json_data.row[i].sign);
                        if (isNaN(Number(json_data.row[i].value)) && json_data.row[i].value) {
                            var values = json_data.row[i].value.replace(sign, '');
                            var valuesList = values.split(',');
                            var valueInt = "";
                            for (i = 0; i < valuesList.length; i++) {
                                valueInt += valuesList[i];
                            }
                            temp_element.value = Ext.util.Format.currency(valueInt, sign);
                        } else {
                            temp_element.value = Ext.util.Format.currency(json_data.row[i].value, sign);
                        }
                    } else {
                        temp_element.value = json_data.row[i].value;
                    }
                }
                temp_element.anchor = "96%";
                if (json_data.row[i].url == null) {
                    if (json_data.row[i].type == "dob") {
                        temp_element.xtype = "dob";
                    } else {
                        temp_element.xtype = "displayfield";
                    }
                } else {
                    temp_element.xtype = "syslink";
                    temp_element.href = json_data.row[i].url;
                }
            }
            if (json_data.row[i].rowspan != null && json_data.row[i].rowspan != undefined) {
                temp_element.rowspan = json_data.row[i].rowspan;
            }
            if (json_data.row[i].colspan != null && json_data.row[i].colspan != undefined) {
                temp_element.colspan = json_data.row[i].colspan;
            }
            form_content[i] = temp_element;
            if (json_data.isType == true) {
                if (json_data.row[i].group != undefined) {
                    groupItem = Ext.create('Ext.form.FieldSet', {
                        collapsible: true,
                        title: json_data.row[i].group,
                        collapsed: false,
                        anchor: '100%',
                        layout: {
                            type: 'table',
                            columns: 2,
                            tableAttrs: {
                                style: {
                                    width: '100%',
                                    margin: '5px 0px 5px 0px'
                                }
                            },
                            tdAttrs: {
                                style: {
                                    width: '50%'
                                }
                            }
                        }
                    });
                    form_group[groupIndex] = groupItem;
                    groupIndex = groupIndex + 1;
                }
                if (groupItem != undefined) {
                    groupItem.add(temp_element);
                }
                if (json_data.row[i].type == 'title') {
                    if (json_data.row[i].titleLabel != undefined && json_data.row[i].titleLabel != '' && json_data.row[i].titleLabel != null) {
                        var fontSize = json_data.row[i].titleFontSize;//==undefined?'12px':json_data.row[i].titleFontSize;
                        var marginLeft = json_data.row[i].marginLeft == undefined ? '360' : json_data.row[i].marginLeft;
                        form_group[groupIndex] = {
                            xtype: 'displayfield',
                            style: 'text-align:center',
                            width: 1024,
                            value: '<font size=' + fontSize + '>' + json_data.row[i].titleLabel + '</font>'
                        };
                        groupIndex = groupIndex + 1;
                    }
                }
            }
            //////
        }
        if (json_data.isType != true) {
            form_items = Ext.create('Ext.container.Container', {
                anchor: '100%',
                layout: {
                    type: 'table',
                    columns: 2,
                    tableAttrs: {
                        style: {
                            width: '100%',
                            margin: '0px 5px 5px 0px'

                        }
                    },
                    tdAttrs: {
                        style: {
                            width: '50%'
                        }
                    }
                },
                items: form_content
            });
        } else {
            form_items = form_group;
        }
        var viewSimple = this.viewSimple = Ext.create('Ext.form.Panel', {
            url: json_data.post_url,
            title: json_data.title,
            iconCls: 'iconForm',
            method: 'POST',
            bodyStyle: 'padding:5px 5px 5px',
            width: '100%',
            fieldDefaults: {
                msgTarget: 'side',
                labelWidth: 150,
                labelStyle: 'font-weight: bold'
            },
            defaultType: 'textfield',
            items: form_items,
            cls: 'editform',
            listeners: {
                afterrender: function (thiz, action) { form_layout(thiz) }
            }
        });
        if (viewSimple.items.get(0).getXType() == 'fieldset') viewSimple.items.get(0).expand();
        if (target_div.innerHTML == null) {
            target_div.add(viewSimple);
        } else {

            if (this.noToolBar == false)
            {
                target_div.style.padding = "40px 0px 0px 5px"; // after change doc type to html5
            }
            else {
                target_div.style.padding = "5px 0px 0px 5px";
            }

            viewSimple.render(target_div);
        }
    },

    editForm: function (target_div, json_data, grid, tabUrl, itemId, itemId1, pre_tab) {
        //---set synchronous loading on this one to avoid problems with rendering
        Ext.apply(Ext.data.Connection.prototype, {
            async: false
        });

        var me = this;
        me.json_data = json_data;
        var dt = new Date();
        var is_pop = false;  // is pop up form or not
        var temp = Ext.create('Ext.container.Container', {
            anchor: '100%',
            layout: {
                type: 'table',
                columns: 2,
                tableAttrs: {
                    style: {
                        width: '100%',
                        margin: '0px 5px 5px 0px'

                    }
                },
                tdAttrs: {
                    style: {
                        width: '50%'
                    }
                }
            }

        });
        if (pre_tab != undefined) {
            me.pre_tab = pre_tab;
            if (tabUrl != undefined) {
                me.index_url = tabUrl;
            }
            if (itemId != undefined) {
                me.index_id = itemId;
            }
            if (itemId1 != undefined) {
                me.index_itemId = itemId1;
            }
        } else {
            me.tabUrl = tabUrl;
            me.itemId = itemId;
            me.itemId1 = itemId1;
        }

        if (target_div.innerHTML == null) {
            is_pop = true;
        } else {
            target_div.style.margin = "5px";
        }
  		// second time or more to click the EDIT button
        if (me.viewSimple != undefined) {
            me.viewSimple.hide();
        }
        if (me.editSimple != undefined) {
 			// reset data to original
            var theForm = me.editSimple.getForm();
            for (i = 0; i < me.json_data.row.length; i++) 
            {
                var data = json_data.row[i];
                var element = theForm.findField(data.name); //me.editSimple.items.items[0].items.items[i];
              
                //alert("n:" + data.name + ", ov: " + theForm.findField(data.name).getValue() + ", nv: " + data.value);

                if (data.type == 'dateTime') {
                    if (data.value != undefined) {
                        dateValue = data.value.substring(0, 10);
                        timeValue = data.value.substring(11, data.value.length);
                
                        element.setField1Value(dateValue);
                        element.setField2Value(timeValue);
                    }
                }
                else if (data.type == 'upload') {
                    element.setField1Value(data.value);
                }
                else {
                    element.setValue(data.value);
                }

                // Ext.get(''MyForm").getForm().findField("NameField").getValue();
                //if (data.type == "input" || data.type == "select" || data.type == "textarea") {
                //    alert("will data name: " + data.name);
                //    alert("name: " + element.name + ", element value: " + element.value + ", data.value: " + data.value);
                //    element.setValue("1");//data.value);
                //}
            }
            me.editSimple.show();
        } else {
            var form_content = [];
            var form_content_left = [];
            var form_content_right = [];

            var form_group = [];
            var form_items = undefined;
            var groupIndex = 0;
            var groupItem = undefined;

            var json_length = json_data.row.length;
            for (i = 0; i < json_data.row.length; i++) {
                var temp_element = {};
                if (json_data.row[i] != undefined) {

                    var regex = json_data.row[i].regex;
                    var regexText = json_data.row[i].regexText;
                    if (regex != undefined && regex != '') {
                        if (regex == "emailRegex") {
                            temp_element.regex = me.emailRegex;
                        } else if (regex == "numberRegex") {
                            temp_element.regex = me.numberRegex;
                        } else if (regex == "doubleRegex") {
                            temp_element.regex = me.doubleRegex;
                        } else if (regex == "intRegex") {
                            temp_element.regex = me.intRegex;
                        } else {
              
                            temp_element.regex = new RegExp(regex); // Ext.decode(regex); if use ext.decode, regex need = /regex content/
                        }
                        if (json_data.row[i].regexText != undefined) {
                            temp_element.regexText = json_data.row[i].regexText;
                        } else {
                            temp_element.regexText = me.regexText;
                        }
                    }
                   if (regexText != undefined && regexText != '') {
                        temp_element.regexText = regexText;
                    }
                    if (json_data.row[i].readOnly == true && json_data.row[i].readOnly != undefined) {
                        temp_element.fieldLabel = json_data.row[i].fieldLabel;
                        temp_element.name = json_data.row[i].name;
                        temp_element.value = json_data.row[i].value;
                        temp_element.hidden = json_data.row[i].hidden;
                        temp_element.anchor = "96%";
                        temp_element.xtype = "displayfield";
                        temp_element.cls = "readonlyField";
                        temp_element.width = json_data.row[i].width;
                        temp_element.height = json_data.row[i].height;
                    } else {
                        temp_element.anchor = "96%";
                        var allowBlank = true;
                        var toAllowBlank = true;
                        var labelCss = '';
                        var containerName = json_data.row[i].name + '_container';
                        if (json_data.row[i].allowBlank != undefined) {
                            allowBlank = json_data.row[i].allowBlank;
                            if (allowBlank == false) {
                                temp_element.labelStyle = 'color:red';
                                labelCss = 'color:red';
                            } else {
                                labelCss = '';
                            }
                        }
                        if (json_data.row[i] != undefined && json_data.row[i].toAllowBlank != undefined) {
                            toAllowBlank = json_data.row[i].toAllowBlank;
                        }
                        if (json_data.row[i] != undefined && json_data.row[i].width != undefined) {
                            temp_element.fieldStyle = "width:" + json_data.row[i].width + "px";
                        }

                        if (json_data.row[i].type == "input" || json_data.row[i].type == "email") { //??????????n????????????validationEngine();
                            temp_element.fieldLabel = json_data.row[i].fieldLabel;
                            temp_element.xtype = 'textfield';
                            if (json_length < 500) {
                                temp_element.plugins = ['clearbutton'];
                            }
                            temp_element.name = json_data.row[i].name;
                            temp_element.value = json_data.row[i].value;

                            temp_element.allowBlank = allowBlank;
                            temp_element.tabIndex = json_data.row[i].tabIndex;
                            temp_element.emptyText = json_data.row[i].emptyText;
                            temp_element.disabled = json_data.row[i].disabled;
                            temp_element.hidden = json_data.row[i].hidden;
                            temp_element.listeners = {};
                            if (json_data.row[i].handler != undefined && json_data.row[i].handler != null) {
                                temp_element.listeners.change = json_data.row[i].handler;
                            }
                        } else if (json_data.row[i].type == "checkboxgroup") { //??????????n????????????validationEngine();
                            temp_element.fieldLabel = json_data.row[i].fieldLabel;
                            temp_element.xtype = 'checkboxgroup';
                            temp_element.columnWidth = 0.3;
                            temp_element.name = json_data.row[i].name;
                            temp_element.columns = json_data.row[i].columns;
                            temp_element.vertical = json_data.row[i].vertical;
                            temp_element.items = json_data.row[i].items;
                            for (var h = 0; h < temp_element.items.length; h++) {
                                temp_element.items[h].uncheckedValue = '0';
                            }
                            temp_element.tabIndex = json_data.row[i].tabIndex;
                            temp_element.cls = 'field_no_padding';
                            temp_element.listeners = {
                                change: function (e, newValue, oldValue, options) {
                                    var form = this.up('form').getForm();
                                    var change_hidden = form.findField('change_fields');
                                    var temp = "";
                                    for (temO in this.originalValue) {
                                        temp = this.originalValue[temO];
                                    }
                                }
                            };
                        } else if (json_data.row[i].type == "radiogroup") { //??????????n????????????validationEngine();
                            temp_element.fieldLabel = json_data.row[i].fieldLabel;
                            temp_element.xtype = 'radiogroup';
                            temp_element.cls = 'field_no_padding';
                            temp_element.name = json_data.row[i].name;
                            temp_element.columns = json_data.row[i].columns;
                            temp_element.vertical = json_data.row[i].vertical;
                            temp_element.tabIndex = json_data.row[i].tabIndex;
                            var items = json_data.row[i].items;
                            var radio_items = [];

                            for (var m = 0; m < items.length; m++) {
                                var temp_item = {};
                                temp_item.boxLabel = items[m].boxLabel;
                                temp_item.name = items[m].name;
                                temp_item.inputValue = items[m].inputValue;
                                temp_item.checked = items[m].checked;
                                temp_item.listeners = {
                                    change: function (e, newValue, oldValue, options) {
                                        var form = this.up('form').getForm();
                                        var radiogroup = this.up('radiogroup').originalValue;
                                        var temp = "";
                                        for (temO in radiogroup) {
                                            temp = radiogroup[temO];
                                        }
                                    }
                                };
                                radio_items[radio_items.length] = temp_item;
                            }
                            temp_element.items = radio_items;

                        } else if (json_data.row[i].type == "month_date") {
                            temp_element = {
                                xtype: 'monthDate',
                                fieldLabel: json_data.row[i].fieldLabel,
                                name: json_data.row[i].name,
                                value: json_data.row[i].value
                            };

                        } else if (json_data.row[i].type == "select_text") { /////////////select_text
                            var value = json_data.row[i].value;
                            var sub_component = json_data.row[i].sub_component;
                            var store = Ext.create('Ext.data.Store', {
                                fields: [{
                                    name: 'id',
                                    type: 'string'
                                }, {
                                    name: 'value',
                                    type: 'string'
                                }, {
                                    name: 'textValue',
                                    type: 'string'
                                }],
                                autoLoad: false,
                                id: 'store_' + dt + '_' + json_data.row[i].name,
                                filters: json_data.row[i].filter,
                                proxy: {
                                    type: 'ajax',
                                    url: json_data.row[i].datasource,
                                    reader: {
                                        type: 'json',
                                        root: 'data',
                                        totalProperty: 'totalCount'
                                    },
                                    sorters: {
                                        property: 'value',
                                        direction: 'ASC'
                                    }

                                }
                            });
                            store.on('beforeload', function (thiz, action, value) {
                                if (thiz.getCount() == 0 && value && value != '') {
                                    thiz.proxy.extraParams.defaultValue = value;
                                }
                            }, true, value);
                            temp_element = Ext.create('com.embraiz.component.ComboBox', {
                                fieldLabel: json_data.row[i].fieldLabel,
                                forceSelection: true,
                                plugins: ['clearbutton'],
                                labelStyle: labelCss,
                                sub_component: sub_component,
                                name: json_data.row[i].name,
                                displayField: 'value',
                                emptyText: 'Please Select',
                                valueField: 'id',
                                forceSelection: true,
                                store: 'store_' + dt + '_' + json_data.row[i].name,
                                init_value: value,
                                value: value,
                                allowBlank: allowBlank,
                                tabIndex: json_data.row[i].tabIndex,
                                inputTextName: json_data.row[i].inputTextName,
                                init_load: 0,
                                queryMode: 'local',
                                listeners: {
                                    render: function () {
                                        var temp = this;
                                        temp.store.load(function (records, operation, success) {
                                            temp.setValue(temp.init_value);
                                            temp.init_load == 1;
                                        });
                                    }, beforequery: listener_ComboboxBeforeQuery,
                                    select: function () {
                                        
                                        var form = this.up('form').getForm();
                                        if (this.inputTextName != undefined) {
                                            for (var s = 0; s < this.store.data.items.length; s++) {
                                                if (this.store.data.keys[s] == this.value) {
                                                    
                                                    var subTextName = undefined;
                                                    alert(this.inputTextName);
                                                    if (this.inputTextName.indexOf("[,]") != -1) {
                                                        subTextName = this.inputTextName.split('[,]');
                                                    } else {
                                                        
                                                        subTextName = this.inputTextName.split(',');
                                                        
                                                    }
                                                    alert("subTextName"+subTextName);
                                                    if (this.store.data.items[s].data.textValue.indexOf("[,]") != -1) {
                                                       
                                                        var subTextValue = this.store.data.items[s].data.textValue.split('[,]');
                                                        for (var mn = 0; mn < subTextName.length; mn++) {
                                                            form.findField(subTextName[mn]).setValue(subTextValue[mn]);
                                                        }
                                                    } else {
                                                        
                                                        var subTextValue = this.store.data.items[s].data.textValue.split(',');
                                                        for (var mn = 0; mn < subTextName.length; mn++) {
                                                            alert("H2, mn:" + mn+ ", value:"+subTextValue[mn]);
                                                            form.findField(subTextName[mn]).setValue(subTextValue[mn]);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        var sub = this;
                                        while (sub.sub_component != undefined) {
                                            subName = sub.sub_component;
                                            var subfieldName = subName.split(',');
                                            for (var s_index = 0; s_index < subfieldName.length; s_index++) {
                                                sub = form.findField(subfieldName[s_index]);
                                                // sub = form.findField(sub.sub_component);
                                                sub.clearValue();
                                                var pvalue = this.value;
                                                if (sub.events.focus != true) {
                                                    sub.events.focus.clearListeners();
                                                }
                                                sub.addListener({
                                                    focus: function (e, op) {
                                                        if (e.store.proxy.url.indexOf("?filter") != -1) {
                                                            var other = "";
                                                            if (e.store.proxy.url.indexOf("&") != -1) {
                                                                other = e.store.proxy.url.substring(e.store.proxy.url.indexOf("&") + 1, e.store.proxy.url.length);
                                                            }
                                                            e.store.proxy.url = e.store.proxy.url.substring(0, e.store.proxy.url.indexOf("?filter"));
                                                            if (other != "") {
                                                                e.store.proxy.url += "?" + other;
                                                            }
                                                        }
                                                        e.store.load({
                                                            params: {
                                                                filter: pvalue
                                                            }
                                                        });
                                                    }
                                                });
                                            }
                                        }
                                    }
                                }
                            });
                            if (json_length < 500) {
                                temp_element.plugins = ['clearbutton'];
                            }
                            if (json_data.row[i].handler != undefined && json_data.row[i].handler != null) {
                                temp_element.addListener({ change: json_data.row[i].handler });
                            }
                            if (json_data.row[i].openLimit) {
                                var pageSize = 25;
                                if (json_data.row[i].pageSize) {
                                    pageSize = json_data.row[i].pageSize;
                                }
                                Ext.apply(temp_element, { pageSize: pageSize });
                                Ext.apply(temp_element.store, { pageSize: pageSize });
                                Ext.apply(temp_element.store.proxy, { extraParams: { openLimit: true } });
                            }
                        } else if (json_data.row[i].type == "select_ajax_text") {

                            var value = json_data.row[i].value;
                            var sub_component = json_data.row[i].sub_component;
                            var store = Ext.create('Ext.data.Store', {
                                fields: [{
                                    name: 'id',
                                    type: 'string'
                                }, {
                                    name: 'value',
                                    type: 'string'
                                }, {
                                    name: 'textValue',
                                    type: 'string'
                                }],
                                autoLoad: false,
                                id: 'store_' + dt + '_' + json_data.row[i].name,
                                filters: json_data.row[i].filter,
                                proxy: {
                                    type: 'ajax',
                                    url: json_data.row[i].datasource,
                                    reader: {
                                        type: 'json',
                                        root: 'data',
                                        totalProperty: 'totalCount'
                                    },
                                    sorters: {
                                        property: 'value',
                                        direction: 'ASC'
                                    }

                                }
                            });
                            store.on('beforeload', function (thiz, action, value) {
                                if (thiz.getCount() == 0 && value && value != '') {
                                    thiz.proxy.extraParams.defaultValue = value;
                                }
                            }, true, value);
                            temp_element = Ext.create('com.embraiz.component.ComboBox', {
                                fieldLabel: json_data.row[i].fieldLabel,
                                forceSelection: true,
                                plugins: ['clearbutton'],
                                labelStyle: labelCss,
                                sub_component: sub_component,
                                name: json_data.row[i].name,
                                displayField: 'value',
                                emptyText: 'Please Select',
                                valueField: 'id',
                                forceSelection: true,
                                store: 'store_' + dt + '_' + json_data.row[i].name,
                                init_value: value,
                                value: value,
                                allowBlank: allowBlank,
                                tabIndex: json_data.row[i].tabIndex,

                                inputFieldName: json_data.row[i].inputFieldName,
                                ajaxURL: json_data.row[i].ajaxURL,

                                init_load: 0,
                                queryMode: 'local',
                                listeners: {
                                    render: function () {
                                        var temp = this;
                                        temp.store.load(function (records, operation, success) {
                                            temp.setValue(temp.init_value);
                                            temp.init_load == 1;
                                        });
                                    }, beforequery: listener_ComboboxBeforeQuery,
                                    select: function () {

                                        var form = this.up('form').getForm();

                                        var targetInputField = form.findField(this.inputFieldName);
                                        Ext.Ajax.request({
                                            url: this.ajaxURL,
                                            params: {
                                                select_value: this.value
                                            },
                                            success: function (o) {
                                                var theAjaxData = Ext.decode(o.responseText);

                                                if (theAjaxData.success == true) {
                                                    targetInputField.setValue(theAjaxData.updateText);
                                                }
                                            }
                                        });
                                    }
                                }
                            });
                            if (json_length < 500) {
                                temp_element.plugins = ['clearbutton'];
                            }
                            if (json_data.row[i].handler != undefined && json_data.row[i].handler != null) {
                                temp_element.addListener({ change: json_data.row[i].handler });
                            }
                            if (json_data.row[i].openLimit) {
                                var pageSize = 25;
                                if (json_data.row[i].pageSize) {
                                    pageSize = json_data.row[i].pageSize;
                                }
                                Ext.apply(temp_element, { pageSize: pageSize });
                                Ext.apply(temp_element.store, { pageSize: pageSize });
                                Ext.apply(temp_element.store.proxy, { extraParams: { openLimit: true } });
                            }
                        } else if (json_data.row[i].type == "select_ajax_threeText") {

                            var value = json_data.row[i].value;
                            var sub_component = json_data.row[i].sub_component;
                            var store = Ext.create('Ext.data.Store', {
                                fields: [{
                                    name: 'id',
                                    type: 'string'
                                }, {
                                    name: 'value',
                                    type: 'string'
                                }, {
                                    name: 'textValue',
                                    type: 'string'
                                }],
                                autoLoad: false,
                                id: 'store_' + dt + '_' + json_data.row[i].name,
                                filters: json_data.row[i].filter,
                                proxy: {
                                    type: 'ajax',
                                    url: json_data.row[i].datasource,
                                    reader: {
                                        type: 'json',
                                        root: 'data',
                                        totalProperty: 'totalCount'
                                    },
                                    sorters: {
                                        property: 'value',
                                        direction: 'ASC'
                                    }

                                }
                            });
                            store.on('beforeload', function (thiz, action, value) {
                                if (thiz.getCount() == 0 && value && value != '') {
                                    thiz.proxy.extraParams.defaultValue = value;
                                }
                            }, true, value);
                            temp_element = Ext.create('com.embraiz.component.ComboBox', {
                                fieldLabel: json_data.row[i].fieldLabel,
                                forceSelection: true,
                                plugins: ['clearbutton'],
                                labelStyle: labelCss,
                                sub_component: sub_component,
                                name: json_data.row[i].name,
                                displayField: 'value',
                                emptyText: 'Please Select',
                                valueField: 'id',
                                forceSelection: true,
                                store: 'store_' + dt + '_' + json_data.row[i].name,
                                init_value: value,
                                value: value,
                                allowBlank: allowBlank,
                                tabIndex: json_data.row[i].tabIndex,

                                inputFieldName1: json_data.row[i].inputFieldName1,
                                inputFieldName2: json_data.row[i].inputFieldName2,
                                inputFieldName3: json_data.row[i].inputFieldName3,
                                ajaxURL: json_data.row[i].ajaxURL,

                                init_load: 0,
                                queryMode: 'local',
                                listeners: {
                                    render: function () {
                                        var temp = this;
                                        temp.store.load(function (records, operation, success) {
                                            temp.setValue(temp.init_value);
                                            temp.init_load == 1;
                                        });
                                    }, beforequery: listener_ComboboxBeforeQuery,
                                    select: function () {

                                        var form = this.up('form').getForm();

                                        var targetInputField1 = form.findField(this.inputFieldName1);
                                        var targetInputField2 = form.findField(this.inputFieldName2);
                                        var targetInputField3 = form.findField(this.inputFieldName3);

                                        Ext.Ajax.request({
                                            url: this.ajaxURL,
                                            params: {
                                                select_value: this.value
                                            },
                                            success: function (o) {
                                                var theAjaxData = Ext.decode(o.responseText);

                                                if (theAjaxData.success == true) {
                                                    targetInputField1.setValue(theAjaxData.updateText1);
                                                    targetInputField2.setValue(theAjaxData.updateText2);
                                                    targetInputField3.setValue(theAjaxData.updateText3);
                                                }
                                            }
                                        });
                                    }
                                }
                            });
                            if (json_length < 500) {
                                temp_element.plugins = ['clearbutton'];
                            }
                            if (json_data.row[i].handler != undefined && json_data.row[i].handler != null) {
                                temp_element.addListener({ change: json_data.row[i].handler });
                            }
                            if (json_data.row[i].openLimit) {
                                var pageSize = 25;
                                if (json_data.row[i].pageSize) {
                                    pageSize = json_data.row[i].pageSize;
                                }
                                Ext.apply(temp_element, { pageSize: pageSize });
                                Ext.apply(temp_element.store, { pageSize: pageSize });
                                Ext.apply(temp_element.store.proxy, { extraParams: { openLimit: true } });
                            }
                        } else if (json_data.row[i].type == "select_ajax_fourText") {

                            var value = json_data.row[i].value;
                            var sub_component = json_data.row[i].sub_component;
                            var store = Ext.create('Ext.data.Store', {
                                fields: [{
                                    name: 'id',
                                    type: 'string'
                                }, {
                                    name: 'value',
                                    type: 'string'
                                }, {
                                    name: 'textValue',
                                    type: 'string'
                                }],
                                autoLoad: false,
                                id: 'store_' + dt + '_' + json_data.row[i].name,
                                filters: json_data.row[i].filter,
                                proxy: {
                                    type: 'ajax',
                                    url: json_data.row[i].datasource,
                                    reader: {
                                        type: 'json',
                                        root: 'data',
                                        totalProperty: 'totalCount'
                                    },
                                    sorters: {
                                        property: 'value',
                                        direction: 'ASC'
                                    }

                                }
                            });
                            store.on('beforeload', function (thiz, action, value) {
                                if (thiz.getCount() == 0 && value && value != '') {
                                    thiz.proxy.extraParams.defaultValue = value;
                                }
                            }, true, value);
                            temp_element = Ext.create('com.embraiz.component.ComboBox', {
                                fieldLabel: json_data.row[i].fieldLabel,
                                forceSelection: true,
                                plugins: ['clearbutton'],
                                labelStyle: labelCss,
                                sub_component: sub_component,
                                name: json_data.row[i].name,
                                displayField: 'value',
                                emptyText: 'Please Select',
                                valueField: 'id',
                                forceSelection: true,
                                store: 'store_' + dt + '_' + json_data.row[i].name,
                                init_value: value,
                                value: value,
                                allowBlank: allowBlank,
                                tabIndex: json_data.row[i].tabIndex,

                                inputFieldName1: json_data.row[i].inputFieldName1,
                                inputFieldName2: json_data.row[i].inputFieldName2,
                                inputFieldName3: json_data.row[i].inputFieldName3,
                                inputFieldName4: json_data.row[i].inputFieldName4,
                                ajaxURL: json_data.row[i].ajaxURL,

                                init_load: 0,
                                queryMode: 'local',
                                listeners: {
                                    render: function () {
                                        var temp = this;
                                        temp.store.load(function (records, operation, success) {
                                            temp.setValue(temp.init_value);
                                            temp.init_load == 1;
                                        });
                                    }, beforequery: listener_ComboboxBeforeQuery,
                                    select: function () {

                                        var form = this.up('form').getForm();

                                        var targetInputField1 = form.findField(this.inputFieldName1);
                                        var targetInputField2 = form.findField(this.inputFieldName2);
                                        var targetInputField3 = form.findField(this.inputFieldName3);
                                        var targetInputField4 = form.findField(this.inputFieldName4);

                                        Ext.Ajax.request({
                                            url: this.ajaxURL,
                                            params: {
                                                select_value: this.value
                                            },
                                            success: function (o) {
                                                var theAjaxData = Ext.decode(o.responseText);

                                                if (theAjaxData.success == true) {
                                                    targetInputField1.setValue(theAjaxData.updateText1);
                                                    targetInputField2.setValue(theAjaxData.updateText2);
                                                    targetInputField3.setValue(theAjaxData.updateText3);
                                                    targetInputField4.setValue(theAjaxData.updateText4);
                                                }
                                            }
                                        });
                                    }
                                }
                            });
                            if (json_length < 500) {
                                temp_element.plugins = ['clearbutton'];
                            }
                            if (json_data.row[i].handler != undefined && json_data.row[i].handler != null) {
                                temp_element.addListener({ change: json_data.row[i].handler });
                            }
                            if (json_data.row[i].openLimit) {
                                var pageSize = 25;
                                if (json_data.row[i].pageSize) {
                                    pageSize = json_data.row[i].pageSize;
                                }
                                Ext.apply(temp_element, { pageSize: pageSize });
                                Ext.apply(temp_element.store, { pageSize: pageSize });
                                Ext.apply(temp_element.store.proxy, { extraParams: { openLimit: true } });
                            }
                        } else if (json_data.row[i].type == "select") {
                            
                            var value = json_data.row[i].value;
                            var sub_component = json_data.row[i].sub_component;
                      

                            //---set synchronous loading on this one to avoid problems with rendering
                            //Ext.apply(Ext.data.Connection.prototype, {
                            //    async: false
                            //});

                            console.log("start datastore: " + new Date().toISOString());
                            var store = Ext.create('Ext.data.Store', {
                                fields: [{
                                    name: 'id',
                                    type: 'string'
                                }, {
                                    name: 'value',
                                    type: 'string'
                                }],
                                remoteSort: true,
                                autoLoad: false,
                                id: 'store_' + dt + '_' + json_data.row[i].name,
                                
                                proxy: {
                                    type: 'ajax',
                                    url: json_data.row[i].datasource,
                                 
                                    reader: {
                                        type: 'json',
                                        root: 'data',
                                        totalProperty: 'totalCount'
                                    },
                                    sorters: {
                                        property: 'value',
                                        direction: 'ASC'
                                    }
                                }
                            });
                            console.log("end datastore: " + new Date().toISOString());

                            store.on('beforeload', function (thiz, action, value) {
                                if (thiz.getCount() == 0 && value && value != '') {
                                    thiz.proxy.extraParams.defaultValue = value;
                                }
                            }, true, value);

                            //$.ajax({
                            //    url: json_data.row[i].datasource,
                            //    dataType: "json",
                            //    type: "GET",
                            //    async: false
                            //}).done(function (o) {
                            //   // store.loadData(o.data);
                            //});

                            temp_element = Ext.create('com.embraiz.component.ComboBox', {
                                fieldLabel: json_data.row[i].fieldLabel,
                                labelStyle: labelCss,
                                plugins: ['clearbutton'],
                                forceSelection: true,
                                disabled: json_data.row[i].disabled,
                                sub_component: sub_component,
                                name: json_data.row[i].name,
                                displayField: 'value',
                                emptyText: 'Please Select',
                                valueField: 'id',
                                store: 'store_' + dt + '_' + json_data.row[i].name,
                                init_value: value,
                                value: value,
                                allowBlank: allowBlank,
                                tabIndex: json_data.row[i].tabIndex,
                                init_load: 0,
                                queryMode: 'local',
                                hidden: json_data.row[i].hidden,
                                sync: json_data.row[i].sync,
                                listeners: {
                                    render: function () {
                                        var temp = this;

                                        temp.store.load(function (records, operation, success) {
                                            temp.setValue(temp.init_value);
                                            temp.init_load == 1;
                                            ///
                                            if (temp.init_value != null && temp.init_value != undefined) {
                                                var form = temp.up('form').getForm();
                                                var sub = temp;
                                                while (sub.sub_component != undefined) {
                                                    subName = sub.sub_component;
                                                    var subfieldName = subName.split(',');
                                                    for (var s_index = 0; s_index < subfieldName.length; s_index++) {
                                                        sub = form.findField(subfieldName[s_index]);
                                                        sub.clearValue();
                                                        var pvalue = temp.value;
                                                        if (sub.events.focus != true) {
                                                            sub.events.focus.clearListeners();
                                                        }
                                                        sub.addListener({
                                                            focus: function (e, op) {
                                                                if (e.store.proxy.url.indexOf("?filter") != -1) {
                                                                    var other = "";
                                                                    if (e.store.proxy.url.indexOf("&") != -1) {
                                                                        other = e.store.proxy.url.substring(e.store.proxy.url.indexOf("&") + 1, e.store.proxy.url.length);
                                                                    }
                                                                    e.store.proxy.url = e.store.proxy.url.substring(0, e.store.proxy.url.indexOf("?filter"));
                                                                    if (other != "") {
                                                                        e.store.proxy.url += "?" + other;
                                                                    }
                                                                }
                                                                e.store.params = {};
                                                                e.store.load({
                                                                    params: {
                                                                        filter: pvalue
                                                                    }
                                                                });
                                                            }
                                                        });
                                                        sub.fireEvent('focus', sub, sub.store);
                                                    }
                                                }
                                                if (temp.sync != undefined && temp.sync != '') {
                                                    var f = form.findField(temp.sync);
                                                    if (parent.id == undefined) {
                                                        if (temp.xtype == 'combobox') {
                                                            f.select(temp.getValue());
                                                            f.fireEvent('select', f, temp);
                                                        } else {
                                                            f.setValue(temp.getValue());
                                                        }
                                                    }
                                                }
                                            }
                                        }); // end temp.store.load
                                    },
                                    beforequery: listener_ComboboxBeforeQuery,

                                    select: function (thiz, parent) {
                                        var form = this.up('form').getForm();

                                        var sub = this;
                                        while (sub.sub_component != undefined) {
                                            subName = sub.sub_component;
                                            var subfieldName = subName.split(',');
                                            for (var s_index = 0; s_index < subfieldName.length; s_index++) {
                                                sub = form.findField(subfieldName[s_index]);
                                                sub.clearValue();
                                                var pvalue = this.value;
                                                if (sub.events.focus != true) {
                                                    sub.events.focus.clearListeners();
                                                }
                                                sub.addListener({
                                                    focus: function (e, op) {
                                                        if (e.store.proxy.url.indexOf("?filter") != -1) {
                                                            var other = "";
                                                            if (e.store.proxy.url.indexOf("&") != -1) {
                                                                other = e.store.proxy.url.substring(e.store.proxy.url.indexOf("&") + 1, e.store.proxy.url.length);
                                                            }
                                                            e.store.proxy.url = e.store.proxy.url.substring(0, e.store.proxy.url.indexOf("?filter"));
                                                            if (other != "") {
                                                                e.store.proxy.url += "?" + other;
                                                            }
                                                        }
                                                        e.store.params = {};
                                                        e.store.load({
                                                            params: {
                                                                filter: pvalue
                                                            }
                                                        });
                                                    }
                                                });
                                                sub.fireEvent('focus', sub, sub.store);
                                            }
                                        }
                                        if (this.sync != undefined && this.sync != '') {
                                            var f = form.findField(this.sync);
                                            if (parent.id == undefined) {
                                                if (this.xtype == 'combobox') {
                                                    f.select(this.getValue());
                                                    f.fireEvent('select', f, this);
                                                } else {
                                                    f.setValue(this.getValue());
                                                }
                                            }
                                        }
                                    }
                                }
                            });

                            //---restore async property to the default value
                            //Ext.apply(Ext.data.Connection.prototype, {
                            //    async: true
                            //});

                            if (json_data.row[i].handler != undefined && json_data.row[i].handler != null) {
                                temp_element.addListener({ change: json_data.row[i].handler });
                            }
                            if (json_length < 500) {
                                temp_element.plugins = ['clearbutton'];
                            }
                            if (json_data.row[i].openLimit) {
                                var pageSize = 25;
                                if (json_data.row[i].pageSize) {
                                    pageSize = json_data.row[i].pageSize;
                                }
                                Ext.apply(temp_element, { pageSize: pageSize });
                                Ext.apply(temp_element.store, { pageSize: pageSize });
                                Ext.apply(temp_element.store.proxy, { extraParams: { openLimit: true } });
                            }

                         
                        } else if (json_data.row[i].type == "multselect") { //?????????????????????????????????????????????
                            var multstore = Ext.create('Ext.data.Store', {
                                fields: [{
                                    name: 'id',
                                    type: 'string'
                                }, {
                                    name: 'value',
                                    type: 'string'
                                }],
                                autoLoad: true,
                                proxy: {
                                    type: 'ajax',
                                    url: json_data.row[i].datasource,
                                    reader: {
                                        type: 'json',
                                        root: 'data',
                                        totalProperty: 'totalCount'

                                    }
                                }
                            });
                            multstore.on('beforeload', function (thiz, op) {
                                Ext.getBody().mask();
                            })
                            multstore.on('load', function (thiz, op, success) {
                                setTimeout(function a() { Ext.getBody().unmask(); }, 1000);
                            })
                            var multi_combox_config = {
                                fieldLabel: json_data.row[i].fieldLabel,
                                labelStyle: labelCss,
                                name: json_data.row[i].name,
                                multiSelect: true,
                                allowBlank: allowBlank,
                                tabIndex: json_data.row[i].tabIndex,
                                init_value: json_data.row[i].value,
                                value: json_data.row[i].value,
                                emptyText: 'Please Select',
                                displayField: 'value',
                                valueField: 'id',
                                forceSelection: true,
                                store: multstore,
                                queryMode: 'local',
                                cls: 'readonlyField',
                                listeners: {
                                    render: function () {
                                        var me = this;
                                        var initValueArray = undefined;
                                        if (me.init_value != undefined && me.init_value != '' && me.init_value != null && me.init_value != 'null') {
                                            var initValue = me.init_value;
                                            if (Ext.encode(initValue).indexOf('"') != -1) {
                                                initValueArray = initValue;
                                            } else {
                                                for (var i = 0; i < initValue.length; i++) {
                                                    initValue[i] = String(initValue[i]);
                                                }
                                                initValueArray = initValue;
                                            }
                                            me.setValue(initValueArray);
                                        }
                                    },
                                    select: function () {
                                        var me = this;
                                        me.setValue(me.lastValue);
                                    }
                                }
                            };

                            if (json_length < 500) {
                                //temp_element = Ext.create('Ext.ux.form.field.BoxSelect', multi_combox_config);
                                temp_element = Ext.create('Ext.ux.ComboFieldBox', multi_combox_config);
                            } else {
                                temp_element = Ext.create('com.embraiz.component.ComboBox', multi_combox_config);
                            }
                        } else if (json_data.row[i].type == "date" || json_data.row[i].type == "dob") { //?????????????????????????????
                            temp_element.format = 'Y-m-d';
                            temp_element.altFormats = 'ymd|Ymd|Y-m-d|Y.m.d|y.m.d',
                            temp_element.fieldLabel = json_data.row[i].fieldLabel;
                            temp_element.name = json_data.row[i].name;
                            temp_element.value = json_data.row[i].value;
                            temp_element.xtype = "datefield";
                            temp_element.allowBlank = allowBlank;
                            temp_element.width = 270;

                            temp_element.tabIndex = json_data.row[i].tabIndex;
                            temp_element.hidden = json_data.row[i].hidden;
                            temp_element.listeners = {};

                            if (json_data.row[i].handler != undefined && json_data.row[i].handler != null) {
                                temp_element.listeners.change = json_data.row[i].handler;
                            }
                        } else if (json_data.row[i].type == "dateRange" || json_data.row[i].type == "date_check") {
                            temp_element.xtype = json_data.row[i].type;
                            temp_element.fieldLabel = json_data.row[i].fieldLabel;
                            temp_element.field1Value = json_data.row[i].value;
                            temp_element.field1Name = json_data.row[i].name;
                            temp_element.hand = json_data.row[i].handler;
                            temp_element.tabIndex = json_data.row[i].tabIndex;
                            if (json_data.row[i].type == "dateRange") {
                                temp_element.toFieldLabel = json_data.row[i].tofieldLabel;
                                temp_element.field2Name = json_data.row[i].toName;
                                temp_element.field2Value = json_data.row[i].toValue;
                            } else if (json_data.row[i].type == "date_check") {
                                temp_element.field2Name = json_data.row[i].checkboxName;
                                temp_element.field2Value = json_data.row[i].checked;
                            }
                        } else if (json_data.row[i].type == "dateTimeRange") {
                            // this part is not in embraiz version

                            temp_element.xtype = json_data.row[i].type;

                            temp_element.hand = json_data.row[i].handler;
                            temp_element.tabIndex = json_data.row[i].tabIndex;

                            temp_element.fieldLabel = json_data.row[i].fieldLabel;
                            temp_element.toFieldLabel = json_data.row[i].tofieldLabel;

                            // from
                            var dt = new Date(); // today
                            if (json_data.row[i].value != undefined && json_data.row[i].value != '')
                                dt = new Date(json_data.row[i].value);

                            var theDate = dt.getFullYear() + '-' + ((dt.getMonth() + 1) < 10 ? '0' + (dt.getMonth() + 1) : (dt.getMonth() + 1)) + '-' + (dt.getDate() < 10 ? ('0' + dt.getDate()) : dt.getDate());
                            var theTime = (dt.getHours() < 10 ? '0' + dt.getHours() : dt.getHours()) + ":" + (dt.getMinutes() < 10 ? '0' + dt.getMinutes() : dt.getMinutes());

                            temp_element.field1Name = json_data.row[i].name;
                            temp_element.field1Value = theDate;

                            temp_element.field2Name = json_data.row[i].timeFromName;
                            temp_element.field2Value = theTime;

                            // to
                            var dt = new Date(); // today
                            if (json_data.row[i].value != undefined && json_data.row[i].value != '')
                                dt = new Date(json_data.row[i].toValue);

                            var theDate = dt.getFullYear() + '-' + ((dt.getMonth() + 1) < 10 ? '0' + (dt.getMonth() + 1) : (dt.getMonth() + 1)) + '-' + (dt.getDate() < 10 ? ('0' + dt.getDate()) : dt.getDate());
                            var theTime = (dt.getHours() < 10 ? '0' + dt.getHours() : dt.getHours()) + ":" + (dt.getMinutes() < 10 ? '0' + dt.getMinutes() : dt.getMinutes());

                            temp_element.field3Name = json_data.row[i].toName;
                            temp_element.field3Value = theDate;

                            temp_element.field4Name = json_data.row[i].timeToName;
                            temp_element.field4Value = theTime;

                        } else if (json_data.row[i].type == "checkbox_label") {
                            temp_element.xtype = json_data.row[i].type;
                            temp_element.fieldLabel = json_data.row[i].fieldLabel;
                            temp_element.field1Value = json_data.row[i].value;
                            temp_element.field1Name = json_data.row[i].name;
                            temp_element.field2Name = json_data.row[i].checkboxName;
                            temp_element.field2Value = json_data.row[i].checked;
                            temp_element.tabIndex = json_data.row[i].tabIndex;
                        } else if (json_data.row[i].type == "product") {
                            temp_element.xtype = json_data.row[i].type;
                            temp_element.field1Name = json_data.row[i].field1Name;
                            temp_element.field2Name = json_data.row[i].field2Name;
                            temp_element.field3Name = json_data.row[i].field3Name;
                            temp_element.field4Name = json_data.row[i].field4Name;
                            temp_element.field1Value = json_data.row[i].field1Value;
                            temp_element.field2Value = json_data.row[i].field2Value;
                            temp_element.field3Value = json_data.row[i].field3Value;
                            temp_element.field4Value = json_data.row[i].field4Value;
                            temp_element.datasource1 = json_data.row[i].datasource1;
                            temp_element.datasource2 = json_data.row[i].datasource2;
                            temp_element.datasource3 = json_data.row[i].datasource3;
                            temp_element.datasource4 = json_data.row[i].datasource4;
                            temp_element.tabIndex = json_data.row[i].tabIndex;
                            temp_element.name = json_data.row[i].name;
                        } else if (json_data.row[i].type == "multiUploadDialog") {
                            temp_element.xtype = json_data.row[i].type;
                            temp_element.name = json_data.row[i].name;
                            temp_element.images = json_data.row[i].images;
                            temp_element.uploadUrl = json_data.row[i].uploadUrl;
                            temp_element.editUrl = json_data.row[i].editUrl;
                            temp_element.editDataUrl = json_data.row[i].editDataUrl;
                            temp_element.removeUrl = json_data.row[i].removeUrl;


                        } else if (json_data.row[i].type == "dateTime") {
                            var values = json_data.row[i].value;
                            dateValue = '';
                            timeValue = '';
                            disabledTime = false;
                            if (json_data.row[i].disabledTime != undefined) {
                                disabledTime = json_data.row[i].disabledTime;
                            }
                            if (values != undefined) {
                                dateValue = values.substring(0, 10);
                                timeValue = values.substring(11, values.length);
                            }
                            temp_element.xtype = json_data.row[i].type;
                            temp_element.name = json_data.row[i].name;
                            temp_element.value = json_data.row[i].value;
                            temp_element.hand = json_data.row[i].handler;
                            temp_element.fieldLabel = json_data.row[i].fieldLabel;
                            temp_element.field1Value = dateValue;
                            temp_element.field1Name = json_data.row[i].name + 'dateValue';
                            temp_element.tabIndex = json_data.row[i].tabIndex;
                            temp_element.allowBlank = allowBlank;
                            if (json_data.row[i].type == "dateTime") {
                                temp_element.field2Name = json_data.row[i].name + 'time';
                                temp_element.field2Value = timeValue;
                                temp_element.field2Disabled = disabledTime;
                            }
                        } else if (json_data.row[i].type == "select_select" || json_data.row[i].type == "firstname_inputs" || json_data.row[i].type == "firstname_input" || json_data.row[i].type == "select_input") {
                            temp_element.xtype = json_data.row[i].type;
                            temp_element.fieldLabel = json_data.row[i].fieldLabel;
                            temp_element.field1Value = json_data.row[i].value;
                            temp_element.datasource = json_data.row[i].datasource;
                            temp_element.field1Name = json_data.row[i].name;
                            temp_element.tabIndex = json_data.row[i].tabIndex;
                            temp_element.pageSize = json_data.row[i].pageSize;
                            temp_element.openLimit = json_data.row[i].openLimit;
                            if (json_data.row[i].type == "select_select") {
                                temp_element.field2Name = json_data.row[i].selectName;
                                temp_element.field2Value = json_data.row[i].selectValue;
                                temp_element.selectDatasource = json_data.row[i].selectDatasource;
                                temp_element.selectFieldLabel = json_data.row[i].selectFieldLabel;
                            } else {
                                temp_element.field2Name = json_data.row[i].input_name;
                                temp_element.field2Value = json_data.row[i].input_value;
                            }

                        } else if (json_data.row[i].type == "TRM_REF") {
                            temp_element.xtype = json_data.row[i].type;
                            temp_element.fieldLabel = 'TRM REF';
                            temp_element.field1Value = json_data.row[i].value;
                            temp_element.field1Name = json_data.row[i].name;
                            temp_element.field2Name = json_data.row[i].name1;
                            temp_element.field2Value = json_data.row[i].value1;
                            temp_element.inputFieldLabel = json_data.row[i].fieldLabel;
                            temp_element.input2FieldLabel = json_data.row[i].fieldLabel1;
                            temp_element.tabIndex = json_data.row[i].tabIndex;
                        } else if (json_data.row[i].type == "currencyFormat") {
                            temp_element.xtype = json_data.row[i].type;
                            temp_element.fieldLabel = json_data.row[i].fieldLabel;
                            temp_element.field1Name = json_data.row[i].name;
                            temp_element.tabIndex = json_data.row[i].tabIndex;
                            temp_element.sign = json_data.row[i].sign;

                            var sign = (json_data.row[i].sign == undefined ? "$" : json_data.row[i].sign);
                            if (isNaN(Number(json_data.row[i].value))) {
                                var values = json_data.row[i].value.replace(sign, '');
                                var valuesList = values.split(',');
                                var valueInt = "";
                                for (i = 0; i < valuesList.length; i++) {
                                    valueInt += valuesList[i];
                                }
                                temp_element.field1Value = Ext.util.Format.currency(valueInt, sign);
                            } else {
                                temp_element.field1Value = Ext.util.Format.currency(json_data.row[i].value, sign);
                            }
                        } else if (json_data.row[i].type == "time") {
                            temp_element = {
                                xtype: 'timefield',
                                name: json_data.row[i].name,
                                value: json_data.row[i].value,
                                fieldLabel: json_data.row[i].fieldLabel,
                                format: 'H:i',
                                // margin: '3 5 3 5',
                                increment: 1,
                                // allowBlank: json_data.row[i].allowBlank,
                                tabIndex: json_data.row[i].tabIndex
                            }
                        } else if (json_data.row[i].type == "label") {
                            temp_element = {
                                xtype: 'label',
                                text: json_data.row[i].fieldLabel,
                                margin: '0 5 3 0'
                            };
                        } else if (json_data.row[i].type == "Linkage") { //????????????????????
                            temp_element.xtype = "container";
                            temp_element.layout = 'hbox';
                            temp_element.items = [{
                                xtype: 'textfield',
                                name: json_data.row[i].name,
                                fieldLabel: json_data.row[i].fieldLabel,
                                value: json_data.row[i].value,
                                margin: '0 5 3 0',
                                allowBlank: allowBlank,
                                tabIndex: json_data.row[i].tabIndex
                            }, {
                                xtype: 'button',
                                text: 'Search',
                                margin: '0 5 3 0',
                                handler: function () {
                                    me.popTree(temp_element);
                                }
                            }
                            ];
                        } else if (json_data.row[i].type == "search_box") {
                            temp_element.xtype = 'searchbox';
                            temp_element.fieldLabel = json_data.row[i].fieldLabel;
                            temp_element.field1Value = json_data.row[i].value;
                            temp_element.field1Name = json_data.row[i].name;
                            temp_element.field2Value = json_data.row[i].value_hidden;
                            temp_element.datejson = json_data.row[i].grid_url;
                            temp_element.tabIndex = json_data.row[i].tabIndex;
                            temp_element.handler = function () {
                                var form = this.up('form');
                                me.popSearch(form.id, this.up('searchbox').datejson);
                            };

                        } else if (json_data.row[i].type == "upload") {
                            temp_element.xtype = 'upload';
                            temp_element.name = json_data.row[i].name;
                            temp_element.fieldLabel = json_data.row[i].fieldLabel;
                            temp_element.fileName = json_data.row[i].fileName;
                            temp_element.field1Value = json_data.row[i].value;
                            temp_element.field1Name = json_data.row[i].name;
                            temp_element.type = json_data.row[i].upType;
                            temp_element.filetype = json_data.row[i].filetype;
                            temp_element.uploadUrl = json_data.row[i].upload_url;
                            //temp_element.downloadpath = json_data.row[i].downloadpath;
                        } else if (json_data.row[i].type == "textarea") { //??????????n????????????
                            temp_element.xtype = 'textarea';
                            temp_element.fieldLabel = json_data.row[i].fieldLabel;
                            temp_element.name = json_data.row[i].name;
                            temp_element.value = json_data.row[i].value;
                            temp_element.tabIndex = json_data.row[i].tabIndex;
                            temp_element.allowBlank = allowBlank;
                            
                            if (json_data.row[i].height == undefined || json_data.row[i].height == 0)
                                temp_element.height = 100;
                            else
                                temp_element.height = json_data.row[i].height;

                        } else if (json_data.row[i].type == "texteditor") { //htmleditor
                            temp_element.fieldLabel = json_data.row[i].fieldLabel;
                            temp_element.name = json_data.row[i].name;
                            temp_element.value = json_data.row[i].value;
                            temp_element.width = json_data.row[i].width;
                            temp_element.height = json_data.row[i].height;
                            temp_element.xtype = 'htmleditor';
                            temp_element.style = 'background-color: white;';
                            temp_element.anchor = '100%';
                            temp_element.labelWidth = 150;
                            temp_element.margin = '5 5 3 5';
                            temp_element.plugins = [new Ext.create('Ext.ux.form.HtmlEditor.imageUpload', {})];

                            //temp_element.plugins=[new Ext.create('Ext.ux.form.HtmlEditor.imageUpload', {})];

                        } else if (json_data.row[i].type == "passWord") {
                            temp_element.xtype = 'textfield';
                            temp_element.fieldLabel = json_data.row[i].fieldLabel;
                            temp_element.inputType = 'password';
                            temp_element.name = json_data.row[i].name;
                            temp_element.value = json_data.row[i].value;
                            temp_element.tabIndex = json_data.row[i].tabIndex;
                        } else if (json_data.row[i].type == "currency") {
                            temp_element.xtype = 'numberfield';
                            temp_element.fieldLabel = json_data.row[i].fieldLabel;
                            temp_element.name = json_data.row[i].name;
                            temp_element.value = json_data.row[i].value;
                            temp_element.tabIndex = json_data.row[i].tabIndex;
                        } else if (json_data.row[i].type == "input_check") {
                            var handler = {};
                            if (json_data.row[i].handler != undefined) {
                                handler = json_data.row[i].handler;
                            }
                            temp_element = Ext.create('Ext.container.Container', {
                                name: containerName,
                                itemCls: 'dateRange_cls',
                                layout: {
                                    type: 'hbox'
                                },
                                items: [{
                                    xtype: 'textfield',
                                    fieldLabel: json_data.row[i].fieldLabel,
                                    name: json_data.row[i].name,
                                    margin: '3 5 3 5',
                                    value: json_data.row[i].value,
                                    listeners: {
                                        change: handler
                                    }

                                }, {
                                    boxLabel: json_data.row[i].checkboxLabel,
                                    xtype: 'checkbox',
                                    name: json_data.row[i].checkboxName,
                                    inputValue: '1',
                                    uncheckedValue: '0',
                                    margin: '0 5 3 0',
                                    checked: json_data.row[i].checked

                                }]
                            });
                        } else if (json_data.row[i].type == "date_input") {
                            // this part else-if is not is embraiz version
                            temp_element.fieldLabel = json_data.row[i].fieldLabel;
                            temp_element.name1 = json_data.row[i].name;
                            temp_element.value = json_data.row[i].value == undefined ? '' : json_data.row[i].value;
                            temp_element.xtype = "date_input";
                            temp_element.allowBlank = allowBlank;
                            temp_element.tabIndex = json_data.row[i].tabIndex;
                        } else if (json_data.row[i].type == "dateTime_threeInput") {
                            // this part else-if is not is embraiz version
                            temp_element.xtype = json_data.row[i].type;
                            temp_element.fieldLabel = json_data.row[i].fieldLabel;
                            temp_element.field1Value = json_data.row[i].value;
                            temp_element.datasource = json_data.row[i].datasource;
                            temp_element.field1Name = json_data.row[i].name;
                            temp_element.tabIndex = json_data.row[i].tabIndex;

                            temp_element.field2Name = json_data.row[i].name2;
                            temp_element.field2Value = json_data.row[i].value2;

                            temp_element.field3Name = json_data.row[i].name3;
                            temp_element.field3Value = json_data.row[i].value3;

                        } else if (json_data.row[i].type == "input_range") {
                            // this part else-if is not is embraiz version
                            temp_element.xtype = json_data.row[i].type;
                            temp_element.customRegex = me.doubleRegex;

                            temp_element.fieldLabel = json_data.row[i].fieldLabel;
                            temp_element.field1Value = json_data.row[i].value;
                            temp_element.datasource = json_data.row[i].datasource;

                            temp_element.field1Name = json_data.row[i].name + "_lower";
                            temp_element.tabIndex = json_data.row[i].tabIndex;

                            temp_element.field2Name = json_data.row[i].name + "_upper";
                            temp_element.field2Value = json_data.row[i].value2;

                        } else if (json_data.row[i].type == "InputNoDuplicate") {
                            // this part else-if is not in embraiz version
                            temp_element.xtype = json_data.row[i].type;
                            temp_element.xtype = "textfield";
                            temp_element.fieldLabel = json_data.row[i].fieldLabel;

                            temp_element.name = json_data.row[i].name;
                            temp_element.value = json_data.row[i].value;
                            temp_element.orgValue = json_data.row[i].value;

                            temp_element.datasource = json_data.row[i].datasource;
                            temp_element.tabIndex = json_data.row[i].tabIndex;

                            temp_element.check_path = json_data.row[i].check_path;

                            temp_element.allowBlank = true;

                            // Support regex
                            //temp_element.regex = /^([1-9][0-9][0-9][0-9])$/;
                            //temp_element.regexText = 'Invalid Data';

                            temp_element.listeners = {};
                            temp_element.listeners.blur = function (e, newValue, oldValue) {
                                if (!e.hasActiveError() && e.orgValue != e.getValue()) {
                                    var fieldValue = e.getValue();

                                    Ext.Ajax.request({
                                        url: e.check_path,    // URL post
                                        success: function (o) {// function called on success
                                            var data_json = Ext.decode(o.responseText);
                                            var duplicate = data_json.duplicate;

                                            if (duplicate) {
                                                e.markInvalid(data_json.message);
                                            }
                                            else {
                                                e.clearInvalid();
                                            }
                                        },
                                        failure: function (o) {// function called on failure

                                        },
                                        params: { value: fieldValue }  // your json data
                                    });
                                }
                            };
                        }
                        else if (json_data.row[i].type == "select_input") {
                            // this part else-if is not in embraiz version
                            var value = json_data.row[i].value;

                            Ext.create('Ext.data.Store', {
                                fields: [{
                                    name: 'id',
                                    type: 'string'
                                }, {
                                    name: 'value',
                                    type: 'string'
                                }],
                                remoteSort: true,
                                autoLoad: false,
                                id: 'store_' + dt + '_' + json_data.row[i].name,
                                proxy: {
                                    type: 'ajax',
                                    url: json_data.row[i].datasource,
                                    reader: {
                                        type: 'json',
                                        root: 'data'
                                    },
                                    sorters: {
                                        property: 'value',
                                        direction: 'ASC'
                                    }

                                }
                            });


                            var temp_select = Ext.create('com.embraiz.component.ComboBox', {
                                fieldLabel: json_data.row[i].fieldLabel,
                                labelStyle: labelCss,
                                plugins: ['clearbutton'],
                                forceSelection: true,
                                sub_component: sub_component,
                                name: json_data.row[i].name,
                                displayField: 'value',
                                emptyText: 'Please Select',
                                valueField: 'id',
                                forceSelection: true,
                                store: 'store_' + dt + '_' + json_data.row[i].name,
                                init_value: value,
                                value: value,
                                allowBlank: allowBlank,
                                tabIndex: json_data.row[i].tabIndex,
                                init_load: 0,
                                queryMode: 'local',
                                hidden: json_data.row[i].hidden,
                                listeners: {
                                    render: function () {
                                        var temp = this;
                                        temp.store.load(function (records, operation, success) {
                                            temp.setValue(temp.init_value);
                                            temp.init_load == 1;
                                        });
                                    }
                                }
                            });

                            temp_element = Ext.create('Ext.container.Container', {
                                name: json_data.row[i].name + "_container",
                                layout: {
                                    type: 'hbox'
                                },
                                items: [temp_select, {
                                    labelWidth: 30,
                                    xtype: 'textfield',
                                    plugins: ['clearbutton'],
                                    allowBlank: allowBlank,
                                    name: json_data.row[i].input_name,
                                    margin: '0 5 3 0',
                                    value: json_data.row[i].input_value
                                }]
                            });
                        }
                        else if (json_data.row[i].type == "input_select") {
                            // this part else-if is not in embraiz version
                            var value = json_data.row[i].value;
                            Ext.create('Ext.data.Store', {
                                fields: [{
                                    name: 'id',
                                    type: 'string'
                                }, {
                                    name: 'value',
                                    type: 'string'
                                }],
                                remoteSort: true,
                                autoLoad: false,
                                id: 'store_' + dt + '_' + json_data.row[i].name,
                                proxy: {
                                    type: 'ajax',
                                    url: json_data.row[i].datasource,
                                    reader: {
                                        type: 'json',
                                        root: 'data'
                                    },
                                    sorters: {
                                        property: 'value',
                                        direction: 'ASC'
                                    }

                                }
                            });
                            var temp_select = Ext.create('com.embraiz.component.ComboBox', {
                                plugins: ['clearbutton'],
                                forceSelection: true,
                                sub_component: sub_component,
                                name: json_data.row[i].name,
                                displayField: 'value',
                                emptyText: 'Please Select',
                                valueField: 'id',
                                forceSelection: true,
                                store: 'store_' + dt + '_' + json_data.row[i].name,
                                init_value: value,
                                value: value,
                                allowBlank: allowBlank,
                                tabIndex: json_data.row[i].tabIndex,
                                init_load: 0,
                                queryMode: 'local',
                                hidden: json_data.row[i].hidden,
                                listeners: {
                                    render: function () {
                                        var temp = this;
                                        temp.store.load(function (records, operation, success) {
                                            temp.setValue(temp.init_value);
                                            temp.init_load == 1;
                                        });
                                    }
                                }
                            });
                            temp_element = Ext.create('Ext.container.Container', {
                                name: json_data.row[i].name + "_container",
                                fieldLabel: json_data.row[i].fieldLabel,
                                layout: {
                                    type: 'hbox'
                                },
                                items: [{
                                    fieldLabel: json_data.row[i].fieldLabel,
                                    labelStyle: labelCss,
                                    //abelWidth:30,
                                    xtype: 'textfield',
                                    plugins: ['clearbutton'],
                                    allowBlank: allowBlank,
                                    name: json_data.row[i].input_name,
                                    margin: '0 5 3 0',
                                    value: json_data.row[i].input_value
                                }, temp_select]
                            });
                        }
                    } //readOnly else
                    if (json_data.row[i].rowspan != null && json_data.row[i].rowspan != undefined) {
                        temp_element.rowspan = json_data.row[i].rowspan;
                    }
                    if (json_data.row[i].colspan != null && json_data.row[i].colspan != undefined) {
                        temp_element.colspan = json_data.row[i].colspan;
                    }
                    form_content[i] = temp_element;
                    var temp_element_hidden = {};
                    //////////
                    if (json_data.isType == true) {
                        if (json_data.row[i].group != undefined) {
                            groupItem = Ext.create('Ext.form.FieldSet', {
                                collapsible: true,
                                title: json_data.row[i].group,
                                collapsed: false,
                                anchor: '100%',
                                layout: {
                                    type: 'table',
                                    columns: 2,
                                    tableAttrs: {
                                        style: {
                                            width: '99%',
                                            margin: '0px 0px 0px 0px'
                                        }
                                    },
                                    tdAttrs: {
                                        style: {
                                            width: '50%'
                                        }
                                    }

                                }
                            });
                            form_group[groupIndex] = groupItem;
                            groupIndex++;
                        }

                        if (groupItem != undefined) {
                            groupItem.add(temp_element);
                        } else {
                            if (groupIndex == 0) {
                                form_group[groupIndex] = temp;
                                groupIndex++;
                            }
                            temp.add(temp_element);
                        }
                        if (json_data.row[i].type == 'title') {
                            if (json_data.row[i].titleLabel != undefined && json_data.row[i].titleLabel != '' && json_data.row[i].titleLabel != null) {
                                var fontSize = json_data.row[i].titleFontSize;//==undefined?'12px':json_data.row[i].titleFontSize;
                                var marginLeft = json_data.row[i].marginLeft == undefined ? '360' : json_data.row[i].marginLeft;
                                form_group[groupIndex] = {
                                    xtype: 'displayfield',
                                    style: 'text-align:center',
                                    width: 1024,
                                    value: '<font size=' + fontSize + '>' + json_data.row[i].titleLabel + '</font>'
                                };
                                groupIndex = groupIndex + 1;
                            }
                        }

                    }
                }
            }
            var temp_element_change = {};
            temp_element_change.xtype = 'hidden';
            temp_element_change.name = 'change_fields';
            temp_element_change.value = '';


            form_content[form_content.length] = temp_element_change;

            ///
            if (json_data.isType == true) {
                for (var n = 1; n < form_group.length + 1; n++) {
                    if (form_group[form_group.length - n].add != undefined) {
                        form_group[form_group.length - n].add(temp_element_change);
                        break;
                    }

                }
            }
            //
            if (json_data.rowhidden != undefined) {
                for (i = 0; i < json_data.rowhidden.length; i++) {
                    var temp_element = {};
                    temp_element.xtype = 'hidden';
                    temp_element.name = json_data.rowhidden[i].name;
                    temp_element.value = json_data.rowhidden[i].value;
                    if (json_data.isType == true) {
                        if (json_data.isType == true) {
                            for (var m = 1; m < form_group.length + 1; m++) {
                                if (form_group[form_group.length - m].add != undefined) {
                                    form_group[form_group.length - m].add(temp_element);
                                    break;
                                }

                            }
                        }
                    } else {
                        //form_content[form_content.length] = form_content;
                        form_content[form_content.length] = temp_element;
                    }
                }

            }
            if (json_data.isType != true) {
                form_items = Ext.create('Ext.container.Container', {
                    anchor: '100%',
                    layout: {
                        type: 'table',
                        columns: 2,
                        tableAttrs: {
                            style: {
                                width: '100%',
                                margin: '0px 5px 5px 0px'

                            }
                        },
                        tdAttrs: {
                            style: {
                                width: '50%'
                            }
                        }

                    },
                    items: form_content
                });
            } else {
                form_items = form_group;
            }

            var form_buttons;
            if (json_data.button_text == null)
                form_buttons = null;
            else {
                form_buttons = [Ext.create('Ext.Button', {
                    text: json_data.button_text + '',
                    iconCls: json_data.button_icon,
                    hidden: json_data.savebtn_hidden,
                    style: {
                        float: 'left'
                    },
                   handler:function () {

                        var save_button = this;

                        if (json_data.confirmSave == true) {
                            Ext.MessageBox.confirm('Confirm', 'Are you sure you want to save?',
                               function (btn) {
                                   if (btn == 'yes') {
                                       save_button.performSave();
                                   }
                               }
                            );
                        }
                        else {
                            save_button.performSave();
                        }
                    },
                    performSave: function () {

                        
                        // no need disable save button
                        //this.setDisabled(true);

                        var save_button = this;
                        Ext.getBody().mask("wait");
                        var form = save_button.up('form').getForm();
                        ///start chang log
                        var change_field_str = "";
                        var form_field = form.getFields().items;
                        for (var i = 0; i < form_field.length; i++) {
                            var current_field = form_field[i];
                            if (current_field.xtype != 'hidden') {
                                var oldValue = '';
                                oldValue = current_field.originalValue == undefined ? '' : current_field.originalValue;
                                //var newValue = current_field.getValue();
                                var newValue = (current_field.getValue() == null) ? '' : current_field.getValue();
                                if (current_field.xtype == 'datefield') {
                                    if (newValue != '' && newValue != undefined) {
                                        newValue = Ext.Date.format(newValue, 'Y-m-d');
                                    } else {
                                        newValue = '';
                                    }
                                    if (oldValue == null || oldValue == 'null') {
                                        oldValue = '';
                                    }
                                    if (oldValue != undefined && oldValue != null && oldValue != '') {
                                        oldValue = Ext.Date.format(oldValue, 'Y-m-d');
                                    }
                                    if (oldValue != newValue) {
                                        change_field_str = change_field_str + "{'field_name':\"" + current_field.fieldLabel + "\",'oldValue':\"" + oldValue + "\",'newValue':\"" + newValue + "\"},";
                                    }
                                } else if (current_field.xtype == 'combofieldbox' || current_field.xtype == 'combobox' || current_field.xtype == 'boxselect' || current_field.xtype == 'comboboxselect') {
                                    if (typeof (oldValue) == 'object') { //
                                        var oldDisplayValue = "";
                                        var isequal = false;

                                        for (var a = 0; a < oldValue.length; a++) {
                                            var flag = false;
                                            for (var b = 0; b < newValue.length; b++) {
                                                if (oldValue[a] == newValue[b]) {
                                                    flag = true;
                                                    break;
                                                }
                                            }
                                            isequal = flag;
                                        }
                                        if (newValue.length == 0 && oldValue.length == 0) {
                                            isequal = true;
                                        }
                                        if (!isequal) {
                                            for (var s = 0; s < current_field.store.data.items.length; s++) {
                                                for (var a = 0; a < oldValue.length; a++) {
                                                    if (current_field.store.data.keys[s] == oldValue[a]) {
                                                        oldDisplayValue = oldDisplayValue + current_field.store.data.items[s].data.value + ",";
                                                    }
                                                }
                                            }
                                            change_field_str = change_field_str + "{'field_name':\"" + current_field.fieldLabel + "\",'oldValue':\"" + oldDisplayValue + "\",'newValue':\"" + current_field.getRawValue() + "\"},";

                                        }
                                    } else {
                                        if (oldValue != newValue) {
                                            for (var s = 0; s < current_field.store.data.items.length; s++) {
                                                if (current_field.store.data.keys[s] == oldValue) {
                                                    var textValue = current_field.store.data.items[s].data.value;
                                                    change_field_str = change_field_str + "{'field_name':\"" + current_field.fieldLabel + "\",'oldValue':\"" + textValue + "\",'newValue':\"" + current_field.getRawValue() + "\"},";
                                                }
                                            }
                                        }
                                    }
                                } else if (current_field.xtype == 'checkboxgroup') {
                                    var checkgroup = current_field.items.items;
                                    for (var c = 0; c < checkgroup.length; c++) {
                                        var oldCheck = checkgroup[c].originalValue;
                                        if (oldCheck != checkgroup[c].checked) {
                                            oldValue = checkgroup[c].originalValue ? "Yes" : "No";
                                            newValue = checkgroup[c].checked ? "Yes" : "No";
                                            change_field_str = change_field_str + "{'field_name':\"" + (current_field.fieldLabel != undefined ? current_field.fieldLabel : current_field.boxLabel) + "\",'oldValue':\"" + oldValue + "\",'newValue':\"" + newValue + "\"},";
                                        }
                                    }

                                } else if (current_field.xtype == 'checkbox') {
                                    var oldCheck = current_field.originalValue;
                                    if (oldCheck != current_field.getValue()) {
                                        newValue = current_field.checked ? "Yes" : "No";
                                        change_field_str = change_field_str + "{'field_name':\"" + (current_field.fieldLabel != undefined ? current_field.fieldLabel : current_field.boxLabel) + "\",'oldValue':\"" + oldValue + "\",'newValue':\"" + newValue + "\"},";
                                    }
                                } else if (current_field.xtype == 'timefield') {
                                    var newValue = current_field.getValue();
                                    if (oldValue != newValue) {
                                        change_field_str = change_field_str + "{'field_name':\"" + (current_field.fieldLabel != undefined ? current_field.fieldLabel : current_field.boxLabel) + "\",'oldValue':\"" + oldValue + "\",'newValue':\"" + newValue + "\"},";
                                    }
                                } else if (current_field.xtype != 'checkboxfield') {
                                    if (typeof oldValue != 'boolean') {
                                        oldValue = oldValue.replace(/[\r\n]/g, "");
                                    } else {
                                        oldValue = oldValue ? 'Yes' : 'No';
                                    }
                                    if (typeof newValue != 'boolean') {
                                        newValue = newValue.replace(/[\r\n]/g, "");
                                    } else {
                                        newValue = newValue ? 'Yes' : 'No';
                                    }
                                    if (oldValue != newValue) {
                                        change_field_str = change_field_str + "{'field_name':\"" + (current_field.fieldLabel != undefined ? current_field.fieldLabel : current_field.boxLabel) + "\",'oldValue':\"" + oldValue + "\",'newValue':\"" + newValue + "\"},";
                                    }
                                }
                            }
                        }
                        if (change_field_str != '') {
                            var change_hidden = form.findField('change_fields');
                            change_hidden.setRawValue(change_field_str);
                        }
                        if (form.isValid()) {

                            form.submit({

                                submitEmptyText: false,
                                success: function (form, action) {
                                    save_button.setDisabled(false);
                                    Ext.getBody().unmask();
                                    if (grid != null) {
                                        grid.search(Ext.decode(action.response.responseText));
                                    } else {
                                        var data = Ext.decode(action.response.responseText);

                                        if (data.url != '' && data.url != undefined) {//insert
                                            //                                        form.reset();
                                         
                                            if (data.icon == undefined && data.url.indexOf('.jsp') != -1) {
                                                Ext.Ajax.request({
                                                    url: data.url,
                                                    async: false,
                                                    success: function (o) {
                                                        if (is_pop) {
                                                            target_div.close();
                                                            if (tabUrl != null && tabUrl != undefined && tabUrl != '') {
                                                                new com.embraiz.tag().tabRefrash(tabUrl, itemId, itemId1);
                                                            }
                                                        } else {

                                                            if (me.pre_tab != undefined && me.index_url != undefined) {
                                                                Ext.getCmp('content-panel').getActiveTab().close();
                                                                new com.embraiz.tag().previous_tab_refresh(me.pre_tab, me.index_url, me.index_id, me.index_itemId);
                                                            } else {
                                                                var dataJson = Ext.decode(o.responseText);
                                                                //  Ext.getCmp('content-panel').getActiveTab().close();
                                                                //  new com.embraiz.tag().openNewTag(dataJson.id, dataJson.title, dataJson.url, dataJson.icon, dataJson.icon, dataJson.icon,data.itemId);

                                                                Ext.MessageBox.confirm('Confirm', 'Done! Would you like to close this tab?',
                                                                function (btn) {
                                                                    if (btn == 'yes') {
                                                                        Ext.getCmp('content-panel').getActiveTab().close();
                                                                        var currentTab = Ext.getCmp('content-panel').getActiveTab();

                                                                        currentTab.close();
                                                                        setTimeout(function a() {

                                                                            new com.embraiz.tag().openNewTag(currentTab.idValue, currentTab.title, currentTab.urlLink, currentTab.iconCls, '', '', currentTab.par, currentTab.extra);
                                                                        }, 200);

                                                                    } else {
                                                                        Ext.getCmp('content-panel').getActiveTab().close();
                                                                        new com.embraiz.tag().openNewTag(dataJson.id, dataJson.title, dataJson.url, dataJson.icon, dataJson.icon, dataJson.icon, data.itemId);
                                                                    }
                                                                });
                                                            }
                                                        }
                                                    }
                                                });
                                            } else {

                                                if (is_pop) {

                                                    if (data.ispass == false) {
                                                        Ext.Msg.show({
                                                            title: 'Warning',
                                                            msg: data.msg || '',
                                                            width: 200,
                                                            buttons: Ext.Msg.OK,
                                                            icon: Ext.MessageBox.INFO
                                                        });
                                                    } else {
                                                        target_div.close();
                                                        if (tabUrl != null && tabUrl != undefined && tabUrl != '') {
                                                            new com.embraiz.tag().tabRefrash(tabUrl, itemId, itemId1);
                                                        }
                                                    }
                                                } else {

                                                    if (me.pre_tab != undefined && me.index_url != undefined) {


                                                        Ext.getCmp('content-panel').getActiveTab().close();
                                                        new com.embraiz.tag().previous_tab_refresh(me.pre_tab, me.index_url, me.index_id, me.index_itemId);
                                                    } else {

                                                        var dataJson = data;
                                                        //  Ext.getCmp('content-panel').getActiveTab().close();
                                                        //   new com.embraiz.tag().openNewTag(dataJson.id, dataJson.title, dataJson.url, dataJson.icon, dataJson.icon, dataJson.icon,dataJson.module_name);
                                                        Ext.MessageBox.confirm('Confirm', 'Done! Would you like to close this tab?',
                                                                function (btn) {
                                                                    if (btn == 'yes') {

                                                                        // need some waiting time before close tab
                                                                        // otherwise occur Uncaught TypeError: Cannot read property 'scrollTop' of undefined 
                                                                        // (maybe problem with MessageBox)
                                                                        setTimeout(function a() {
                                                                            Ext.getCmp('content-panel').getActiveTab().close();

                                                                            var currentTab = Ext.getCmp('content-panel').getActiveTab();

                                                                            currentTab.close();
                                                                            new com.embraiz.tag().openNewTag(currentTab.idValue, currentTab.title, currentTab.urlLink, currentTab.iconCls, '', '', currentTab.par, currentTab.extra);

                                                                        }, 200);

                                                                    } /*else {
                                                                    Ext.getCmp('content-panel').getActiveTab().close();
                                                                    new com.embraiz.tag().openNewTag(dataJson.id, dataJson.title, dataJson.url, dataJson.icon, dataJson.icon, dataJson.icon, dataJson.module_name);
                                                                }*/
                                                                });
                                                    }
                                                }

                                                if (data.msg != undefined && data.msg != '') {
                                                    Ext.Msg.show({
                                                        title: 'Warning',
                                                        msg: data.msg || '',
                                                        width: 200,
                                                        buttons: Ext.Msg.OK,
                                                        icon: Ext.MessageBox.INFO
                                                    });
                                                }
                                            }
                                        } else { //insert, data.url is undefine
                                            
                                            if (is_pop) {

                                                if (action.result.ispass == false) {
                                                    Ext.Msg.show({
                                                        title: 'Warning',
                                                        msg: action.result.msg || '',
                                                        width: 200,
                                                        buttons: Ext.Msg.OK,
                                                        icon: Ext.MessageBox.INFO
                                                    });
                                                } else {

                                                    Ext.Msg.show({
                                                        title: 'Success',
                                                        msg: data.msg || '',
                                                        width: 200,
                                                        buttons: Ext.Msg.OK,
                                                        icon: Ext.MessageBox.INFO,
                                                        fn: function (btn) {
                                                            if (btn == 'ok') {
                                                                target_div.close();
                                                                if (tabUrl != null && tabUrl != undefined && tabUrl != '') {
                                                                    if (tabUrl.indexOf('refresh_add_tab') != -1) {
                                                                        Ext.decode(tabUrl + "('" + data.id + "')");
                                                                    } else {
                                                                        new com.embraiz.tag().tabRefrash(tabUrl, itemId, itemId1);
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    });

                                                }
                                            } else {

                                                if (tabUrl != null && tabUrl != undefined && tabUrl != '') {  //是否刷新当前tab                                            
                                                    //new com.embraiz.tag().tabRefrash(tabUrl, itemId, itemId1);     
                                                    var currentTab = Ext.getCmp('content-panel').getActiveTab();
                                                    setTimeout(function a() {
                                                        new com.embraiz.tag().openNewTag(currentTab.idValue, currentTab.title, currentTab.urlLink, currentTab.iconCls, '', '', currentTab.par, currentTab.extra);
                                                    }, 200);
                                                } else {
                                                    var closeAndRefreshConfirmBox = function () { //弹出提示是否刷新list tab
                                                  
                                                        Ext.MessageBox.confirm('Confirm', 'Done! Would you like to close this tab?',
                                                            function (btn) {
                                                                if (btn == 'yes') {
                                                                    
                                                                    setTimeout(function a() {
                                                                        Ext.getCmp('content-panel').getActiveTab().close();
                                                                        var currentTab = Ext.getCmp('content-panel').getActiveTab();
                                                                        currentTab.close();
                                                                       
                                                                        new com.embraiz.tag().openNewTag(currentTab.idValue, currentTab.title, currentTab.urlLink, currentTab.iconCls, '', '', currentTab.par, currentTab.extra);
                                                                    }, 200);
                                                                }
                                                            }
                                                        );
                                                    };

                                                    
                                                    if (data.msg != undefined && data.msg != '') { // has custom message
                                                        Ext.Msg.show({
                                                            title: 'Success',
                                                            msg: data.msg || '',
                                                            width: 200,
                                                            buttons: Ext.Msg.OK,
                                                            icon: Ext.MessageBox.INFO,
                                                            fn: function (button) {
                                                                closeAndRefreshConfirmBox();
                                                            }
                                                        });
                                                    } else {
                                                        closeAndRefreshConfirmBox();
                                                    }
                                                }

                                                if (data.ispass != false) {
                                                    if (me.tabUrl != null && me.tabUrl != undefined && me.tabUrl != '') {
                                                        new com.embraiz.tag().tabRefrash(me.tabUrl, me.itemId, me.itemId1);
                                                    }
                                                    if (me.pre_tab != undefined && me.index_url != undefined) {
                                                        Ext.getCmp('content-panel').getActiveTab().close();
                                                        new com.embraiz.tag().previous_tab_refresh(me.pre_tab, me.index_url, me.index_id, me.index_itemId);
                                                    }
                                                }

                                               
                                            }
                                        }
                                    }


                                },
                                failure: function (f, action) {
                                    Ext.getBody().unmask();

                                    if (action.result != undefined && action.result.success == false) {

                                        Ext.Msg.show({
                                            title: 'Failure',
                                            msg: action.result.msg || '',
                                            width: 200,
                                            buttons: Ext.Msg.OK,
                                            icon: Ext.MessageBox.INFO,
                                            fn: function (btn) {
                                                if (btn == 'ok') {

                                                    if (action.result.closeWhenFail)
                                                        target_div.close();

                                                    if (tabUrl != null && tabUrl != undefined && tabUrl != '') {
                                                        if (tabUrl.indexOf('refresh_add_tab') != -1) {
                                                            Ext.decode(tabUrl + "('" + data.id + "')");
                                                        } else {
                                                            new com.embraiz.tag().tabRefrash(tabUrl, itemId, itemId1);
                                                        }
                                                    }
                                                }
                                            }
                                        });
                                    } else {

                                        var form = Ext.create('Ext.form.Panel', {
                                            layout: 'absolute',
                                            url: 'modules/systemError/insert_data.jsp',
                                            defaultType: 'textfield',
                                            border: false,
                                            items: [{
                                                fieldLabel: 'Would you like to send this error to server',
                                                labelAlign: 'top',
                                                xtype: 'textarea',
                                                style: 'margin:5',
                                                name: 'msg',
                                                value: action.response.responseText,
                                                anchor: '-5 -5 -5'

                                            }]
                                        });
                                        var win = Ext.create('Ext.window.Window', {
                                            title: 'System error',
                                            width: 500,
                                            modal: true,
                                            height: 300,
                                            layout: 'fit',
                                            items: form,
                                            buttons: [{
                                                text: 'Send',
                                                handler: function (button) {
                                                    var form1 = form.getForm();
                                                    form1.submit({
                                                        success: function (form, action) {
                                                            button.up('window').close();
                                                        }
                                                    });
                                                }
                                            }, {
                                                text: 'Cancel',
                                                handler: function (button) {
                                                    button.up('window').close();
                                                }
                                            }]
                                        });
                                        win.show();
                                    }
                                }
                            });
                        } else {
                            save_button.setDisabled(false);
                            Ext.getBody().unmask();
                            var error_msg = "";
                            form.getFields().each(function (item) {
                                if (item.getActiveError()) {
                                    
                                    if (item.hideErrorMessage == undefined || item.hideErrorMessage != true) {
                                        if (item.getActiveError().indexOf('This field is required') > -1)
                                            error_msg += item.fieldLabel + " is required" + '<br>';
                                        else
                                            error_msg = item.fieldLabel + ": " + item.getActiveError() + '<br>';
                                    }

                                }
                            });
                         
                            Ext.Msg.show({
                                title: 'Failure',
                                msg: error_msg,
                                width: 200,
                                buttons: Ext.Msg.OK,
                                icon: Ext.MessageBox.INFO,
                                fn: function (btn) {
                                }
                            });
                        }
                            
                    }// end button performSave() function

                }), {
                    text: json_data.button_cancel_text == null || json_data.button_cancel_text == undefined ? 'Clear All' : json_data.button_cancel_text,
                    hidden: json_data.cancelbtn_hidden == null || json_data.cancelbtn_hidden == undefined ? false : json_data.cancelbtn_hidden,
                    handler: function () {
                        var form = this.up('form').getForm();//.reset();
                        form.reset();
                        var form_field = form.getFields().items;
                        for (ff = 0; ff < form_field.length; ff++) {
                            var current_field = form_field[ff];
                            if (current_field.readOnly != true && current_field.xtype != 'hidden') {
                                if (current_field.xtype == 'timefield' || current_field.xtype == 'combofieldbox'
                                    || current_field.xtype == 'combobox' || current_field.xtype == 'boxselect'
                                    || current_field.xtype == 'comboboxselect' || current_field.xtype == 'upload') {
                                    form_field[ff].clearValue();
                                } else if (current_field.xtype == 'datefield' || current_field.xtype == 'textareafield' || current_field.xtype == 'textfield' || current_field.xtype == 'htmleditor')
                                    form_field[ff].setValue('');
                                else
                                    form_field[ff].setValue('');
                            }
                        }
                    }
                }];
            } // [end] form buttons


            var editSimple = this.editSimple = Ext.create('Ext.form.Panel', {
                url: json_data.post_params,
                title: json_data.title,
                iconCls: json_data.icon,
                method: 'POST',
                bodyStyle: 'padding:5px 5px 5px',
                margin: '2 2 2 2',
                width: '100%',
                fieldDefaults: {
                    msgTarget: 'side',
                    labelWidth: 150
                },
                defaultType: 'textfield',
                defaults: {
                    anchor: '100%'

                },
                listeners: {
                    afterrender: form_layout,
                    beforeaction: function (thiz, action, op) {
                        var values = thiz.getValues();
                        for (i in values) {
                            if (values[i] == 'Please Select') {
                                if (thiz.findField(i).xtype == 'combobox' || thiz.findField(i).xtype == 'combofieldbox') {
                                    values[i] = '';
                                }
                            }
                            if (thiz.findField(i).ownerCt.xtype == 'currencyFormat') {
                                if (isNaN(Number(values[i]))) {
                                    var newvalues = values[i].substring(1, values[i].length);
                                    var valuesList = newvalues.split(',');
                                    var valueInt = "";
                                    for (j = 0; j < valuesList.length; j++) {
                                        valueInt += valuesList[j];
                                    }
                                    valueIndex = i;
                                    values[i] = valueInt;//thiz.findField(i).value=valueInt;
                                }
                            }
                        }
                        thiz.setValues(values);
                        for (var i = 0; i < thiz.getFields().items.length; i++) {
                            var field = thiz.getFields().items[i];
                            if (field.xtype != 'combobox' && field.xtype != 'combofieldbox') {
                                continue;
                            }
                            if (field.value == null) {
                                field.value = '';
                            }
                        }
                    }, beforeRender: function (thiz, op) {
                        if (json_data.render) {
                            eval(json_data.render);
                        }
                    }
                },

                cls: 'editform',
                items: form_items,
                buttonAlign: 'left',
                buttons:
                    form_buttons

            });

            if (json_data.myConfig) {
                var idbtn = Ext.create('Ext.button.Button', {
                    text: 'Config',
                    name: 'config',
                    //iconCls:'iconConfig',
                    handler: function () {
                        new com.embraiz.common.js.identify().initTag(json_data.myConfig, 'search');
                    }
                })
                editSimple.dockedItems.items[0].add(idbtn);
            }
            if (json_data.copyConfig) {
                var idbtn = Ext.create('Ext.button.Button', {
                    text: 'Copy Config',
                    name: 'copy',
                    //iconCls:'iconCopy',
                    handler: function () {
                        new com.embraiz.common.js.identify().copy(json_data.myConfig);
                    }
                })
                editSimple.dockedItems.items[0].add(idbtn);
            }
            if (is_pop) {
                target_div.add(editSimple);
            } else {
                editSimple.render(target_div);
            }
            //start   search Enter key
            if (grid != null) {
                var form = editSimple.getForm();
                var formFields = form.getFields().items
                Ext.each(formFields, function (o, j) {
                    if (formFields[j].xtype != 'combobox' && formFields[j].xtype != 'boxselect' && formFields[j].xtype != 'comboboxselect' && formFields[j].xtype != 'hidden') {
                        var config = [{
                            key: 13,
                            fn: function (key, e) {
                                if (form.isValid()) {
                                    form.submit({
                                        submitEmptyText: false,
                                        success: function (form, action) {
                                            grid.search(Ext.decode(action.response.responseText));
                                        }
                                    });
                                }
                            },
                            scope: form,
                            stopEvent: false
                        }];
                        new Ext.util.KeyMap(formFields[j].id, config);
                    }
                });  //search Enter key

                Ext.each(editSimple.items, function (o, i) {
                    var j = 0;
                    if (editSimple.items.get(i).getXType() == 'fieldset') {
                        if (j == 0) {
                            editSimple.items.get(i).expand();
                        } else {
                            editSimple.items.get(i).collapse();
                        }
                        j++;
                    }
                });
            }
            if (temp.items.length == 1) {// 处理items==1独占一行问题
                temp.anchor = '50%';
            }
            ///end



        }
        //        Ext.getBody().unmask();

        //---restore async property to the default value
        Ext.apply(Ext.data.Connection.prototype, {
            async: true
        });
    },  //end editForm

    ///////////////??????????????
    validationEngine: function (comp, e) {
        if (comp.getValue() == "" || comp.getValue() == null) {
            var tip = Ext.create('Ext.tip.ToolTip', {
                target: comp,
                html: 'Can not be empty, please fill out the content'
            });
        }
    },

    popSearch: function (id, json_data) {
        Ext.Ajax.request({
            url: json_data.post_header,
            success: showGrid,
            scope: this
        });

        function showGrid(o) {
            var gird_info = Ext.decode(o.responseText);

            var dt = new Date();
            search_window = Ext.create('widget.window', {
                title: 'Layout Window',
                closable: true,
                closeAction: 'hide',
                modal: true,
                width: 600,
                height: 400,
                layout: 'fit',
                bodyStyle: 'padding: 5px;',
                html: '<div id="gridContentId_' + dt + '" style="height:370px;overflow-y:scroll;"></div>'
            });

            search_window.show();
            this.grid = Ext.create('com.embraiz.component.gird', {
                grider_div: 'gridContentId_' + dt,
                json_data: gird_info,
                grid_url: json_data.post_url + "?form_id=" + id
            });
            if (json_data.post_url.indexOf("?form_id") != -1) {
                json_data.post_url = json_data.post_url.substring(0, json_data.post_url.indexOf("?form_id"));
            }
            //json_data.post_url = json_data.post_url + "?form_id=" + id;
            //this.grid.search(json_data);

        }
    },
    putValueToInputField: function (form_id, name, value, valueHidden) {
        search_window.close();
        var form = Ext.ComponentManager.get(form_id).getForm();

        var fieldName = name.split(',');
        var fieldValue = value.split(',');
        var fieldValueHidden = valueHidden.split(',');
        for (var mm = 0; mm < fieldName.length; mm++) {
            var field = form.findField(fieldName[mm]);
            field.setRawValue(fieldValue[mm]);
            if (form.findField(fieldName[mm] + "_hidden") != null && form.findField(fieldName[mm] + "_hidden") != undefined) {
                var fieldHidden = form.findField(fieldName[mm] + "_hidden");
                fieldHidden.setRawValue(fieldValueHidden[mm]);
            }
        }
    },

    uploadImage: function (formId, e, fileName, img, label, uploadUrl, names, upType) { //????????????????????
        var fileText = Ext.ComponentManager.get(formId).getForm().findField(fileName + '_hidden');
        var fileTextdis = Ext.ComponentManager.get(formId).getForm().findField(fileName + "_img");
        var container = e.up('container');
        var me = this;
        var uploadPanel = Ext.create('Ext.form.Panel', {
            width: 500,
            standardSubmit: false,
            frame: true,
            defaults: {
                anchor: '100%',
                allowBlank: false,
                msgTarget: 'side',
                labelWidth: 50
            },

            items: [{
                xtype: 'filefield',
                // id: 'form-file',
                emptyText: 'Select an File',
                fieldLabel: 'File',
                name: names
            }],

            buttons: [{
                text: 'Upload',
                handler: function () {
                    var form = this.up('form').getForm();
                    if (form.isValid()) {
                        form.submit({
                            url: uploadUrl,
                            waitMsg: 'Uploading...',
                            success: function (form, action) {
                                Ext.Msg.alert('Success', 'Processed file "' + action.result.file + '" on the server');
                                fileText.setValue(action.result.file);
                                if (upType == 'file') {
                                    fileTextdis.setValue(action.result.file);
                                } else {
                                    img.setSrc(action.result.file);
                                }

                                winUpload.close();
                            }
                        });
                    }
                }
            }]
        });

        var winUpload = Ext.create('widget.window', {
            title: 'File Upload',
            closable: true,
            closeAction: 'hide',
            modal: true,
            width: 500,
            height: 120,
            layout: 'fit',
            bodyStyle: 'padding: 5px;',
            items: uploadPanel

        });
        winUpload.show();

    },
    ////////// toolBar
    showToolBar: function (target_div, json_data_url, tool_div) {
        var me = this;
        var viewBtn = undefined;
        var editBtn = undefined;
        var saveBtn = undefined;
        var summryBtn = undefined;
        var fullBtn = undefined;
        var modifyBtn = undefined;
        var refresh = undefined;
        var hidden = false;
        var toolbar = undefined;
        this.toolbar=toolbar = Ext.create('Ext.toolbar.Toolbar', {
            //id: tool_div,           
            x: 0,
            y: 0,
            //renderTo: Ext.getCmp('content-panel').getActiveTab().getEl(),
            floating: true,
            width: '100%',
            height:36,
            margin: '0 0 0 0',
            cls: 'component_toolbar',
            listeners:{ 
        		beforerender:function(thiz){
        			Ext.getCmp('content-panel').getActiveTab().layout.outerCt.dom.style.height=35;
		        	if(me.parentType&&me.parentId){
			        	var btnpar=Ext.create('Ext.Button', {
				    	    text: 'Parent',
				    	    iconCls:'iconParent',
				    	    handler: function() {
				    		new com.embraiz.tag().openNewTag(me.parentType+':'+me.parentId,me.parentType+":"+me.parentId,'com.embraiz.common.js.edit','icon'+me.parentType,'icon'+me.parentType,'icon'+me.parentType,me.parentType)
				    		}
				    	});
		    		}
		        	if(me.approvalProcess!=undefined&&me.approvalProcess!=''){
		        		return new com.embraiz.approvalProcess.js.index().initTag(thiz,me.id,me.approvalProcess,me.url,me.appGrid);
		        	}
        		}
        	}
        });

        //}
        Ext.Ajax.request({
            url: json_data_url,
            async: false,
            success: function (o) {
                var json_data = Ext.decode(o.responseText);
                var fn = new Array(72, 74, 75, 76);
                for (i = 0; i < json_data.toolData.length; i++) {
                    if (json_data.toolData[i].hidden != undefined) {
                        hidden = json_data.toolData[i].hidden;
                    } else {
                        hidden = false;
                    }
                    if (json_data.toolData[i].name == 'view') {
                        var name = '';
                        var module = json_data.toolData[i].module;
                        if (module == null || module == '' || module == undefined) {
                            name = json_data.toolData[i].name;
                        } else {
                            name = module + ':' + json_data.toolData[i].name;
                        }

                        viewBtn = Ext.create('Ext.button.Button', {
                            text: json_data.toolData[i].text,
                            scope: this,
                            hidden: hidden,
                            iconCls: json_data.toolData[i].iconUrl,
                            name: name,
                            handler: function (b, e) {
                                me.viewForm(me.target_div, me.json_data_url, me.pre_tab, me.index_url, me.index_id, me.index_itemId);
                                viewBtn.hide();
                                if (editBtn != undefined) {
                                    editBtn.show();
                                }
                                if (saveBtn != undefined) {
                                    saveBtn.hide();
                                }
                                if (summryBtn != undefined) {
                                    summryBtn.show();
                                }
                            }
                        });
                        //toolbar.removeAll();
                        toolbar.add(viewBtn);
                        new Ext.util.KeyMap(Ext.getCmp('content-panel').getActiveTab().id, {
                            key: 82,
                            ctrl: true,
                            alt: true,
                            handler: function (b, e) {
                                me.viewForm(me.target_div, me.json_data_url);
                                viewBtn.hide();
                                if (editBtn != undefined) {
                                    editBtn.show();
                                }
                            }
                        });
                    } else if (json_data.toolData[i].name == 'edit') {

                        var name = '';
                        var module = json_data.toolData[i].module;
                        if (module == null || module == '' || module == undefined) {
                            name = json_data.toolData[i].name;
                        } else {
                            name = module + ':' + json_data.toolData[i].name;
                        }
                        var tabUrl = json_data.toolData[i].tabUrl;
                        var tab_id = json_data.toolData[i].tab_id;
                        var tab_itemId = json_data.toolData[i].tab_itemId;
                        editBtn = Ext.create('Ext.button.Button', {
                            text: json_data.toolData[i].text,
                            scope: this,
                            iconCls: json_data.toolData[i].iconUrl,
                            hidden: hidden,
                            name: name,
                            handler: function (b, e) {
                                //tabUrl  tab_id   tab_itemId
                                if (tabUrl != undefined) {
                                    me.editForm(me.target_div, me.json_data, null, tabUrl, tab_id, tab_itemId);
                                } else {
                                    me.editForm(me.target_div, me.json_data);
                                }
                                var forms = Ext.getCmp(me.target_div.lastChild.id);
                                var fieldSets = forms.items;
                                Ext.each(fieldSets, function (o, i) {
                                    if (fieldSets.get(i).getXType() == 'fieldset') fieldSets.get(i).expand();
                                });
                                editBtn.hide();
                                if (viewBtn != undefined) {
                                    viewBtn.show();
                                }
                                if (saveBtn != undefined) {
                                    saveBtn.show();
                                }
                                if (fullBtn != undefined) {
                                    fullBtn.show();
                                }
                            }
                        });
                        toolbar.add(editBtn);
                        new Ext.util.KeyMap(Ext.getCmp('content-panel').getActiveTab().id, {
                            key: 89,
                            ctrl: true,
                            alt: true,
                            handler: function (b, e) {
                                me.editForm(me.target_div, me.json_data);
                                editBtn.hide();
                                if (viewBtn != undefined) {
                                    viewBtn.show();
                                }
                            }
                        });
                    } else if (json_data.toolData[i].name == 'fullview') {
                        fullBtn = Ext.create('Ext.button.Button', {
                            text: json_data.toolData[i].text,
                            scope: this,
                            iconCls: json_data.toolData[i].iconUrl,
                            hidden: hidden,
                            name: json_data.toolData[i].name,
                            handler: function (b, e) {
                                var curtag = Ext.getCmp("content-panel").getActiveTab();
                                var curid = curtag.id;
                                Ext.Ajax.request({
                                    url: 'modules/history/getEntry.jsp?caseId=' + curid + '',
                                    success: function (o) {
                                        var DataJson = Ext.decode(o.responseText);
                                        new com.embraiz.tag().tabRefrash('com.embraiz.entry2.js.edit', curid, DataJson.id);
                                    }
                                });
                            }
                        });
                        toolbar.add(fullBtn);
                    } else if (json_data.toolData[i].name == 'summaryview') {
                        summryBtn = Ext.create('Ext.button.Button', {
                            text: json_data.toolData[i].text,
                            scope: this,
                            iconCls: json_data.toolData[i].iconUrl,
                            hidden: hidden,
                            name: json_data.toolData[i].name,
                            handler: function (b, e) {
                                var curtag = Ext.getCmp("content-panel").getActiveTab();
                                var curid = curtag.id;
                                Ext.Ajax.request({
                                    url: 'modules/history/getEntry.jsp?caseId=' + curid + '',
                                    success: function (o) {
                                        var DataJson = Ext.decode(o.responseText);
                                        //new com.embraiz.tag().openNewTag(curtag.id, curtag.title, 'com.embraiz.entry.js.edit', 'iconEntry','iconEntry','iconEntry',DataJson.id);
                                        new com.embraiz.tag().tabRefrash('com.embraiz.entry.js.edit', curid, DataJson.id);
                                    }
                                });
                            }
                        });
                        toolbar.add(summryBtn);
                    } else if (json_data.toolData[i].name == 'modify') {
                        var tab_id = json_data.toolData[i].tab_id;
                        var tab_itemId = json_data.toolData[i].tab_itemId;
                        modifyBtn = Ext.create('Ext.button.Button', {
                            text: json_data.toolData[i].text,
                            scope: this,
                            iconCls: json_data.toolData[i].iconUrl,
                            hidden: hidden,
                            name: json_data.toolData[i].name,
                            handler: function (b, e) {
                                new com.embraiz.tag().tabRefrash('com.embraiz.entry3.js.edit', tab_id, tab_itemId);
                            }
                        });
                        toolbar.add(modifyBtn);
                    } else if (json_data.toolData[i].name == 'refresh') {
                        var tabUrl = json_data.toolData[i].tabUrl;
                        var tab_id = json_data.toolData[i].tab_id;
                        var tab_itemId = json_data.toolData[i].tab_itemId;
                        refresh = Ext.create('Ext.button.Button', {
                            text: json_data.toolData[i].text,
                            scope: this,
                            iconCls: json_data.toolData[i].iconUrl,
                            hidden: hidden,
                            name: json_data.toolData[i].name,
                            handler: function (b, e) {
                                if (tabUrl != undefined) {
                                    new com.embraiz.tag().tabRefrash(tabUrl, tab_id, tab_itemId);
                                }

                            }
                        });
                        toolbar.add(refresh);
                    } else if (json_data.toolData[i].name == 'save') {
                        saveBtn = Ext.create('Ext.button.Button', {
                            text: json_data.toolData[i].text,
                            scope: this,
                            iconCls: json_data.toolData[i].iconUrl,
                            hidden: hidden,
                            name: json_data.toolData[i].name,
                            handler: function (b, e) {
                               
                                ///
                                Ext.getBody().mask("wait");
                                var form = me.editSimple.getForm();
                                var json_data = me.json_data;
                                ////
                                var change_field_str = "";
                                var form_field = form.getFields().items;
                                for (var i = 0; i < form_field.length; i++) {
                                    var current_field = form_field[i];
                                    if (current_field.xtype != 'hidden') {
                                        var oldValue = '';
                                        oldValue = current_field.originalValue == undefined ? '' : current_field.originalValue;
                                        //var newValue = current_field.getValue();
                                        var newValue = (current_field.getValue() == null) ? '' : current_field.getValue();
                                        if (current_field.xtype == 'datefield') {
                                            if (newValue != '' && newValue != undefined) {
                                                newValue = Ext.Date.format(newValue, 'Y-m-d');
                                            } else {
                                                newValue = '';
                                            }
                                            if (oldValue == null || oldValue == 'null') {
                                                oldValue = '';
                                            }
                                            if (oldValue != undefined && oldValue != null && oldValue != '') {
                                                oldValue = Ext.Date.format(oldValue, 'Y-m-d');
                                            }
                                            if (oldValue != newValue) {
                                                change_field_str = change_field_str + "{'field_name':\"" + current_field.fieldLabel + "\",'oldValue':\"" + oldValue + "\",'newValue':\"" + newValue + "\"},";
                                            }
                                        } else if (current_field.xtype == 'combofieldbox' || current_field.xtype == 'combobox' || current_field.xtype == 'boxselect' || current_field.xtype == 'comboboxselect') {
                                            if (typeof (oldValue) == 'object') { //
                                                var oldDisplayValue = "";
                                                var isequal = false;
                                                for (var a = 0; a < oldValue.length; a++) {
                                                    var flag = false;
                                                    for (var b = 0; b < newValue.length; b++) {
                                                        if (oldValue[a] == newValue[b]) {
                                                            flag = true;
                                                            break;
                                                        }
                                                    }
                                                    isequal = flag;
                                                }
                                                if (newValue.length == 0 && oldValue.length == 0) {
                                                    isequal = true;
                                                }
                                                if (!isequal) {
                                                    for (var s = 0; s < current_field.store.data.items.length; s++) {
                                                        for (var a = 0; a < oldValue.length; a++) {
                                                            if (current_field.store.data.keys[s] == oldValue[a]) {
                                                                oldDisplayValue = oldDisplayValue + current_field.store.data.items[s].data.value + ",";
                                                            }
                                                        }
                                                    }
                                                    change_field_str = change_field_str + "{'field_name':\"" + current_field.fieldLabel + "\",'oldValue':\"" + oldDisplayValue + "\",'newValue':\"" + current_field.getRawValue() + "\"},";

                                                }
                                            } else {
                                                if (oldValue != newValue) {
                                                    for (var s = 0; s < current_field.store.data.items.length; s++) {
                                                        if (current_field.store.data.keys[s] == oldValue) {
                                                            var textValue = current_field.store.data.items[s].data.value;
                                                            change_field_str = change_field_str + "{'field_name':\"" + current_field.fieldLabel + "\",'oldValue':\"" + textValue + "\",'newValue':\"" + current_field.getRawValue() + "\"},";
                                                        }
                                                    }
                                                }
                                            }
                                        } else if (current_field.xtype == 'checkboxgroup') {
                                            var checkgroup = current_field.items.items;
                                            for (var c = 0; c < checkgroup.length; c++) {
                                                var oldCheck = checkgroup[c].originalValue;
                                                if (oldCheck != checkgroup[c].checked) {
                                                    newValue = checkgroup[c].checked ? "Yes" : "No";
                                                    change_field_str = change_field_str + "{'field_name':\"" + (current_field.fieldLabel != undefined ? current_field.fieldLabel : current_field.boxLabel) + "\",'oldValue':\"" + oldValue + "\",'newValue':\"" + newValue + "\"},";
                                                }
                                            }

                                        } else if (current_field.xtype == 'checkbox') {
                                            var oldCheck = current_field.originalValue;
                                            if (oldCheck != current_field.getValue()) {
                                                newValue = current_field.checked ? "Yes" : "No";
                                                change_field_str = change_field_str + "{'field_name':\"" + (current_field.fieldLabel != undefined ? current_field.fieldLabel : current_field.boxLabel) + "\",'oldValue':\"" + oldValue + "\",'newValue':\"" + newValue + "\"},";
                                            }
                                        } else if (current_field.xtype == 'timefield') {
                                            var newValue = current_field.getValue();
                                            if (oldValue != newValue) {
                                                change_field_str = change_field_str + "{'field_name':\"" + (current_field.fieldLabel != undefined ? current_field.fieldLabel : current_field.boxLabel) + "\",'oldValue':\"" + oldValue + "\",'newValue':\"" + newValue + "\"},";
                                            }
                                        } else if (current_field.xtype == 'hiddenfield') {
                                            var newValue = current_field.getValue();
                                            if (oldValue != newValue) {
                                                change_field_str = change_field_str + "{'field_name':\"" + (current_field.name) + "\",'oldValue':\"" + oldValue + "\",'newValue':\"" + newValue + "\"},";
                                            }
                                        } else if (current_field.xtype != 'checkboxfield') {
                                            if (typeof oldValue != 'boolean') {
                                                oldValue = oldValue.replace(/[\r\n]/g, "");
                                            } else {
                                                oldValue = oldValue ? 'Yes' : 'No';
                                            }
                                            if (typeof newValue != 'boolean') {
                                                newValue = newValue.replace(/[\r\n]/g, "");
                                            } else {
                                                newValue = newValue ? 'Yes' : 'No';
                                            }
                                            if (oldValue != newValue) {
                                                change_field_str = change_field_str + "{'field_name':\"" + (current_field.fieldLabel != undefined ? current_field.fieldLabel : current_field.boxLabel) + "\",'oldValue':\"" + oldValue + "\",'newValue':\"" + newValue + "\"},";
                                            }
                                        }
                                    }
                                }
                                if (change_field_str != '') {
                                    var change_hidden = form.findField('change_fields');
                                    change_hidden.setRawValue(change_field_str);
                                }
                                if (form.isValid()) {
                                    form.submit({
                                        submitEmptyText: false,
                                        success: function (form, action) {
                                            Ext.getBody().unmask();
                                            var data = Ext.decode(action.response.responseText);
                                            if (data.url != '') {
                                                form.reset();
                                                Ext.Ajax.request({
                                                    url: data.url,
                                                    success: function (o) {
                                                        var dataJson = Ext.decode(o.responseText);
                                                        Ext.getCmp('content-panel').getActiveTab().close();
                                                    }
                                                });
                                            } else {

                                                Ext.Msg.show({
                                                    title: 'Success',
                                                    msg: action.result.msg || '',
                                                    width: 200,
                                                    buttons: Ext.Msg.OK,
                                                    icon: Ext.MessageBox.INFO
                                                });
                                                if (me.tabUrl != null && me.tabUrl != undefined && me.tabUrl != '') {
                                                    new com.embraiz.tag().tabRefrash(me.tabUrl, me.itemId, me.itemId1);
                                                }
                                                if (me.pre_tab != undefined && me.index_url != undefined) {
                                                    new com.embraiz.tag().previous_tab_refresh(me.pre_tab, me.index_url, me.index_id, me.index_itemId);
                                                }
                                            }
                                        },
                                        failure: function (form, action) {

                                            Ext.getBody().unmask();
                                            Ext.Msg.alert('Failed', action.result.msg);
                                        }
                                    });

                                } else {
                                    Ext.getBody().unmask();
                                }
                            }
                        });
                        toolbar.add(saveBtn);

                    } else {
                        if (json_data.toolData[i].items == undefined) {
                            toolbar.add({
                                xtype: json_data.toolData[i].xtype,
                                text: json_data.toolData[i].text,
                                iconCls: json_data.toolData[i].iconUrl,
                                name: json_data.toolData[i].name,
                                hidden: hidden,
                               
                                newTag_method: json_data.toolData[i].newTag_method,
                                newTag_id: json_data.toolData[i].newTag_id,
                                newTag_title: json_data.toolData[i].newTag_title,
                                newTag_url: json_data.toolData[i].newTag_url,
                                newTag_iconCls: json_data.toolData[i].newTag_iconCls,
                                newTag_iconClsC: json_data.toolData[i].newTag_iconClsC,
                                newTag_iconClsE: json_data.toolData[i].newTag_iconClsE,
                                newTag_itemId: json_data.toolData[i].newTag_itemId,
                                newTag_extra: json_data.toolData[i].newTag_extra,

                                handler: function () {

                                    if (this.newTag_method != undefined) {
                                        if (this.newTag_method == "open_new_tag")
                                            new com.embraiz.tag().openNewTag(
                                                this.newTag_id,
                                                this.newTag_title,
                                                this.newTag_url,
                                                this.newTag_iconCls,
                                                this.newTag_iconClsC,
                                                this.newTag_iconClsE,
                                                this.newTag_itemId,
                                                this.newTag_extra
                                            );
                                        else if (this.newTag_method == "open_pop_up")
                                            new com.embraiz.tag().open_pop_up(
                                                this.newTag_id,
                                                this.newTag_title,
                                                this.newTag_url,
                                                this.newTag_iconCls,
                                                this.newTag_iconClsC,
                                                this.newTag_iconClsE,
                                                this.newTag_itemId,
                                                this.newTag_extra
                                            );

                                    }
                                },
                                checkDao: json_data.toolData[i].dao,
                                checkMethod: json_data.toolData[i].method,
                                keyid: json_data.toolData[i].keyid,
                                listeners: {
                                    beforeRender: function (thiz, op) {
                                        if (this.checkMethod != undefined && this.checkMethod != '') {
                                            Ext.Ajax.request({
                                                url: 'common/checkMethod.jsp',
                                                params: {
                                                    dao: thiz.checkDao,
                                                    method: thiz.checkMethod,
                                                    id: thiz.keyid
                                                },
                                                success: function (o) {
                                                    var result = Ext.decode(o.responseText);
                                                    if (result.flag) {
                                                        thiz.show();
                                                    } else {
                                                        thiz.hide();
                                                    }

                                                },
                                                failure: function () {
                                                    thiz.hide();
                                                }
                                            })
                                        }
                                    }
                                }
                            });
                        } else {
                            toolbar.add({
                                xtype: json_data.toolData[i].xtype,
                                text: json_data.toolData[i].text,
                                iconCls: json_data.toolData[i].iconUrl,
                                name: json_data.toolData[i].name,
                                hidden: hidden,
                                plain: true,
                                listeners: json_data.toolData[i].listeners,
                                menu: {
                                    items: json_data.toolData[i].items
                                }
                            });

                        }
                        if (json_data.toolData[i]) {
                            new Ext.util.KeyMap(Ext.getCmp('content-panel').getActiveTab().id, {
                                key: fn[i - 2],
                                alt: true,
                                handler: json_data.toolData[i].href
                            });
                        }
                    } //if
                } //////for

                new com.embraiz.roleManagement.js.index().initTag(this.toolbar);
            },
            scope: this
        });
        toolbar.render(Ext.getCmp('content-panel').getActiveTab().getEl());
        //toolbar.render(target_div);
        toolbar.focus();
    }
});