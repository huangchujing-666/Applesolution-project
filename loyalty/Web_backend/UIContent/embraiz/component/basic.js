/*!
 * Extensible 1.5.1
 * Copyright(c) 2010-2011 Extensible, LLC
 * licensing@ext.ensible.com
 * http://ext.ensible.com
 */
Ext.Loader.setConfig({
    enabled: true,
    disableCaching: false,
    paths: {
        "Extensible": "../../src",
        "Extensible.example": ".."
    }
});
Ext.require([
    'Extensible.calendar.data.MemoryEventStore',
    'Extensible.calendar.CalendarPanel',
    'Extensible.example.calendar.data.Events'
]);

Extensible.calendar.data.EventMappings.Who = {
    name: 'Who',
    mapping: 'who',
    type: 'string'
};
Extensible.calendar.data.EventModel.reconfigure();

//for the pop window
Extensible.calendar.form.EventWindow.override({
    whoLabelText: 'Who',
    getFormItemConfigs: function() {
        var items = [{
            xtype: 'textfield',
            itemId: this.id + '-title',
            name: Extensible.calendar.data.EventMappings.Title.name,
            fieldLabel: this.titleLabelText,
            anchor: '100%'
        },{
            xtype: 'textfield',
            itemId: this.id + '-who',
            name: Extensible.calendar.data.EventMappings.Who.name,
            fieldLabel: this.whoLabelText,
            anchor: '100%'
        },{
            xtype: 'extensible.daterangefield',
            itemId: this.id + '-dates',
            name: 'dates',
            anchor: '95%',
            singleLine: true,
            fieldLabel: this.datesLabelText
        }];

        if(this.calendarStore){
            items.push({
                xtype: 'extensible.calendarcombo',
                itemId: this.id + '-calendar',
                name: Extensible.calendar.data.EventMappings.CalendarId.name,
                anchor: '100%',
                fieldLabel: this.calendarLabelText,
                store: this.calendarStore
            });
        }

        return items;
    }
});

//for the detail window
Extensible.calendar.form.EventDetails.override({
    whoLabelText: 'Who',
    initComponent: function(){

        this.addEvents({

       // * @event eventadd
            // * Fires after a new event is added
            // * @param {Extensible.calendar.form.EventDetails} this
            // * @param {Extensible.calendar.data.EventModel} rec The new {@link Extensible.calendar.data.EventModel record} that was added
            eventadd: true,

       // * @event eventupdate
            // * Fires after an existing event is updated
            // * @param {Extensible.calendar.form.EventDetails} this
            // * @param {Extensible.calendar.data.EventModel} rec The new {@link Extensible.calendar.data.EventModel record} that was updated
            eventupdate: true,

            // * @event eventdelete
            // * Fires after an event is deleted
            // * @param {Extensible.calendar.form.EventDetails} this
            // * @param {Extensible.calendar.data.EventModel} rec The new {@link Extensible.calendar.data.EventModel record} that was deleted
            eventdelete: true,

            // * @event eventcancel
            // * Fires after an event add/edit operation is canceled by the user and no store update took place
            // * @param {Extensible.calendar.form.EventDetails} this
            // * @param {Extensible.calendar.data.EventModel} rec The new {@link Extensible.calendar.data.EventModel record} that was canceled
         eventcancel: true
        });

        this.titleField = Ext.create('Ext.form.TextField', {
            fieldLabel: this.titleLabelText,
            name: Extensible.calendar.data.EventMappings.Title.name,
            anchor: '90%'
        });
        // Added who field
        this.whoField = Ext.create('Ext.form.TextField', {
            fieldLabel: this.whoLabelText,
            name: Extensible.calendar.data.EventMappings.Who.name,
            anchor: '90%'
        });
        this.dateRangeField = Ext.create('Extensible.form.field.DateRange', {
            fieldLabel: this.datesLabelText,
            singleLine: false,
            anchor: '90%',
            listeners: {
                'change': Ext.bind(this.onDateChange, this)
            }
        });
        this.reminderField = Ext.create('Extensible.calendar.form.field.ReminderCombo', {
            name: Extensible.calendar.data.EventMappings.Reminder.name,
           fieldLabel: this.reminderLabelText,
           anchor: '70%'
        });
        this.notesField = Ext.create('Ext.form.TextArea', {
            fieldLabel: this.notesLabelText,
            name: Extensible.calendar.data.EventMappings.Notes.name,
            grow: true,
            growMax: 150,
            anchor: '100%'
        });
        this.locationField = Ext.create('Ext.form.TextField', {
            fieldLabel: this.locationLabelText,
            name: Extensible.calendar.data.EventMappings.Location.name,
            anchor: '100%'
        });
        this.urlField = Ext.create('Ext.form.TextField', {
            fieldLabel: this.webLinkLabelText,
            name: Extensible.calendar.data.EventMappings.Url.name,
            anchor: '100%'
        });

        var leftFields = [this.titleField, this.whoField, this.dateRangeField, this.reminderField], 
              rightFields = [this.notesField, this.locationField, this.urlField];

        if(this.enableRecurrence){
            this.recurrenceField = Ext.create('Extensible.form.recurrence.Fieldset', {
                name: Extensible.calendar.data.EventMappings.RRule.name,
                fieldLabel: this.repeatsLabelText,
                anchor: '90%'
            });
            leftFields.splice(2, 0, this.recurrenceField);
        }

        if(this.calendarStore){
            this.calendarField = Ext.create('Extensible.calendar.form.field.CalendarCombo', {
                store: this.calendarStore,
                fieldLabel: this.calendarLabelText,
                name: Extensible.calendar.data.EventMappings.CalendarId.name,
                anchor: '70%'
            });
            leftFields.splice(2, 0, this.calendarField);
        };

        this.items = [{
            id: this.id+'-left-col',
            columnWidth: this.colWidthLeft,
            layout: 'anchor',
            fieldDefaults: {
                labelWidth: this.labelWidth
            },
            border: false,
            items: leftFields
        },{
            id: this.id+'-right-col',
            columnWidth: this.colWidthRight,
            layout: 'anchor',
            fieldDefaults: {
                labelWidth: this.labelWidthRightCol || this.labelWidth
            },
            border: false,
            items: rightFields
        }];

        this.fbar = [{
            text:this.saveButtonText, scope: this, handler: this.onSave
        },{
            itemId:this.id+'-del-btn', text:this.deleteButtonText, scope:this, handler:this.onDelete
        },{
            text:this.cancelButtonText, scope: this, handler: this.onCancel
        }];

        this.addCls('ext-evt-edit-form');

        this.callParent(arguments);
    }
});

Ext.onReady(function(){
    var eventStore = Ext.create('Extensible.calendar.data.MemoryEventStore', {
        // defined in ../data/Events.js
        data: Ext.create('Extensible.example.calendar.data.Events')
    });
    
    //
    // example 1: simplest possible stand-alone configuration
    //
    Ext.create('Extensible.calendar.CalendarPanel', {
        eventStore: eventStore,
        renderTo: 'simple',
        title: 'Basic Calendar',
        width: 700,
        height: 500
    });
    
    //
    // example 2: shows off some common Ext.Panel configs as well as a 
    // few extra CalendarPanel-specific configs + a calendar store
    //
    Ext.create('Extensible.calendar.CalendarPanel', {
        id: 'cal-example2',
        eventStore: eventStore,
        renderTo: 'panel',
        title: 'Calendar with Panel Configs',
        activeItem: 1, // default to week view
        width: 700,
        height: 500,
        
        // Standard Ext.Panel configs:
        frame: true,
        collapsible: true,
        bbar: [{text: 'A Button', handler: function(){
            Ext.Msg.alert('Button', 'I work!');
        }}],
        
        listeners: {
            // A simple example showing how to handle a custom calendar event to
            // override default behavior. See the docs for all available events.
            'eventclick': {
                fn: function(panel, rec, el){
                    // override the default edit handling
                    //Ext.Msg.alert('App Click', 'Editing: ' + rec.data.Title);
                    
                    // return false to tell the CalendarPanel that we've handled the click and it 
                    // should ignore it (e.g., do not show the default edit window)
                    //return false;
                },
                scope: this
            }
        }
    });
});
