using System.Threading.Tasks;

namespace pvblocks_api
{
    public interface IHttpService
    {
        string Client { set; }
    
        Task<T> Get<T>(string uri);
        Task<T> Post<T>(string uri, object value);
        Task<T> Delete<T>(string uri);
        Task<T> Put<T>(string uri, object value);
    }
}