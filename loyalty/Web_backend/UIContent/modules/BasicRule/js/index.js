Ext.define('com.palmary.basicrule.js.index',{

    initTag:function (tab,url,title,id,itemId){
        
	    grider_div=document.createElement("div");
	    tab.getEl().dom.lastChild.appendChild(grider_div);

	    //grider_div2 = document.createElement("div");
	    //tab.getEl().dom.lastChild.appendChild(grider_div2);

	    grider_div3 = document.createElement("div");
	    tab.getEl().dom.lastChild.appendChild(grider_div3);

		var row_edit = new com.embraiz.component.roweditgird();
		row_edit.render('../BasicRule/GetHeader/Purchase', '../BasicRule/GetData/Purchase', grider_div3);
		
	   // var row_edit= new com.embraiz.component.roweditgird();
	   // row_edit.render('../BasicRule/GetHeader/PostPaid', '../BasicRule/GetData/PostPaid', grider_div);

	    //var row_edit = new com.embraiz.component.roweditgird();
	    //row_edit.render('../BasicRule/GetHeader/PrePaid', '../BasicRule/GetData/PrePaid', grider_div2);


	}
});