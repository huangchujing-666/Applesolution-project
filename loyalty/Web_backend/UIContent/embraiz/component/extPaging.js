

Ext.define('com.embraiz.component.extPaging', {
	extend: 'Ext.toolbar.Paging',
	url: undefined,
	params : undefined,


// private
onPagingKeyDown : function(field, e){

    var k = e.getKey(),
        pageData = this.getPageData(),
        increment = e.shiftKey ? 10 : 1,
        pageNum,
        me = this;

    if (k == e.RETURN) {
        e.stopEvent();
        pageNum = me.readPageFromInput(pageData);
        if (pageNum !== false) {
            pageNum = Math.min(Math.max(1, pageNum), pageData.total);
            if(me.fireEvent('beforechange', me, pageNum) !== false){
//                me.store.loadPage(pageNum);
                me.store.currentPage=pageNum;
                me.store.load({
                    url: this.url,
                    params: this.params
                    
                });
            }
        }
    } else if (k == e.HOME || k == e.END) {
        e.stopEvent();
        pageNum = k == e.HOME ? 1 : pageData.pageCount;
        field.setValue(pageNum);
    } else if (k == e.UP || k == e.PAGEUP || k == e.DOWN || k == e.PAGEDOWN) {
        e.stopEvent();
        pageNum = me.readPageFromInput(pageData);
        if (pageNum) {
            if (k == e.DOWN || k == e.PAGEDOWN) {
                increment *= -1;
            }
            pageNum += increment;
            if (pageNum >= 1 && pageNum <= pageData.pages) {
                field.setValue(pageNum);
            }
        }
    }
},



/**
 * Move to the first page, has the same effect as clicking the 'first' button.
 */
moveFirst : function(){

    var me = this;
    if(me.fireEvent('beforechange', me, 1) !== false){
        me.store.currentPage=1;
        me.store.load({
            url: this.url,
            params: this.params
            
        });
    }
},

/**
 * Move to the previous page, has the same effect as clicking the 'previous' button.
 */
movePrevious : function(){

    var me = this,
        prev = me.store.currentPage - 1;
    
    if(me.fireEvent('beforechange', me, prev) !== false){
    	 me.store.currentPage=prev;
         me.store.load({
             url: this.url,
             params: this.params
             
         });
    }
},

/**
 * Move to the next page, has the same effect as clicking the 'next' button.
 */
moveNext : function(){
    var me = this;        
    if(me.fireEvent('beforechange', me, me.store.currentPage + 1) !== false){
    	me.store.currentPage=me.store.currentPage + 1;
        me.store.load({
            url: this.url,
            params: this.params
            
        });
    }
},

/**
 * Move to the last page, has the same effect as clicking the 'last' button.
 */
moveLast : function(){
    var me = this, 
        last = this.getPageData().pageCount;
    
    if(me.fireEvent('beforechange', me, last) !== false){
        me.store.currentPage=last;
        me.store.load({
            url: this.url,
            params: this.params
            
        });
    }
},

/**
 * Refresh the current page, has the same effect as clicking the 'refresh' button.
 */
    doRefresh : function(){
        var me = this,
        current = me.store.currentPage;
  
        // alert(this.url);
        // alert(
        if(me.fireEvent('beforechange', me, current) !== false){
            me.store.load({
                    url: this.url,
                    params: this.params
            
            });
        }
    }
});

