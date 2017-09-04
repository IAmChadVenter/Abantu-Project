using Abantu_System.Models;
using AbantuTech.Models;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AbantuTech.Helpers
{
    public class DeactImplementation : IProfileDeactivation
    {
        private readonly ApplicationDbContext _context = new ApplicationDbContext();
        SendGridKey _key = new SendGridKey();
        public string emailMessageForDeact(int memberID)
        {
            string header = "", body = "";
            var member = viewDeactRequest(memberID);
            if (member != null)
            {
                header = "Greetings " + member.FirstName + " " + member.Surname + "<br/><br/>";

                if (member.deactApproved)
                {
                    body = "Your request for deactivation of your personal profile page has been completed by Admin.<br/><br/>"
                            + "However, it is indeed reversible. To do so, log in and go to 'Manage Acount' page";
                }
            }

            string footer = "Kind Regards <br/> Abantu Tech";
            return header + body + footer;
        }

        public string emailMessageForReact(int memberID)
        {
            string header = "", body = "";
            var member = viewReactRequest(memberID);
            if (member != null)
            {
                header = "Greetings " + member.FirstName + " " + member.Surname + "<br/><br/>";

                if (member.reactApproved)
                {
                    body = "We noticed that you are requesting reversal of the deactivation of your profile page. <br/><br/>" +
                            "With great news, we have done just that ! Sign in, to confirm its success";
                }
            }

            string footer = "Kind Regards <br/> Abantu Tech";
            return header + body + footer;
        }

        public IEnumerable<AbantuMember> getDeactRequests()
        {
            var deactrequests = _context.Members
                .Where(x => x.isDeactRequested == true)
                .ToList();
            return deactrequests;
        }

        public IEnumerable<AbantuMember> getReactRequests()
        {
            var reactrequests = _context.Members
                .Where(x => x.isProfileActive == false)
                .ToList();
            return reactrequests;
        }

        public void initDeactivation(int memberID)
        {
            var member = viewDeactRequest(memberID);
            if (member != null)
            {
                member.isProfileActive = false;
                member.deactApproved = true;
                Save();
                issueDeactEmail("Account Deactivation | Abantu Tech!", member.Member_ID);
            }
        }

        public void initReactivation(int memberID)
        {
            var member = viewReactRequest(memberID);
            if (member != null)
            {
                member.isProfileActive = true;
                member.reactApproved = true;
                Save();
                issueReactEmail("Account Reactivation | Abantu Tech!", member.Member_ID);
            }
        }

        public string issueDeactEmail(string subject, int memberID)
        {
            var member = viewDeactRequest(memberID);
            if (member != null)
            {
                var to = new EmailAddress(member.Email, member.FirstName);
                var client = new SendGridClient("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");
                var from = new EmailAddress("no-reply@abantutech.com", "Abantu Tech");

                var htmlContent = emailMessageForDeact(member.Member_ID);
                var msg = MailHelper.CreateSingleEmail(from, to, subject, string.Empty, htmlContent);
                var response = client.SendEmailAsync(msg);
                return response.ToString();
            }
            return null;

        }

        public string issueReactEmail(string subject, int memberID)
        {
            var member = viewReactRequest(memberID);
            if (member != null)
            {
                var to = new EmailAddress(member.Email, member.FirstName);
                var client = new SendGridClient("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");
                var from = new EmailAddress("no-reply@abantutech.com", "Abantu Tech");

                var htmlContent = emailMessageForReact(member.Member_ID);
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

        public AbantuMember viewDeactRequest(int id)
        {
            var deactrequest = _context.Members
                .FirstOrDefault(a => a.Member_ID == id && a.isDeactRequested);
            return deactrequest;
        }

        public AbantuMember viewReactRequest(int id)
        {
            var reactrequest = _context.Members
                .FirstOrDefault(a => a.Member_ID == id);
            return reactrequest;
        }
    }
}
