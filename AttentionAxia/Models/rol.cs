namespace AttentionAxia.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ROL", Schema = "AXIA")]
    public partial class Rol
    {
        public Rol()
        {
            Usuario = new HashSet<Usuario>();
        }
        [Column("id_rol")]
        public int Id { get; set; }

        [Column("descripcion")]
        [Required(ErrorMessage = "Campo requerido.")]
        [Display(Name = "Descripción del rol")]
        [StringLength(100)]
        public string Descripcion { get; set; }
    
        public virtual ICollection<Usuario> Usuario { get; set; }
    }
}
