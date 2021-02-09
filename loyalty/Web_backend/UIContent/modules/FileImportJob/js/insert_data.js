Ext.define('Ext.ux.form.field.Select_fileimportjobType', {
    extend: 'Ext.form.FieldContainer',
    mixins: {
        field: 'Ext.form.field.Field'
    },
    alias: 'widget.select_fileimportjobType',
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
        var store = Ext.create('Ext.data.Store', {
            fields: [{
                name: 'id',
                type: 'string'
            }, {
                name: 'value',
                type: 'string'
            }],

            remoteSort: true,
            autoLoad: true,
            proxy: {
                type: 'ajax',
                url: this.datasource,
                reader: {
                    type: 'json',
                    root: 'data',
                    totalProperty: 'totalCount'
                }
            }
        });
        store.on('beforeload', function (thiz, action, value) {
            if (thiz.getCount() == 0 && value && value != '') {
                thiz.proxy.extraParams.defaultValue = value;
            }
        }, true, this.field1Value);
        this.items = [
            Ext.apply({
                xtype: 'myCombobox',
                fieldLabel: '',
                name: this.name,
                store: store,
                value: this.value,
                valueField: 'id',
                displayField: 'value',
                typeAhead: true,
                padding: 0,
                queryMode: 'local',//remote
                emptyText: 'Please Select',
                tabIndex: this.tabIndex,
                allowBlank: false,
                forceSelection: true,
                listeners: {
                    select: function () {

                        var theValue = parseInt(me.items.items[0].value);

                    }
                }
            })
        ];
    }
});




Ext.define('Ext.ux.form.field.MyAddImportFileJob', {
    extend: 'Ext.form.FieldContainer',
    mixins: {
        field: 'Ext.form.field.Field'
    },
    alias: 'widget.myAddImportFileJob',
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
        var fieldSet = Ext.getCmp("importJobColumn_criteria_set");

        var counter = fieldSet.items.getCount()
        
        

        var now = new Date();
        var newID = now.getHours() + '' + now.getMinutes() + '' + now.getSeconds() + '' + now.getMilliseconds();
        var comp_id = "ijc_criteriaRow_" + newID;
        var item3_id = 'ijc_valueConnector_' + newID;
        var item4_id = 'ijc_valueItem_' + newID;

        var selectName = '';
        var selectEmptyText = 'Select Service';
        var selectDatasource = "../Table/GetListItems/servicecategory";
        var selectValue = '';

        var item1 = {
            xtype: 'textfield',
         
            name: "column_id",
            padding: '3 0 0 0',
            width: 150,
            value: "Column "+counter,
           // emptyText: "value",
            //regex: /^\d{0,4}(\.\d{0,2}){0,1}$/,
            allowBlank: false,
            readOnly: true
        };

        var typeValue = parseInt(Ext.getCmp("import_job_type_select").items.items[0].value);
        var select_column_jsonurl = "";
     
        if (typeValue == 1)
            select_column_jsonurl = '../PromotionRule/GetTransactionColumn'
        else
            select_column_jsonurl = '../PromotionRule/GetMemberColumn'

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
                    url: select_column_jsonurl,
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



Ext.define('com.palmary.fileimportjob.js.insert', {
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
                       id: "import_job_type_select",
                       name: "import_job_type_select",
                       fieldLabel: "Import Job Type",
                       xtype: "select_fileimportjobType",
                       value: "",
                       rowspan: 1,
                       colspan: 2,
                       datasource: "../PromotionRule/GetImportJobType",
                       height: 30
                   }
               );

	            form_content.push(
                 {
                   //  id: "description",
                     fieldLabel: "Job Name",
                     xtype: "textfield",
                     name: "name",
                     value: "",
                     rowspan: 1,
                     colspan: 2,
                     allowBlank: false
                 }
                );

	            form_content.push({
	                name: "schedule_interval",
	                fieldLabel: "Schedule Interval",
	                xtype: "mySelect",
	                value: "",
	                rowspan: 1,
	                colspan: 2,
	                datasource: "../PromotionRule/GetScheduleInterval",
	                allowBlank: false,
	                height: 30
	            });

	            form_content.push({
	                name: "schedule_time",
	                fieldLabel: "Schedule Time",
	                xtype: "timefield",
	                increment: 15,
	                format: 'H:i',
	                minValue: '12:00 AM',
	                maxValue: '11:45 PM',
	                value: "",
	                rowspan: 1,
	                colspan: 2,
	                
	                allowBlank: false,
	              // width: 150
	            });

	            form_content.push({
	                name: "file_type",
	                fieldLabel: "File Type",
	                xtype: "mySelect",
	                value: "",
	                rowspan: 1,
	                colspan: 2,
	                datasource: "../PromotionRule/GetFileType",
	                allowBlank: false,
	                height: 30
	            });

	            form_content.push(
                {
                    //  id: "description",
                    fieldLabel: "File Path",
                    xtype: "textfield",
                    name: "file path",
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
	                fieldLabel: "New Column Mapping",
	                xtype: "myAddImportFileJob",
	                value: "",
	                rowspan: 1,
	                colspan: 2,
	                height: 30
	            };

	            // member_group_criteria_set
	            var member_group_criteria_set = {
	                xtype: 'fieldset',
	                title: 'Column Criteria',
	                id: 'importJobColumn_criteria_set',
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
	                id: 'crtForm_fileimportjob',
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
	                title: 'Create File Import Job',
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
                            text: 'Create Job',
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
