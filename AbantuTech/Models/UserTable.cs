using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AbantuTech.Models
{
    public class UserTable
    {
        public int Id { get; set; }
        public string Username { get; set; } //Email
        public string Name { get; set; }
        public string ContactNo { get; set; }
    }
}