Ext.onReady(function (){
   var historybar = Ext.create('Ext.toolbar.Toolbar'); 
   historybar.render('top_toolbar_box');
   Ext.define('com.embraiz.history',{
   viewHistory:[],   
   dohistory:function (id, href, title, cls) {
	var check = true;
	var t_o = {};
	var i = 0;
	t_o.href = href;
	t_o.title = title;
	t_o.cls = cls;
	t_o.id = "history_" + id;
	for (i = 0; i < viewHistory.length; i++) {
		if (viewHistory[i].href == href) {
			check = false;
		}
	}
	if (check) {
		viewHistory.push(t_o);
		if (viewHistory.length > 5) {
			viewHistory.shift();
		}
	}
	new com.embraiz.history().showHistory();
},
 showHistory:function(o) {
	if(Ext.get('top_toolbar_box')!=null && Ext.get('top_toolbar_box')!=undefined){ //start if
	
    eval('var showRecord='+o.responseText); 
    historybar.destroy();
	historybar= Ext.create('Ext.toolbar.Toolbar', {  
    items: []          
  }); 
	historybar.render("top_toolbar_box");
	for (i = 0; i < showRecord.length; i++) {
		temp_index0 = i;
		var showText=showRecord[i].title+":"+showRecord[i].name;
		if(showText.length>15){
		   showText=showText.substring(0,15)+"..";
		}
		var spmodule=showRecord[i].module.split(',');
		var module='';
		if(spmodule.length<2){
			module=spmodule[0];
		}else{
			module=spmodule[1];
		}
		historybar.add({id:showRecord[i].title+":"+showRecord[i].id, text:showText, iconCls:showRecord[i].cls, h_url:"com.embraiz."+module+".js.edit"});		
		Ext.getCmp(showRecord[i].title+":"+showRecord[i].id).on("click", function (btn, e) {
		    		    
			id = btn.id;
			iconCls = btn.iconCls;
			href = btn.h_url;
			title = btn.text;
			
			if(href.indexOf('entry')!=-1){
			   var in_array=new Array();var j=0;
			   var tagid=id.substring(id.indexOf(':')+1,id.length);
			   for(var i=0;i<Ext.getCmp("content-panel").items.getCount();i++){
			       var curid=Ext.getCmp("content-panel").items.get(i).getId();
			       curid=curid.substring(curid.indexOf(":")+1,curid.length);
			       in_array[i]=curid;
			       if(curid==tagid){j=i;}
			   }
			   if(in_array.in_array(tagid)){
			      Ext.getCmp("content-panel").setActiveTab(j);
			   }else{
			      Ext.Ajax.request({
			         url:'modules/history/getEntry.jsp?caseId='+id+'',
			         success:function(o){
			            var DataJson = Ext.decode(o.responseText);
			            new com.embraiz.tag().openNewTag(id, title, href, iconCls,iconCls,iconCls,DataJson.id);
			         }
			      });   
			      
			   }
			}else{
				if(spmodule.length<2){
					new com.embraiz.tag().openNewTag(id, title, href, iconCls);
				}else{
					new com.embraiz.tag().openNewTag(id, title, href, iconCls,iconCls,iconCls,spmodule[0]);
				}
			}
		});
	}
    document.onhelp=function(){return false};   
    window.onhelp=function(){return false}; 
    document.onkeydown = function(event){ 
    event = window.event || event;  
    if(event.keyCode==113 || (event.keyCode==114) || (event.keyCode==115) || (event.keyCode==116)){  
        return false;
    }   
    }  

	Array.prototype.S = String.fromCharCode(2);   
	Array.prototype.in_array = function(e) {   
	    var r = new RegExp(this.S+e+this.S);   
	    return (r.test(this.S+this.join(this.S)+this.S));   
	}; 
 

	//var fn=new Array(90,88,67,86,77);
	var fn=new Array(78,66,86,67,88);
	/////历史按钮事件
	
    for(var j=0;j<showRecord.length;j++){
		var showText=showRecord[j].title+":"+showRecord[j].name;
		if(showText.length>15){
		   showText=showText.substring(0,15)+"..";
		}
	        id = showRecord[j].title+":"+showRecord[j].id;
			iconCls = showRecord[j].cls;
			href = "com.embraiz."+showRecord[j].module+".js.edit";
			title = showText;	    
	   	var nav=new Ext.util.KeyMap(document,{
            stopEvent: true,
            ctrl:true,
            alt:true,
            key:fn[j],
            fn:function(key,e){
            new com.embraiz.tag().openNewTag(id,title,href,iconCls);
    }
    });
	}
	
	}//end if
			
},
 getHistory:function(){  
  	Ext.Ajax.request({
      url: 'modules/history/gethistory.jsp',
      success: function(o){
      new com.embraiz.history().showHistory(o);
      }
   });
 },
 addHistory:function(id,itemId,title){
   Ext.Ajax.request({
      url:'modules/history/addhistory.jsp?id='+id+'&title='+title+'&itemid='+itemId+'',
      success:function(o){
        new com.embraiz.history().getHistory();
      }
   });
 },
 updateHistory:function(id){
   Ext.Ajax.request({
      url:'modules/history/updatehistory.jsp?id='+id,
      success:function(o){
       new com.embraiz.history().getHistory();
      }
   });
 }
 });
});