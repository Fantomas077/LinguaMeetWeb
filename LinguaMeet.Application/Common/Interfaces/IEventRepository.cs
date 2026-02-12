using LinguaMeet.Domain.Entities;

namespace LinguaMeet.Application.Common.Interfaces
{
    public interface IEventRepository
    {
        Task<IEnumerable<Event>> GetUpcomingEventsAsync();
        Task<IEnumerable<Event>> GetAllEvent();
        Task<IEnumerable<Event>> GetEventsByCityAsync(string city);
        Task<Event?> GetEventByIdAsync(int eventId);

        Task AddAsync(Event eventEntity);
        Task UpdateAsync(Event eventEntity);
        Task<List<Event>> GetEventsCreatedByUserAsync(string userId);

    }
}
