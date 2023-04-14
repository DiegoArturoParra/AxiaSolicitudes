using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace AttentionAxia.Models
{

    [Table("CELULA_INICIATIVA", Schema = "AXIA")]
    public partial class Celula
    {
        public Celula()
        {
            Solicitudes = new HashSet<Solicitud>();
        }
        [Column("id_celula")]
        public int Id { get; set; }
        [Column("descripcion")]
        [Required(ErrorMessage = "Campo requerido.")]
        [StringLength(500)]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }
        public virtual ICollection<Solicitud> Solicitudes { get; set; }
    }
}