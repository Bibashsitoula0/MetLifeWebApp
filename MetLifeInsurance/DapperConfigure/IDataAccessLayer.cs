using System.Data;

namespace MetLifeInsurance.DapperConfigure
{
    public interface IDataAccessLayer
    {
        List<T> FetchDerivedModel<T>(string query);
        List<T> FetchDerivedModel<T>(string query, object param);
        List<T> FetchDerivedModel<T>(string query, object param, CommandType commandType);
        Task<List<T>> FetchDerivedModelAsync<T>(string query);
        Task<List<T>> FetchDerivedModelAsync<T>(string query, object param);
        Task<int> EntityInsertAsync<T>(object model);
        Task<bool> EntityUpdateAsync<T>(object model);
        Task<bool> EntityDeleteAsync<T>(object model);
    }
}
