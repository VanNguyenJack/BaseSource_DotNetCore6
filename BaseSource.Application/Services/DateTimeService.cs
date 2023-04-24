using BaseSource.Domain.Shared;

namespace BaseSource.Application.Services
{
    public class DateTimeService : IDateTimeService
    {
        public DateTime NowUtc => DateTime.UtcNow;
    }
}
