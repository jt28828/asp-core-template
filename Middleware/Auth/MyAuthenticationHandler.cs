using System;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using DotnetCoreWebApiTemplate.Database;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DotnetCoreWebApiTemplate.Middleware.Auth
{
    public class MyAuthenticationHandler : AuthenticationHandler<MyAuthenticationOptions>
    {
        private readonly ServerDbContext _dbConnection;
        private readonly ILogger<MyAuthenticationHandler> _logger;

        public MyAuthenticationHandler(IOptionsMonitor<MyAuthenticationOptions> options,
            ILoggerFactory loggerFactory,
            UrlEncoder encoder, ISystemClock clock, ServerDbContext database) : base(
            options,
            loggerFactory, encoder, clock)
        {
            _dbConnection = database;
            _logger = loggerFactory.CreateLogger<MyAuthenticationHandler>();
        }

        /// <summary>
        /// Check the required authorisation token if required.
        /// Also decrypt incoming encrypted data string
        /// </summary>
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            // Implement your own Authentication here if required.
            // Return an authentication ticket full of Claim objects to provide info to controller routes

            // Need an await here so the override fits the signature.
            // Usually will have an await on database access anyway
            await Task.Delay(100);

            return AuthenticateResult.Success(new AuthenticationTicket(new ClaimsPrincipal(), Scheme.Name));
        }
    }
}