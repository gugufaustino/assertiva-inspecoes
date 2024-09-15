using Differencial.IOC;
using Differencial.Repository.Context;
using Differencial.Service;
using Differencial.Web.Configurations;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;


namespace Differencial.Web
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IHostEnvironment HostEnvironment { get; }

        public Startup(IHostEnvironment hostingEnvironment)
        {
           
            var builder = new ConfigurationBuilder()
                               .SetBasePath(hostingEnvironment.ContentRootPath)
                               .AddJsonFile("appsettings.json", true, true)
                               .AddJsonFile($"appsettings.{hostingEnvironment.EnvironmentName}.json", true, true)
                               .AddEnvironmentVariables();

            this.Configuration = builder.Build();
            this.HostEnvironment = hostingEnvironment;
        }


        public void ConfigureServices(IServiceCollection services)
        {
            var mvcBuilder = services.AddControllersWithViews(ConfigureMvcOptions)                
                                    .AddNewtonsoftJson(options =>
                                    {
                                        options.UseMemberCasing();
                                        options.SerializerSettings.Culture = new System.Globalization.CultureInfo("pt-BR");
                                        options.SerializerSettings.Converters.Add(new Helpers.DecimalDoubleJsonConverter());
                                        // Converters = new List<JsonConverter> { new DecimalDoubleConverter() }
                                    });

            if (HostEnvironment.IsDevelopment())
            { 
                mvcBuilder.AddRazorRuntimeCompilation();
            }

            services.AddLogging(builder => builder.AddConsole());
            services.AddSingleton<IFileProvider>(new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));




            services.AddDbContext<DifferencialContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DifferencialConnection")));
           
            //services.AddAutoMapper(typeof(Startup));
            Infra.AutoMapperConfig.RegisterAutoMapper();

            services.ResolveDependencies();

            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(1200);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            services.AddSignalR(i=>
            {
                i.EnableDetailedErrors = true;
                i.MaximumReceiveMessageSize = long.MaxValue;
            });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options => {
                options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                options.SlidingExpiration = true;
                options.AccessDeniedPath = "/Error401/";
                options.LoginPath = "/Home/Login/";
            });
            

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            using (var scope = app.ApplicationServices.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<DifferencialContext>();
                dbContext.Database.Migrate(); // Aplica as migrations automaticamente
            } 

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else {
                //app.UseExceptionHandler("/Home/Error");
                app.UseDeveloperExceptionPage(); //TODO melhorar isso
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSession();
            app.UseGlobalizationConfig();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                
                endpoints.MapHub<NotificationHub>("/hubs");
                endpoints.MapControllers().RequireAuthorization();
            });
        }

        private void ConfigureMvcOptions(MvcOptions mvcOptions)
        {
            mvcOptions.ModelBinderProviders.Insert(0, new DecimalModelBinderProvider());
        }

    }
}
