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
            List<Event> events = _db.Events.Include(x=>x.Type).ToList();
            foreach (Event e in events)
            {
                e.setDate();
            }
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
            return View();
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
        public IActionResult CreateEvent()
        {
            Console.WriteLine("this is called");
            ViewBag.Type = _db.Events.ToList();
            return PartialView("_PartialEvent");
        }
        [HttpPost]
        public async Task<IActionResult> CreateEvent(Event Event)
        {
            ViewBag.Type = _db.Events.ToList();
            _db.Events.Add(Event);
            await _db.SaveChangesAsync();
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
        public IActionResult CreateTimeLine() 
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateTimeLine(EventSchedule EventSchedule)
        {
            _db.EventsSchedules.Add(EventSchedule);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");

        }
        public async Task<IActionResult> EditTimeLine(EventSchedule EventSchedule) 
        { 
            _db.EventsSchedules.Update(EventSchedule);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        public IActionResult Timeline(string EventId)
        {
            return View(new PaginateEventSchedule 
            { 
                Pagination = new Pagination(),
                EventSchedules = _db.EventsSchedules.Where(x => x.Event.Guid == Guid.Parse(EventId)).Include(x=>x.Event).ToList()
            });
        }
    }
}
