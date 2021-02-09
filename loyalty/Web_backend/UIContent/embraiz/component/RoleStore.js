 Ext.onReady(function(){
		Ext.define("roleStore",{})
		//roleStore.test="test";
		roleStore.rStore = new Ext.data.Store({
			fields:[
				{name:'menuId'},
				{name:'module'},
				{name:'rightR'},
				{name:'rightU'},
				{name:'rightD'},
				{name:'rightI'},
				{name:'rightL'},
				{name:'rightE'}
			],			

			proxy:{
				type:'ajax',
				url:'common/role_menu_by_user.jsp',
				reader: {
                    type: 'json',
                    root: 'items'
                }
			},
			//model:'person',
			autoLoad:true
		});
//		s.each(function(model){
//			alert(model.get('name'));
//		});
		/*s.load(function(records, operation, success){
			Ext.Array.each(records,function(model){
				alert(model.get('menuId'));
			});
			
		});*/
	})

