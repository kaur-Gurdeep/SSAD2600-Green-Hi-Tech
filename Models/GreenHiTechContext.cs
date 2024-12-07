using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace GreenHiTech.Models;

public partial class GreenHiTechContext : DbContext
{
    public GreenHiTechContext()
    {
    }

    public GreenHiTechContext(DbContextOptions<GreenHiTechContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AddressDetail> AddressDetails { get; set; }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<CartProduct> CartProducts { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductImage> ProductImages { get; set; }

    public virtual DbSet<User> Users { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Server=VIVOASUS; Database=GreenHiTech; TrustServerCertificate=True; Trusted_Connection=True; MultipleActiveResultSets=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AddressDetail>(entity =>
        {
            entity.HasKey(e => e.PkId).HasName("PK__AddressD__A7C03E1891DE1E57");

            entity.Property(e => e.PkId).HasColumnName("PkID");
            entity.Property(e => e.City)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Country)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.HouseNumber)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.PostalCode)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Province)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Street)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Unit)
                .HasMaxLength(10)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => e.PkId).HasName("PK__Cart__A7C03E18831B1B6F");

            entity.ToTable("Cart");

            entity.Property(e => e.PkId).HasColumnName("PkID");
            entity.Property(e => e.FkUserId).HasColumnName("FkUserID");
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(9, 2)");

            entity.HasOne(d => d.FkUser).WithMany(p => p.Carts)
                .HasForeignKey(d => d.FkUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKCart316510");
        });

        modelBuilder.Entity<CartProduct>(entity =>
        {
            entity.HasKey(e => e.PkId).HasName("PK__CartProd__A7C03E18B076E77D");

            entity.ToTable("CartProduct");

            entity.Property(e => e.PkId).HasColumnName("PkID");
            entity.Property(e => e.FkCartId).HasColumnName("FkCartID");
            entity.Property(e => e.FkProductId).HasColumnName("FkProductID");

            entity.HasOne(d => d.FkCart).WithMany(p => p.CartProducts)
                .HasForeignKey(d => d.FkCartId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKCartProduc150139");

            entity.HasOne(d => d.FkProduct).WithMany(p => p.CartProducts)
                .HasForeignKey(d => d.FkProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKCartProduc893105");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.PkId).HasName("PK__Category__A7C03E182497B186");

            entity.ToTable("Category");

            entity.Property(e => e.PkId).HasColumnName("PkID");
            entity.Property(e => e.Description)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(75)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.PkId).HasName("PK__Order__A7C03E18508B0348");

            entity.ToTable("Order");

            entity.Property(e => e.PkId).HasColumnName("PkID");
            entity.Property(e => e.FkUserId).HasColumnName("FkUserID");
            entity.Property(e => e.Status)
                .HasMaxLength(75)
                .IsUnicode(false);
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(9, 2)");

            entity.HasOne(d => d.FkUser).WithMany(p => p.Orders)
                .HasForeignKey(d => d.FkUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKOrder677398");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => e.PkId).HasName("PK__OrderDet__A7C03E1840D1E2A8");

            entity.Property(e => e.PkId).HasColumnName("PkID");
            entity.Property(e => e.FkOrderId).HasColumnName("FkOrderID");
            entity.Property(e => e.FkProductId).HasColumnName("FkProductID");

            entity.HasOne(d => d.FkOrder).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.FkOrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKOrderDetai54985");

            entity.HasOne(d => d.FkProduct).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.FkProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKOrderDetai737279");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PkId).HasName("PK__Payment__A7C03E18151948A4");

            entity.ToTable("Payment");

            entity.Property(e => e.PkId).HasColumnName("PkID");
            entity.Property(e => e.Amount).HasColumnType("decimal(9, 2)");
            entity.Property(e => e.FkOderId).HasColumnName("FkOderID");
            entity.Property(e => e.TransactionId).HasColumnName("TransactionID");

            entity.HasOne(d => d.FkOder).WithMany(p => p.Payments)
                .HasForeignKey(d => d.FkOderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKPayment376314");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.PkId).HasName("PK__Product__A7C03E18D1ABC323");

            entity.ToTable("Product");

            entity.Property(e => e.PkId).HasColumnName("PkID");
            entity.Property(e => e.Description)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.FkCategoryId).HasColumnName("FkCategoryID");
            entity.Property(e => e.Manufacturer)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Price).HasColumnType("decimal(9, 2)");

            entity.HasOne(d => d.FkCategory).WithMany(p => p.Products)
                .HasForeignKey(d => d.FkCategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKProduct442106");
        });

        modelBuilder.Entity<ProductImage>(entity =>
        {
            entity.HasKey(e => e.PkId).HasName("PK__ProductI__A7C03E1879D38D53");

            entity.ToTable("ProductImage");

            entity.Property(e => e.PkId).HasColumnName("PkID");
            entity.Property(e => e.AltText)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.FkProductId).HasColumnName("FkProductID");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("ImageURL");

            entity.HasOne(d => d.FkProduct).WithMany(p => p.ProductImages)
                .HasForeignKey(d => d.FkProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKProductIma467861");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.PkId).HasName("PK__User__A7C03E18AAC968C6");

            entity.ToTable("User");

            entity.HasIndex(e => e.Email, "EmailUniqueness").IsUnique();

            entity.Property(e => e.PkId).HasColumnName("PkID");
            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FkAddressId).HasColumnName("FkAddressID");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.Role)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.FkAddress).WithMany(p => p.Users)
                .HasForeignKey(d => d.FkAddressId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKUser458995");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
