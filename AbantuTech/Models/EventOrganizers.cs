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
    public class EventOrganizers
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int eventTeamId { get; set; }
        [Required]
        [DisplayName("Team Name")]
        public string teamName { get; set; }
        public int eventId { get; set; }
        public Event events { get; set; }
        public ICollection<ProgrammeMember> pmember { get; set; }
        public ICollection<EventTaskRole> eventroles { get; set; }
    }
}