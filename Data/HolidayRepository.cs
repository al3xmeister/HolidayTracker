namespace HolidayTracker.Data
{
    public class HolidayRepository : IRepository<Holiday>
    {
        private readonly SQLiteAsyncConnection _database;

        public HolidayRepository(SQLiteAsyncConnection database)
        {
            _database = database;
            _database.CreateTableAsync<Holiday>().Wait();
        }

        public Task<int> DeleteItemAsync(Holiday item)
        {
            return _database.DeleteAsync(item);
        }

        public async Task<Holiday?> GetItemAsync(int id)
        {
            var item = await _database.Table<Holiday>().Where(i => i.Id == id).FirstOrDefaultAsync();

            return item;
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
