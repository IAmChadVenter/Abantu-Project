using Abantu_System.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AbantuTech.Models
{
    public class EmergiencyContact
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid contactID { get; set; }
        [Required]
        [Display(Name = "Full Name")]
        public string name { get; set; }
        [Required]
        [Display(Name = "Relationship")]
        public string relationship { get; set; }
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Home Phone")]
        public string homePhone { get; set; }
        [Required]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Mobile Number")]
        public string alternativePhone { get; set; }
        public int Member_ID { get; set; }
        public AbantuMember Member { get; set; }


    }
}