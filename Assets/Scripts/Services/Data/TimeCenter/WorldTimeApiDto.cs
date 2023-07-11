using System;

namespace Services.Data.TimeCenter
{
    [Serializable]
    public class WorldTimeApiDto
    {
        public string abbreviation;
        public string client_ip;
        public string datetime;
        public int day_of_week;
        public int day_of_year;
        public bool dst;
        public string dst_from;
        public int dst_offset;
        public string dst_until;
        public int raw_offset;
        public string timezone;
        public long unixtime;
        public string utc_datetime;
        public string utc_offset;
        public int week_number;
    }
}