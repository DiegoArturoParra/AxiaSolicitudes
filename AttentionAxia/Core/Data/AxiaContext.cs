using AttentionAxia.Models;
using System.Data.Entity;

namespace AttentionAxia.Core.Data
{
    public partial class AxiaContext : DbContext
    {
        public AxiaContext() : base("name=AxiaContext")
        {

        }

        public AxiaContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {

        }
        public virtual DbSet<Rol> TablaRoles { get; set; }
        public virtual DbSet<Usuario> TablaUsuarios { get; set; }
        public virtual DbSet<Celula> TablaCelulas { get; set; }
        public virtual DbSet<Linea> TablaLineas { get; set; }
        public virtual DbSet<EstadoSolicitud> TablaEstados { get; set; }
        public virtual DbSet<Responsable> TablaResponsables { get; set; }
        public virtual DbSet<Solicitud> TablaSolicitudes { get; set; }
        public virtual DbSet<Sprint> TablaSprints { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Rol>()
                .Property(e => e.Descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<Rol>()
                .HasMany(e => e.Usuario)
                .WithRequired(e => e.Rol)
                .HasForeignKey(e => e.RolId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Usuario>()
                .Property(e => e.Nombres)
                .IsUnicode(false);

            modelBuilder.Entity<Usuario>()
                .Property(e => e.Apellidos)
                .IsUnicode(false);

            modelBuilder.Entity<Usuario>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<Usuario>()
                .Property(e => e.Clave)
                .IsUnicode(false);
            modelBuilder.Entity<Celula>()
              .HasMany(e => e.Responsables)
              .WithRequired(e => e.CelulaPertenece)
              .HasForeignKey(e => e.CelulaPerteneceId)
              .WillCascadeOnDelete(false);

            modelBuilder.Entity<Linea>()
               .HasMany(e => e.Responsables)
               .WithRequired(e => e.LineaPertenece)
               .HasForeignKey(e => e.LineaPerteneceId)
               .WillCascadeOnDelete(false);

            modelBuilder.Entity<Solicitud>()
                .Property(e => e.Iniciativa)
                .IsUnicode(false);

            modelBuilder.Entity<EstadoSolicitud>()
                .HasMany(e => e.DetalleSolicitudes)
                .WithRequired(e => e.Estado)
                .HasForeignKey(e => e.EstadoId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Responsable>()
                .Property(e => e.Nombres)
                .IsUnicode(false);

            modelBuilder.Entity<Responsable>()
                .HasMany(e => e.DetalleSolicitudes)
                .WithRequired(e => e.Responsable)
                .HasForeignKey(e => e.ResponsableId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Sprint>()
                .HasMany(e => e.DetalleSolicitudes)
                .WithRequired(e => e.Sprint)
                .HasForeignKey(e => e.SprintId)
                .WillCascadeOnDelete(false);
        }
    }
}
