using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace mvc.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } ="";
        public string Author { get; set; } = "";
        
        public DateTime DateCreated {get; set;} = DateTime.Now;

        public DateTime DateModified {get;set;}=DateTime.Now;

        public long Price {get;set;}=0;

        public int Stock {get;set;}=0;

    }
    public class PaginateBook : IPagination
    {
        public Pagination Pagination { get; set; }
        public List<Book> Book { get; set; }
    }

}