using LinguaMeet.Application.Services;
using LinguaMeet.Domain.Entities;
using LinguaMeet.Domain.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LinguaMeet.Controllers
{
   
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly AdminService _adminService;
        private readonly UserManager<ApplicationUser> _usermanager;

        public AdminController(AdminService adminService, UserManager<ApplicationUser> usermanager)
        {
            _adminService = adminService;
            _usermanager = usermanager;
        }

        public async Task<IActionResult> Index()
        {
            var events = await _adminService.GetAllEventAsync();
            var users = await _adminService.GetAllUserAsync();

            var vm = new MyAdminVM
            {
                ListEvents = events,
                ListUsers = users,
                TotalUsers = users.Count,
                TotalEvents = events.Count,
                TotalRegistrations = await _adminService.GetTotalRegistrationsAsync(),
                EventsToday = events.Count(e => e.StartDate.Date == DateTime.Today)
            };

            return View(vm);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _usermanager.FindByIdAsync(id);
            if (user != null)
                await _usermanager.DeleteAsync(user);

            return RedirectToAction("Index");
        }

    }

}
