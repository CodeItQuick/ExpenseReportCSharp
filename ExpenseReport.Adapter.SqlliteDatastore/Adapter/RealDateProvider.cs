using Application.Services;

namespace Application.Adapter;

public class RealDateProvider : IDateProvider
{
    public RealDateProvider() {
    }

    public DateTimeOffset CurrentDate() {
        return DateTimeOffset.Now;
    }
}