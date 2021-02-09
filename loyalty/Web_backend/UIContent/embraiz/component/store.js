Ext.require([
    'Ext.data.*'
]);

		Ext.define('com.embraiz.component.store', {
			extend: 'Ext.data.Store',
			sub_component : undefined,
			constructor : function(config){
				
				config = config || {};
				Ext.apply(this,config);
				this.callParent([config]);
				var me= this;
			}
		});
