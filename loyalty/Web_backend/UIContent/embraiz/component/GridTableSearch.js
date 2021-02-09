Ext.define('com.embraiz.component.girdTableSearch', {
	render:function(target_div,grider_div,search_form_url){
		    Ext.Ajax.request({
		 		url: search_form_url,
		 		success:showGrid,
		 		scope: this		
	 		});	
	 		
	 		function showGrid(o){
		 	   target_div.style.margin="5px";
				 grider_div.style.margin="5px";	
		 			var gird_info = Ext.decode(o.responseText);	 	
		 
		    	var gridTable=new com.embraiz.component.girdTable();	
		    	gridTable.initGrid(gird_info.post_header,gird_info.post_url,grider_div);
			
		 		 	if(gird_info.row!=undefined&&gird_info.row!=null&&gird_info.row!=""){
		 			var forms=new com.embraiz.component.form();
					forms.editForm(target_div, gird_info,gridTable);
				}
			}	
		}
});