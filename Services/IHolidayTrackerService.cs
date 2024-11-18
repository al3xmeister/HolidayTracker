
namespace HolidayTracker.Services
{
    public interface IHolidayTrackerService
    {
        Task<List<Holiday>> GetAllHolidays();
        Task<int> DeleteHolidays(Holiday booked);
        Task<int> SaveHoliday(Holiday booked);
        Task ResetDatabase();
        int CalculateDaysTakenForAlex(DateTime startDate, DateTime endDate);
        int CalculateDaysTakenForElla(DateTime startDate, DateTime endDate);

        Task SeedDataAsync();
    }
}