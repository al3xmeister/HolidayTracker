using System.Diagnostics;

namespace HolidayTracker.Services
{
    public class HolidayTrackerService(HolidayRepository holidayRepository) : IHolidayTrackerService
    {
        private readonly HolidayRepository _holidayRepository = holidayRepository;

        public async Task<List<Holiday>> GetAllHolidays()
        {
            return await _holidayRepository.GetItemsAsync();
        }

        public async Task<int> DeleteHolidays(Holiday booked)
        {
            return await _holidayRepository.DeleteItemAsync(booked);
        }

        public async Task<int> SaveHoliday(Holiday booked)
        {
            return await _holidayRepository.DeleteItemAsync(booked);
        }

        public async Task ResetDatabase()
        {
            List<Holiday> holidayList = await GetAllHolidays();
            for (int i = 0; i < holidayList.Count; i++)
            {
                try
                {
                    Holiday holiday = holidayList[i];
                    await _holidayRepository.DeleteItemAsync(holiday);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }

            await SeedDataAsync();
        }

        public int CalculateDaysTakenForAlex(Holiday holiday)
        {
            if (holiday is null)
            {
                throw new ArgumentNullException();
            }
            int workdayCount = 0;

            // Loop through each date in the range
            for (var date = holiday.StartDate; date <= holiday.EndDate && date.Year == DateTime.Today.Year; date = date.AddDays(1))
            {
                // Check if the day is a weekday (Monday to Friday)
                if (date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday)
                {
                    workdayCount++;
                }
            }

            return workdayCount;
        }

        public int CalculateDaysTakenForElla(Holiday holiday)
        {
            if (holiday is null)
            {
                throw new ArgumentNullException();
            }
            int workdayCount = 0;

            // Loop through each date in the range
            for (var date = holiday.StartDate; date <= holiday.EndDate && date.Year == DateTime.Today.Year; date = date.AddDays(1))
            {
                // Check if the day is a work day (Thu, Wed, Fri,Sat)
                if (date.DayOfWeek == DayOfWeek.Tuesday || date.DayOfWeek == DayOfWeek.Wednesday || date.DayOfWeek == DayOfWeek.Friday || date.DayOfWeek == DayOfWeek.Saturday)
                {
                    workdayCount++;
                }
            }

            return workdayCount;
        }

        public async Task SeedDataAsync()
        {
            try
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
                    new Holiday { Name = "Crăciun", StartDate = new DateTime(i, 12, 25), EndDate = new DateTime(i, 12, 26), Person="Ella", Status = "Aprobat" },
                    new Holiday { Name = "Zi de naștere", StartDate = new DateTime(i, 11, 26), EndDate = new DateTime(i, 11, 26), Person = "Ella", Status = "Aprobat" },
                        };

                        toAdd.AddRange(preApproved);
                    }

                    await _holidayRepository.InsertItemsAsync([.. toAdd.OrderBy(d => d.StartDate)]);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        public int CalculateDaysTakenForAlex(DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        public int CalculateDaysTakenForElla(DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        public async Task<Holiday?> GetHolidayById(int holidayId)
        {
           return await _holidayRepository.GetItemAsync(holidayId);
        }
    }
}
