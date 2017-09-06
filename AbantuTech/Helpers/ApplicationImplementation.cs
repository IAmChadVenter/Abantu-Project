using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Abantu_System.Models;
using AbantuTech.Models;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace AbantuTech.Helpers
{
    public class ApplicationImplementation : IApplication
    {
        private readonly ApplicationDbContext _context = new ApplicationDbContext();
        SendGridKey _key = new SendGridKey();
        public void postApplication(AbantuMember member)
        {
            if(member != null)
            {
                _context.Members.Add(member);
                Save();
            }
        }
        public void postEmergiencyContact(EmergencyContact contact, int id)
        {
            AbantuMember application = _context.Members
                .FirstOrDefault(m => m.Member_ID == id);
            if(application != null)
            {
                contact.Member_ID = application.Member_ID;
                contact.Member = application;
                _context.Contacts.Add(contact);
                application.Contacts.Add(contact);
                Save();
            }
            
        }

        public AbantuMember viewApplication(int id)
        {
            var application = _context.Members
                .Include(x => x.Contacts)
                .FirstOrDefault(a => a.Member_ID == id);
            return application;
        }

        public IEnumerable<AbantuMember> getApplications()
        {
            var applications = _context.Members
                .Where(x => x.isAccepted == false)
                .ToList();
            return applications;
        }

        public void acceptApplication(int memberID)
        {
            var member = viewApplication(memberID);
            if(member != null)
            {
                member.isAccepted = true;
                member.isProfileActive = true;
                Save();
                // create user
                var user = createUser(member.Member_ID);
                // if user was created
                if (user != null)
                    issueAnEmail("Congratulations | Abantu Tech!", member.Member_ID);
            }
        }

        public void rejectApplication(int memberID)
        {
            AbantuMember member = viewApplication(memberID);
            if(member != null)
            {
                // the 'isAccepted' value is already false
                // issue rejection email
                issueAnEmail("Application Rejected | Abantu Tech", member.Member_ID);
            }
        }
        public string issueAnEmail(string subject, int memberID)
        {
            var member = viewApplication(memberID);
            if(member != null)
            {
                var to = new EmailAddress(member.Email, member.FirstName);
                var client = new SendGridClient("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");
                var from = new EmailAddress("no-reply@abantutech.com", "Abantu Tech");

                var htmlContent = emailMessage(member.Member_ID);
                var msg = MailHelper.CreateSingleEmail(from, to, subject, string.Empty, htmlContent);
                var response = client.SendEmailAsync(msg);
                return response.ToString();
            }
            return null;
            
        }
        public ApplicationUser createUser(int memberID)
        {
            var UserManager = new UserManager<ApplicationUser>
                (new UserStore<ApplicationUser>(_context));
            var member = viewApplication(memberID);
            if(member != null)
            {
                var PasswordHash = new PasswordHasher();
                string Password = generatePassword(member.Member_ID);
                var user = new ApplicationUser
                {
                    Email = member.Email,
                    UserName = member.Email,
                    PhoneNumber = member.PhoneNumber,
                    PhoneNumberConfirmed = true,
                    EmailConfirmed = true,
                    PasswordHash = PasswordHash.HashPassword(Password)
                };
                UserManager.Create(user);
                return user;
            }
            return null;
        }

        public string generatePassword(int memberID)
        {
            var member = viewApplication(memberID);
            string password = "@Abantu" + member.Surname.Substring(0, 2).ToLower()
                + DateTime.Now.Day +
                member.FirstName.Substring(0, 2).ToUpper();
            return password;
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public string emailMessage(int memberID)
        {
            string header = "", body = "";
            var member = viewApplication(memberID);
            if(member != null)
            {
                header = "Greetings " + member.FirstName + " " + member.Surname + "<br/><br/>";

                if (member.isAccepted)
                {
                    body = "Congratulations! You've been approved to join Abantu Tech. <br/><br/>" +
                        "Your role has also been assigned, please the credentials provided below to access our online system <br/><br/>" +
                        "<b>UserName: </b>" + member.Email + "<br/>" +
                        "<b>Password: </b>" + generatePassword(member.Member_ID) + "<br/><br/>";
                    
                }
                else
                {
                    body = "We regret to inform you that your application to join AbantuTech has been rejected. <br/><br/>";
                }
            }
           
            string footer = "Kind Regards <br/> Abantu Tech";
            return header + body + footer;
        }
    }
}