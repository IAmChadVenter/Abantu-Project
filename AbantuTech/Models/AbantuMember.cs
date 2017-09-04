using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.ComponentModel;
using AbantuTech.Models;

namespace Abantu_System.Models
{
    public class AbantuMember
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "*")]
        public int Member_ID { get; set; }

        [Required(ErrorMessage = "*")]
        [StringLength(35, MinimumLength = 4)]
        public string Surname { get; set; }

        [Required(ErrorMessage = "*")]
        [StringLength(35, MinimumLength = 4)]
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "*")]
        [DisplayName("Date Of Birth")]
        [DataType(DataType.Date)]

        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "*")]
        [MaxLength(10)]
        public string Gender { get; set; }

        [Required(ErrorMessage = "*")]
        [DataType(DataType.EmailAddress)]
        [MaxLength(50)]
        [RegularExpression(@"[a-z0-9.%+-]+@[a-z0-9.-]+\.[a-z]{2,9}", ErrorMessage = "Please enter correct email address")]
        [DisplayName("Email Address")]
        public string Email { get; set; }

       

        //[Required(ErrorMessage = "Telephone Number Required")]
        //[Display(Name = "Phone Number")]
        //[DataType(DataType.PhoneNumber)]
        //[RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Entered phone format is not valid.")]
        //public string PhoneNumber { get; set; }


        [Required(ErrorMessage = "Telephone Number Required")]
        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber)]
       
        [RegularExpression(@"^((?:\+27|27)|0)(\d{2})-?(\d{3})-?(\d{4})$", ErrorMessage = "Entered phone format is not valid.")]
        public string PhoneNumber { get; set; }

        [Required]
        [Display(Name = "City")]
        public string City { get; set; }

        [Required]
        [Display(Name = "Zip Code")]
        public string ZipCode { get; set; }

        [Required]
        [Display(Name = "Province")]
        public string Province { get; set; }

        [DisplayName("Committee Name ")]
        public string CommitteeName { get; set; }

        [Required(ErrorMessage = "*")]
        [StringLength(10)]
        [DisplayName("Member Type")]
        public string Type { get; set; }
        public bool isAccepted { get; set; }
        public bool isDeactRequested { get; set; }
        public bool deactApproved { get; set; }
        public bool reactApproved { get; set; }
        public bool isReactRequested { get; set; }
        public string DeactReason { get; set; }
        public bool isProfileActive { get; set; }
        public int? Branch_ID { get; set; }
        public virtual Branch Branch { get; set; }
        public int? Committee_ID { get; set; }
        public Committee Committee { get; set; }
        public ICollection<EmergencyContact> Contacts { get; set; }


    }
}