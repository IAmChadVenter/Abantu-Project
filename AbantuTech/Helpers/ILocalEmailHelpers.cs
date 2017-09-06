using Abantu_System.Models;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbantuTech.Helpers
{
    public interface ILocalEmailHelpers
    {
        // Get the committee that's calling the meeting
        Committee getCommitee(int? committeID);
        IEnumerable<AbantuMember> getCommitteMembers(int? committeID);
        List<EmailAddress> getEmailAddressMembers(int? committeID);
        Meeting getMeeting(int meetingID);
        string BroadcastEmail(int? CommitteID, int MeetingID, string subject);
        string MeetingMessage(int meetingID);
    }
}
