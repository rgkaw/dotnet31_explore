using mvc.Models.Event;
using Microsoft.AspNetCore.Mvc;
using mvc.Data;
using System.Linq;
using System.Threading.Tasks;
using mvc.Models;
using System;
using System.Collections.Generic;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using mvc.Models.DTO;
using System.ComponentModel;

namespace mvc.Controllers
{
    public class EventController : Controller
    {
        private readonly ApplicationDbContext _db;
        public EventController(ApplicationDbContext db) 
        {
            _db = db;
        }
        public IActionResult text() {
            return View();
        }
        public IActionResult Index()
        {
            List<Event> events = _db.Events.Include(x=>x.Type).Include(x=> x.Schedules).ToList();
            foreach (Event e in events)
            {
                e.setDate();
            }
            var Types= _db.EventTypes.ToList();
            ViewBag.Types = _db.EventTypes.ToList();
            return View(
                    new PaginateEvent
                    {
                        Pagination = new Pagination(),
                        Event = events
                    }
                );
                
        }
        [HttpGet]
        public IActionResult CreateType() 
        {
            return PartialView("Modal/_CreateType");
        }
        [HttpPost]
        public async Task<IActionResult> CreateType(EventType EventType) 
        {
            if (!ModelState.IsValid) {
                return RedirectToAction("Index");
            }
            _db.EventTypes.Add(EventType);
            await _db.SaveChangesAsync();
            return RedirectToAction("Types");
        }

        [HttpGet]
        public IActionResult EditType() 
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> EditType(EventType EventType)
        {
            _db.EventTypes.Update(EventType);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Types() {
            return View(
                
                new PaginateType 
                { 
                    Pagination = new Pagination(),
                    EventType = _db.EventTypes.ToList()
                });
        }
        [HttpGet]

        [HttpPost]
        public async Task<IActionResult> CreateEvent(EventDto Event)
        {
         
            if(ModelState.IsValid){
                if(!_db.EventTypes.Any(x=> x.Guid.ToString()==Event.Type)){
                    ViewData.Add("", "Invalid TypeId");
                    return RedirectToAction("Index");
                }
                Event e = new Event
                {
                    Name = Event.Name,
                    Description = Event.Description,
                    Type = _db.EventTypes.FirstOrDefault(x=> x.Guid.ToString() == Event.Type)
                };
                Console.WriteLine(Event.Name, Event.Description, Event.Type);
                _db.Events.Add(e);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            Console.WriteLine("Failed Creating Event");
            ViewData.Add("","Invalid Model");
            return RedirectToAction("Index");
            
        }
        [HttpGet]
        public IActionResult EditEvent()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> EditEvent(Event Event)
        {
            _db.Events.Update(Event);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult CreateEventSchedule() 
        {
            Console.WriteLine("CreateEventSchedule PartialView");
            return PartialView("_CreateEventSchedule");
        }
        [HttpPost]
        public async Task<IActionResult> CreateEventSchedule(EventScheduleDto EventSchedule)
        {
            if(!ModelState.IsValid){
                return RedirectToAction("ManageSchedule", new {id=EventSchedule.EventGuid});
            }
            if(EventSchedule.Start<EventSchedule.End){
                ModelState.AddModelError("DateRangeError","Start Date Must Before End Date");
                return RedirectToAction("ManageSchedule", new {id=EventSchedule.EventGuid});
            }
            Console.WriteLine("CreateEventSchedule");
            // foreach(PropertyDescriptor descriptor in TypeDescriptor.GetProperties(EventSchedule))
            // {
            //     string name = descriptor.Name;
            //     object value = descriptor.GetValue(EventSchedule);
            //     Console.WriteLine("{0}={1}", name, value);
            // }
            EventSchedule es = new EventSchedule{
                Event = _db.Events.FirstOrDefault(x=>x.Guid.ToString() == EventSchedule.EventGuid),
                Start = EventSchedule.Start,
                End = EventSchedule.End,
                Description = EventSchedule.Description
            };
            List<EventSchedule> ScheduleList = _db.EventsSchedules.Where(x=> x.Event == es.Event).OrderByDescending(x=>x.End).ToList();
            if(ScheduleList.Count>0){
                if(EventSchedule.Start<ScheduleList.First().End){
                    ModelState.AddModelError("DateRangeError","Start Date Must After Previous Schedule");
                    return RedirectToAction("ManageSchedule", new {id=EventSchedule.EventGuid});
                }
            }
            _db.EventsSchedules.Add(es);
            await _db.SaveChangesAsync();
            return RedirectToAction("ManageSchedule", new {id=EventSchedule.EventGuid});

        }
        public async Task<IActionResult> EditTimeLine(EventSchedule EventSchedule) 
        { 
            _db.EventsSchedules.Update(EventSchedule);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        public IActionResult Timeline(string EventId)
        {
            if(string.IsNullOrEmpty(EventId)){
                return RedirectToAction("Index");
            }
            return View(new PaginateEventSchedule 
            { 
                Pagination = new Pagination(),
                EventSchedules = _db.EventsSchedules.Where(x => x.Event.Guid == Guid.Parse(EventId)).Include(x=>x.Event).ToList()
            });
        }
    
        [HttpPost]
        public async Task<IActionResult> DeleteEvent(Guid Guid){
            if(!_db.Events.Any(x=> x.Guid == Guid)){
                ViewData.Add("","Item is not exists");
                return RedirectToAction("Index");
            }
            Event e = _db.Events.FirstOrDefault(x=> x.Guid==Guid);
            _db.Events.Remove(e);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult DeleteType(Guid Guid){
            if(!_db.Events.Any(x=> x.Guid==Guid)){
                ViewData.Add("", "Item is not exists");
                return RedirectToAction("Index");
            }
            EventType e = _db.EventTypes.FirstOrDefault(x=> x.Guid==Guid);
            _db.EventTypes.Remove(e);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult ManageSchedule(string id){
            Event e = _db.Events.Include(x=> x.Schedules).FirstOrDefault(x=> x.Guid.ToString() == id);
            return View(e);
        }
        public JsonResult isScheduleModelValid(EventScheduleDto e){
            if(!ModelState.IsValid){
                Console.WriteLine("false");
                return Json(ModelState);
            }
            if(e.Start<e.End){
                ModelState.AddModelError("DateRangeError","Start Date Must Before End Date");
                Console.WriteLine("false");
                return Json(ModelState);
            }
            List<EventSchedule> ScheduleList = _db.EventsSchedules.Include(x=>x.Event).Where(x=> x.Event.Guid.ToString() == e.EventGuid).OrderByDescending(x=>x.End).ToList();
            if(ScheduleList.Count>0){
                if(e.Start<ScheduleList.First().End){
                    ModelState.AddModelError("DateRangeError","Start Date Must After Previous Schedule");
                    Console.WriteLine("false");
                    return Json(ModelState);
                }
            }
            Console.WriteLine("true");
            return Json(true);
        }
    }
}
