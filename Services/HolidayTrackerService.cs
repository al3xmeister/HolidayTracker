using System.Diagnostics;

namespace HolidayTracker.Services
{
    public class HolidayTrackerService(HolidaysRepository bookedDaysRepository, HolidaysRepository holidaysRepository) : IHolidayTrackerService
    {
        private readonly HolidaysRepository _HolidaysRepository = bookedDaysRepository;
        private readonly HolidaysRepository _holidaysRepository = holidaysRepository;

        public async Task<List<Holiday>> GetAllHolidays()
        {
            return await _HolidaysRepository.GetItemsAsync();
        }

        public async Task<int> DeleteHolidays(Holiday booked)
        {
            return await _HolidaysRepository.DeleteItemAsync(booked);
        }

        public async Task<int> SaveHoliday(Holiday booked)
        {
            return await _HolidaysRepository.DeleteItemAsync(booked);
        }

        public async Task ResetDatabase()
        {
            List<Holiday> bookedDayList = await GetAllHolidays();
            for (int i = 0; i < bookedDayList.Count; i++)
            {
                try
                {
                    Holiday bookedDay = bookedDayList[i];
                    await _HolidaysRepository.DeleteItemAsync(bookedDay);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }

            List<Holiday> holidayList = await GetAllHolidays();
            for (int i = 0; i < holidayList.Count; i++)
            {
                try
                {
                    Holiday holiday = holidayList[i];
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

        public int CalculateDaysTakenForAlex(DateTime startDate, DateTime endDate)
        {
            int weekdayCount = 0;

            // Loop through each date in the range
            for (var date = startDate; date <= endDate; date = date.AddDays(1))
            {
                // Check if the day is a weekday (Monday to Friday)
                if (date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday)
                {
                    weekdayCount++;
                }
            }

            return weekdayCount;
        }

        public int CalculateDaysTakenForElla(DateTime startDate, DateTime endDate)
        {
            int weekdayCount = 0;

            // Loop through each date in the range
            for (var date = startDate; date <= endDate; date = date.AddDays(1))
            {
                // Check if the day is a work day (Thu, Wed, Fri,Sat)
                if (date.DayOfWeek == DayOfWeek.Tuesday || date.DayOfWeek == DayOfWeek.Wednesday || date.DayOfWeek == DayOfWeek.Friday || date.DayOfWeek == DayOfWeek.Saturday)
                {
                    weekdayCount++;
                }
            }

            return weekdayCount;
        }

        public async Task SeedDataAsync()
        {
            // Check if there are already items in the database
            var existingItems = await GetAllHolidays();
            var existingItemsCount = existingItems.Count;
            if (existingItemsCount == 0) // If the table is empty, insert initial data
            {
                var startYearIndex = DateTime.Today.Year;

                var toAdd = new List<Holiday>();
                for (int i = startYearIndex; i <= startYearIndex + 1; i++)
                {
                    var preApproved = new List<Holiday>
                    {
                    new Holiday { Name = "Crăciun", StartDate = new DateTime(i, 12, 25), EndDate = new DateTime(i, 12, 26), Person="Ella", Status = "Aprobat", Taken = CalculateDaysTakenForElla(new DateTime(i, 12, 25), new DateTime(i, 12, 26)) },
                    new Holiday { Name = "Zi de naștere", StartDate = new DateTime(i, 11, 26), EndDate = new DateTime(i, 11, 26), Person = "Ella", Status = "Aprobat", Taken = CalculateDaysTakenForElla(new DateTime(i, 11, 26), new DateTime(i, 11, 26)) },
                    };

                    toAdd.AddRange(preApproved);
                }

                await _holidaysRepository.InsertItemsAsync([.. toAdd.OrderBy(d => d.StartDate)]);
            }
        }
    }
}
