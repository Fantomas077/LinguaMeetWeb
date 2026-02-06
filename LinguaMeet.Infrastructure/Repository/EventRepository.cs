using LinguaMeet.Application.Common.Interfaces;
using LinguaMeet.Domain.Entities;
using LinguaMeet.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace LinguaMeet.Infrastructure.Repository
{
    public class EventRepository : IEventRepository
    {
        private readonly ApplicationDbContext _db;
        public EventRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async   Task AddAsync(Event eventEntity)
        {
            _db.Events.Add(eventEntity);
            await _db.SaveChangesAsync();
        }

        public async  Task<Event?> GetEventByIdAsync(int eventId)
        {
          return   await  _db.Events.Include(r=>r.Registrations).FirstOrDefaultAsync(x => x.Id == eventId);
        }

        public async  Task<IEnumerable<Event>> GetEventsByCityAsync(string city)
        {
           return await  _db.Events.Where(x => x.City == city && x.EventStatus==EventStatus.Published).ToListAsync();
           
            
        }

        public async Task<IEnumerable<Event>> GetUpcomingEventsAsync()
        {
           return await   _db.Events.Where(x => x.StartDate > DateTime.Now && x.EventStatus==EventStatus.Published).ToListAsync();
        }

        public async  Task UpdateAsync(Event eventEntity)
        {
            _db.Events.Update(eventEntity);
            await _db.SaveChangesAsync();
            
        }
    }
}
