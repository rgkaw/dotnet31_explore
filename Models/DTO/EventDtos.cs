using System.ComponentModel.DataAnnotations;

namespace mvc.Models.DTO
{
    public class EventDto
    {

        [Required]
        public string Name {get;set;}
        [Required]
        public string Description { get; set; } = string.Empty;
        [Required]
        public string Type {get;set;}
        
    }
    
}