using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Ersti.Models;

public partial class ErstiContext : DbContext
{
    public ErstiContext()
    {
    }

    public ErstiContext(DbContextOptions<ErstiContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Personen> Personens { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-CQGSRN6;Initial Catalog=Ersti;Integrated Security=True;Trust Server Certificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Personen>(entity =>
        {
            entity.ToTable("Personen");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Aufgabe).HasMaxLength(50);
            entity.Property(e => e.Gebäude)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Raum).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
