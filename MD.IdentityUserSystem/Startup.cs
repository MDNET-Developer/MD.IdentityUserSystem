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
                */
            }).AddEntityFrameworkStores<MDContext>();
            services.AddDbContext<MDContext>(opt =>
            {
                opt.UseSqlServer(Configuration.GetConnectionString("LocalDb"));
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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
