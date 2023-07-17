using System.ComponentModel.DataAnnotations;

namespace mvc.Models
{
    public class Role
    {
        [Key]
        public int RoleId { get; }
        public string RoleName { get; set; }
    }
}
