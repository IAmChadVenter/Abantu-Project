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
        public string text { get; set; }
        [Required(ErrorMessage = "*")]

        [DisplayName("Start Date")]
        [DataType(DataType.Date)]
        public DateTime start_date { get; set; }
        [Required(ErrorMessage = "*")]

        [DataType(DataType.Date)]
        [DisplayName("End Date")]
        public DateTime end_date { get; set; }

        [Required(ErrorMessage = "*")]
        [MaxLength(50)]
        public string Name { get; set; }
      
        [Required(ErrorMessage = "*")]
        [MaxLength(50)]
        public string Venue { get; set; }


        public int Programme_ID { get; set; }
        public virtual Programme programme { get; set; }

        //public int Programme_ID { get; set; }
        //public virtual Programme Programme { get; set; }

    }
}