using System.Diagnostics;
using System.Linq;

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

        public async Task<List<Holiday>> GetHolidaysByYear(int year)
        {
            var items = await _holidayRepository.GetItemsAsync();
            var current = items.Where(d => d.StartDate.Year == year);
            return current.ToList();
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

            List<GiftDay> giftDaysList = await GetGiftedHolidays();
            for (int i = 0; i < giftDaysList.Count; i++)
            {
                try
                {
                    GiftDay gift = giftDaysList[i];
                    await _giftDayRepository.DeleteItemAsync(gift);
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
            for (var date = holiday.StartDate; date <= holiday.EndDate && date.Year == holiday.StartDate.Year; date = date.AddDays(1))
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
            for (var date = holiday.StartDate; date <= holiday.EndDate && date.Year == holiday.StartDate.Year; date = date.AddDays(1))
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

                    for (int i = startYearIndex; i <= (startYearIndex + 3); i++)
                    {
                        var preApprovedAlex = new List<Holiday>
                        {
                            new Holiday { Name = $"Anul Nou {i}", StartDate = new DateTime(i, 1, 1), EndDate = new DateTime(i, 1, 1), Person = Enums.Person.Alex.ToString(), Status = Enums.Status.Aprobat.ToString() },
                        };
                        toAddHolidays.AddRange(preApprovedAlex);
                    }

                    var aprovedAlex = new List<Holiday>
                        {
                            //2024
                            new Holiday { Name = "Feb 24", StartDate = new DateTime(2024, 2, 15), EndDate = new DateTime(2024, 2, 15), Person=Enums.Person.Alex.ToString(), Status = Enums.Status.Aprobat.ToString() },
                            new Holiday { Name = "Apr 24", StartDate = new DateTime(2024, 4, 22), EndDate = new DateTime(2024, 4, 22), Person=Enums.Person.Alex.ToString(), Status = Enums.Status.Aprobat.ToString() },
                            new Holiday { Name = "Apr 24", StartDate = new DateTime(2024, 4, 29), EndDate = new DateTime(2024, 4, 29), Person=Enums.Person.Alex.ToString(), Status = Enums.Status.Aprobat.ToString() },
                            new Holiday { Name = "May 24", StartDate = new DateTime(2024, 5, 3), EndDate = new DateTime(2024, 5, 6), Person=Enums.Person.Alex.ToString(), Status = Enums.Status.Aprobat.ToString() },
                            new Holiday { Name = "June 24", StartDate = new DateTime(2024, 6, 7), EndDate = new DateTime(2024, 6, 21), Person=Enums.Person.Alex.ToString(), Status = Enums.Status.Aprobat.ToString() },
                            new Holiday { Name = "July 24", StartDate = new DateTime(2024, 7, 1), EndDate = new DateTime(2024, 7, 1), Person=Enums.Person.Alex.ToString(), Status = Enums.Status.Aprobat.ToString() },
                            new Holiday { Name = "Aug 24", StartDate = new DateTime(2024, 8, 8), EndDate = new DateTime(2024, 8, 8), Person=Enums.Person.Alex.ToString(), Status = Enums.Status.Aprobat.ToString(), HalfDay = true },
                            new Holiday { Name = "Aug 24", StartDate = new DateTime(2024, 8, 12), EndDate = new DateTime(2024, 8, 12), Person=Enums.Person.Alex.ToString(), Status = Enums.Status.Aprobat.ToString()},
                            new Holiday { Name = "Sept 24", StartDate = new DateTime(2024, 9, 30), EndDate = new DateTime(2024, 9, 30), Person=Enums.Person.Alex.ToString(), Status = Enums.Status.Aprobat.ToString()},
                            new Holiday { Name = "Croazieră", StartDate = new DateTime(2024, 11, 21), EndDate = new DateTime(2024, 12, 2), Person=Enums.Person.Alex.ToString(), Status = Enums.Status.Aprobat.ToString()},
                            new Holiday { Name = "Crăciun", StartDate = new DateTime(2024, 12, 25), EndDate = new DateTime(2024, 12, 26), Person=Enums.Person.Alex.ToString(), Status = Enums.Status.Aprobat.ToString()},
                            new Holiday { Name = "Crăciun2", StartDate = new DateTime(2024, 12, 27), EndDate = new DateTime(2024, 12, 27), Person=Enums.Person.Alex.ToString(), Status = Enums.Status.AprobatDoarInTimetastic.ToString(), HalfDay = true},
                            new Holiday { Name = "Crăciun3", StartDate = new DateTime(2024, 12, 28), EndDate = new DateTime(2024, 12, 31), Person=Enums.Person.Alex.ToString(), Status = Enums.Status.AprobatDoarInTimetastic.ToString()},
                        
                            //2025
                            new Holiday { Name = "Cluj", StartDate = new DateTime(2025, 6, 12), EndDate = new DateTime(2025, 6, 21), Person = Enums.Person.Alex.ToString(), Status = Enums.Status.CerutPentruAprobare.ToString() },
                            new Holiday { Name = "Mangalia", StartDate = new DateTime(2025, 10, 12), EndDate = new DateTime(2025, 10, 20), Person = Enums.Person.Alex.ToString(), Status = Enums.Status.Necerut.ToString() },

                    };
                    toAddHolidays.AddRange(aprovedAlex);

                    //Ella
                    for (int j = startYearIndex; j <= (startYearIndex + 3); j++)
                    {
                        var preApprovedElla = new List<Holiday>
                        {
                           new Holiday { Name = "Crăciun", StartDate = new DateTime(j, 12, 25), EndDate = new DateTime(j, 12, 26), Person=Enums.Person.Ella.ToString(), Status = Enums.Status.Aprobat.ToString() },
                           new Holiday { Name = "Zi de naștere", StartDate = new DateTime(j, 11, 26), EndDate = new DateTime(j, 11, 26), Person = Enums.Person.Ella.ToString(), Status = Enums.Status.Aprobat.ToString() },
                           new Holiday { Name = "Anul nou", StartDate = new DateTime(j, 1, 1), EndDate = new DateTime(j, 1, 2), Person = Enums.Person.Ella.ToString(), Status = Enums.Status.Aprobat.ToString() },
                           new Holiday { Name = "Revelion", StartDate = new DateTime(j, 12, 31), EndDate = new DateTime(j, 12, 31), Person = Enums.Person.Ella.ToString(), Status = Enums.Status.Aprobat.ToString() },
                        };

                        var gifted = new List<GiftDay>
                        {
                           new GiftDay { Name = "Zi de naștere", Day = new DateTime(j, 11, 26), Person=Enums.Person.Ella.ToString() },
                           new GiftDay { Name = "Crăciun 1", Day = new DateTime(j,12,25), Person=Enums.Person.Ella.ToString() },
                           new GiftDay { Name = "Crăciun 2", Day = new DateTime(j,12,26), Person=Enums.Person.Ella.ToString() },
                        };

                        toAddHolidays.AddRange(preApprovedElla);
                        toAddGifts.AddRange(gifted);
                    }

                    var approved = new List<Holiday>
                        {
                           //2024
                           new Holiday { Name = "Ian2", StartDate = new DateTime(2024, 1, 23), EndDate = new DateTime(2024, 1, 23), Person = Enums.Person.Ella.ToString(), Status = Enums.Status.Aprobat.ToString() },
                           new Holiday { Name = "Feb", StartDate = new DateTime(2024, 2, 20), EndDate = new DateTime(2024, 2, 24), Person = Enums.Person.Ella.ToString(), Status = Enums.Status.Aprobat.ToString() },
                           new Holiday { Name = "May", StartDate = new DateTime(2024, 5, 3), EndDate = new DateTime(2024, 5, 4), Person = Enums.Person.Ella.ToString(), Status = Enums.Status.Aprobat.ToString() },
                           new Holiday { Name = "June", StartDate = new DateTime(2024, 6, 7), EndDate = new DateTime(2024, 6, 22), Person = Enums.Person.Ella.ToString(), Status = Enums.Status.Aprobat.ToString() },
                           new Holiday { Name = "Croazieră", StartDate = new DateTime(2024, 11, 22), EndDate = new DateTime(2024, 11, 30), Person = Enums.Person.Ella.ToString(), Status = Enums.Status.Aprobat.ToString() },
                           //2025
                           new Holiday { Name = "Cluj", StartDate = new DateTime(2025, 6, 13), EndDate = new DateTime(2025, 6, 21), Person = Enums.Person.Ella.ToString(), Status = Enums.Status.Aprobat.ToString() },
                           new Holiday { Name = "Mangalia", StartDate = new DateTime(2025, 10, 12), EndDate = new DateTime(2025, 10, 20), Person = Enums.Person.Ella.ToString(), Status = Enums.Status.Necerut.ToString() },
                        };

                    toAddHolidays.AddRange(approved);
                    await _holidayRepository.InsertItemsAsync([.. toAddHolidays.OrderBy(d => d.StartDate)]);

                    var otherGifted = new List<GiftDay>
                        {
                           new GiftDay { Name = "În schimb pt o joi din Sept", Day = new DateTime(2024, 11, 21), Person=Enums.Person.Ella.ToString() },
                           new GiftDay { Name = "În schimb pt 30 Dec", Day = new DateTime(2024, 12, 31), Person=Enums.Person.Ella.ToString() },
                        };

                    toAddGifts.AddRange(otherGifted);
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
