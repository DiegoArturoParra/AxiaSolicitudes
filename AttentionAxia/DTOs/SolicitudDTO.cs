using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace AttentionAxia.DTOs
{
    public class CreateSolicitudDTO
    {
        public int ResponsableId { get; set; }
        public int EstadoId { get; set; }
        public int? SprintInicioId { get; set; }
        public int? SprintFinId { get; set; }
        public int CelulaId { get; set; }
        public string FechaInicial { get; set; }
        public string FechaFinal { get; set; }
        public DateTime? FechaInicialParse => SprintInicioId.HasValue ? DateTime.ParseExact(FechaInicial, "dd/MM/yyyy", CultureInfo.InvariantCulture) : default(DateTime?);
        public DateTime? FechaFinalParse => SprintFinId.HasValue ? DateTime.ParseExact(FechaFinal, "dd/MM/yyyy", CultureInfo.InvariantCulture) : default(DateTime?);
        public string Iniciativa { get; set; }
    }

    public class EditSolicitudByStateDTO
    {
        public int SolicitudId { get; set; }
        public int EstadoId { get; set; }
        public byte Avance { get; set; }
    }

    public class EditSolicitudDTO
    {
        public int SolicitudId { get; set; }
        public int ResponsableId { get; set; }
        public int EstadoId { get; set; }
        public int SprintInicioId { get; set; }
        public int SprintFinId { get; set; }
        public int CelulaId { get; set; }
        public string FechaInicial { get; set; }
        public string FechaFinal { get; set; }
        public DateTime FechaInicialParse => DateTime.ParseExact(FechaInicial, "dd/MM/yyyy", CultureInfo.InvariantCulture);
        public DateTime FechaFinalParse => DateTime.ParseExact(FechaFinal, "dd/MM/yyyy", CultureInfo.InvariantCulture);
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
        public DateTime? SprintInicioFechaGeneracion { get; set; }
        [JsonIgnore]
        public DateTime? SprintFinFechaGeneracion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaComienzo { get; set; }
        public DateTime? FechaFinalizacion { get; set; }
        public string Celula { get; set; }
        public string Linea { get; set; }
        public short? CycleTimeEsperado { get; set; }
        public short? CycleTimeReal { get; set; }
        public short? LeadTime { get; set; }
        public short? PorcentajeDeCumplimiento { get; set; }
        public string Iniciativa { get; set; }
        public string NombreArchivo { get; set; }
        public string RutaArchivo { get; set; }
        public byte Avance { get; set; }
        public string DateCreated => FechaCreacion.ToString("dd/MM/yyyy");
        public string DateProcess => FechaComienzo.HasValue ? FechaComienzo.Value.ToString("dd/MM/yyyy") : "N/A";
        public string DateFinish => FechaFinalizacion.HasValue ? FechaFinalizacion.Value.ToString("dd/MM/yyyy") : "N/A";
        public string SprintInicioFullText => SprintInicioFechaGeneracion.HasValue ? string.Format("{0} - {1:dd/MM/yyyy}", SprintInicio, SprintInicioFechaGeneracion) : "N/A";
        public string SprintFinFullText => SprintFinFechaGeneracion.HasValue ? string.Format("{0} - {1:dd/MM/yyyy}", SprintFin, SprintFinFechaGeneracion) : "N/A";
    }
    public class ListarSolicitudDTO : PaginadorDTO
    {
        public IEnumerable<SolicitudDTO> Solicitudes { get; set; }
    }
}