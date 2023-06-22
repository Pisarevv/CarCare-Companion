using CarCare_Companion.Core.Contracts;
using CarCare_Companion.Core.Services;
using CarCare_Companion.Infrastructure.Data;
using CarCare_Companion.Infrastructure.Data.Models.Identity;
using CarCare_Companion.Infrastructure.Data.Seeding;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace CarCare_Companion.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            ConfigureServices(builder.Services, builder.Configuration);
            var app = builder.Build();
            Configure(app);
            app.Run();     

        }

        private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            services.AddDbContext<CarCareCompanionDbContext>(options =>
                options.UseSqlServer(connectionString));
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddDefaultIdentity<ApplicationUser>()
                .AddRoles<ApplicationRole>()
                .AddEntityFrameworkStores<CarCareCompanionDbContext>();

            services.Configure<IdentityOptions>(options =>
            {
                options.SignIn.RequireConfirmedEmail = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidAudience = configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))

                    };
                });

            services.AddScoped<IIdentityService, IdentityService>();

            services.AddMvc();
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    policy =>
                    {
                        policy.WithOrigins("http://localhost:5173");
                    });
            });


            services.AddControllers();

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
        }

        private static void Configure(WebApplication app)
        {

            // Seed data on application startup
            using (var serviceScope = app.Services.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<CarCareCompanionDbContext>();
                dbContext.Database.Migrate();
                new CarCareCompanionDbContextSeeder().SeedAsync(dbContext, serviceScope.ServiceProvider).GetAwaiter().GetResult();
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseRouting();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseCors();

            app.UseAuthentication(); 
            app.UseAuthorization();

            app.UseEndpoints(endpoints => 
            { 
                endpoints.MapControllers(); 
            });
 
        }
    }
}