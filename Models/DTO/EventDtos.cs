using System;
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
    public class EventScheduleDto
    {
        public EventScheduleDto(){

        }
        public EventScheduleDto(string id){
            EventGuid=id;
        }
        [Required]
        public string EventGuid {get;set;}
        [Required]
        public DateTime Start {get;set;} = DateTime.Now;
        [Required]
        public DateTime End {get;set;} = DateTime.Now;
        [Required]
        public string Description {get;set;}
    }
    
}