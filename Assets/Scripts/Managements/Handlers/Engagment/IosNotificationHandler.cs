using GameWarriors.EngagementDomain.Abstraction;
using GameWarriors.EngagementDomain.Data;
using System.Threading.Tasks;
#if UNITY_IOS
using Unity.Notifications.iOS;

namespace Managements.Handlers.Engagement
{
    public class IosNotificationHandler : INotificationHandler
    {
        private const string DEFAULT_CATEGORY = "DefaulCategory";

        public IosNotificationHandler()
        {
        }

        public string AddNotification(NotificationData data)
        {
            //var timeTrigger = new iOSNotificationCalendarTrigger()
            //{
            //    TimeInterval = new TimeSpan(0, minutes, seconds),
            //    Repeats = data.RepeatInterval.HasValue
            //};
            string category = data.ChannelId;
            if (string.IsNullOrEmpty(category))
            {
                category = DEFAULT_CATEGORY;
            }
            iOSNotificationCalendarTrigger calendarTrigger = new iOSNotificationCalendarTrigger()
            {
                Repeats = data.RepeatInterval.HasValue,
                Year = data.ShowTime.Year,
                Month = data.ShowTime.Month,
                Day = data.ShowTime.Day,
                Hour = data.ShowTime.Hour,
                Minute = data.ShowTime.Minute
            };
            var notification = new iOSNotification()
            {
                Title = data.Title,
                Body = data.Context,
                Subtitle = string.Empty,
                ShowInForeground = true,
                ForegroundPresentationOption = (PresentationOption.Alert | PresentationOption.Sound),
                CategoryIdentifier = category,
                ThreadIdentifier = "thread1",
                Trigger = calendarTrigger,
            };
            iOSNotificationCenter.ScheduleNotification(notification);
            return notification.Identifier;
        }

        public void AddNotification(string id, NotificationData data)
        {
            string category = data.ChannelId;
            if (string.IsNullOrEmpty(category))
            {
                category = DEFAULT_CATEGORY;
            }
            iOSNotificationCalendarTrigger calendarTrigger = new iOSNotificationCalendarTrigger()
            {
                Repeats = data.RepeatInterval.HasValue,
                Year = data.ShowTime.Year,
                Month = data.ShowTime.Month,
                Day = data.ShowTime.Day,
                Hour = data.ShowTime.Hour,
                Minute = data.ShowTime.Minute
            };
            var notification = new iOSNotification()
            {
                Identifier= id,
                Title = data.Title,
                Body = data.Context,
                Subtitle = string.Empty,
                ShowInForeground = true,
                ForegroundPresentationOption = (PresentationOption.Alert | PresentationOption.Sound),
                CategoryIdentifier = category,
                ThreadIdentifier = "thread1",
                Trigger = calendarTrigger,
            };
            iOSNotificationCenter.ScheduleNotification(notification);
        }

        public void RemoveAllNotification()
        {
            iOSNotificationCenter.RemoveAllDeliveredNotifications();
            iOSNotificationCenter.RemoveAllScheduledNotifications();
        }

        public void RemoveNotification(string id)
        {
            iOSNotificationCenter.RemoveDeliveredNotification(id);
            iOSNotificationCenter.RemoveScheduledNotification(id);
        }

        public async Task Setup()
        {
            AuthorizationOption authorizationOption = AuthorizationOption.Alert | AuthorizationOption.Badge;
            using (AuthorizationRequest req = new AuthorizationRequest(authorizationOption, true))
            {
#if !UNITY_EDITOR
                while (!req.IsFinished)
                {
                    await Task.Delay(100);
                };
#endif
                string res = "\n RequestAuthorization:";
                res += "\n finished: " + req.IsFinished;
                res += "\n granted :  " + req.Granted;
                res += "\n error:  " + req.Error;
                res += "\n deviceToken:  " + req.DeviceToken;
            }
        }

        public void UpdateNotification(string id, NotificationData data)
        {
            
        }
    }
}
#endif