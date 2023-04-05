using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AttentionAxia.Models
{
    [Table("ESTADO_SOLICITUD", Schema = "AXIA")]
    public class EstadoSolicitud
    {
        public EstadoSolicitud()
        {
            DetalleSolicitudes = new HashSet<Solicitud>();
        }
        [Column("id_estado_solicitud")]
        public int Id { get; set; }
        [Column("descripcion")]
        [Required(ErrorMessage = "Campo requerido.")]
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