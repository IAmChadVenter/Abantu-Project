using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Abantu_System.Models
{
    public class Event
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Event_ID { get; set; }

        [Required(ErrorMessage = "*")]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required(ErrorMessage = "*")]
        [MaxLength(200)]
        public string Description { get; set; }

        [Required(ErrorMessage = "*")]
        [MaxLength(50)]
        public string Venue { get; set; }

        [Required(ErrorMessage = "*")]
        [DisplayName("Start Time ")]
        public string  Start_Time { get; set; }
        [DisplayName("Finish Time ")]
        public string Finish_Time { get; set; }

        //public int Programme_ID { get; set; }
        //public virtual Programme Programme { get; set; }
 
    }
}