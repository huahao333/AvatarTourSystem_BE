using AvatarTourSystem_BE.JsonPolicies;
using BusinessObjects.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Repositories;
using Repositories.Interfaces;
using Repositories.Repositories;
using Services.Interfaces;
using Services.Services;
using System;
using System.Text;

namespace AvatarTourSystem_BE
{
    static class DependencyInjection
    {
        public static void AddApiWebService(this IServiceCollection services, WebApplicationBuilder builder)
        {
            //config authen
            builder.Services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new OpenApiInfo { Title = "Artwork Sharing Platform", Version = "v.10.24" });
                opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                opt.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                   {
                     new OpenApiSecurityScheme
                     {
                         Reference = new OpenApiReference
                         {
                             Type=ReferenceType.SecurityScheme,
                             Id="Bearer"
                          }
                     },
                     new string[]{}
                     // Array.Empty<string>()
                   }
                });
            });


            builder.Services
                .AddIdentity<Account, IdentityRole>()
                .AddEntityFrameworkStores<AvatarTourDBContext>()
                .AddDefaultTokenProviders();


            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<ISupplierService, SupplierService>();
            services.AddScoped<ISupplierRepository, SupplierRepository>();

            services.AddScoped<ITourSegmentService, TourSegmentService>();
            services.AddScoped<ITourSegmentRepository, TourSegmentRepository>();

            services.AddScoped<IPackageTourService, PackageTourService>();
            services.AddScoped<IPackageTourRepository, PackageTourRepository>();

            services.AddScoped<ITicketTypeService, TicketTypeService>();
            services.AddScoped<ITicketTypeRepository, TicketTypeRepository>();

            services.AddScoped<IDailyTicketService, DailyTicketService>();
            services.AddScoped<IDailyTicketRepository, DailyTicketRepository>();

            services.AddScoped<IPaymentMethodService, PaymentMethodService>();
            services.AddScoped<IPaymentMethodRepository, PaymentMethodRepository>();

            services.AddScoped<ITicketService, TicketService>();
            services.AddScoped<ITicketRepository, TicketRepositor>();

            services.AddScoped<ITransactionsHistoryService, TransactionHistoryService>();
            services.AddScoped<ITransactionHistoryRepository, TransactionHistoryRepository>();

            services.AddScoped<ICityRepository, CityRepository>();
            services.AddScoped<ICityService, CityService>();

            services.AddScoped<IDestinationRepository, DestinationRepository>();
            services.AddScoped<IDestinationService, DestinationService>();

            services.AddScoped<ILocationRepository, LocationRepository>();
            services.AddScoped<ILocationService, LocationService>();

            services.AddScoped<IPOITypeRepository, POITypeRepository>();
            services.AddScoped<IPOITypeService, POITypeService>();

            services.AddScoped<IPointOfInterestRepository, PointOfInterestRepository>();
            services.AddScoped<IPointOfInterestService, PointOfInterestService>();

            services.AddScoped<IServiceRepository, ServiceRepository>();
            services.AddScoped<IServiceService, ServiceService>();

            services.AddScoped<IServiceTypeRepository, ServiceTypeRepository>();
            services.AddScoped<IServiceTypeService, ServiceTypeService>();

            services.AddScoped<IServiceUsedByTicketRepository, ServiceUsedByTicketRepository>();
            services.AddScoped<IServiceUsedByTicketService, ServiceUsedByTicketService>();

            services.AddScoped<IServiceByTourSegmentRepository, ServiceByTourSegmentRepository>();
            services.AddScoped<IServiceByTourSegmentService, ServiceByTourSegmentService>();

            services.AddScoped<IBookingRepository, BookingRepository>();
            services.AddScoped<IBookingService, BookingService>();

            services.AddScoped<IRevenueRepository, RevenueRepository>();
            services.AddScoped<IRevenueService, RevenueService>();

            services.AddScoped<IBookingByRevenueRepository, BookingByRevenueRepository>();
            services.AddScoped<IBookingByRevenueService, BookingByRevenueService>();

            services.AddScoped<IFeedbackService, FeedbackService>();
            services.AddScoped<IFeedbackRepository, FeedbackRepository>();

            services.AddScoped<IRateService, RateService>();
            services.AddScoped<IRateRepository, RateRepository>();

            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IAccountRepository, AccountRepository>();

            services.AddScoped<ICustomerSupportService, CustomerSupportService>();
            services.AddScoped<ICustomerSupportRepository, CustomerSupportRepository>();

            services.AddScoped<IRequestTypeService, RequestTypeService>();
            services.AddScoped<IRequestTypeRepository, RequestTypeRepository>();

            services.AddScoped<INotificatonService, NotificationService>();
            services.AddScoped<INotificationRepository, NotificationRepository>();

            services.AddScoped<IDailyTourService, DailyTourSerivce>();
            services.AddScoped<IDailyTourRepository, DailyTourRepository>();

            services.AddScoped<IDailyTourFlowService, DailyTourFlowService>();
            services.AddScoped<IDailyTourFlowRepository, DailyTourFlowRepository>();

            services.AddScoped<IPackageTourFlowService,  PackageTourFlowService>();

            services.AddScoped<IDashboardService, DashboardService>();

            //auto mapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


            //services.AddIdentity<Account, IdentityRole>()
            //    .AddEntityFrameworkStores<AvatarTourDBContext>()
            //    .AddDefaultTokenProviders();

            // kebab
            services.AddControllers().AddJsonOptions(options =>
            {
               // options.JsonSerializerOptions.PropertyNamingPolicy = new KebabCaseNamingPolicy();
               // options.JsonSerializerOptions.DictionaryKeyPolicy = new KebabCaseNamingPolicy();
            });

            //DBcontext
            builder.Services.AddDbContext<AvatarTourDBContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("AvatarTourSystem"));
            });


            // Add Authentication and JwtBearer
            builder.Services
                .AddAuthentication(options =>
                {
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
                        ValidAudience = builder.Configuration["JWT:ValidAudience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:SecretKey"]))
                    };
                });

        }
    }
}
