namespace AttentionAxia.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("usuario")]
    public partial class Usuario
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("nombre")]
        [Required]
        [StringLength(100)]
        public string Nombres { get; set; }

        [Column("apellido")]
        [Required]
        [StringLength(100)]
        public string Apellidos { get; set; }

        [Column("email")]
        [Required]
        [StringLength(100)]
        public string Email { get; set; }

        [Column("nick_name")]
        [Required]
        [StringLength(100)]
        public string NickName { get; set; }

        [Column("clave", TypeName = "text")]
        [Required]
        public string Clave { get; set; }

        [Column("rol_id")]
        public int RolId { get; set; }

        public virtual Rol Rol { get; set; }
    }
}
