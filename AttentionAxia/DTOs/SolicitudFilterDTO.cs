using System;

namespace AttentionAxia.DTOs
{
    public class SolicitudFilterDTO
    {
        public int? EstadoSolicitudId { get; set; }
        public int? SprintId { get; set; }
        public int? LineaId { get; set; }
        public int? CelulaId { get; set; }
        public int? ResponsableId { get; set; }
        public DateTime? FechaInicial { get; set; }
        public DateTime? FechaFinal { get; set; }
        public int? Avance { get; set; }
        public PaginacionDTO Paginacion { get; set; } = new PaginacionDTO();
    }
}