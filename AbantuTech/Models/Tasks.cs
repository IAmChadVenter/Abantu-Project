using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Abantu_System.Models
{
    public enum status
    {
        TODO, Doing, Done
    }
    public class Tasks
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required(ErrorMessage = "*")]
        [StringLength(50)]
        [Display(Name ="Task Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "*")]
        [StringLength(200)]
        public string Description { get; set; }

        //[Display(Name = "Commitee Name")]
        public string CName { get; set; }


        [Required(ErrorMessage = "*")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "*")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
      
        public status status { get; set; }
        //Navigational Properties
        public int Committee_ID { get; set; }
        public Committee Committee { get; set; }

    }
}
