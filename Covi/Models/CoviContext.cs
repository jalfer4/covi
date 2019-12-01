using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Covi.Models
{
    public partial class CoviContext : DbContext
    {
        public CoviContext()
        {
        }

        public CoviContext(DbContextOptions<CoviContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Evento> Evento { get; set; }
        public virtual DbSet<Local> Local { get; set; }
        public virtual DbSet<Reserva> Reserva { get; set; }
        public virtual DbSet<TipoEvento> TipoEvento { get; set; }
        public virtual DbSet<TipoLocal> TipoLocal { get; set; }
        public virtual DbSet<TipoUsuario> TipoUsuario { get; set; }
        public virtual DbSet<Usuario> Usuario { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=192.168.65.59;Initial Catalog=Covi;User ID=AdminCovi;Password=1q2w3e4r%T");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<Evento>(entity =>
            {
                entity.Property(e => e.FechaAlta).HasColumnType("datetime");

                entity.Property(e => e.NombreArtista).IsRequired();

                entity.HasOne(d => d.TipoEventoNavigation)
                    .WithMany(p => p.Evento)
                    .HasForeignKey(d => d.TipoEventoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Evento_TipoEvento");

                entity.HasOne(d => d.LocalNavigation)
                   .WithMany(p => p.Evento)
                   .HasForeignKey(d => d.LocalId)
                   .OnDelete(DeleteBehavior.ClientSetNull)
                   .HasConstraintName("FK_Evento_Local");


            });

            modelBuilder.Entity<Local>(entity =>
            {
                entity.Property(e => e.Direccion).IsRequired();

                entity.Property(e => e.Nombre).IsRequired();

                entity.Property(e => e.Telefono).IsRequired();

                entity.HasOne(d => d.TipoLocal)
                    .WithMany(p => p.Local)
                    .HasForeignKey(d => d.TipoLocalId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Local_TipoLocal");

                entity.HasOne(d => d.Usuario)
                    .WithMany(p => p.Local)
                    .HasForeignKey(d => d.UsuarioId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Local_Usuario");
            });

            modelBuilder.Entity<Reserva>(entity =>
            {
                entity.HasKey(e => e.ReservaId);

                entity.Property(e => e.FechaAlta).HasColumnType("datetime");

                entity.HasOne(d => d.Evento)
                    .WithMany(p => p.Reserva)
                    .HasForeignKey(d => d.EventoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Reserva_Evento");

                entity.HasOne(d => d.Usuario)
                    .WithMany(p => p.Reserva)
                    .HasForeignKey(d => d.UsuarioId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Reserva_Usuario");
            });

            modelBuilder.Entity<TipoEvento>(entity =>
            {
                entity.Property(e => e.Nombre).IsRequired();
            });

            modelBuilder.Entity<TipoLocal>(entity =>
            {
                entity.HasKey(e => e.TipoLocald);

                entity.Property(e => e.Nombre).IsRequired();
            });

            modelBuilder.Entity<TipoUsuario>(entity =>
            {
                entity.Property(e => e.Nombre).IsRequired();
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.Property(e => e.Apellido).IsRequired();

                entity.Property(e => e.ApyNomb).IsRequired();

                entity.Property(e => e.Clave).IsRequired();

                entity.Property(e => e.Dni).IsRequired();

                entity.Property(e => e.Email).IsRequired();

                entity.Property(e => e.Nombre).IsRequired();

                entity.Property(e => e.Telefono).IsRequired();

                entity.HasOne(d => d.TipoUsuarioNavigation)
                    .WithMany(p => p.Usuario)
                    .HasForeignKey(d => d.TipoUsuarioId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Usuario_TipoUsuario");
            });
        }
    }
}
