Ext.require( [ 'Ext.form.*' ]);

Ext.define('com.embraiz.component.ComboBox', {
	extend : 'Ext.form.field.ComboBox',
	alias: ['widget.myCombobox', 'widget.Mycombo'],
	sub_component : undefined,
	init_value : undefined,
	init_load : undefined,
	inputTextName : undefined,
	constructor : function(config) {
		if(config.store==undefined){
			config.store= Ext.create('Ext.data.Store', {
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
                autoLoad: true,
                proxy: {
                    type: 'ajax',
                    url: config.storeUrl,
                    getMethod: function(){ return 'POST'; },
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
			config.store.on('beforeload',function(thiz,action,value){                            	
        		if(thiz.getCount()==0&&!value.getValue()&&value.getValue()==''){
        			value=value.init_value;
        		}else{
        			value=value.getValue();
        		}
        		thiz.proxy.extraParams.defaultValue=value;
            },true,this);
			if(config.openLimit){
				var pageSize=25;
	        	if(config.pageSize){
	        		pageSize=config.pageSize;
	        	}
	        	Ext.apply(config,{pageSize:pageSize});
	        	Ext.apply(config.store,{pageSize:pageSize});
	        	Ext.apply(config.store.proxy,{extraParams:{openLimit:true}});
			}
		}
		     
		config = config || {};
		Ext.apply(this, config);
		this.callParent( [ config ]);
		var me = this;
	},
	createPicker : function() {
		var me = this, picker, pickerCfg = Ext.apply( {
			xtype : 'boundlist',
			pickerField : me,
			selModel : {
				mode : me.multiSelect ? 'SIMPLE' : 'SINGLE'
			},
			floating : true,
			hidden : true,
			store : me.store,
			displayField : me.displayField,
			focusOnToFront : false,
			pageSize : me.pageSize,
			tpl : me.tpl
		}, me.listConfig, me.defaultListConfig);

		picker = me.picker = Ext.widget(pickerCfg);
		if (me.pageSize) {
			picker.pagingToolbar.on('beforechange', me.onPageChange, me);
		}

		me.mon(picker, {
			itemclick : me.onItemClick,
			refresh : me.onListRefresh,
			scope : me
		});

		me.mon(picker.getSelectionModel(), {
			beforeselect : me.onBeforeSelect,
			beforedeselect : me.onBeforeDeselect,
			selectionchange : me.onListSelectionChange,
			scope : me
		});
		picker.minWidth=450;
//		picker.on('afterrender',function(thiz){
//			thiz;
//		})
		return picker;
	}	
});
Ext.override(Ext.view.BoundList, {
	createPagingToolbar : function() {
		return Ext.widget('pagingtoolbar', {
			id : this.id + '-paging-toolbar',
			pageSize : this.pageSize,
			store : this.store,
			border : false,
			ownerCt : this,
			items:[{xtype:'field',name:'field',width:80},
			       {xtype:'button',iconCls:'iconFind',handler:function(thiz,action){
						var field=thiz.ownerCt.items.get(11);
						var combo=thiz.ownerCt.ownerCt.pickerField;
						combo.lastQuery=field.getValue();
						combo.loadPage(1);
			       }}
			],
			ownerLayout : this.getComponentLayout()
		});
	}
});
Ext.override(Ext.form.field.ComboBox, {
    getValue: function() {
    // If the user has not changed the raw field value since a value was selected from the list,
    // then return the structured value from the selection. If the raw field value is different
    // than what would be displayed due to selection, return that raw value.
    var me = this,
        picker = me.picker,
        rawValue = me.getRawValue(), //current value of text field
        value = me.value; //stored value from last selection or setValue() call

    if (me.getDisplayValue() !== rawValue) {
        value = rawValue;
        me.value = me.displayTplData = me.valueModels = null;
        if (picker) {
            me.ignoreSelection++;
            picker.getSelectionModel().deselectAll();
            me.ignoreSelection--;
        }
    }
    if(value==null||value=="Please Select"){
    	value="";    	
    }
    return value;
	}
});
