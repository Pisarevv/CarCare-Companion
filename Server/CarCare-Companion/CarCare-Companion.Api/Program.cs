using System.Text;

using System.Reflection;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

using Amazon.S3;

using Quartz;

using Serilog;

using CarCare_Companion.Core.Contracts;
using CarCare_Companion.Core.Services;
using CarCare_Companion.Infrastructure.Data;
using CarCare_Companion.Infrastructure.Data.Common;
using CarCare_Companion.Infrastructure.Data.Models.Identity;
using CarCare_Companion.Infrastructure.Data.Seeding;
using CarCare_Companion.Api.Extensions.Quartz;
using CarCare_Companion.Api.Extensions.Mail;
using CarCare_Companion.Core.Messaging;

namespace CarCare_Companion.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .CreateLogger();

            builder.Host.UseSerilog();
            ConfigureServices(builder.Services, builder.Configuration);

            try
            {
                var app = builder.Build();
                Log.Information("Application starting up.");
                Configure(app);
                await app.RunAsync();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "The application failed to start correctly.");
                throw;
            }
            finally 
            {
                await Log.CloseAndFlushAsync();
            }
             

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

            //Add memory cache
            services.AddMemoryCache();

            //Adding AWS services
            services.AddDefaultAWSOptions(configuration.GetAWSOptions());
            services.AddAWSService<IAmazonS3>();

            //Add Quartz scheduler
            services.AddQuartz();

            //Add mail options
            services.Configure<MailSettings>(configuration.GetSection("MailSettings"));

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
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!)),
                        ClockSkew = TimeSpan.Zero

                    };
                });

            services.AddScoped<IRepository, Repository>();
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<IAdService, AdService>();
            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<IVehicleService, VehicleService>();
            services.AddScoped<ITripRecordsService, TripRecordsService>();
            services.AddScoped<IServiceRecordsService, ServiceRecordsService>();
            services.AddScoped<ITaxRecordsService, TaxRecordsService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IEmailSender, GmailEmailSender>();
            services.AddScoped<IMessagingService, MessagingService>();
            services.AddScoped<IJWTService, JWTService>();
            services.AddScoped<IRefreshTokenService,RefreshTokenService>();

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                                   policy =>
                                  {
                                      policy.WithOrigins("http://localhost:5173",
                                                         "http://172.22.144.1:5173")
                                      .AllowAnyMethod()
                                      .AllowAnyHeader()
                                      .AllowCredentials();
                                  });
            });


            services.AddControllers();

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
            });
        }

        private static void Configure(WebApplication app)
        {

            // Seed data on application startup
            using (var serviceScope = app.Services.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<CarCareCompanionDbContext>();
                new CarCareCompanionDbContextSeeder().SeedAsync(dbContext, serviceScope.ServiceProvider).GetAwaiter().GetResult();
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseRouting();

            app.UseCors();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseSerilogRequestLogging();

          

            app.UseAuthentication(); 
            app.UseAuthorization();

            app.UseEndpoints(endpoints => 
            { 
                endpoints.MapControllers(); 
            });

           

        }

       
    }
}