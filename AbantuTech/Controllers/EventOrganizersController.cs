using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AbantuTech.Models;
using Abantu_System.Models;

namespace AbantuTech.Controllers
{
    public class EventOrganizersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: EventOrganizers
        public ActionResult Index()
        {
            return View(db.Organizers.ToList());
        }

        // GET: EventOrganizers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventOrganizers eventOrganizers = db.Organizers.Find(id);
            if (eventOrganizers == null)
            {
                return HttpNotFound();
            }
            return View(eventOrganizers);
        }

        // GET: EventOrganizers/Create
        public ActionResult Create()
        {

            return View();
        }

        // POST: EventOrganizers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "eventTeamId,teamName,eventId")] EventOrganizers eventOrganizers, int id)
        {
            var @event = db.Events.FirstOrDefault(x => x.Event_ID == id); 
            if (ModelState.IsValid)
            {
                eventOrganizers.eventId = @event.Event_ID;
                db.Organizers.Add(eventOrganizers);
                db.SaveChanges();
                return RedirectToAction("programmeMemberList", new { id = eventOrganizers.eventTeamId });
            }

            return View(eventOrganizers);
        }
        [HttpGet]
        public ActionResult getEvents()
        {
            return View(db.Events.ToList());
        }

        // GET: EventOrganizers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventOrganizers eventOrganizers = db.Organizers.Find(id);
            if (eventOrganizers == null)
            {
                return HttpNotFound();
            }
            return View(eventOrganizers);
        }

        // POST: EventOrganizers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "eventTeamId,teamName,eventId")] EventOrganizers eventOrganizers)
        {
            if (ModelState.IsValid)
            {
                db.Entry(eventOrganizers).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(eventOrganizers);
        }

        // GET: EventOrganizers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventOrganizers eventOrganizers = db.Organizers.Find(id);
            if (eventOrganizers == null)
            {
                return HttpNotFound();
            }
            return View(eventOrganizers);
        }

        // POST: EventOrganizers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EventOrganizers eventOrganizers = db.Organizers.Find(id);
            db.Organizers.Remove(eventOrganizers);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public IEnumerable<ProgrammeMember> getProgrammeMembers(int id)
        {
            var @event = db.Events.FirstOrDefault(x => x.Event_ID == id);
            if (@event != null)
            {
                var pmembers = db.ProgrammeMembers.Include(a => a.Programme).Where(x => x.Programme_ID == @event.Programme_ID).ToList();
                return pmembers;
            }
            return null;
        }
        public ActionResult programmeMemberList(Event @event)
        {
            if(ModelState.IsValid)
            {
                return View(getProgrammeMembers(@event.Event_ID));
            }
            return View(@event);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddToTeam(ProgrammeMember member, int id)
        {
            if (ModelState.IsValid)
            {
                var team = db.Organizers.FirstOrDefault(x => x.eventTeamId == id);
                if (team != null)
                {
                    var teammember = team.pmember.FirstOrDefault(x => x.Member_ID == member.Member_ID && x.organizers.eventTeamId == member.eventTeamId);
                    if (teammember != null)
                    {
                        return View(member);
                    }
                    else
                    {
                        member.eventTeamId = team.eventTeamId;
                        member.organizers = team;
                        team.pmember.Add(member);
                        return RedirectToAction("AssignTasks", new { id = member.eventTeamId });
                    }
                }
            }
            return View();
        }
        public ActionResult viewTeam(int id)
        {
            if (ModelState.IsValid)
            {
                var team = db.Organizers.Include(x => x.events).Include(x => x.pmember).Where(x => x.eventTeamId == id);
                if (team != null)
                {
                    return View(team.ToList());
                }
            }
            return View();
        }
        [HttpGet]
        public ActionResult AssignTasks(int id)
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AssignTasks(EventTaskRole roles, int id)
        {
            var @event = db.Events.FirstOrDefault(x => x.Event_ID == id);
            if (ModelState.IsValid)
            {
                var team = db.Organizers.FirstOrDefault(x => x.eventId == @event.Event_ID);
                if(team!=null)
                {
                    var member = team.pmember.FirstOrDefault(x => x.eventTeamId == team.eventTeamId);
                    if (member != null)
                    {
                        roles.eventTeamId = team.eventTeamId;
                        team.eventroles.Add(roles);
                        member.role = roles.eventRoleName;
                        return RedirectToAction("viewTeam", new { id = roles.eventTeamId });
                    }
                }
            }
            return View(roles);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
