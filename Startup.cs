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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using MathGame.Controllers;
using MathGame.Services.Interfaces;
using MathGame.Services;
using MathGame.Token;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System;

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
            //services.AddSession();
            services.AddSession(options =>
            {
                options.Cookie.IsEssential = true;
                options.IdleTimeout = TimeSpan.FromMinutes(30);
            });
            services.AddMemoryCache();
            services.AddSignalR();

            services.AddTransient<IContext, Context>();
            services.AddScoped<IJWTGenerator, JWTGenerator>();

            services.AddScoped<IActivePlayersService, ActivePlayersService>();
            services.AddHttpClient<ActivePlayersService>();
            services.AddScoped<IGameRoundsInformation, GameRoundsInformation>();
            services.AddScoped<IGameRoundsRepository, GameRoundsRepository>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSignalR();


            //services.AddScoped<IRoomExpressionGeneratorScheduler, RoomExpressionGeneratorScheduler>();

            services.AddDbContext<MathGameDbContext>(
                options => options.UseSqlServer("name=ConnectionStrings:gameDbConnectionString"));

            services.AddIdentity<IdentityUser, IdentityRole>(
                opt => {
                    opt.Password.RequireDigit = false;
                    opt.Password.RequireLowercase = false;
                    opt.Password.RequireUppercase = false;
                    opt.Password.RequireNonAlphanumeric = false;
                    opt.Password.RequiredLength = 2;

                    opt.User.RequireUniqueEmail = true;
                }).AddEntityFrameworkStores<MathGameDbContext>();

            services.AddAuthentication(cfg =>
            {
                cfg.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                cfg.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {

                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Token:Key"])),
                    ValidIssuer = Configuration["Token:Issuer"],
                    ValidAudience = Configuration["Jwt:Audience"],
                    ValidateIssuer = true,
                    ValidateAudience = false,
                };
            });
            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //    .AddJwtBearer(options =>
            //    {
            //        options.TokenValidationParameters = new TokenValidationParameters
            //        {
            //            ValidateIssuer = true,
            //            ValidateAudience = true,
            //            ValidateLifetime = true,
            //            ValidateIssuerSigningKey = true,
            //            ValidIssuer = Configuration["Jwt:Issuer"],
            //            ValidAudience = Configuration["Jwt:Audience"],
            //            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
            //        };
            //    });

            services.AddAuthorization();
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

            app.UseAuthentication();
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
