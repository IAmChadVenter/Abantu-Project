using Abantu_System.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AbantuTech.Models
{
    public class Notification
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid notifyId { get; set; }
        public string message { get; set; }
        public bool seen { get; set; }
        public string userName { get; set; }
        public int ID { get; set; }
        public Tasks Task { get; set; }
    }
}