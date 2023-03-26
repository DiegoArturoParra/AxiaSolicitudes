using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AttentionAxia.Models
{
    [Table("responsable")]
    public class Responsable
    {
        public Responsable()
        {
            DetalleSolicitudes = new HashSet<Solicitud>();
        }

        [Column("id")]
        public int Id { get; set; }
        [Column("nombres")]
        [Required]
        [StringLength(500)]
        public string Nombres { get; set; }
        [Column("celula_id")]
        [Required]
        public int CelulaPerteneceId { get; set; }
        [Column("linea_id")]
        [Required]
        public int LineaPerteneceId { get; set; }
        public virtual Celula CelulaPertenece { get; set; }
        public virtual Linea LineaPertenece { get; set; }
        public virtual ICollection<Solicitud> DetalleSolicitudes { get; set; }
    }
}