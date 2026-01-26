using LinguaMeet.Application.Common.Interfaces;
using LinguaMeet.Application.Services;
using LinguaMeet.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace LiguaMeetNUnitTest
{
    [TestFixture]
    public class EventRegistrationNUnitTest
    {
        private static Mock<UserManager<ApplicationUser>> MockUserManager(ApplicationUser user)
        {
            var store = new Mock<IUserStore<ApplicationUser>>();

            var userManager = new Mock<UserManager<ApplicationUser>>(
                store.Object,
                null, null, null, null, null, null, null, null
            );

            userManager
                .Setup(u => u.FindByIdAsync(user.Id))
                .ReturnsAsync(user);

            return userManager;
        }

        [Test]
        public async Task RegisterEventAsync_ValidData_RegistersUser()
        {

            // =====================
            // ARRANGE
            // =====================

            var userId = "user-1";
            var eventId = 1;

            var ev = new Event
            {
                Id = eventId,
                EventStatus = EventStatus.Published,
                Capacity = 10
            };

            var user = new ApplicationUser
            {
                Id = userId
            };

            var mockEventRepo = new Mock<IEventRepository>();
            var mockRegRepo = new Mock<IEventRegistrationRepository>();
            var mockUserManager = MockUserManager(user);

            mockEventRepo
                .Setup(r => r.GetEventByIdAsync(eventId))
                .ReturnsAsync(ev);

            mockRegRepo
                .Setup(r => r.ExistsAsync(userId, eventId))
                .ReturnsAsync(false);

            mockRegRepo
                .Setup(r => r.IsEventFull(eventId))
                .ReturnsAsync(false);

            var service = new EventRegistrationService(
                mockEventRepo.Object,
                mockUserManager.Object,
                mockRegRepo.Object
            );

            // =====================
            // ACT
            // =====================

            await service.RegisterEventAsync(eventId, userId);

            // =====================
            // ASSERT
            // =====================

            mockRegRepo.Verify(
                r => r.AddAsync(It.Is<EventRegistration>(
                    e => e.UserId == userId && e.EventId == eventId
                )),
                Times.Once
            );
        }


    }
}
