using LinguaMeet.Application.Common.Interfaces;
using LinguaMeet.Application.Services;
using LinguaMeet.Domain.Entities;
using LinguaMeet.Domain.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;

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
        
        public async Task<IActionResult> Index()
        {
            var obj = await _evService.GetUpcomingEventsAsync();
            return View(obj);
           
        }


        public async Task<IActionResult> Details(int id)
        {
            
            var ev = await _evService.GetEventByIdAsync(id);
            if (ev == null)
                return NotFound();

            
            var userId = _usermanager.GetUserId(User);

            
            bool isRegistered = userId != null && await _evRegistration.IsRegisteredAsync(id, userId);
            int registeredCount = await _evRegistration.GetRegisteredCountAsync(id);

            
            var vm = new EventDetailsVM
            {
                Event = ev,
                RegisteredCount = registeredCount,
                IsRegistered = isRegistered
            };

            return View(vm);
        }



        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Register(int eventId)
        {
            var userId = _usermanager.GetUserId(User);

            try
            {
                await _evRegistration.RegisterEventAsync(eventId, userId);
                TempData["Success"] = "You are registered successfully";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            
            return RedirectToAction("Details", new { id = eventId });
        }

        [HttpPost]
     
        public async Task<IActionResult> CancelRegistration(int eventId)
        {
            var userId = _usermanager.GetUserId(User);

            await _evRegistration.Cancel(eventId, userId);

            TempData["Success"] = "Registration cancelled";
            return RedirectToAction("Details", new { id = eventId });
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult>Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EventVM model)
        {
           
            if (!ModelState.IsValid)
                return View(model);

 

            // Upload image
            if (model.CoverPhoto != null)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
                var ext = Path.GetExtension(model.CoverPhoto.FileName).ToLower();

                if (!allowedExtensions.Contains(ext))
                {
                    ModelState.AddModelError("CoverPhoto", "Format d'image non supporté.");
                    return View(model);
                }

                if (model.CoverPhoto.Length > 2 * 1024 * 1024)
                {
                    ModelState.AddModelError("CoverPhoto", "L'image ne doit pas dépasser 2MB.");
                    return View(model);
                }

                string uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "events");

                if (!Directory.Exists(uploadFolder))
                    Directory.CreateDirectory(uploadFolder);

                model.CoverPhotoPath = Guid.NewGuid().ToString() + ext;
                string imagePath = Path.Combine(uploadFolder, model.CoverPhotoPath);

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await model.CoverPhoto.CopyToAsync(stream);
                }
            }

            // Mapping vers Event
            var newEvent = new Event
            {
                Title = model.Title,
                Description = model.Description,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                Capacity = model.Capacity,
                City = model.City,
                Adresse = model.Adresse,
                EventType = model.EventType,
                EventStatus = model.EventStatus,
                OnlineLink = model.OnlineLink,
                CoverPhotoPath = model.CoverPhotoPath != null ? "/images/events/" + model.CoverPhotoPath : null
            };

            try
            {
                await _evService.CreateEventAsync(newEvent);
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }



    }
}
