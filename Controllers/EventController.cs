using mvc.Models.Event;
using Microsoft.AspNetCore.Mvc;
using mvc.Data;
using System.Linq;
using System.Threading.Tasks;
using mvc.Models;

namespace mvc.Controllers
{
    public class EventController : Controller
    {
        private readonly ApplicationDbContext _db;
        public EventController(ApplicationDbContext db) 
        {
            _db = db;
        }
        public IActionResult Index()
        {

            return View(
                new PaginateEvent { 
                    Pagination = new Pagination(),
                    Event = _db.Events.ToList()
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
            return RedirectToAction("Index");
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
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateEvent(Event Event)
        {
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


        public IActionResult Timeline(Event Event)
        {
            return View(new PaginateEventSchedule 
            { 
                Pagination = new Pagination(),
                EventSchedule = _db.EventsSchedules.Where(x => x.Event == Event).ToList()
            });
        }
    }
}
