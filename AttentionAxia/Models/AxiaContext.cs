using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace AttentionAxia.Models
{
    public partial class AxiaContext : DbContext
    {
        public AxiaContext()
            : base("name=AxiaContext")
        {
        }

        public virtual DbSet<Rol> rol { get; set; }
        public virtual DbSet<Usuario> usuario { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Rol>()
                .Property(e => e.descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<Rol>()
                .HasMany(e => e.usuario)
                .WithRequired(e => e.rol)
                .HasForeignKey(e => e.rol_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Usuario>()
                .Property(e => e.nombre)
                .IsUnicode(false);

            modelBuilder.Entity<Usuario>()
                .Property(e => e.apellido)
                .IsUnicode(false);

            modelBuilder.Entity<Usuario>()
                .Property(e => e.email)
                .IsUnicode(false);

            modelBuilder.Entity<Usuario>()
                .Property(e => e.clave)
                .IsUnicode(false);

            modelBuilder.Entity<Usuario>()
                .Property(e => e.semilla)
                .IsUnicode(false);
        }
    }
}
