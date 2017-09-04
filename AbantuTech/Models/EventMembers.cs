using Abantu_System.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AbantuTech.Models
{
    public class EventMembers
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EventMembers_ID { get; set; }
        public int Event_ID { get; set; }
        public Event Event { get; set; }
        public int Member_ID { get; set; }
        public AbantuMember AbantuMember { get; set; }
    }
}