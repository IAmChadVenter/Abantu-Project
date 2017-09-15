using Abantu_System.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AbantuTech.Models
{
    public class EventOrganizer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int eventTeamId { get; set; }
        [Required]
        public string eventTask { get; set; }
        public int? eventId { get; set; }
        public virtual Event Events { get; set; }
        public virtual ICollection<ProgrammeMember> pmember { get; set; }

    }
}