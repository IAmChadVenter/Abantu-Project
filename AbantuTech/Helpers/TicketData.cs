using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AbantuTech.Models;

namespace AbantuTech.Helpers
{
    public class TicketData : ITicketData
    {
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

        public ApplicationDbContext Context
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