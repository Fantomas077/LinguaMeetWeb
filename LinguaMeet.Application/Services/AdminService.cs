using LinguaMeet.Application.Common.Interfaces;
using LinguaMeet.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LinguaMeet.Application.Services
{
    public  class AdminService
    {
        private readonly IAdminRepository _rep;
        public AdminService(IAdminRepository rep)
        {
            _rep = rep;

        }

        public async Task<List<Event>> GetAllEventAsync()
        {
            var obj= await _rep.EventListAsync();
            return obj;
        }
        public async Task<List<ApplicationUser>> GetAllUserAsync()
        {
            var obj = await _rep.UserListAsync();
            return obj;
        }
        public async Task<int> GetTotalRegistrationsAsync()
        {
            return await _rep.GetTotalRegistrationsAsync();
        }

    }
}
