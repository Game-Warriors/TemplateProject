using System;

namespace Services.Data.TimeCenter
{
    [Serializable]
    public class WorldClockApiDto
    {
        public string currentDateTime;
        public string utcOffset;
        public bool isDayLightSavingsTime;
        public string dayOfTheWeek;
        public string timeZoneName;
        public long currentFileTime;
        public string ordinalDate;
        public string serviceResponse;
    }
}