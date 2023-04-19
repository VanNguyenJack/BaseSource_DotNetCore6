using System;

namespace BaseSource.Domain.Shared
{
    public interface IDateTimeService
    {
        DateTime NowUtc { get; }
    }
}
