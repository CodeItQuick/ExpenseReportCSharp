namespace Application.Services;

public interface DateProvider
{
    DateTimeOffset CurrentDate();
}