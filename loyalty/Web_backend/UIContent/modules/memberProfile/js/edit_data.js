Ext.define('com.palmary.memberProfile.js.edit', {
	gridPanel:undefined,
	initTag: function (tab, url, title, id, itemId) {

	    // Check user seesion 
	    checkSession();

	    var form_tool=new com.embraiz.component.form_tool();
	    var tool_div=form_tool.genTool(tab,id);
	    var target_div=form_tool.gen_form_div(tab);	
	    var _id = id.substring(id.indexOf(':')+1,id.length);
	
	    // Transaction Earn
	    grider_div2 = document.createElement("div");
	    tab.getEl().dom.lastChild.appendChild(grider_div2);
	    grider_div3 = document.createElement("div");
	    tab.getEl().dom.lastChild.appendChild(grider_div3);

	    // Gift Redemption
	    target_div_gr = document.createElement("div");
	    tab.getEl().dom.lastChild.appendChild(target_div_gr);
	    grider_div_gr = document.createElement("div");
	    tab.getEl().dom.lastChild.appendChild(grider_div_gr);

	    // Service Contract
	    //target_div_sc = document.createElement("div");
	    //tab.getEl().dom.lastChild.appendChild(target_div_sc);
	    //grider_div_sc = document.createElement("div");
	    //tab.getEl().dom.lastChild.appendChild(grider_div_sc);

	    // Service Payment
	    target_div_sp = document.createElement("div");
	    tab.getEl().dom.lastChild.appendChild(target_div_sp);
	    grider_div_sp = document.createElement("div");
	    tab.getEl().dom.lastChild.appendChild(grider_div_sp);

	    // Product Purchase
	    target_div_pp = document.createElement("div");
	    tab.getEl().dom.lastChild.appendChild(target_div_pp);
	    grider_div_pp = document.createElement("div");
	    tab.getEl().dom.lastChild.appendChild(grider_div_pp);

	    // Passcode Registry
	    //target_div_pr = document.createElement("div");
	    //tab.getEl().dom.lastChild.appendChild(target_div_pr);
	    //grider_div_pr = document.createElement("div");
	    //tab.getEl().dom.lastChild.appendChild(grider_div_pr);

        // ---

        // view edit form
        var form = new com.embraiz.component.form();
        form.viewForm(target_div, '../member/getModule/' + _id);
        form.showToolBar(tool_div, '../member/ToolbarData/' + _id, id);

	    // Transaction
        Ext.Ajax.request({
            url: "../table/InitWithSearchColumn/transaction/" + _id,
            async: false,
            success: function (o) {
                var data_json = Ext.decode(o.responseText);
                new com.embraiz.component.gridSearch().render(grider_div2, grider_div3, data_json, true);
            },
            scope: this
        });

        // Gift Redemption
        Ext.Ajax.request({
            url: "../table/InitWithSearchColumn/giftredemption/" + _id,
            async: false,
            success: function (o) {
                var data_json = Ext.decode(o.responseText);
                new com.embraiz.component.gridSearch().render(target_div_gr, grider_div_gr, data_json, true);
            },
            scope: this
        });

	    // Service Contract
        //Ext.Ajax.request({
        //    url: "../table/InitWithSearchColumn/serviceContract/" + _id,
        //    async: true,
        //    success: function (o) {
        //        var data_json = Ext.decode(o.responseText);
        //        new com.embraiz.component.gridSearch().render(target_div_sc, grider_div_sc, data_json, true);
        //    },
        //    scope: this
        //});

	    // Service Payment
        //Ext.Ajax.request({
        //    url: "../table/InitWithSearchColumn/servicePayment/" + _id,
        //    async: true,
        //    success: function (o) {
        //        var data_json = Ext.decode(o.responseText);
        //        new com.embraiz.component.gridSearch().render(target_div_sp, grider_div_sp, data_json, true);
        //    },
        //    scope: this
        //});

	    // Product Purchase
        Ext.Ajax.request({
            url: "../table/InitWithSearchColumn/ProductPurchase/" + _id,
            async: false,
            success: function (o) {
                var data_json = Ext.decode(o.responseText);
                new com.embraiz.component.gridSearch().render(target_div_pp, grider_div_pp, data_json, true);
            },
            scope: this
        });

	    // Passcode Registry
        //Ext.Ajax.request({
        //    url: "../table/InitWithSearchColumn/passcodeRegistry/" + _id,
        //    async: true,
        //    success: function (o) {
        //        var data_json = Ext.decode(o.responseText);
        //        new com.embraiz.component.gridSearch().render(target_div_pr, grider_div_pr, data_json, true);
        //    },
        //    scope: this
        //});
	}
});