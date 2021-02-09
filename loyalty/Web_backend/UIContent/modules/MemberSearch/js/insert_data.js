// Constant
var _searchField = {
    availablePoint: 1,
    birthdayMonth: 2,
    email: 3,
    gender: 4,
    hkid: 5,
    lastPurchaseDate: 6,
    locationVisit: 7,
    locationVisitDate: 8,
    memberCode: 9,
    memberLevel: 10,
    missionDate: 11,
    mobileNo: 12,
    name: 13,
    purchaseAmount: 14,
    purchaseCategory: 15,
    purchaseItem: 16,
    purchaseItemCount: 17,
    purchaseDate: 18,
    registrationDate: 19,
    redeemCategory: 20,
    redeemDate: 21,
    redeemItem: 22,
    redeemItemCount: 23,
    status: 24
};

var _compareCondition = {
    is: 1,
    like: 2,
    lessThan: 3,
    lessOrEqual: 4,
    equal: 5,
    largerOrEqual: 6,
    largerThan: 7
};

// global variable
var _groupNo = 1;

// widget.memberSearchGroupFieldset
Ext.define('Ext.ux.form.field.memberSearchGroupFieldset', {
    extend: 'Ext.form.FieldContainer',
    mixins: {
        field: 'Ext.form.field.Field'
    },
    alias: 'widget.memberSearchGroupFieldset',
    layout: 'hbox',
    height: 22,
    cls: 'custom_field',

    initComponent: function () {
        var me = this;
        me.buildField();
        me.callParent();
        me.submitValue = false;
        this.textfield = this.down('textfield');
        this.checkbox = this.down('checkbox');
        me.initField();
    },
    buildField: function () {
        var me = this;

        this.items = [
            {
                xtype: 'button',
                text: 'Add Criteria',
                padding: '3 0 0 0',
                margin: '6',
                handler: this.addDetailRow
            },
             {
                 xtype: 'button',
                 text: 'Add Or Group',
                 padding: '3 0 0 0',
                 margin: '6',
                 handler: this.addGroup
             }

        ];
    },
    addDetailRow: function (e, b) {
       
        var me = this;
        var fieldSet = e.ownerCt.ownerCt; //Ext.getCmp("memberSearchColumn_criteria_set");
        fieldSet.lastRowNo++;
        var rowID = "g" + int99ToString(fieldSet.groupNo) + "r" + int99ToString(fieldSet.lastRowNo);

        var counter = fieldSet.items.getCount();

        var now = new Date();
        var newID = now.getHours() + '' + now.getMinutes() + '' + now.getSeconds() + '' + now.getMilliseconds();
        var comp_id = "ijc_criteriaRow_" + newID;
        var item3_id = 'ijc_valueConnector_' + newID;
        var item4_id = 'ijc_valueItem_' + newID;

        var selectName = '';
        var selectEmptyText = 'Select Service';
       // var selectDatasource = "../Table/GetListItems/servicecategory";
        var selectValue = '';

        var typeValue = 1; //parseInt(Ext.getCmp("import_job_type_select").items.items[0].value);
        var select_column_jsonurl = "";

        if (typeValue == 1)
            select_column_jsonurl = '../PromotionRule/GetTransactionColumn'
        else
            select_column_jsonurl = '../PromotionRule/GetMemberColumn'

        var item1 = {
            xtype: 'combobox',
            name: rowID + '_target_field',
            //specialID: newID,
            padding: '3 0 0 0',
            plugins: ['clearbutton'],
            displayField: 'value',
            valueField: 'id',
            emptyText: 'Select Field',
            width: 200,
            allowBlank: false,
            store: Ext.create('Ext.data.Store', {
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
                    url: "../Table/GetListItems/MemberAdvanceSearchField",
                    reader: {
                        type: 'json',
                        root: 'data'
                    }
                }
            }),
            queryMode: 'local',
            value: selectValue,
            lastSelectedValue: 0,
            listeners: {
                select: function(e) {
                   
                    var parent = e.ownerCt;
                  
                    var selectedValue = parseInt(this.value);
                   
                    // change condition field
                    var conditionField = e.ownerCt.items.items[1];

                    var equalArray = [_searchField.gender, _searchField.status];
                    var isArray = [_searchField.birthdayMonth, _searchField.locationVisit, _searchField.memberLevel,
                        _searchField.purchaseCategory, _searchField.purchaseItem, _searchField.redeemCategory, _searchField.redeemItem];
                    
                    var likeArray = [_searchField.email, _searchField.hkid, _searchField.memberCode, _searchField.mobileNo, _searchField.name];
                    var rangeArray = [_searchField.availablePoint, _searchField.lastPurchaseDate, _searchField.locationVisitDate,
                        _searchField.missionDate, _searchField.purchaseAmount, _searchField.purchaseItemCount, _searchField.purchaseDate, _searchField.registrationDate, _searchField.redeemDate];

                    if ($.inArray(selectedValue, equalArray) >= 0) {
                        if ($.inArray(e.lastSelectedValue, rangeArray) >= 0) {
                            // need to change store

                            var newStore = Ext.create('Ext.data.Store', {
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
                                    url: "../Table/GetListItems/CompareCondition",
                                    reader: {
                                        type: 'json',
                                        root: 'data'
                                    }
                                }
                            });

                            conditionField.bindStore(newStore);
                        }

                        conditionField.setValue(_compareCondition.equal.toString());
                        conditionField.setReadOnly(true);
                    }
                    else if ($.inArray(selectedValue, isArray) >= 0) {
                        if ($.inArray(e.lastSelectedValue, rangeArray) >= 0) {
                            // need to change store

                            var newStore = Ext.create('Ext.data.Store', {
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
                                    url: "../Table/GetListItems/CompareCondition",
                                    reader: {
                                        type: 'json',
                                        root: 'data'
                                    }
                                }
                            });

                            conditionField.bindStore(newStore);
                        }

                        conditionField.setValue(_compareCondition.is.toString());
                        conditionField.setReadOnly(true);
                    }
                    else if ($.inArray(selectedValue, likeArray) >= 0) {
                        if ($.inArray(e.lastSelectedValue, rangeArray) >= 0) {
                            // need to change store

                            var newStore = Ext.create('Ext.data.Store', {
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
                                    url: "../Table/GetListItems/CompareCondition",
                                    reader: {
                                        type: 'json',
                                        root: 'data'
                                    }
                                }
                            });

                            conditionField.bindStore(newStore);
                        }

                        conditionField.setValue(_compareCondition.like.toString());
                        conditionField.setReadOnly(true);
                    }
                    else { //range array
                        var newStore = Ext.create('Ext.data.Store', {
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
                                url: "../Table/GetListItems/CompareCondition_onlyrange",
                                reader: {
                                    type: 'json',
                                    root: 'data'
                                }
                            }
                        });

                        conditionField.bindStore(newStore);

                        conditionField.reset();
                        conditionField.setReadOnly(false);
                    }

                    // change input value field
                    var textArray = [_searchField.email, _searchField.hkid, _searchField.memberCode, _searchField.mobileNo, _searchField.name];
                    var textIntArray = [_searchField.availablePoint, _searchField.purchaseAmount, _searchField.purchaseItemCount];
                    var dateArray = [_searchField.lastPurchaseDate, _searchField.locationVisitDate, _searchField.missionDate, _searchField.purchaseDate, _searchField.registrationDate,
                        _searchField.redeemDate];
                    var selectArray = [_searchField.gender, _searchField.status];
                    var multiSelectArray = [_searchField.birthdayMonth, _searchField.locationVisit, _searchField.memberLevel, _searchField.purchaseCategory, _searchField.purchaseItem,
                        _searchField.redeemCategory, _searchField.redeemItem];

                    var valueField = e.ownerCt.items.items[2];
                
                    var valueFieldName = rowID + "v1_target_value";

                    if ($.inArray(selectedValue, textArray) >= 0 || $.inArray(selectedValue, textIntArray) >= 0)
                    {
                        parent.remove(valueField);

                        var newValueField = {
                            xtype: 'textfield',

                            name: valueFieldName,
                            padding: '3 0 0 0',
                            width: 150,
                            value: "",
                            // emptyText: "value",
                            //regex: /^\d{0,4}(\.\d{0,2}){0,1}$/,
                            allowBlank: false            
                        }

                        parent.insert(2, newValueField);
                    }
                    else if ($.inArray(selectedValue, dateArray) >= 0) {

                        parent.remove(valueField);

                        var newValueField = {
                            xtype: 'datefield',

                            name: valueFieldName,
                            padding: '3 0 0 0',
                            width: 150,
                            value: "",
                            // emptyText: "value",
                            //regex: /^\d{0,4}(\.\d{0,2}){0,1}$/,
                            allowBlank: false
                        }

                        parent.insert(2, newValueField);
                    }
                    else if ($.inArray(selectedValue, selectArray) >= 0) {

                        parent.remove(valueField);

                        var selectDataURL = "";
                        if (selectedValue == _searchField.gender)
                            selectDataURL = "../Table/GetListItems/Gender";
                        else if (selectedValue == _searchField.status)
                            selectDataURL = "../Table/GetListItems/Status";

                        var newValueField = {
                            xtype: 'combobox',
                            name: valueFieldName,
                            //specialID: newID,
                            padding: '3 0 0 0',
                            plugins: ['clearbutton'],
                            displayField: 'value',
                            valueField: 'id',
                            emptyText: 'Select',
                            width: 200,
                            allowBlank: false,
                            store: Ext.create('Ext.data.Store', {
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
                                    url: selectDataURL,
                                    reader: {
                                        type: 'json',
                                        root: 'data'
                                    }
                                }
                            }),
                            queryMode: 'local',
                            value: ''
                        };

                        parent.insert(2, newValueField);
                    }
                    else if ($.inArray(selectedValue, multiSelectArray) >= 0) {

                        parent.remove(valueField);

                        var selectDataURL = "";
                        if (selectedValue == _searchField.birthdayMonth)
                            selectDataURL = "../Table/GetListItems/Month";
                        else if (selectedValue == _searchField.locationVisit)
                            selectDataURL = "../Table/GetListItems/location";
                        else if (selectedValue == _searchField.memberLevel)
                            selectDataURL = "../Table/GetListItems/MemberLevel";
                        else if (selectedValue == _searchField.purchaseCategory)
                            selectDataURL = "../Table/GetListItems/ProductCategory";
                        else if (selectedValue == _searchField.purchaseItem)
                            selectDataURL = "../Table/GetListItems/Product";
                        else if (selectedValue == _searchField.redeemCategory)
                            selectDataURL = "../Table/GetListItems/GiftCategory";
                        else if (selectedValue == _searchField.redeemItem)
                            selectDataURL = "../Table/GetListItems/Gift";

                        var newValueField = {
                           // id: "member_level_test",
                          //  fieldLabel: "Member Level",
                            xtype: "combofieldbox",
                            name: valueFieldName,
                            multiSelect: true,
                            allowBlank: false,
                            //labelStyle: 'color:red',
                            tabIndex: '',
                            //init_value: [-1, 1, 2, 3, 4],
                            value: '',
                            emptyText: 'Please Select',
                            displayField: 'value',
                            valueField: 'id',
                            forceSelection: true,
                            width: 400,
                            store: Ext.create('Ext.data.Store', {
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
                                    url: selectDataURL,
                                    reader: {
                                        type: 'json',
                                        root: 'data'
                                    }
                                }
                            }),
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

                        //var newValueField = {
                        //    xtype: 'combobox',
                           
                        //    name: 'target_value',
                        //    //specialID: newID,
                        //    padding: '3 0 0 0',
                        //    plugins: ['clearbutton'],
                        //    displayField: 'value',
                        //    valueField: 'id',
                        //    emptyText: 'Select',
                        //    width: 400,
                        //    allowBlank: false,
                        //    multiSelect: true,
                        //    //cls: 'readonlyField',
                        //    store: Ext.create('Ext.data.Store', {
                        //        fields: [{
                        //            name: 'id',
                        //            type: 'string'
                        //        }, {
                        //            name: 'value',
                        //            type: 'string'
                        //        }],
                        //        autoLoad: true,
                        //        proxy: {
                        //            type: 'ajax',
                        //            url: "../Table/GetListItems/MemberAdvanceSearchField",
                        //            reader: {
                        //                type: 'json',
                        //                root: 'data'
                        //            }
                        //        }
                        //    }),
                        //    queryMode: 'local',
                        //    value: ''
                        //};

                        parent.insert(2, newValueField);
                    }

                    e.lastSelectedValue = selectedValue;
                }
            }
        };

        var item2 = {
            xtype: 'combobox',
            name: rowID + '_target_condition',
            specialID: newID,
            padding: '3 0 0 0',
            plugins: ['clearbutton'],
            displayField: 'value',
            valueField: 'id',
            emptyText: 'Select Condition',
            allowBlank: false,
            store: Ext.create('Ext.data.Store', {
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
                    url: "../Table/GetListItems/CompareCondition",
                    reader: {
                        type: 'json',
                        root: 'data'
                    }
                }
            }),
            queryMode: 'local',
            value: selectValue,
            width: 150,
            listeners: {
                select: function () {
                    var theValue = parseInt(this.value);


                }
            }
        };

        var item3 = {
            xtype: 'textfield',

            name: rowID + "_target_value",
            padding: '3 0 0 0',
            width: 150,
            value: "",
            // emptyText: "value",
            //regex: /^\d{0,4}(\.\d{0,2}){0,1}$/,
            allowBlank: false            
        };

        var fieldContainer = {
            xtype: 'fieldcontainer',
            //id: comp_id,
            layout: 'hbox',
            //height: 30,    // has multi select field, cannot set fix hieght
            cls: 'custom_field',
            padding: '0 0 0 0',
            rowspan: 1,
            colspan: 2,
            items: [

                item1,
                item2,
                item3,
                {
                    xtype: 'button',
                    text: 'Remove',
                    padding: '3 0 0 0',
                    margin: '6',
                    handler: function (e, b) {
                        var p = e.ownerCt.ownerCt;
                        p.remove(e.ownerCt);
                    }
                }]
        };
        fieldSet.add(fieldContainer);
    },
    addGroup: function (e, b) {
        _groupNo++;

        var theContainer = e.ownerCt.ownerCt.ownerCt;
        //var testAdd2 = {
        //    xtype: 'label',
        //    text: 'Or',
        //   // layout: 'hbox',
        //    height: 30,
        //    cls: 'custom_field',
        //    padding: '15 0 0 5',
        //    rowspan: 1,
        //    colspan: 2,
        //    //items: [

        //    //    {
        //    //        xtype: 'textfield',

        //    //        name: "abcd2",
        //    //        padding: '3 0 0 0',
        //    //        width: 150,
        //    //        value: "ColumnOKOK222 ",
        //    //        // emptyText: "value",
        //    //        //regex: /^\d{0,4}(\.\d{0,2}){0,1}$/,
        //    //        allowBlank: false,
        //    //        readOnly: true
        //    //    }
        //    //]
        //};
        //theContainer.add(testAdd2);

        containerAddOrGroup(theContainer, false);

        //var testSet = {
        //    xtype: 'fieldset',
        //    title: 'Or group',
        //    //  id: 'memberSearchColumn_criteria_set',
        //    colspan: 2,
        //    collapsible: true,
        //    defaultType: 'textfield',

        //    layout: {
        //        type: 'table',
        //        columns: 2,
        //        tableAttrs: {
        //            style: {
        //                width: '100%',
        //                margin: '0px 5px 5px 0px'

        //            }
        //        },
        //        tdAttrs: {
        //            style: {
        //                width: '50%'
        //            }
        //        }
        //    },
        //    items: [
        //        {
        //            //id: "group_fieldset_additem",
        //            name: "group_fieldset_additem",
        //            fieldLabel: "New Column Mapping",
        //            xtype: "memberSearchGroupFieldset",
        //            value: "",
        //            rowspan: 1,
        //            colspan: 2,
        //            height: 30
        //        }
        //    ]
        //};
        //theContainer.add(testSet);

        
    }
});
// ========================
// functions for member search add
function containerAddOrGroup(container, firstGroup) {

    var title = "";
    if (firstGroup)
        title = "Criteria group";
    else
        title = "Or group";

    var fieldSet = {
        xtype: 'fieldset',
        title: title,
        colspan: 2,
        collapsible: true,
        defaultType: 'textfield',
        groupNo: _groupNo,
        lastRowNo: 0,
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
        items: [
            {
                name: "group_fieldset_additem",
                fieldLabel: "Match following criteria",
                xtype: "memberSearchGroupFieldset",
                value: "",
                rowspan: 1,
                colspan: 2,
                height: 30
            }
        ]
    };
    container.add(fieldSet);
}

function int99ToString(intValue) {

    var strValue = "";
    if (intValue < 10) {
        strValue = "0" + intValue.toString();
    }
    else {
        strValue = intValue.toString();
    }

    return strValue;
}

// ========================

Ext.define('com.palmary.membersearch.js.insert', {
	gridPanel:undefined,
	initTag: function (tab, url, title) {

	    // Check user seesion 
	    checkSession();
	
	    target_div=document.createElement("div");
	    tab.getEl().dom.lastChild.appendChild(target_div);
	    grider_div=document.createElement("div");
	    tab.getEl().dom.lastChild.appendChild(grider_div);	

	    //Ext.Ajax.request({
	    //    url: '../Member/Insert',
	    //    success:function(o){
	    //        var data_json = Ext.decode(o.responseText);
	    //        new com.embraiz.component.form().editForm(target_div, data_json,null);
	    //    },
	    //    scope:this
	    //});

	    // start to generate form
	   // var form_content = [];

       //form_content.push(
       //    {
       //       // id: "import_job_type_select",
       //        name: "import_job_type_select",
       //        fieldLabel: "Import Job Type",
       //        xtype: "select_fileimportjobType",
       //        value: "",
       //        rowspan: 1,
       //        colspan: 2,
       //        datasource: "../PromotionRule/GetImportJobType",
       //        height: 30
       //    }
       //);

	   // // member_group_criteria_set
	   // //var group_fieldset_additem = {
	   // //    //id: "group_fieldset_additem",
	   // //    name: "group_fieldset_additem",
	   // //    fieldLabel: "New Column Mapping",
	   // //    xtype: "memberSearchGroupFieldset",
	   // //    value: "",
	   // //    rowspan: 1,
	   // //    colspan: 2,
	   // //    height: 30
	   // //};


      

	   // var member_group_criteria_set = {
	   //     xtype: 'fieldset',
	   //     title: 'Criteria group',
	   //    // id: 'memberSearchColumn_criteria_set',
	   //     colspan: 2,
	   //     collapsible: true,
	   //     defaultType: 'textfield',

	   //     layout: {
	   //         type: 'table',
	   //         columns: 2,
	   //         tableAttrs: {
	   //             style: {
	   //                 width: '100%',
	   //                 margin: '0px 5px 5px 0px'

	   //             }
	   //         },
	   //         tdAttrs: {
	   //             style: {
	   //                 width: '50%'
	   //             }
	   //         }
	   //     },
	   //     items: [

	   //         //{
	   //         //    xtype: 'fieldcontainer',
	   //         //    layout: 'hbox',
	   //         //    height: 30,
	   //         //    cls: 'custom_field',
	   //         //    padding: '0 0 0 0',
	   //         //    rowspan: 1,
	   //         //    colspan: 2,
	   //         //    items: [

       //         //        {
       //         //            xtype: 'label',
       //         //            text: 'match of following criteria',
       //         //            layout: 'hbox',
       //         //            height: 30,
       //         //            cls: 'custom_field',
       //         //            padding: '3 0 0 0',
       //         //            rowspan: 1,
       //         //            colspan: 2,
       //         //        }
	   //         //    ]
	   //         //},
       //         {
       //             name: "membersearch_group_fieldset",
       //             fieldLabel: "Match following criteria",
       //             xtype: "memberSearchGroupFieldset",
       //             value: "",
       //             rowspan: 1,
       //             colspan: 2,
       //             height: 30
       //         }

	   //     ]
	   // };
	   // form_content.push(member_group_criteria_set);

	    // Form Main Container
	    var formContainer = Ext.create('Ext.container.Container', {
	        anchor: '100%',
	       // id: 'crtForm_fileimportjob',
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
	       // items: form_content
	        //initComponent: function() {

	        //    this.items = form_content;

	        //   this.callParent();
	        //}
	    });

	    var nameField = {
	        xtype: 'textfield',
            fieldLabel: "Name",
	        name: "name",
	        padding: '3 0 0 0',
	        width: 400,
	        value: "",
	        rowspan: 1,
	        colspan: 2,
	        // emptyText: "value",
	        //regex: /^\d{0,4}(\.\d{0,2}){0,1}$/,
	        allowBlank: false
	    };
	    formContainer.add(nameField);

	    containerAddOrGroup(formContainer, true);

	    // form
	    var viewSimple = this.viewSimple = Ext.create('Ext.form.Panel', {
	        url: '../MemberAdvanceSearch/Create',
	        title: 'Create Advance Member Search Group',
	        iconCls: 'iconForm',
	        method: 'POST',
	        margin: '5px 5px 5px 5px',
	        width: '99%',

	        fieldDefaults: {
	            msgTarget: 'side',
	            labelWidth: 150,
	            labelStyle: 'font-weight: bold'
	        },
	        defaultType: 'textfield',
	        items: formContainer,
	        //cls: 'editform',
	        buttonAlign: 'left',
	        buttons: [
                Ext.create('Ext.Button', {
                    text: 'Create Advacne Search Group',
                    iconCls: '',
                    style: {
                        float: 'left'
                    },

                    handler: function () {
                        var form = this.up('form').getForm();
                        if (form.isValid()) {
                            form.submit({
                                success: function (form, action) {
                                    Ext.Msg.alert('Success', action.result.msg);
                                },
                                failure: function (form, action) {
                                    Ext.Msg.alert('Failed', action.result.msg);
                                }
                            });
                        }
                    }
                })
	        ],
	        renderTo: target_div
	    });


	//    var test = Ext.getCmp("content-panel").getActiveTab().getEl().dom.firstChild;
	   // for (key in test) {
	   //     alert(key);
	    // }

	    //alert(test);
	    //alert(test.id);
	    //alert(test.children.length);
	    //alert(test.children[1].id);  // ext-genxxxx
	    //var test2 = test.firstChild;
	    //var msg = "";
	    // for (key in test2) {
	    //     msg += key + ", ";
	    // }
	    //console.log(msg);
	    
	    //alert(test2);
        //alert(test2.id);
	    

	}
});
