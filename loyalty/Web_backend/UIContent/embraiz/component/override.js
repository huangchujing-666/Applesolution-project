

Ext.override(Ext.grid.Panel, {
      updateLayout: function(options) {
			var me = this,
		    defer,
		    lastBox = me.lastBox,
		    isRoot = options && options.isRoot;
			
			if (lastBox) {
			    // remember that this component's last layout result is invalid and must be
			    // recalculated
			    lastBox.invalid = true;
			}
			
			if (!me.rendered || me.layoutSuspendCount || me.suspendLayout) {
			    return;
			}
			
			if (me.hidden) {
			    Ext.AbstractComponent.cancelLayout(me);
			} else if (typeof isRoot != 'boolean') {
			    isRoot = me.isLayoutRoot();
			}
			
			// if we aren't the root, see if our ownerLayout will handle it...
			if (isRoot || !me.ownerLayout || !me.ownerLayout.onContentChange(me)) {
			    // either we are the root or our ownerLayout doesn't care
			    if (!me.isLayoutSuspended()) {
			        // we aren't suspended (knew that), but neither is any of our ownerCt's...
			        defer = (options && options.hasOwnProperty('defer')) ? options.defer : me.deferLayouts;
			        Ext.AbstractComponent.updateLayout(me, defer);
			        me.fireEvent('afterlayout',me);			        
			    }			
			
        	}
    }
    
});
