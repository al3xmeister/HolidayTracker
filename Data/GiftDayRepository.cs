namespace HolidayTracker.Data
{
    public class GiftDayRepository : IRepository<GiftDay>
    {
        private readonly SQLiteAsyncConnection _database;

        public GiftDayRepository(SQLiteAsyncConnection database)
        {
            _database = database;
            _database.CreateTableAsync<GiftDay>().Wait();
        }

        public Task<int> DeleteItemAsync(GiftDay item)
        {
            return _database.DeleteAsync(item);
        }

        public async Task<GiftDay?> GetItemAsync(int id)
        {
            var item = await _database.Table<GiftDay>().Where(i => i.Id == id).FirstOrDefaultAsync();

            return item;
        }

        public Task<List<GiftDay>> GetItemsAsync()
        {
            return _database.Table<GiftDay>().ToListAsync();
        }

        public Task<int> SaveItemAsync(GiftDay item)
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

        public async Task<int> InsertItemsAsync(List<GiftDay> items)
        {
            return await _database.InsertAllAsync(items);
        }
    }
}
