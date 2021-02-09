Ext.define('com.embraiz.component.girdTable', {
    grid_headerUrl: undefined,
    grid_dataUrl: undefined,
    el: undefined,
    ids: undefined,
    header: undefined,
    dataIndex: undefined,
    headerType: undefined,
    footerIndex: undefined,
    orderIndex: undefined,
    sortable: undefined,
    orderIdIndex: undefined,
    orderFile: undefined,
    type_checked: undefined,
    filterParams: undefined,
    text_align:undefined,
    sort: '',
    sortOrder: 'desc',
    start: 0,
    limit: 10,
    totalCount: 0,
    title: '',
    tbodyEl: undefined,
    tableEl: undefined,
    pageDiv: undefined,
    delete_url: undefined,
    edit_hidden: undefined,
    commemt_hidden: undefined,
    delete_hidden: undefined,
    download_hidden:undefined,
    colJson:undefined,

    initGrid: function(grid_headerUrl, grid_dataUrl, el) {
        var me = this;
        this.grid_headerUrl = grid_headerUrl;
        this.grid_dataUrl = grid_dataUrl;
        this.el = el;
        Ext.Ajax.request({        	
            url: grid_headerUrl,
            success: this.headerResponse,
            scope: this
        });

    },
    Content_render: function(tbodyEl) {
    	//
    	var e2 = document.createElement("tr"); 
        tbodyEl.appendChild(e2);
        var index_ids = 0;
        var ids = [];
        var orderIdIndex = [];
        var orderFile = [];        
        var colJson=this.colJson;
        var order=this.dataIndex.concat();
        for(var j=0;j<order.length;j++){
        	var name=order[j];
	        for(var i=0;i<name.length;i++){
	    		if((name.charCodeAt(i)>=65)&&(name.charCodeAt(i)<=90)){
	    			name=name.substring(0,i)+'_'+name.substring(i,i+1).toLowerCase()+name.substring(i+1,name.length);
	    		}           		
	    	}
	        order[j]=name;
        }
        var orderIndex=order;
        for (var i = 0; i < this.header.length; i++) {
            var e3 = document.createElement("th");
            if (colJson[i].width != null) {
                e3.width = colJson[i].width;
            }

            if (this.sortable[i] == true) {
                var orderA = document.createElement("a");
                Ext.id(orderA, 'Embraiz');
                orderA.innerHTML = this.header[i];
                e3.appendChild(orderA);
                var imgEl = document.createElement("img");
                Ext.id(imgEl, 'Embraiz');
                imgEl.className = '';
                imgEl.src = 'images/s.gif';
                orderA.appendChild(imgEl);
                orderIdIndex[index_ids] = imgEl.id;
                ids[index_ids] = orderA.id;
                orderFile[index_ids] = orderIndex[i];
                index_ids++;
            } else {
                e3.innerHTML = this.header[i];
            }
            e2.appendChild(e3);
        }
        this.ids = ids;
        this.orderIdIndex = orderIdIndex;
        this.orderFile = orderFile;
        for (var i = 0; i < this.ids.length; i++) {
            Ext.get(this.ids[i]).addListener('click', this.orderSort, this);
        }

        if (this.edit_hidden == false || this.commemt_hidden == false || this.delete_hidden != true) {
            var deleTh = document.createElement("th");
            deleTh.width = "65";
            deleTh.innerHTML = '&nbsp;'; ////////Delete
            e2.appendChild(deleTh);
        }
    	//
        Ext.Ajax.request({        	
            url: this.grid_dataUrl,
            params: {
                limit: this.limit,
                start: this.start,
                sort: this.sort,
                sortOrder: this.sortOrder,
                filter: this.filterParams
            },
            success: function(o){
                var data_json = Ext.decode(o.responseText);  
                this.totalCount = data_json.totalCount;
                var col = data_json.items;
                for (var j = 0; j < col.length; j++) {
                    var e22 = document.createElement("tr");
                    if (j % 2 == 1) {
                        e22.className = "alt";
                    }
                    Ext.id(e22, 'Embraiz');                   
                    tbodyEl.appendChild(e22);   
                    for (var k = 0; k < this.header.length; k++) {
                    	if(this.headerType[k] == 'currency'){
                    		col[j][this.dataIndex[k]]=Ext.util.Format.currency(col[j][this.dataIndex[k]], '$', 2);
                    	}
                        var e33 = document.createElement("td");                        
                           e33.align=this.text_align[k]; 
                        if (this.headerType[k] == 'title') {
                            e33.innerHTML = Ext.String.format('<a href="{1}" target="_self" >{0}</a>', col[j][this.dataIndex[k]],(col[j].href.indexOf('javascript:')>=0?col[j].href:'javascript:'+col[j].href) );
                        } else if (this.headerType[k] == 'title1') {
                            e33.innerHTML = Ext.String.format('<a href="{1}" target="_self" >{0}</a>', col[j][this.dataIndex[k]], (col[j].href.indexOf('javascript:')>=0?col[j].href1:'javascript:'+col[j].href1));
                        } else if (this.headerType[k] == 'title2') {
                            e33.innerHTML = Ext.String.format('<a href="{1}" target="_self" >{0}</a>', col[j][this.dataIndex[k]], (col[j].href.indexOf('javascript:')>=0?col[j].href2:'javascript:'+col[j].href2));
                        } else {
                            if (this.type_checked[k] == true) {
                                if (col[j][this.dataIndex[k]] == '1') {
                                    e33.innerHTML = Ext.String.format('<input type="checkbox" value="1" disabled=true checked/>', (col[j].href.indexOf('javascript:')>=0?col[j].href:'javascript:'+col[j].href));
                                } else if (col[j][this.dataIndex[k]] == 'have') {
                                    e33.innerHTML = Ext.String.format('<input type="checkbox" value="1"/>', col[j].href);
                                } else if (col[j][this.dataIndex[k]] == 'no') {
                                    e33.innerHTML = '&nbsp;';
                                } else {
                                    e33.innerHTML = Ext.String.format('<input type="checkbox" value="1" disabled=true/>', (col[j].href.indexOf('javascript:')>=0?col[j].href:'javascript:'+col[j].href));
                                }
                            } else {
                                e33.innerHTML = col[j][this.dataIndex[k]]+'&nbsp;';

                            }

                        }
                        e22.appendChild(e33);
                    }
                    var idd = col[j].id;
                    if (this.edit_hidden == false || this.commemt_hidden == false || this.delete_hidden != true) {
                        var dele = document.createElement("td");
                        e22.appendChild(dele);
                    }
                    if (col[j].id != '') { //start
                        if (this.edit_hidden == false) {
                            if (col[j].edit_dispaly != false) {
                                dele.innerHTML = Ext.String.format('<a href="{0}" target="_self" ><img src="icons/edit16.gif"/></a>', (col[j].href.indexOf('javascript:')>=0?col[j].href:'javascript:'+col[j].href));
                            }
                        }
                        if (this.commemt_hidden == false) {
                            if (col[j].comment_dispaly!=false) {
                            	dele.innerHTML = dele.innerHTML + Ext.String.format('<a href="{0}" target="_self" >'+col[j].commemt_text+'</a>', (col[j].commemt_url.indexOf('javascript:')>=0?col[j].commemt_url:'javascript:'+col[j].commemt_url));
                               // dele.innerHTML = dele.innerHTML + Ext.String.format('<a href="{0}" target="_self" ><img src="icons/comment.png"/></a>', col[j].commemt_url);
                            }
                        }
                        if (this.download_hidden == false) {                            
                                dele.innerHTML = dele.innerHTML + Ext.String.format('<a href="{0}" target="_self" ><img src="icons/down16.gif"/></a>', col[j].download_url);                          
                        }
                        if (this.delete_hidden != true) {
                        	if (col[j].delete_dispaly != false) {
	                            var delEl = document.createElement("a");
	                            Ext.id(delEl, idd + ',Embraiz');
	                            delEl.innerHTML = '<img src="icons/cross.png"/>';
	                            dele.appendChild(delEl);
	
	                         //   Ext.get(delEl.id).addListener('click',
	                            Ext.get(delEl).addListener('click',
	                            function(a, b) {
	                                var pid = b.parentNode.id.split(',');
	                                if(this.delete_url.indexOf('?')!=-1){
	                                	this.delete_url+='&';
	                                }else{
	                                	this.delete_url+='?';
	                                }
	                                this.gridDelFun(this.delete_url + "id=" + pid[0], b);
	                            },
	                            this);
	                        }
                        }
                    } //end
                }
                this.changeFooter();
                var divarr = Ext.query("*[class=gridWrapper]");
                Ext.each(divarr,
                function(o, i) {
                    var tab = Ext.query("table", divarr[i]);
                    new Ext.util.KeyMap(Ext.getCmp('content-panel').getActiveTab().id, {
                        key: 71,
                        alt: true,
                        fn: function() {
                            for (var kk = 0; kk < tab[0].rows.length; kk++) {
                                if (tab[0].rows[kk].bgColor == '#8db2e3') {
                                    tab[0].rows[kk].bgColor == '#FFFFFF';
                                    if (col != null) {
                                        if (col[kk - 1] != null) {
                                            var str;
                                            var array;
                                            var str1 = col[kk - 1].href;
                                            if (str1.indexOf('openNewTag') != -1) {
                                                str = str1.substring(str1.indexOf('openNewTag') + 10, str1.length);
                                                str = str.substring(str.indexOf("(") + 1, str.lastIndexOf(")"));
                                                array = str.split(',');

                                                new com.embraiz.tag().openNewTag(array[0].replace("'", '').replace("'", ''), array[1].replace("'", '').replace("'", ''), array[2].replace("'", '').replace("'", ''), array[3].replace("'", '').replace("'", ''), array[4].replace("'", '').replace("'", ''), array[5].replace("'", '').replace("'", ''), array[6].replace("'", '').replace("'", ''));

                                            }
                                        }
                                    }
                                }
                            }
                        }
                    });
                });

            
            },
            scope: this
        });////end
    },
    headerResponse: function(o) {
        var me = this;
        var header_json = Ext.decode(o.responseText);
        var colJson = this.colJson=header_json.columns;       
        var header = [];
        var dataIndex = [];
        var headerType = [];
        var orderIndex = [];
        var sortable = [];
        var type_checked = [];
        var text_align=[];
        var currency=[];
        for (var i = 0; i < colJson.length; i++) {
            header[i] = colJson[i].header;
            dataIndex[i] = colJson[i].dataIndex;
            headerType[i] = colJson[i].type;
            orderIndex[i] = colJson[i].orderIndex;
            if(colJson[i].text_align==null || colJson[i].text_align==undefined){
            	text_align[i]='left';
            }else{
                text_align[i]=colJson[i].text_align;
            }
            if (colJson[i].checked == undefined) {
                type_checked[i] = false;
            } else {
                type_checked[i] = colJson[i].checked;
            }

            if (colJson[i].sortable == undefined) {
                sortable[i] = false;
            } else {
                sortable[i] = colJson[i].sortable;
            }
        }
        this.header = header;
        this.dataIndex = dataIndex;
        this.text_align=text_align;
        this.headerType = headerType;
        this.orderIndex = orderIndex;
        this.sortable = sortable;
        this.type_checked = type_checked;
        this.sort = header_json.sort;
        this.title = header_json.title;
        this.delete_url = header_json.delete_url;
        this.edit_hidden = header_json.edit_hidden;
        this.commemt_hidden = header_json.commemt_hidden;
        this.download_hidden=header_json.download_hidden;
        this.delete_hidden = header_json.delete_hidden;
        //////////////////
        var table_div = this.el;
        // var table_div =document.getElementById(this.el.id);	
        table_div.className = 'gridWrapper';
        table_div.style.margin = "5px";

        ///title				     
        var e_title = document.createElement("div");
        Ext.id(e_title, 'Embraiz');
        e_title.className = "titleDiv";
        ///////
        if (header_json.iconSrc == null) {
            header_json.iconSrc = "icons/connect.png";
        }
        ///////                	
        e_title.innerHTML = Ext.String.format('<img src="{0}" align="absmiddle" width="16px" heigth="16px"/><span width="16px" heigth="16px">{1}</span>', header_json.iconSrc, this.title); //?????????    
        table_div.appendChild(e_title);
        var imgRefresh = document.createElement("img");
        Ext.id(imgRefresh, 'Embraiz');
        imgRefresh.className = 'imgRefresh';
        imgRefresh.src = 'icons/arrow_refresh.png';
        var attr_align = document.createAttribute('align');
        attr_align.value = 'absmiddle';
        imgRefresh.setAttributeNode(attr_align);

        e_title.appendChild(imgRefresh);

        if (header_json.export_href != '' && header_json.export_href != null && header_json.export_href != undefined) {
            e_title.innerHTML = e_title.innerHTML + Ext.String.format('<a href="{0}" ><img src="icons/excel.gif" align="absmiddle" width="16px" alt="123"  heigth="16px"/></a>', header_json.export_href);            
        }
        if(header_json.button!='' && header_json.button!=null && header_json.button!=undefined){
        	for(var index=0;index<header_json.button.length;index++){
        	if(header_json.button[index].type=='image'){ 
        	   e_title.innerHTML = e_title.innerHTML + Ext.String.format('<a href="{0}" ><img src="{1}" align="absmiddle" width="16px" alt="123"  heigth="16px"/></a>', header_json.button[index].export_href,header_json.button[index].imgSrc);
        	}else{
        		e_title.innerHTML = e_title.innerHTML + Ext.String.format('<a href="{0}" style="TEXT-DECORATION:none; font-weight: bold;color:#15428b;">{1}</a>',header_json.button[index].export_href,header_json.button[index].text);
        	}
        	} 
        }
        Ext.get(imgRefresh.id).on('click',function() {
            me.refresh();
        }); /////////////////////////////////////////////////////			      	 
        var tableEl = this.tableEl = document.createElement("table");
        table_div.appendChild(tableEl);
        tableEl.className = 'gridTable';
        var attr = document.createAttribute('cellspacing');
        attr.value = '0';
        tableEl.setAttributeNode(attr);
        ///
        var tbodyEl = document.createElement("TBODY");
        Ext.id(tbodyEl, 'Embraiz');
        this.tbodyEl = tbodyEl.id;
        tableEl.appendChild(tbodyEl); 

        var pageDiv = this.pageDiv = document.createElement("div");
        Ext.id(pageDiv, 'Embraiz');
        pageDiv.className = "footerDiv";
        table_div.appendChild(pageDiv);
        this.initPage();
        this.Content_render(tbodyEl);
       
    },



    initPage: function() {
        var footerIndex = [];
        var firstEl = document.createElement("a");
        Ext.id(firstEl, 'Embraiz');
        footerIndex[0] = firstEl.id;
        var firstImage = document.createElement("img");
        firstImage.src = 'images/page-first.gif';
        firstImage.width = 16;
        firstImage.height = 16;
        firstEl.appendChild(firstImage);
        firstEl.style.display = 'none';
        this.pageDiv.appendChild(firstEl);

        var preEl = document.createElement("a");
        Ext.id(preEl, 'Embraiz');
        footerIndex[1] = preEl.id;
        var preImage = document.createElement("img");
        preImage.src = 'images/page-prev.gif';
        preImage.width = 16;
        preImage.height = 16;
        preEl.appendChild(preImage);
        preEl.style.float = 'left';
        preEl.style.display = 'none';
        this.pageDiv.appendChild(preEl);

        var pageNumEl = document.createElement("div");
        Ext.id(pageNumEl, 'Embraiz');
        pageNumEl.className = "page-num";
        pageNumEl.innerHTML = '1';
        footerIndex[2] = pageNumEl.id;
        pageNumEl.style.display = 'none';
        this.pageDiv.appendChild(pageNumEl);

        var nextEl = document.createElement("a");
        Ext.id(nextEl, 'Embraiz');
        footerIndex[3] = nextEl.id;
        var nextImage = document.createElement("img");
        nextImage.src = 'images/page-next.gif';
        nextImage.width = 16;
        nextImage.height = 16;
        nextEl.appendChild(nextImage);
        nextEl.style.display = 'none';
        this.pageDiv.appendChild(nextEl);

        var lastEl = document.createElement("a");
        Ext.id(lastEl, 'Embraiz');
        footerIndex[4] = lastEl.id;
        var lastImage = document.createElement("img");
        lastImage.src = 'images/page-last.gif';
        lastImage.width = 16;
        lastImage.height = 16;
        lastEl.appendChild(lastImage);
        lastEl.style.display = 'none';
        this.pageDiv.appendChild(lastEl);
        this.footerIndex = footerIndex;
        Ext.get(firstEl.id).addListener('click', this.goFirst, this);

        Ext.get(preEl.id).addListener('click', this.goPre, this);

        Ext.get(nextEl.id).addListener('click', this.goNext, this);

        Ext.get(lastEl.id).addListener('click', this.goLast, this);

        //this.changeFooter();   	
    },

    goFirst: function() {
        this.start = 0;
        this.render();
    },
    goPre: function() {
        this.start = this.start - this.limit;
        this.render();
    },
    goNext: function() {
        this.start = this.start + this.limit;
        this.render();
    },
    goLast: function() {
        this.start = Math.floor(this.totalCount / this.limit) * this.limit;
        if (this.start == this.totalCount) {
            this.start = this.start - this.limit;
        }
        this.render();
    },
    render: function() {
        var pNode = Ext.get(this.tbodyEl).dom.parentNode;
        pNode.removeChild(Ext.get(this.tbodyEl).dom);
        var tbodyEl = document.createElement("TBODY");
        Ext.id(tbodyEl, 'Embraiz');
        this.tableEl.appendChild(tbodyEl);
        this.tbodyEl = tbodyEl.id;
        this.Content_render(tbodyEl);
    },
    gridDelFun: function(delLink, el) {
        Ext.MessageBox.confirm('Confirm', 'Are you sure you want to delete?',
        function(btn) {
            if (btn == 'yes') {
                Ext.Ajax.request({
                    url: delLink,
                    success: function(o) {
                        var resText = Ext.decode(o.responseText);
                        if (resText.size != undefined) {
                            if (resText.size > 0) {
                                Ext.Msg.alert('Status', 'This data cannot be deleted.');
                            } else {
                                Ext.Msg.alert('Status', 'Delete successfully.');
                                var trEl = el.parentNode.parentNode.parentNode;
                                var pNode = trEl.parentNode;
                                pNode.removeChild(trEl);
                            }
                        } else {
                            var trEl = el.parentNode.parentNode.parentNode;
                            var pNode = trEl.parentNode;
                            pNode.removeChild(trEl);
                        }
                    }
                });
            } else {
                this.close();
            }
        });
    },
    changeFooter: function() {
        if (Ext.get(this.footerIndex[0]) && Ext.get(this.footerIndex[0])) {
            var curpage = Math.floor(this.start / this.limit) + 1;
            var totalPage = Math.floor(this.totalCount / this.limit);
            if (totalPage * this.limit < this.totalCount) {
                totalPage = totalPage + 1;
            }

            if (curpage > 1) {
                Ext.get(this.footerIndex[0]).dom.style.display = '';

                Ext.get(this.footerIndex[1]).dom.style.display = '';
            } else {
                Ext.get(this.footerIndex[0]).dom.style.display = 'none';

                Ext.get(this.footerIndex[1]).dom.style.display = 'none';
            }

            Ext.get(this.footerIndex[2]).dom.innerHTML = curpage + '/' + totalPage;
            if (this.totalCount < 1) {
                Ext.get(this.footerIndex[2]).dom.style.display = 'none';
            } else {
                Ext.get(this.footerIndex[2]).dom.style.display = '';
            }
            if (curpage < totalPage) {
                Ext.get(this.footerIndex[3]).dom.style.display = '';

                Ext.get(this.footerIndex[4]).dom.style.display = '';
            } else {
                Ext.get(this.footerIndex[3]).dom.style.display = 'none';

                Ext.get(this.footerIndex[4]).dom.style.display = 'none';
            }
        }
    },
    refresh: function() {
        this.render();
    },
    orderSort: function(e) {
        e.preventDefault();
        var target = e.getTarget();
        
        for (var i = 0; i < this.ids.length; i++) {
            if (this.ids[i] == target.id) {
                if (this.sort == this.orderFile[i]) {
                    if (this.sortOrder == 'asc') {
                        document.getElementById(this.orderIdIndex[i]).className = 'x-grid3-desc-icon';
                        this.sortOrder = 'desc';
                    } else {
                        document.getElementById(this.orderIdIndex[i]).className = 'x-grid3-asc-icon';
                        this.sortOrder = 'asc';
                    }
                } else {
                    this.sort = this.orderFile[i];
                    document.getElementById(this.orderIdIndex[i]).className = 'x-grid3-desc-icon';
                    this.sortOrder = 'desc';
                }

            } else {
                document.getElementById(this.orderIdIndex[i]).className = '';
            }
        }
        this.render();
    },
    search: function(post_data) {
        this.filterParams = post_data.params;
        this.refresh();
    }

})