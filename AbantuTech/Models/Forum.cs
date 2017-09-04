using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AbantuTech.Models
{
    public class Forum
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Topic { get; set; }
        public int Views { get; set; }
        public string Creator { get; set; }
        [DataType(DataType.Date)]
        public DateTime DataCreated { get; set; }
    }
}