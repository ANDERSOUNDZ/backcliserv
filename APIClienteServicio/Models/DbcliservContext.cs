using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace APIClienteServicio.Models;

public partial class DbcliservContext : DbContext
{
    public DbcliservContext()
    {
    }

    public DbcliservContext(DbContextOptions<DbcliservContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<ClienteServicio> ClienteServicios { get; set; }

    public virtual DbSet<Servicio> Servicios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Cliente__3213E83F644C953D");

            entity.ToTable("Cliente");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Correo)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("correo");
            entity.Property(e => e.Estado)
                .HasDefaultValue(true)
                .HasColumnName("estado");
            entity.Property(e => e.NombreCliente)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("nombreCliente");
        });

        modelBuilder.Entity<ClienteServicio>(entity =>
        {
            entity.HasKey(e => new { e.IdCliente, e.IdServicio }).HasName("PK__ClienteS__04BFCFFFE17CD079");

            entity.ToTable("ClienteServicio");

            entity.Property(e => e.IdCliente).HasColumnName("idCliente");
            entity.Property(e => e.IdServicio).HasColumnName("idServicio");
            entity.Property(e => e.Estado)
                .HasDefaultValue(true)
                .HasColumnName("estado");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");

            entity.HasOne(d => d.IdClienteNavigation).WithMany(p => p.ClienteServicios)
                .HasForeignKey(d => d.IdCliente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ClienteSe__idCli__0D7A0286");

            entity.HasOne(d => d.IdServicioNavigation).WithMany(p => p.ClienteServicios)
                .HasForeignKey(d => d.IdServicio)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ClienteSe__idSer__0E6E26BF");
        });

        modelBuilder.Entity<Servicio>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Servicio__3213E83FEC9C935D");

            entity.ToTable("Servicio");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.Estado)
                .HasDefaultValue(true)
                .HasColumnName("estado");
            entity.Property(e => e.NombreServicio)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("nombreServicio");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
