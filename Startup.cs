using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using stake_place_web.Hubs;
using stake_place_web.Service;
namespace stake_place_web
{
    public class Startup
    {
        public Startup (IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration
        {
            get;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices (IServiceCollection services)
        {

            services.AddControllersWithViews ();

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles (configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });
            services.AddSignalR();
            services.AddScoped<ILoginService, LoginService> ();
            services.AddScoped<IFilterService, FilterService> ();
            services.AddScoped<IUserService, UserService> ();
            services.AddSingleton<ITicketService, TicketService> ();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure (IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider, IHostApplicationLifetime hostApplicationLifetime)
        {
            if (env.IsDevelopment ())
            {
                app.UseDeveloperExceptionPage ();
            }
            else
            {
                app.UseExceptionHandler ("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts ();
            }
            app.UseHttpsRedirection ();
            app.UseStaticFiles ();
            app.UseSpaStaticFiles ();

            app.UseRouting ();

            app.UseEndpoints (endpoints =>
            {
                endpoints.MapControllerRoute (
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
                endpoints.MapHub<MainFormResultHub>("/ConnectionHub");
            });

            app.UseSpa (spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment ())
                {
                    spa.UseReactDevelopmentServer (npmScript: "start");
                }
            });
        }
    }
}