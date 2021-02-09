Ext.define('com.embraiz.component.gridkeyboard', {
    keyboard:function(divarr){
          var key=new Array(85,73,79,80);
          //var divarr=Ext.query("*[class=gridWrapper]");
          Ext.each(divarr,function(o,i){
              new Ext.util.KeyMap(Ext.getCmp('content-panel').getActiveTab().id,{
			  key:key[i],
			  alt:true,
			  fn:function(){//Ext.Msg.alert('sf');
			       var divgrid=Ext.query("*[class=gridWrapper]");
			       Ext.each(divgrid,function(o,j){
			           var tabarr=Ext.query("table",divgrid[j]);
			           for(var m=0;m<tabarr[0].rows.length;m++){
			              tabarr[0].rows[m].bgColor="#FFFFFF"; 
			           }
			       });
			       divarr[i].focus();
	               var tab=Ext.query("table",divarr[i]);
	               for(var j=0;j<divarr.length;j++){
	                  if(j!=i){
	                     divarr[j].removeAttribute("tabindex");
	                  }
	               }
	               if(tab[0].rows.length>=2){
	                  tab[0].rows[1].bgColor="#8db2e3";
	               }
			       key_event(divarr[i],tab);

			  }
		  });
          });
	     function key_event(div,tab){
	     div.setAttribute("tabindex","1");
         div.focus();
	     var rowNo= -1; 
         var selectedColor = "#8db2e3";
		 new Ext.util.KeyMap(div,[{
		     key:38,
		     fn:function(key,e){
		     // var gridtable=getElementsByClassName('gridTable','table');
		     for(var k=0;k<tab[0].rows.length;k++) 
		     { 
				 tab[0].rows[k].bgColor="#FFFFFF"; 

			 } 
			 if(rowNo == 0) 
			 { 
				 rowNo++; 
			 }
			 tab[0].rows[--rowNo%tab[0].rows.length].bgColor=selectedColor;
			 e.preventDefault(); 
			 return false;		            
			 },scope:div,
			 stopEvent: false
		 },
		 {
		     key:40,
		     fn:function(key,e){
		        for(var k=1;k<tab[0].rows.length;k++) 
				{ 
					tab[0].rows[k].bgColor="#FFFFFF"; 
				}
				tab[0].rows[++rowNo%tab[0].rows.length].bgColor=selectedColor;
				if(tab[0].rows[1].bgColor==selectedColor)
				tab[0].rows[0].bgColor="#FFFFFF"; 
				e.preventDefault();
				return false;
			 },scope:div,
			 stopEvent: false
		 }]);
		 //    
	 }
    }
});