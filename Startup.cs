using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using mvc.Data;
using System.Text;
using Swashbuckle.AspNetCore.Filters;
using System;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace mvc
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
            services.AddSingleton(Configuration);
            services.AddDbContext<ApplicationDbContext>(option => option.UseSqlServer(Configuration.GetConnectionString("DockerConnection")));
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Implement Swagger UI",
                    Description = "Desc SImple"

                });


                c.OperationFilter<SecurityRequirementsOperationFilter>();
            });
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(opt => 
                {
                    opt.Cookie.Name ="Test";
                    opt.LoginPath = "/Auth/Login";
                    opt.ExpireTimeSpan = TimeSpan.FromHours(1);
                    opt.LogoutPath = "/Auth/Login";
                    opt.AccessDeniedPath = "/Auth/Denied";
                    opt.SlidingExpiration = true;
                });
            services.AddAuthorization(opt =>
            {
                opt.AddPolicy("Admin", policy=>
                    policy.RequireClaim(ClaimTypes.Role, "admin")
                );
                opt.AddPolicy("User", policy=>
                    policy.RequireClaim(ClaimTypes.Role, "user")
                );
            });
            
            
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
                app.UseExceptionHandler("/Auth/Login");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Showing API V1");
            });
            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Book}/{action=Index}/{id?}");
            });
            app.UseStaticFiles();
        }
    }
}
