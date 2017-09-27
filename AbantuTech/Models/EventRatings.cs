using Abantu_System.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AbantuTech.Models
{
    public class EventRatings
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int RatingsId { get; set; }
        public int RatingAmt { get; set; }
        public int? EventMemberID { get; set; }
        public EventMembers EventMembers { get; set; }
        public int? EventID { get; set; }
        public Event Events { get; set; }
        
    }
}