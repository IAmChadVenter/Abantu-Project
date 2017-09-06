using Abantu_System.Models;
using AbantuTech.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbantuTech.Helpers
{
    public interface IApplication
    {
        void postApplication(AbantuMember member);
        void postEmergiencyContact(EmergencyContact contact, int id);
        AbantuMember viewApplication(int id);
        IEnumerable<AbantuMember> getApplications();
        void acceptApplication(int memberID);
        void rejectApplication(int memberID);
        string generatePassword(int memberID);
        ApplicationUser createUser(int memberID);
        string issueAnEmail(string subject, int memberID);
        string emailMessage(int memberID);

        void Save();
    }
}
