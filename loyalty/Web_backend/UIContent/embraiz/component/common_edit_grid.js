Ext.define('com.embraiz.common.js.editGrid', {

	gridPanel : undefined,
	grid : function(tab, id, parentType, parentIdField, parentTypeField) {

		for ( var i = 5; i < arguments.length; i++) {
			var grider_div = document.createElement("div");
			tab.getEl().dom.lastChild.appendChild(grider_div);
			new com.embraiz.component.girdTable().initGrid(
					'common/gird_header.jsp?module_name=' + arguments[i],
					'common/grid_data.jsp?module_name=' + arguments[i] + "&id="
					+id+"&parentType="+parentType+"&parentIdField="+parentIdField+"&parentTypeField="+parentTypeField, grider_div);

		}
	}
})