namespace HolidayTracker.Data
{
    public interface IRepository<T> where T : class
    {
        Task<int> SaveItemAsync(T item);

        Task<int> DeleteItemAsync(T item);

        Task<List<T>> GetItemsAsync();

        Task<T?> GetItemAsync(int id);
        Task<int> InsertItemsAsync(List<T> items);
    }
}
