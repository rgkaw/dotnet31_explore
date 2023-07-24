
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace mvc.Models
{
    public class Attachment
    {
        [Key]
        public Guid Guid { get; set; } = Guid.NewGuid();

        [Required]
        public string Filename {get;set;}
        public string Location {get;set;}
        public long Size {get;set;}
        public DateTime DateCreated {get;} = DateTime.Now;
    }

    public class PaginateAttachment : IPagination 
    {
        public Pagination Pagination {get;set;}
        public List<Attachment> Attachment {get;set;}
    }

    
}
