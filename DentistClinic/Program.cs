using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Usermanagement.Seeds;
using DentistClinic.Core.Models;
using DentistClinic.Data.Context;
using DentistClinic.Services.Interfaces;
using DentistClinic.Services.Repository;

namespace DentistClinic
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseLazyLoadingProxies()
                .UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.User.RequireUniqueEmail = true;

            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultUI()
                .AddDefaultTokenProviders();

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                options.Cookie.Name = "YourAppCookieName";
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                options.LoginPath = "/Identity/Account/Login";
                // ReturnUrlParameter requires 
                //using Microsoft.AspNetCore.Authentication.Cookies;
                options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
                options.SlidingExpiration = true; //renew expirationTime if i take any action
            });

           

            builder.Services.AddScoped<IPatientRepository, PatientRepository>();
            builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
            builder.Services.AddScoped<IMedicineRepository, MedicineRepository>();
            builder.Services.AddScoped<IXrayRepository, XrayRepository>();
            builder.Services.AddScoped<IAnalysisRepository, AnalysisRepository>();
            builder.Services.AddScoped<IContactRepository, ContactRepository>();
            builder.Services.AddScoped<IPaymentsRepository, PaymentsRepository>();
            builder.Services.AddScoped<IMedicalHistoryRepository, MedicalHistoryRepository>();
            builder.Services.AddScoped<IMedicalHistoryImageRepository, MedicalHistoryImageRepository>();
            builder.Services.AddScoped<ITreatmentPlansRepository, TreatmentPlansRepository>();
            builder.Services.AddScoped<IToothRepository, ToothRepository>();
            builder.Services.AddScoped<IPrescriptionRepository, PrescriptionRepository>();
            builder.Services.AddScoped<IMedicinePrescriptionRepository, MedicinePrescriptionRepository>();
            builder.Services.AddScoped<IAnalysisPrescriptionRepository, AnalysisPrescriptionRepository>();
            builder.Services.AddScoped<IXraysPrescriptionRepository, XraysPrescriptionRepository>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            IServiceScopeFactory scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
            using IServiceScope scope = scopeFactory.CreateScope();

            var roleManger = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManger = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            await DefaultRoles.SeedingRolesAsync(roleManger);
            await DefaultAdmin.SeedingAdminAsync(userManger, context);

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapRazorPages();
            app.Run();
        }
    }
}
