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
        [Required]
        public string Comment { get; set; }
        [Required]
        public string tCreatedBy { get; set; }
        public bool isMember { get; set; }
        public DateTime tCreatedOn { get; set; }
        public HttpPostedFileBase AddFiles { get; set; }
        public int? cId { get; set; }
        public HelpCategory Category { get; set; }
    }
}