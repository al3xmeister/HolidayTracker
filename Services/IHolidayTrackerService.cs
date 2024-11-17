
namespace HolidayTracker.Services
{
    public interface IHolidayTrackerService
    {
        Task<List<Holidays>> GetHolidays();
        Task<int> DeleteHolidays(Holidays booked);
        Task<int> SaveHolidays(Holidays booked);
        Task ResetDatabase();
    }
}