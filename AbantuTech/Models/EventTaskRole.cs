using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AbantuTech.Models
{
    public class EventTaskRole
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int eventRoleId { get; set; }
        [Required]
        public string eventRoleName { get; set; }
        public int? eventTeamId { get; set; }
        public EventOrganizers Organizers { get; set; }
    }
}