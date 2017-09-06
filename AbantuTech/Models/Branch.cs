using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Abantu_System.Models
{
    public class Branch
    {
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Branch_ID { get; set; }

        [Required(ErrorMessage = "*")]
        [MaxLength(50)]
        [DisplayName("Branch Name")]
        public string Branch_Name { get; set; }
    
        [DataType(DataType.PhoneNumber)]
        [RegularExpression("[0-9+]+", ErrorMessage = "Please enter a valid telephone number")]
        [Required(ErrorMessage = "*")]
        [MaxLength(50)]
        public string Telephone { get; set; }

        [DisplayName("Physical Address")]
        [Required(ErrorMessage = "*")]
        [MaxLength(50)]
        public string PhysicalAddress { get; set; }

        //Navigational Properties
        public ICollection<AbantuMember> Members { get; set; }
    }
}