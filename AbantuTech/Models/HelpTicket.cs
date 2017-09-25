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
    public class HelpTicket
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int TicketId { get; set; }
        [Required]
        [DisplayName("Ticket Creation Date")]
        public DateTime tCreatedOn { get; set; }
        [Required]
        
        [DisplayName("Ticket Creator Username")]
        public string tCreatedBy { get; set; }
        public int? cId { get; set; }
        public string Comment { get; set; }
        public string AdminResponse { get; set; }
        public bool hasResponse { get; set; }
        public HelpCategory Category { get; set; }
        public int? memberId { get; set; }
        public AbantuMember Members { get; set; }
    }
}