using LinguaMeet.Application.Common.Interfaces;
using LinguaMeet.Domain.Entities;
using LinguaMeet.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace LinguaMeet.Infrastructure.Repository
{
    public class AdminRepository : IAdminRepository
    {
        private readonly ApplicationDbContext _db;
        public AdminRepository(ApplicationDbContext db)
        {
            _db = db;
            
        }
        public async Task<List<Event>> EventListAsync()
        {
            return await _db.Events
                .Include(e => e.User)
                .Include(e => e.Registrations)
                .ToListAsync();
        }


        public async Task<List<ApplicationUser>> UserListAsync()
        {
            return await _db.Users.ToListAsync();
        }
        public async Task<int> GetTotalRegistrationsAsync()
        {
            return await _db.EventRegistrations.CountAsync();
        }

    }
}
