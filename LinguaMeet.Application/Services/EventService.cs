
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
            if(obj!=null)
            {
                if (obj.EventStatus != EventStatus.Finished)
                {
                    obj.EventStatus = EventStatus.Cancelled;
                    await _rep.UpdateAsync(obj);

                }
                else
                    throw new Exception("Event can not be cancelled");
            }

            throw new Exception("Element introuvable");
        }
       
    }
}
