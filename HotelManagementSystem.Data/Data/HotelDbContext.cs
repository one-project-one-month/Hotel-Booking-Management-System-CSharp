using System;
using System.Collections.Generic;
using HotelManagementSystem.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementSystem.Data.Data;

public partial class HotelDbContext : DbContext
{
    public HotelDbContext()
    {
    }

    public HotelDbContext(DbContextOptions<HotelDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CheckInOut> CheckInOuts { get; set; }

    public virtual DbSet<TblBooking> TblBookings { get; set; }

    public virtual DbSet<TblCoupon> TblCoupons { get; set; }

    public virtual DbSet<TblGuest> TblGuests { get; set; }

    public virtual DbSet<TblInvoice> TblInvoices { get; set; }

    public virtual DbSet<TblRole> TblRoles { get; set; }

    public virtual DbSet<TblRoom> TblRooms { get; set; }

    public virtual DbSet<TblRoomBooking> TblRoomBookings { get; set; }

    public virtual DbSet<TblRoomType> TblRoomTypes { get; set; }

    public virtual DbSet<TblUser> TblUsers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=LAPTOP-NFU692OK\\LOCALDB;Initial Catalog=HBMS_Dbo;Integrated Security=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CheckInOut>(entity =>
        {
            entity.HasKey(e => e.CheckInOutId).HasName("PK__Check_In__C68602BFA778E43C");

            entity.ToTable("Check_In_Out");

            entity.Property(e => e.CheckInOutId).HasColumnName("CheckInOut_Id");
            entity.Property(e => e.CheckInTime)
                .HasColumnType("datetime")
                .HasColumnName("CheckIn_Time");
            entity.Property(e => e.CheckOutTime)
                .HasColumnType("datetime")
                .HasColumnName("CheckOut_Time");
            entity.Property(e => e.ExtraCharges)
                .HasColumnType("decimal(8, 2)")
                .HasColumnName("Extra_Charges");
            entity.Property(e => e.GuestId).HasColumnName("Guest_Id");
            entity.Property(e => e.Status).HasMaxLength(10);

            entity.HasOne(d => d.Guest).WithMany(p => p.CheckInOuts)
                .HasForeignKey(d => d.GuestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Guest_InOut");
        });

        modelBuilder.Entity<TblBooking>(entity =>
        {
            entity.HasKey(e => e.BookingId).HasName("PK__Tbl_Book__35ABFDC0DB6CC04B");

            entity.ToTable("Tbl_Booking");

            entity.Property(e => e.BookingId)
                .ValueGeneratedNever()
                .HasColumnName("Booking_Id");
            entity.Property(e => e.BookingStatus)
                .HasMaxLength(50)
                .HasColumnName("Booking_Status");
            entity.Property(e => e.CheckInTime).HasColumnName("CheckIn_Time");
            entity.Property(e => e.CheckOutTime).HasColumnName("CheckOut_Time");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DepositAmount)
                .HasColumnType("decimal(8, 2)")
                .HasColumnName("Deposit_Amount");
            entity.Property(e => e.GuestCount).HasColumnName("Guest_Count");
            entity.Property(e => e.GuestId).HasColumnName("Guest_Id");
            entity.Property(e => e.PaymentType).HasMaxLength(50);
            entity.Property(e => e.TotalAmount)
                .HasColumnType("decimal(8, 2)")
                .HasColumnName("Total_Amount");

            entity.HasOne(d => d.Guest).WithMany(p => p.TblBookings)
                .HasForeignKey(d => d.GuestId)
                .HasConstraintName("FK_User_Booking_Guest");

            entity.HasOne(d => d.User).WithMany(p => p.TblBookings)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_User_Booking_User");
        });

        modelBuilder.Entity<TblCoupon>(entity =>
        {
            entity.HasKey(e => e.CouponId).HasName("PK__Tbl_Coup__384AF1BA7532E7BA");

            entity.ToTable("Tbl_Coupons");

            entity.Property(e => e.CouponId).ValueGeneratedNever();
            entity.Property(e => e.BookingId).HasColumnName("Booking_Id");
            entity.Property(e => e.CouponCode)
                .HasMaxLength(50)
                .HasColumnName("Coupon_Code");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnName("Created_At");
            entity.Property(e => e.DiscountPct)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("Discount_Pct");
            entity.Property(e => e.ExpireDate)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnName("Expire_Date");
            entity.Property(e => e.GuestId).HasColumnName("Guest_Id");
            entity.Property(e => e.IsClaimed)
                .HasDefaultValue(false)
                .HasColumnName("Is_Claimed");

            entity.HasOne(d => d.Booking).WithMany(p => p.TblCoupons)
                .HasForeignKey(d => d.BookingId)
                .HasConstraintName("FK_Booking_Coupon");

            entity.HasOne(d => d.Guest).WithMany(p => p.TblCoupons)
                .HasForeignKey(d => d.GuestId)
                .HasConstraintName("FK_Coupons_Guest");
        });

        modelBuilder.Entity<TblGuest>(entity =>
        {
            entity.HasKey(e => e.GuestId).HasName("PK__Tbl_Gues__CB8B0D334BC19F16");

            entity.ToTable("Tbl_Guest");

            entity.Property(e => e.GuestId)
                .ValueGeneratedNever()
                .HasColumnName("Guest_Id");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.Nrc)
                .HasMaxLength(50)
                .HasColumnName("NRC");
            entity.Property(e => e.PhoneNo)
                .HasMaxLength(50)
                .HasColumnName("Phone_No");

            entity.HasOne(d => d.User).WithMany(p => p.TblGuests)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_User_Guest");
        });

        modelBuilder.Entity<TblInvoice>(entity =>
        {
            entity.HasKey(e => e.InvoiceId).HasName("PK__Tbl_Invo__0DE6057488D536A6");

            entity.ToTable("Tbl_Invoice");

            entity.Property(e => e.InvoiceId)
                .ValueGeneratedNever()
                .HasColumnName("Invoice_Id");
            entity.Property(e => e.CheckInTime)
                .HasColumnType("datetime")
                .HasColumnName("CheckIn_Time");
            entity.Property(e => e.CheckOutTime)
                .HasColumnType("datetime")
                .HasColumnName("CheckOut_Time");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.Deposite).HasColumnType("decimal(8, 2)");
            entity.Property(e => e.ExtraCharges)
                .HasColumnType("decimal(8, 2)")
                .HasColumnName("Extra_Charges");
            entity.Property(e => e.GuestId).HasColumnName("Guest_Id");
            entity.Property(e => e.PaymentType).HasMaxLength(50);
            entity.Property(e => e.TotalAmount)
                .HasColumnType("decimal(8, 2)")
                .HasColumnName("Total_Amount");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getutcdate())");

            entity.HasOne(d => d.Guest).WithMany(p => p.TblInvoices)
                .HasForeignKey(d => d.GuestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Guest_Invoice");
        });

        modelBuilder.Entity<TblRole>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Tbl_Role__8AFACE1ADBE921D7");

            entity.ToTable("Tbl_Roles");

            entity.Property(e => e.RoleId).ValueGeneratedNever();
            entity.Property(e => e.RoleName).HasMaxLength(50);
        });

        modelBuilder.Entity<TblRoom>(entity =>
        {
            entity.HasKey(e => e.RoomId).HasName("PK__Tbl_Room__19EE6A134F5CE945");

            entity.ToTable("Tbl_Rooms");

            entity.HasIndex(e => e.RoomNo, "UQ__Tbl_Room__19EF81FDB28A5247").IsUnique();

            entity.Property(e => e.RoomId)
                .ValueGeneratedNever()
                .HasColumnName("Room_Id");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.GuestLimit).HasColumnName("Guest_Limit");
            entity.Property(e => e.ImgUrl).HasColumnName("Img_URL");
            entity.Property(e => e.RoomNo)
                .HasMaxLength(50)
                .HasColumnName("Room_No");
            entity.Property(e => e.RoomStatus)
                .HasMaxLength(15)
                .HasColumnName("Room_Status");
            entity.Property(e => e.RoomTypeId).HasColumnName("RoomType_Id");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getutcdate())");

            entity.HasOne(d => d.RoomType).WithMany(p => p.TblRooms)
                .HasForeignKey(d => d.RoomTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Room_RoomType");
        });

        modelBuilder.Entity<TblRoomBooking>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tbl_Room__3214EC0753BE3E89");

            entity.ToTable("Tbl_Room_Booking");

            entity.Property(e => e.BookingId).HasColumnName("Booking_Id");
            entity.Property(e => e.RoomId).HasColumnName("Room_Id");

            entity.HasOne(d => d.Booking).WithMany(p => p.TblRoomBookings)
                .HasForeignKey(d => d.BookingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Book_Room");

            entity.HasOne(d => d.Room).WithMany(p => p.TblRoomBookings)
                .HasForeignKey(d => d.RoomId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Room_Book");
        });

        modelBuilder.Entity<TblRoomType>(entity =>
        {
            entity.HasKey(e => e.RoomTypeId).HasName("PK__Tbl_Room__ADB3BCFB035A2943");

            entity.ToTable("Tbl_RoomType");

            entity.Property(e => e.RoomTypeId)
                .ValueGeneratedNever()
                .HasColumnName("RoomType_Id");
            entity.Property(e => e.Price).HasColumnType("decimal(8, 2)");
            entity.Property(e => e.RoomTypeName)
                .HasMaxLength(50)
                .HasColumnName("RoomType_Name");
        });

        modelBuilder.Entity<TblUser>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Tbl_User__1788CC4CBDEA471F");

            entity.ToTable("Tbl_Users");

            entity.HasIndex(e => e.Email, "UQ__Tbl_User__A9D105343C68F124").IsUnique();

            entity.Property(e => e.UserId).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.ForgetPasswordOtp)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("ForgetPasswordOTP");
            entity.Property(e => e.OtpExpireAt)
                .HasColumnType("datetime")
                .HasColumnName("OTP_ExpireAt");
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.TokenExpireAt).HasColumnType("datetime");
            entity.Property(e => e.UserName).HasMaxLength(50);

            entity.HasOne(d => d.Role).WithMany(p => p.TblUsers)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tbl_Users_Tbl_Roles");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
