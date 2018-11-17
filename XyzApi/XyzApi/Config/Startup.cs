using System;
using System.IO;
using System.Reflection;
using Dapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Swashbuckle.AspNetCore.Swagger;
using XyzApi.Repository;
using XyzApi.Repository.Impl;
using XyzApi.Services;
using XyzApi.Services.Impl;
using XyzApi.Utility;
using XyzApi.Utility.Impl;

namespace XyzApi.Config
{
    // ReSharper disable once ClassNeverInstantiated.Global
    /// <summary>
    /// 
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // ReSharper disable once MemberCanBePrivate.Global
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            //
            //    Log level 
            LogEventLevel logEventLevel;
            try
            {
                logEventLevel = (LogEventLevel) Enum.Parse(typeof(LogEventLevel),
                    Configuration["LoggerSettings:LogLevel"], true);
            }
            catch (Exception)
            {
                logEventLevel = LogEventLevel.Information;
            }

            //
            //    Serilog configuration
            var loggerConfiguration = new LoggerConfiguration()
                .MinimumLevel.Is(logEventLevel)
                .Enrich.FromLogContext();
            if (bool.Parse(Configuration["LoggerSettings:LogToConsole"]))
            {
                loggerConfiguration = loggerConfiguration.WriteTo.Console();
            }

            if (bool.Parse(Configuration["LoggerSettings:LogToFile"]))
            {
                loggerConfiguration = loggerConfiguration.WriteTo.File(
                    Configuration["LoggerSettings:LogFile"],
                    fileSizeLimitBytes: 1_000_000,
                    rollOnFileSizeLimit: true,
                    shared: true,
                    flushToDiskInterval: TimeSpan.FromSeconds(1));
            }

            Log.Logger = loggerConfiguration.CreateLogger();

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "XYZ Widget API",
                    Description = "A API for working with the XYZ Widget backend",
                    TermsOfService = "None",
                    Contact = new Contact
                    {
                        Name = "Mike Melusky",
                        Email = "mmelusky@ainq.com",
                        Url = "https://twitter.com/mrjavascript"
                    },
                    License = new License
                    {
                        Name = "Use under Apache License",
                        Url = "https://www.apache.org/licenses/LICENSE-2.0"
                    }
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            //
            //    Dependency Injection  for services and repositories
            IDatabaseConnectionFactory databaseConnectionFactory =
                new DatabaseConnectionFactory(Configuration.GetConnectionString("XYZ_WIDGET"));
            services.AddSingleton<IWidgetRepository>(s =>
            {
                if (s == null) throw new ArgumentNullException(nameof(s));
                return new WidgetRepository(databaseConnectionFactory);
            });
            services.AddSingleton<IWidgetService, WidgetService>();

            //
            //    Dapper config
            DefaultTypeMap.MatchNamesWithUnderscores = true;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "XYZ Widget API V1"); });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}