﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Abantu_System.Models;
using AbantuTech.Models;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SendGrid.Helpers.Mail;
using SendGrid;

namespace AbantuTech.Helpers
{
    public class EventImplementation : IEventMethods
    {
        private readonly ApplicationDbContext _context = new ApplicationDbContext();
        public void addToTeam(EventOrganizers organizer)
        {
            if(organizer!=null)
            {
                var m = getPMembers(organizer.events.Event_ID);
                if (m != null)
                {
                    var mem = m.FirstOrDefault(x => x.eventTeamId == organizer.eventTeamId);
                    Event @event = _context.Events.FirstOrDefault(x => x.Programme_ID == mem.Programme_ID);
                    if (mem == null && @event !=null)
                    {
                        var team = _context.Organizers.FirstOrDefault(x => x.eventId == @event.Event_ID);
                        if(team!=null)
                        {
                            team.pmember.Add(mem);
                        }
                        Save();
                    }
                }
            }
        }
        public void assignTask(EventOrganizers organizer, int id)
        {
            ProgrammeMember member = viewTeamMember(id);
            if(member!=null)
            {
                var roles = getEventRoles();
                if(roles != null)
                {
                    var role = roles.FirstOrDefault(x=>x.)
                }
                organizer.eventroles.Add(role);
                Save();

                issueAnEmail("Event Organizer Team || AbantuTech", member.Member_ID);
            }
        }

        public string emailMessage(int id)
        {
            string header = "", body = "";
            var member = viewTeamMember(id);
            var task = _context.EventTaskRoles.FirstOrDefault(x => x.eventTeamId == member.eventTeamId);
            if (member != null)
            {
                header = "Greetings " + member.Member.FirstName + " " + member.Member.Surname + "<br/><br/>";

                    body = "You have been added to an event organizing team for the " + member.organizers.events.Name + " event. <br/><br/>"
                        + "Your task for this event is " + task + ", We are grateful for your help. Thank you ! <br/><br/>"
                        + "The date of this event is " + member.organizers.events.start_date + ".<br/><br/>";
            }

            string footer = "Kind Regards <br/> Abantu Tech";
            return header + body + footer;
        }

        public IEnumerable<EventTaskRole> getEventRoles()
        {
            return _context.EventTaskRoles.ToList();
        }

        public IEnumerable<ProgrammeMember> getPMembers(int id)
        {
            var @event = _context.Events.FirstOrDefault(x => x.Event_ID == id);
            if (@event != null)
            {
                var pmembers = _context.ProgrammeMembers.Include(a=>a.Programme).Where(x => x.Programme_ID == @event.Programme_ID).ToList();
                return pmembers;
            }
            return null;
        }

        public EventOrganizers getTeam(int id)
        {
            var eventTeam = _context.Organizers.Include(x=>x.pmember).FirstOrDefault(x=>x.eventTeamId==id);
            return eventTeam;
            
        }


        public string issueAnEmail(string subject, int id)
        {
            var member = viewTeamMember(id);
            if(member!=null)
            {
                var to = new EmailAddress(member.Member.Email, member.Member.FirstName);
                var client = new SendGridClient("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");
                var from = new EmailAddress("no-reply@abantutech.com", "Abantu Tech");

                var htmlContent = emailMessage(member.Member_ID);
                var msg = MailHelper.CreateSingleEmail(from, to, subject, string.Empty, htmlContent);
                var response = client.SendEmailAsync(msg);
                return response.ToString();
            }
            return null;
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public ProgrammeMember viewTeamMember(int id)
        {
            var team = getTeam(id);
            var teammember = _context.ProgrammeMembers.Include(x => x.organizers).FirstOrDefault(x=>x.eventTeamId==team.eventTeamId);
            return teammember;
        }
    }
}