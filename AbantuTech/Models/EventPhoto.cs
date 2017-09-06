using Abantu_System.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AbantuTech.Models
{
    public class EventPhoto
    {
        [Key]

        public int PhotoId { get; set; }

        [Display(Name = "Description")]

        [Required]

        public String Description { get; set; }

        [Display(Name = "Image Path")]

        public String ImagePath { get; set; }

        [Display(Name = "Thumb Path")]

        public String ThumbPath { get; set; }

        [Display(Name = "Created On")]

        public DateTime CreatedOn { get; set; }

        public int? eventid { get; set; }
        public Event Event { get; set; }


    }
}