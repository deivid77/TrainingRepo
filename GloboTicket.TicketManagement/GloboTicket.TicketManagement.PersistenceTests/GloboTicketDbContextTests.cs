using NUnit.Framework;
using GloboTicket.TicketManagement.Application.Contracts;
using GloboTicket.TicketManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Moq;
using GloboTicket.TicketManagement.Persistence;
using System;
using System.Threading.Tasks;

namespace GloboTicket.TicketManagement.PersistenceTests
{
    public class GloboTicketDbContextTests
    {
        private GloboTicketDbContext _globoTicketDbContext;
        private Mock<ILoggedInUserService> _loggedInUserServiceMock;
        private string _loggedInUserId;

        [SetUp]
        public void Setup()
        {
            var dbContextOptions = new DbContextOptionsBuilder<GloboTicketDbContext>()
                                        .UseInMemoryDatabase(Guid.NewGuid().ToString())
                                        .Options;

            _loggedInUserId = "00000000-0000-0000-0000-000000000000";
            _loggedInUserServiceMock = new Mock<ILoggedInUserService>();
            _loggedInUserServiceMock.Setup(m => m.UserId).Returns(_loggedInUserId);

            _globoTicketDbContext = new GloboTicketDbContext(dbContextOptions, _loggedInUserServiceMock.Object);
        }

        [Test]
        public async Task Save_SetCreatedByProperty()
        {
            //Arrange
            var ev = new Event() { EventId = Guid.NewGuid(), Name = "Test event" };

            //Act
            _globoTicketDbContext.Events.Add(ev);
            await _globoTicketDbContext.SaveChangesAsync();

            //Assert
            Assert.AreEqual(ev.CreatedBy, _loggedInUserId);
        }
    }
}