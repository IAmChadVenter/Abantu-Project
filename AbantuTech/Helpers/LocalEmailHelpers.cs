using AbantuTech.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SendGrid;
using SendGrid.Helpers;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;
using Abantu_System.Models;

namespace AbantuTech.Helpers
{
    
    public class LocalEmailHelpers : ILocalEmailHelpers
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        public Committee getCommitee(int? committeID)
        {
            var committe = _db.Committtes
                .FirstOrDefault(x => x.Committee_ID == committeID);
            return committe;
        }
        public IEnumerable<AbantuMember> getCommitteMembers(int? committeID)
        {
            var committe = getCommitee(committeID);
            var members = _db.Members
                .Where(x => x.Committee_ID == committe.Committee_ID);
            return members;
        }
        public List<EmailAddress> getEmailAddressMembers(int? committeID)
        {
            var members = getCommitteMembers(committeID);
            List<EmailAddress> emails = new List<EmailAddress>();

            if(members != null)
            {
                foreach(var member in members)
                {
                    EmailAddress _memberEmail = new EmailAddress(member.Email);
                    emails.Add(_memberEmail);
                }
            }
            return emails;
        }
        public string BroadcastEmail(int? CommitteID, int MeetingID, string subject)
        {
            List<EmailAddress> emails = getEmailAddressMembers(CommitteID);
            var client = new SendGridClient("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");
            var from = new EmailAddress("no-reply@abantutech.com", "Abantu Tech");

            var htmlContent = MeetingMessage(MeetingID);
            var msg = MailHelper.CreateSingleEmailToMultipleRecipients(from, emails, subject, string.Empty, htmlContent);
            var response = client.SendEmailAsync(msg);
            return response.ToString();
        }
        public Meeting getMeeting(int meetingID)
        {
            var meeting = _db.Meetings
                .FirstOrDefault(x => x.Meeting_ID == meetingID);
            return meeting;
        }
        public string MeetingMessage(int meetingID)
        {
            var meeting = getMeeting(meetingID);
            var committe = getCommitee(meeting.Committee_ID);
            string header = "Greetings, Abantu Member! <br/><br/>";

            string body = "You're invited attend the following meeting: <br/> <b> Facilitator : </b><br/>" + committe.Committee_Name + 
                "<br/> <b>Purpose : </b>" + meeting.Purpose + 
                "<br/> <b>Venue : </b>" + meeting.Location +
                "<br/> <b>Date : </b>" + meeting.Date.Date +
                "<br/> <b>Time : </b>" + meeting.Start_Time + " - " + meeting.End_Time + "<br/> <br/>";
            string footer = "Kind Regards <br/> Abantu Tech";
            return header + body + footer;
        }

    }
    
}