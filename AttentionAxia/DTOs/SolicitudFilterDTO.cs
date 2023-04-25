using System;

namespace AttentionAxia.DTOs
{
    public class SolicitudFilterDTO
    {
        public int? Estado { get; set; }
        public int? Sprint { get; set; }
        public int? Linea { get; set; }
        public int? Celula { get; set; }
        public int? Responsable { get; set; }
        public DateTime? FechaInicial { get; set; }
        public DateTime? FechaFinal { get; set; }
        public int? Avance { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 25;
    }
}