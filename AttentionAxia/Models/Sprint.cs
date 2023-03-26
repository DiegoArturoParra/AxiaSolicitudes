using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AttentionAxia.Models
{
    [Table("sprint")]
    public class Sprint
    {
        public Sprint()
        {
            DetalleSolicitudes = new HashSet<Solicitud>();
        }
        [Column("id")]
        public int Id { get; set; }
        [Column("descripcion")]
        [Required]
        [StringLength(10)]
        public string Sigla { get; set; }
        [Column("periodo")]
        [Required]
        [StringLength(15)]
        public string Periodo { get; set; }
        [Column("fecha_generacion")]
        [Required]
        public DateTime FechaGeneracion { get; set; }
        public virtual ICollection<Solicitud> DetalleSolicitudes { get; set; }
    }
}