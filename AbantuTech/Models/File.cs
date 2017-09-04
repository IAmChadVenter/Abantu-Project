using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace AbantuTech.Models
{
    public enum FileType
    {

        Pdf

    }
    public class File
    {
        public int FileId { get; set; }
        [StringLength(255)]
        public string FileName { get; set; }
        public FileType FileType { get; set; }
        public byte[] Content { get; set; }
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



