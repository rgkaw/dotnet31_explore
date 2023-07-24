using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace mvc.Models.Event
{
    public class EventSchedule
    {
        [Key]
        public Guid Guid { get; set; } = Guid.NewGuid();
        public Event Event { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
    public class PaginateEventSchedule : IPagination
    {
        public Pagination Pagination { get; set; }
        public List<EventSchedule> EventSchedule { get; set; }
    }
}
