using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.Data.SqlClient;
using System.Data;

namespace MetLifeInsurance.DapperConfigure
{
    public class DataAccessLayer : IDataAccessLayer
    {
        public static IConfiguration GetConfig()
        {
            var builder = new ConfigurationBuilder()
          .SetBasePath(Directory.GetCurrentDirectory())
          .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }
        public IDbConnection GetDbConnection()
        {
            var settings = GetConfig();
            var connectionString = settings["ConnectionStrings:connection"];
            return new SqlConnection(connectionString);
        }
        public List<T> FetchDerivedModel<T>(string query)
        {
            using (IDbConnection dbConnection = GetDbConnection())
            {
                dbConnection.Open();
                var dta = dbConnection.Query<T>(query);
                dbConnection.Dispose();
                dbConnection.Close();
                return dta.AsList();
            }
        }


        public List<T> FetchDerivedModel<T>(string query, object param)
        {
            using (IDbConnection db = GetDbConnection())
            {
                var dta = db.Query<T>(query, param);
                db.Dispose();
                db.Close();
                return dta.AsList();
            }
        }
        public List<T> FetchDerivedModel<T>(string query, object param, CommandType commandType)
        {
            using (IDbConnection db = GetDbConnection())
            {
                var dta = db.Query<T>(query, param, commandType: commandType);
                db.Dispose();
                db.Close();
                return dta.AsList();
            }
        }

        public async Task<List<T>> FetchDerivedModelAsync<T>(string query)
        {
            using (IDbConnection dbConnection = GetDbConnection())
            {
                dbConnection.Open();
                var dta = await dbConnection.QueryAsync<T>(query);
                return dta.AsList();
            }
        }

        public async Task<List<T>> FetchDerivedModelAsync<T>(string query, object param)
        {
            using (IDbConnection db = GetDbConnection())
            {
                var dta = await db.QueryAsync<T>(query, param);
                return dta.AsList();
            }
        }

        public async Task<int> EntityInsertAsync<T>(object model)
        {
            using (IDbConnection db = GetDbConnection())
            {
                var id = await db.InsertAsync(model);
                db.Dispose();
                db.Close();
                return id;
            }
        }

        public async Task<bool> EntityUpdateAsync<T>(object model)
        {
            using (IDbConnection db = GetDbConnection())
            {
                var isSuccess = await db.UpdateAsync(model);
                db.Dispose();
                db.Close();
                return isSuccess;
            }
        }

        public async Task<bool> EntityDeleteAsync<T>(object model)
        {
            using (IDbConnection db = GetDbConnection())
            {
                var isSuccess = await db.DeleteAsync(model);
                db.Dispose();
                db.Close();
                return isSuccess;
            }
        }
    }
}
