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

    }

    public class PaginateUser : IPagination 
    { 
        public Pagination Pagination { get; set; }
        public List<User> User { get; set; }
    }
}
