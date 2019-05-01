using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using DotnetCoreWebApiTemplate.Database;
using DotnetCoreWebApiTemplate.Services;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;

namespace DotnetCoreWebApiTemplate.Middleware.Health
{
    public class HealthCheck : IHealthCheck
    {
        private readonly ServerDbContext _database;
        private readonly ILogger<HealthCheck> _logger;
        private readonly HealthService _healthService;

        public HealthCheck(ServerDbContext database, ILogger<HealthCheck> logger, HealthService healthService)
        {
            _database = database;
            _logger = logger;
            _healthService = healthService;
        }


        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
            CancellationToken cancellationToken = new CancellationToken())
        {
            // Check if the database is accessible
            bool databaseConnectionOk;
            try
            {
                databaseConnectionOk = await _database.Database.CanConnectAsync(cancellationToken);
            }
            catch (Exception)
            {
                // Can't connect to the DB
                databaseConnectionOk = false;
            }

            // Also check if server has been manually set to deactive
            bool serverStillActive = _healthService.IsHealthy;

            string version = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion;

            HealthCheckResult serverHealthStatus;
            if (databaseConnectionOk && serverStillActive)
            {
                // Database can be connected to and the server hasn't been shut down
                serverHealthStatus = HealthCheckResult.Healthy($"Connection is healthy. Server version: {version}");
            }
            else if (!databaseConnectionOk)
            {
                // Database could not be connected to.
                serverHealthStatus = HealthCheckResult.Unhealthy(
                    $"Unhealthy! A connection to the database could not be established. Server version: {version}");
            }
            else
            {
                // Server has been manually shut down
                serverHealthStatus =
                    HealthCheckResult.Unhealthy(
                        $"Unhealthy! Server has been manually deactivated. Server version: {version}");
            }

            return serverHealthStatus;
        }
    }
}