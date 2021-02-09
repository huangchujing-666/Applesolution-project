Ext.define('com.embraiz.keyboard.js',{
    treeKeyboard:function(treePanel){
        new Ext.util.KeyMap(document,{
	    key:112,
	    fn:function(){
	        treePanel.getView().focus();
	        treePanel.getSelectionModel().select(0,true); 
            treePanel.getView().focusRow(0);
	    }
	});
	
	treePanel.getView().on('itemkeydown',function(view,record,item,index,e,options){ 
	    if(e.getKey()==13){
	       var treenodes=treePanel.getView().getSelectedNodes();
	       var treerecord=treePanel.getView().getRecord(treenodes[0]);
	       if(treerecord.get('leaf')){
	            new com.embraiz.tag().openNewTag(treerecord.getId(), 'List: '+treerecord.get('text'),treerecord.raw.url,treerecord.raw.iconCls,treerecord.raw.iconClsC,treerecord.raw.iconClsE);               
	       }
	       	treePanel.getView().focus(false,50);
	        treePanel.getSelectionModel().select(treerecord.getId()-1,true); 
            treePanel.getView().focusRow(treerecord.getId()-1);
		   }	    
	});
    },
    formKeyboard:function(form){
         Ext.each(form.items.get(0).items.get(0).items, function (o, i) {
                var config = [{
                    key: 38,
                    //上移
                    fn: function (key, e) {
                        if (i > 0 && i < form.items.get(0).items.get(0).items.getCount()) {
                            if (form.items.get(0).items.get(0).items.get(i - 1).getXType() == 'checkboxgroup') {
                                var t = Ext.query('input', Ext.getDom(form.items.get(0).items.get(0).items.get(i - 1).getId()));
                                Ext.each(t, function (o, i, t) {
                                    t[0].focus();
                                });
                                e.preventDefault();
                            }
                            if (form.items.get(0).items.get(0).items.get(i - 1).getXType() == 'container') {
                                var t = Ext.query('input', Ext.getDom(form.items.get(0).items.get(0).items.get(i - 1).getId()));
                                Ext.each(t, function (o, i, t) {
                                    t[0].focus();
                                });
                            } else {
                                form.items.get(0).items.get(0).items.get(i - 1).focus();
                                e.stopPropagation();
                                return false;
                            }
                        }
                    },
                    scope: form,
                    stopEvent: false
                }, {
                    key: 40,
                    //下移
                    fn: function (key, e) {
                        if (i < form.items.get(0).items.get(0).items.getCount() - 2) {
                            if (form.items.get(0).items.get(0).items.get(i + 1).getXType() == 'checkboxgroup') {
                                var t = Ext.DomQuery.select('input[type=button]', Ext.getDom(form.items.get(0).items.get(0).items.get(i + 1).getId()));
                                Ext.each(t, function (o, i) {
                                    t[0].focus();
                                });
                                e.preventDefault();
                                return false;
                            } else {
                                form.items.get(0).items.get(0).items.get(i + 1).focus();
                                e.stopPropagation();
                                return false;
                            }
                        } else if (i == form.items.get(0).items.get(0).items.getCount() - 2) {
                            if (form.items.get(0).items.get(0).items.get(i + 1).getXType() == 'hiddenfield') {
                                form.items.get(0).items.get(1).items.get(0).focus();
                                e.stopPropagation();
                                return false;
                            } else {
                                form.items.get(0).items.get(0).items.get(i + 1).focus();
                            }
                        } else if (i == form.items.get(0).items.get(0).items.getCount() - 1) {
                            if (form.items.get(0).items.get(1).items.get(0).getXType() == 'checkboxgroup') {
                                var t = Ext.query('input', Ext.getDom(form.items.get(0).items.get(1).items.get(0).getId()));
                                Ext.each(t, function (o, i, t) {
                                    t[0].focus();
                                });
                                e.preventDefault();
                                return false;
                            } else {
                                form.items.get(0).items.get(1).items.get(0).focus();
                                e.stopPropagation();
                                return false;
                            }
                        }
                    },
                    scope: form,
                    stopEvent: false
                }, {
                    key: 37,
                    fn: function (key, e) {
                        if (form.items.get(0).items.get(1).items.get(i).getXType() == 'container') {
                            var t = Ext.query('input', Ext.getDom(form.items.get(0).items.get(1).items.get(i).getId()));
                            Ext.each(t, function (o, i, t) {
                                t[0].focus();
                            });
                        }
                    }
                }, {
                    key: 39,
                    //右移
                    fn: function (key, e) {
                        if (form.items.get(0).items.get(1).items.getCount() >= i) {
                            if (form.items.get(0).items.get(1).items.get(i).readOnly != true) {
                                if (form.items.get(0).items.get(0).items.get(i).value != '' && form.items.get(0).items.get(0).items.get(i).value != null) {
                                    var len = getPosition(form.items.get(0).items.get(0).items.get(i));
                                    if (form.items.get(0).items.get(0).items.get(i).getRawValue().length == len) {
                                        if (form.items.get(0).items.get(1).items.get(i).getXType() == 'checkboxgroup') {
                                            var t = Ext.query('input', Ext.getDom(form.items.get(0).items.get(1).items.get(i).getId()));
                                            Ext.each(t, function (o, i, t) {
                                                t[0].focus();
                                            });
                                            e.preventDefault();
                                        } else {
                                            form.items.get(0).items.get(1).items.get(i).focus();
                                            e.stopPropagation();
                                            return false;
                                        }
                                    }
                                } else {
                                    if (form.items.get(0).items.get(1).items.get(i).getXType() == 'checkboxgroup') {
                                        var t = Ext.query('input', Ext.getDom(form.items.get(0).items.get(1).items.get(i).getId()));
                                        Ext.each(t, function (o, i, t) {
                                            t[0].focus();
                                        });
                                    }
                                    if (form.items.get(0).items.get(1).items.get(i).getXType() == 'container') {
                                        var t = Ext.query('input', Ext.getDom(form.items.get(0).items.get(1).items.get(i).getId()));
                                        Ext.each(t, function (o, j, t) {
                                            t[0].focus();
                                        });
                                    } else {
                                        form.items.get(0).items.get(1).items.get(i).focus();
                                        e.stopPropagation();
                                        return false;
                                    }
                                }
                            }
                        }
                    },
                    scope: form,
                    stopEvent: false
                }, {
                    key: 13,
                    fn: function (key, e) {
                        var types = form.items.get(0).items.get(0).items.get(i).getXTypes().split('/');
                        if (types[types.length - 1] == 'combobox' || types[types.length - 1] == 'datefield') {
                            if (i == form.items.get(0).items.get(0).items.getCount() - 2) {
                                if (form.items.get(0).items.get(0).items.get(i + 1).getXTypes() == 'hiddenfield') {
                                    form.items.get(0).items.get(1).items.get(0).focus();
                                } else {
                                    form.items.get(0).items.get(0).items.get(i + 1).focus();
                                }
                            } else if (i == form.items.get(0).items.get(0).items.getCount() - 1) {
                                form.items.get(0).items.get(1).items.get(0).focus();
                            } else {
                                form.items.get(0).items.get(0).items.get(i + 1).focus();
                            }
                        } else if (types[types.length - 1] == 'checkboxgroup') {
                            if (form.items.get(0).items.get(0).items.get(i).checked == false) {
                                form.items.get(0).items.get(0).items.get(i).checked == true;
                            } else {
                                form.items.get(0).items.get(0).items.get(i).checked == false;
                            }
                        }
                    },
                    scope: form,
                    stopEvent: false
                }];
                new Ext.util.KeyMap(form.items.get(0).items.get(0).items.get(i).id, config);
            });

            Ext.each(form.items.get(0).items.get(1).items, function (o, i) {
                var config = [{
                    key: 38,
                    //上移
                    fn: function (key, e) {
                        if (i == 0) {
                            if (form.items.get(0).items.get(0).items.get(form.items.get(0).items.get(0).items.getCount() - 1).getXType() == 'hiddenfield') {
                                if (form.items.get(0).items.get(0).items.get(form.items.get(0).items.get(0).items.getCount() - 2).getXType() == 'checkboxgroup') {
                                    var t = Ext.query('input', Ext.getDom(form.items.get(0).items.get(0).items.get(form.items.get(0).items.get(0).items.getCount() - 2).getId()));
                                    Ext.each(t, function (o, i, t) {
                                        t[0].focus();
                                    });
                                    e.preventDefault();
                                } else {
                                    form.items.get(0).items.get(0).items.get(form.items.get(0).items.get(0).items.getCount() - 2).focus();
                                }
                            } else {
                                if (form.items.get(0).items.get(0).items.get(form.items.get(0).items.get(0).items.getCount() - 1).getXType() == 'checkboxgroup') {
                                    var t = Ext.query('input', Ext.getDom(form.items.get(0).items.get(0).items.get(form.items.get(0).items.get(0).items.getCount() - 1).getId()));
                                    Ext.each(t, function (o, i, t) {
                                        t[0].focus();
                                    });
                                    e.preventDefault();
                                } else {
                                    form.items.get(0).items.get(0).items.get(form.items.get(0).items.get(0).items.getCount() - 1).focus();
                                }
                            }
                        }
                        if (i > 0 && i < form.items.get(0).items.get(1).items.getCount()) {
                            if (form.items.get(0).items.get(1).items.get(i - 1).getXType() == 'checkboxgroup') {
                                var t = Ext.query('input', Ext.getDom(form.items.get(0).items.get(1).items.get(i - 1).getId()));
                                Ext.each(t, function (o, i, t) {
                                    t[0].focus();
                                });
                                e.preventDefault();
                            } else if (form.items.get(0).items.get(1).items.get(i - 1).getXType() == 'container') {
                                var t = Ext.query('input', Ext.getDom(form.items.get(0).items.get(1).items.get(i - 1).getId()));
                                Ext.each(t, function (o, i, t) {
                                    t[0].focus();
                                });
                            } else {
                                form.items.get(0).items.get(1).items.get(i - 1).focus(true);
                                e.stopPropagation();
                                return false;
                            }
                        }
                    },
                    scope: form,
                    stopEvent: false
                }, {
                    key: 40,
                    //下移
                    fn: function (key, e) {
                        if (i < form.items.get(0).items.get(1).items.getCount() - 1) {
                            if (form.items.get(0).items.get(1).items.get(i + 1).getXType() == 'checkboxgroup') {
                                var t = Ext.DomQuery.select('input', Ext.getDom(form.items.get(0).items.get(1).items.get(i + 1).getId()));
                                Ext.each(t, function (o, i, t) {
                                    t[0].focus();
                                });
                                e.preventDefault();
                            }
                            if (form.items.get(0).items.get(1).items.get(i + 1).getXType() == 'container') {
                                var t = Ext.query('input', Ext.getDom(form.items.get(0).items.get(1).items.get(i + 1).getId()));
                                Ext.each(t, function (o, i, t) {
                                    t[0].focus();
                                });
                            } else {
                                form.items.get(0).items.get(1).items.get(i + 1).focus(true);
                            }
                            e.stopPropagation();
                            return false;
                        }
                    },
                    scope: form,
                    stopEvent: false
                }, {
                    key: 37,
                    //左移
                    fn: function (key, e) {
                        if (form.items.get(0).items.get(1).items.get(i).readOnly != true) {
                            if (form.items.get(0).items.get(1).items.get(i).value != '' && form.items.get(0).items.get(1).items.get(i).value != null) {
                                var len = getPosition(form.items.get(0).items.get(1).items.get(i));
                                if (len == 0) {
                                    if (form.items.get(0).items.get(0).items.get(i).getXType() == 'checkboxgroup') {
                                        var t = Ext.query('input', Ext.getDom(form.items.get(0).items.get(0).items.get(i).getId()));
                                        Ext.each(t, function (o, i, t) {
                                            t[0].focus();
                                        });
                                        e.preventDefault();
                                    } else {
                                        form.items.get(0).items.get(0).items.get(i).focus();
                                        e.stopPropagation();
                                        return false;
                                    }
                                }
                            }
                            if (form.items.get(0).items.get(1).items.get(i).getXType() == 'container') {
                                var t = Ext.query('input', Ext.getDom(form.items.get(0).items.get(1).items.get(i).getId()));
                                Ext.each(t, function (o, j, t) {
                                    if (document.activeElement.id == t[0].id) {
                                        form.items.get(0).items.get(0).items.get(i).focus();
                                    } else {
                                        t[0].focus();
                                    }
                                    e.stopPropagation();
                                    return false;
                                });
                            } else {
                                if (form.items.get(0).items.get(0).items.get(i).getXType() == 'checkboxgroup') {
                                    var t = Ext.query('input', Ext.getDom(form.items.get(0).items.get(0).items.get(i).getId()));
                                    Ext.each(t, function (o, i, t) {
                                        t[0].focus();
                                    });
                                    e.preventDefault();
                                } else {
                                    form.items.get(0).items.get(0).items.get(i).focus();
                                    e.stopPropagation();
                                    return false;
                                }
                            }
                        }
                    },
                    scope: form,
                    stopEvent: false
                }, {
                    key: 39,
                    fn: function (key, e) {
                        if (form.items.get(0).items.get(1).items.get(i).getXType() == 'container') {
                            var t = Ext.query('input', Ext.getDom(form.items.get(0).items.get(1).items.get(i).getId()));
                            Ext.each(t, function (o, i, t) {
                                t[1].focus();
                                new Ext.KeyMap(t[1].id, {
                                    key: 37,
                                    fn: function (key, e) {
                                        t[0].focus();
                                        e.preventDefault();
                                    }
                                });
                            });
                        }
                    }
                }, {
                    key: 13,
                    fn: function (key, e) {
                        var types = form.items.get(0).items.get(1).items.get(i).getXTypes().split('/');
                        if (types[types.length - 1] == 'combobox') {
                            var typess = form.items.get(0).items.get(1).items.get(i + 1).getXTypes().split('/');
                            if (typess[typess.length - 1] == 'checkboxgroup') {
                                var t = Ext.query('input', Ext.getDom(form.items.get(0).items.get(1).items.get(i + 1).getId()));
                                Ext.each(t, function (o, i, t) {
                                    t[0].focus();
                                });
                                e.preventDefault();
                            } else if (typess[typess.length - 1] == 'container') {
                                var t = Ext.query('input', Ext.getDom(form.items.get(0).items.get(1).items.get(i + 1).getId()));
                                Ext.each(t, function (o, i, t) {
                                    t[0].focus();
                                });
                                e.preventDefault();
                            } else {
                                form.items.get(0).items.get(1).items.get(i + 1).focus();
                            }
                        } else if (types[types.length - 1] == 'checkboxgroup') {
                            if (form.items.get(0).items.get(1).items.get(i).checked == false) {
                                form.items.get(0).items.get(1).items.get(i).checked == true;
                            } else {
                                form.items.get(0).items.get(1).items.get(i).checked == false;
                            }
                        } else if (types[types.length - 1] == 'datefield') {
                            var typess = form.items.get(0).items.get(1).items.get(i + 1).getXTypes().split('/');
                            if (typess[typess.length - 1] == 'checkboxgroup') {
                                var t = Ext.query('input', Ext.getDom(form.items.get(0).items.get(1).items.get(i + 1).getId()));
                                Ext.each(t, function (o, i, t) {
                                    t[0].focus();
                                });
                                e.preventDefault();
                            } else {
                                form.items.get(0).items.get(1).items.get(i + 1).focus();
                            }
                        } else {
                            var typess = form.items.get(0).items.get(1).items.get(i + 1).getXTypes().split('/');
                            if (typess[typess.length - 1] == 'checkboxgroup') {
                                var t = Ext.query('input', Ext.getDom(form.items.get(0).items.get(1).items.get(i + 1).getId()));
                                Ext.each(t, function (o, i, t) {
                                    t[0].focus();
                                });
                                e.preventDefault();
                            } else if (typess[typess.length - 1] == 'container') {
                                var t = Ext.query('input', Ext.getDom(form.items.get(0).items.get(1).items.get(i + 1).getId()));
                                Ext.each(t, function (o, i, t) {
                                    t[0].focus();
                                });
                                e.preventDefault();
                            } else {
                                form.items.get(0).items.get(1).items.get(i + 1).focus();
                            }
                        }
                    },
                    scope: form,
                    stopEvent: false
                }];
                new Ext.util.KeyMap(form.items.get(0).items.get(1).items.get(i).id, config);
            });
         function getPosition(obj) {
            var result = 0;
            if (document.all) { //IE  
                var rng;
                if (obj.tagName == "TEXTAREA") { //如果是文本域  
                    rng = event.srcElement.createTextRange();
                    rng.moveToPoint(event.x, event.y);
                } else { //输入框
                    rng = document.selection.createRange();
                }
                rng.moveStart("character", -event.srcElement.value.length);
                result = rng.text.length;
            } else { //非IE浏览器 
                var t = Ext.query('input', Ext.getDom(obj.getId()));
                Ext.each(t, function (o, i, t) {
                    result = t[0].selectionStart;
                });
            }
            return result;
        }
    },
    focusToForm:function(contentPanel,tab){
	new Ext.util.KeyMap(document,{
	    key:113,//f
	    fn:function(){
	       var activeTab=contentPanel.getActiveTab();
	       activeTab.focus();
	       var curTabElement = null;   
           var curExtCtl = null;           
           for (var i = 0; i < contentPanel.items.getCount(); i++) {    
                curTabElement = contentPanel.items.get(i);   
                if (curTabElement.getId() == activeTab.getId()) {   
                    curExtCtl = contentPanel.items.get(i);                    
                    break;   
                }   
           }          
           if (curExtCtl == null) {                     
               return;   
           }                
           var divs=curExtCtl.getEl().dom.lastChild;
           if(divs!=null && divs!=undefined){
           var curExtForm= Ext.DomQuery.select('input[type=text][class!="x-tbar-page-number"],input[type!=hidden][class!="x-tbar-page-number"]',divs);
               Ext.each(curExtForm,function(o,i,curExtForm){
               if(i==0){
                   Ext.get(o).focus();
               }
               });
           if(tool_div!=undefined && tool_div!=null) {
               var div=curExtCtl.getEl().dom.firstChild;
               var curExtForm= Ext.DomQuery.select('input[type=text][class!="x-tbar-page-number"],input[type!=hidden][class!="x-tbar-page-number"]',div);
               Ext.each(curExtForm,function(o,i,curExtForm){
               if(i==0){
                   Ext.get(o).focus();
               }
               }); 
           }
           }
	    }
	});
    },
    gridKeyboard:function(grid){
       new Ext.util.KeyMap(document,{
	    key:119,
	    fn:function(){
	        grid.getView().focus();
	        grid.getSelectionModel().select(0,true); 
            grid.getView().focusRow(0);
	    }
	});
		grid.getView().on('itemkeydown',function(view,record,item,index,e,options){ 
	    if(e.getKey()==13){
	       var gridnodes=grid.getView().getSelectedNodes();
	       var gridrecord=grid.getView().getRecord(gridnodes[0]);
	       var str1= gridrecord.get('href');
            Ext.decode(str1);
	       	//grid.getView().focus(false,50);
	        //grid.getSelectionModel().select(gridrecord.getId()-1,true); 
            //grid.getView().focusRow(gridrecord.getId()-1);
		   }	    
	});
    },
    closeTAb:function(){
      	new Ext.util.KeyMap(document,{
	    key:27,
	    handler:function(){ 
	       if(Ext.getCmp("content-panel").getActiveTab()!=null && Ext.getCmp("content-panel").getActiveTab().id!='docs_ctagpanel'){
	          Ext.getCmp("content-panel").getActiveTab().close();
	       }
	    }
	});
    }
});