Ext.define('com.palmary.calendar.js.index', {
    target_div: undefined,
    searchPanel: undefined,//搜索框
    firstCalendarPanel: undefined,//初始的日历
    calendarPanel: undefined,//加载数据后的日历
    timeTableId: undefined,

    // set time
    times: [
	       { time: '07:00-07:15' }, { time: '07:15-07:30' }, { time: '07:30-07:45' }, { time: '07:45-08:00' }, { time: '08:00-08:15' }, { time: '08:15-08:30' },
	       { time: '08:30-08:45' }, { time: '08:45-09:00' },
	 		{ time: '09:00-09:15' }, { time: '09:15-09:30' }, { time: '09:30-09:45' }, { time: '09:45-10:00' }, { time: '10:00-10:15' }, { time: '10:15-10:30' },
	  		{ time: '10:30-10:45' }, { time: '10:45-11:00' }, { time: '11:00-11:15' }, { time: '11:15-11:30' }, { time: '11:30-11:45' }, { time: '11:45-12:00' },
	        { time: '12:00-12:15' }, { time: '12:15-12:30' }, { time: '12:30-12:45' }, { time: '12:45-13:00' }, { time: '13:00-13:15' }, { time: '13:15-13:30' },
	  		{ time: '13:30-13:45' }, { time: '13:45-14:00' }, { time: '14:00-14:15' }, { time: '14:15-14:30' }, { time: '14:30-14:45' }, { time: '14:45-15:00' },
	  		{ time: '15:00-15:15' }, { time: '15:15-15:30' }, { time: '15:30-15:45' }, { time: '15:45-16:00' }, { time: '16:00-16:15' }, { time: '16:15-16:30' },
	  		{ time: '16:30-16:45' }, { time: '16:45-17:00' }, { time: '17:00-17:15' }, { time: '17:15-17:30' }, { time: '17:30-17:45' }, { time: '17:45-18:00' },
	  		{ time: '18:00-18:15' }, { time: '18:15-18:30' }, { time: '18:30-18:45' }, { time: '18:45-19:00' }, { time: '19:00-19:15' }, { time: '19:15-19:30' },
	  		{ time: '19:30-19:45' }, { time: '19:45-20:00' }, { time: '20:00-20:15' }, { time: '20:15-20:30' }, { time: '20:30-20:45' }, { time: '20:45-21:00' }
    ],

    initTag: function (tab, url, title, id, itemId, extra) {
        var me = this;

        // serach div
        tool_div = document.createElement("div");
        tool_div.style.margin = "5px 5px 5px 5px";
        tab.getEl().dom.lastChild.appendChild(tool_div);

        // calendar div
        target_div = me.target_div = document.createElement("div");
        target_div.style.margin = "0px 5px 5px 5px";
        tab.getEl().dom.lastChild.appendChild(target_div);

        me.timeTableId = id;

        //搜索框
        var searchPanel = me.searchPanel = Ext.create('Ext.panel.Panel', {
            title: 'Instructor Schedule',
            width: '100%',
            renderTo: tool_div,
            iconCls: 'iconScheduleSearch',
            layout: 'form',
            items: [{
                anchor: '90%',
                layout: {
                    type: 'table',
                    columns: 2,
                    tableAttrs: {
                        style: {
                            width: '100%',
                            margin: '0px 0px 0px 0px'
                        }
                    },
                    tdAttrs: {
                        style: {
                            height: 40,
                            padding: '0px 0px 0px 10px'
                        }
                    }
                },

                items: [
                       {
                           xtype: 'combobox',
                           fieldLabel: 'View',
                           name: 'view',
                           displayField: 'value',
                           valueField: 'id',
                           forceSelection: true,
                           width: 400,
                           value: 1,
                           store: Ext.create('Ext.data.Store', {
                               fields: ['id', 'value'],
                               autoLoad: true,
                               data: [{ "id": 1, "value": "Daily" },
                                    { "id": 2, "value": "Weekly" }]
                           }),
                           queryMode: 'local',
                           listeners: {
                               //extjs选择框设置默认值
                               render: function () {
                                   var temp = this;
                                   temp.store.load(function (records, operation, success) {
                                       if (records.length == 1) {
                                           temp.setValue(records[0].raw.id);
                                       }
                                       temp.fireEvent('select', this);
                                   });
                               }
                           }
                       },
                        {
                            xtype: 'datefield',
                            fieldLabel: 'Date',
                            name: 'dateWeek',
                            itemId: 'dateWeek',
                            format: 'Y-m-d',
                            altFormats: 'ymd|Ymd|Y-m-d|Y.m.d|y.m.d',
                            value: Ext.Date.format(new Date(), 'Y-m-d'),
                            width: 400
                        },/*{
							xtype:'combobox',
							fieldLabel:'Instructor',
							name:'instructor',
							itemId:'instructor',
							emptyText: 'Please Select',
							multiSelect:true,
			                displayField: 'instructorName', 
			                valueField: 'instructorId',
			                forceSelection:true, 
			                width:400,
			                store:  Ext.create('Ext.data.Store', {
			                    fields: [{
			                        name: 'instructorId',
			                        type: 'string'
			                    }, {
			                        name: 'instructorName',
			                        type: 'string'
			                    }],
			                    autoLoad: true,		                       
			                    proxy: {
			                        type: 'ajax',
			                        url: '../Calendar/instructor',
			                        reader: {
			                            type: 'json',
			                            root: 'data'
			                        }
			                    }
			                })
			              }*/
                        Ext.create('Ext.ux.ComboFieldBox', {
                            fieldLabel: 'Instructor',
                            name: 'instructor',
                            itemId: 'instructor',
                            emptyText: 'Please Select',
                            multiSelect: true,
                            displayField: 'instructorName',
                            valueField: 'instructorId',
                            forceSelection: true,
                            width: 400,
                            store: Ext.create('Ext.data.Store', {
                                fields: [{
                                    name: 'instructorId',
                                    type: 'string'
                                }, {
                                    name: 'instructorName',
                                    type: 'string'
                                }],
                                autoLoad: true,
                                proxy: {
                                    type: 'ajax',
                                    url: '../Calendar/instructor',
                                    reader: {
                                        type: 'json',
                                        root: 'data'
                                    }
                                }
                            }),
                            queryMode: 'local',
                            cls: 'readonlyField',
                            listeners: {
                                render: function () {
                                    var me = this;
                                    if (me.store != null) {
                                        me.store.load(function (records, operation, success) {
                                            me.setValue(me.lastValue);
                                        });
                                    }
                                },
                                select: function () {
                                    var me = this;
                                    me.setValue(me.lastValue);
                                }
                            }
                        })
                ],
                buttons: {
                    align: 'left',
                    items: [{
                        text: 'View',
                        handler: function () {
                            //search下的items的第一项items[0]中的items的第一项items[0]
                            var view = searchPanel.items.items[0].items.items[0].getValue();
                            var date = searchPanel.items.items[0].items.items[1].getRawValue();
                            //下拉框一开始没有select动作的时候，value是一个“”的字符串，长度为0
                            //下拉框一旦有select动作，value就变成一个数组，长度始终大于等于1，此时如果没有选项value是长度为1且第一项为“”的数组
                            var instructorId = searchPanel.items.items[0].items.items[2].getValue();
                            //编辑instructorId数组，以“，”隔开，在后台在根据“，”拆分成数组
                            var instructorIds = "";
                            for (var i = 0; i < instructorId.length; i++) {
                                if (i == 0) {
                                    instructorIds = instructorId[i];
                                } else {
                                    instructorIds = instructorIds + "," + instructorId[i];
                                }
                            }
                            /*for(var i =0;i<instructorId.length;i++){
                                if(i==0){
                                    if(instructorId[i]!=""){
                                        instructorIds = instructorId[i].data.instructorId;
                                    }
                                }else{
                                    instructorIds = instructorIds+","+instructorId[i].data.instructorId; 
                                }
                            }*/
                            me.calendar_init(view, date, instructorIds, id);
                        }
                    }, {
                        text: 'Download',
                        //点击打开一个新的html页面，并写入内容
                        handler: function () {
                            //var html = me.target_div.innerHTML;
                            var html = Ext.get('calendarDiv' + id).dom.innerHTML;

                            var win = window.open("../UIContent/modules/Calendar/calendar.html");
                            win.document.write("<html><head><title>calendar.html</title>");
                            win.document.write("<meta http-equiv='content-type' content='text/html; charset=UTF-8'>");
                            win.document.write("<link rel='stylesheet' type='text/css' href='../UIContent/modules/Calendar/css/calendarHtml.css'></head>");
                            win.document.write("<body><div><div class='fix'>" + Ext.get('fixTableDiv' + id).dom.innerHTML + "</div>");
                            win.document.write("<div class='header'>" + Ext.get('HeaderTableDiv' + id).dom.innerHTML + "</div>");
                            win.document.write("<div class='col'>" + Ext.get('colTableDiv' + id).dom.innerHTML + "</div>");
                            win.document.write("<div class='data'>" + Ext.get('dataTableDiv' + id).dom.innerHTML + "</div></div></body></html>");
                        }
                    }, {
                        text: 'Cancel',
                        handler: function () {
                            searchPanel.items.items[0].items.items[1].setValue(Ext.Date.format(new Date(), 'Y-m-d'));
                            searchPanel.items.items[0].items.items[2].setValue('');
                        }
                    }]
                }
            }
            ]
        });

        //初始加载日历表，默认view为daily，日期为当前时间
        var today = new Date();
        me.calendar_init("1", Ext.Date.format(today, 'Y-m-d'), " ", id);
    },

    //滚动条联动div事件
    /*scrollMove:function(){
		var me = this;
		var id = me.timeTableId;
		var top = Ext.fly('dataTableDiv'+id).dom.scrollTop;
		//Ext.fly('colTableDiv').dom.style.top = (70-top)+"px";
		Ext.fly('colTableDiv'+id).setStyle("top",(70-top)+"px");
		var left = Ext.fly('dataTableDiv'+id).dom.scrollLeft;
		//Ext.fly('HeaderTableDiv').dom.style.left = (96-left)+"px";
		Ext.fly('HeaderTableDiv'+id).setStyle("left",(96-left)+"px");
	},*/

    //加载日历表事件,这里的date已转成年月日格式
    calendar_init: function (view, date, instructorId, id) {
        var me = this;
        target_div = me.target_div;
        //删除原日历表
        while (target_div.hasChildNodes()) //remove 
        {
            //IE9必须先清除listener
            //Ext.fly('dataTableDiv'+id).removeListener('scroll',me.scrollMove);
            Ext.fly('dataTableDiv' + id).removeListener('scroll', function () {
                var top = Ext.fly('dataTableDiv' + id).dom.scrollTop;
                Ext.fly('colTableDiv' + id).setStyle("top", (70 - top) + "px");
                var left = Ext.fly('dataTableDiv' + id).dom.scrollLeft;
                Ext.fly('HeaderTableDiv' + id).setStyle("left", (96 - left) + "px");
            });
            target_div.removeChild(target_div.firstChild);
        }

        //建立新的日历表panel
        var calendarPanel = me.calendarPanel = Ext.create('Ext.panel.Panel', {
            title: 'CalendarPanel',
            width: '100%',
            height: 680,
            renderTo: target_div,
            iconCls: 'iconCalendar',
            //给panel设置滚动条
            /*autoScroll:true,
			bodyStyle:'overflow-x:scroll; overflow-y:scroll',*/
            html: '<div id = "calendarDiv' + id + '" >' +
					'<div class="up"><div id="fixTableDiv' + id + '" class="fixTableDiv"></div>' +
					'<div id="HeaderTableDiv' + id + '" class="HeaderTableDiv"></div></div>' +
					'<div id="colTableDiv' + id + '" class="colTableDiv"></div>' +
					'<div id="dataTableDiv' + id + '" class="dataTableDiv"></div>' +
				'</div>',
            listeners: {
                render: function (m) {
                    //滚动条联动div事件
                    //Ext.fly('dataTableDiv'+id).on('scroll',me.scrollMove);
                    Ext.fly('dataTableDiv' + id).on('scroll', function () {
                        var top = Ext.fly('dataTableDiv' + id).dom.scrollTop;
                        Ext.fly('colTableDiv' + id).setStyle("top", (70 - top) + "px");
                        var left = Ext.fly('dataTableDiv' + id).dom.scrollLeft;
                        Ext.fly('HeaderTableDiv' + id).setStyle("left", (96 - left) + "px");
                    });
                }
            }
        });

        //设置dataDiv的height和width
        var dataTableDiv = Ext.fly('dataTableDiv' + id);
        var h1 = me.calendarPanel.body.dom.clientHeight - 71;
        dataTableDiv.setStyle('height', h1 + "px");
        var w1 = me.calendarPanel.body.dom.clientWidth - 101;
        dataTableDiv.setStyle('width', w1 + "px");

        //view是daily
        if (view == 1) {
            me.daily_init(date, instructorId, calendarPanel, id);
        }
            //view是weekly
        else {
            me.weekly_init(date, instructorId, calendarPanel, id);
        }

    },



    //daily日历表生成
    daily_init: function (date, instructorId, calendarPanel, id) {
        var me = this;
        Ext.Ajax.request({
            url: '../calendar/daily',
            //传出的值是日期date和instructor的id（若没有选择，则为空）
            params: {
                date: date,
                instructorId: instructorId
            },
            success: function (o) {
                //获得的数据包括instructors(instructor集合)，information(内容信息),背景颜色color
                var data_json = Ext.decode(o.responseText);

                //根据日期获取星期的事件
                var nowDate = new Date(Date.parse(date.replace(/-/g, "/")));//将date转回Date格式
                var dayNames = new Array("(Sun)", "(Mon)", "(Tue)", "(Wed)", "(Thu)", "(Fri)", "(Sat)");
                var dateForWeek = dayNames[nowDate.getDay()];

                //获得instructor的个数，用于th的colspan
                var instructorNum = data_json.instructors.length;
                if (instructorNum == 0) {
                    instructorNum = 1;
                }

                //编辑数据集data
                var data = {
                    times: me.times,
                    instructors: data_json.instructors
                };

                //表头的固定table
                var tableFix = new Ext.XTemplate(
						'<table id="fixTable' + id + '" cellspacing="0" class="fixTable" >',
							'<tr>',
								'<th width="100px">Daily</th>',
							'</tr>',
							'<tr>',
								'<td width="100px" style="background-color:white;border:1px solid #E1E1E1;"><div class="calendar_instrutor_td_div"></div></td>',
							'</tr>',
						'</table>'
				);
                //获取固定表头的div
                var fixTableDiv = Ext.fly('fixTableDiv' + id);
                tableFix.overwrite(fixTableDiv, data);

                //标题table
                var tableHeader = new Ext.XTemplate(
						'<table id="HeaderTable' + id + '" cellspacing="0" class="HeaderTable" width="' + (instructorNum * 120) + '">',
							//日期行
							'<tr>',
								'<th colspan = "' + instructorNum + '">' + date + '<br/>' + dateForWeek + '</th>',
							'</tr>',
							//instructor行
							'<tr>',
								'<tpl for="instructors">',
		 							'<td><div class="calendar_instrutor_td_div">{instructorName}</div></td>',
		 						'</tpl>',
		 					'</tr>',
						'</table>'
				);
                //获取标题table的div
                var HeaderTableDiv = Ext.fly('HeaderTableDiv' + id);
                tableHeader.overwrite(HeaderTableDiv, data);
                //修改HeaderTableDiv的长度，增加scroll的宽度
                var len = Ext.fly('HeaderTableDiv' + id).dom.clientWidth + 17;
                Ext.fly('HeaderTableDiv' + id).setStyle('width', len + "px");

                //时刻表table
                var tableCol = new Ext.XTemplate(
						'<table id="TimeTable' + id + '" cellspacing="0" class="TimeTable">',
							'<tpl for="times">',
		 						'<tr>',
		 							'<td><div class="calendar_time_div">{time}</div></td>',
		 						'</tr>',
		 					'</tpl>',
						'</table>'
				);
                //获取时刻表table的div
                var colTableDiv = Ext.fly('colTableDiv' + id);
                tableCol.overwrite(colTableDiv, data);

                //数据table
                var tableData = new Ext.XTemplate(
						'<table id="dataTable' + id + '" cellspacing="0" class="calendarTable" width="' + (instructorNum * 120) + '">',
						'<tr>',
 		 					'<tpl for="instructors">',
 		 						//id是tag的id，代表该div是唯一的。命名规则：(daily)-(tag的id)-(instructorId)
 		 						'<td><div class="calendar_td_div"><div id="daily-' + id + '-{instructorId}"></div></div></td>',
	 						'</tpl>',
						'</tr>',
						'</table>'
				);
                //获取时刻表table的div
                var dataTableDiv = Ext.fly('dataTableDiv' + id);
                tableData.overwrite(dataTableDiv, data);

                //加载日历表的内容
                if (data_json.information.length > 0) {
                    var information = data_json.information;
                    for (var i = 0; i < information.length; i++) {
                        var inforJson = information[i];
                        if (inforJson.time_length != null && inforJson.start_time != null) {
                            var timeLength = inforJson.time_length;
                            var startTime = inforJson.start_time;

                            var height = parseFloat(timeLength) / 15 * 25 - 4;//计算长度
                            var top = me.getDivTop(startTime);//计算高度
                            if (height >= 1396) {
                                height = 1394;
                            };

                            var div = document.createElement("div");
                            div.className = "calendar_class_div";//另外一个样式是calendar_leave_div
                            div.style.top = top + "px";
                            div.style.height = height + "px";
                            div.style.background = inforJson.color;
                            me.entryData(1, div, inforJson);
                            //div.innerHTML=inforJson.content;//在div中inner数据，可以创建一个方法自定义录入数据
                            //						div.id = "";//设置div的id
                            Ext.fly('daily-' + id + '-' + inforJson.instructorId).dom.appendChild(div);//找到对应的div
                        }
                    }
                }
            }
        });
    },

    //weekly日历表生成
    weekly_init: function (date, instructorId, calendarPanel, id) {
        var me = this;
        Ext.Ajax.request({
            url: '../Calendar/weekly',
            //传出的值是日期date和instructor的id（若没有选择，则为空）
            params: {
                date: date,
                instructorId: instructorId
            },
            success: function (o) {
                //获得的数据包括weekly(整个星期)，instructors(instructor集合)，information(内容信息)，背景颜色color
                var data_json = Ext.decode(o.responseText);

                //获得instructor的个数，用于th的colspan
                var instructorNum = data_json.instructors.length;
                if (instructorNum == 0) {
                    instructorNum = 1;
                }

                //给date日期的json添加css属性项
                var weekDays = data_json.weekDays;
                for (var i = 0; i < 7; i++) {




                    weekDays[i].num = i + 1;





                }
                /*for(var i=0;i<7;i++){
					if(i==0){
						weekDays[i].style = "margin-right:20px;color:white;background:red";
					}else if(i==6){
						weekDays[i].style = "margin-right:20px;color:blue;background:yellow";
					}else{
						weekDays[i].style = "margin-right:20px";
					}
				}*/
                //编辑数据集data

                var data = {
                    times: me.times,
                    instructors: data_json.instructors,
                    timeTable_id: id,
                    weekDays: weekDays
                };

                //表头的固定table
                var tableFix = new Ext.XTemplate(
						'<table id="fixTable' + id + '" cellspacing="0" class="fixTable" >',
							'<tr>',
								'<th width="100px">Weekly</th>',
							'</tr>',
							'<tr>',
								'<td width="100px" style="background-color:white;border:1px solid #E1E1E1;"><div class="calendar_instrutor_td_div"></div></td>',
							'</tr>',
						'</table>'
				);
                //获取固定表头的div
                var fixTableDiv = Ext.fly('fixTableDiv' + id);
                tableFix.overwrite(fixTableDiv, data);

                //标题table
                var tableHeader = new Ext.XTemplate(
						'<table id="HeaderTable' + id + '" cellspacing="0" class="HeaderTable" width="' + ((instructorNum * 120) * 7) + '">',
							//日期行
							'<tr height="40px">',
								'<tpl for="weekDays">',
									//'<tpl if="num == 1">',
									//	'<th colspan = "' + instructorNum + '"  style="margin-right:20px;color:white;background:red">{day}</th>',
									//'<tpl elseif="num == 7">',
									//	'<th colspan = "' + instructorNum + '"  style="margin-right:20px;color:blue;background:yellow">{day}</th>',
									//'<tpl else>',
									//	'<th colspan = "' + instructorNum + '"  style="margin-right:20px">{day}</th>',
									//'</tpl>',
                                    '<th colspan = "' + instructorNum + '"  style="margin-right:20px;{colorStyle}">{day}</th>',
								'</tpl>',
							'</tr>',
							//instructor行
							'<tr>',
								'<tpl for="instructors">',
			 						'<td><div class="calendar_instrutor_td_div">{instructorName}</div></td>',
			 					'</tpl>',
			 					'<tpl for="instructors">',
		 							'<td><div class="calendar_instrutor_td_div">{instructorName}</div></td>',
		 						'</tpl>',
		 						'<tpl for="instructors">',
			 						'<td><div class="calendar_instrutor_td_div">{instructorName}</div></td>',
			 					'</tpl>',
	 		 					'<tpl for="instructors">',
			 						'<td><div class="calendar_instrutor_td_div">{instructorName}</div></td>',
		 						'</tpl>',
			 					'<tpl for="instructors">',
		 							'<td><div class="calendar_instrutor_td_div">{instructorName}</div></td>',
		 						'</tpl>',
		 						'<tpl for="instructors">',
		 							'<td><div class="calendar_instrutor_td_div">{instructorName}</div></td>',
		 						'</tpl>',
		 						'<tpl for="instructors">',
		 							'<td><div class="calendar_instrutor_td_div">{instructorName}</div></td>',
		 						'</tpl>',
			 				'</tr>',
						'</table>'
				);
                //获取标题table的div
                var HeaderTableDiv = Ext.fly('HeaderTableDiv' + id);
                tableHeader.overwrite(HeaderTableDiv, data);
                //修改HeaderTableDiv的长度，增加scroll的宽度
                var len = Ext.fly('HeaderTableDiv' + id).dom.clientWidth + 17;
                Ext.fly('HeaderTableDiv' + id).setStyle('width', len + "px");
                //给th的星期六和星期日修改背景字体样式
                //Ext.fly('day-Sun').setStyle({color:'white',background:'red'});
                //Ext.fly('day-Sat').setStyle({color:'blue',background:'yellow'});

                //时刻表table
                var tableCol = new Ext.XTemplate(
						'<table id="TimeTable' + id + '" cellspacing="0" class="TimeTable">',
							'<tpl for="times">',
		 						'<tr>',
		 							'<td><div class="calendar_time_div">{time}</div></td>',
		 						'</tr>',
		 					'</tpl>',
						'</table>'
				);
                //获取时刻表table的div
                var colTableDiv = Ext.fly('colTableDiv' + id);
                tableCol.overwrite(colTableDiv, data);

                //数据table
                var tableData = new Ext.XTemplate(
						'<table id="dataTable' + id + '" cellspacing="0" class="calendarTable" width="' + ((instructorNum * 120) * 7) + '">',
						//td日历表内容,分为七个tpl，代表七天，tpl为instructors
		 					'<tr>',
		 		 				'<tpl for="instructors">',
		 		 					//id是tag的id，代表该div是唯一的。{id}是instructors的id。命名规则：(daily)-(tag的id)-(星期几1~7)-(instructorId)
		 		 					'<td><div class="calendar_td_div"><div id="weekly-' + id + '-1-' + '{instructorId}"> </div></div></td>',
			 					'</tpl>',
			 					'<tpl for="instructors">',
		 		 					'<td><div class="calendar_td_div"><div id="weekly-' + id + '-2-' + '{instructorId}"> </div></div></td>',
	 		 					'</tpl>',
			 						'<tpl for="instructors">',
	 		 						'<td><div class="calendar_td_div"><div id="weekly-' + id + '-3-' + '{instructorId}"> </div></div></td>',
	 		 					'</tpl>',
			 						'<tpl for="instructors">',
		 		 					'<td><div class="calendar_td_div"><div id="weekly-' + id + '-4-' + '{instructorId}"> </div></div></td>',
		 						'</tpl>',
		 						'<tpl for="instructors">',
		 		 					'<td><div class="calendar_td_div"><div id="weekly-' + id + '-5-' + '{instructorId}"> </div></div></td>',
		 						'</tpl>',
		 						'<tpl for="instructors">',
		 		 					'<td><div class="calendar_td_div"><div id="weekly-' + id + '-6-' + '{instructorId}"> </div></div></td>',
		 						'</tpl>',
		 						'<tpl for="instructors">',
		 		 					'<td><div class="calendar_td_div"><div id="weekly-' + id + '-7-' + '{instructorId}"> </div></div></td>',
		 						'</tpl>',
	 						'</tr>',
						'</table>'
				);
                //获取时刻表table的div
                var dataTableDiv = Ext.fly('dataTableDiv' + id);
                tableData.overwrite(dataTableDiv, data);

                //加载日历表的内容，利用绝对定位


                if (data_json.information.length > 0) {
                    var information = data_json.information;
                    for (var i = 0; i < information.length; i++) {
                        var inforJson = information[i];
                        if (inforJson.time_length != null && inforJson.start_time != null) {
                            var timeLength = inforJson.time_length;
                            var startTime = inforJson.start_time;
                            var height = parseFloat(inforJson.time_length) / 15 * 25 - 4;//计算长度
                            var top = me.getDivTop(inforJson.start_time);//计算高度
                            if (height >= 1396) {
                                height = 1394;
                            };

                            var div = document.createElement("div");
                            div.className = "calendar_class_div";//另外一个样式是calendar_leave_div
                            div.style.top = top + "px";
                            div.style.height = height + "px";
                            div.style.background = inforJson.color;
                            me.entryData(2, div, inforJson);
                            //div.innerHTML=inforJson.content;//在div中inner数据，可以创建一个方法自定义录入数据
                            //						div.id = "";//设置div的id
                            Ext.fly('weekly-' + id + '-' + inforJson.week_day + '-' + inforJson.instructorId).dom.appendChild(div);//找到对应的div
                        }
                    }
                }
            }
        });
    },

    //数据录入
    entryData: function (view, div, data) {
        var me = this;
        div.innerHTML = "</br>" + data.content + "</br></br>";
        //按钮生成
        var button_text = data.button_text;//button的内容
        var button_url = data.button_url;//tag的url



        if (button_text != "" && button_text != null) {
            var a1 = document.createElement("a");
            a1.id = "url" + me.timeTableId + "-" + view + "-" + data.week_day + "-" + data.instructorId;
            a1.innerHTML = button_text;
            //a1.id = "change-"+me.timeTableId+"-"+view+"-"+data.week_day+"-"+data.instructorId;
            //a1.innerHTML = "Change time";
            div.appendChild(a1);
            a1.onclick = function (e) {
                //url是否为空
                if (button_url == "") {
                    alert("No button_url!");
                } else {
                    new com.embraiz.tag().openNewTag(data.button_tag_id + ":" + data.button_target_id, data.button_tag_name, button_url, 'iconRole16', 'iconRole16', 'iconRole16', data.button_target_id);
                }
            };
        }


    },

    //获取div的top
    getDivTop: function (startTime) {
        var top = 0;
        if (startTime != null) {
            var starttime = startTime.split(":");
            var startH = starttime[0];
            var startT = starttime[1];
            top = ((parseFloat(startH) - 7) * 4 + parseFloat(startT) / 15) * 25;
            if (top != 0) {
                top = top - 2;
            };
        }
        return top;
    }


    //修改時間
    /*change_time:function(data){
		var me = this;
		var win = Ext.create('widget.window',{
			closable:true,
			title:'Change time',
			width:400,
			height:150,
			modal:true,
			layout:{type:'anchor'},
			defaults:{anchor:'100%'},
			html:'<div id="change_time_gird"></div>'
		});
		
		win.show();
		
		 //var changeTime_div = Ext.fly("change_time_gird").dom;	     
	     var form_div=document.createElement("div");//form
	     form_div.style.margin="5px";
	     Ext.fly("change_time_gird").dom.appendChild(form_div);
		
		//获取搜索框的信息
		var view = me.searchPanel.items.items[0].items.items[0].getValue();
	 	var date = me.searchPanel.items.items[0].items.items[1].getRawValue();
	 	var instructorId = me.searchPanel.items.items[0].items.items[2].getValue();
		
		
		var form = Ext.create('Ext.form.Panel',{
			url:'',//
			layout:'anchor',
			defaults:{anchor:'100%'},
			renderTo:form_div,
			defaultType:'textfield',
			items:[
			       {
			    	   fieldLabel:'Start Time',
			    	   name:'start_time',
			    	   value:data.start_time,//设置默认值为原开始时间
			    	   allowBlank:false,
			       },{
			    	   fieldLabel:'Time Length',
			    	   name:'time_length',
			    	   value:data.time_length,//设置默认值为原持续时间
			    	   allowBlank:false
			       }
			],
			buttons:[{
				text:'Submit',
				formBind:true,
				disabled: true,
		        handler: function() {
		           // var form = this.up('form').getForm();
		            if (form.form.isValid()) {
		                form.form.submit({
		                    success: function(form, action) {
		                    	Ext.Msg.alert('Failed', 'Change Time Success');
		                    	win.close();
		                		me.calendar_init(view,date,instructorId,id);//刷新表格
		                		
		                    },
		                    failure: function(form, action) {
		                        Ext.Msg.alert('Failed', 'Change Time Failed');
		                    }
		                });
		            }
					win.close();
					me.calendar_init(view,date,instructorId,me.timeTableId);//刷新表格
					alert(1);
		        }
			},{
		        text: 'Reset',
		        handler: function() {
		            form.form.reset();
		            win.close();
		        }
		    },],
			
		});
	 	
	 	
	}*/



});