using MediatR;
using Microsoft.EntityFrameworkCore;
using Api;
using Api.Data;
using Api.Infrastructure.Notifications;
using Xunit;

namespace Test
{
    public abstract class BaseTestClass : IClassFixture<ServerTestFixture<Startup>>
    {
        protected readonly ApiDbContext db;
        protected readonly NotificationContext notificationContext;
        protected readonly IMediator mediator;

        public BaseTestClass(ServerTestFixture<Startup> fixture)
        {
            fixture.NotificationContext.Reset();
            fixture.Seeder.Run().GetAwaiter().GetResult();

            db = fixture.DbContext;
            mediator = fixture.Mediator;
            notificationContext = fixture.NotificationContext;

            Assert.True(db.Database.IsInMemory());
        }
    }
}
