using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace mvc.Models.Event
{
    public class EventType
    {
        [Key]
        public Guid Guid { get; set; } = Guid.NewGuid();
        public string Name { get; set; } 

        public List<Event> Events { get; set; }
    }

    public class PaginateType : IPagination
    {
        public Pagination Pagination { get; set; }
        public List<EventType> EventType { get; set; }
    }

}
