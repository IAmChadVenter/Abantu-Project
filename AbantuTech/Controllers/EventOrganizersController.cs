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
using SendGrid.Helpers.Mail;
using System.Net.Mail;

namespace AbantuTech.Controllers
{
    public class EventOrganizersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: EventOrganizers
        public ActionResult Index(string searchString, string sortOrder, string currentFilter, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            var we = from s in db.Events
                     select s;
            we = db.Events.OrderBy(b => b.Name);

            if (!String.IsNullOrEmpty(searchString))
            {
                we = we.Where(s => s.Name.Contains(searchString));
            }

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            int pageSize = 8;
            int pageNumber = (page ?? 1);
            return View(we.ToPagedList(pageNumber, pageSize));
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
                var pmembers = db.ProgrammeMembers.Include(a => a.Programme).Where(x => x.Programme_ID == @event.Programme_ID && x.role == null).ToList();
                return pmembers;
            }
            return null;
        }
        public ActionResult programmeMemberList(Event @event, TeamMessages message)
        {
            ViewBag.StatusMessage =
                message == TeamMessages.TeamMemberLimit ? "Team has reached the limit"
                : "";
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
                        if (team.pmember.Count < 5)
                        {
                            member.eventTeamId = team.eventTeamId;
                            member.organizers = team;
                            team.pmember.Add(member);
                            return RedirectToAction("AssignTasks", new { id = member.eventTeamId });
                        }
                        else
                        {
                            return RedirectToAction("programmeMemberList", new { TeamMessages.TeamMemberLimit });
                        } 

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
                if (team != null)
                {
                    var member = team.pmember.FirstOrDefault(a=>a.eventTeamId==team.eventTeamId && a.role == null);
                    if (member != null)
                    {
                        roles.eventTeamId = team.eventTeamId;
                        team.eventroles.Add(roles);
                        member.role = roles.eventRoleName;
                        MailMessage message = new MailMessage();
                        message.From = new MailAddress("abantusystem@gmail.com");
                        message.To.Add(new MailAddress(member.Member.Email));
                        message.Subject = "Event Team for " + @event.Name;
                        message.Body = "You have been chosen to be a part of " + team.teamName + ".<br/><br/>"
                                    + "It is an event organizing team for the " + @event.Name + " event. <br/><br/>"
                                    + "You have been assigned the " + member.role + " task, We appreciate your help.";

                        using (var smtp = new SmtpClient())
                        {
                            var credential = new NetworkCredential
                            {
                                UserName = "abantusystem@gmail.com",  // replace with valid value
                                Password = "Abantu2017"  // replace with valid value
                            };
                            smtp.Credentials = credential;
                            smtp.Host = "smtp.gmail.com";
                            smtp.Port = 587;
                            smtp.EnableSsl = true;
                            smtp.Send(message);
                        }
                        return RedirectToAction("programmeMemberList", new { id = roles.eventTeamId });
                    }
                    else
                    {

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
        public enum TeamMessages
        {
            TeamMemberLimit,
            TeamMemberExist,
            AlreadyHasTask
        }

    }
}
