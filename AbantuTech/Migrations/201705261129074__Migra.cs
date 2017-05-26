namespace AbantuTech.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _Migra : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Branches",
                c => new
                    {
                        Branch_ID = c.Int(nullable: false, identity: true),
                        Branch_Name = c.String(nullable: false, maxLength: 50),
                        Telephone = c.String(nullable: false, maxLength: 50),
                        PhysicalAddress = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Branch_ID);
            
            CreateTable(
                "dbo.AbantuMembers",
                c => new
                    {
                        Member_ID = c.Int(nullable: false, identity: true),
                        Surname = c.String(nullable: false, maxLength: 35),
                        FirstName = c.String(nullable: false, maxLength: 35),
                        DateOfBirth = c.DateTime(nullable: false),
                        Gender = c.String(nullable: false, maxLength: 10),
                        Email = c.String(nullable: false, maxLength: 50),
                        PhoneNumber = c.String(nullable: false),
                        City = c.String(nullable: false),
                        Province = c.String(nullable: false),
                        ZipCode = c.String(nullable: false),
                        Type = c.String(nullable: false, maxLength: 10),
                        isAccepted = c.Boolean(nullable: false),
                        Branch_ID = c.Int(),
                        Committee_ID = c.Int(),
                    })
                .PrimaryKey(t => t.Member_ID)
                .ForeignKey("dbo.Branches", t => t.Branch_ID)
                .ForeignKey("dbo.Committees", t => t.Committee_ID)
                .Index(t => t.Branch_ID)
                .Index(t => t.Committee_ID);
            
            CreateTable(
                "dbo.Committees",
                c => new
                    {
                        Committee_ID = c.Int(nullable: false, identity: true),
                        Committee_Name = c.String(nullable: false, maxLength: 50),
                        Description = c.String(nullable: false, maxLength: 200),
                    })
                .PrimaryKey(t => t.Committee_ID);
            
            CreateTable(
                "dbo.Meetings",
                c => new
                    {
                        Meeting_ID = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Start_Time = c.String(),
                        End_Time = c.String(),
                        Location = c.String(),
                        Purpose = c.String(),
                        Committee_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Meeting_ID)
                .ForeignKey("dbo.Committees", t => t.Committee_ID, cascadeDelete: true)
                .Index(t => t.Committee_ID);
            
            CreateTable(
                "dbo.Programmes",
                c => new
                    {
                        Programme_ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Description = c.String(nullable: false, maxLength: 200),
                        Committee_ID = c.Int(),
                    })
                .PrimaryKey(t => t.Programme_ID)
                .ForeignKey("dbo.Committees", t => t.Committee_ID)
                .Index(t => t.Committee_ID);
            
            CreateTable(
                "dbo.Events",
                c => new
                    {
                        Event_ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Description = c.String(nullable: false, maxLength: 200),
                        Venue = c.String(nullable: false, maxLength: 50),
                        Start_Time = c.String(nullable: false),
                        Finish_Time = c.String(),
                        Programme_Programme_ID = c.Int(),
                    })
                .PrimaryKey(t => t.Event_ID)
                .ForeignKey("dbo.Programmes", t => t.Programme_Programme_ID)
                .Index(t => t.Programme_Programme_ID);
            
            CreateTable(
                "dbo.Tasks",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Description = c.String(nullable: false, maxLength: 200),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        status = c.Int(nullable: false),
                        Committee_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Committees", t => t.Committee_ID, cascadeDelete: true)
                .Index(t => t.Committee_ID);
            
            CreateTable(
                "dbo.EmergencyContacts",
                c => new
                    {
                        contactID = c.Guid(nullable: false, identity: true),
                        name = c.String(nullable: false),
                        relationship = c.String(nullable: false),
                        homePhone = c.String(),
                        alternativePhone = c.String(nullable: false),
                        Member_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.contactID)
                .ForeignKey("dbo.AbantuMembers", t => t.Member_ID, cascadeDelete: true)
                .Index(t => t.Member_ID);
            
            CreateTable(
                "dbo.BudgetExpenses",
                c => new
                    {
                        ExpenseName = c.String(nullable: false, maxLength: 50),
                        Amount = c.Double(nullable: false),
                        Budget_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ExpenseName)
                .ForeignKey("dbo.Budgets", t => t.Budget_ID, cascadeDelete: true)
                .Index(t => t.Budget_ID);
            
            CreateTable(
                "dbo.Budgets",
                c => new
                    {
                        Budget_ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Amount = c.Double(nullable: false),
                        Programme_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Budget_ID)
                .ForeignKey("dbo.Programmes", t => t.Programme_ID, cascadeDelete: true)
                .Index(t => t.Programme_ID);
            
            CreateTable(
                "dbo.ProgrammeMembers",
                c => new
                    {
                        ID = c.Guid(nullable: false, identity: true),
                        Member_ID = c.Int(nullable: false),
                        Programme_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AbantuMembers", t => t.Member_ID, cascadeDelete: true)
                .ForeignKey("dbo.Programmes", t => t.Programme_ID, cascadeDelete: true)
                .Index(t => t.Member_ID)
                .Index(t => t.Programme_ID);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.ProgrammeMembers", "Programme_ID", "dbo.Programmes");
            DropForeignKey("dbo.ProgrammeMembers", "Member_ID", "dbo.AbantuMembers");
            DropForeignKey("dbo.Budgets", "Programme_ID", "dbo.Programmes");
            DropForeignKey("dbo.BudgetExpenses", "Budget_ID", "dbo.Budgets");
            DropForeignKey("dbo.EmergencyContacts", "Member_ID", "dbo.AbantuMembers");
            DropForeignKey("dbo.Tasks", "Committee_ID", "dbo.Committees");
            DropForeignKey("dbo.Events", "Programme_Programme_ID", "dbo.Programmes");
            DropForeignKey("dbo.Programmes", "Committee_ID", "dbo.Committees");
            DropForeignKey("dbo.AbantuMembers", "Committee_ID", "dbo.Committees");
            DropForeignKey("dbo.Meetings", "Committee_ID", "dbo.Committees");
            DropForeignKey("dbo.AbantuMembers", "Branch_ID", "dbo.Branches");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.ProgrammeMembers", new[] { "Programme_ID" });
            DropIndex("dbo.ProgrammeMembers", new[] { "Member_ID" });
            DropIndex("dbo.Budgets", new[] { "Programme_ID" });
            DropIndex("dbo.BudgetExpenses", new[] { "Budget_ID" });
            DropIndex("dbo.EmergencyContacts", new[] { "Member_ID" });
            DropIndex("dbo.Tasks", new[] { "Committee_ID" });
            DropIndex("dbo.Events", new[] { "Programme_Programme_ID" });
            DropIndex("dbo.Programmes", new[] { "Committee_ID" });
            DropIndex("dbo.Meetings", new[] { "Committee_ID" });
            DropIndex("dbo.AbantuMembers", new[] { "Committee_ID" });
            DropIndex("dbo.AbantuMembers", new[] { "Branch_ID" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.ProgrammeMembers");
            DropTable("dbo.Budgets");
            DropTable("dbo.BudgetExpenses");
            DropTable("dbo.EmergencyContacts");
            DropTable("dbo.Tasks");
            DropTable("dbo.Events");
            DropTable("dbo.Programmes");
            DropTable("dbo.Meetings");
            DropTable("dbo.Committees");
            DropTable("dbo.AbantuMembers");
            DropTable("dbo.Branches");
        }
    }
}
