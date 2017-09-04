using Abantu_System.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbantuTech.Helpers
{
    public interface IProfileDeactivation
    {
        AbantuMember viewDeactRequest(int id);
        AbantuMember viewReactRequest(int id);
        IEnumerable<AbantuMember> getDeactRequests();
        IEnumerable<AbantuMember> getReactRequests();
        void initDeactivation(int memberID);
        void initReactivation(int memberID);
        string issueDeactEmail(string subject, int memberID);
        string emailMessageForDeact(int memberID);
        string issueReactEmail(string subject, int memberID);
        string emailMessageForReact(int memberID);
        void Save();
    }
}
