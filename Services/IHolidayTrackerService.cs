
namespace HolidayTracker.Services
{
    public interface IHolidayTrackerService
    {
        Task<List<Holiday>> GetAllHolidays();
        Task<int> DeleteHolidays(Holiday booked);
        Task<int> SaveHoliday(Holiday booked);
        Task ResetDatabase();
        Task SeedDataAsync();
        Task<Holiday?> GetHolidayById(int holidayId);
        Task<List<Holiday>> GetHolidaysByYear(int year);
    }
}