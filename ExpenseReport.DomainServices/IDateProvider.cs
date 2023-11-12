namespace Application.Services;

public interface IDateProvider
{

    public DateTimeOffset CurrentDate();
}