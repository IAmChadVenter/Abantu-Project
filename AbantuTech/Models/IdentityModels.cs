using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Abantu_System.Models;

namespace AbantuTech.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        public DbSet<DonationAmount> DonationAmount { get; set; }
        public DbSet<AbantuMember> Members { get; set; }

        public DbSet<Branch> Branches { get; set; }

        public DbSet<Committee> Committtes { get; set; }

        public DbSet<Tasks> Tasks { get; set; }

        public DbSet<Budget> Budgets { get; set; }

        public DbSet<Event> Events { get; set; }

        public DbSet<Programme> Programmes { get; set; }

        public DbSet<EventMembers > EventMembers { get; set; }
        public DbSet<ProgrammeMember> ProgrammeMembers { get; set; }

        public DbSet<Meeting> Meetings { get; set; }

        public DbSet<BudgetExpenses> BudgetExpenses { get; set; }
        public DbSet<EmergencyContact> Contacts { get; set; }
        public DbSet<UserTable> UserTables { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        public DbSet<Support> Supports { get; set; }
        public DbSet<FileDetail> FileDetails { get; set; }
        public DbSet<Forum> Fora { get; set; }

        public System.Data.Entity.DbSet<AbantuTech.Models.File> Files { get; set; }
        public DbSet<ApplicationForm> Applications { get; set; }
    }
}