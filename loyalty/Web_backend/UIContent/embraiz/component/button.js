Ext.require([
    'Ext.button.*'
]);

		Ext.define('com.embraiz.component.Button', {
			extend: 'Ext.button.Button',
			json_data : undefined,
			bindingImg:undefined,
			bindingLabel:undefined,
			uploadUrl:undefined,
			upType:undefined,
			constructor : function(config){
				
				config = config || {};
				Ext.apply(this,config);
				this.callParent([config]);
				var me= this;
			}
		});
