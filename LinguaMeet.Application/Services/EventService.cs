
using LinguaMeet.Application.Common.Exceptions;
using LinguaMeet.Application.Common.Interfaces;
using LinguaMeet.Domain.Entities;
using Microsoft.EntityFrameworkCore;
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

        public async Task<Event?> GetEventByIdAsync(int eventId)
        {
            var obj= await _rep.GetEventByIdAsync(eventId);
            if(obj==null)
            {
                throw new NotFoundException("Event No found");

            }
            return obj;
        }
        public async Task<IEnumerable<Event>> GetUpcomingEventsByCityAsync(string city)
        {
            var events = await _rep.GetEventsByCityAsync(city);

            
            var upcomingEvents = events.Where(o => o.StartDate > DateTime.Now && o.EventStatus == EventStatus.Published);

            return upcomingEvents;
        }
        public async Task<IEnumerable<Event>> GetUpcomingEventsAsync()
        {
            var events = await _rep.GetUpcomingEventsAsync();
            var upcomingEvents = events.Where(o => o.StartDate > DateTime.Now && o.EventStatus == EventStatus.Published);
            return upcomingEvents;
        }
        public async Task<IEnumerable<Event>> GetAllEvent()
        {
            var obj = await _rep.GetAllEvent();
            return obj;
        }

        public async Task<Event> CreateEventAsync(Event newEvent)
        {
            
            if (string.IsNullOrWhiteSpace(newEvent.Title))
                throw new InvalidEventOperationException("Title is required");

            if (newEvent.StartDate >= newEvent.EndDate)
                throw new InvalidEventOperationException("Start date must be before end date");

            if (newEvent.Capacity <= 0)
                throw new InvalidEventOperationException("Capacity must be greater than 0");
            if(string.IsNullOrEmpty(newEvent.CoverPhotoPath))
                throw new InvalidEventOperationException("Cover photo is required");



            newEvent.EventStatus = EventStatus.Published;

            await _rep.AddAsync(newEvent);

            return newEvent;
        }
        public async Task<Event> EditEventAsync(Event newEvent)
        {
            var obj = await _rep.GetEventByIdAsync(newEvent.Id);
            if (obj == null)
            {
                throw new NotFoundException("Event not found");
            }
            if(obj.EventStatus==EventStatus.Cancelled || obj.EventStatus==EventStatus.Finished)
            {
                
                    throw new InvalidEventOperationException("can not  be  modified");
            }
            if (string.IsNullOrWhiteSpace(obj.CoverPhotoPath) && string.IsNullOrWhiteSpace(newEvent.CoverPhotoPath))
            {
                throw new InvalidEventOperationException("Cover photo is required");
            }
            if (!string.IsNullOrWhiteSpace(newEvent.CoverPhotoPath))
            {
                obj.CoverPhotoPath = newEvent.CoverPhotoPath;
            }


            if (string.IsNullOrWhiteSpace(newEvent.Title))
                throw new InvalidEventOperationException("Title is required");

            if (newEvent.StartDate >= newEvent.EndDate)
                throw new InvalidEventOperationException("Start date must be before end date");

            if (newEvent.Capacity <= 0)
                throw new InvalidEventOperationException("Capacity must be greater than 0");

            obj.Capacity = newEvent.Capacity;
            obj.StartDate = newEvent.StartDate;
            obj.EndDate = newEvent.EndDate;
            obj.City = newEvent.City;
            obj.Adresse = newEvent.Adresse;
            obj.Description = newEvent.Description;
           

            obj.EventStatus = EventStatus.Published;

            await _rep.UpdateAsync(obj);

            return obj;
        }
        public async Task<List<Event>> GetEventsCreatedByUserAsync(string userId)
        {
            return await _rep.GetEventsCreatedByUserAsync(userId);
        }


    }
}
