using AbantuTech.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Abantu_System.Models
{
    public class Meeting
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Meeting_ID { get; set; }

        //[Required(ErrorMessage = "*")]
        //[StringLength(50)]
        //public string Topic { get; set; }

        //[Required(ErrorMessage = "*")]
        //public List<string> Agenda { get; set; } //List of items to be discussed in  the meeting

        //[Required(ErrorMessage = "*")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [DataType(DataType.Time)]
        [DisplayName("Start Time ")]
        public string Start_Time { get; set; }
        [DataType(DataType.Time)]
        [DisplayName("End Time ")]
        public string End_Time { get; set; }
        public string Location { get; set; }
        public String Purpose { get; set; }

        //Navigational Properties
        public int Committee_ID { get; set; }
        public Committee Committee { get; set; }

   
    }
}