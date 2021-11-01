using Dapper;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace MicroserviceNET.Monitoring
{
    public class MsSqlStartupHealthCheck : IHealthCheck
    {
        private readonly string _connectionString;
        public MsSqlStartupHealthCheck(string connectionString)
        {          
            _connectionString = connectionString;
        }
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            await using var conn = new SqlConnection(_connectionString);
            var result = await conn.QuerySingleAsync<int>("SELECT 1");

            return result == 1
                ? HealthCheckResult.Healthy()
                : HealthCheckResult.Degraded();
        }
    }
}
