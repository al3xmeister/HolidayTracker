namespace HolidayTracker.Data
{
    public class HolidaysRepository : IRepository<Holidays>
    {
        private readonly SQLiteAsyncConnection _database;

        public HolidaysRepository(SQLiteAsyncConnection database)
        {
            _database = database;
            _database.CreateTableAsync<Holidays>().Wait();
        }

        public async Task SeedDataAsync()
        {
            // Check if there are already items in the database
            var existingItems = await _database.Table<Holidays>().CountAsync();
            if (existingItems == 0) // If the table is empty, insert initial data
            {
                var startYearIndex = DateTime.Today.Year;

                var toAdd = new List<Holidays>();
                for (int i = startYearIndex; i <= startYearIndex + 1; i++)
                {
                    var schoolHolidays = new List<Holidays>
                    {
                        new Holidays { Name = "Summer holidays", StartDate = new DateTime(i, 6, 28), EndDate = new DateTime(i,8,16)  },
                        new Holidays { Name = "In service day", StartDate = new DateTime(i, 8, 19), EndDate = new DateTime(i,8,19)  },
                        new Holidays { Name = "In service days", StartDate = new DateTime(i, 9, 16), EndDate = new DateTime(i,9,17)  },
                        new Holidays { Name = "October holidays", StartDate = new DateTime(i, 10, 14), EndDate = new DateTime(i,10,25)  },
                        new Holidays { Name = "Christmas holidays", StartDate = new DateTime(i, 12, 23), EndDate = new DateTime(i+1,1,3)  },
                     };

                    toAdd.AddRange(schoolHolidays.Where(d => d.StartDate >= DateTime.Today));
                }

                var otherDays = new List<Holidays>
                    {
                        new Holidays { Name = "Părinți", StartDate = new DateTime(2025, 5, 10), EndDate = new DateTime(2025, 5, 27) },
                    };

                toAdd.AddRange(otherDays.Where(d => d.StartDate >= DateTime.Today));
                await _database.InsertAllAsync(toAdd.OrderBy(d => d.StartDate));
            }
        }

        public Task<int> DeleteItemAsync(Holidays item)
        {
            return _database.DeleteAsync(item);
        }

        public Task<Holidays> GetItemAsync(int id)
        {
            return _database.Table<Holidays>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public Task<List<Holidays>> GetItemsAsync()
        {
            return _database.Table<Holidays>().ToListAsync();
        }

        public Task<int> SaveItemAsync(Holidays item)
        {
            if (item.Id != 0)
            {
                return _database.UpdateAsync(item);
            }
            else
            {
                return _database.InsertAsync(item);
            }
        }
    }
}
