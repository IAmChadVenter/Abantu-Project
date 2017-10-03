using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AbantuTech.Models
{
    public enum Category
    {
        Applications = 0,
        Branches = 1,
        Committees = 2,
        Donations = 3,
        Events = 4,
        Forums = 5,
        Meetings = 6,
        Programmes = 7, 
        ShoppingCart = 8,
        Other = 9
    }
}