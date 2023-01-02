using GameWarriors.EngagementDomain.Abstraction;
using GameWarriors.EngagementDomain.Data;

namespace Managements.Handlers.Engagement
{
    using System.Threading.Tasks;
#if UNITY_ANDROID
    using Unity.Notifications.Android;

    public class AndroidNotificationHandler : INotificationHandler
    {
        private const string DEFAULT_CHANNEL_ID = "default_channel";

        public AndroidNotificationHandler()
        {
            var channel = new AndroidNotificationChannel()
            {
                Id = DEFAULT_CHANNEL_ID,
                Name = "Default Channel",
                Importance = Importance.Default,
                Description = "Generic notifications",
            };
            AndroidNotificationCenter.RegisterNotificationChannel(channel);
        }

        public string AddNotification(NotificationData data)
        {
            var notification = new AndroidNotification
            {
                Title = data.Title,
                Text = data.Context,
                FireTime = data.ShowTime,
                RepeatInterval = data.RepeatInterval
            };

            string channelId = data.ChannelId;
            if (string.IsNullOrEmpty(channelId))
                channelId = DEFAULT_CHANNEL_ID;

            int id = AndroidNotificationCenter.SendNotification(notification, channelId);
            return System.Convert.ToString(id);
        }

        public void AddNotification(string notificationId, NotificationData data)
        {
            var notification = new AndroidNotification
            {
                Title = data.Title,
                Text = data.Context,
                FireTime = data.ShowTime,
                RepeatInterval = data.RepeatInterval
            };

            string channelId = data.ChannelId;
            if (string.IsNullOrEmpty(channelId))
                channelId = DEFAULT_CHANNEL_ID;
            if (int.TryParse(notificationId, out var id))
                AndroidNotificationCenter.SendNotificationWithExplicitID(notification, channelId, id);
        }

        public void RemoveAllNotification()
        {
            AndroidNotificationCenter.CancelAllNotifications();
        }

        public void RemoveNotification(string notificationId)
        {
            if (int.TryParse(notificationId, out var id))
                AndroidNotificationCenter.CancelNotification(id);
        }

        public Task Setup()
        {
            return Task.CompletedTask;
        }

        public void UpdateNotification(string notificationId, NotificationData data)
        {
            var notification = new AndroidNotification
            {
                Title = data.Title,
                Text = data.Context,
                FireTime = data.ShowTime,
                RepeatInterval = data.RepeatInterval
            };

            string channelId = data.ChannelId;
            if (string.IsNullOrEmpty(channelId))
                channelId = DEFAULT_CHANNEL_ID;
            if (int.TryParse(notificationId, out var id))
                AndroidNotificationCenter.UpdateScheduledNotification(id, notification, channelId);
        }
    }
#endif
}