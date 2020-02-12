using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Evento.Api.Framework;
using Evento.Core.Repositories;
using Evento.Infrastructure.Mappers;
using Evento.Infrastructure.Repositories;
using Evento.Infrastructure.Services;
using Evento.Infrastructure.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace Evento.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IContainer Container { get; set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {            
            services.AddMvc()
                .AddJsonOptions(x => x.SerializerSettings.Formatting = Formatting.Indented)
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddMemoryCache();
            services.AddAuthorization(x => x.AddPolicy("HasAdminRole", p => p.RequireRole("admin")));
            //services.AddScoped<IEventRepository, EventRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IEventService, EventService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITicketService, TicketService>();
            services.AddScoped<IDataInitializer, DataInitializer>();
            services.AddSingleton(AutoMapperConfig.Initialize());
            services.AddSingleton<IJwtHandler, JwtHandler>();
            services.Configure<JwtSettings>(Configuration.GetSection("jwt"));
            services.Configure<AppSettings>(Configuration.GetSection("app"));

            var sp = services.BuildServiceProvider();
            var jwtSettings = sp.GetService<IOptions<JwtSettings>>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = jwtSettings.Value.Issuer, //"https://localhost:44305/",
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Value.Key))
                };
            });

            var builder = new ContainerBuilder();
            builder.Populate(services);
            builder.RegisterType<EventRepository>().As<IEventRepository>().InstancePerLifetimeScope();
            Container = builder.Build();

            return new AutofacServiceProvider(Container);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime appLiftime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseMvc();
            SeedData(app);
            app.UseErrorHandler();
            appLiftime.ApplicationStopped.Register(() => Container.Dispose());
        }

        private void SeedData(IApplicationBuilder app)
        {
            var settings = app.ApplicationServices.GetService<IOptions<AppSettings>>();

            if (settings.Value.SeedData)
            {
                var dataInitializer = app.ApplicationServices.GetService<IDataInitializer>();
                dataInitializer.SeedAsync();
            }
        }
    }
}
