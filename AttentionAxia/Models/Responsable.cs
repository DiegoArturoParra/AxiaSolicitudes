using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AttentionAxia.Models
{
    [Table("RESPONSABLE", Schema = "AXIA")]
    public partial class Responsable
    {
        public Responsable()
        {
            DetalleSolicitudes = new HashSet<Solicitud>();
        }

        [Column("id_responsable")]
        public int Id { get; set; }
        [Column("nombres")]
        [Display(Name = "Nombre Completo")]
        [Required(ErrorMessage = "Campo requerido.")]
        [StringLength(500)]
        public string Nombres { get; set; }
        [Column("celula_trabajo_id")]
        [Required(ErrorMessage = "Campo requerido.")]
        [Display(Name = "Pertenece a la Célula")]
        public int CelulaPerteneceId { get; set; }
        [Column("linea_trabajo_id")]
        [Required(ErrorMessage = "Campo requerido.")]
        [Display(Name = "Pertenece a la Línea de trabajo")]
        public int LineaPerteneceId { get; set; }
        public virtual Celula CelulaPertenece { get; set; }
        public virtual Linea LineaPertenece { get; set; }
        public virtual ICollection<Solicitud> DetalleSolicitudes { get; set; }
    }
}