using AutoMapper;
using AvatarTourSystem_BE.JsonPolicies;
using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using Repositories;
using Repositories.Interfaces;
using Repositories.Repositories;
using Services.Common;
using Services.Interfaces;
using Services.Services;
using System.Security.AccessControl;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//WORKFLOW test 4
builder.Services.AddControllers()
    .AddJsonOptions(options => { options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = new KebabCaseNamingPolicy();
        options.JsonSerializerOptions.DictionaryKeyPolicy = new KebabCaseNamingPolicy();
    });

builder.Services.AddDbContext<AvatarTourDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("AvatarTourSystem"));
});

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<ISupplierService, SupplierService>();
builder.Services.AddScoped<ISupplierRepository, SupplierRepository>();

builder.Services.AddScoped<ITourSegmentService, TourSegmentService>();
builder.Services.AddScoped<ITourSegmentRepository, TourSegmentRepository>();

builder.Services.AddScoped<IPackageTourService, PackageTourService>();
builder.Services.AddScoped<IPackageTourRepository, PackageTourRepository>();

builder.Services.AddScoped<ITicketTypeService, TicketTypeService>();
builder.Services.AddScoped<ITicketTypeRepository, TicketTypeRepository>();

builder.Services.AddScoped<IDailyTicketService, DailyTicketService>();
builder.Services.AddScoped<IDailyTicketRepository, DailyTicketRepository>();

builder.Services.AddScoped<IPaymentMethodService, PaymentMethodService>();
builder.Services.AddScoped<IPaymentMethodRepository, PaymentMethodRepository>();

builder.Services.AddScoped<ITicketService, TicketService>();
builder.Services.AddScoped<ITicketRepository, TicketRepositor>();

builder.Services.AddScoped<ITransactionsHistoryService, TransactionHistoryService>();
builder.Services.AddScoped<ITransactionHistoryRepository, TransactionHistoryRepository>();

builder.Services.AddScoped<ICityRepository, CityRepository>();
builder.Services.AddScoped<ICityService, CityService>();

builder.Services.AddScoped<IDestinationRepository, DestinationRepository>();
builder.Services.AddScoped<IDestinationService, DestinationService>();

builder.Services.AddScoped<ILocationRepository, LocationRepository>();
builder.Services.AddScoped<ILocationService, LocationService>();

builder.Services.AddScoped<IPOITypeRepository, POITypeRepository>();
builder.Services.AddScoped<IPOITypeService, POITypeService>();

builder.Services.AddScoped<IPointOfInterestRepository, PointOfInterestRepository>();
builder.Services.AddScoped<IPointOfInterestService, PointOfInterestService>();

builder.Services.AddScoped<IServiceRepository, ServiceRepository>();
builder.Services.AddScoped<IServiceService, ServiceService>();

builder.Services.AddScoped<IServiceTypeRepository, ServiceTypeRepository>();
builder.Services.AddScoped<IServiceTypeService, ServiceTypeService>();

builder.Services.AddScoped<IServiceUsedByTicketRepository, ServiceUsedByTicketRepository>();
builder.Services.AddScoped<IServiceUsedByTicketService, ServiceUsedByTicketService>();

builder.Services.AddScoped<IServiceByTourSegmentRepository, ServiceByTourSegmentRepository>();
builder.Services.AddScoped<IServiceByTourSegmentService, ServiceByTourSegmentService>();

builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IBookingService, BookingService>();

builder.Services.AddScoped<IRevenueRepository, RevenueRepository>();
builder.Services.AddScoped<IRevenueService, RevenueService>();

builder.Services.AddScoped<IBookingByRevenueRepository, BookingByRevenueRepository>();
builder.Services.AddScoped<IBookingByRevenueService, BookingByRevenueService>();

builder.Services.AddScoped<IFeedbackService, FeedbackService>();
builder.Services.AddScoped<IFeedbackRepository, FeedbackRepository>();

builder.Services.AddScoped<IRateService, RateService>();
builder.Services.AddScoped<IRateRepository, RateRepository>();

builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();

builder.Services.AddScoped<ICustomerSupportService, CustomerSupportService>();
builder.Services.AddScoped<ICustomerSupportRepository, CustomerSupportRepository>();

builder.Services.AddScoped<IRequestTypeService, RequestTypeService>();
builder.Services.AddScoped<IRequestTypeRepository, RequestTypeRepository>();

builder.Services.AddScoped<INotificatonService, NotificationService>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();

builder.Services.AddScoped<IDailyTourService, DailyTourSerivce>();
builder.Services.AddScoped<IDailyTourRepository, DailyTourRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
