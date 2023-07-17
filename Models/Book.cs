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


    }
    public class PaginateBook : IPagination
    {
        public Pagination Pagination { get; set; }
        public List<Book> Book { get; set; }
    }

}