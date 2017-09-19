using Abantu_System.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AbantuTech.Models
{
    public class HelpTicket
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int TicketId { get; set; }
        public DateTime tCreatedOn { get; set; }
        public int? cId { get; set; }
        public HelpCategory Category { get; set; }
        public ICollection<TicketComment> Comments { get; set; }
        public int? memberId { get; set; }
        public AbantuMember Members { get; set; }
    }
}