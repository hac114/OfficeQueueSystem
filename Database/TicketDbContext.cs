using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Database;

public partial class TicketDbContext : DbContext
{
    public TicketDbContext()
    {
    }

    public TicketDbContext(DbContextOptions<TicketDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ServiceTypes> ServiceTypes { get; set; }

    public virtual DbSet<Tickets> Tickets { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ServiceTypes>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ServiceT__3214EC07496C76CF");

            entity.Property(e => e.Tag).HasMaxLength(50);
        });

        modelBuilder.Entity<Tickets>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tickets__3214EC0793272B30");

            entity.HasIndex(e => e.TicketCode, "UQ__Tickets__598CF7A3F0DD91D1").IsUnique();

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.ServiceType).HasMaxLength(50);
            entity.Property(e => e.TicketCode).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
