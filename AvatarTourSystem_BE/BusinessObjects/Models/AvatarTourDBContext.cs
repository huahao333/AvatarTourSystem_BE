using BusinessObjects.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models
{
    public partial class AvatarTourDBContext : IdentityDbContext<Account>
    {
        public AvatarTourDBContext()
        {
        }

        public AvatarTourDBContext(DbContextOptions<AvatarTourDBContext> options)
            : base(options)
        {
        }
        public virtual DbSet<Account> Accounts { get; set; } = null!;
        public virtual DbSet<Booking> Bookings { get; set; } = null!;
        public virtual DbSet<BookingByRevenue> BookingByRevenues { get; set; } = null!;
        public virtual DbSet<City> Cities { get; set; } = null!;
        public virtual DbSet<CustomerSupport> CustomerSupports { get; set; } = null!;
        public virtual DbSet<DailyTour> DailyTours { get; set; } = null!;
        public virtual DbSet<DailyTicket> DailyTickets { get; set; } = null!;
        public virtual DbSet<Destination> Destinations { get; set; } = null!;
        public virtual DbSet<Feedback> Feedbacks { get; set; } = null!;
        public virtual DbSet<Location> Locations { get; set; } = null!;
        public virtual DbSet<Notification> Notifications { get; set; } = null!;
        public virtual DbSet<PackageTour> PackageTours { get; set; } = null!;
        public virtual DbSet<PaymentMethod> PaymentMethods { get; set; } = null!;
        public virtual DbSet<PointOfInterest> PointOfInterests { get; set; } = null!;
        public virtual DbSet<POIType> POITypes { get; set; } = null!;
        public virtual DbSet<Rate> Rates { get; set; } = null!;
        public virtual DbSet<RequestType> RequestTypes { get; set; } = null!;
        public virtual DbSet<Revenue> Revenues { get; set; } = null!;
        public virtual DbSet<Service> Services { get; set; } = null!;
        public virtual DbSet<ServiceByTourSegment> ServiceByTourSegments { get; set; } = null!;
        public virtual DbSet<ServiceType> ServiceTypes { get; set; } = null!;
        public virtual DbSet<ServiceUsedByTicket> ServiceUsedByTickets { get; set; } = null!;
        public virtual DbSet<Supplier> Suppliers { get; set; } = null!;
        public virtual DbSet<Ticket> Tickets { get; set; } = null!;
        public virtual DbSet<TicketType> TicketTypes { get; set; } = null!;
        public virtual DbSet<TourSegment> TourSegments { get; set; } = null!;
        public virtual DbSet<TransactionsHistory> TransactionsHistories { get; set; } = null!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            SeedData.Initialize(modelBuilder);
            modelBuilder.Entity<Account>(entity =>
            {
                entity.ToTable("Account");
                entity.Property(e => e.Address).HasMaxLength(250);
                entity.Property(e => e.FullName).HasMaxLength(150);
            });

            modelBuilder.Entity<Booking>(entity =>
            {
                entity.HasKey(e => e.BookingId)
                    .HasName("PK__Booking__551479477F27FEF3");

                entity.ToTable("Booking");

                entity.Property(e => e.BookingDate)
                    .HasColumnType("date");
                entity.Property(e => e.CreateDate)
                    .HasColumnType("date");
                entity.Property(e => e.UpdateDate)
                    .HasColumnType("date");

                entity.HasOne(d => d.Accounts)
                   .WithMany(p => p.Bookings)
                   .HasForeignKey(d => d.UserId)
                   .HasConstraintName("FK__Booking__UserId__3A81B327");

                entity.HasOne(d => d.PaymentMethods)
                   .WithMany(p => p.Bookings)
                   .HasForeignKey(d => d.PaymentId)
                   .HasConstraintName("FK__Booking__PaymentId__3A81B328");

                entity.HasOne(d => d.DailyTours)
                   .WithMany(p => p.Bookings)
                   .HasForeignKey(d => d.DailyTourId)
                   .HasConstraintName("FK__Booking__DailyTourId__3A81B329");

            });

            modelBuilder.Entity<BookingByRevenue>(entity =>
            {
                entity.HasKey(e => e.BookingByRevenueId)
                    .HasName("PK__BookingByRevenue__551479477F27FEF9");
                entity.ToTable("BookingByRevenue");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("date");
                entity.Property(e => e.UpdateDate)
                    .HasColumnType("date");

                entity.HasOne(d => d.Revenues)
                  .WithMany(p => p.BookingByRevenues)
                  .HasForeignKey(d => d.RevenueId)
                  .HasConstraintName("FK__BookingByRevenue__RevenueId__3A81C227");

                entity.HasOne(d => d.Bookings)
                  .WithMany(p => p.BookingByRevenues)
                  .HasForeignKey(d => d.BookingId)
                  .HasConstraintName("FK__BookingByRevenue__BookingId__3A81D227");
            });

            modelBuilder.Entity<City>(entity =>
            {
                entity.HasKey(e => e.CityId)
                   .HasName("PK__Booking__551479477F27FEF4");
                entity.ToTable("City");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("date");
                entity.Property(e => e.UpdateDate)
                    .HasColumnType("date");
              

            });

            modelBuilder.Entity<CustomerSupport>(entity =>
            {
                entity.HasKey(e => e.CusSupportId)
                    .HasName("PK__CustomerSupport__551479477C27FEF5");
                entity.ToTable("CustomerSupport");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("date");
                entity.Property(e => e.UpdateDate)
                    .HasColumnType("date");

                entity.HasOne(d => d.Accounts)
                  .WithMany(p => p.CustomerSupports)
                  .HasForeignKey(d => d.UserId)
                  .HasConstraintName("FK__CustomerSupport__UserId__3A81A227");

                entity.HasOne(d => d.RequestTypes)
                  .WithMany(p => p.CustomerSupports)
                  .HasForeignKey(d => d.RequestTypeId)
                  .HasConstraintName("FK__CustomerSupport__RequestTypeId__3A81A229");
            });

            modelBuilder.Entity<DailyTicket>(entity =>
            {
                entity.HasKey(e => e.DailyTicketId)
                   .HasName("PK__DailyTicket__551479467C27FEC5");
                entity.ToTable("DailyTicket");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("date");
                entity.Property(e => e.UpdateDate)
                    .HasColumnType("date");

                entity.HasOne(d => d.TicketTypes)
                  .WithMany(p => p.DailyTickets)
                  .HasForeignKey(d => d.TicketTypeId)
                  .HasConstraintName("FK__DailyTicket__TicketTypeId__3A81E217");

                entity.HasOne(d => d.DailyTours)
                  .WithMany(p => p.DailyTickets)
                  .HasForeignKey(d => d.DailyTourId)
                  .HasConstraintName("FK__DailyTicket__DailyTourId__3A81E267");
            });

            modelBuilder.Entity<DailyTour>(entity =>
            {
                entity.HasKey(e => e.DailyTourId)
                  .HasName("PK__DailyTour__551479467C27FEF1");
                entity.ToTable("DailyTour");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("date");
                entity.Property(e => e.UpdateDate)
                    .HasColumnType("date");



                entity.HasOne(d => d.PackageTours)
                  .WithMany(p => p.DailyTours)
                  .HasForeignKey(d => d.PackageTourId)
                  .HasConstraintName("FK__DailyTour__PackageTourId__3A81G267");
            });

            modelBuilder.Entity<Destination>(entity =>
            {
                entity.HasKey(e => e.DestinationId)
                  .HasName("PK__Destination__551479467C17FEF7");
                entity.ToTable("Destination");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("date");
                entity.Property(e => e.UpdateDate)
                    .HasColumnType("date");



                entity.HasOne(d => d.Cities)
                  .WithMany(p => p.Destinations)
                  .HasForeignKey(d => d.CityId)
                  .HasConstraintName("FK__Destination__CityId__3A81G227");
            });


            modelBuilder.Entity<Feedback>(entity =>
            {
                entity.HasKey(e => e.FeedbackId)
                  .HasName("PK__Feedback__551479467C17FHF1");
                entity.ToTable("Feedback");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("date");
                entity.Property(e => e.UpdateDate)
                    .HasColumnType("date");



                entity.HasOne(d => d.Accounts)
                  .WithMany(p => p.Feedbacks)
                  .HasForeignKey(d => d.UserId)
                  .HasConstraintName("FK__Feedback__UserId__3A81G827");
                entity.HasOne(d => d.Bookings)
                  .WithMany(p => p.Feedbacks)
                  .HasForeignKey(d => d.BookingId)
                  .HasConstraintName("FK__Feedback__BookingId__3A81G887");

            });

            modelBuilder.Entity<Location>(entity =>
            {
                entity.HasKey(e => e.LocationId)
                  .HasName("PK__Location__551279467C17FHE1");
                entity.ToTable("Location");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("date");
                entity.Property(e => e.UpdateDate)
                    .HasColumnType("date");



                entity.HasOne(d => d.Destinations)
                  .WithMany(p => p.Locations)
                  .HasForeignKey(d => d.DestinationId)
                  .HasConstraintName("FK__Location__DestinationId__3A81E127");
            });

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.HasKey(e => e.NotifyId)
                  .HasName("PK__Notification__531279467C17FFE2");
                entity.ToTable("Notification");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("date");
                entity.Property(e => e.UpdateDate)
                    .HasColumnType("date");



                entity.HasOne(d => d.Accounts)
                  .WithMany(p => p.Notifications)
                  .HasForeignKey(d => d.UserId)
                  .HasConstraintName("FK__Notification__UserId__3A81F117");
            });

            modelBuilder.Entity<PackageTour>(entity =>
            {
                entity.HasKey(e => e.PackageTourId)
                   .HasName("PK__PackageTour__531279467C17FFG3");
                entity.ToTable("PackageTour");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("date");
                entity.Property(e => e.UpdateDate)
                    .HasColumnType("date");



                entity.HasOne(d => d.Cities)
                  .WithMany(p => p.PackageTours)
                  .HasForeignKey(d => d.CityId)
                  .HasConstraintName("FK__PackageTour__CityId__3C81F217");
            });

            modelBuilder.Entity<PaymentMethod>(entity =>
            {
                entity.HasKey(e => e.PaymentId)
                  .HasName("PK__PaymentMethod__539279467C17FFG3");
                entity.ToTable("PaymentMethod");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("date");
                entity.Property(e => e.UpdateDate)
                    .HasColumnType("date");



            });

            modelBuilder.Entity<PointOfInterest>(entity =>
            {
                entity.HasKey(e => e.PointId)
                   .HasName("PK__PointOfInterest__511379467C17FFC3");
                entity.ToTable("PointOfInterest");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("date");
                entity.Property(e => e.UpdateDate)
                    .HasColumnType("date");



                entity.HasOne(d => d.POITypes)
                  .WithMany(p => p.PointOfInterests)
                  .HasForeignKey(d => d.POITypeId)
                  .HasConstraintName("FK__PointOfInterest__POITypeId__3F21F217");
                entity.HasOne(d => d.Locations)
                  .WithMany(p => p.PointOfInterests)
                  .HasForeignKey(d => d.LocationId)
                  .HasConstraintName("FK__PointOfInterest__LocationId__3F21C417");
            });

            modelBuilder.Entity<POIType>(entity =>
            {
                entity.HasKey(e => e.POITypeId)
                   .HasName("PK__POIType__511372467B17FFC3");
                entity.ToTable("POIType");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("date");
                entity.Property(e => e.UpdateDate)
                    .HasColumnType("date");
            });

            modelBuilder.Entity<Rate>(entity =>
            {
                entity.HasKey(e => e.RateId)
                  .HasName("PK__Rate__511379427A17FFC3");
                entity.ToTable("Rate");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("date");
                entity.Property(e => e.UpdateDate)
                    .HasColumnType("date");



                entity.HasOne(d => d.Accounts)
                  .WithMany(p => p.Rates)
                  .HasForeignKey(d => d.UserId)
                  .HasConstraintName("FK__Rate__UserId__3C51F217");
                entity.HasOne(d => d.Bookings)
                  .WithMany(p => p.Rates)
                  .HasForeignKey(d => d.BookingId)
                  .HasConstraintName("FK__Rate__BookingId__3D71C417");
            });

            modelBuilder.Entity<RequestType>(entity =>
            {
                entity.HasKey(e => e.RequestTypeId)
                   .HasName("PK__RequestType__511372221B17FFC3");
                entity.ToTable("RequestType");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("date");
                entity.Property(e => e.UpdateDate)
                    .HasColumnType("date");
            });

            modelBuilder.Entity<Revenue>(entity =>
            {
                entity.HasKey(e => e.RevenueId)
                   .HasName("PK__Revenue__511311221B17FEC3");
                entity.ToTable("Revenue");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("date");
                entity.Property(e => e.UpdateDate)
                    .HasColumnType("date");
            });

            modelBuilder.Entity<Service>(entity =>
            {
                entity.HasKey(e => e.ServiceId)
                 .HasName("PK__Service__511379427A17DFC9");
                entity.ToTable("Service");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("date");
                entity.Property(e => e.UpdateDate)
                    .HasColumnType("date");



                entity.HasOne(d => d.ServiceTypes)
                  .WithMany(p => p.Services)
                  .HasForeignKey(d => d.ServiceTypeId)
                  .HasConstraintName("FK__Service__ServiceTypeId__3B21F217");
                entity.HasOne(d => d.Locations)
                  .WithMany(p => p.Services)
                  .HasForeignKey(d => d.LocationId)
                  .HasConstraintName("FK__Service__LocationId__3D71F217");
                entity.HasOne(d => d.Suppliers)
                  .WithMany(p => p.Services)
                  .HasForeignKey(d => d.SupplierId)
                  .HasConstraintName("FK__Service__SupplierId__3D71E717");
            });

            modelBuilder.Entity<ServiceByTourSegment>(entity =>
            {
                entity.HasKey(e => e.SBTSId)
                .HasName("PK__ServiceByTourSegment__511379427A17CFF2");
                entity.ToTable("ServiceByTourSegment");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("date");
                entity.Property(e => e.UpdateDate)
                    .HasColumnType("date");



                entity.HasOne(d => d.TourSegments)
                  .WithMany(p => p.ServiceByTourSegments)
                  .HasForeignKey(d => d.TourSegmentId)
                  .HasConstraintName("FK__ServiceByTourSegment__TourSegmentId__3C71F227");
                entity.HasOne(d => d.Services)
                  .WithMany(p => p.ServiceByTourSegments)
                  .HasForeignKey(d => d.ServiceId)
                  .HasConstraintName("FK__ServiceByTourSegment__ServiceId__3E78F217");
            });

            modelBuilder.Entity<ServiceType>(entity =>
            {
                entity.HasKey(e => e.ServiceTypeId)
                .HasName("PK__ServiceType__511379427A17CGF4");
                entity.ToTable("ServiceType");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("date");
                entity.Property(e => e.UpdateDate)
                    .HasColumnType("date");
            });

            modelBuilder.Entity<ServiceUsedByTicket>(entity =>
            {
                entity.HasKey(e => e.SUBTId)
                .HasName("PK__ServiceUsedByTicket__511378227A17CEF2");
                entity.ToTable("ServiceUsedByTicket");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("date");
                entity.Property(e => e.UpdateDate)
                    .HasColumnType("date");



                entity.HasOne(d => d.Tickets)
                  .WithMany(p => p.ServiceUsedByTickets)
                  .HasForeignKey(d => d.TicketId)
                  .HasConstraintName("FK__ServiceUsedByTicket__TicketId__3B21F227");
                entity.HasOne(d => d.Services)
                  .WithMany(p => p.ServiceUsedByTickets)
                  .HasForeignKey(d => d.ServiceId)
                  .HasConstraintName("FK__ServiceUsedByTicket__ServiceId__3E21F217");
            });

            modelBuilder.Entity<Supplier>(entity =>
            {
                entity.HasKey(e => e.SupplierId)
                .HasName("PK__Supplier__512219427A17CGF4");
                entity.ToTable("Supplier");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("date");
                entity.Property(e => e.UpdateDate)
                    .HasColumnType("date");
            });

            modelBuilder.Entity<Ticket>(entity =>
            {
                entity.HasKey(e => e.TicketId)
                .HasName("PK__Ticket__521178227A17CEF2");
                entity.ToTable("Ticket");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("date");
                entity.Property(e => e.UpdateDate)
                    .HasColumnType("date");



                entity.HasOne(d => d.Bookings)
                  .WithMany(p => p.Tickets)
                  .HasForeignKey(d => d.BookingId)
                  .HasConstraintName("FK__Ticket__BookingId__3G23F227");
                entity.HasOne(d => d.TicketTypes)
                  .WithMany(p => p.Tickets)
                  .HasForeignKey(d => d.TicketTypeId)
                  .HasConstraintName("FK__Ticket__TicketTypeId__3G28F217");
            });

            modelBuilder.Entity<TicketType>(entity =>
            {
                entity.HasKey(e => e.TicketTypeId)
              .HasName("PK__TicketType__521178227A17CFF8");
                entity.ToTable("TicketType");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("date");
                entity.Property(e => e.UpdateDate)
                    .HasColumnType("date");



                entity.HasOne(d => d.PackageTours)
                  .WithMany(p => p.TicketTypes)
                  .HasForeignKey(d => d.PackageTourId)
                  .HasConstraintName("FK__TicketType__PackageTourId__3G13F297");
            });

            modelBuilder.Entity<TourSegment>(entity =>
            {
                entity.HasKey(e => e.TourSegmentId)
             .HasName("PK__TourSegment__521178777A17CFE8");
                entity.ToTable("TourSegment");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("date");
                entity.Property(e => e.UpdateDate)
                    .HasColumnType("date");



                entity.HasOne(d => d.PackageTours)
                  .WithMany(p => p.TourSegments)
                  .HasForeignKey(d => d.PackageTourId)
                  .HasConstraintName("FK__TourSegment__PackageTourId__3G13F297");
                entity.HasOne(d => d.Destinations)
                  .WithMany(p => p.TourSegments)
                  .HasForeignKey(d => d.DestinationId)
                  .HasConstraintName("FK__TourSegment__DestinationId__3F33F297");
            });

            modelBuilder.Entity<TransactionsHistory>(entity =>
            {
                entity.HasKey(e => e.TransactionId)
            .HasName("PK__TransactionsHistory__521171177A17CFE8");
                entity.ToTable("TransactionsHistory");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("date");
                entity.Property(e => e.UpdateDate)
                    .HasColumnType("date");



                entity.HasOne(d => d.Accounts)
                  .WithMany(p => p.TransactionsHistorys)
                  .HasForeignKey(d => d.UserId)
                  .HasConstraintName("FK__TransactionsHistory__UserId__3G44F122");
                entity.HasOne(d => d.Bookings)
                  .WithMany(p => p.TransactionsHistories)
                  .HasForeignKey(d => d.BookingId)
                  .HasConstraintName("FK__TransactionsHistory__BookingId__3E99F888");
            });

        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
