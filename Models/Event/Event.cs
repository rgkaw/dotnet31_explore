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
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public EventType Type { get; set; }

        public DateTime StartDate { get; set; } = DateTime.Now;
        public DateTime EndDate { get; set; } = DateTime.Now;
        public List<EventSchedule> Schedules { get; set; }

        public void setDate() 
        {

            List<EventSchedule> events = Schedules;
            if (Schedules == null ||events.Count == 0 ) { StartDate= DateTime.Now; EndDate = DateTime.Now; return; }
            Console.WriteLine(events==null);
            events.Sort((x,y)=> x.Start.CompareTo(y.Start));
            StartDate = events.First().Start;
            events.Sort((x, y) => x.End.CompareTo(y.End));
            EndDate = events.Last().End;
            
        }

    }

    public class PaginateEvent : IPagination {
        public Pagination Pagination { get; set; }
        public List<Event> Event { get; set; }
    }
}
