using Abantu_System.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AbantuTech.Models
{
    public class EventOrganizers
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int eventTeamId { get; set; }
        public int eventTask { get; set; }
        public int eventId { get; set; }
        public Event events { get; set; }
        public ICollection<ProgrammeMember> pmember { get; set; }
    }
}