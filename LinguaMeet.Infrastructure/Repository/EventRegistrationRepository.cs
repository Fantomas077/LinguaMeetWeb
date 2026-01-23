using LinguaMeet.Application.Common.Interfaces;
using LinguaMeet.Domain.Entities;
using LinguaMeet.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace LinguaMeet.Infrastructure.Repository
{
    public class EventRegistrationRepository : IEventRegistrationRepository
    {
        private readonly ApplicationDbContext _db;
        public EventRegistrationRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async  Task AddAsync(EventRegistration eventRegistration)
        {
            await _db.EventRegistrations.AddAsync(eventRegistration);
            await _db.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(string userId, int eventId)
        {
            return await _db.EventRegistrations
                .AnyAsync(x => x.UserId == userId && x.EventId == eventId);
        }

        public async Task<bool> IsEventFull(int eventId)
        {
            var capacity = await _db.Events
                .Where(e => e.Id == eventId)
                .Select(e => e.Capacity)
                .FirstOrDefaultAsync();

            if (capacity == 0)
                return false;

            var registrationsCount = await _db.EventRegistrations
                .CountAsync(r => r.EventId == eventId
                              && r.Status == RegistrationStatus.Registered);

            return registrationsCount >= capacity;
        }
        public async Task CancelRegistrationAsync(string userId, int eventId)
        {
            var registration = await _db.EventRegistrations
                .FirstOrDefaultAsync(r => r.UserId == userId && r.EventId == eventId && r.Status == RegistrationStatus.Registered);

            if (registration != null)
            {
                registration.Status = RegistrationStatus.Cancelled;
                 
                await _db.SaveChangesAsync();
            }
        }

    }
}
