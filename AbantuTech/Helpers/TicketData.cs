using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AbantuTech.Models;

namespace AbantuTech.Helpers
{
    public class TicketData : ITicketData
    {
        private readonly ApplicationDbContext _context = new ApplicationDbContext();
        public IRepository<Category> Categories
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IRepository<Comment> Comments
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IRepository<Image> Images
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IRepository<Ticket> Tickets
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IRepository<User> Users
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public int SaveChanges()
        {
            throw new NotImplementedException();
        }
    }
}