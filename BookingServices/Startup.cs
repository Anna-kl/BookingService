using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
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
using Microsoft.Extensions.PlatformAbstractions;
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

            services.AddSwaggerGen(c =>
            {
                c.IncludeXmlComments(XmlCommentsFilePath);
                c.IncludeXmlComments(XmlCommentsFileClass);
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Заказ услуг",
                    Version = "v1",
                    Description = "Описание API для предоставления сервисов",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact
                    {
                        Name = "Климова Анна",
                        Email = "klimova_88@mail.ru",
                        Url = new Uri("http://ocpio.com"),
                    }
                });
            });
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

            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Shedule Services API V1");

                // To serve SwaggerUI at application's root page, set the RoutePrefix property to an empty string.
                c.RoutePrefix = string.Empty;
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        static string XmlCommentsFilePath
        {
            get
            {
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var fileName = typeof(Startup).GetTypeInfo().Assembly.GetName().Name + ".xml";
                return Path.Combine(basePath, fileName);
            }
        }
        static string XmlCommentsFileClass
        {
            get
            {
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var fileName = "ServicesModel" + ".xml";
                return Path.Combine(basePath, fileName);
            }
        }
    }
}
