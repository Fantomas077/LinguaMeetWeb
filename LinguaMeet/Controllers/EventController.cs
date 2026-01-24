using LinguaMeet.Application.Common.Interfaces;
using LinguaMeet.Application.Services;
using LinguaMeet.Domain.Entities;
using LinguaMeet.Domain.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LinguaMeet.Controllers
{
    public class EventController : Controller
    {
        private readonly EventService _evService;
        private readonly EventRegistrationService _evRegistration;
        private readonly UserManager<ApplicationUser> _usermanager;


        public EventController(EventService ev, EventRegistrationService evRegistration, UserManager<ApplicationUser> usermanager)
        {
            _evService = ev;
            _evRegistration = evRegistration;
            _usermanager = usermanager;
        }
        [HttpPost]
        public async Task<IActionResult> Index()
        {
            var obj = await _evService.GetUpcomingEventsAsync();
            return View(obj);
           
        }
        [HttpPost]

        public async Task<IActionResult> Details(int id)
        {
            var obj = await _evService.GetEventByIdAsync(id);
            return View(obj);
            
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Register(int eventId)
        {
            var userId = _usermanager.GetUserId(User);

            try
            {
                await _evRegistration.Register(eventId, userId);
                TempData["Success"] = "You are registered successfully";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction("Details", new { id = eventId });
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CancelRegistration(int eventId)
        {
            var userId = _usermanager.GetUserId(User);

            await _evRegistration.Cancel(eventId, userId);

            TempData["Success"] = "Registration cancelled";
            return RedirectToAction("Details", new { id = eventId });
        }

        [HttpGet]
        public async Task<IActionResult>Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EventVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            //  Upload image
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/events");
            Directory.CreateDirectory(uploadsFolder);

            var fileName = Guid.NewGuid() + Path.GetExtension(model.CoverPhoto.FileName);
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await model.CoverPhoto.CopyToAsync(stream);
            }

            // Mapper to Event
            var newEvent = new Event
            {
                Title = model.Title,
                Description = model.Description,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                Capacity = model.Capacity,
                City = model.City,
                Adresse = model.Adresse,
                EventType=model.EventType,
                OnlineLink=model.OnlineLink,
                CoverPhotoPath = "/images/events/" + fileName
            };

            // Call service
            try
            {
                await _evService.CreateEventAsync(newEvent);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }


    }
}
