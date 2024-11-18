using System.Diagnostics;

namespace HolidayTracker.Services
{
    public class HolidayTrackerService(HolidayRepository holidayRepository, GiftDayRepository giftDayRepository) : IHolidayTrackerService
    {
        private readonly HolidayRepository _holidayRepository = holidayRepository;
        private readonly GiftDayRepository _giftDayRepository = giftDayRepository;

        public async Task<List<Holiday>> GetAllHolidays()
        {
            return await _holidayRepository.GetItemsAsync();
        }

        public async Task<List<GiftDay>> GetGiftedHolidays()
        {
            return await _giftDayRepository.GetItemsAsync();
        }

        public async Task<int> DeleteHolidays(Holiday booked)
        {
            return await _holidayRepository.DeleteItemAsync(booked);
        }

        public async Task<int> DeleteGiftDay(GiftDay gift)
        {
            return await _giftDayRepository.DeleteItemAsync(gift);
        }

        public async Task<int> SaveHoliday(Holiday booked)
        {
            return await _holidayRepository.DeleteItemAsync(booked);
        }

        public async Task<int> SaveGiftDay(GiftDay gift)
        {
            return await _giftDayRepository.DeleteItemAsync(gift);
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

        public async Task<double> CalculateDaysTakenForAlex(Holiday holiday)
        {
            if (holiday is null)
            {
                throw new ArgumentNullException();
            }
            double workdayCount = 0;
            var allGiftDays = await _giftDayRepository.GetItemsAsync();
            var alexGiftDays = allGiftDays.Where(d => d.Person == Enums.Person.Alex.ToString());

            // Loop through each date in the range
            for (var date = holiday.StartDate; date <= holiday.EndDate && date.Year == DateTime.Today.Year; date = date.AddDays(1))
            {
                // Check if the day is a weekday (Monday to Friday)
                if (date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday)
                {
                    if (!alexGiftDays.Any(d => d.Day == date))
                    {
                        if (holiday.HalfDay)
                        {
                            workdayCount = workdayCount + 0.5;
                        }
                        else
                        {
                            workdayCount++;
                        }
                    }
                }
            }

            return workdayCount;
        }

        public async Task<double> CalculateDaysTakenForElla(Holiday holiday)
        {
            if (holiday is null)
            {
                throw new ArgumentNullException();
            }
            double workdayCount = 0;
            var allGiftDays = await _giftDayRepository.GetItemsAsync();
            var ellaGiftDays = allGiftDays.Where(d => d.Person == Enums.Person.Ella.ToString());

            // Loop through each date in the range
            for (var date = holiday.StartDate; date <= holiday.EndDate && date.Year == DateTime.Today.Year; date = date.AddDays(1))
            {
                // Check if the day is a work day (Thu, Wed, Fri,Sat)
                if (date.DayOfWeek == DayOfWeek.Tuesday || date.DayOfWeek == DayOfWeek.Wednesday || date.DayOfWeek == DayOfWeek.Friday || date.DayOfWeek == DayOfWeek.Saturday)
                {
                    if (!ellaGiftDays.Any(d => d.Day == date))
                    {
                        if (holiday.HalfDay)
                        {
                            workdayCount = workdayCount + 0.5;
                        }
                        else
                        {
                            workdayCount++;
                        }
                    }
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

                    var toAddHolidays = new List<Holiday>();
                    var toAddGifts = new List<GiftDay>();

                    //Alex
                    for (int i = startYearIndex; i < startYearIndex + 1; i++)
                    {
                        var aproved = new List<Holiday>
                        {
                            new Holiday { Name = "New Year24", StartDate = new DateTime(i, 1, 1), EndDate = new DateTime(i, 1, 1), Person=Enums.Person.Alex.ToString(), Status = Enums.Status.Aprobat.ToString() },
                            new Holiday { Name = "Feb24", StartDate = new DateTime(i, 2, 15), EndDate = new DateTime(i, 2, 15), Person=Enums.Person.Alex.ToString(), Status = Enums.Status.Aprobat.ToString() },
                            new Holiday { Name = "Apr24", StartDate = new DateTime(i, 4, 22), EndDate = new DateTime(i, 4, 22), Person=Enums.Person.Alex.ToString(), Status = Enums.Status.Aprobat.ToString() },
                            new Holiday { Name = "Apr24", StartDate = new DateTime(i, 4, 29), EndDate = new DateTime(i, 4, 29), Person=Enums.Person.Alex.ToString(), Status = Enums.Status.Aprobat.ToString() },
                            new Holiday { Name = "May24", StartDate = new DateTime(i, 5, 3), EndDate = new DateTime(i, 5, 6), Person=Enums.Person.Alex.ToString(), Status = Enums.Status.Aprobat.ToString() },
                            new Holiday { Name = "June24", StartDate = new DateTime(i, 6, 7), EndDate = new DateTime(i, 6, 21), Person=Enums.Person.Alex.ToString(), Status = Enums.Status.Aprobat.ToString() },
                            new Holiday { Name = "July24", StartDate = new DateTime(i, 7, 1), EndDate = new DateTime(i, 7, 1), Person=Enums.Person.Alex.ToString(), Status = Enums.Status.Aprobat.ToString() },
                            new Holiday { Name = "Aug24", StartDate = new DateTime(i, 8, 8), EndDate = new DateTime(i, 8, 8), Person=Enums.Person.Alex.ToString(), Status = Enums.Status.Aprobat.ToString(), HalfDay = true },
                            new Holiday { Name = "Aug24", StartDate = new DateTime(i, 8, 12), EndDate = new DateTime(i, 8, 12), Person=Enums.Person.Alex.ToString(), Status = Enums.Status.Aprobat.ToString()},
                            new Holiday { Name = "Sept24", StartDate = new DateTime(i, 9, 30), EndDate = new DateTime(i, 9, 30), Person=Enums.Person.Alex.ToString(), Status = Enums.Status.Aprobat.ToString()},
                            new Holiday { Name = "Croazieră", StartDate = new DateTime(i, 11, 21), EndDate = new DateTime(i, 12, 2), Person=Enums.Person.Alex.ToString(), Status = Enums.Status.Aprobat.ToString()},
                            new Holiday { Name = "Crăciun", StartDate = new DateTime(i, 12, 25), EndDate = new DateTime(i, 12, 26), Person=Enums.Person.Alex.ToString(), Status = Enums.Status.Aprobat.ToString()},
                            new Holiday { Name = "Crăciun2", StartDate = new DateTime(i, 12, 27), EndDate = new DateTime(i, 12, 27), Person=Enums.Person.Alex.ToString(), Status = Enums.Status.AprobatDoarInTimetastic.ToString(), HalfDay = true},
                            new Holiday { Name = "Crăciun3", StartDate = new DateTime(i, 12, 28), EndDate = new DateTime(i, 12, 31), Person=Enums.Person.Alex.ToString(), Status = Enums.Status.AprobatDoarInTimetastic.ToString()},
                        };

                        toAddHolidays.AddRange(aproved);
                    }

                    //Ella
                    for (int i = startYearIndex; i <= startYearIndex + 1; i++)
                    {
                        var preApproved = new List<Holiday>
                        {
                           // new Holiday { Name = "Crăciun", StartDate = new DateTime(i, 12, 25), EndDate = new DateTime(i, 12, 26), Person=Enums.Person.Ella.ToString(), Status = Enums.Status.Aprobat.ToString() },
                           new Holiday { Name = "Zi de naștere", StartDate = new DateTime(i, 11, 26), EndDate = new DateTime(i, 11, 26), Person = Enums.Person.Ella.ToString(), Status = Enums.Status.Aprobat.ToString() },
                        };

                        toAddHolidays.AddRange(preApproved);
                    }

                    var approved = new List<Holiday>
                        {
                           new Holiday { Name = "Ian1", StartDate = new DateTime(2024, 1, 2), EndDate = new DateTime(2024, 1, 2), Person=Enums.Person.Ella.ToString(), Status = Enums.Status.Aprobat.ToString() },
                           new Holiday { Name = "Ian2", StartDate = new DateTime(2024, 1, 23), EndDate = new DateTime(2024, 1, 23), Person = Enums.Person.Ella.ToString(), Status = Enums.Status.Aprobat.ToString() },
                           new Holiday { Name = "Feb", StartDate = new DateTime(2024, 2, 20), EndDate = new DateTime(2024, 2, 24), Person = Enums.Person.Ella.ToString(), Status = Enums.Status.Aprobat.ToString() },
                           new Holiday { Name = "May", StartDate = new DateTime(2024, 5, 3), EndDate = new DateTime(2024, 5, 4), Person = Enums.Person.Ella.ToString(), Status = Enums.Status.Aprobat.ToString() },
                           new Holiday { Name = "June", StartDate = new DateTime(2024, 6, 7), EndDate = new DateTime(2024, 6, 22), Person = Enums.Person.Ella.ToString(), Status = Enums.Status.Aprobat.ToString() },
                           new Holiday { Name = "Croazieră", StartDate = new DateTime(2024, 11, 22), EndDate = new DateTime(2024, 11, 30), Person = Enums.Person.Ella.ToString(), Status = Enums.Status.Aprobat.ToString() },
                        };

                    toAddHolidays.AddRange(approved);
                    await _holidayRepository.InsertItemsAsync([.. toAddHolidays.OrderBy(d => d.StartDate)]);


                    var gifted = new List<GiftDay>
                        {
                           new GiftDay { Name = "Zi de naștere", Day = new DateTime(DateTime.Today.Year, 11, 26), Person=Enums.Person.Ella.ToString() },
                        };

                    toAddGifts.AddRange(gifted);
                    await _giftDayRepository.InsertItemsAsync([.. toAddGifts.OrderBy(d => d.Day)]);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        public async Task<Holiday?> GetHolidayById(int holidayId)
        {
            return await _holidayRepository.GetItemAsync(holidayId);
        }
    }
}
