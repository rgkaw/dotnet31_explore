using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace mvc.Models.Event
{
    public class Event
    {
        [Key]
        public Guid Guid { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string Description { get; set; }
        public EventType Type { get; set; }

        public List<EventSchedule> Schedules { get; set; }

        public DateTime getStartDate() 
        {

            List<EventSchedule> events = Schedules;
            if (events.Count == 0) { return DateTime.Now; }
            events.Sort((x,y)=> x.Start.CompareTo(y.Start));
            return events.First().Start;
        }
        public DateTime getEndDate()
        {
            List<EventSchedule> events = Schedules;
            if (events.Count == 0) { return DateTime.Now; }
            events.Sort((x, y) => x.End.CompareTo(y.End));
            return events.Last().End;
        }

    }

    public class PaginateEvent : IPagination {
        public Pagination Pagination { get; set; }
        public List<Event> Event { get; set; }
    }
}
