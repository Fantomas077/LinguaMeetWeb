using System.Diagnostics;
using LinguaMeet.Application.Services;
using LinguaMeet.Models;
using Microsoft.AspNetCore.Mvc;

namespace LinguaMeet.Controllers
{
    public class HomeController : Controller
    {
        private readonly EventService _eventservice;
        public HomeController(EventService eventService)
        {
            _eventservice = eventService;
            
        }
        public async Task<IActionResult> Index()
        {
            var obj= await _eventservice.GetUpcomingEventsAsync();
            return View(obj);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
