using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using PROJECT_S17.Models;

namespace PROJECT_S17.Data;

public partial class S17DbContext : DbContext
{
    public S17DbContext()
    {
    }

    public S17DbContext(DbContextOptions<S17DbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Anagrafica> Anagrafica { get; set; }

    public virtual DbSet<Verbale> Verbale { get; set; }

    public virtual DbSet<Violazione> Violazione { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Server=DESKTOP-QN22R8J\\SQLEXPRESS; Database=PROGETTO_S15; User Id=sa; Password=sa; TrustServerCertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Anagrafica>(entity =>
        {
            entity.HasKey(e => e.Idanagrafica).HasName("PK__ANAGRAFI__7AB1023C59FABAB7");

            entity.Property(e => e.Idanagrafica).ValueGeneratedNever();
        });

        modelBuilder.Entity<Verbale>(entity =>
        {
            entity.HasOne(d => d.Anagrafica).WithMany(p => p.Verbale)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ID_ANAGRAFICA");

            entity.HasOne(d => d.Violazione).WithMany(p => p.Verbale)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ID_VIOLAZIONE");
        });

        modelBuilder.Entity<Violazione>(entity =>
        {
            entity.HasKey(e => e.Idviolazione).HasName("PK__VIOLAZIO__AF77BD92D7D56570");

            entity.Property(e => e.Idviolazione).ValueGeneratedNever();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
