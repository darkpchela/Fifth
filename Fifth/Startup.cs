using Fifth.Extensions;
using Fifth.Interfaces;
using Fifth.Services;
using Fifth.Services.BasicCRUD;
using Fifth.Services.DataContext;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Fifth
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options => options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Account/SignIn"));
            services.AddHttpContextAccessor();
            services.AddDistributedMemoryCache();
            services.AddSession();
            services.AddDbContext<AppDbContext>(o => o.UseSqlServer(Configuration.GetConnectionString("Default")));
            services.AddSingleton<IGameInstanceRepository, GameInstanceRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IAppAuthenticationService, AppAuthenticationService>();
            services.AddTransient<ITagCrudService, TagCrudService>();
            services.AddTransient<IUserCrudService, UserCrudService>();
            services.AddTransient<IGamesCrudService, GamesCrudService>();
            services.AddTransient<IGamesManager, GamesManager>();
            services.AddTransient<ISessionTagCrudService, SessionTagCrudService>();
            services.AddTransient<ITagCrudService, TagCrudService>();
            services.AddAutomapperProfiles();
            services.AddSignalR();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseSession();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapFallbackToController("Index", "Home");
                endpoints.MapHub<MainHub>("/MainHub");
                endpoints.MapHub<GameHub>("/GameHub");
            });
        }
    }
}