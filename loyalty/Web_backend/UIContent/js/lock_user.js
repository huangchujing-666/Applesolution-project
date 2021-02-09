Ext.require([
    'Ext.util.History',
    'Ext.tab.Panel'
]);
Ext.onReady(function(){
Ext.fly("lock_user_link").on("click", function(){//lock user	
	 Ext.Ajax.request({
         url: 'lock_user.jsp',
         async: true,
         success: function(){
		 
	    },
         scope: this
     });
		 var simple = Ext.create('Ext.form.Panel', {
		        url:'lock_user_login.jsp',		       		       
		        bodyStyle:'padding:5px 5px 0',
		        width: 380,
		        fieldDefaults: {
		            msgTarget: 'side',
		            labelWidth: 120
		        },
		        defaultType: 'textfield',
		        defaults: {
		            anchor: '100%'
		        },
		        items: [{
		            fieldLabel: 'User Name',
		            name: 'loginName',		           
		            allowBlank:false
		        },{
		            fieldLabel: 'Password',
		            name: 'password',
		            inputType:'password',
		            allowBlank:false
		        }],
		        buttons: [{
		            text: 'Login',
		            handler:function(){		        
		        	var form = this.up('form').getForm();		        	
		        	var loginName=form.findField('loginName').getRawValue();
		        	var password=form.findField('password').getRawValue();
		        	if(loginName==''&&password==''){
		        		Ext.Msg.alert('Warn!', 'User Name and Password are required!');
		        	}else{
		        		if (form.isValid()) {
						form.submit({
							success: function(form, action) {
							   Ext.Msg.alert('Success', 'Login success!');
							   win.close();
						    },
						    failure: function(form, action) {
								Ext.Msg.alert('Failed', 'Login failed!User Name or Password is error');
							}
						});
					 }
		        	}
		           }
		        },{
		            text: 'Cancel',
		            handler: function () {
	                       this.up('form').getForm().reset();                      
	                   }
		        }]
		    });
		 var win = Ext.create('widget.window', {
				title: 'Login',
				closable: true,
				width: 410,
				height: 250,
				closable:false,
				bodyStyle: 'padding: 5px;',
				modal: true,
				items:[simple]
		 });	    	 
	    win.show();
	});
});