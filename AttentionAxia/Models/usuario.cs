namespace AttentionAxia.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("USUARIO", Schema = "AXIA")]
    public partial class Usuario
    {
        [Column("id_usuario")]
        public int Id { get; set; }

        [Column("nombre")]
        [Required(ErrorMessage = "Campo requerido.")]
        [StringLength(100)]
        public string Nombres { get; set; }

        [Column("apellido")]
        [Required(ErrorMessage = "Campo requerido.")]
        [StringLength(100)]
        public string Apellidos { get; set; }

        [Column("email")]
        [Required(ErrorMessage = "Campo requerido.")]
        [StringLength(100)]
        public string Email { get; set; }

        [Column("nick_name")]
        [Required(ErrorMessage = "Campo requerido.")]
        [StringLength(100)]
        public string NickName { get; set; }

        [Column("clave", TypeName = "text")]
        [Required(ErrorMessage = "Campo requerido.")]
        public string Clave { get; set; }

        [Column("rol_id")]
        [Required(ErrorMessage = "Campo requerido.")]
        public int RolId { get; set; }

        public virtual Rol Rol { get; set; }
    }
}
