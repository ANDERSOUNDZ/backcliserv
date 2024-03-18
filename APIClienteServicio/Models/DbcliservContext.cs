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
            entity.HasKey(e => e.Id).HasName("PK__Cliente__3213E83F3672583F");

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
            entity.HasKey(e => e.Id).HasName("PK__ClienteS__3213E83FA959C017");

            entity.ToTable("ClienteServicio");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Estado)
                .HasDefaultValue(true)
                .HasColumnName("estado");
            entity.Property(e => e.IdCliente).HasColumnName("idCliente");
            entity.Property(e => e.IdServicio).HasColumnName("idServicio");

            entity.HasOne(d => d.IdClienteNavigation).WithMany(p => p.ClienteServicios)
                .HasForeignKey(d => d.IdCliente)
                .HasConstraintName("FK__ClienteSe__idCli__47DBAE45");

            entity.HasOne(d => d.IdServicioNavigation).WithMany(p => p.ClienteServicios)
                .HasForeignKey(d => d.IdServicio)
                .HasConstraintName("FK__ClienteSe__idSer__48CFD27E");
        });

        modelBuilder.Entity<Servicio>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Servicio__3213E83FBE02854E");

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
