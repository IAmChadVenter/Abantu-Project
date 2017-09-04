using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace AbantuTech.Models
{
    public class RolesView
    {
        [Required]
        public string RoleName { get; set; }
    }
}