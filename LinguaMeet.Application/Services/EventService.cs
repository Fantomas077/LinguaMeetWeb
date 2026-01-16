
using LinguaMeet.Application.Common.Exceptions;
using LinguaMeet.Application.Common.Interfaces;
using LinguaMeet.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace LinguaMeet.Application.Services
{
    public class EventService
    {
        private readonly IEventRepository _rep;
        public EventService(IEventRepository rep)
        {
            _rep = rep;
            
        }
        public async Task CancelEventAsync(int eventId)
        {
            var obj = await _rep.GetEventByIdAsync(eventId);
            if (obj == null)
            {
                throw new NotFoundException("Event not found");
            }
               

            if (obj.EventStatus == EventStatus.Finished)
            {
                throw new InvalidEventOperationException("Event cannot be cancelled");

            }
               

            obj.EventStatus = EventStatus.Cancelled;
            await _rep.UpdateAsync(obj);

        }
        public async Task FinishEventAsync(int eventId)
        {
            var obj = await _rep.GetEventByIdAsync(eventId);
            if (obj == null)
            {

                throw new NotFoundException("Event not found");

            }

            if (obj.EventStatus == EventStatus.Cancelled || DateTime.Now <= obj.EndDate)
            {
                throw new InvalidEventOperationException("Event cannot be finished");
            }
              
           
                obj.EventStatus = EventStatus.Finished;
                await _rep.UpdateAsync(obj);

        }
        public async Task<IEnumerable<Event>> GetUpcomingEventsByCityAsync(string city)
        {
            var events = await _rep.GetEventsByCityAsync(city);

            
            var upcomingEvents = events.Where(o => o.StartDate > DateTime.Now && o.EventStatus == EventStatus.Published);

            return upcomingEvents;
        }


    }
}
