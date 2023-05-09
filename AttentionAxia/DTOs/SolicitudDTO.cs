using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace AttentionAxia.DTOs
{
    public class CreateSolicitudDTO
    {
        public int ResponsableId { get; set; }
        public int EstadoId { get; set; }
        public int SprintInicioId { get; set; }
        public int SprintFinId { get; set; }
        public int CelulaId { get; set; }
        public DateTime FechaInicial { get; set; }
        public DateTime FechaFinal { get; set; }
        public string Iniciativa { get; set; }
    }

    public class EditSolicitudDTO
    {
        public int SolicitudId { get; set; }
        public int ResponsableId { get; set; }
        public int EstadoId { get; set; }
        public int SprintInicioId { get; set; }
        public int SprintFinId { get; set; }
        public int CelulaId { get; set; }
        public DateTime FechaInicial { get; set; }
        public DateTime FechaFinal { get; set; }
        public string Iniciativa { get; set; }
        public byte Avance { get; set; }
    }

    public class SolicitudDTO
    {
        public int Id { get; set; }
        public string Responsable { get; set; }
        public string Estado { get; set; }
        public string ColorEstado { get; set; }
        public int EstadoId { get; set; }
        public string SprintFin { get; set; }
        public string SprintInicio { get; set; }
        [JsonIgnore]
        public DateTime SprintInicioFechaGeneracion { get; set; }
        [JsonIgnore]
        public DateTime SprintFinFechaGeneracion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaComienzo { get; set; }
        public DateTime? FechaFinalizacion { get; set; }
        public string Celula { get; set; }
        public string Linea { get; set; }
        public short? CycleTime { get; set; }
        public short? LeadTime { get; set; }
        public string Iniciativa { get; set; }
        public string NombreArchivo { get; set; }
        public string RutaArchivo { get; set; }
        public byte Avance { get; set; }
        public string SprintInicioFullText => string.Format("{0} - {1:yyyy}", SprintInicio, SprintInicioFechaGeneracion);
        public string SprintFinFullText => string.Format("{0} - {1:yyyy}", SprintFin, SprintFinFechaGeneracion);
    }
    public class ListarSolicitudDTO : PaginadorDTO
    {
        public IEnumerable<SolicitudDTO> Solicitudes { get; set; }
    }
}