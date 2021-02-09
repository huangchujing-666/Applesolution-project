Ext.define('com.embraiz.common.js.edit',{
	module_name:undefined,
	initTag : function (tab,url,title,id,module_name){
		var form_tool=new com.embraiz.component.form_tool();
		if(String(id).indexOf(':')!=-1){
			id=id.substring(id.indexOf(':')+1,id.length);	
		}			
	    var form = new com.embraiz.component.form();
	    form.tabUrl=this.$className;
	    form.itemId=id;
	    form.itemId1=module_name;
	    var appGrid = document.createElement("div");
		tab.getEl().dom.lastChild.appendChild(appGrid);
	    Ext.Ajax.request({
	    	url:'common/pageconfig.jsp',
	    	params:{
	    	    id:id,
	    		module_name:module_name
	    	},
	    	success:function(o){
	    		var resault=Ext.decode(o.responseText);
				form.appGrid=appGrid;
	    		form.approvalProcess=resault.approvalProcess;
	    		form.url=resault.url;
	    		form.parentType=resault.parentType;
	    		form.parentId=resault.parentId;
	    		form.id=id;
	    		if(resault.refjs!=undefined&&resault.refjs){//判断是否自定义function
	    			eval(resault.jsurl);
	    		}else{
	    			var target_div=form_tool.gen_form_div(tab);
	    			if(resault.formurl!=undefined&&resault.formurl!=''){//判断是否自定义form url
	    				form.viewForm(target_div, resault.formurl+'?id='+id);
	    			}else{	    				
	    				  form.viewForm(target_div, 'common/editJson.jsp?module_name='+module_name+'&id='+id);	    				
	    		    }   
		    	    if(resault.grid!=undefined&&resault.grid){//判断是否有grid		    	    	
		    	    	if(resault.gridurl instanceof Array){
			    	    	for(var i=0;i<resault.gridurl.length;i++){
			    	    		if(resault.gridurl[i].item!=undefined){
			    	    			eval(resault.gridurl[i].item);
			    	    		}else if(resault.gridurl[i] instanceof Object){
			    	    			new com.embraiz.common.js.index().grid(tab,id,resault.gridurl[i]);
			    	    		}else{
			    	    			eval(resault.gridurl[i]);
			    	    		}
			    	    	}
		    	    	}else{
		    	    		eval(resault.gridurl);
		    	    	}
		    	    	
		    	    }
		    	    if(resault.toolbar!=undefined&&resault.toolbar){//判断是否有toolbar
		    			if(resault.toolbarurl==undefined||resault.toolbarurl==''){//判断是否自定义 toolbar url
		    				form.showToolBar(target_div,'common/toolbar_data.jsp?id='+id+'&module_name='+module_name,module_name+'-'+id);		
		    			}else{
		    				form.showToolBar(target_div,resault.toolbarurl+'?id='+id,module_name+'-'+id);	
		    			}
		    		}
		    	    
		    	    if(resault.functions!=undefined&&resault.functions!=''){//判断是否有自定义js逻辑
		    	    	setTimeout(function(){eval(resault.functions)},300);
		    	    }
	    		}
	    	},
	    	socpe:this
	    });
	      
	}
});
