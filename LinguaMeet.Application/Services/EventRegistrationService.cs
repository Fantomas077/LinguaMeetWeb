using LinguaMeet.Application.Common.Exceptions;
using LinguaMeet.Application.Common.Interfaces;
using LinguaMeet.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace LinguaMeet.Application.Services
{
    public  class EventRegistrationService
    {
        private readonly IEventRepository _rep;
        private readonly UserManager<ApplicationUser> _usermanager;
        private readonly IEventRegistrationRepository _repRegis;

        public EventRegistrationService(IEventRepository rep, UserManager<ApplicationUser> usermanager, IEventRegistrationRepository repRegis   )
        {
            _rep = rep;
            _usermanager = usermanager;
            _repRegis = repRegis;
        }

        public async Task RegisterEventAsync(int eventId,string userID)
        {

            var obj = await _rep.GetEventByIdAsync(eventId);
            if (obj == null)
            {
                throw new NotFoundException("Event not found");
            }
            var userexist = await _usermanager.FindByIdAsync(userID);
            if(userexist==null)
            {
                throw new NotFoundException("User not found");
            }
            if(obj.EventStatus==EventStatus.Cancelled)
            {
                throw new InvalidEventOperationException("The Event is canceled can not register");
            }
            if (await _repRegis.ExistsAsync(userID, eventId))
            {
                throw new InvalidEventOperationException("User already registered");
            }
            if (await _repRegis.IsEventFull(eventId))
            {
                throw new InvalidEventOperationException("Event is full");
            }
            var isFull = await _repRegis.IsEventFull(eventId);

            var newUserEvent = new EventRegistration
            {
                RegisteredAt = DateTime.Now,
                EventId = eventId,
                UserId = userID,
                 Status = isFull ? RegistrationStatus.Attended : RegistrationStatus.Registered
               

            };
           

            await _repRegis.AddAsync(newUserEvent);
        }
        public async Task Cancel(int eventId, string userID)
        {
            await _repRegis.CancelRegistrationAsync(userID, eventId);
        }

        public async Task<bool> IsRegisteredAsync(int eventId, string userId)
        {
            return await _repRegis.ExistsAsync(userId, eventId);
        }
        public async Task<int> GetRegisteredCountAsync(int eventId)
        {
            var evt = await _rep.GetEventByIdAsync(eventId);
            if (evt == null)
                throw new NotFoundException("Event not found");

            int count = 0;
            if (evt.Registrations != null)
            {
                count = evt.Registrations
                           .Count(r => r.Status == RegistrationStatus.Registered);
            }

            return count;
        }
        public async Task<List<Event>> GetEventsJoinedByUserAsync(string userId)
        {
            return await _repRegis.GetEventsJoinedByUserAsync(userId);
        }

    }
}
