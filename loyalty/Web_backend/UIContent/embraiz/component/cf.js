Ext.require([
    'Ext.form.*',
    'Ext.tip.QuickTipManager'
]);


Ext.define('Ext.ux.form.field.TRM_REF', {
    extend:'Ext.form.FieldContainer',
	mixins: {
        field: 'Ext.form.field.Field'
    },
	alias: 'widget.TRM_REF',
	layout: 'hbox',
    height: 22,
	cls: 'custom_field',
	field1Name: null,
	field2Name: null,
	field1Value: null,
	field2Value: null,
    combineErrors: true,
    msgTarget :'side',
	initComponent: function() {
        var me = this;
		me.buildField();
        me.callParent();
		me.submitValue = false;
        this.textfield = this.down('textfield');
        this.checkbox = this.down('checkbox');
        me.initField();
    },
	buildField: function(){
		 var me = this;
        this.items = [
            Ext.apply({
			    xtype: 'textfield',
				fieldLabel: this.inputFieldLabel,
				labelWidth: 35,
				//padding: 3,
	            padding: '3 0 0 0',
				width:105,
				value: this.field1Value,
				name:this.field1Name,
				tabIndex:this.tabIndex
	        }),
	         Ext.apply({
	            xtype: 'textfield',
				fieldLabel: this.input2FieldLabel,
				labelWidth: 30,
				//padding: 3,
	            padding: '3 0 0 0',
				width:105,
				name:this.field2Name,
				value:this.field2Value,
				tabIndex:this.tabIndex
	        })
        ]
    },
	setfield1Name: function(value){
		this.field1Name = value;
	},
	setfield2Name: function(value){
		this.field2Name = value;
	},
	setfield1Value: function(value){
		this.field1Value = value;
	},
	setfield2Value: function(value){
		this.field2Value = value;
	}
});
Ext.define('Ext.ux.form.field.CheckboxWithLabel', {
    extend:'Ext.form.FieldContainer',
	mixins: {
        field: 'Ext.form.field.Field'
    },
	alias: 'widget.checkbox_label',
	layout: 'hbox',
    height: 22,
	cls: 'custom_field',
	field1Name: null,
	field2Name: null,
	field1Value: null,
	field2Value: null,
    combineErrors: true,
    msgTarget :'side',
	initComponent: function() {
        var me = this;
        me.buildField();
        me.callParent();
		me.submitValue = false;
        this.textfield = this.down('textfield');
        this.checkbox = this.down('checkbox');
        me.initField();
    },
	buildField: function(){
        this.items = [
            Ext.apply({
	            xtype: 'checkbox',
				name:this.field2Name,
	            padding: '3 0 0 0',
				checked: this.field2Value,
				uncheckedValue:'0',
				inputValue:'1',
				tabIndex:this.tabIndex
	        }),
	        Ext.apply({
			    xtype: 'label',
				text: this.field1Value,
				name:this.field1Name,
	            padding: '3 0 0 0',
				tabIndex:this.tabIndex
	        })
        ]
    },
	setfield1Name: function(value){
		this.field1Name = value;
	},
	setfield2Name: function(value){
		this.field2Name = value;
	},
	setfield1Value: function(value){
		this.field1Value = value;
	},
	setfield2Value: function(value){
		this.field2Value = value;
	}
});
Ext.define('Ext.ux.form.field.FirstNameWithInput', {
    extend:'Ext.form.FieldContainer',
	mixins: {
        field: 'Ext.form.field.Field'
    },
	alias: 'widget.firstname_input',
	layout: 'hbox',
    height: 22,
	cls: 'custom_field',
	field1Name: null,
	field2Name: null,
	field1Value: null,
	field2Value: null,
    combineErrors: true,
    msgTarget :'side',
	initComponent: function() {
        var me = this;
		me.buildField();
        me.callParent();
		me.submitValue = false;
        this.textfield = this.down('textfield');
        this.checkbox = this.down('checkbox');
        me.initField();
    },
	buildField: function(){
		 var me = this;
		var store= Ext.create('Ext.data.Store', {
					fields: [{
                                    name: 'id',
                                    type: 'string'
                                }, {
                                    name: 'value',
                                    type: 'string'
                             }],
					data   : [
								{id : 'Mr',   value: 'Mr'},
								{id : 'Mrs',  value: 'Mrs'},
								{id : 'Miss', value: 'Miss'},
								{id : 'Dr', value: 'Dr'},
								{id : 'Prof', value: 'Prof'}
							]
				});/*Ext.create('Ext.data.Store', {
			 model: 'date',
			 data : [
				 {id: 'Ed',    value: 'Spencer'},
				 {id: 'Tommy', value: 'Maintz'}
			 ]
		 });*/
        this.items = [
            Ext.apply({
				xtype: 'combobox',
				fieldLabel: '',
				name: this.field1Name,
				store:store,
				padding: 3,
				width:50,
				value:this.field1Value,
				valueField: 'id',
				displayField: 'value',
				typeAhead: true,
				queryMode: 'local',//remote ??ε??????ajax
				emptyText: 'Please Select',
				tabIndex:this.tabIndex
	        }),
	         Ext.apply({
	            xtype: 'textfield',
				flex:1,
				padding: 3,
				name:this.field2Name,
				value:this.field2Value,
				tabIndex:this.tabIndex
	        })
        ]
    },
	setfield1Name: function(value){
		this.field1Name = value;
	},
	setfield2Name: function(value){
		this.field2Name = value;
	},
	setfield1Value: function(value){
		this.field1Value = value;
	},
	setfield2Value: function(value){
		this.field2Value = value;
	}
});

Ext.define('Ext.ux.form.field.SelectWithInput', {
    extend:'Ext.form.FieldContainer',
	mixins: {
        field: 'Ext.form.field.Field'
    },
	alias: 'widget.select_input',
	layout: 'hbox',
    height: 22,
	cls: 'custom_field',
	field1Name: null,
	field2Name: null,
	field1Value: null,
	field2Value: null,
    combineErrors: true,
    msgTarget :'side',
	initComponent: function() {
        var me = this;
		me.buildField();
        me.callParent();
		me.submitValue = false;
        this.textfield = this.down('textfield');
        this.checkbox = this.down('checkbox');
        me.initField();
    },
	buildField: function(){
		 var me = this;
		var store=Ext.create('Ext.data.Store', {
					fields: [{
                                    name: 'id',
                                    type: 'string'
                                }, {
                                    name: 'value',
                                    type: 'string'
                                }],
					remoteSort: true,
					autoLoad: true,
					proxy : {
							type: 'ajax',
							url: this.datasource,
							reader: {
								type: 'json',
								root: 'data'
							}
						}
				});
        this.items = [
            Ext.apply({
				xtype: 'combobox',
				fieldLabel: '',
				name: this.field1Name,
				store:store,
				value:this.field1Value,
				valueField: 'id',
				displayField: 'value',
				typeAhead: true,
				padding:3,
				queryMode: 'local',//remote ??ε??????ajax
				emptyText: 'Please Select',
				tabIndex:this.tabIndex
	        }),
	        Ext.apply({
	            xtype: 'textfield',
				padding:8,
				name:this.field2Name,
				value:this.field2Value,
				tabIndex:this.tabIndex
	        })
        ]
    },
	setfield1Name: function(value){
		this.field1Name = value;
	},
	setfield2Name: function(value){
		this.field2Name = value;
	},
	setfield1Value: function(value){
		this.field1Value = value;
	},
	setfield2Value: function(value){
		this.field2Value = value;
	}
});

Ext.define('Ext.ux.form.field.DateWithTime', {
    extend:'Ext.form.FieldContainer',
	mixins: {
        field: 'Ext.form.field.Field'
    },
	alias: 'widget.dateTime',
	layout: 'column',
	cls: 'custom_field',
    height: 22,
	field1Name: null,
	field2Name: null,
	field1Value: null,
	field2Value: null,
	name:null,
	value:null,
    combineErrors: true,
    msgTarget :'side',
	initComponent: function() {
        var me = this;
        me.buildField();
        me.callParent();
		me.submitValue = false;
        this.textfield = this.down('textfield');
        this.checkbox = this.down('checkbox');
        me.initField();
    },
	buildField: function(){
    	var time=Ext.Date.parse('00:00','H:i');
    	if(this.field2Value!=undefined&&this.field2Value!=''){
    		time=Ext.Date.parse(this.field2Value,'H:i');
    	}
        this.items = [
            Ext.apply({
	            xtype: 'datefield',
				name:this.field1Name,
				format : 'Y-m-d',
				altFormats : 'ymd|Ymd|Y-m-d|Y.m.d|y.m.d',
				padding:'3,0,0,0',
				width:100,
				value: this.field1Value,
				tabIndex:this.tabIndex,
				listeners:{
                	change:function(e,newValue,oldValue){
            			e.ownerCt.items.items[2].setValue(e.ownerCt.items.items[0].getRawValue()+' '+e.ownerCt.items.items[1].getRawValue());
                     }
                 }
	        }),
	        Ext.apply({
	           xtype: 'timefield',
	            fieldLabel: 'Time',
	            padding: '3,0,0,0',
				name:this.field2Name,
				width:100,
				labelWidth:30,
				value: time,
				xtype: 'timefield',
				increment: 15,
				format:'H:i',
				minValue: '00:00 AM',
				maxValue: '24:00 PM',
				tabIndex:this.tabIndex,
				listeners:{
                	change:function(e,newValue,oldValue){
	        		 e.ownerCt.items.items[2].setValue(e.ownerCt.items.items[0].getRawValue()+' '+e.ownerCt.items.items[1].getRawValue());
                    }
                 }
	        }),
	        Ext.apply({
	        	    xtype: 'hiddenfield',
                    name: this.name,
                    value: this.value
            })
        ]
    },
	setfield1Name: function(value){
		this.field1Name = value;
	},
	setfield2Name: function(value){
		this.field2Name = value;
	},
	setfield1Value: function(value){
		this.field1Value = value;
	},
	setfield2Value: function(value){
		this.field2Value = value;
	}
});

Ext.define('Ext.ux.form.field.DateWithThreeInput', {
    extend:'Ext.form.FieldContainer',
	mixins: {
        field: 'Ext.form.field.Field'
    },
	alias: 'widget.dateTime_threeInput',
	layout: 'hbox',
    height: 22,
	cls: 'custom_field',
	field1Name: null,
	field2Name: null,
	field3Name: null,
	field1Value: null,
	field2Value: null,
	field3Value: null,
	yearRegex: /^([1-9][0-9][0-9][0-9])$/,
	monthRegex: /^([1-9]|1[0-2])$/,
	dayRegex: /^([1-9]|[1-2][0-9]|3[0-1])$/,
    combineErrors: true,
    msgTarget :'side',
	initComponent: function() {
        var me = this;
		me.buildField();
        me.callParent();
		me.submitValue = false;
        this.textfield = this.down('textfield');
        this.checkbox = this.down('checkbox');
        me.initField();
    },
		
	buildField: function(){
		var me = this;
		
		var dateCheck = function (value) {
									
									var yearObj = me.items.items[0];
									var monthObj = me.items.items[2];
									var dayObj = me.items.items[4];
									
									var y = parseInt(yearObj.getValue());
									var m = parseInt(monthObj.getValue());
									var d = parseInt(dayObj.getValue());
								
									var result = false;
									
									var date = new Date(y,m-1,d);

									if (y>0 && m>0 && d>0){//all fields have value
										if (date.getFullYear() == y && date.getMonth() + 1 == m && date.getDate() == d) {
											result = true;
										} 
									}else if (isNaN(y) && m>0 && d>0){//only month and day have value
										y = 2012; //2012-2 has 29 days
										date = new Date(y,m-1,d);
										if (date.getFullYear() == y && date.getMonth() + 1 == m && date.getDate() == d) {
											result = true;
										}
									}else if (y>0 && m>0 && isNaN(d)){
										result = true;
									}
									else if (isNaN(y) && isNaN(m) && isNaN(d)){
										result = true;
									}
									
									if (result){
										yearObj.clearInvalid();
										monthObj.clearInvalid();
										dayObj.clearInvalid();
									 	return true;
									}else{
										var errorMessage = 'Invalid Date';
										yearObj.markInvalid(errorMessage);
										monthObj.markInvalid(errorMessage);
										dayObj.markInvalid(errorMessage);
										return errorMessage;
									}
										
								}
						;
						
						
        this.items = [
            Ext.apply({
				xtype: 'textfield',
				padding:4,
				name:this.field1Name,
				value:this.field1Value,
                allowBlank: true,
				regex : this.yearRegex,
				regexText: 'Invalid Year',
				tabIndex:this.tabIndex,
				width:50,
				validator: dateCheck
	        }),
			Ext.apply({
                    xtype:'label',
                    text: "-",
                    style: {
								margin: '10px 0px 0px 0px'
							},
                    labelStyle: 'font-weight:bold; padding-top:30px'
                 }),
	        Ext.apply({
	            xtype: 'textfield',
				padding:4,
				name:this.field2Name,
				value:this.field2Value,
				regex : this.monthRegex,
				regexText: 'Invalid Month',
				tabIndex:this.tabIndex,
				width:30,
				validator: dateCheck
	        }),
			Ext.apply({
                    xtype:'label',
                    text: "-",
					style: {
								margin: '10px 0px 0px 0px'
							},
                    labelStyle: 'font-weight:bold;'
			}),
	        Ext.apply({
	            xtype: 'textfield',
				padding:4,
				name:this.field3Name,
				value:this.field3Value,
				regex : this.dayRegex,
				regexText: 'Invalid Day',
				tabIndex:this.tabIndex,
                width:30,
				listeners : {
								//blur: dateCheck
								
							},
				validator: dateCheck
	        })
        ]
    },
	setfield1Name: function(value){
		this.field1Name = value;
	},
	setfield2Name: function(value){
		this.field2Name = value;
	},
	setfield3Name: function(value){
		this.field3Name = value;
	},
	setfield1Value: function(value){
		this.field1Value = value;
	},
	setfield2Value: function(value){
		this.field2Value = value;
	},
	setfield3Value: function(value){
		this.field3Value = value;
	}
});

Ext.define('Ext.ux.form.field.input_range', {
    extend: 'Ext.form.FieldContainer',
    mixins: {
        field: 'Ext.form.field.Field'
    },
    alias: 'widget.input_range',
    layout: 'hbox',
    height: 22,
    cls: 'custom_field',
    field1Name: null,
    field2Name: null,
    
    field1Value: null,
    field2Value: null,

    customRegex: null,
    
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
        var rangeCheck = function (value) {

            

            var lowerLimitObj = me.items.items[0];
            var upperLimitObj = me.items.items[2];

            var lower = parseFloat(lowerLimitObj.getValue());
            var upper = parseFloat(upperLimitObj.getValue());

            if (!isNaN(lower) && !isNaN(upper))
            {
                var result = false;
                
                if (upper >= lower)
                    result = true;

                if (result) {
                    
                    lowerLimitObj.clearInvalid();
                    upperLimitObj.clearInvalid();

                    return true;
                } else {
                    var errorMessage = 'Invalid Number Range';
                    lowerLimitObj.markInvalid(errorMessage);
                    upperLimitObj.markInvalid(errorMessage);

                    return errorMessage;
                }
            }
            return true;
        };

        this.items = [
            Ext.apply({
                xtype: 'textfield',
                padding: 4,
                name: this.field1Name,
                value: this.field1Value,
                allowBlank: true,
                regex: this.customRegex,
                regexText: 'Invalid Lower Limit',
                tabIndex: this.tabIndex,
                width: 50,
                validator: rangeCheck
            }),
			Ext.apply({
			    xtype: 'label',
			    text: "-",
			    style: {
			        margin: '10px 0px 0px 0px'
			    },
			    labelStyle: 'font-weight:bold; padding-top:30px'
			}),
	        Ext.apply({
	            xtype: 'textfield',
	            padding: 4,
	            name: this.field2Name,
	            value: this.field2Value,
	            regex: this.customRegex,
	            regexText: 'Invalid Upper Limit',
	            tabIndex: this.tabIndex,
	            width: 50,
	            validator: rangeCheck
	        })
        ]
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
    },
});


Ext.define('Ext.ux.form.field.SelectWithSelect', {
    extend:'Ext.form.FieldContainer',
	mixins: {
        field: 'Ext.form.field.Field'
    },
	alias: 'widget.select_select',
	layout: 'hbox',
    height: 22,
	cls: 'custom_field',
	field1Name: null,
	field2Name: null,
	field1Value: null,
	field2Value: null,
    combineErrors: true,
    msgTarget :'side',
	initComponent: function() {
        var me = this;
		me.buildField();
        me.callParent();
		me.submitValue = false;
        this.textfield = this.down('textfield');
        this.checkbox = this.down('checkbox');
        me.initField();
    },
	buildField: function(){
		 var me = this;
		var store=Ext.create('Ext.data.Store', {
					fields: [{
                                    name: 'id',
                                    type: 'string'
                                }, {
                                    name: 'value',
                                    type: 'string'
                                }],
					remoteSort: true,
					autoLoad: true,
					proxy : {
							type: 'ajax',
							url: this.datasource,
							reader: {
								type: 'json',
								root: 'data'
							}
						}
				});
		var store2=Ext.create('Ext.data.Store', {
					fields: [{
                                    name: 'id',
                                    type: 'string'
                                }, {
                                    name: 'value',
                                    type: 'string'
                                }],
					remoteSort: true,
					autoLoad: true,
					proxy : {
							type: 'ajax',
							url: this.selectDatasource,
							reader: {
								type: 'json',
								root: 'data'
							}
						}
				});
		/*store2.on('beforeload', function(thiz,options) {
	 			thiz.proxy.extraParams.itemid='';
	 			}); */
        this.items = [
            Ext.apply({
				xtype: 'combobox',
				fieldLabel: '',
				name: this.field1Name,
				store:store,
				padding:3,
				width: 120,
				value:this.field1Value,
				valueField: 'id',
				displayField: 'value',
				typeAhead: true,
				queryMode: 'local',//remote ??ε??????ajax
				emptyText: 'Please Select',
				tabIndex:this.tabIndex
	        }),
	        Ext.apply({
	           xtype: 'combobox',
				fieldLabel: this.selectFieldLabel,
				name: this.field2Name,
				store: store2,
				value:this.field2Value,
				padding:'3,0,0,0',
				valueField: 'id',
				labelWidth: 45,
				width: 165,
				displayField: 'value',
				typeAhead: true,
				queryMode: 'local',
				emptyText: 'Please Select',
				tabIndex:this.tabIndex
	        })
        ]
    },
	setfield1Name: function(value){
		this.field1Name = value;
	},
	setfield2Name: function(value){
		this.field2Name = value;
	},
	setfield1Value: function(value){
		this.field1Value = value;
	},
	setfield2Value: function(value){
		this.field2Value = value;
	}
});

Ext.define('Ext.ux.form.field.InputWithCheckbox', {
    extend:'Ext.form.FieldContainer',
	mixins: {
        field: 'Ext.form.field.Field'
    },
	alias: 'widget.inputwithcheckbox',
	layout: 'hbox',
    height: 22,
	cls: 'custom_field',
	field1Name: null,
	field2Name: null,
	field1Value: null,
	field2Value: null,
    combineErrors: true,
    msgTarget :'side',
	initComponent: function() {
        var me = this;
        me.buildField();
        me.callParent();
		me.submitValue = false;
        this.textfield = this.down('textfield');
        this.checkbox = this.down('checkbox');
        me.initField();
    },
	buildField: function(){
        this.items = [
            Ext.apply({
	            xtype: 'textfield',
				flex:1,
				name:this.field1Name,
				tabIndex:this.tabIndex
	        }),
	        Ext.apply({
	            xtype: 'checkbox',
	            padding: 3,
				name:this.field2Name,
				inputValue:'1',
				tabIndex:this.tabIndex
	        })
        ]
    },
	setfield1Name: function(value){
		this.field1Name = value;
	},
	setfield2Name: function(value){
		this.field2Name = value;
	},
	setfield1Value: function(value){
		this.field1Value = value;
	},
	setfield2Value: function(value){
		this.field2Value = value;
	}
});

Ext.define('Ext.ux.form.field.DateWithCheckbox', {
    extend:'Ext.form.FieldContainer',
	mixins: {
        field: 'Ext.form.field.Field'
    },
	alias: 'widget.date_check',
	layout: 'column',
    height: 22,
	cls: 'custom_field',
	field1Name: null,
	field2Name: null,
	field1Value: null,
	field2Value: null,
	hand:null,
    combineErrors: true,
    msgTarget :'side',
	initComponent: function() {
        var me = this;
        me.buildField();
        me.callParent();
        if(this.hand!=null&&this.hand!=undefined){
        	me.items.items[0].addListener('change',this.hand,false);
        }
		me.submitValue = false;
        this.textfield = this.down('textfield');
        this.checkbox = this.down('checkbox');
        me.initField();
    },
	buildField: function(){
        this.items = [
            Ext.apply({
	            xtype: 'datefield',
				flex:1,
				name:this.field1Name,
				value: this.field1Value,
				format : 'Y-m-d',
				altFormats : 'ymd|Ymd|Y-m-d|Y.m.d|y.m.d',
				width:100,
				tabIndex:this.tabIndex
	        }),
	        Ext.apply({
	            xtype: 'checkbox',
	            padding: 3,
				name:this.field2Name,
				checked: this.field2Value,
				uncheckedValue:'0',
				cls: 'column_checkbox',
				inputValue:'1',
				tabIndex:this.tabIndex
	        })
        ]
    },
	setfield1Name: function(value){
		this.field1Name = value;
	},
	setfield2Name: function(value){
		this.field2Name = value;
	},
	setfield1Value: function(value){
		this.field1Value = value;
	},
	setfield2Value: function(value){
		this.field2Value = value;
	}
});

Ext.define('Ext.ux.form.field.DateWithDate', {
    extend:'Ext.form.FieldContainer',
	mixins: {
        field: 'Ext.form.field.Field'
    },
	alias: 'widget.dateRange',
	layout: 'column',
	cls: 'custom_field',
    height: 22,
	field1Name: null,
	field2Name: null,
	field1Value: null,
	field2Value: null,
	allowBlank:null,
	labelCss:null,
    combineErrors: true,
    msgTarget :'side',
	initComponent: function() {
        var me = this;
        me.buildField();
        me.callParent();
		me.submitValue = false;
        this.textfield = this.down('textfield');
        this.checkbox = this.down('checkbox');
        me.initField();
    },
	buildField: function(){
        this.items = [
            Ext.apply({
	            xtype: 'datefield',
				name:this.field1Name,//'startdt',
				format : 'Y-m-d',
				altFormats : 'ymd|Ymd|Y-m-d|Y.m.d|y.m.d',
				width:100,
	            padding: '3 0 0 0',
	            allowBlank:this.allowBlank,
	            labelStyle:this.labelCss,
				//padding: 3,
				//vtype: 'daterange',
				value: this.field1Value,
            	endDateField: 'enddt',
				tabIndex:this.tabIndex
	        }),
	        Ext.apply({
	            xtype: 'datefield',
	            padding: '3 0 0 0',
				name:this.field2Name,//'enddt',
				allowBlank:this.allowBlank,
				labelStyle:this.labelCss,
				format : 'Y-m-d',
				altFormats : 'ymd|Ymd|Y-m-d|Y.m.d|y.m.d',
				width:100,
				//padding: 3,
				labelWidth:10,
				fieldLabel: this.toFieldLabel,
            	//vtype: 'daterange',
				value: this.field2Value,
            	startDateField: 'startdt',
				tabIndex:this.tabIndex
	        })
        ]
    },
	setfield1Name: function(value){
		this.field1Name = value;
	},
	setfield2Name: function(value){
		this.field2Name = value;
	},
	setfield1Value: function(value){
		this.field1Value = value;
	},
	setfield2Value: function(value){
		this.field2Value = value;
	}
});

Ext.define('Ext.ux.form.field.SearchBox', {
    extend:'Ext.form.FieldContainer',
	mixins: {
        field: 'Ext.form.field.Field'
    },
	alias: 'widget.searchbox',
	layout: 'hbox',
	cls: 'custom_field',
    height: 22,
    combineErrors: true,
    msgTarget :'side',
	field1Name: null,
	field1Value: null,
	field2Value: null,
	handler: null,
	datejson: null,
	initComponent: function() {
        var me = this;
        me.buildField();
        me.callParent();
		me.submitValue = false;
        this.textfield = this.down('textfield');
        this.checkbox = this.down('checkbox');
        me.initField();
    },
	buildField: function(){
        this.items = [
            Ext.apply({
				flex:1,
	            xtype: 'triggerfield',
				name:this.field1Name,
				value: this.field1Value,
            	onTriggerClick: this.handler,
				tabIndex:this.tabIndex
	        }),
	        Ext.apply({
	            xtype: 'hiddenfield',
				name:this.field1Name+ '_hidden',
				value: this.field2Value,
				tabIndex:this.tabIndex
	        })
        ]
    },
	setDatejson: function(value){
		this.datejson = value;
	},
	setfield1Name: function(value){
		this.field1Name = value;
	},
	setHandler: function(value){
		this.handler = value;
	},
	setfield1Value: function(value){
		this.field1Value = value;
	},
	setfield2Value: function(value){
		this.field2Value = value;
	}
});

Ext.define('Ext.ux.form.field.Upload', {
    extend:'Ext.form.FieldContainer',
	mixins: {
        field: 'Ext.form.field.Field'
    },
	alias: 'widget.upload',
	layout: 'hbox',
	cls: 'custom_field',
    height: 50,
    combineErrors: true,
    msgTarget :'side',
	field1Name: null,
	field1Value: null,
	type:null,
	uploadUrl:null,
	downloadpath:null,
	initComponent: function() {
        var me = this;
        me.buildField();
        me.callParent();
		me.submitValue = false;
        this.textfield = this.down('textfield');
        this.checkbox = this.down('checkbox');
        me.initField();
    },
	buildField: function(){
		var me =this;
		this.items=[];
		var delete_btn = {
			margin: '4,0,0,0',
	        xtype: 'button',
			text: 'delete',
			handler: function() {
				//this.items.items[0].destroy();
				if(this.type=='img'){
					images.setSrc("");
				}else{
					this.items.items[0].setText("");
				}
				hiddenFiels.setValue("");
            },
			scope:this
		}
		var file_location ={
				flex:1,
	            xtype: 'filefield',
				value: this.field1Value,
				padding: 3,
				buttonConfig: {
					width:50
				},
            	buttonText: 'Select'
	        };
		var images = Ext.create('Ext.Img',{
				src: this.field1Value,
				width:50,
				height:50,
				margin:2
			});
		var fileHtml=this.field1Value==undefined?"": this.field1Value;
		var fileHtmlName=(this.field1Value==undefined||this.field1Value=="")?"": " 查看  ";
		var file_link=Ext.create('Ext.form.Label',{
			margin: 5,
			width:50,
			height:50,
			html: '<a href="'+fileHtml+'"  target="_blank" >'+fileHtmlName+'</a>'

		});
		var hiddenFiels=Ext.create('Ext.form.field.Hidden',{
			margin: '2,0,0,0',
			name:this.field1Name,
			value:this.field1Value
		});

		var upload_btn={
			margin: '4,0,0,0',
	        xtype: 'button',
			text: 'Upload',
			icon: 'icons/application_get.png',
			handler: function() {
				 var uploadPanel = Ext.create('Ext.form.Panel', {
					width: 500,
					standardSubmit:false,
					frame: true,
					defaults: {
						anchor: '100%',
						allowBlank: false,
						msgTarget: 'side',
						labelWidth: 50
					},

					items: [{
					    xtype: 'filefield',
					    name: this.field1Name,
						emptyText: 'Select an File',
						fieldLabel: 'File'
						//name:this.fieldName
					}],
					buttons: [{
						text: 'Upload',
						handler: function () {
							var form = uploadPanel.form;
							if (form.isValid()) {
								form.submit({
									//url: 'modules/quotation/upload_file.jsp',
									url:me.uploadUrl,
									waitMsg: 'Uploading...',
									success: function (form, action) {
										var succ=action.result.success;
										if(succ) {
											Ext.Msg.alert('Success', 'Processed file "' + action.result.file + '" on the server');
											   if(me.type=='file'){
												   var htmlText='<a href="'+action.result.url+'" target="_blank">查看</a>';
												   file_link.setText(htmlText, false);
											  }else{
											       images.setSrc(action.result.file);
											  }
											  hiddenFiels.setValue(action.result.file);
											  winUpload.close();
										}else{
										    Ext.Msg.alert('Failed', action.result.message);
										    winUpload.close();
										}
									},
									failure: function (form, action) {
									    Ext.Msg.alert('Failed', action.result.message);
									    winUpload.close();
                                    }
								});
							}
						},
						scope:this
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
			scope:this
		};


		if(this.type==null || this.type==undefined){
			this.items.push(file_location);
		}else{
			if(this.type=='img'){
				this.items.push(images);
			}else{
				this.items.push(file_link);
			}
			this.items.push(hiddenFiels);
			this.items.push(upload_btn);
			this.items.push(delete_btn);

		}
    },
    setfield1Name: function(value){
		this.field1Name = value;
	},
	setfield2Name: function(value){
		this.field2Name = value;
	},
	setfield1Value: function(value){
		value==undefined?"": this.value;
		this.field1Value = value;
	},
	setfield2Value: function(value){
		this.field2Value = value;
	}
});

Ext.define('Ext.ux.form.field.InputWithInput', {
    extend:'Ext.form.FieldContainer',
	mixins: {
        field: 'Ext.form.field.Field'
    },
	alias: 'widget.inputwithinput',
	layout: 'hbox',
    height: 22,
	cls: 'custom_field',
	field1Name: null,
	field2Name: null,
	field1Value: null,
	field2Value: null,
    combineErrors: true,
    msgTarget :'side',
	initComponent: function() {
        var me = this;
        me.buildField();
        me.callParent();
		me.submitValue = false
        this.textfield = this.down('textfield');
        this.checkbox = this.down('checkbox');
        me.initField();
    },
	buildField: function(){
        this.items = [
            Ext.apply({
	            xtype: 'textfield',
				flex:1,
				name:this.field1Name,
				width:200,
				tabIndex:this.tabIndex
	        }),
	        Ext.apply({
	            xtype: 'textfield',
	             padding: '0 0 0 3',
				name:this.field2Name,
				width:200,
				tabIndex:this.tabIndex
	        })
        ]
    },
	setfield1Name: function(value){
		this.field1Name = value;
	},
	setfield2Name: function(value){
		this.field2Name = value;
	},
	setfield1Value: function(value){
		this.field1Value = value;
	},
	setfield2Value: function(value){
		this.field2Value = value;
	}
});

Ext.apply(Ext.form.field.VTypes, {
        daterange: function(val, field) {
            var date = field.parseDate(val);

            if (!date) {
                return false;
            }
            if (field.startDateField && (!this.dateRangeMax || (date.getTime() != this.dateRangeMax.getTime()))) {
                var start = field.ownerCt.items.items[0];// field.up('form').down('#' + field.startDateField);
                start.setMaxValue(date);
                start.validate();
                this.dateRangeMax = date;
            }
            else if (field.endDateField && (!this.dateRangeMin || (date.getTime() != this.dateRangeMin.getTime()))) {
                var end = field.ownerCt.items.items[1];

                end.setMinValue(date);
                end.validate();
                this.dateRangeMin = date;
            }
            return true;
        },

        daterangeText: 'Start date must be less than end date'
    });
Ext.define('Ext.ux.form.field.DateWithInput', {
    extend:'Ext.form.FieldContainer',
	mixins: {
        field: 'Ext.form.field.Field'
    },
	alias: 'widget.date_input',
	layout: 'column',
    height: 22,
	cls: 'custom_field',
	name1: null,
	allowBlank:false,
	value: null,
	hand:null,
    combineErrors: true,
    msgTarget :'side',
	initComponent: function() {
        var me = this;
        me.buildField();
        me.callParent();
        if(this.hand!=null&&this.hand!=undefined){
        	me.items.items[0].addListener('change',this.hand,false);
        }
		me.submitValue = false;
		this.hiddenfield = this.down('hiddenfield');
		//this.datefield = this.down('datefield');
		//this.textfield = this.down('textfield');
        me.initField();
    },
	buildField: function(){
        this.items = [
            Ext.apply({
	            xtype: 'datefield',
				flex:1,
				name:this.name1+'[-]date',
				value:this.value.match(/^(\d{1,4})(-|\/)(\d{1,2})\2(\d{1,2})$/)!=null?this.value:'',
				format : 'Y-m-d',
				altFormats : 'ymd|Ymd|Y-m-d|Y.m.d|y.m.d',
				width:100,
				//allowBlank:this.allowBlank,
				tabIndex:this.tabIndex,
				  listeners: {
                    change: function (e,n,o) {
	            	if(n!=''&&n!=null){
	            	     var f=this.up('form').getForm();
	            	     var name=this.name.split('[-]')[0];
	            	     f.findField(name+'[-]input').setValue('');
	            	     f.findField(name).setValue(Ext.Date.format(n,'Y-m-d'));
	                    }
	               }
                  }
	        }),
	        Ext.apply({
	            xtype: 'textfield',
	            padding: 3,
				name:this.name1+'[-]input',
				value: this.value.match(/^(\d{1,4})(-|\/)(\d{1,2})\2(\d{1,2})$/)==null?this.value:'',
				cls: 'column_checkbox',
				width:240,
				//allowBlank:this.allowBlank,
				tabIndex:this.tabIndex,
				  listeners: {
                    change: function (e,n,o) {
		        	if(n!=''&&n!=null){
			        	 var f=this.up('form').getForm();
		        	     var name=this.name.split('[-]')[0];
		        	     f.findField(name+'[-]date').setValue('');
		        	     f.findField(name).setValue(this.value);
		        	}
                    }
                  }
	        }),
	        Ext.apply({
	            xtype: 'hiddenfield',
				name:this.name1,
				value:this.value
	        })
        ]
    },
	setname1: function(value){
		this.name1 = value;
    },
	setvalue: function(value){
		this.value = value;
	}
});

Ext.define('Ext.ux.form.field.ProductField', {
    extend:'Ext.form.FieldContainer',
	mixins: {
        field: 'Ext.form.field.Field'
    },
	alias: 'widget.product',
	fieldLabel:'Crieria',
	layout:{
        type:'table',
        columns:1,
        tableAttrs: {
            style: {
                width: '100%',
                margin:'0px 0px 0px 0px'
            }
        }
        },
	cls: 'custom_field',
	field1Name: null,
	field2Name: null,
	field3Name: null,
	field4Name: null,
	field1Value: null,
	field2Value: null,
	field3Value: null,
	field4Value: null,
	datasource1:null,
	datasource2:null,
	datasource3:null,
	datasource4:null,
    combineErrors: true,
    msgTarget :'side',
    name:null,
	initComponent: function() {
        var me = this;
        me.buildField();
        me.callParent();
		me.submitValue = false;
        this.textfield = this.down('textfield');
        this.checkbox = this.down('checkbox');
        me.initField();
       // me.init_comp();

    },
	buildField: function(){
        this.items = [
                      Ext.apply({
                    	  xtype:'fieldcontainer',
                    	  layout: 'hbox',
                    	  height:22,
                    	  padding: '0 0 0 0',
                    	  cls: 'custom_field',
                    	  items:[{
              			    xtype: 'combobox',
            			    emptyText:'Product',
            				value: this.field1Value,
            				name:this.field1Name,
            	            padding: '3 0 0 0',
            				tabIndex:this.tabIndex,
		    				plugins: ['clearbutton'],
		                    displayField: 'value',
		                    valueField: 'id',
            			    store:  Ext.create('Ext.data.Store', {
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
 	                                url: this.datasource1,
 	                                reader: {
 	                                    type: 'json',
 	                                    root: 'data'
 	                                }
 	                            }
 	                        }),
 	                       queryMode: 'local',
 	                       listeners: {
                              select: function () {
                                  if(this.lastValue==1){
		                                var p=this.ownerCt.ownerCt;
													    		  if(p.items.length>1){
													    			 for(j=p.items.length-1;j>0;j--){
													    				 p.remove(p.items.items[j]);
													    			 }
													    		  }
													    		  this.ownerCt.items.items[1].hide();
													    		  this.ownerCt.items.items[2].hide();
													    		  this.ownerCt.items.items[3].hide();
													    		  this.ownerCt.items.items[4].hide();
	                              }else{
	                              	  this.ownerCt.items.items[1].show();
													    		  this.ownerCt.items.items[2].show();
													    		  this.ownerCt.items.items[3].show();
													    		  this.ownerCt.items.items[4].show();
	                              }
                              }
                          }
            	        },{
            			    xtype: 'combofieldbox',
            			    width:150,
            			    emptyText:'Category',
            				value: this.field2Value,
            				name:this.field2Name,
            	            padding: '3 0 0 0',
            				tabIndex:this.tabIndex,
            				multiSelect: true,
		                    displayField: 'value',
		                    valueField: 'id',
            			    store:  Ext.create('Ext.data.Store', {
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
 	                               url: this.datasource2,
 	                                reader: {
 	                                    type: 'json',
 	                                    root: 'data'
 	                                }
 	                            }
 	                        }),
 	                       queryMode: 'local',
 	                        listeners: {
                              change: function () {
                                   sub = this.ownerCt.items.items[2];
	                                 sub.clearValue();
	                                 var pvalue = this.lastValue;
	                                 if (sub.events.focus != true) {
	                                     sub.events.focus.clearListeners();
	                                 }
	                                 sub.addListener({
	                                 focus: function (e, op) {
	                                 if (e.store.proxy.url.indexOf("?filter") != -1){
	                                 var other="";
	                                 if(e.store.proxy.url.indexOf("&") != -1){
	                                    other=e.store.proxy.url.substring(e.store.proxy.url.indexOf("&")+1,e.store.proxy.url.length);
	                                  }
	                                  e.store.proxy.url = e.store.proxy.url.substring(0, e.store.proxy.url.indexOf("?filter"));
	                                 if(other!=""){
	                                   e.store.proxy.url+="?"+other;
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
	                                 sub.fireEvent('focus',sub,sub.store);
                              }
                          }
            	        },{
            			    xtype: 'combofieldbox',
            			    width:150,
            			    emptyText:'Product',
            				value: this.field3Value,
            				name:this.field3Name,
            	            padding: '3 0 0 0',
            				tabIndex:this.tabIndex,
            				multiSelect: true,
		                    displayField: 'value',
		                    valueField: 'id',
            			    store:  Ext.create('Ext.data.Store', {
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
 	                               url: this.datasource3,
 	                                reader: {
 	                                    type: 'json',
 	                                    root: 'data'
 	                                }
 	                            }
 	                        }),
 	                       queryMode: 'local',
            	        },{
            			    xtype: 'combobox',
            			    emptyText:'Qty',
            				value: this.field4Value,
            				name:this.field4Name,
            	            padding: '3 0 0 0',
            				tabIndex:this.tabIndex,
		    				plugins: ['clearbutton'],
		                    displayField: 'value',
		                    valueField: 'id',
            			    store:  Ext.create('Ext.data.Store', {
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
 	                                url: this.datasource4,
 	                                reader: {
 	                                    type: 'json',
 	                                    root: 'data'
 	                                }
 	                            }
 	                        }),
 	                       queryMode: 'local',
            	        },{
            			    xtype: 'button',
            				text: 'Add',
            	            padding: '3 0 0 0',
            	            margin:'6',
            	            handler:this.getDetailComp
            	        }]
                      })
        ]
    },
    getDetailComp:function(e,b){
    	var me=this;
    	var product1=e.ownerCt.items.items[2].getValue();
    	var Qty=e.ownerCt.items.items[3].getValue();
    	var qtyUrl=e.ownerCt.items.items[3].store.proxy.url;
    	if(product1==undefined||product1==''){
    		Ext.Msg.alert('Warn','Please select Product!');
    	}else if(Qty==undefined||Qty==''){
    		Ext.Msg.alert('Warn','Please select Qty!');
    	}else{
    		var p=e.ownerCt.ownerCt;
    		if(p.items.length>1){
    			for(j=p.items.length-1;j>0;j--){
    				p.remove(p.items.items[j]);
    			}
    		}
    		for(i=0;i<e.ownerCt.items.items[2].lastValue.length;i++){
    		var pId=e.ownerCt.items.items[2].lastValue[i];
    		var pValue=e.ownerCt.items.items[2].store.data.get(pId).data.value;
	    	var deleteC={
		        	   xtype:'fieldcontainer',
	        	       layout: 'hbox',
	        	       height:22,
	        	      cls: 'custom_field',
	        	      padding: '0 0 0 0',
					   items:[
					      { xtype: 'textfield',
		    				name:'product_'+pId,
		    	            padding: '3 0 0 0',
		    	            width:440,
		    	            value:pValue
		    	        },{
		    			    xtype: 'combobox',
		    			    emptyText:'Qty',
		    			    name:'qty_'+pId,
		    	            padding: '3 0 0 0',
		    				plugins: ['clearbutton'],
		                    displayField: 'value',
		                    valueField: 'id',
		    			    store:  Ext.create('Ext.data.Store', {
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
	                              url: qtyUrl,
	                              reader: {
	                                  type: 'json',
	                                  root: 'data'
	                              }
	                          }
	                      }),
	                     queryMode: 'local',
	                     value:Qty
		    	        },{
		    			    xtype: 'button',
		    				text: 'Remove',
		    	            padding: '3 0 0 0',
		    	            margin:'6',
		    	            handler:function(e,b){
		    	        	var p=e.ownerCt.ownerCt;
		    	        	p.remove(e.ownerCt);

		    	        }
		    	        }]
		        };
		        	///////////
		        	e.ownerCt.ownerCt.add(deleteC);
    		}
    	}
    },
	setfield1Name: function(value){
		this.field1Name = value;
	},
	setfield2Name: function(value){
		this.field2Name = value;
	},
	setfield3Name: function(value){
		this.field3Name = value;
	},
	setfield4Name: function(value){
		this.field4Name = value;
	},
	setfield1Value: function(value){
		this.field1Value = value;
	},
	setfield2Value: function(value){
		this.field2Value = value;
	},
	setfield3Value: function(value){
		this.field3Value = value;
	},
	setfield4Value: function(value){
		this.field4Value = value;
	},
	setdatasource1: function(value){
		this.datasource1 = value;
	},
	setdatasource2: function(value){
		this.datasource2 = value;
	},
	setdatasource3: function(value){
		this.datasource3 = value;
	},
	setdatasource4: function(value){
		this.datasource4 = value;
	},
	setname: function(value){
		this.name = value;
	}

});
Ext.define('Ext.ux.form.field.multiUploadDialog.', {
    extend:'Ext.form.FieldContainer',
	mixins: {
        field: 'Ext.form.field.Field'
    },
	alias: 'widget.multiUploadDialog',
 layout:{
        type:'table',
        columns:1,
        tableAttrs: {
            style: {
                width: '100%',
                margin:'0px 0px 0px 0px'
            }
        }
        },
  combineErrors: true,
  name:null,
  images:null,
  uploadUrl:null,
  addUrl:null,
  editUrl:null,
  editDataUrl:null,
  removeUrl:null,
  cls: 'custom_field',
  msgTarget :'side',
  fieldLabel:'Images',
	initComponent: function() {
        var me = this;
		    me.buildField();
        me.callParent();
		    me.submitValue = false;
        this.textfield = this.down('textfield');
        this.checkbox = this.down('checkbox');
        me.initField();
    },
   /* listeners: {
		render: function () {
		      this.drag_drop();//拖动排序
	    }
	},*/
	buildField: function(){
		 var me = this;
		 var images_temp=[];
		 var imgs=me.images;
		 for(var i=0;i<imgs.length;i++){
           images_temp[i]={
		        	 xtype:'fieldcontainer',
	        	      layout: 'hbox',	 
	        	      height:22,
	        	      padding: '0 0 0 0',
					        items:[
						       {xtype:'image',colspan:3,src:imgs[i].src,name:imgs[i].id,width:150,height:150},
						       {xtype:'checkbox', cls: 'custom_field_image',name:me.name+"_check"},
						       {xtype: 'button',				    			   
				    				iconCls:'iconPre',
				    	            handler:function(e,b){
										var imageCon=e.ownerCt.ownerCt;
										var curr_img=e.ownerCt;
										var curr_img_index=null;
										var new_img=null;
										var image_count=imageCon.items.length;
										for(var i=0;i<image_count;i++){
										if(e.ownerCt==e.ownerCt.ownerCt.items.items[i]){
                                                 curr_img_index=i;
                                                 break;
											}
									    }
									    if(curr_img_index>0){//判断当前image不是第一个
                                               new_img=e.ownerCt.ownerCt.items.items[curr_img_index-1];
                                               imageCon.move(curr_img_index,curr_img_index-1);
									    }

								   }
								},
						        {xtype: 'button',									
									iconCls:'iconNext',
									handler:function(e,b){
											var imageCon=e.ownerCt.ownerCt;
											var curr_img=e.ownerCt;
											var curr_img_index=null;
											var new_img=null;
											var image_count=imageCon.items.length;
											for(var i=0;i<image_count;i++){
											      if(e.ownerCt==e.ownerCt.ownerCt.items.items[i]){
										          curr_img_index=i;
										            break;
													}
												}
												if(curr_img_index<image_count-1){//判断当前image不是最后一个
										                 new_img=e.ownerCt.ownerCt.items.items[curr_img_index+1];
										                 imageCon.move(curr_img_index,curr_img_index+1);
												}
											}

								}
					        ] };
			      }


        this.items = [
						Ext.apply({
	        	        xtype:'toolbar',
				         width   : '100%',
				         margin: '0 0 0 2',
				      //  cls: 'component_toolbar',
	        	    	items:[
				        	   	{
				    			   xtype: 'button',
				    			   text: 'Add images',
				    	            padding: '3 0 0 0',
				    	            margin:'6',
				    	            iconCls:'iconAdd',
				    	            handler:function(e,b){
				    	        	  me.add_images(me);
				    	            }
				    	        },

				               {
				    			   xtype: 'button',
				    			    text: 'Edit selected',
				    				iconCls:'iconEdit',
				    	            padding: '3 0 0 0',
				    	            margin:'6',
				    	            handler:function(e,b){
				    	            	var flag=false;
				    	            	var pImage=new Array();
				    	        	   	var form=e.up('form').getForm();
				    	            	var form_field=form.getFields().items;
					    	        	  for(var i=0;i<form_field.length;i++){
				    	            	if(form_field[i].name==me.name+'_check'){
						    	            		if(form_field[i].value==true){
						    	            		 	 var p=form_field[i].ownerCt.items.items[0];
						    	            		 	 pImage.push(p);
						    	            		 	  flag=true;
						    	            		 }
				    	            		 	 }
				    	            		}
				    	            		if(flag){
				    	            			 me.edit_image(me,e,pImage);
				    	            		}else{
				    	            		   Ext.Msg.alert('Waring','Please select edit image!');
				    	            		}

				    	           }
				    	        },
				              {
				    			    xtype: 'button',
				    			    text: 'Remove selected',
				    				iconCls:'iconRemove',
				    	            padding: '3 0 0 0',
				    	            margin:'6',
				    	            handler:function(e,b){
				    	            	var flag=false;
				    	            	var pImage=new Array();
				    	            	var imgNames='';
				    	        	   	var form=e.up('form').getForm();
				    	            	var form_field=form.getFields().items;
					    	        	  for(var i=0;i<form_field.length;i++){
				    	            	if(form_field[i].name==me.name+'_check'){
						    	            		if(form_field[i].value==true){
						    	            		 	  var imgName=form_field[i].ownerCt.items.items[1].name;
						    	            		 	  var pima=form_field[i].ownerCt.items.items[1];
						    	            		 	  pImage.push(pima);
						    	            		 	  imgNames=imgNames+imgName+',';
						    	            		 	  flag=true;
						    	            		 }
				    	            		 	 }
				    	            		}
				    	            		if(flag){
												if(imgNames!=''){
													imgNames=imgNames.substring(0,imgNames.length-1);
													}
												 Ext.Ajax.request({
												  url:me.removeUrl,
												  params:{
													  ids:imgNames
													  },
												  success:function(o){
								                      var DataJson = Ext.decode(o.responseText);
								                       for(var j=0;j<pImage.length;j++){
														   pImage[j].ownerCt.ownerCt.remove(pImage[j].ownerCt);
														   }
													     Ext.Msg.alert('Status','Remove success!');
													 }
			                                   });

				    	            		}else{
				    	            		Ext.Msg.alert('Waring','Please select image!');
				    	            		}
				    	           }
				    	        },
				              {
				    			       xtype: 'button',
				    				     text: 'Select all',
				    				     iconCls:'iconUnSelectAll',
				    	            padding: '3 0 0 0',
				    	            margin:'6',
				    	            handler:function(e,b){
				    	            	var form=e.up('form').getForm();
				    	            	var form_field=form.getFields().items;


					    	        	  if(e.iconCls=='iconSelectAll'){
					    	        	  	   e.setIconCls('iconUnSelectAll');
					    	        	  	   for(var i=0;i<form_field.length;i++){
				    	            		   if(form_field[i].name==me.name+'_check'){
				    	            		 	    form_field[i].setValue(false);
				    	            		 	   }
				    	            		   }

					    	        	  }else if(e.iconCls=='iconUnSelectAll'){
					    	        	  	   e.setIconCls('iconSelectAll');
					    	        	  	   		 for(var i=0;i<form_field.length;i++){
				    	            		   if(form_field[i].name==me.name+'_check'){
				    	            		 	    form_field[i].setValue(true);
				    	            		 	   }
				    	            		   }
					    	        	  }
				    	           }
				    	        }
	    	        ]
	    	        }),
	    	    Ext.apply({
	        	   xtype:'fieldcontainer',
        	       layout:{
					         type:'table',
					         columns:4,
					         tableAttrs: {
					            style: {
					              width: '100%',
					              margin:'0px 0px 0px 0px'

					             }
					         },
					         tdAttrs:{
					           style: {
					             width: '25%'
					            }
					         }
					     },
        	            padding: '0 0 0 0',
				        items:images_temp
				        })
        ]
    },
    add_images:function(me){
		  var upload = Ext.create('Ext.ux.multiupload.Panel', {
				title: 'Upload',
				width: 600,
				height: 300,
				frame: true,
				uploadConfig: {
					uploadUrl: me.uploadUrl,
					maxFileSize: 4 * 1024 * 1024,
					maxQueueLength: 5
				}
			});

		  upload.on('fileuploadcomplete', function (fileData) {//一个一个上传后，add到现实列表
		         var img_src=Ext.decode(fileData).imageSrc;
		         var img_name=Ext.decode(fileData).imageId;
			   	  me.items.items[1].add({
			   		   xtype:'fieldcontainer',
			   		   layout: 'hbox',
			   			height:22,
			   			padding: '0 0 0 0',
			   			items:[
			   			       {xtype:'image',src:img_src,name:img_name,width:150,height:150},
			   			       {xtype:'checkbox', cls: 'custom_field_image',name:me.name+"_check"},
						    		 {xtype: 'button',				    			  
				    			     iconCls:'iconEdit',
				    	            handler:function(e,b){
										var imageCon=e.ownerCt.ownerCt;
										var curr_img=e.ownerCt;
										var curr_img_index=null;
										var new_img=null;
										var image_count=imageCon.items.length;
										for(var i=0;i<image_count;i++){
										if(e.ownerCt==e.ownerCt.ownerCt.items.items[i]){
                                                 curr_img_index=i;
                                                 break;
											}
									    }
									    if(curr_img_index>0){//判断当前image不是第一个
                                               new_img=e.ownerCt.ownerCt.items.items[curr_img_index-1];
                                               imageCon.move(curr_img_index,curr_img_index-1);
									    }

								   }
								},
						        {xtype: 'button',									
									iconCls:'iconEdit',
									handler:function(e,b){
											var imageCon=e.ownerCt.ownerCt;
											var curr_img=e.ownerCt;
											var curr_img_index=null;
											var new_img=null;
											var image_count=imageCon.items.length;
											for(var i=0;i<image_count;i++){
											      if(e.ownerCt==e.ownerCt.ownerCt.items.items[i]){
										          curr_img_index=i;
										            break;
													}
												}
												if(curr_img_index<image_count-1){//判断当前image不是最后一个
										                 new_img=e.ownerCt.ownerCt.items.items[curr_img_index+1];
										                 imageCon.move(curr_img_index,curr_img_index+1);
												}
											}

								}]
					});
             });


          upload.on('filequeuedatacomplete', function (source, data) {//所有上传完后
		         win.close();

             });


			 var win = Ext.create('widget.window', {
					closable: true,
					width: 610,
					height: 330,
					modal: true,
					layout:{
					  type:'anchor'
					},
					defaults: {
					 anchor: '100%'
				   },
				   autoScroll:true,
				   items:[upload]

			 });

			 win.show();
    	},
 edit_image:function(me,e,check_images){
   	var temp=[];
   	var tempIndex=0;
 	  for(var j=0;j<check_images.length;j++){
 		   temp[tempIndex]={
		         	xtype:'image',
		         	src:check_images[j].src,
		         	width:100,
		         	height:100,
		         	rowspan:2
	         	};
	         	tempIndex=tempIndex+1;
	         temp[tempIndex]=	{
          	    xtype: 'textfield',
				        name: 'name',
				        fieldLabel: 'Name',
				        colspan:2
           };
           tempIndex=tempIndex+1;
           temp[tempIndex]={
          	    xtype: 'textareafield',
				        name: 'description',
				        fieldLabel: 'Description',
				        colspan:2
           };
           tempIndex=tempIndex+1;
 		    }

    	 var form=Ext.create('Ext.form.Panel',{
    	   	url: me.editUrl,
	        bodyPadding: '5 5 5 5',
	        width: 600,
	        height:360,
	        autoScroll:true,
	        fieldDefaults: {
	            labelAlign: 'left',
	            msgTarget: 'side'
	        },
	        anchor: '100%',
            layout:{
                type:'table',
                columns:3,
                tableAttrs: {
                    style: {
                        width: '100%',
                        margin:'0px 5px 5px 0px'
                    }
                }
            },
	         items: temp,
	            buttons: [{
	                text: 'Save changes',
	                handler:function(){
	            	 var f= this.up('form').getForm();
	            	 if(f.isValid()){
	            		  f.submit({
	            		  waitMsg: 'Update...',
                         success: function(fp, o) {
                    	     editWin.close();
                              Ext.Msg.alert('Success', 'Update success!');
						  }

                  });
	            	 }
	               }
	            },{
	            	text: 'Close',
	                handler:function(){
	                	editWin.close();
	                	}
	            	}]
		});
		 var editWin = Ext.create('widget.window', {
				closable: true,
				title: 'upload',
				width: 600,
				height: 400,
				modal: true,
				layout:{
			      type:'anchor'
		        },
		        defaults: {
                 anchor: '100%'
               },
			   items:[form]
		 });
	     editWin.show();
    },
    setname:function(value){
    		this.name=value;
    },
    setimages:function(value){
    	this.images=value;
    },
    setuploadUrl:function(value){
	    this.uploadUrl=value;
    },
    seteditUrl:function(value){
   	    	this.editUrl=value;
    },
    seteditDataUrl:function(value){
   	    	this.editDataUrl=value;
    },
    setremoveUrl:function(value){
   	    	this.removeUrl=value;
    }
});