using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace mvc.Models
{
    public class User
    {
        public User()
        {
            Username = "";
        }
        [Key]
        public string Username { get; set; } = string.Empty;
        [Required]
        public byte[] PasswordHash { get; set; }
        [Required]
        public byte[] PasswordSalt { get; set; }
        [Required]
        public virtual DateTime CreationDate { get; } = DateTime.Now;
        public string Role {get;set;} = "user";

        public List<OwnedBook> OwnedBooks {get;set;}
        public double balance = 1000;

    }
    public class OwnedBook
    {
        [Key]
        public Guid Guid {get;set;} = Guid.NewGuid();
        [Required]
        public User User {get;set;}
        [Required]
        public Book Book {get;set;}
    }

    public class PaginateUser : IPagination 
    { 
        public Pagination Pagination { get; set; }
        public List<User> User { get; set; }
    }

    public enum ROLE
    {

    }

}
