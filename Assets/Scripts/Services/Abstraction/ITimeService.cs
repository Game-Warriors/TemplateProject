using System;

namespace Services.Abstraction
{
    public interface ITimeService
    {
        bool IsTimeValid { get; }
        bool HasRemoteTime { get; }
        DateTime DefaultIdleCoin { get; }
        DateTime UtcNow { get; }
        DateTime FestivalIdleCoin { get; }

        TimeSpan RemainTime(string key);
        bool StartTime(string key, TimeSpan offset);
        void RefreshTime(string key, TimeSpan offset);
        bool HasKey(string freeChestTimerKey);
        void UpdateIdelCoin(DateTime newDate);
        void UpdateTime(string key, TimeSpan offset);
        bool EndTime(string key);
        void ForceUpdateRemoteTime();
        void UpdateFestivalIdelCoin(DateTime value);
    }
}