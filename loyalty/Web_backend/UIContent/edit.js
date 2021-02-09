Ext.define('com.embraiz.product.js.index',{	  
	 change_type:function(e,newValue,oldValue){		
		 var form = e.up('form').getForm(); 
		 if(newValue=='2'){		 
		  form.findField('method').show();    
		}else{
			 form.findField('method').hide();   
			}   
		},
		change_method:function(e,newValue,oldValue){
			 var form = e.up('form').getForm(); 
		 if(newValue!='1'){		 
		  form.findField('product').show();    
		}else{
			 form.findField('product').hide();   
			}
		}
	});