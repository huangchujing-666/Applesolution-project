Ext.onReady(function(){
var task = {
		  run: function(){
		      Ext.Ajax.request({
		      url: 'refresh.jsp'});
          },
		  interval: 120000 
		}
var runner = new Ext.util.TaskRunner();
runner.start(task);
});