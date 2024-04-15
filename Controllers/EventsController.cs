using event_project_1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using EventsRoomsViewModel = event_project_1.ViewModels.EventsRoomsViewModel;

namespace event_project_1.Controllers
{
    public class EventsController : Controller
    {
        private readonly ApplicationDbContext _context;
        public EventsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult AddRoom()
        {
            //var events = _context.Events.ToList();
            return View();
        }

        [Authorize]
        public IActionResult FullCalendar1()
        {
            // var events = _context.Events.ToList();
            // return View(events);
            var events = _context.Events.ToList();
            var rooms = _context.Rooms.ToList();
            var viewModelList = new List<EventsRoomsViewModel>();

            var viewModel = new EventsRoomsViewModel
            {
                Eventss = events,
                Roomss = rooms
            };
            viewModelList.Add(viewModel);
            // var x = viewModel.ToList(); 
            return View(viewModelList);
        }

        public IActionResult Rooms()
        {
            var events = _context.Events.ToList();
            var rooms = _context.Rooms.ToList();
            var viewModelList = new List<EventsRoomsViewModel>();

            var viewModel = new EventsRoomsViewModel
            {
                Eventss = events,
                Roomss = rooms
            };
            viewModelList.Add(viewModel);
           // var x = viewModel.ToList(); 
            return View(viewModelList);
        }

        [HttpPost]
        public async Task<IActionResult> AddRoom1([Bind("room_name")] Room r)
        {
            //var x = await _context.Events.ToListAsync();
            var username = User.Identity.Name;
            var user = _context.Users.FirstOrDefault(u => u.Username == username);
            if (user != null && ModelState.IsValid)
            {
                var room = new Room
                {
                   room_name = r.room_name,
                   user_id = user.Id
                };

                _context.Rooms.Add(room);
                await _context.SaveChangesAsync();

                return RedirectToAction("Rooms");

            }

            return View("AddRoom");
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> FullCalendar23([Bind("Id,Title,StartTime,EndTime,Description,RoomId,Recurrence,RecurrenceInterval,RecurrenceFrequency,RecurrenceEndType,EndDate,Occurrences")] Event viewModel)
        {
            var username = User.Identity.Name;

            // Find the user based on the username
            var user = _context.Users.FirstOrDefault(u => u.Username == username);
           // @event.RoomId = user?.Role == "Admin" ? @event.RoomId : null;
            if (ModelState.IsValid)
            {
                var @event = new Event
                {

                    Title = viewModel.Title,
                    StartTime = viewModel.StartTime,
                    EndTime = viewModel.EndTime,
                    Description = viewModel.Description,
                    Recurrence = viewModel.Recurrence,
                    RecurrenceInterval = 1,
                    RecurrenceFrequency = viewModel.RecurrenceFrequency,
                    RecurrenceEndType = viewModel.RecurrenceEndType,
                    EndDate = viewModel.EndDate!=null?viewModel.EndDate: null,
                    Occurrences = viewModel.Occurrences!=null?viewModel.Occurrences:null,
                    UserId = user.Id,
                    RoomId = user.Role == "Admin" ? viewModel.RoomId : null,
                };
                _context.Events.Add(@event);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(FullCalendar1));
            }
            return View(nameof(FullCalendar1), viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEvent([Bind("Title,StartTime,EndTime,Description,RoomId,Recurrence")] Event viewModel)
        {
            //
            var username = User.Identity.Name;

            // Find the user based on the username
            var user =  _context.Users.FirstOrDefault(u => u.Username == username);
            var x = viewModel.RoomId;
            if (ModelState.IsValid)
            {
               var @event = new Event
                {
                    
                    Title = viewModel.Title,
                    StartTime = viewModel.StartTime,
                    EndTime = viewModel.EndTime,
                    Description = viewModel.Description,
                    Recurrence = viewModel.Recurrence,
                    RecurrenceInterval = 1,
                    RecurrenceFrequency = "daily",
                    RecurrenceEndType = "occurrences",
                    EndDate = null,
                    Occurrences = null,
                    UserId = user.Id,
                    RoomId = user.Role == "Admin"?viewModel.RoomId : null,
               };

                _context.Events.Add(@event);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(FullCalendar1));
            }

            return View(nameof(FullCalendar1), viewModel);
        }



        [HttpPost]
        public async Task<IActionResult>  UpdateEvent(Event updatedEvent, int eventId)
        {
            var existingEvent = _context.Events.Find(eventId);
            var username = User.Identity.Name;

            // Find the user based on the username
            var user = _context.Users.FirstOrDefault(u => u.Username == username);
            if (existingEvent != null && user.AdminOverride)
            {
                // Update the existing event with the new details
                existingEvent.Title = updatedEvent.Title;
                existingEvent.StartTime = updatedEvent.StartTime;
                existingEvent.EndTime = updatedEvent.EndTime;
                existingEvent.Description = updatedEvent.Description;
                existingEvent.Recurrence = updatedEvent.Recurrence;
                existingEvent.RoomId = user.Role == "Admin" ? updatedEvent.RoomId : null;

                if (updatedEvent.Recurrence == "custom")
                {
                    existingEvent.RecurrenceInterval = updatedEvent.RecurrenceInterval;
                    existingEvent.RecurrenceFrequency = updatedEvent.RecurrenceFrequency;
                    existingEvent.RecurrenceEndType = updatedEvent.RecurrenceEndType;
                    existingEvent.Occurrences = updatedEvent.Occurrences > 0 ? updatedEvent.Occurrences : null;
                    existingEvent.EndDate = updatedEvent.EndDate >= updatedEvent.StartTime ? updatedEvent.EndDate : null;


                }
                // Save the changes to the database
                //_context.SaveChanges();
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(FullCalendar1));

        }

        [HttpPost]
        public ActionResult DeleteEvent(int eventId)
        {
            var existingEvent = _context.Events.Find(eventId);
            var username = User.Identity.Name;

            // Find the user based on the username
            var user = _context.Users.FirstOrDefault(u => u.Username == username);

               // user.Role == "Admin"
            if (existingEvent != null && (user.AdminOverride))
            {
                _context.Events.Remove(existingEvent);
                _context.SaveChanges();
            }

            // Return success or any relevant response
            return RedirectToAction(nameof(FullCalendar1));
        }

    }
}
