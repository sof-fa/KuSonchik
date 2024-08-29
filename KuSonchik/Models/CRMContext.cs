using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace KuSonchik.Models;

public partial class CRMContext : DbContext
{
    public CRMContext()
    {
    }

    public CRMContext(DbContextOptions<CRMContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<Delievery> Delieveries { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=course_work_crm;Username=postgres;Password=postgres");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("clients_pkey");

            entity.ToTable("clients");

            entity.HasIndex(e => e.NumberPhone, "clients_number_phone_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Firstname).HasColumnName("firstname");
            entity.Property(e => e.NumberPhone)
                .HasPrecision(10)
                .HasColumnName("number_phone");
            entity.Property(e => e.Patronymic).HasColumnName("patronymic");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.Surname).HasColumnName("surname");
        });

        modelBuilder.Entity<Delievery>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("delieveries_pkey");

            entity.ToTable("delieveries");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdOrder).HasColumnName("id_order");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.Type).HasColumnName("type");

            entity.HasOne(d => d.IdOrderNavigation).WithMany(p => p.Delieveries)
                .HasForeignKey(d => d.IdOrder)
                .HasConstraintName("delieveries_id_order_fkey");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("orders_pkey");

            entity.ToTable("orders");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Cost).HasColumnName("cost");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.IdClient).HasColumnName("id_client");
            entity.Property(e => e.IdProduct).HasColumnName("id_product");
            entity.Property(e => e.Status).HasColumnName("status");

            entity.HasOne(d => d.IdClientNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.IdClient)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("orders_id_client_fkey");

            entity.HasOne(d => d.IdProductNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.IdProduct)
                .HasConstraintName("orders_id_product_fkey");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("products_pkey");

            entity.ToTable("products");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Category).HasColumnName("category");
            entity.Property(e => e.Color).HasColumnName("color");
            entity.Property(e => e.IdSupplier).HasColumnName("id_supplier");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.Size).HasColumnName("size");

            entity.HasOne(d => d.IdSupplierNavigation).WithMany(p => p.Products)
                .HasForeignKey(d => d.IdSupplier)
                .HasConstraintName("products_id_supplier_fkey");
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("suppliers_pkey");

            entity.ToTable("suppliers");

            entity.HasIndex(e => e.Inn, "suppliers_inn_key").IsUnique();

            entity.HasIndex(e => e.NumberPhone, "suppliers_number_phone_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Inn)
                .HasPrecision(10)
                .HasColumnName("inn");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.NumberPhone)
                .HasPrecision(10)
                .HasColumnName("number_phone");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
