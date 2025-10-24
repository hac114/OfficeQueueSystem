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
        modelBuilder.Entity<Tickets>(entity =>
        {
            entity.Property(e => e.CalledAt).HasColumnType("datetime");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
