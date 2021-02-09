Ext.define('com.palmary.giftCategory.js.insert', {
	gridPanel:undefined,
	initTag: function (tab, url, title) {

	    // Check user seesion 
	    checkSession();

	    target_div=document.createElement("div");
	    tab.getEl().dom.lastChild.appendChild(target_div);
	    grider_div=document.createElement("div");
	    tab.getEl().dom.lastChild.appendChild(grider_div);	

	    Ext.Ajax.request({
	        url:'../giftCategory/insert',
	        success:function(o){
	            var data_json = Ext.decode(o.responseText);
	            new com.embraiz.component.form().editForm(target_div, data_json,null);
	        },
	        scope:this
	    });
	}
});

Ext.define('com.embraiz.user.js.copyuser',{
	initTag : function (tab,url,title,id){
		target_div=document.createElement("div");
		tab.getEl().dom.lastChild.appendChild(target_div);
		grider_div=document.createElement("div");
		tab.getEl().dom.lastChild.appendChild(grider_div);	
	
		Ext.Ajax.request({
		    url:'modules/user/copy_user_json.jsp',
		    params:{
			   userId:id
		    },
		    success:function(o){
		        var data_json = Ext.decode(o.responseText);
		        new com.embraiz.component.form().editForm(target_div, data_json,null);
		    },
		    scope:this
		});
	}
});