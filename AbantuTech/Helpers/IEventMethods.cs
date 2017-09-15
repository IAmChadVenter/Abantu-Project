using Abantu_System.Models;
using AbantuTech.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbantuTech.Helpers
{
    public interface IEventMethods
    {
        IEnumerable<ProgrammeMember> getPMembers(int id);
        void addToTeam(EventOrganizers organizer);
        EventOrganizers getTeam(int id);
        void assignTask(EventOrganizers organizer, int id);
        ProgrammeMember viewTeamMember(int id);
        string issueAnEmail(string subject, int id);
        string emailMessage(int memberID);
        void Save();
        
    }
}
