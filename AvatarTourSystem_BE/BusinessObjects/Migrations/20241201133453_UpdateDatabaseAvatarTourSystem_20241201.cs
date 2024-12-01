using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessObjects.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDatabaseAvatarTourSystem_20241201 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Gender = table.Column<bool>(type: "bit", nullable: true),
                    Dob = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AvatarUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ZaloUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefreshTokenExpiryTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true),
                    Roles = table.Column<int>(type: "int", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "City",
                columns: table => new
                {
                    CityId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CityName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "date", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "date", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Booking__551479477F27FEF4", x => x.CityId);
                });

            migrationBuilder.CreateTable(
                name: "PaymentMethod",
                columns: table => new
                {
                    PaymentMethodId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PaymentType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "date", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "date", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__PaymentMethod__539279467C17FFG3", x => x.PaymentMethodId);
                });

            migrationBuilder.CreateTable(
                name: "RequestType",
                columns: table => new
                {
                    RequestTypeId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Priority = table.Column<int>(type: "int", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "date", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "date", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__RequestType__511372221B17FFC3", x => x.RequestTypeId);
                });

            migrationBuilder.CreateTable(
                name: "ServiceType",
                columns: table => new
                {
                    ServiceTypeId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ServiceTypeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "date", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "date", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ServiceType__511379427A17CGF4", x => x.ServiceTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Supplier",
                columns: table => new
                {
                    SupplierId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SupplierName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "date", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "date", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Supplier__512219427A17CGF4", x => x.SupplierId);
                });

            migrationBuilder.CreateTable(
                name: "Notification",
                columns: table => new
                {
                    NotifyId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    SendDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "date", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "date", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Notification__531279467C17FFE2", x => x.NotifyId);
                    table.ForeignKey(
                        name: "FK__Notification__UserId__3A81F117",
                        column: x => x.UserId,
                        principalTable: "Account",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Destination",
                columns: table => new
                {
                    DestinationId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CityId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    DestinationName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DestinationAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DestinationImgUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DestinationHotline = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DestinationGoogleMap = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DestinationOpeningDate = table.Column<int>(type: "int", nullable: true),
                    DestinationClosingDate = table.Column<int>(type: "int", nullable: true),
                    DestinationOpeningHours = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DestinationClosingHours = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "date", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "date", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Destination__551479467C17FEF7", x => x.DestinationId);
                    table.ForeignKey(
                        name: "FK__Destination__CityId__3A81G227",
                        column: x => x.CityId,
                        principalTable: "City",
                        principalColumn: "CityId");
                });

            migrationBuilder.CreateTable(
                name: "PackageTour",
                columns: table => new
                {
                    PackageTourId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CityId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    PackageTourName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PackageTourImgUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "date", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "date", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__PackageTour__531279467C17FFG3", x => x.PackageTourId);
                    table.ForeignKey(
                        name: "FK__PackageTour__CityId__3C81F217",
                        column: x => x.CityId,
                        principalTable: "City",
                        principalColumn: "CityId");
                });

            migrationBuilder.CreateTable(
                name: "CustomerSupport",
                columns: table => new
                {
                    CusSupportId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    RequestTypeId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateResolved = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "date", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "date", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__CustomerSupport__551479477C27FEF5", x => x.CusSupportId);
                    table.ForeignKey(
                        name: "FK__CustomerSupport__RequestTypeId__3A81A229",
                        column: x => x.RequestTypeId,
                        principalTable: "RequestType",
                        principalColumn: "RequestTypeId");
                    table.ForeignKey(
                        name: "FK__CustomerSupport__UserId__3A81A227",
                        column: x => x.UserId,
                        principalTable: "Account",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Location",
                columns: table => new
                {
                    LocationId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LocationName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LocationImgUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LocationHotline = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LocationAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LocationGoogleMap = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LocationOpeningHours = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LocationClosingHours = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DestinationId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "date", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "date", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Location__551279467C17FHE1", x => x.LocationId);
                    table.ForeignKey(
                        name: "FK__Location__DestinationId__3A81E127",
                        column: x => x.DestinationId,
                        principalTable: "Destination",
                        principalColumn: "DestinationId");
                });

            migrationBuilder.CreateTable(
                name: "DailyTour",
                columns: table => new
                {
                    DailyTourId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PackageTourId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    DailyTourName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DailyTourPrice = table.Column<float>(type: "real", nullable: true),
                    ImgUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "date", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Discount = table.Column<int>(type: "int", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "date", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__DailyTour__551479467C27FEF1", x => x.DailyTourId);
                    table.ForeignKey(
                        name: "FK__DailyTour__PackageTourId__3A81G267",
                        column: x => x.PackageTourId,
                        principalTable: "PackageTour",
                        principalColumn: "PackageTourId");
                });

            migrationBuilder.CreateTable(
                name: "TicketType",
                columns: table => new
                {
                    TicketTypeId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PackageTourId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TicketTypeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MinBuyTicket = table.Column<int>(type: "int", nullable: true),
                    PriceDefault = table.Column<float>(type: "real", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "date", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "date", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__TicketType__521178227A17CFF8", x => x.TicketTypeId);
                    table.ForeignKey(
                        name: "FK__TicketType__PackageTourId__3G13F297",
                        column: x => x.PackageTourId,
                        principalTable: "PackageTour",
                        principalColumn: "PackageTourId");
                });

            migrationBuilder.CreateTable(
                name: "TourSegment",
                columns: table => new
                {
                    TourSegmentId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DestinationId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    PackageTourId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "date", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "date", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__TourSegment__521178777A17CFE8", x => x.TourSegmentId);
                    table.ForeignKey(
                        name: "FK__TourSegment__DestinationId__3F33F297",
                        column: x => x.DestinationId,
                        principalTable: "Destination",
                        principalColumn: "DestinationId");
                    table.ForeignKey(
                        name: "FK__TourSegment__PackageTourId__3G13F297",
                        column: x => x.PackageTourId,
                        principalTable: "PackageTour",
                        principalColumn: "PackageTourId");
                });

            migrationBuilder.CreateTable(
                name: "PointOfInterest",
                columns: table => new
                {
                    PointId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PointName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LocationId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "date", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "date", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__PointOfInterest__511379467C17FFC3", x => x.PointId);
                    table.ForeignKey(
                        name: "FK__PointOfInterest__LocationId__3F21C417",
                        column: x => x.LocationId,
                        principalTable: "Location",
                        principalColumn: "LocationId");
                });

            migrationBuilder.CreateTable(
                name: "Service",
                columns: table => new
                {
                    ServiceId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ServiceName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ServicePrice = table.Column<float>(type: "real", nullable: true),
                    ServiceImgUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ServiceTypeId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    LocationId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    SupplierId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "date", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "date", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Service__511379427A17DFC9", x => x.ServiceId);
                    table.ForeignKey(
                        name: "FK__Service__LocationId__3D71F217",
                        column: x => x.LocationId,
                        principalTable: "Location",
                        principalColumn: "LocationId");
                    table.ForeignKey(
                        name: "FK__Service__ServiceTypeId__3B21F217",
                        column: x => x.ServiceTypeId,
                        principalTable: "ServiceType",
                        principalColumn: "ServiceTypeId");
                    table.ForeignKey(
                        name: "FK__Service__SupplierId__3D71E717",
                        column: x => x.SupplierId,
                        principalTable: "Supplier",
                        principalColumn: "SupplierId");
                });

            migrationBuilder.CreateTable(
                name: "Booking",
                columns: table => new
                {
                    BookingId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    BookingDate = table.Column<DateTime>(type: "date", nullable: true),
                    ExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DailyTourId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TotalPrice = table.Column<float>(type: "real", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "date", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "date", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Booking__551479477F27FEF3", x => x.BookingId);
                    table.ForeignKey(
                        name: "FK__Booking__DailyTourId__3A81B329",
                        column: x => x.DailyTourId,
                        principalTable: "DailyTour",
                        principalColumn: "DailyTourId");
                    table.ForeignKey(
                        name: "FK__Booking__UserId__3A81B327",
                        column: x => x.UserId,
                        principalTable: "Account",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DailyTicket",
                columns: table => new
                {
                    DailyTicketId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TicketTypeId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    DailyTourId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Capacity = table.Column<int>(type: "int", nullable: true),
                    DailyTicketPrice = table.Column<float>(type: "real", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "date", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "date", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__DailyTicket__551479467C27FEC5", x => x.DailyTicketId);
                    table.ForeignKey(
                        name: "FK__DailyTicket__DailyTourId__3A81E267",
                        column: x => x.DailyTourId,
                        principalTable: "DailyTour",
                        principalColumn: "DailyTourId");
                    table.ForeignKey(
                        name: "FK__DailyTicket__TicketTypeId__3A81E217",
                        column: x => x.TicketTypeId,
                        principalTable: "TicketType",
                        principalColumn: "TicketTypeId");
                });

            migrationBuilder.CreateTable(
                name: "ServiceByTourSegment",
                columns: table => new
                {
                    SBTSId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TourSegmentId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ServiceId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "date", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "date", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ServiceByTourSegment__511379427A17CFF2", x => x.SBTSId);
                    table.ForeignKey(
                        name: "FK__ServiceByTourSegment__ServiceId__3E78F217",
                        column: x => x.ServiceId,
                        principalTable: "Service",
                        principalColumn: "ServiceId");
                    table.ForeignKey(
                        name: "FK__ServiceByTourSegment__TourSegmentId__3C71F227",
                        column: x => x.TourSegmentId,
                        principalTable: "TourSegment",
                        principalColumn: "TourSegmentId");
                });

            migrationBuilder.CreateTable(
                name: "Feedback",
                columns: table => new
                {
                    FeedbackId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    BookingId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    FeedbackMsg = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "date", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "date", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Feedback__551479467C17FHF1", x => x.FeedbackId);
                    table.ForeignKey(
                        name: "FK__Feedback__BookingId__3A81G887",
                        column: x => x.BookingId,
                        principalTable: "Booking",
                        principalColumn: "BookingId");
                    table.ForeignKey(
                        name: "FK__Feedback__UserId__3A81G827",
                        column: x => x.UserId,
                        principalTable: "Account",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Payment",
                columns: table => new
                {
                    PaymentId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PaymentMethodId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    BookingId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    AppId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransTime = table.Column<long>(type: "bigint", nullable: true),
                    Amount = table.Column<float>(type: "real", nullable: true),
                    MerchantTransId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResultCode = table.Column<int>(type: "int", nullable: true),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExtraData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "date", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "date", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Payment__539279267C17FFF1", x => x.PaymentId);
                    table.ForeignKey(
                        name: "FK__Payment__BookingId__3C81C217",
                        column: x => x.BookingId,
                        principalTable: "Booking",
                        principalColumn: "BookingId");
                    table.ForeignKey(
                        name: "FK__Payment__PaymentMethodId__3C822F217",
                        column: x => x.PaymentMethodId,
                        principalTable: "PaymentMethod",
                        principalColumn: "PaymentMethodId");
                });

            migrationBuilder.CreateTable(
                name: "Rate",
                columns: table => new
                {
                    RateId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    BookingId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    RateStar = table.Column<int>(type: "int", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "date", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "date", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Rate__511379427A17FFC3", x => x.RateId);
                    table.ForeignKey(
                        name: "FK__Rate__BookingId__3D71C417",
                        column: x => x.BookingId,
                        principalTable: "Booking",
                        principalColumn: "BookingId");
                    table.ForeignKey(
                        name: "FK__Rate__UserId__3C51F217",
                        column: x => x.UserId,
                        principalTable: "Account",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TransactionsHistory",
                columns: table => new
                {
                    TransactionId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    BookingId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    OrderId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "date", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "date", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__TransactionsHistory__521171177A17CFE8", x => x.TransactionId);
                    table.ForeignKey(
                        name: "FK__TransactionsHistory__BookingId__3E99F888",
                        column: x => x.BookingId,
                        principalTable: "Booking",
                        principalColumn: "BookingId");
                    table.ForeignKey(
                        name: "FK__TransactionsHistory__UserId__3G44F122",
                        column: x => x.UserId,
                        principalTable: "Account",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Ticket",
                columns: table => new
                {
                    TicketId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BookingId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    DailyTicketId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TicketName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: true),
                    QRImgUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberReference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<float>(type: "real", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "date", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "date", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Ticket__521178227A17CEF2", x => x.TicketId);
                    table.ForeignKey(
                        name: "FK__Ticket__BookingId__3G23F227",
                        column: x => x.BookingId,
                        principalTable: "Booking",
                        principalColumn: "BookingId");
                    table.ForeignKey(
                        name: "FK__Ticket__DailyTicketId__3G28F217",
                        column: x => x.DailyTicketId,
                        principalTable: "DailyTicket",
                        principalColumn: "DailyTicketId");
                });

            migrationBuilder.CreateTable(
                name: "ServiceUsedByTicket",
                columns: table => new
                {
                    SUBTId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TicketId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ServiceId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "date", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "date", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ServiceUsedByTicket__511378227A17CEF2", x => x.SUBTId);
                    table.ForeignKey(
                        name: "FK__ServiceUsedByTicket__ServiceId__3E21F217",
                        column: x => x.ServiceId,
                        principalTable: "Service",
                        principalColumn: "ServiceId");
                    table.ForeignKey(
                        name: "FK__ServiceUsedByTicket__TicketId__3B21F227",
                        column: x => x.TicketId,
                        principalTable: "Ticket",
                        principalColumn: "TicketId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Booking_DailyTourId",
                table: "Booking",
                column: "DailyTourId");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_UserId",
                table: "Booking",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerSupport_RequestTypeId",
                table: "CustomerSupport",
                column: "RequestTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerSupport_UserId",
                table: "CustomerSupport",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyTicket_DailyTourId",
                table: "DailyTicket",
                column: "DailyTourId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyTicket_TicketTypeId",
                table: "DailyTicket",
                column: "TicketTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyTour_PackageTourId",
                table: "DailyTour",
                column: "PackageTourId");

            migrationBuilder.CreateIndex(
                name: "IX_Destination_CityId",
                table: "Destination",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Feedback_BookingId",
                table: "Feedback",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_Feedback_UserId",
                table: "Feedback",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Location_DestinationId",
                table: "Location",
                column: "DestinationId");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_UserId",
                table: "Notification",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PackageTour_CityId",
                table: "PackageTour",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_BookingId",
                table: "Payment",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_PaymentMethodId",
                table: "Payment",
                column: "PaymentMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_PointOfInterest_LocationId",
                table: "PointOfInterest",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Rate_BookingId",
                table: "Rate",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_Rate_UserId",
                table: "Rate",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Service_LocationId",
                table: "Service",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Service_ServiceTypeId",
                table: "Service",
                column: "ServiceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Service_SupplierId",
                table: "Service",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceByTourSegment_ServiceId",
                table: "ServiceByTourSegment",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceByTourSegment_TourSegmentId",
                table: "ServiceByTourSegment",
                column: "TourSegmentId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceUsedByTicket_ServiceId",
                table: "ServiceUsedByTicket",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceUsedByTicket_TicketId",
                table: "ServiceUsedByTicket",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_BookingId",
                table: "Ticket",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_DailyTicketId",
                table: "Ticket",
                column: "DailyTicketId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketType_PackageTourId",
                table: "TicketType",
                column: "PackageTourId");

            migrationBuilder.CreateIndex(
                name: "IX_TourSegment_DestinationId",
                table: "TourSegment",
                column: "DestinationId");

            migrationBuilder.CreateIndex(
                name: "IX_TourSegment_PackageTourId",
                table: "TourSegment",
                column: "PackageTourId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionsHistory_BookingId",
                table: "TransactionsHistory",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionsHistory_UserId",
                table: "TransactionsHistory",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerSupport");

            migrationBuilder.DropTable(
                name: "Feedback");

            migrationBuilder.DropTable(
                name: "Notification");

            migrationBuilder.DropTable(
                name: "Payment");

            migrationBuilder.DropTable(
                name: "PointOfInterest");

            migrationBuilder.DropTable(
                name: "Rate");

            migrationBuilder.DropTable(
                name: "ServiceByTourSegment");

            migrationBuilder.DropTable(
                name: "ServiceUsedByTicket");

            migrationBuilder.DropTable(
                name: "TransactionsHistory");

            migrationBuilder.DropTable(
                name: "RequestType");

            migrationBuilder.DropTable(
                name: "PaymentMethod");

            migrationBuilder.DropTable(
                name: "TourSegment");

            migrationBuilder.DropTable(
                name: "Service");

            migrationBuilder.DropTable(
                name: "Ticket");

            migrationBuilder.DropTable(
                name: "Location");

            migrationBuilder.DropTable(
                name: "ServiceType");

            migrationBuilder.DropTable(
                name: "Supplier");

            migrationBuilder.DropTable(
                name: "Booking");

            migrationBuilder.DropTable(
                name: "DailyTicket");

            migrationBuilder.DropTable(
                name: "Destination");

            migrationBuilder.DropTable(
                name: "Account");

            migrationBuilder.DropTable(
                name: "DailyTour");

            migrationBuilder.DropTable(
                name: "TicketType");

            migrationBuilder.DropTable(
                name: "PackageTour");

            migrationBuilder.DropTable(
                name: "City");
        }
    }
}
