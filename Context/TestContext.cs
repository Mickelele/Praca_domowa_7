using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using WebApplication4.Models;

namespace WebApplication4.Context;

public partial class TestContext : DbContext
{
    public TestContext()
    {
    }

    public TestContext(DbContextOptions<TestContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductWarehouse> ProductWarehouses { get; set; }

    public virtual DbSet<Warehouse> Warehouses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https: //go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder
            .UseSqlServer(
                "Data Source=DESKTOP-7P8MU5T\\SQL2022;Initial Catalog=TEST;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False")
            .LogTo(Console.WriteLine, LogLevel.Information);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.IdOrder).HasName("Order_pk");

            entity.ToTable("Order");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.FulfilledAt).HasColumnType("datetime");

            entity.HasOne(d => d.IdProductNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.IdProduct)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Receipt_Product");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.IdProduct).HasName("Product_pk");

            entity.ToTable("Product");

            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.Price).HasColumnType("numeric(25, 2)");
        });

        modelBuilder.Entity<ProductWarehouse>(entity =>
        {
            entity.HasKey(e => e.IdProductWarehouse).HasName("Product_Warehouse_pk");

            entity.ToTable("Product_Warehouse");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.Price).HasColumnType("numeric(25, 2)");

            entity.HasOne(d => d.IdOrderNavigation).WithMany(p => p.ProductWarehouses)
                .HasForeignKey(d => d.IdOrder)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Product_Warehouse_Order");

            entity.HasOne(d => d.IdProductNavigation).WithMany(p => p.ProductWarehouses)
                .HasForeignKey(d => d.IdProduct)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("_Product");

            entity.HasOne(d => d.IdWarehouseNavigation).WithMany(p => p.ProductWarehouses)
                .HasForeignKey(d => d.IdWarehouse)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("_Warehouse");
        });

        modelBuilder.Entity<Warehouse>(entity =>
        {
            entity.HasKey(e => e.IdWarehouse).HasName("Warehouse_pk");

            entity.ToTable("Warehouse");

            entity.Property(e => e.Address).HasMaxLength(200);
            entity.Property(e => e.Name).HasMaxLength(200);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
