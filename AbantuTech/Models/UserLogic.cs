using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AbantuTech.Models;

namespace AbantuTech.Models
{
    public class UserLogic
    {
        ApplicationDbContext db = new ApplicationDbContext();

        //Adds A User To UseTable From REGISTER View
        //After Capturing User Login Details
        public void AddNew(RegisterViewModel model)
        {
            //ContactNo, Name, Username
            UserTable table = new UserTable();


            table.Username = model.Email;

            db.UserTables.Add(table);
            db.SaveChanges();
        }

        /*
        //Adds Uses Information For User Who Uses Facebook Login (External Login)
        public void facebookNewUser(RegisterExternalLoginModel model)
        {
            UsersTable table = new UsersTable();

            table.ContactNo = model.ContactNo;
            table.Name = model.Name;
            table.Username = model.UserName;

            db.UsersTables.Add(table);
            db.SaveChanges();

        }*/

        //Search For A User Based On Username
        public UserTable FindOne(string username)
        {
            return db.UserTables.First(x => x.Username.Equals(username));
        }
    }
}
