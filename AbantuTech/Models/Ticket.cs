﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AbantuTech.Models
{
    public class Ticket
    {
        public class Ticket
        {
            private ICollection<Comment> comments;

            public Ticket()
            {
                this.comments = new HashSet<Comment>();
            }

            [Key]
            public int Id { get; set; }

            [DefaultValue(PriorityType.Medium)]
            public PriorityType Priority { get; set; }

            [Required]
            [StringLength(50)]
            public string Title { get; set; }

            [StringLength(1000)]
            public string Description { get; set; }

            public string AuthorId { get; set; }

            public virtual User Author { get; set; }

            public int CategoryId { get; set; }

            public virtual Category Category { get; set; }


            //public virtual ICollection<Comment> Comments
            //{
            //    get { return this.comments; }
            //    set { this.comments = value; }
            //}

        }
    }