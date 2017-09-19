using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AbantuTech.Models
{
    public class TicketComment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int comID { get; set; }
        public string Comment { get; set; }
        public int? tId { get; set; }
        public HelpTicket HelpTicket { get; set; }
    }
}