using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using DotnetCoreWebApiTemplate.Middleware.Auth;
using DotnetCoreWebApiTemplate.Middleware.Health;
using DotnetCoreWebApiTemplate.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Swashbuckle.AspNetCore.Swagger;

namespace DotnetCoreWebApiTemplate
{
    public class Startup
    {
        private const string ApiName = "API Name Here";
        private const string ApiVersion = "v1";
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // Add Entity Framework here with any provider. Demonstrated using postgres.
            // Retrieve connection data from the appSettings.Json file
            // services.AddEntityFrameworkNpgsql().AddDbContext<ServerDbContext>(options =>
            // options.UseNpgsql(_configuration.GetConnectionString("DatabaseConnection")));

            // TODO Use Custom Authentication Middleware if required. Otherwise remove
//            services.AddAuthentication("MyAuthenticationName")
//                .AddScheme<MyAuthenticationOptions, MyAuthenticationHandler>("MyAuthenticationName", null);


            // Setup the Health service to allow manual temporary shutting down of the server
            services.AddSingleton<HealthService>();

            // Setup the Health Check service to allow checking of health for load balancers / manual confirmation
            services.AddHealthChecks().AddCheck<HealthCheck>("health_check");

            services.AddSwaggerGen(opts =>
            {
                opts.SwaggerDoc(ApiVersion, new Info {Title = ApiName, Version = ApiVersion});

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                opts.IncludeXmlComments(xmlPath);
            });

            // Remove if you don't need CORS access but everyone hates CORS so I doubt it
            services.AddCors();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                // Enable middleware to serve generated Swagger as a JSON endpoint.
                app.UseSwagger();

                // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
                // specifying the Swagger JSON endpoint.
                app.UseSwaggerUI(c => { c.SwaggerEndpoint($"/swagger/{ApiVersion}/swagger.json", ApiName); });
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
                app.UseHttpsRedirection();
            }

            // Adds a route to check for the health of the server at http://serverUrl/server-health
            app.UseHealthChecks("/server-health", new HealthCheckOptions {ResponseWriter = WriteHealthResponse});

            // Remove if you don't need CORS access but everyone hates CORS so I doubt it
            app.UseCors(options => options.WithOrigins("*").AllowAnyMethod().AllowAnyHeader());

            // TODO Use Custom Authentication Middleware if required. Otherwise remove
            app.UseAuthentication();

            app.UseMvc();
        }

        /// <summary> Actually returns the value of a health check in the request </summary>
        private static Task WriteHealthResponse(HttpContext httpContext, HealthReport result)
        {
            httpContext.Response.ContentType = "text/plain";
            return httpContext.Response.WriteAsync(result.Entries["health_check"].Description);
        }
    }
}