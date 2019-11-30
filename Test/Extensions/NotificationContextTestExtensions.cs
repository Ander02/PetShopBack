using FluentAssertions;
using PetShop.Infrastructure.Notifications;
using System.Linq;

namespace Test.Extensions
{
    public static class NotificationContextTestExtensions
    {
        public static void EnsureHasNotifications(this NotificationContext notificationContext)
        {
            notificationContext.HasNotifications.Should().BeTrue();
            notificationContext.Notifications.Any().Should().BeTrue();
        }

        public static void EnsureHasNoNotifications(this NotificationContext notificationContext)
        {
            var errorMessage = !notificationContext.HasNotifications ? string.Empty : $"Has notifcations: {notificationContext.Notifications.Select(d => d.Message).Aggregate((s1, s2) => $"{s1}{s2}")}";
            notificationContext.HasNotifications.Should().BeFalse(errorMessage);
            notificationContext.Notifications.Any().Should().BeFalse(errorMessage);
        }
    }
}
