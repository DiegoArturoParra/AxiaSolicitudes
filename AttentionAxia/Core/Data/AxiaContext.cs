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
        public virtual DbSet<Celula> TablaCelula { get; set; }
        public virtual DbSet<Linea> TablaLinea { get; set; }
        public virtual DbSet<Estados> TablaEstado { get; set; }
        public virtual DbSet<Responsables> TablaResponsables { get; set; }
        public virtual DbSet<Sprint> TablaSprint { get; set; }

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
        }
    }
}
