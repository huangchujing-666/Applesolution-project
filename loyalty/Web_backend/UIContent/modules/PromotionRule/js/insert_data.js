//Ext.onReady(function (){
Ext.require([
    'com.embraiz.component.ComboBox'
]);

Ext.define('Ext.ux.form.field.Select_ruleType', {
    extend: 'Ext.form.FieldContainer',
    mixins: {
        field: 'Ext.form.field.Field'
    },
    alias: 'widget.select_ruleType',
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
                        
                        if (theValue == 1) // Purchase
                        {
                            Ext.getCmp("transaction_criteria").show();
                            Ext.getCmp("transaction_criteria").setDisabled(false);
                            // --
                            Ext.getCmp("purchase_criteria_set").show();
                            Ext.getCmp("purchase_criteria_set").setDisabled(false);

                            Ext.getCmp("choose_purchase_product_type").setDisabled(false);
                            Ext.getCmp("choose_purchase_product_type").show();

                            Ext.getCmp("purchase_earn_point").show();
                            Ext.getCmp("purchase_earn_point").setDisabled(false);

                            Ext.getCmp("comp_earn_point").hide();
                            Ext.getCmp("comp_earn_point").setDisabled(true);

                            Ext.getCmp("gift").show();
                            Ext.getCmp("gift").setDisabled(false);

                            Ext.getCmp("registration").setDisabled(false);
                            Ext.getCmp("registration").show();

                            Ext.getCmp("referral").setDisabled(false);
                            Ext.getCmp("referral").show();

                            
                            Ext.getCmp("earn_set").show();
                            Ext.getCmp("earn_set").setDisabled(false);
                            
                            Ext.getCmp("redeem_discount").hide();
                            Ext.getCmp("redeem_discount").setDisabled(true);

                            Ext.getCmp("service_criteria_set").hide();
                            Ext.getCmp("service_criteria_set").setDisabled(false);
                        }
                        else if (theValue == 2) //Redeem
                        {
                            Ext.getCmp("transaction_criteria").hide();
                            Ext.getCmp("transaction_criteria").setDisabled(true);

                            Ext.getCmp("redeem_discount").show();
                            Ext.getCmp("redeem_discount").setDisabled(false);

                            Ext.getCmp("choose_purchase_product_type").hide();
                            Ext.getCmp("choose_purchase_product_type").setDisabled(true);
                            
                            Ext.getCmp("purchase_criteria_set").hide();
                            Ext.getCmp("purchase_criteria_set").setDisabled(true);

                            Ext.getCmp("gift").hide();
                            Ext.getCmp("gift").setDisabled(true);

                            Ext.getCmp("purchase_earn_point").hide();
                            Ext.getCmp("purchase_earn_point").setDisabled(true);

                            Ext.getCmp("comp_earn_point").hide();
                            Ext.getCmp("comp_earn_point").setDisabled(true);

                            Ext.getCmp("registration").setDisabled(true);
                            Ext.getCmp("registration").hide();

                            Ext.getCmp("referral").setDisabled(true);
                            Ext.getCmp("referral").hide();

                            Ext.getCmp("earn_set").show();
                            Ext.getCmp("earn_set").setDisabled(false);

                            Ext.getCmp("service_criteria_set").hide();
                            Ext.getCmp("service_criteria_set").setDisabled(false);
                        }
                        else if (theValue == 3) // Complementary
                        {
                            Ext.getCmp("transaction_criteria").hide();
                            Ext.getCmp("transaction_criteria").setDisabled(true);

                            Ext.getCmp("redeem_discount").hide();
                            Ext.getCmp("redeem_discount").setDisabled(true);


                            Ext.getCmp("purchase_criteria_set").hide();
                            Ext.getCmp("purchase_criteria_set").setDisabled(true);

                            Ext.getCmp("gift").show();
                            Ext.getCmp("gift").setDisabled(false);

                            Ext.getCmp("purchase_earn_point").hide();
                            Ext.getCmp("purchase_earn_point").setDisabled(true);

                            Ext.getCmp("registration").setDisabled(false);
                            Ext.getCmp("registration").show();

                            Ext.getCmp("referral").setDisabled(false);
                            Ext.getCmp("referral").show();

                            Ext.getCmp("earn_set").show();
                            Ext.getCmp("earn_set").setDisabled(false);

                            Ext.getCmp("comp_earn_point").show();
                            Ext.getCmp("comp_earn_point").setDisabled(false);

                            Ext.getCmp("service_criteria_set").hide();
                            Ext.getCmp("service_criteria_set").setDisabled(false);
                        }
                        else if (theValue == 4) // Service Payment
                        {
                            Ext.getCmp("transaction_criteria").show();
                            Ext.getCmp("transaction_criteria").setDisabled(false);

                            Ext.getCmp("service_criteria_set").show();
                            Ext.getCmp("service_criteria_set").setDisabled(false);

                            // --
                            Ext.getCmp("purchase_criteria_set").hide();
                            Ext.getCmp("purchase_criteria_set").setDisabled(true);

                            Ext.getCmp("choose_purchase_product_type").hide();
                            Ext.getCmp("choose_purchase_product_type").setDisabled(true);

                            Ext.getCmp("purchase_earn_point").show();
                            Ext.getCmp("purchase_earn_point").setDisabled(false);

                            Ext.getCmp("comp_earn_point").hide();
                            Ext.getCmp("comp_earn_point").setDisabled(true);

                            Ext.getCmp("gift").show();
                            Ext.getCmp("gift").setDisabled(false);

                            Ext.getCmp("registration").setDisabled(false);
                            Ext.getCmp("registration").show();

                            Ext.getCmp("referral").setDisabled(false);
                            Ext.getCmp("referral").show();


                            Ext.getCmp("earn_set").show();
                            Ext.getCmp("earn_set").setDisabled(false);

                            Ext.getCmp("redeem_discount").hide();
                            Ext.getCmp("redeem_discount").setDisabled(true);
                        }
                    }
                }
            })
        ];
    }
});

Ext.define('Ext.ux.form.field.MySelect', {
    extend: 'Ext.form.FieldContainer',
    mixins: {
        field: 'Ext.form.field.Field'
    },
    alias: 'widget.mySelect',
    layout: 'hbox',
    height: 22,
    cls: 'custom_field',
    allowBlank: false,
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
                value: this.field1Value,
                valueField: 'id',
                displayField: 'value',
                typeAhead: true,
                padding: 3,
                queryMode: 'local',//remote
                emptyText: 'Please Select',
                tabIndex: this.tabIndex,
                allowBlank: this.allowBlank
            })
        ];
    }
});

Ext.define('Ext.ux.form.field.MySelect_productType', {
    extend: 'Ext.form.FieldContainer',
    mixins: {
        field: 'Ext.form.field.Field'
    },
    alias: 'widget.mySelect_productType',
    layout: 'hbox',
    height: 22,
    cls: 'custom_field',
    selectFieldID: '',
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
                id: this.selectFieldID,
                xtype: 'myCombobox',
                fieldLabel: '',
                name: this.field1Name,
                store: store,
                value: this.field1Value,
                valueField: 'id',
                displayField: 'value',
                typeAhead: true,
                padding: 3,
                queryMode: 'local',//remote
                emptyText: 'Please Select',
                tabIndex: this.tabIndex
               
            }),
            {
                xtype: 'button',
                text: 'Add',
                padding: '3 0 0 0',
                margin: '6',
                handler: this.addDetailRow
            }
        ];
    },
    addDetailRow: function(e,b){
        var me=this;
        var fieldSet = Ext.getCmp("purchase_criteria_set");
        var now = new Date();
        var newID = now.getHours() + '' + now.getMinutes() + '' + now.getSeconds() + '' + now.getMilliseconds();
        var comp_id = "product_comp_" + newID;

      

        var selectName = '';
        var selectEmptyText = '';
        var selectDatasource = '';
        var selectValue = '';

        var type_value = parseInt(Ext.getCmp("purchase_product_type_select").getValue());

        if (type_value == 1) //productcategory
        {
            selectEmptyText = 'Select Category';
            selectDatasource = "../Table/GetListItems/productcategory";

        } else if (type_value == 2) //product
        {  
            selectEmptyText = 'Select Product';
            selectDatasource = "../Table/GetListItems/product";
        }

        var item1 = {};

        if (type_value == 1 || type_value == 2)
        {
            item1 = {
                xtype: 'combobox',
                name: 'purchase_target_id',
                padding: '3 0 0 0',
                plugins: ['clearbutton'],
                displayField: 'value',
                valueField: 'id',
                emptyText: selectEmptyText,
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
                        url: selectDatasource,
                        reader: {
                            type: 'json',
                            root: 'data'
                        }
                    }
                }),
                queryMode: 'local',
                value: selectValue,
                width: 300
            };
        }
        else if (type_value == 3) {
            item1 = {
                xtype: 'textfield',
                name: 'purchase_target_id',
                padding: '3 0 0 0',
                width: 300,
                emptyText: 'Any Product',
                readOnly: true
            };
        } else
            Ext.Msg.alert("Waring","Please select Product Type");

        
        if (type_value == 1 || type_value == 2 || type_value == 3) {
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
                    {
                        xtype: 'hidden',
                        name: 'purchase_target_type',
                        value: type_value.toString()
                    },

                   item1,
                   {
                       xtype: 'combobox',
                       name: 'purchase_target_criteria',
                       padding: '3 0 0 0',
                       plugins: ['clearbutton'],
                       displayField: 'value',
                       valueField: 'id',
                       emptyText: 'Select Criteria',
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
                               url: '../PromotionRule/GetPurchaseProductCriteria',
                               reader: {
                                   type: 'json',
                                   root: 'data'
                               }
                           }
                       }),
                       queryMode: 'local',
                       value: selectValue,
                       width: 150
                   },
                   {
                       xtype: 'textfield',
                       name: 'purchase_target_value',
                       padding: '3 0 0 0',
                       width: 100,
                       value: '',
                       emptyText: "at least",
                       regex: /^\d{0,4}(\.\d{0,2}){0,1}$/,
                       allowBlank: false
                   },
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
    }
});

Ext.define('Ext.ux.form.field.MyAddService', {
    extend: 'Ext.form.FieldContainer',
    mixins: {
        field: 'Ext.form.field.Field'
    },
    alias: 'widget.myAddService',
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
        var fieldSet = Ext.getCmp("service_criteria_set");
        var now = new Date();
        var newID = now.getHours() + '' + now.getMinutes() + '' + now.getSeconds() + '' + now.getMilliseconds();
        var comp_id = "service_comp_" + newID;

        var selectName = '';
        var selectEmptyText = 'Select Service';
        var selectDatasource = "../Table/GetListItems/servicecategory";
        var selectValue = '';

        var item1 = {};
        item1 = {
            xtype: 'combobox',
            name: 'service_target_id',
            padding: '3 0 0 0',
            plugins: ['clearbutton'],
            displayField: 'value',
            valueField: 'id',
            emptyText: selectEmptyText,
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
                    url: selectDatasource,
                    reader: {
                        type: 'json',
                        root: 'data'
                    }
                }
            }),
            queryMode: 'local',
            value: selectValue,
            width: 300,
       
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
                {
                    xtype: 'combobox',
                    name: 'service_target_criteria',
                    padding: '3 0 0 0',
                    plugins: ['clearbutton'],
                    displayField: 'value',
                    valueField: 'id',
                    emptyText: 'Select Criteria',
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
                            url: '../PromotionRule/GetServicePaymentCriteria',
                            reader: {
                                type: 'json',
                                root: 'data'
                            }
                        }
                    }),
                    queryMode: 'local',
                    value: selectValue,
                    width: 150
                },
                {
                    xtype: 'textfield',
                    name: 'service_target_value',
                    padding: '3 0 0 0',
                    width: 100,
                    value: '',
                    emptyText: "at least",
                    regex: /^\d{0,4}(\.\d{0,2}){0,1}$/,
                    allowBlank: false
                },
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

Ext.define('Ext.ux.form.field.MyRadio', {
    extend: 'Ext.form.FieldContainer',
    mixins: {
        field: 'Ext.form.field.Field'
    },
    alias: 'widget.myRadio',
    layout: 'hbox',
    cls: 'custom_field',
    checked: false,
    field1ID: null,
    combineErrors: true,
    msgTarget: 'side',
    initComponent: function () {
        var me = this;
        me.buildField();
        me.callParent();
        me.submitValue = false;
        //  this.textfield = this.down('textfield');
        //        this.checkbox = this.down('checkbox');
        me.initField();
    },
    buildField: function () {
        var me = this;
       
        this.items = [
	        Ext.apply({
	            xtype: 'radiofield',
	            padding: 3,
	            id: this.field1ID,
	            name: this.name,
	            inputValue: this.value,
	            tabIndex: this.tabIndex,
	            checked: this.checked,
	            listeners: {
	                click: {
	                    element: 'el', //bind to the underlying el property on the panel
	                    fn: function ()
	                    {
	                        Ext.getCmp(me.field1ID).setRawValue(true);

	                        // de-select referral
	                        Ext.getCmp('referral_radio').setRawValue(false);
	                        Ext.getCmp('referral_select').setDisabled(true);

	                        // de-select birthday
	                        Ext.getCmp('birthday_radio').setRawValue(false);
	                        Ext.getCmp('birthday_select').setDisabled(true);
	                    }
	                }
	            }
	        })
        ];

    },
    setfield1Name: function (value) {
        this.field1Name = value;
    },
 
    setfield1Value: function (value) {
        this.field1Value = value;
    }
});


Ext.define('Ext.ux.form.field.MyRadioRegistration', {
    extend: 'Ext.form.FieldContainer',
    mixins: {
        field: 'Ext.form.field.Field'
    },
    alias: 'widget.myRadioRegistration',
    layout: 'hbox',
    cls: 'custom_field',
    checked: false,
    field1ID: null,
    combineErrors: true,
    msgTarget: 'side',
    initComponent: function () {
        var me = this;
        me.buildField();
        me.callParent();
        me.submitValue = false;
        //  this.textfield = this.down('textfield');
        //        this.checkbox = this.down('checkbox');
        me.initField();
    },
    buildField: function () {
        var me = this;

        this.items = [
	        Ext.apply({
	            xtype: 'radiofield',
	            padding: 3,
	            id: this.field1ID,
	            name: this.name,
	            inputValue: this.value,
	            tabIndex: this.tabIndex,
	            checked: this.checked,
	            listeners: {
	                click: {
	                    element: 'el', //bind to the underlying el property on the panel
	                    fn: function () {
	                        Ext.getCmp(me.field1ID).setRawValue(true);

	                        // de-select referral
	                        Ext.getCmp('referral_radio').setRawValue(false);
	                        Ext.getCmp('referral_select').setDisabled(true);

	                        // de-select birthday
	                        Ext.getCmp('birthday_radio').setRawValue(false);
	                        Ext.getCmp('birthday_select').setDisabled(true);
	                    }
	                }
	            }
	        }),
            {
                xtype: 'textfield',
                name: 'new_registration_value',
                padding: '3 0 0 0',
                width: 200,
                value: '',
                emptyText: "registration after no of day",
                regex: /^\d{0,4}(\.\d{0,2}){0,1}$/,
                allowBlank: false
            }
        ];

    },
    setfield1Name: function (value) {
        this.field1Name = value;
    },

    setfield1Value: function (value) {
        this.field1Value = value;
    }
});

Ext.define('Ext.ux.form.field.MyRadioWithSelect', {
    extend: 'Ext.form.FieldContainer',
    mixins: {
        field: 'Ext.form.field.Field'
    },
    alias: 'widget.myRadioWithSelect',
    layout: 'hbox',
    cls: 'custom_field',
    height: 22,
    
    id: null,
    field1Name: null,
    field2Name: null,
    field1Value: null,
    field2Value: null,
    field1ID: null,
    field2ID: null,

    combineErrors: true,
    msgTarget: 'side',
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
        }, true, this.field2Value);

        this.items = [
	        Ext.apply({
	            xtype: 'radiofield',
	            id: this.field1ID,
	            padding: 3,
	            name: this.field1Name,
	            inputValue: this.field1Value,
	            tabIndex: this.tabIndex,

	            listeners: {
	                click: {
	                    element: 'el', //bind to the underlying el property on the panel
	                    fn: function (radio, checked) {
	                        
	                        var clickedRadio = this;

	                        if (clickedRadio.id == "referral_radio")
	                        {
	                            Ext.getCmp('referral_radio').setRawValue(true);
	                            Ext.getCmp('referral_select').setDisabled(false);

	                            // de-select birthday
	                            Ext.getCmp('birthday_radio').setRawValue(false);
	                            Ext.getCmp('birthday_select').setDisabled(true);

	                            // de-select registration
	                            Ext.getCmp('registration_radio').setRawValue(false);
	                        } else if (clickedRadio.id == "birthday_radio") {
	                            Ext.getCmp('birthday_radio').setRawValue(true);
	                            Ext.getCmp('birthday_select').setDisabled(false);

	                            // de-select referral
	                            Ext.getCmp('referral_radio').setRawValue(false);
	                            Ext.getCmp('referral_select').setDisabled(true);

	                            // de-select registration
	                            Ext.getCmp('registration_radio').setRawValue(false);
	                        }  
	                    }
	                }
	            }
	        }),
            Ext.apply({
                xtype: 'myCombobox',
                fieldLabel: '',
                id: this.field2ID,
                name: this.field2Name,
                store: store,
                value: this.field2Value,
                valueField: 'id',
                displayField: 'value',
                typeAhead: true,
                padding: 3,
                queryMode: 'local',
                emptyText: 'Please Select',
                tabIndex: this.tabIndex,
                disabled: true,
                allowBlank: false
            })
        ];

    },
    setfield1Name: function (value) {
        this.field1Name = value;
    },
    setfield2Name: function (value) {
        this.field2Name = value;
    },
    setfield1Value: function (value) {
        this.field1Value = value;
    },
    setfield2Value: function (value) {
        this.field2Value = value;
    }
});

Ext.define('Ext.ux.form.field.MyMultiSelect', {
    extend: 'Ext.form.FieldContainer',
    mixins: {
        field: 'Ext.form.field.Field'
    },
    alias: 'widget.myMultiSelect',
    layout: 'hbox',
    height: 22,
    multiSelect: true,
    displayField: 'value',
    valueField: 'id',
    forceSelection: true,
    
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
                url: this.datasource,
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
            fieldLabel: this.fieldLabel,
            labelStyle: this.labelCss,
            name: "",
            multiSelect: true,
            allowBlank: this.allowBlank,
            tabIndex: 0,
            init_value: 0,
            value: "",
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
        this.items = [
            Ext.apply({
                xtype: 'myCombobox',
                fieldLabel: '',
                name: this.field1Name,
                store: multstore,
                value: this.field1Value,
                valueField: 'id',
                displayField: 'value',
                typeAhead: true,
                padding: 3,
                queryMode: 'local',//remote
                emptyText: 'Please Select',
                tabIndex: this.tabIndex
            })
        ];
    }
});

Ext.define('com.palmary.promotionrule.js.insert', {
    gridPanel: undefined,
    addTag: function () {
        new com.embraiz.tag().openNewTag('rule:501', 'Rule:Add', 'com.palmary.promotionrule.js.insert', 'iconRole16');
    },
    delRole: function () {
        new com.embraiz.tag().openNewTag('rule:501', 'Rule:Add', 'com.palmary.promotionrule.js.insert', 'iconRole16');
    },
    initTag: function (tab, url, title) {

       // Check user seesion 
        checkSession();

        target_div = document.createElement("div");
        tab.getEl().dom.lastChild.appendChild(target_div);
        grider_div = document.createElement("div");
        tab.getEl().dom.lastChild.appendChild(grider_div);

        Ext.Ajax.request({
            url: '../PromotionRule/CreateForm',
            success: function (o) {
                var data_json = Ext.decode(o.responseText);

                // start to generate form
                var form_content = [];

                form_content.push({
                    id: "ruleType",
                    fieldLabel: "Rule Type",
                    xtype: "select_ruleType",
                    name: "type",
                    value: "",
                    rowspan: 1,
                    colspan: 2,
                    datasource: "../PromotionRule/GetRuleType",
                    height: 30
                });

                form_content.push(
                    {
                        id: "rule_name",
                        fieldLabel : "Rule Name",
                        xtype : "textfield",
                        name: "name",
                        value : "",
                        rowspan : 1,
                        colspan: 2,
                        allowBlank: false
                    }
                );
                
               
                form_content.push({
                    id: "start_date",
                    fieldLabel: "Start Date",
                    xtype : "dateTime",
                    name : "start_date",
                    value : "",
                    rowspan : 1,
                    colspan : 2,
                    datasource: "../PromotionRule/GetRuleType",
                    timeMinValue: "12:00 AM",
                    timeMaxValue: "11:45 PM",
                    timeSelectValue: "00:00"
                });
              

                form_content.push({
                    id: "end_date",
                    fieldLabel: "End Date",
                    xtype : "dateTime",
                    name : "end_date",
                    value : "",
                    rowspan : 1,
                    colspan : 2,
                    datasource: "../PromotionRule/GetRuleType",
                    timeMinValue: "12:00 AM",
                    timeMaxValue: "11:45 PM",
                    timeSelectValue: "00:00"
                });
                
             

                
                
                form_content.push({
                    id : "transaction_criteria",
                    name : "transaction_criteria",
                    fieldLabel : "Transaction Criteria",
                    xtype : "mySelect",
                    value : "",
                    rowspan : 1,
                    colspan : 2,
                    datasource: "../PromotionRule/GetTransactionType",
                    allowBlank: false,
                    height: 30
                });
                
                var member_level_store = Ext.create('Ext.data.Store', {
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
                        url: "../Table/GetListItems/MemberLevel",
                        reader: {
                            type: 'json',
                            root: 'data',
                            totalProperty: 'totalCount'

                        }
                    }
                });
                member_level_store.on('beforeload', function (thiz, op) {
                    Ext.getBody().mask();
                });
                member_level_store.on('load', function (thiz, op, success) {
                    setTimeout(function a() { Ext.getBody().unmask(); }, 1000);
                });
               
              
                
             
                var multstore_mc = Ext.create('Ext.data.Store', {
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
                        url: "../Table/GetListItems/MemberCategory",
                        reader: {
                            type: 'json',
                            root: 'data',
                            totalProperty: 'totalCount'
                        }
                    }
                });
                multstore_mc.on('beforeload', function (thiz, op) {
                    Ext.getBody().mask();
                });
                multstore_mc.on('load', function (thiz, op, success) {
                    setTimeout(function a() { Ext.getBody().unmask(); }, 1000);
                });

              

                //form_content.push({
                //    name: "conjunction",
                //    fieldLabel: "Hit in conjunction with other rules",
                //    xtype: "mySelect",
                //    value: "",
                //    rowspan: 1,
                //    colspan: 2,
                //    datasource: "../Table/GetListItems/YesNo",
                //    allowBlank: false,
                //    height: 30
                //});

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

                var member_level = {
                    id: "member_level",
                    fieldLabel: "Member Level",
                    xtype: "combofieldbox",
                    name: "member_level",
                    multiSelect: true,
                    allowBlank: false,
                    //labelStyle: 'color:red',
                    tabIndex: '',
                    init_value: [-1,1,2,3,4],
                    value: '',
                    emptyText: 'Please Select',
                    displayField: 'value',
                    valueField: 'id',
                    forceSelection: true,
                    store: member_level_store,
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

                var member_category = {
                    id: "member_category",
                    fieldLabel: "Member Category",
                    xtype: "combofieldbox",
                    name: "member_category",
                    multiSelect: true,
                    allowBlank: true,
                    //labelStyle: 'color:red',
                    tabIndex: '',
                    init_value: [5,6,7,8,9,10],
                    value: '',
                    emptyText: 'Please Select',
                    displayField: 'value',
                    valueField: 'id',
                    forceSelection: true,
                    store: multstore_mc,
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

                //var member_gender = {
                //    id: "member_gender",
                //    fieldLabel: "Member Gender",
                //    xtype: "mySelect",
                //    name: "member_gender",
                //    value: "",
                //    rowspan: 1,
                //    colspan: 2,
                //    datasource: "../Table/GetListItems/Gender",
                //    allowBlank: true,
                //    height: 30
                //};

                //var member_ageRange = {
                //    id: "member_ageRange",
                //    fieldLabel: "Age Range",
                //    xtype: "input_range",
                //    name: "member_ageRange",
                //    value: "",
                //    rowspan: 1,
                //    colspan: 2,
                //    height: 30
                //};

                

                //var member_custom_group_store = Ext.create('Ext.data.Store', {
                //    fields: [{
                //        name: 'id',
                //        type: 'string'
                //    }, {
                //        name: 'value',
                //        type: 'string'
                //    }],
                //    autoLoad: true,
                //    proxy: {
                //        type: 'ajax',
                //        url: "../Table/GetListItems/MemberCustomGroup",
                //        reader: {
                //            type: 'json',
                //            root: 'data',
                //            totalProperty: 'totalCount'

                //        }
                //    }
                //});
                //member_custom_group_store.on('beforeload', function (thiz, op) {
                //    Ext.getBody().mask();
                //});
                //member_custom_group_store.on('load', function (thiz, op, success) {
                //    setTimeout(function a() { Ext.getBody().unmask(); }, 1000);
                //});


                //var member_custom_group = {
                //    id: "member_custom_group",
                //    fieldLabel: "Member Custom Group",
                //    xtype: "combofieldbox",
                //    name: "member_custom_group",
                //    multiSelect: true,
                //    allowBlank: true,
                //    //labelStyle: 'color:red',
                //    tabIndex: '',
                //    init_value: '',
                //    value: '',
                //    emptyText: 'Please Select',
                //    displayField: 'value',
                //    valueField: 'id',
                //    forceSelection: true,
                //    store: member_custom_group_store,
                //    queryMode: 'local',
                //    cls: 'readonlyField',
                //    listeners: {
                //        render: function () {
                //            var me = this;
                //            var initValueArray = undefined;
                //            if (me.init_value != undefined && me.init_value != '' && me.init_value != null && me.init_value != 'null') {
                //                var initValue = me.init_value;
                //                if (Ext.encode(initValue).indexOf('"') != -1) {
                //                    initValueArray = initValue;
                //                } else {
                //                    for (var i = 0; i < initValue.length; i++) {
                //                        initValue[i] = String(initValue[i]);
                //                    }
                //                    initValueArray = initValue;
                //                }
                //                me.setValue(initValueArray);
                //            }
                //        },
                //        select: function () {
                //            var me = this;
                //            me.setValue(me.lastValue);
                //        }
                //    }
                //};

                var memberCriteria_fieldset = {
                    xtype: 'fieldset',
                    title: 'Member Criteria',
                    id: 'member_criteria_set',
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
                    defaults: {
                        anchor: '100%'
                    },
                    items: [member_level, member_category] //, member_gender, member_ageRange, member_custom_group]
                    
                };
                form_content.push(memberCriteria_fieldset);


                var none = {
                    id: "special_none",
                    field1ID: "special_none_radio",
                    fieldLabel: "None",
                    xtype: "myRadio",
                    name: "special_criteria_type",
                    value: "0",
                    rowspan: 1,
                    colspan: 2,
                    checked: true
                };

                var registration = {
                    id: "registration",
                    field1ID: "registration_radio",
                    fieldLabel : "New Registration",
                    xtype: "myRadioRegistration",
                    name : "special_criteria_type",
                    value: "1",
                    rowspan : 1,
                    colspan : 2
                };
         
                var birthday = {
                    id: "birthday",
                    field1ID: "birthday_radio",
                    field1Name: "special_criteria_type",
                    field1Value: "2",
                    field2ID: "birthday_select",
                    field2Name: "birthday_select",
                    fieldLabel : "Birthday",
                    xtype : "myRadioWithSelect",
                    datasource : "../PromotionRule/GetBirthdayType",
                    rowspan : 1,
                    colspan: 2,
                    height: 30
                };
                
                var referral = {
                    id: "referral",
                    name: "referral",
                    field1ID: "referral_radio",
                    field1Name: "special_criteria_type",
                    field1Value: "3",
                    field2ID: "referral_select",
                    field2Name: "referral_select",
                    fieldLabel: "Referral",
                    xtype: "myRadioWithSelect",
                    datasource: "../PromotionRule/GetEarnTarget",

                    rowspan: 1,
                    colspan: 2,
                    height: 30
                };

                var specialCriteria_fieldset = {
                    xtype: 'fieldset',
                    title: 'Special Criteria',
                    id: 'special_criteria_set',
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
                    defaults: {
                        anchor: '100%'
                    },
                    items: [none, registration, referral, birthday]
                };
                form_content.push(specialCriteria_fieldset);

                var choose_purchase_product_type = {
                    id : "choose_purchase_product_type",
                    name: "choose_purchase_product_type",
                    fieldLabel: "Purchase Type",
                    xtype: "mySelect_productType",
                    datasource: "../PromotionRule/GetPurchaseProductType",
                    value : "",
                    rowspan : 1,
                    colspan : 2,
                    selectFieldID: "purchase_product_type_select",
                    height: 30
                };

                var purchase_fieldset = {
                    xtype: 'fieldset',
                    title: 'Purchase Criteria',
                    id: 'purchase_criteria_set',
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
                    items: [choose_purchase_product_type]
                };
                form_content.push(purchase_fieldset);

                var service_fieldset_additem = {
                    id: "service_fieldset_additem",
                    name: "service_fieldset_additem",
                    fieldLabel: "Service Type",
                    xtype: "myAddService",
                    value: "",
                    rowspan: 1,
                    colspan: 2,
                    height: 30
                };

                var tran_custom_group_store = Ext.create('Ext.data.Store', {
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
                        url: "../Table/GetListItems/TransactionCustomGroup",
                        reader: {
                            type: 'json',
                            root: 'data',
                            totalProperty: 'totalCount'

                        }
                    }
                });
                tran_custom_group_store.on('beforeload', function (thiz, op) {
                    Ext.getBody().mask();
                });
                tran_custom_group_store.on('load', function (thiz, op, success) {
                    setTimeout(function a() { Ext.getBody().unmask(); }, 1000);
                });


                var tran_custom_group = {
                   // id: "member_custom_group",
                    colspan: 2,
                    fieldLabel: "Transaction Custom Group",
                    xtype: "combofieldbox",
                    name: "tran_custom_group",
                    multiSelect: true,
                    allowBlank: true,
                    //labelStyle: 'color:red',
                    tabIndex: '',
                    init_value: '',
                    value: '',
                    emptyText: 'Please Select',
                    displayField: 'value',
                    valueField: 'id',
                    forceSelection: true,
                    store: tran_custom_group_store,
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

                var service_fieldset = {
                    xtype: 'fieldset',
                    title: 'Service Criteria',
                    id: 'service_criteria_set',
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
                    items: [tran_custom_group, service_fieldset_additem]
                };
                form_content.push(service_fieldset);
                        
                var redeem_discount = {
                    id : "redeem_discount",
                    xtype: "textfield",
                    fieldLabel: "Redemption Discount(%)",
                    name: "redeem_discount",
                    value : "",
                    rowspan : 1,
                    colspan : 2
                };
               
                var purchase_earn_point = {
                    id : "purchase_earn_point",
                    xtype: "select_input",
                    fieldLabel: "Purchase Earn Point",
                    field1Name: "purchase_earn_point",
                    field2Name: "purchase_earn_point_value",
                    field2Regex: "numberRegex",
                    value : "",
                    rowspan : 1,
                    colspan : 2,
                    datasource: "../PromotionRule/GetEarnPointType",
                    height: 30
                };
               
                var comp_earn_point = {
                    id : "comp_earn_point",
                    name : "comp_earn_point",
                    fieldLabel : "Earn Point",
                    xtype : "textfield",
                    value : "",
                    rowspan : 1,
                    colspan: 2,
                    regex: ""
                };
                
                var gift = {
                    id : "gift",
                    xtype: "select_input",
                    fieldLabel: "Gift",
                    field1Name: "gift",
                    field2Name: "gift_quantity",
                    field2Regex: "numberRegex",
                    value : "",
                    rowspan : 1,
                    colspan : 2,
                    datasource : "../Table/GetListItems/gift",
                    field2EmptyText: "quantity",
                    height: 30
                };
                
                var earn_fieldset = {
                    xtype: 'fieldset',
                    title: 'Earn',
                    id: 'earn_set',

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
                    defaults: {
                        anchor: '100%'
                    },
                    items: [redeem_discount, purchase_earn_point, comp_earn_point, gift]
                };
                form_content.push(earn_fieldset);

                var form_items = Ext.create('Ext.container.Container', {
                    anchor: '100%',
                    id: 'myForm',
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

                var viewSimple = this.viewSimple = Ext.create('Ext.form.Panel', {
                    url: '../PromotionRule/Update',
                    title: 'Promotion Rule',
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
                            text: 'Create Rule',
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

                            //listeners: {
                            //    'click': function (c) {
                            //        alert("OK");
                            //        var el = c.getEl().child('img');
                            //    }

                            //    //afterrender: function (thiz, action) { form_layout; }
                            //}
                        })
                    ],  
                    renderTo: target_div
                });
               
                // transaction_criteria
                Ext.getCmp("transaction_criteria").hide();
                Ext.getCmp("transaction_criteria").setDisabled(true);

                // special criteria
                Ext.getCmp("registration").setDisabled(true);
                Ext.getCmp("registration").hide();

                Ext.getCmp("referral").setDisabled(true);
                Ext.getCmp("referral").hide();

                
                // purchase_criteria_set
                Ext.getCmp("choose_purchase_product_type").hide();
                Ext.getCmp("choose_purchase_product_type").setDisabled(true);

                Ext.getCmp("purchase_criteria_set").hide();
                Ext.getCmp("purchase_criteria_set").setDisabled(true);

                // service_criteria_set
                Ext.getCmp("service_criteria_set").hide();
                Ext.getCmp("service_criteria_set").setDisabled(false);

                // earn_set
                Ext.getCmp("earn_set").hide();
                Ext.getCmp("earn_set").setDisabled(true);

                Ext.getCmp("redeem_discount").hide();
                Ext.getCmp("redeem_discount").setDisabled(true);

                Ext.getCmp("gift").hide();
                Ext.getCmp("gift").setDisabled(true);

                Ext.getCmp("purchase_earn_point").hide();
                Ext.getCmp("purchase_earn_point").setDisabled(true);

                Ext.getCmp("comp_earn_point").hide();
                Ext.getCmp("comp_earn_point").setDisabled(true);

            },
            scope: this
        });
    }
});