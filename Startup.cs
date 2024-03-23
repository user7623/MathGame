using MathGame.BAL;
using MathGame.BAL.Interfaces;
using MathGame.Repositories;
using MathGame.DataAccess;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OnlineUsers;
using MathGame.Repositories.Interfaces;

namespace MathGame
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddSession();
            services.AddMemoryCache();
            services.AddSignalR();

            services.AddTransient<IContext, Context>();

            services.AddScoped<IGameRoundsInformation, GameRoundsInformation>();
            services.AddScoped<IGameRoundsRepository, GameRoundsRepository>();

            services.AddDbContext<MathGameDbContext>(
                options => options.UseSqlServer("name=ConnectionStrings:gameDbConnectionString"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            //might not be needed
            app.UseOnlineUsers();

            app.UseRouting();

            app.UseAuthorization();

            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapHub<OnlineUsersHub>("/onlineUsersHub");
            });
        }
    }
}
