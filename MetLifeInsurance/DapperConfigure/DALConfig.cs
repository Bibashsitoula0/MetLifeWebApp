using Microsoft.Data.SqlClient;
using System.Data;

namespace MetLifeInsurance.DapperConfigure
{
    public class DALConfig
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
    }
}
