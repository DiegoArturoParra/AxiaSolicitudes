namespace AttentionAxia.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("rol")]
    public partial class Rol
    {
        public Rol()
        {
            Usuario = new HashSet<Usuario>();
        }
        [Column("id")]
        public int Id { get; set; }

        [Column("descripcion")]
        [Required]
        [Display(Name = "Descripci�n del rol")]
        [StringLength(100)]
        public string Descripcion { get; set; }
    
        public virtual ICollection<Usuario> Usuario { get; set; }
    }
}
