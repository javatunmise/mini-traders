using System;

namespace Shared
{
    public interface ICurrentDate
    {
        DateTime Now();
    }

    public class ServerDateTime : ICurrentDate
    {
        private readonly TimeZoneInfo cstZone;

        public ServerDateTime()
        {
            try
            {
                cstZone = TimeZoneInfo.FindSystemTimeZoneById("W. Central Africa Standard Time");
            }
            catch (TimeZoneNotFoundException)
            {
                cstZone = null;
            }
            catch (InvalidTimeZoneException)
            {
                cstZone = null;
            }
        }
        public DateTime Now()
        {
            DateTime timeUtc = DateTime.UtcNow;
            if (cstZone == null)
                return timeUtc.AddHours(1);

            DateTime cstTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, cstZone);
            return cstTime;
        }
    }
}