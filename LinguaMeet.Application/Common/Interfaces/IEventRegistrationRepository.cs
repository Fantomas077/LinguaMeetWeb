using LinguaMeet.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LinguaMeet.Application.Common.Interfaces
{
    public interface IEventRegistrationRepository
    {
        Task AddAsync(EventRegistration eventRegistration);
        Task<bool> ExistsAsync(string userId, int eventId);
        Task<bool> IsEventFull( int eventId);
    }
}
