function form_layout(e){
	var list_tr  = e.container.query('.editform table.x-table-layout>tbody>tr');
	
	for(i=0;i<list_tr.length;i++){
		var tr = list_tr[i];
		if(i%2==0){
			tr.style.backgroundColor = '#EFEFEF';
		}else{
			tr.style.backgroundColor = '#ffffff';
		}
		var trHeight = tr.offsetHeight;
		var innerTable = Ext.get(tr).query('table.x-form-item');
		for(j=0;j<innerTable.length;j++){
			if(Ext.isIE){
			innerTable[j].style.height = (trHeight+5)+'px';
			}
			var tdLabel = Ext.get(innerTable[j]).query('.x-field-label-cell');
			var tdItem = Ext.get(innerTable[j]).query('.x-form-item-body');
			tdLabel[0].style.borderRight = '1px solid #A9BFD3'; 
		}

		
		if(trHeight<2){
			var tdhidden = Ext.get(tr).query('td.x-table-layout-cell:not(:has(div))');
			for(j=0;j<tdhidden.length;j++){
				tdhidden[j].style.border = 'none';
			}
		}
	}
}