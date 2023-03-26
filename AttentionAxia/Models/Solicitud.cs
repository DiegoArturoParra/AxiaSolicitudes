using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AttentionAxia.Models
{

    [Table("detalle_solicitud")]
    public class Solicitud
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("responsable_id")]
        public int ResponsableId { get; set; }
        [Column("estado_id")]
        public int EstadoId { get; set; }
        [Column("sprint_id")]
        public int SprintId { get; set; }
        [Column("iniciativa")]
        [Required]
        [StringLength(5000)]
        public string Iniciativa { get; set; }
        [Required]
        [Column("fecha_inicio_sprint")]
        public DateTime FechaInicioSprint { get; set; }
        [Required]
        [Column("fecha_fin_sprint")]
        public DateTime FechaFinSprint { get; set; }
        [Column("avance")]
        public byte Avance { get; set; }
        public virtual Estado Estado { get; set; }
        public virtual Responsable Responsable { get; set; }
        public virtual Sprint Sprint { get; set; }
    }
}