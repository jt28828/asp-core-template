namespace DotnetCoreWebApiTemplate.Services
{
    /// <summary>
    /// The health service is a singleton that contains a single value representing
    /// whether or not the server should present itself as healthy.
    /// Used to shut down the server gracefully
    /// </summary>
    public class HealthService
    {
        public bool IsHealthy { get; set; }

        public HealthService()
        {
            IsHealthy = true;
        }
    }
}