using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AttentionAxia.Models
{
    [Table("estado")]
    public class Estado
    {
        public Estado()
        {
            DetalleSolicitudes = new HashSet<Solicitud>();
        }
        [Column("id")]
        public int Id { get; set; }
        [Column("descripcion")]
        [Required]
        [Display(Name = "Descripción")]
        [StringLength(500)]
        public string Descripcion { get; set; }
        [Column("nivel")]
        [Required]
        [StringLength(7)]
        public string Nivel { get; set; }
        public virtual ICollection<Solicitud> DetalleSolicitudes { get; set; }
    }
}