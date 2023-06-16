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
        public int? SprintInicioId { get; set; }
        [Column("sprint_final_id")]
        public int? SprintFinId { get; set; }
        [Column("celula_id")]
        public int CelulaId { get; set; }
        [Column("iniciativa")]
        [Required(ErrorMessage = "Campo requerido.")]
        [StringLength(5000)]
        public string Iniciativa { get; set; }
        [Column("fecha_inicio_planeada")]
        public DateTime? FechaInicioPlaneada { get; set; }
        [Column("fecha_fin_planeada")]
        public DateTime? FechaFinPlaneada { get; set; }
        [Required(ErrorMessage = "Fecha Creación requerida.")]
        [Column("fecha_creacion_solicitud")]
        public DateTime FechaCreacionSolicitud { get; set; }
        [Column("fecha_inicio_real")]
        public DateTime? FechaInicioReal { get; set; }
        [Column("fecha_fin_real")]
        public DateTime? FechaFinReal { get; set; }
        [Column("avance_porcentual")]
        public byte Avance { get; set; }
        [Column("porcentaje_cumplimiento")]
        public short? PorcentajeCumplimiento { get; set; }
        [Column("ruta_archivo")]
        public string RutaArchivo { get; set; }
        [Column("nombre_archivo")]
        public string NombreArchivo { get; set; }
        [Column("cycle_time_planeado")]
        public short? CycleTimePlaneado { get; set; }
        [Column("cycle_time_real")]
        public short? CycleTimeReal { get; set; }
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