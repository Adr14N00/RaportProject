using RaportAPI.Core.Application.Common.Interfaces;

namespace RaportAPI.Infrastructure.Infrastructure.Services
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
