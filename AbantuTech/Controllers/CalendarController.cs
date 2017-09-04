using Abantu_System.Models;
using AbantuTech.Models;
using DHTMLX.Common;
using DHTMLX.Scheduler;
using DHTMLX.Scheduler.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AbantuTech.Controllers
{
    public class CalendarController : Controller
    {        // GET: Calendar
        public ActionResult Index()
        {
            var sched = new DHXScheduler(this);
            sched.Skin = DHXScheduler.Skins.Terrace;
            sched.LoadData = true;
            sched.EnableDataprocessor = true;
            sched.InitialDate = new DateTime(2016, 5, 5);
            return View(sched);
        }

        public ContentResult Data()
        {
            return (new SchedulerAjaxData(
                 new ApplicationDbContext().Events.
                 Select(e => new {id= e.Event_ID, e.text, e.start_date, e.end_date, e.Name, e.Venue})
                )
             );
            //var data = new SchedulerAjaxData(new ApplicationDbContext().Events.Select(e => new { id = e.Event_ID, startdate = e.start_date, end_date = e.end_date, text = "" }));
            //return data;

        }
        
        public ContentResult Save(int? id, FormCollection actionValues)
        {
            var action = new DataAction(actionValues);
            var changedEvent = DHXEventsHelper.Bind<Event>(actionValues);
            if(id!=null)
            {
                changedEvent.Event_ID = id.Value;
            }

            var entities = new ApplicationDbContext();
            try
            {
                switch (action.Type)
                {
                    case DataActionTypes.Insert:
                        entities.Events.Add(changedEvent);
                        break;
                    case DataActionTypes.Delete:
                        changedEvent = entities.Events.FirstOrDefault(ev => ev.Event_ID == action.SourceId);
                        entities.Events.Remove(changedEvent);
                        break;
                    default:// "update"
                        var target = entities.Events.Single(e => e.Event_ID == changedEvent.Event_ID);
                        DHXEventsHelper.Update(target, changedEvent, new List<string> { "Event_ID" });
                        break;
                }
                entities.SaveChanges();
                action.TargetId = changedEvent.Event_ID;
            }
            catch (Exception a)
            {
                action.Type = DataActionTypes.Error;
            }
            return (new AjaxSaveResponse(action));
        }

    }
}