using MD.IdentityUserSystem.Context;
using MD.IdentityUserSystem.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using MD.IdentityUserSystem.CustomError;

namespace MD.IdentityUserSystem
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940

        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentity<AppUser, AppRole>(opt =>
            {
                /*
                opt.Password.RequireNonAlphanumeric=false, @#$% sifrede olma mecburiyyetini levg edir
                opt.Password.RequireDigit=false, reqemlerin sifrede olma mecburiyyetini levg edir
                opt.Password.RequiredLength=3, Buradan sifrenin uzunlugunu mueyyen ede bilerik. Default olaraq 6 gelir. Burada 3 verilib.
                opt.Password.RequireLowercase=false, Sirede kicik herf mecburiyyetini aradan qaldirir
                opt.Password.RequireUppercase=false, Sirede boyuk herf mecburiyyetini aradan qaldirir
                opt.SignIn.RequireConfirmedEmail = true; Mail dogrulamasi default olaraq passive(false) gelir. Bu yolla aktiv(true) ede bilerik
                */
                opt.Lockout.MaxFailedAccessAttempts = 3;
                

            }).AddErrorDescriber<CustomErrorDescriber>().AddEntityFrameworkStores<MDContext>();
            services.AddDbContext<MDContext>(opt =>
            {
                opt.UseSqlServer(Configuration.GetConnectionString("LocalDb"));
            });

            services.ConfigureApplicationCookie(option =>
            {
                option.Cookie.Name = "Murad_Net";
                //HttpOnly nədir?;
                //Yuxarıdakı nümunədə Server tərəfindən Set - Cookie - ni təyin edərkən HttpOnly bayrağını da görürük, bu nə edir?
                //Bu Bayraqdan istifadə edərək server brauzerə JavaScript vasitəsilə kukiyə girişə icazə verməməyi bildirir.Cookie JavaScript sayəsində oğurlana bilər, çünki JavaScript kodları XSS ​​hücumunda icra edilə bilər. HttpOnly sayəsində JavaScript kodlarının Cookie məlumatlarını oxumasına icazə vermir, XSS hücumundan qorunur.Başqasının kukisi ələ keçirilərsə, Təcavüzkar Sessiya zamanı kuki məlumatının ələ keçirildiyi şəxs kimi çıxış edə bilər(bax: Sessiyanın oğurlanması).
                option.Cookie.HttpOnly = true;
                //Eğer kritik bir cookie’yi (authentication token, session id gibi) Same Site olarak işaretlerseniz, tarayıcınız bunu sadece kendi websitesinden giden isteklerde POST ediyor. A sitesinden B sitesine giden isteklerde, bu cookie yokmuş gibi davranıyor.
                //Lax - The cookie will be sent with "same-site" requests, and with "cross-site" top level navigation.
                //None - The cookie will be sent with all requests (see remarks).
                //Strict - When the value is Strict the cookie will only be sent along with "same-site" requests.
                option.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Strict;
                //Always - Secure is always marked true. Use this value when your login page and all subsequent pages requiring the authenticated identity are HTTPS. Local development will also need to be done with HTTPS urls.
                //None - Secure is not marked true. Use this value when your login page is HTTPS, but other pages on the site which are HTTP also require authentication information. This setting is not recommended because the authentication information provided with an HTTP request may be observed and used by other computers on your local network or wireless connection.
                //SameAsRequest	 - If the URI that provides the cookie is HTTPS, then the cookie will only be returned to the server on subsequent HTTPS requests. Otherwise if the URI that provides the cookie is HTTP, then the cookie will be returned to the server on all HTTP and HTTPS requests. This value ensures HTTPS for all authenticated requests on deployed servers, and also supports HTTP for localhost development and for servers that do not have HTTPS support.
                option.Cookie.SecurePolicy = Microsoft.AspNetCore.Http.CookieSecurePolicy.SameAsRequest;
                //Coockie-nin həyatda qalma müddəti
                option.ExpireTimeSpan = TimeSpan.FromDays(30);
                option.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Home/SignIn");
                option.LogoutPath = new Microsoft.AspNetCore.Http.PathString("/Home/LogOut");
                option.AccessDeniedPath = new Microsoft.AspNetCore.Http.PathString("/Home/AccessDenied");//İcazəsiz giriş zamanı yönləndirdiyi səhifə


            });
            services.AddControllersWithViews();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                RequestPath = "/node_modules",
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "node_modules"))
            });
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
