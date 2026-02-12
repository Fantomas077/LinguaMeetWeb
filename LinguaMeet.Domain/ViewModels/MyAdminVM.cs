using LinguaMeet.Domain.Entities;

namespace LinguaMeet.Domain.ViewModels
{
    public class MyAdminVM
    {
        public List<Event> ListEvents { get; set; }
        public List<ApplicationUser> ListUsers { get; set; }

        public int TotalUsers { get; set; }
        public int TotalEvents { get; set; }
        public int TotalRegistrations { get; set; }
        public int EventsToday { get; set; }
    }
}
