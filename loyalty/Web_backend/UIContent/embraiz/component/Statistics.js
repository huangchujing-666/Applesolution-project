Ext.onReady(function() {
	var a = 0;
	Ext.define('com.embraiz.component.statistics', {
		headerJson : undefined,
		data_url : undefined,
		target_div : undefined,

		render : function(target_div, header_url, data_url, b) {
			var me = this;
			a = b;
			target_div.style.margin = "5px";
			target_div.className = "graphic_panel";
			me.target_div = target_div;
			me.data_url = data_url;
			Ext.Ajax.request( {
				url : header_url,
				async : true,
				success : me.header_response,
				scope : this
			});
		},
		header_response : function(o) {
			var me = this;
			me.headerJson = Ext.decode(o.responseText);
			var fields = me.headerJson.fields;
			var fieldString = Ext.encode(fields);
			var file = fieldString.split(',');
			var yfields = me.headerJson.yfields;
			if (me.headerJson.isdate) {

				Ext.define('chartModel', {
					extend : 'Ext.data.Model',
					fields : fields
				});

				Ext.chart.theme.myTheme = Ext.extend(Ext.chart.theme.Base, {
					constructor : function(config) {
						Ext.chart.theme.Base.prototype.constructor.call(this,
								Ext.apply( {
									colors : [ '#4F94CD', '#FF0000', '#7FFF00',
											'#EE30A7', '#8B7B8B', '#00EE00',
											'#5B5B5B', '#8B2500', '#D2691E',
											'#E9967A', '#191970', '#CD919E',
											'#00688B', '#CD2626', '#71C671',
											'#CAFF70', '#8B8970', '#388E8E',
											'#8B0000', '#CDCDB4', '#6E8B3D',
											'#DAA520', '#0000EE', '#2F4F4F',
											'#EEDC82', '#AB82FF', '#8B2323',
											'#228B22', '#B8860B', '#EE4000' ],

									series : {
										'stroke-opacity' : 0
									// 'stroke-width': 'none' ,//0表示无边线，1表示有边线
									// 'stroke':'#FFFFFF'
									}

								}, config));
					}
				});

				var store = Ext.create('Ext.data.JsonStore', {
					model : 'chartModel',
					proxy : {
						type : 'ajax',
						url : me.data_url,
						reader : {
							type : 'json',
							root : 'items'
						}
					},
					autoLoad : true
				});
				var label = {};
				if (a == 1) {
					label = {
						rotate : {
							degrees : 270
						}
					};
				} else {

				}
				Ext.create('Ext.chart.Chart', {
					renderTo : me.target_div,
					width : 1000,
					height : 500,
					animate : true,
					store : store,
					shadow : true,
					theme : 'myTheme', // 'Category1',
					legend : {
						position : 'right'

					},
					axes : [ {
						type : 'Numeric',
						position : 'left',
						fields : yfields,
						minimum : 0,
						label : {
							renderer : Ext.util.Format.numberRenderer('0,0')
						},
						grid : true
					}, {
						type : 'Category',
						position : 'bottom',
						fields : [ 'name' ],
						label : label
					} ],
					series : [ {
						type : 'column',
						axis : 'left',
						label : {
							display : 'insideEnd',
							'text-anchor' : 'middle',
							field : yfields,
							renderer : Ext.util.Format.numberRenderer('0'),
							orientation : 'vertical',
							color : '#333'
						},

						xField : 'name',
						yField : yfields
					} ]
				});

			}

		}
	});
});