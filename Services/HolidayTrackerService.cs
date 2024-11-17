using System.Diagnostics;

namespace HolidayTracker.Services
{
    public class HolidayTrackerService(HolidaysRepository bookedDaysRepository, HolidaysRepository holidaysRepository) : IHolidayTrackerService
    {
        private readonly HolidaysRepository _HolidaysRepository = bookedDaysRepository;
        private readonly HolidaysRepository _holidaysRepository = holidaysRepository;

        public async Task<List<Holidays>> GetHolidays()
        {
            return await _HolidaysRepository.GetItemsAsync();
        }

        public async Task<int> DeleteHolidays(Holidays booked)
        {
            return await _HolidaysRepository.DeleteItemAsync(booked);
        }

        public async Task<int> SaveHolidays(Holidays booked)
        {
            return await _HolidaysRepository.DeleteItemAsync(booked);
        }

        public async Task ResetDatabase()
        {
            List<Holidays> bookedDayList = await GetHolidays();
            for (int i = 0; i < bookedDayList.Count; i++)
            {
                try
                {
                    Holidays bookedDay = bookedDayList[i];
                    await _HolidaysRepository.DeleteItemAsync(bookedDay);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }

            List<Holidays> holidayList = await GetHolidays();
            for (int i = 0; i < holidayList.Count; i++)
            {
                try
                {
                    Holidays holiday = holidayList[i];
                    await _holidaysRepository.DeleteItemAsync(holiday);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }

            await _HolidaysRepository.SeedDataAsync();
            await _holidaysRepository.SeedDataAsync();
        }
    }
}
