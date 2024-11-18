namespace HolidayTracker.Data
{
    public class HolidaysRepository : IRepository<Holiday>
    {
        private readonly SQLiteAsyncConnection _database;

        public HolidaysRepository(SQLiteAsyncConnection database)
        {
            _database = database;
            _database.CreateTableAsync<Holiday>().Wait();
        }

        public async Task SeedDataAsync()
        {
            // Check if there are already items in the database
            var existingItems = await _database.Table<Holiday>().CountAsync();
            if (existingItems == 0) // If the table is empty, insert initial data
            {
                var startYearIndex = DateTime.Today.Year;

                var toAdd = new List<Holiday>();
                for (int i = startYearIndex; i <= startYearIndex + 1; i++)
                {
                    var preApproved = new List<Holiday>
                    {
                    new Holiday { Name = "Crăciun", StartDate = new DateTime(i, 12, 25), EndDate = new DateTime(i, 12, 26), Person="Ella", Status = "Aprobat" };
                    new Holiday { Name = "Zi de naștere", StartDate = new DateTime(i, 11, 26), EndDate = new DateTime(i, 11, 26), Person = "Ella", Status = "Aprobat" };
                    };

                    toAdd.AddRange(preApproved);
                }

                await _database.InsertAllAsync(toAdd.OrderBy(d => d.StartDate));
            }
        }

        public Task<int> DeleteItemAsync(Holiday item)
        {
            return _database.DeleteAsync(item);
        }

        public Task<Holiday> GetItemAsync(int id)
        {
            return _database.Table<Holiday>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public Task<List<Holiday>> GetItemsAsync()
        {
            return _database.Table<Holiday>().ToListAsync();
        }

        public Task<int> SaveItemAsync(Holiday item)
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

        public async Task<int> InsertItemsAsync(List<Holiday> items)
        {
            return await _database.InsertAllAsync(items);
        }
    }
}
