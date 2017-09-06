using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Abantu_System.Models
{
    public class ProgrammeMember
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }

        public int Member_ID { get; set; }
        //Association class
        public virtual AbantuMember Member { get; set; }
        //Association class
        public int Programme_ID { get; set; }
        public virtual Programme Programme { get; set; }
        
    }
}