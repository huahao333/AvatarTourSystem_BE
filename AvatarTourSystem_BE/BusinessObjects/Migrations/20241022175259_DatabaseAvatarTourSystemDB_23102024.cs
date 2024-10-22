using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BusinessObjects.Migrations
{
    /// <inheritdoc />
    public partial class DatabaseAvatarTourSystemDB_23102024 : Migration
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
                    PaymentId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PaymentType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "date", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "date", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__PaymentMethod__539279467C17FFG3", x => x.PaymentId);
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
                    PackageTourPrice = table.Column<float>(type: "real", nullable: true),
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
                    PaymentId = table.Column<string>(type: "nvarchar(450)", nullable: true),
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
                        name: "FK__Booking__PaymentId__3A81B328",
                        column: x => x.PaymentId,
                        principalTable: "PaymentMethod",
                        principalColumn: "PaymentId");
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
                name: "Ticket",
                columns: table => new
                {
                    TicketId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BookingId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TicketTypeId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TicketName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: true),
                    QR = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                        name: "FK__Ticket__TicketTypeId__3G28F217",
                        column: x => x.TicketTypeId,
                        principalTable: "TicketType",
                        principalColumn: "TicketTypeId");
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

            migrationBuilder.InsertData(
                table: "Account",
                columns: new[] { "Id", "AccessFailedCount", "Address", "AvatarUrl", "ConcurrencyStamp", "CreateDate", "Dob", "Email", "EmailConfirmed", "FullName", "Gender", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "RefreshToken", "RefreshTokenExpiryTime", "Roles", "SecurityStamp", "Status", "TwoFactorEnabled", "UpdateDate", "UserName", "ZaloUser" },
                values: new object[] { "068c7cd9-3758-43c5-bc35-a0d8f1fa4ecf", 0, "Quận 10, Hồ chí minh", "data:image/png;base64,iVBO", "624ba5d9-c60e-4c14-a382-143d71c2eeba", new DateTime(2024, 10, 23, 0, 52, 59, 102, DateTimeKind.Local).AddTicks(2474), new DateTime(1990, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "huahao04@gmail.com", false, "Hứa Thượng Hảo", true, false, null, null, null, null, "0395840777", false, null, null, null, "7577f71e-9245-45dd-88fa-16b5484e53cf", 0, false, null, null, "" });

            migrationBuilder.InsertData(
                table: "City",
                columns: new[] { "CityId", "CityName", "CreateDate", "Status", "UpdateDate" },
                values: new object[,]
                {
                    { "1", "Hồ Chí Minh", new DateTime(2024, 10, 23, 0, 52, 59, 102, DateTimeKind.Local).AddTicks(2267), 0, null },
                    { "2", "Hà Nội", new DateTime(2024, 10, 23, 0, 52, 59, 102, DateTimeKind.Local).AddTicks(2297), 1, null }
                });

            migrationBuilder.InsertData(
                table: "PaymentMethod",
                columns: new[] { "PaymentId", "CreateDate", "PaymentType", "Status", "UpdateDate" },
                values: new object[] { "1", new DateTime(2024, 10, 23, 0, 52, 59, 102, DateTimeKind.Local).AddTicks(2967), "Momo", 0, null });

            migrationBuilder.InsertData(
                table: "RequestType",
                columns: new[] { "RequestTypeId", "CreateDate", "Priority", "Status", "Type", "UpdateDate" },
                values: new object[] { "1", new DateTime(2024, 10, 23, 0, 52, 59, 102, DateTimeKind.Local).AddTicks(3118), 1, 0, "Support", null });

            migrationBuilder.InsertData(
                table: "ServiceType",
                columns: new[] { "ServiceTypeId", "CreateDate", "ServiceTypeName", "Status", "UpdateDate" },
                values: new object[,]
                {
                    { "1", new DateTime(2024, 10, 23, 0, 52, 59, 102, DateTimeKind.Local).AddTicks(2555), "Tour", 0, null },
                    { "2", new DateTime(2024, 10, 23, 0, 52, 59, 102, DateTimeKind.Local).AddTicks(2557), "Hotel", 1, null }
                });

            migrationBuilder.InsertData(
                table: "Supplier",
                columns: new[] { "SupplierId", "CreateDate", "Status", "SupplierName", "UpdateDate" },
                values: new object[,]
                {
                    { "1", new DateTime(2024, 10, 23, 0, 52, 59, 102, DateTimeKind.Local).AddTicks(2527), 0, "FPT", null },
                    { "2", new DateTime(2024, 10, 23, 0, 52, 59, 102, DateTimeKind.Local).AddTicks(2529), 1, "VNPT", null }
                });

            migrationBuilder.InsertData(
                table: "CustomerSupport",
                columns: new[] { "CusSupportId", "CreateDate", "DateResolved", "Description", "RequestTypeId", "Status", "UpdateDate", "UserId" },
                values: new object[] { "1", new DateTime(2024, 10, 23, 0, 52, 59, 102, DateTimeKind.Local).AddTicks(3094), new DateTime(2024, 10, 23, 0, 52, 59, 102, DateTimeKind.Local).AddTicks(3093), "Support", "1", 0, null, "068c7cd9-3758-43c5-bc35-a0d8f1fa4ecf" });

            migrationBuilder.InsertData(
                table: "Destination",
                columns: new[] { "DestinationId", "CityId", "CreateDate", "DestinationAddress", "DestinationClosingDate", "DestinationClosingHours", "DestinationGoogleMap", "DestinationHotline", "DestinationImgUrl", "DestinationName", "DestinationOpeningDate", "DestinationOpeningHours", "Status", "UpdateDate" },
                values: new object[,]
                {
                    { "1", "1", new DateTime(2024, 10, 23, 0, 52, 59, 102, DateTimeKind.Local).AddTicks(2676), null, null, null, null, null, null, "Quận 1", null, null, 0, null },
                    { "2", "2", new DateTime(2024, 10, 23, 0, 52, 59, 102, DateTimeKind.Local).AddTicks(2679), null, null, null, null, null, null, "Quận 2", null, null, 1, null }
                });

            migrationBuilder.InsertData(
                table: "Notification",
                columns: new[] { "NotifyId", "CreateDate", "Message", "SendDate", "Status", "Title", "Type", "UpdateDate", "UserId" },
                values: new object[] { "1", new DateTime(2024, 10, 23, 0, 52, 59, 102, DateTimeKind.Local).AddTicks(3072), "Chúc mừng bạn đã đặt tour thành công", new DateTime(2024, 10, 23, 0, 52, 59, 102, DateTimeKind.Local).AddTicks(3065), 0, "Thành công", "Success", null, "068c7cd9-3758-43c5-bc35-a0d8f1fa4ecf" });

            migrationBuilder.InsertData(
                table: "PackageTour",
                columns: new[] { "PackageTourId", "CityId", "CreateDate", "PackageTourImgUrl", "PackageTourName", "PackageTourPrice", "Status", "UpdateDate" },
                values: new object[,]
                {
                    { "1", "1", new DateTime(2024, 10, 23, 0, 52, 59, 102, DateTimeKind.Local).AddTicks(2768), null, "Tour Hồ Chí Minh", 500000f, 0, null },
                    { "2", "2", new DateTime(2024, 10, 23, 0, 52, 59, 102, DateTimeKind.Local).AddTicks(2771), null, "Tour Hà Nội", 600000f, 1, null }
                });

            migrationBuilder.InsertData(
                table: "Service",
                columns: new[] { "ServiceId", "CreateDate", "LocationId", "ServiceImgUrl", "ServiceName", "ServicePrice", "ServiceTypeId", "Status", "SupplierId", "UpdateDate" },
                values: new object[,]
                {
                    { "1", new DateTime(2024, 10, 23, 0, 52, 59, 102, DateTimeKind.Local).AddTicks(2584), null, null, "Tour Hồ Chí Minh", null, "1", 0, "1", null },
                    { "2", new DateTime(2024, 10, 23, 0, 52, 59, 102, DateTimeKind.Local).AddTicks(2587), null, null, "Tour Hà Nội", null, "1", 1, "2", null }
                });

            migrationBuilder.InsertData(
                table: "DailyTour",
                columns: new[] { "DailyTourId", "CreateDate", "DailyTourName", "DailyTourPrice", "Description", "Discount", "EndDate", "ExpirationDate", "ImgUrl", "PackageTourId", "StartDate", "Status", "UpdateDate" },
                values: new object[] { "1", new DateTime(2024, 10, 23, 0, 52, 59, 102, DateTimeKind.Local).AddTicks(2835), "Tour Hồ Chí Minh", 790000f, "Tour tham quan du lịch tp Hồ Chí Minh", 10, new DateTime(2024, 9, 12, 17, 30, 0, 0, DateTimeKind.Unspecified), null, "data:image/jpeg;base64,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", "1", new DateTime(2024, 9, 12, 14, 30, 0, 0, DateTimeKind.Unspecified), 0, null });

            migrationBuilder.InsertData(
                table: "Location",
                columns: new[] { "LocationId", "CreateDate", "DestinationId", "LocationClosingHours", "LocationGoogleMap", "LocationHotline", "LocationImgUrl", "LocationName", "LocationOpeningHours", "Status", "UpdateDate" },
                values: new object[,]
                {
                    { "1", new DateTime(2024, 10, 23, 0, 52, 59, 102, DateTimeKind.Local).AddTicks(2710), "1", null, null, null, null, "Nhà hàng", null, 0, null },
                    { "2", new DateTime(2024, 10, 23, 0, 52, 59, 102, DateTimeKind.Local).AddTicks(2712), "1", null, null, null, null, "Khách sạn", null, 1, null }
                });

            migrationBuilder.InsertData(
                table: "TicketType",
                columns: new[] { "TicketTypeId", "CreateDate", "MinBuyTicket", "PackageTourId", "Status", "TicketTypeName", "UpdateDate" },
                values: new object[,]
                {
                    { "1", new DateTime(2024, 10, 23, 0, 52, 59, 102, DateTimeKind.Local).AddTicks(2861), null, "1", 0, "Vé người lớn (>=16 tuổi)", null },
                    { "2", new DateTime(2024, 10, 23, 0, 52, 59, 102, DateTimeKind.Local).AddTicks(2863), null, "1", 1, "Vé trẻ em", null }
                });

            migrationBuilder.InsertData(
                table: "TourSegment",
                columns: new[] { "TourSegmentId", "CreateDate", "DestinationId", "PackageTourId", "Status", "UpdateDate" },
                values: new object[,]
                {
                    { "1", new DateTime(2024, 10, 23, 0, 52, 59, 102, DateTimeKind.Local).AddTicks(2647), "1", "1", 0, null },
                    { "2", new DateTime(2024, 10, 23, 0, 52, 59, 102, DateTimeKind.Local).AddTicks(2649), "2", "2", 1, null }
                });

            migrationBuilder.InsertData(
                table: "Booking",
                columns: new[] { "BookingId", "BookingDate", "CreateDate", "DailyTourId", "ExpirationDate", "PaymentId", "Status", "TotalPrice", "UpdateDate", "UserId" },
                values: new object[] { "1", new DateTime(2024, 10, 23, 0, 52, 59, 102, DateTimeKind.Local).AddTicks(2887), new DateTime(2024, 10, 23, 0, 52, 59, 102, DateTimeKind.Local).AddTicks(2889), "1", null, "1", 0, 500000f, null, "068c7cd9-3758-43c5-bc35-a0d8f1fa4ecf" });

            migrationBuilder.InsertData(
                table: "DailyTicket",
                columns: new[] { "DailyTicketId", "Capacity", "CreateDate", "DailyTicketPrice", "DailyTourId", "Status", "TicketTypeId", "UpdateDate" },
                values: new object[,]
                {
                    { "1", 10, new DateTime(2024, 10, 23, 0, 52, 59, 102, DateTimeKind.Local).AddTicks(2804), null, "1", 0, "1", null },
                    { "2", 10, new DateTime(2024, 10, 23, 0, 52, 59, 102, DateTimeKind.Local).AddTicks(2807), null, "1", 1, "2", null }
                });

            migrationBuilder.InsertData(
                table: "PointOfInterest",
                columns: new[] { "PointId", "CreateDate", "LocationId", "PointName", "Status", "UpdateDate" },
                values: new object[,]
                {
                    { "1", new DateTime(2024, 10, 23, 0, 52, 59, 102, DateTimeKind.Local).AddTicks(2740), "1", "Chợ Bến Thành", 0, null },
                    { "2", new DateTime(2024, 10, 23, 0, 52, 59, 102, DateTimeKind.Local).AddTicks(2743), "1", "Bảo tàng Hồ Chí Minh", 1, null }
                });

            migrationBuilder.InsertData(
                table: "ServiceByTourSegment",
                columns: new[] { "SBTSId", "CreateDate", "ServiceId", "Status", "TourSegmentId", "UpdateDate" },
                values: new object[,]
                {
                    { "1", new DateTime(2024, 10, 23, 0, 52, 59, 102, DateTimeKind.Local).AddTicks(2619), "1", 0, "1", null },
                    { "2", new DateTime(2024, 10, 23, 0, 52, 59, 102, DateTimeKind.Local).AddTicks(2622), "2", 1, "2", null }
                });

            migrationBuilder.InsertData(
                table: "Feedback",
                columns: new[] { "FeedbackId", "BookingId", "CreateDate", "FeedbackMsg", "Status", "UpdateDate", "UserId" },
                values: new object[] { "1", "1", new DateTime(2024, 10, 23, 0, 52, 59, 102, DateTimeKind.Local).AddTicks(3017), "Rất tuyệt vời", 0, null, "068c7cd9-3758-43c5-bc35-a0d8f1fa4ecf" });

            migrationBuilder.InsertData(
                table: "Rate",
                columns: new[] { "RateId", "BookingId", "CreateDate", "RateStar", "Status", "UpdateDate", "UserId" },
                values: new object[] { "1", "1", new DateTime(2024, 10, 23, 0, 52, 59, 102, DateTimeKind.Local).AddTicks(3043), 5, 0, null, "068c7cd9-3758-43c5-bc35-a0d8f1fa4ecf" });

            migrationBuilder.InsertData(
                table: "Ticket",
                columns: new[] { "TicketId", "BookingId", "CreateDate", "Price", "QR", "Quantity", "Status", "TicketName", "TicketTypeId", "UpdateDate" },
                values: new object[,]
                {
                    { "1", "1", new DateTime(2024, 10, 23, 0, 52, 59, 102, DateTimeKind.Local).AddTicks(2917), 100000f, "", 10, 0, "Vé vào cổng", "1", null },
                    { "2", "1", new DateTime(2024, 10, 23, 0, 52, 59, 102, DateTimeKind.Local).AddTicks(2921), 50000f, "", 10, 1, "Vé ăn uống", "1", null }
                });

            migrationBuilder.InsertData(
                table: "TransactionsHistory",
                columns: new[] { "TransactionId", "BookingId", "CreateDate", "OrderId", "Status", "UpdateDate", "UserId" },
                values: new object[] { "1", "1", new DateTime(2024, 10, 23, 0, 52, 59, 102, DateTimeKind.Local).AddTicks(2994), null, 0, null, "068c7cd9-3758-43c5-bc35-a0d8f1fa4ecf" });

            migrationBuilder.InsertData(
                table: "ServiceUsedByTicket",
                columns: new[] { "SUBTId", "CreateDate", "ServiceId", "Status", "TicketId", "UpdateDate" },
                values: new object[,]
                {
                    { "1", new DateTime(2024, 10, 23, 0, 52, 59, 102, DateTimeKind.Local).AddTicks(2944), "1", 0, "1", null },
                    { "2", new DateTime(2024, 10, 23, 0, 52, 59, 102, DateTimeKind.Local).AddTicks(2947), "2", 1, "2", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Booking_DailyTourId",
                table: "Booking",
                column: "DailyTourId");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_PaymentId",
                table: "Booking",
                column: "PaymentId");

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
                name: "IX_Ticket_TicketTypeId",
                table: "Ticket",
                column: "TicketTypeId");

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
                name: "DailyTicket");

            migrationBuilder.DropTable(
                name: "Feedback");

            migrationBuilder.DropTable(
                name: "Notification");

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
                name: "TicketType");

            migrationBuilder.DropTable(
                name: "Destination");

            migrationBuilder.DropTable(
                name: "DailyTour");

            migrationBuilder.DropTable(
                name: "PaymentMethod");

            migrationBuilder.DropTable(
                name: "Account");

            migrationBuilder.DropTable(
                name: "PackageTour");

            migrationBuilder.DropTable(
                name: "City");
        }
    }
}
