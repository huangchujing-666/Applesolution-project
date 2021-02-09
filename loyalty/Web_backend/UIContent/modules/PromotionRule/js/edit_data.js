Ext.define('com.palmary.promotionrule.js.edit', {
	gridPanel:undefined,
	initTag : function (tab,url,title,id){
	
	    // Check user seesion 
	    checkSession();

	    target_div_summary = document.createElement("div");
	    tab.getEl().dom.lastChild.appendChild(target_div_summary);

	    // Product Purchase Criteria
	    grider_div_pp = document.createElement("div");
	    tab.getEl().dom.lastChild.appendChild(grider_div_pp);

        // service criteria
	    grider_div2 = document.createElement("div");
	    tab.getEl().dom.lastChild.appendChild(grider_div2);

	    //grider_div3 = document.createElement("div");
	    //tab.getEl().dom.lastChild.appendChild(grider_div3);

	    //tool_div=document.createElement("div");
	    //tool_div.style.height=27;
	    //id=id.substring(id.indexOf(':')+1,id.length);
	    //tool_div.id="tool_div"+id;
	    //tab.getEl().dom.lastChild.appendChild(tool_div);
	    //target_div=document.createElement("div");
	    //target_div.setAttribute("tabindex","0"); 
	    //tab.getEl().dom.lastChild.appendChild(target_div);
	    //grider_div=document.createElement("div");
	    //tab.getEl().dom.lastChild.appendChild(grider_div);	

	    id = id.substring(id.indexOf(':') + 1, id.length);

	    Ext.Ajax.request({
	        url: '../PromotionRule/ViewBasicData/' + id,
	        success: function (o) {
	            var data_json = Ext.decode(o.responseText);
	            new com.embraiz.component.form().editForm(target_div_summary, data_json, null, '', '', '');

	        },
	        scope: this
	    });
	
	    //var form = new com.embraiz.component.form();
	    //form.viewForm(target_div, '../role/GetRole?id=' + id);
	    //form.showToolBar(tool_div, '../role/ToolBarData?id=' + id, id);

	    var row_edit = new com.embraiz.component.roweditgird();

	    // Product Purchase Criteria
	    row_edit.render('../PromotionRule/PurchaseHeader', '../PromotionRule/PurchaseData/' + id, grider_div_pp);

	    // Service Criteria
	    row_edit.render('../PromotionRule/ServiceHeader', '../PromotionRule/ServiceData/' + id, grider_div2);

	    //var row_edit = new com.embraiz.component.roweditgird();
	    //row_edit.render('../PromotionRule/GiftEarnHeader', '../PromotionRule/GiftEarnData/' + id, grider_div3);

	    //new com.embraiz.component.girdTable().initGrid('../user/GridHeader?id=' + id, '../user/ListData?id=' + id, grider_div);
	    //new com.embraiz.component.gridkeyboard().keyboard([grider_div]);

	    
	}
});