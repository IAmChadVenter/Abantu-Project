using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AbantuTech.Models
{
    public class Newsletter
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "*")]
        public int Email_ID { get; set; }        

        [Required(ErrorMessage = "*")]
        [DataType(DataType.EmailAddress)]
        [MaxLength(50)]
        [RegularExpression(@"[a-z0-9.%+-]+@[a-z0-9.-]+\.[a-z]{2,9}", ErrorMessage = "Please enter correct email address")]
        [DisplayName("Email Address")]
        public string Email { get; set; }

    }
}