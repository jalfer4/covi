using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Covi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace Covi
{
    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Home/Login";
                    options.LogoutPath = "/Home/Logout";
                    options.AccessDeniedPath = "/Home/Restringido";
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["TokenAuthentication:Issuer"],
                        ValidAudience = configuration["TokenAuthentication:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(configuration["TokenAuthentication:SecretKey"])),
                    };
                });
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Administrador", policy => policy.RequireClaim(ClaimTypes.Role, "Administrador"));
            });

            services.AddDbContext<CoviContext>(options => options.UseSqlServer(configuration["ConnectionStrings:DefaultConnection"]));

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddTransient<IRepositorio<TipoUsuario>, RepositorioTipoUsuario>();
            services.AddTransient<IRepositorioTipoUsuario, RepositorioTipoUsuario>();

            services.AddTransient<IRepositorio<TipoLocal>, RepositorioTipoLocal>();
            services.AddTransient<IRepositorioTipoLocal, RepositorioTipoLocal>();

            services.AddTransient<IRepositorio<TipoEvento>, RepositorioTipoEvento>();
            services.AddTransient<IRepositorioTipoEvento, RepositorioTipoEvento>();

            services.AddTransient<IRepositorio<Reserva>, RepositorioReserva>();
            services.AddTransient<IRepositorioReserva, RepositorioReserva>();

            services.AddTransient<IRepositorio<Local>, RepositorioLocal>();
            services.AddTransient<IRepositorioLocal, RepositorioLocal>();

            services.AddTransient<IRepositorio<Evento>, RepositorioEvento>();
            services.AddTransient<IRepositorioEvento, RepositorioEvento>();

            services.AddTransient<IRepositorio<Usuario>, RepositorioUsuario>();
            services.AddTransient<IRepositorioUsuario, RepositorioUsuario>();

            //services.Configure<CookiePolicyOptions>(options =>
            //{
            //    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
            //    options.CheckConsentNeeded = context => true;
            //    options.MinimumSameSitePolicy = SameSiteMode.None;
            //});

            //services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            //   .AddCookie(options =>
            //   {
            //       options.LoginPath = "/Home/Login";
            //       options.LogoutPath = "/Home/Logout";
            //       options.AccessDeniedPath = "/Home/Restringido";
            //   })
            //   .AddJwtBearer(options =>
            //   {
            //       options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
            //       {
            //           ValidateIssuer = true,
            //           ValidateAudience = true,
            //           ValidateLifetime = true,
            //           ValidateIssuerSigningKey = true,
            //           ValidIssuer = configuration["TokenAuthentication:Issuer"],
            //           ValidAudience = configuration["TokenAuthentication:Audience"],
            //           IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(configuration["TokenAuthentication:SecretKey"])),
            //       };
            //   });
            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("Administrador", policy => policy.RequireClaim(ClaimTypes.Role, "Administrador"));
            //});


            //services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            //  var clave = configuration["ConnectionStrings:DefaultConnection"];

            ////services.AddDbContext<CoviContext>(options => options.UseSqlServer("Data Source=192.168.65.59;Initial Catalog=Covi;User ID=AdminCovi;Password=1q2w3e4r%T"));


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // Habilitar CORS
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
            // Uso de archivos estáticos (*.html, *.css, *.js, etc.)
            app.UseStaticFiles();
            // Permitir cookies
            app.UseCookiePolicy(new CookiePolicyOptions
            {
                MinimumSameSitePolicy = SameSiteMode.None,
            });
            // Habilitar autenticación
            app.UseAuthentication();
            // App en ambiente de desarrollo?
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();//página amarilla de errores
            }
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "login",
                    template: "login/{**accion}",
                    defaults: new { controller = "Home", action = "Login" });
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            //////    if (env.IsDevelopment())
            //////    {
            //////        app.UseDeveloperExceptionPage();
            //////    }
            //////    else
            //////    {
            //////        app.UseExceptionHandler("/Home/Error");
            //////        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            //////        app.UseHsts();
            //////    }

            //////    app.UseHttpsRedirection();
            //////    app.UseStaticFiles();
            //////    app.UseCookiePolicy();

            //////    app.UseMvc(routes =>
            //////    {
            //////        routes.MapRoute(
            //////            name: "default",
            //////            template: "{controller=Home}/{action=Index}/{id?}");
            //////    });
        }
    }
}
