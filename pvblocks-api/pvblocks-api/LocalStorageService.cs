using System.Threading.Tasks;

namespace pvblocks_api
{
    public class LocalStorageService : ILocalStorageService
    {
        public async Task<T> GetItem<T>(string key)
        {
            await Task.CompletedTask;
            return default(T);
        }

        public async Task SetItem<T>(string key, T value)
        {
            await Task.CompletedTask;
        }

        public async Task RemoveItem(string key)
        {
            await Task.CompletedTask;
        }
    }
}