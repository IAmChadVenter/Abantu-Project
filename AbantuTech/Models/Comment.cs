using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AbantuTech.Models
{
    public class Comment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int CommentId { get; set; }
        [Required]
        public string CommentText { get; set; }
        [Required]
        public int ticketId { get; set; }
        [Required]
        public string CommentedBy { get; set; }
        [Required]
        public DateTime CommentedOn { get; set; }
    }
}