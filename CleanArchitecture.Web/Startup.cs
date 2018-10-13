using System;
using System.Collections.Generic;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.ViewModels.Configuration;
using CleanArchitecture.Infrastructure.Data;
using CleanArchitecture.Web.Extensions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;
using StructureMap;
using CleanArchitecture.Core.SharedKernel;
using CleanArchitecture.Core.Interfaces.Repository;
using CleanArchitecture.Infrastructure.Services.Repository;
using CleanArchitecture.Infrastructure.Data.Transaction;
using CleanArchitecture.Infrastructure.Interfaces;
using CleanArchitecture.Infrastructure.Services;
using CleanArchitecture.Core.Interfaces.Configuration;
using CleanArchitecture.Infrastructure.Services.Configuration;
using CleanArchitecture.Infrastructure.Services.Transaction;
using CleanArchitecture.Core.Services.RadisDatabase;
using CleanArchitecture.Core.Services.Session;

namespace CleanArchitecture.Web
{
    public class Startup
    {

        // Order or run
        //1) Constructor
        //2) Configure services
        //3) Configure
        private IHostingEnvironment HostingEnvironment { get; }
        public static IConfiguration Configuration { get; set; }

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            HostingEnvironment = env;
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

                    //define Redis Configuration
            services.Configure<RedisConfiguration>(Configuration.GetSection("redis"));
            services.AddSession();
            services.AddDistributedRedisCache(options =>
            {
                options.Configuration = Configuration.GetValue<string>("redis:host");
                options.InstanceName = "master";
            });
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddSingleton<RedisSessionStorage>();
            ////debugging environment
            services.AddPreRenderDebugging(HostingEnvironment);

            services.AddOptions();

            //// Reponse compression
            services.AddResponseCompression();

            //// dtabase connection
            services.AddCustomDbContext();

            //// custom token
            services.AddCustomIdentity(Configuration);
            //// OpenIddict
            services.AddCustomOpenIddict(HostingEnvironment);

            // Store memory Cache
            services.AddMemoryCache();

            //// Depedenecy injection
            services.RegisterCustomServices();

            // Localization Cluture wise lanaguage
            services.AddCustomLocalization();

            // MVC Model filter validataion
            services.AddCustomizedMvc();


            // MVC Redis Cache Store Mamory
            services.RegisterRedisServer();

            //// Start Swagger           

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Clean Architecture Api", Version = "v1" });

                // Swagger 2.+ support
                var security = new Dictionary<string, IEnumerable<string>>
                {
                    {"Bearer", new string[] { }},
                };

                c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });
                c.AddSecurityRequirement(security);

            });


            services.AddAuthentication();
            ////End Swagger
            ////services.AddMvcCore();
            //services.AddDistributedMemoryCache();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            //services.Configure<EmailSettings>(Configuration.GetSection("Email"));
            //services.Configure<SMSSetting>(Configuration.GetSection("SMS"));

            services.AddMediatR(typeof(Startup));

            Container container = new Container();

            container.Configure(config =>
            {
                config.Scan(_ =>
                {
                    _.AssemblyContainingType(typeof(Startup)); // Web
                    _.AssemblyContainingType(typeof(BaseEntity)); // Core
                    _.Assembly("CleanArchitecture.Infrastructure"); // Infrastructure
                    _.WithDefaultConventions();
                    _.ConnectImplementationsToTypesClosing(typeof(IHandle<>));
                    _.ConnectImplementationsToTypesClosing(typeof(IRequestHandler<,>));
                });

                // TODO: Add Registry Classes to eliminate reference to Infrastructure

                // TODO: Move to Infrastucture Registry
                config.For(typeof(IRepository<>)).Add(typeof(EfRepository<>));
                config.For(typeof(ICommonRepository<>)).Add(typeof(EFCommonRepository<>));
                config.For(typeof(IWalletRepository)).Add(typeof(WalletRepository));
                config.For(typeof(IMessageRepository<>)).Add(typeof(MessageRepository<>));
                config.For(typeof(IWebApiRepository)).Add(typeof(WebApiDataRepository));
                config.For<IMediator>().Use<Mediator>();
                //vsolanki 8-10-2018 for wallet
                config.For(typeof(IBasePage)).Add(typeof(BasePage));
                config.For(typeof(IWalletService)).Add(typeof(WalletService));
                config.For(typeof(IWebApiSendRequest)).Add(typeof(WebAPISendRequest));
                config.For(typeof(IGetWebRequest)).Add(typeof(GetWebRequest));
                config.For(typeof(IWalletConfigurationService)).Add(typeof(WalletConfigurationService));
                
                //  config.For(typeof(ILogger));

                // added by nirav savariya for common repository on 10-04-2018
                config.For(typeof(ICustomRepository<>)).Add(typeof(CustomRepository<>));
                config.For(typeof(ITransactionProcess)).Add(typeof(NewTransaction));
                               
                
                //Populate the container using the service collection
                config.Populate(services);
               
            });

            return container.GetInstance<IServiceProvider>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddFile(Configuration["LogPath"].ToString());//Take from Setting file
            if (env.IsDevelopment())
            {
                // app.UseDeveloperExceptionPage();
                app.AddDevMiddlewares();
            }
            else
            {
                app.UseHsts();
                app.UseResponseCompression();
            }

            // NOTE: For SPA swagger needs adding before MVC
            app.UseCustomSwaggerApi(Configuration);
            app.UseHttpsRedirection();

            // https://github.com/openiddict/openiddict-core/issues/518
            // And
            // https://github.com/aspnet/Docs/issues/2384#issuecomment-297980490
            var forwarOptions = new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            };
            forwarOptions.KnownNetworks.Clear();
            forwarOptions.KnownProxies.Clear();

            app.UseForwardedHeaders(forwarOptions);

            app.UseAuthentication();

            app.UseCookiePolicy();

            app.Use(async (context, next) =>
            {
                // Do work that doesn't write to the Response.
                await next.Invoke();
                // Do logging or other work that doesn't write to the Response.
            });

            app.UseSession();

            /*

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Clean Architecture Api V1");
            });
         */
            app.UseMvc();
        }
    }
} 