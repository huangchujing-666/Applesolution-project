Ext.define('Ext.ux.baseForm', {
    extend:'Ext.form.Panel',
	alias: 'widget.baseform',
	anchor: '100%',
    layout:{
        type:'table',
        columns:2,
        tableAttrs: {
            style: {
                width: '100%',
                margin:'0px 5px 5px 0px'
                	
            }
        },
        tdAttrs:{
        	style: {
            	width: '50%'
        	}
        }
        
    },
	fieldDefaults: {
    	  msgTarget: 'side',
          labelWidth: 150,
          labelStyle : 'font-weight: bold'
	},
	defaultType: 'textfield'
});