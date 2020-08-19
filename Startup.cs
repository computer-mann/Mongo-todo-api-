using System.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using ToDoApi.Auth.Models;
using ToDoApi.Auth.Models.DbContexts;
using ToDoApi.Infrastructure.Interfaces;
using ToDoApi.Infrastructure.Settings;
using ToDoApi.Services;

namespace ToDoApi
{
    public class Startup
    {
        public Startup (IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        const string ToDoApiCorsPolicy = "ToDoApiCorsPolicy";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AuthDbContext>(options=>{
                options.UseSqlite("Data Source=todoapi.db");
            });

            services.AddIdentityCore<AppUser>().AddEntityFrameworkStores<AuthDbContext>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options=>{
                    options.TokenValidationParameters=new TokenValidationParameters()
                    {
                        ValidateIssuer=true,
                        ValidateAudience=true,
                        ValidateIssuerSigningKey=true,
                        ValidateLifetime=true,
                        ValidAudience=Configuration["JwtSettings:Audience"],
                        ValidIssuer=Configuration["JwtSettings:Issuer"],
                        IssuerSigningKey=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtSettings:SignInKey"]))
                    };
                });
            services.AddAuthorization();

            services.Configure<MongoDbSettings>(Configuration.GetSection("MongoDbSettings"));
            services.Configure<JwtSettings>(Configuration.GetSection("JwtSettings"));
            
            services.AddSingleton<ITodoService, ToDoService> ();
            services.AddTransient<JwtSettings>();

            services.AddCors(options =>
            {
                options.AddPolicy(ToDoApiCorsPolicy,policy
                    =>policy.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader());
            });

            services.AddControllers ();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure (IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment ())
            {
                app.UseDeveloperExceptionPage ();
            }

            app.UseHttpsRedirection ();
            app.UseCors(ToDoApiCorsPolicy);
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseRouting();

            app.UseAuthorization ();

            app.UseEndpoints (endpoints =>
            {
                endpoints.MapControllers ();
            });
        }
    }
}