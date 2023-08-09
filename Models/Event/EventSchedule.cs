using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using mvc.Data.Validator;

namespace mvc.Models.Event
{
    public class EventSchedule
    {

        [Key]
        public Guid Guid { get; set; } = Guid.NewGuid();
        public Event Event { get; set; }
        public DateTime Start { get; set; } = DateTime.Now;
        public DateTime End { get; set; } = DateTime.Now;
        public bool IsCompleted { get; set; } = false;
        public bool IsDoing { get; set; } = false;
        public string Description { get; set; } = string.Empty;
    }
    public class PaginateEventSchedule : IPagination
    {
        public Pagination Pagination { get; set; }
        public List<EventSchedule> EventSchedules { get; set; }
    }
}
