using Abantu_System.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        [DisplayName("Attendance Confirmed")]
        public bool attendanceConfirmed { get; set; }
        [DisplayName("Arrival Time")]
        public DateTime arrivalTime { get; set; }
        public bool hasRated { get; set; }
        public Event Event { get; set; }
        public int Member_ID { get; set; }
        [DisplayName("Email")]
        public string email_ { get; set; }
        public string Name { get; set; }
        [DisplayName("Last Name")]
        public string LName { get; set; }
        public AbantuMember AbantuMember { get; set; }
        public int RatingID { get; set; }

    }
}