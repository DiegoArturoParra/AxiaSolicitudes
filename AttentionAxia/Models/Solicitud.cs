using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AttentionAxia.Models
{

    [Table("DETALLE_SOLICITUD", Schema = "AXIA")]
    public partial class Solicitud
    {
        [Column("id_solicitud")]
        public int Id { get; set; }
        [Column("responsable_id")]
        public int ResponsableId { get; set; }
        [Column("estado_solicitud_id")]
        public int EstadoId { get; set; }
        [Column("sprint_inicial_id")]
        public int SprintInicioId { get; set; }
        [Column("sprint_final_id")]
        public int SprintFinId { get; set; }
        [Column("celula_id")]
        public int CelulaId { get; set; }
        [Column("iniciativa")]
        [Required(ErrorMessage = "Campo requerido.")]
        [StringLength(5000)]
        public string Iniciativa { get; set; }
        [Required(ErrorMessage = "Campo requerido.")]
        [Column("fecha_inicio_sprint")]
        public DateTime FechaInicioSprint { get; set; }
        [Required(ErrorMessage = "Campo requerido.")]
        [Column("fecha_fin_sprint")]
        public DateTime FechaFinSprint { get; set; } 
        [Required(ErrorMessage = "Fecha Creación requerida.")]
        [Column("fecha_creacion_solicitud")]
        public DateTime FechaCreacionSolicitud { get; set; }
        [Column("fecha_comienzo_solicitud")]
        public DateTime? FechaComienzoSolicitud { get; set; }
        [Column("fecha_finalizacion_solicitud")]
        public DateTime? FechaFinalizacionSolicitud { get; set; }
        [Column("avance_porcentual")]
        public byte Avance { get; set; }
        [Column("ruta_archivo")]
        public string RutaArchivo { get; set; }
        [Column("nombre_archivo")]
        public string NombreArchivo { get; set; }
        [Column("cycle_time")]
        public short? CycleTime { get; set; }
        [Column("lead_time")]
        public short? LeadTime { get; set; }
        public virtual EstadoSolicitud Estado { get; set; }
        public virtual Responsable Responsable { get; set; }
        [ForeignKey("SprintInicioId")]
        public virtual Sprint SprintInicio { get; set; }
        [ForeignKey("SprintFinId")]
        public virtual Sprint SprintFin { get; set; }
        public virtual Celula Celula { get; set; }
    }
}