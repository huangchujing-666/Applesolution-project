Ext.ns('Ext.Portal.textPortlet');
Ext.Portal.textPortlet.data = '<p>Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Sed metus nibh, sodales a, ' +
    'porta at, vulputate eget, dui. Pellentesque ut nisl. Maecenas tortor turpis, interdum non, sodales non, iaculis ac, ' +
    'lacus. Vestibulum auctor, tortor quis iaculis malesuada, libero lectus bibendum purus, sit amet tincidunt quam turpis ' +
    'vel lacus. In pellentesque nisl non sem. Suspendisse nunc sem, pretium eget, cursus a, fringilla vel, urna.<br/><br/></p>';

Ext.define('com.palmary.portlet.textPortlet', {
    extend: 'com.palmary.app.portalTextPanel',
    alias: 'widget.textPortlet',
    initComponent: function () {
        var me = this;
        me.setConfig_data(Ext.Portal.textPortlet.data);
        this.callParent(arguments);
    }
});