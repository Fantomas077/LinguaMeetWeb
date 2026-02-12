using LinguaMeet.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LinguaMeet.Application.Common.Interfaces
{
    public interface IAdminRepository
    {
        Task<List<Event>> EventListAsync();
        Task<List<ApplicationUser>> UserListAsync();
        Task<int> GetTotalRegistrationsAsync();

    }
}
