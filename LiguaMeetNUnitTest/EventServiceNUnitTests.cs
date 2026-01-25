using LinguaMeet.Application.Common.Exceptions;
using LinguaMeet.Application.Common.Interfaces;
using LinguaMeet.Application.Services;
using LinguaMeet.Domain.Entities;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Internal.Execution;
using NUnit.Framework.Legacy;

namespace LiguaMeetNUnitTest
{
    [TestFixture]
    public class EventServiceNUnitTests
    {
        [Test]
        public async Task CreateEventAsync_ValidEvent_CreatesEvent()
        {
            // ARRANGE
            var mockRepo = new Mock<IEventRepository>();
            var service = new EventService(mockRepo.Object);

            var newEvent = new LinguaMeet.Domain.Entities.Event
            {
                Title = "Lingua Meetup",
                Description = "Practice languages together",
                StartDate = DateTime.Now.AddDays(1),
                EndDate = DateTime.Now.AddDays(2),
                Capacity = 20,
                CoverPhotoPath = "image.jpg"
            };

            // ACT
            var result = await service.CreateEventAsync(newEvent);

            // ASSERT
            ClassicAssert.NotNull(result);
            mockRepo.Verify(r => r.AddAsync(newEvent), Times.Once);
        }

        [Test]
        public async Task CreateEventAsync_InvalidCapacity_ThrowsException()
        {
            // ARRANGE
            var mockRepo = new Mock<IEventRepository>();
            var service = new EventService(mockRepo.Object);

            var newEvent = new LinguaMeet.Domain.Entities.Event
            {
                Title = "Bad Event",
                Description = "Invalid capacity",
                StartDate = DateTime.Now.AddDays(1),
                EndDate = DateTime.Now.AddDays(2),
                Capacity = 0,
                CoverPhotoPath = "image.jpg"
            };

            // ACT + ASSERT
            ClassicAssert.ThrowsAsync<InvalidEventOperationException>(
                () => service.CreateEventAsync(newEvent)
            );
        }
        [Test]
        public async Task CreateEventAsync_EmptyTitle_ThrowsException()
        {
            // ARRANGE
            var mockRepo = new Mock<IEventRepository>();
            var service = new EventService(mockRepo.Object);

            var newEvent = new LinguaMeet.Domain.Entities.Event
            {
                Title = "",
                Description = "Invalid capacity",
                StartDate = DateTime.Now.AddDays(1),
                EndDate = DateTime.Now.AddDays(2),
                Capacity = 2,
                CoverPhotoPath = "image.jpg"
            };

            // ACT + ASSERT
            ClassicAssert.ThrowsAsync<InvalidEventOperationException>(
                () => service.CreateEventAsync(newEvent)
            );
        }
        [Test]
        public async Task CreateEventAsync_EmptyCoverPath_ThrowsException()
        {
            // ARRANGE
            var mockRepo = new Mock<IEventRepository>();
            var service = new EventService(mockRepo.Object);

            var newEvent = new LinguaMeet.Domain.Entities.Event
            {
                Title = "Afro",
                Description = "Invalid capacity",
                StartDate = DateTime.Now.AddDays(1),
                EndDate = DateTime.Now.AddDays(2),
                Capacity = 2,
                CoverPhotoPath = ""
            };

            // ACT + ASSERT
            ClassicAssert.ThrowsAsync<InvalidEventOperationException>(
                () => service.CreateEventAsync(newEvent)
            );
        }
        [Test]
        public async Task CreateEventAsync_StartDateAfterEndDate_ThrowsException()
        {
            // ARRANGE
            var mockRepo = new Mock<IEventRepository>();
            var service = new EventService(mockRepo.Object);

            var newEvent = new LinguaMeet.Domain.Entities.Event
            {
                Title = "Afro",
                Description = "Invalid capacity",
                StartDate = DateTime.Now.AddDays(2),
                EndDate = DateTime.Now.AddDays(1),
                Capacity = 2,
                CoverPhotoPath = "image.jpg"
            };

            // ACT + ASSERT
            ClassicAssert.ThrowsAsync<InvalidEventOperationException>(
                () => service.CreateEventAsync(newEvent)
            );
        }
        [Test]
        public async Task FinishEventAsync_EventBeforeEndDate_ThrowsException()
        {
            // ARRANGE
            var eventId = 2;

            var ongoingEvent = new LinguaMeet.Domain.Entities.Event
            {
                Id = eventId,
                EventStatus = EventStatus.Published,
                EndDate = DateTime.Now.AddHours(2) 
            };

            var mockRepo = new Mock<IEventRepository>();
            mockRepo
                .Setup(r => r.GetEventByIdAsync(eventId))
                .ReturnsAsync(ongoingEvent);

            var service = new EventService(mockRepo.Object);

            // ACT & ASSERT
             Assert.ThrowsAsync<InvalidEventOperationException>(
                () => service.FinishEventAsync(eventId)
            );
        }
        [Test]
        public async Task CancelEventAsync_FinishedEvent_ThrowsException()
        {
            //Arrange
            var eventId = 3;

            var thisevent = new LinguaMeet.Domain.Entities.Event
            {
                Id = eventId,
                EventStatus=EventStatus.Finished

            };
            var mockRepo = new Mock<IEventRepository>();
            mockRepo.Setup(r => r.GetEventByIdAsync(eventId)).ReturnsAsync(thisevent);
            var service = new EventService(mockRepo.Object);

            //Act&Assert

            Assert.ThrowsAsync<InvalidEventOperationException> (() => service.CancelEventAsync(eventId));
            
 
        }
        [Test]
        public async Task GetEventByIdAsync_CorrectID_ReturnEvent()
        {
            // Arrange
            var eventId = 3;
            var thisevent = new LinguaMeet.Domain.Entities.Event
            {
                Id = eventId,
                Title = "AfroTrap"
            };
            var mockRepo = new Mock<IEventRepository>();
            mockRepo.Setup(r => r.GetEventByIdAsync(eventId)).ReturnsAsync(thisevent);
            var service = new EventService(mockRepo.Object);

            // Act
            var result = await service.GetEventByIdAsync(eventId);

            // Assert
            ClassicAssert.AreEqual("AfroTrap", result.Title);
        }
        [Test]
        public async Task GetEventByIdAsync_NotFoundId_ThrowException()
        {
            // Arrange
            var eventId = 5;
            var mockRepo = new Mock<IEventRepository>();
            mockRepo.Setup(r => r.GetEventByIdAsync(eventId)).ReturnsAsync((LinguaMeet.Domain.Entities.Event?)null);
            var service = new EventService(mockRepo.Object);

            // Act & Assert
            Assert.ThrowsAsync<NotFoundException>(() => service.GetEventByIdAsync(eventId));
        }


    }
}
