Ext.define('Ext.ux.form.field.MyAddTransactionGroupCriteria', {
    extend: 'Ext.form.FieldContainer',
    mixins: {
        field: 'Ext.form.field.Field'
    },
    alias: 'widget.myAddTransactionGroupCriteria',
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
                text: 'Add',
                padding: '3 0 0 0',
                margin: '6',
                handler: this.addDetailRow
            }
        ];
    },
    addDetailRow: function (e, b) {
        var me = this;
        var fieldSet = Ext.getCmp("transaction_group_criteria_set");
        var now = new Date();
        var newID = now.getHours() + '' + now.getMinutes() + '' + now.getSeconds() + '' + now.getMilliseconds();
        var comp_id = "tc_criteriaRow_" + newID;
        var item3_id = 'tc_valueConnector_' + newID;
        var item4_id = 'tc_valueItem_' + newID;

        var selectName = '';
        var selectEmptyText = 'Select Service';
        var selectDatasource = "../Table/GetListItems/servicecategory";
        var selectValue = '';

        var item1 = {
            xtype: 'combobox',
            name: 'row_head_criteria',
            
            padding: '3 0 0 0',
            plugins: ['clearbutton'],
            displayField: 'value',
            valueField: 'id',
            emptyText: '',
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
                    url: '../PromotionRule/GetRowHead',
                    reader: {
                        type: 'json',
                        root: 'data'
                    }
                }
            }),
            queryMode: 'local',
            value: selectValue,
            width: 70
        };

        var item2 = {
            xtype: 'combobox',
            name: 'column_target_criteria',
            specialID: newID,
            padding: '3 0 0 0',
            plugins: ['clearbutton'],
            displayField: 'value',
            valueField: 'id',
            emptyText: 'Select Column',
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
                    url: '../PromotionRule/GetTransactionColumn',
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

                    if (theValue == 1 || theValue == 4 || theValue == 6 || theValue == 7) {// int, float
                        var new_item3 = {
                            xtype: 'combobox',
                            id: item3_id,
                            name: item3_id,
                        
                            specialID: newID,
                            padding: '3 0 0 0',
                            plugins: ['clearbutton'],
                            displayField: 'value',
                            valueField: 'id',
                            emptyText: 'Select',
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
                                    url: '../PromotionRule/GetValueConnector_intfloat',
                                    reader: {
                                        type: 'json',
                                        root: 'data'
                                    }
                                }
                            }),
                            queryMode: 'local',
                            value: selectValue,
                            width: 100,
                            allowBlank: false
                        };

                        var new_item4 = {
                            xtype: 'textfield',
                            id: item4_id,
                            name: item4_id,
                            padding: '3 0 0 0',
                            width: 150,
                            value: '',
                            emptyText: "value",
                            //regex: /^\d{0,4}(\.\d{0,2}){0,1}$/,
                            allowBlank: false
                        };


                        var parent = Ext.getCmp(comp_id);
                        var current_item3 = Ext.getCmp(item3_id);
                        var current_item4 = Ext.getCmp(item4_id);

                        parent.remove(current_item3);
                        parent.remove(current_item4);

                        parent.insert(2, new_item3);
                        parent.insert(3, new_item4)

                    }
                    else if (theValue == 10 || theValue == 11) { //date
                        var new_item3 = {
                            xtype: 'combobox',
                            id: item3_id,
                            name: item3_id,

                            specialID: newID,
                            padding: '3 0 0 0',
                            plugins: ['clearbutton'],
                            displayField: 'value',
                            valueField: 'id',
                            emptyText: 'Select',
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
                                    url: '../PromotionRule/GetValueConnector_intfloat',
                                    reader: {
                                        type: 'json',
                                        root: 'data'
                                    }
                                }
                            }),
                            queryMode: 'local',
                            value: selectValue,
                            width: 100,
                            allowBlank: false
                        };

                        var new_item4 = {
                            xtype: 'datefield',
                            id: item4_id,
                            name: item4_id,
                            padding: '3 0 0 0',
                            width: 100,
                            value: '',
                            emptyText: "value",
                            //regex: /^\d{0,4}(\.\d{0,2}){0,1}$/,
                            width: 150,
                            allowBlank: false
                        };


                        var parent = Ext.getCmp(comp_id);
                        var current_item3 = Ext.getCmp(item3_id);
                        var current_item4 = Ext.getCmp(item4_id);

                        parent.remove(current_item3);
                        parent.remove(current_item4);

                        parent.insert(2, new_item3);
                        parent.insert(3, new_item4)

                    }
                    //string
                    else if (theValue == 2 || theValue == 3 || theValue == 5) {
                        var new_item3 = {
                            xtype: 'combobox',
                            id: item3_id,
                            name: item3_id,

                            specialID: newID,
                            padding: '3 0 0 0',
                            plugins: ['clearbutton'],
                            displayField: 'value',
                            valueField: 'id',
                            emptyText: 'Select',
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
                                    url: '../PromotionRule/GetValueConnector_str',
                                    reader: {
                                        type: 'json',
                                        root: 'data'
                                    }
                                }
                            }),
                            queryMode: 'local',
                            value: selectValue,
                            width: 100,
                            allowBlank: false
                        };

                        var new_item4 = {
                            xtype: 'textfield',
                            id: item4_id,
                            name: item4_id,
                            padding: '3 0 0 0',
                            width: 150,
                            value: '',
                            emptyText: "value",
                           // regex: /^\d{0,4}(\.\d{0,2}){0,1}$/,
                            allowBlank: false
                        };


                        var parent = Ext.getCmp(comp_id);
                        var current_item3 = Ext.getCmp(item3_id);
                        var current_item4 = Ext.getCmp(item4_id);

                        parent.remove(current_item3);
                        parent.remove(current_item4);

                        parent.insert(2, new_item3);
                        parent.insert(3, new_item4);
                    }
                    else if (theValue == 8 || theValue == 9) { //select

                        var select_json_url = "";
                        if (theValue == 8)
                            select_json_url = "../Table/GetListItems/PaymentStauts";
                        else if (theValue == 9)
                            select_json_url = "../Table/GetListItems/PaymentMethod";

                        
                        var new_item3 = {
                            xtype: 'combobox',
                            id: item3_id,
                            name: item3_id,

                            specialID: newID,
                            padding: '3 0 0 0',
                            plugins: ['clearbutton'],
                            displayField: 'value',
                            valueField: 'id',
                            emptyText: 'Select',
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
                                    url: '../PromotionRule/GetValueConnector_select',
                                    reader: {
                                        type: 'json',
                                        root: 'data'
                                    }
                                }
                            }),
                            queryMode: 'local',
                            value: selectValue,
                            width: 100,
                            allowBlank: false
                        };

                        var new_item4 = { 
                            xtype: 'combobox',
                            id: item4_id,
                            name: item4_id,

                            specialID: newID,
                            padding: '3 0 0 0',
                            plugins: ['clearbutton'],
                            displayField: 'value',
                            valueField: 'id',
                            emptyText: 'Select',
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
                                    url: select_json_url,
                                    reader: {
                                        type: 'json',
                                        root: 'data'
                                    }
                                }
                            }),
                            queryMode: 'local',
                            value: selectValue,
                            width: 150,
                            allowBlank: false
                        };


                        var parent = Ext.getCmp(comp_id);
                        var current_item3 = Ext.getCmp(item3_id);
                        var current_item4 = Ext.getCmp(item4_id);

                        parent.remove(current_item3);
                        parent.remove(current_item4);

                        parent.insert(2, new_item3);
                        parent.insert(3, new_item4);
                    }
                    
                }
            }
        };

        var item3 = {
            xtype: 'textfield',
            id: item3_id,
            name: item3_id,
            padding: '3 0 0 0',
            width: 100,
            value: '',
            emptyText: "connector",
            regex: /^\d{0,4}(\.\d{0,2}){0,1}$/,
            allowBlank: false
        };

        var item4 = {
            xtype: 'textfield',
            id: item4_id,
            name: item4_id,
            padding: '3 0 0 0',
            width: 100,
            value: '',
            emptyText: "value",
            regex: /^\d{0,4}(\.\d{0,2}){0,1}$/,
            allowBlank: false
        };

        var item5 = {
            xtype: 'combobox',
            name: 'row_item_5',

            specialID: newID,
            padding: '3 0 0 0',
            plugins: ['clearbutton'],
            displayField: 'value',
            valueField: 'id',
            emptyText: 'Select',
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
                    url: '../PromotionRule/GetBracketEnd',
                    reader: {
                        type: 'json',
                        root: 'data'
                    }
                }
            }),
            queryMode: 'local',
            value: selectValue,
            width: 100,
            allowBlank: false
        };

        var item6 = {
            xtype: 'combobox',
            name: 'row_item_6',

            specialID: newID,
            padding: '3 0 0 0',
            plugins: ['clearbutton'],
            displayField: 'value',
            valueField: 'id',
            emptyText: 'Select',
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
                    url: '../PromotionRule/GetRowEnd',
                    reader: {
                        type: 'json',
                        root: 'data'
                    }
                }
            }),
            queryMode: 'local',
            value: selectValue,
            width: 100,
            allowBlank: false
        };

        var components = {
            xtype: 'fieldcontainer',
            id: comp_id,
            layout: 'hbox',
            height: 30,
            cls: 'custom_field',
            padding: '0 0 0 0',
            rowspan: 1,
            colspan: 2,
            items: [
               
                item1,
                item2,
                item3,
                item4,
                item5,
                item6,
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
        fieldSet.add(components);
    }
});

// =====================================================



Ext.define('com.palmary.transactiongroup.js.insert', {
	gridPanel:undefined,
	initTag: function (tab, url, title) {

	    // Check user seesion 
	    checkSession();

	    target_div=document.createElement("div");
	    tab.getEl().dom.lastChild.appendChild(target_div);
	    grider_div=document.createElement("div");
	    tab.getEl().dom.lastChild.appendChild(grider_div);	

	    Ext.Ajax.request({
	        url: '../PromotionRule/CreateForm',
	        success: function (o) {
	            var data_json = Ext.decode(o.responseText);

	            // start to generate form
	            var form_content = [];

	            form_content.push(
                   {
                       //id: "group_name",
                       fieldLabel: "Group Name",
                       xtype: "textfield",
                       name: "name",
                       value: "",
                       rowspan: 1,
                       colspan: 2,
                       allowBlank: false
                   }
               );

	            form_content.push(
                 {
                     //id: "description",
                     fieldLabel: "Description",
                     xtype: "textfield",
                     name: "description",
                     value: "",
                     rowspan: 1,
                     colspan: 2,
                     allowBlank: false
                 }
                );

	            form_content.push({
	                name: "status",
	                fieldLabel: "Status",
	                xtype: "mySelect",
	                value: "",
	                rowspan: 1,
	                colspan: 2,
	                datasource: "../Table/GetListItems/Status",
	                allowBlank: false,
	                height: 30
	            });

	            // member_group_criteria_set - inner items
	            var group_fieldset_additem = {
	                //id: "group_fieldset_additem",
	                name: "group_fieldset_additem",
	                fieldLabel: "New Criteria",
	                xtype: "myAddTransactionGroupCriteria",
	                value: "",
	                rowspan: 1,
	                colspan: 2,
	                height: 30
	            };

	            // member_group_criteria_set
	            var member_group_criteria_set = {
	                xtype: 'fieldset',
	                title: 'Group Criteria',
	                id: 'transaction_group_criteria_set',
	                colspan: 2,
	                collapsible: true,
	                defaultType: 'textfield',

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
	                items: [group_fieldset_additem]
	            };
	            form_content.push(member_group_criteria_set);

	            // form - inner items
	            var form_items = Ext.create('Ext.container.Container', {
	                anchor: '100%',
	                id: 'crtForm_transactionGroup',
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
	                //initComponent: function() {

	                //    this.items = form_content;

	                //   this.callParent();
	                //}
	            });

                // form
	            var viewSimple = this.viewSimple = Ext.create('Ext.form.Panel', {
	                url: '../MemberField/PerformAdd',
	                title: 'Create Transaction Group',
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
	                items: form_items,
	                //cls: 'editform',
	                buttonAlign: 'left',
	                buttons: [
                        Ext.create('Ext.Button', {
                            text: 'Create Group',
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

	        },
	        scope: this
	    });
	}
});
