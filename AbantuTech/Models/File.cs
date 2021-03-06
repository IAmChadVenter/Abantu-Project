﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace AbantuTech.Models
{
    public enum FileType
    {
        Pdf,
        Png,
        Txt
    }
    public class File
    {
        public int FileId { get; set; }
        [StringLength(255)]
        [DisplayName("File Name")]
        public string FileName { get; set; }
        [DisplayName("File Type")]
        public FileType FileType { get; set; }
        public byte[] Content { get; set; }
        public int? helpID { get; set; }
        public HelpTicket HelpTicket { get; set; }
    }
    public class ApplicationForm
    {
        [Key]
        public int FormId { get; set; }
        [StringLength(255)]
        public string FileName { get; set; }
        public FileType FileType { get; set; }
        public byte[] Content { get; set; }
    }
}



