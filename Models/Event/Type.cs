using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace mvc.Models.Event
{
    public class EventType
    {
        public EventType(){
            
        }
        public EventType(string id){
            Guid = Guid.Parse(id);
        }
        [Key]
        public Guid Guid { get; set; } = Guid.NewGuid();
        [Required]
        public string Name { get; set; } = string.Empty;

        public List<Event> Events { get; set; }
    }

    public class PaginateType : IPagination
    {
        public Pagination Pagination { get; set; }
        public List<EventType> EventType { get; set; }
    }

}
