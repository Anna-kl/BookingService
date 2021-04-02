using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Application.Helpers.Authentication;
using Application.Helpers.Responce;
using BookingServices.BookingServices.Account.AccountServices;
using BookingServices.Helpers.ImageWorker;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using ServicesModel.Context;
using ServicesModel.Models.Auth;

namespace BookingServices
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson(options =>
     options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
 );
            services.AddCors(o => o.AddPolicy("CorsPolicy", builder =>
            {
                builder.
                AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
            }));
            services.AddScoped<IAutentication, AuthenticationRepository>();
            //services.AddCors(o => o.AddPolicy(MyAllowSpecificOrigins, builder =>
            //{
            //    builder.
            //    AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().WithOrigins("http://localhost:4200",
            //    "http://ocpio.com/");
            //}));


            services.AddTransient<IImageWriter,
                                  ImageWriter>();
            services.AddTransient<IImageReader,
                                  ImageReader>();
            services.AddTransient<IImageHandler, ImageHandler>();
            services.AddScoped<IResponce, ResponeRepository>();
            services.AddScoped<IAccount, AccountRepository>();
            services.AddDbContext<ServicesContext>(options =>
       options.UseNpgsql("Host=185.220.35.179;Username=postgres;Password=2537300;Database=postgres;Port=5432",
       b => b.MigrationsAssembly("ServicesModel"))
     // options.UseNpgsql("Host=localhost;Username=postgres;Password=2537300;Database=postgres;Port=5432",
     //b => b.MigrationsAssembly("ServicesModel"))
     //   options.UseNpgsql(settings.DBASE, b => b.MigrationsAssembly("ApplicationBase"))
     );
            //services.AddIdentity<Auth, IdentityRole<int>>(options=>
            //{
            //    options.ClaimsIdentity.UserIdClaimType = "UserID";
            //})
            //    .AddEntityFrameworkStores<ServicesContext>().AddDefaultTokenProviders();
            services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
           .AddJwtBearer(x =>
           {
               x.RequireHttpsMetadata = false;
               x.SaveToken = true;
               x.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidateIssuerSigningKey = true,
                   IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("key to some_big_key_value_here_secret")),
                   ValidateIssuer = false,
                   ValidateAudience = false
               };
           });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseHttpsRedirection();
          

            app.UseCors("CorsPolicy");
            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
