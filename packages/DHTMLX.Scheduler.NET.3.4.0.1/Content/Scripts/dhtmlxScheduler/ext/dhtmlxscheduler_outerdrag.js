/*
@license
dhtmlxScheduler.Net v.3.4.0 Professional Evaluation

This software is covered by DHTMLX Evaluation License. Contact sales@dhtmlx.com to get Commercial or Enterprise license. Usage without proper license is prohibited.

(c) Dinamenta, UAB.
*/
Scheduler.plugin(function(e){e.attachEvent("onTemplatesReady",function(){function t(t,n,i,r){if(!e.checkEvent("onBeforeExternalDragIn")||e.callEvent("onBeforeExternalDragIn",[t,n,i,r,a])){var o=e.attachEvent("onEventCreated",function(n){e.callEvent("onExternalDragIn",[n,t,a])||(this._drag_mode=this._drag_id=null,this.deleteEvent(n))}),d=e.getActionData(a),s={start_date:new Date(d.date)};if(e.matrix&&e.matrix[e._mode]){var l=e.matrix[e._mode];s[l.y_property]=d.section;var _=e._locate_cell_timeline(a);
s.start_date=l._trace_x[_.x],s.end_date=e.date.add(s.start_date,l.x_step,l.x_unit)}e._props&&e._props[e._mode]&&(s[e._props[e._mode].map_to]=d.section),e.addEventNow(s),e.detachEvent(o)}}var a,n=new dhtmlDragAndDropObject,i=n.stopDrag;n.stopDrag=function(e){return a=e||event,i.apply(this,arguments)},n.addDragLanding(e._els.dhx_cal_data[0],{_drag:function(e,a,n,i){t(e,a,n,i)},_dragIn:function(e,t){return e},_dragOut:function(e){return this}}),dhtmlx.DragControl&&dhtmlx.DragControl.addDrop(e._els.dhx_cal_data[0],{
onDrop:function(e,n,i,r){var o=dhtmlx.DragControl.getMaster(e);a=r,t(e,o,n,r.target||r.srcElement)},onDragIn:function(e,t,a){return t}},!0)})});