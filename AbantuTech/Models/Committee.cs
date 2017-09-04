using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Abantu_System.Models
{   //Committees are groups of Members who oversee Programmes. 
    public class Committee
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Committee_ID { get; set; }

        [Required(ErrorMessage = "*")]
        [MaxLength(50)]
        [DisplayName("Committee Name")]
        public string Committee_Name { get; set; }


        [Required(ErrorMessage = "*")]
        [MaxLength(200)]
        [DisplayName("Description")]

        public string Description { get; set; }

        public ICollection <AbantuMember> Members { get; set; }
        public ICollection<Tasks> Tasks { get; set; }
        public ICollection<Meeting> Meetings  { get; set; }
        public ICollection<Programme> Programmes { get; set; }

    }
}