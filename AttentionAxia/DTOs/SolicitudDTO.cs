using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web;

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
        public string Celula { get; set; }
        public string Linea { get; set; }
        public string Iniciativa { get; set; }
        public string Archivo { get; set; }
        public byte Avance { get; set; }
        [JsonIgnore]
        public DateTime FechaInicial { get; set; }
        [JsonIgnore]
        public DateTime FechaFinal { get; set; }
        public String FechaInicialSprint => string.Format("{0:dd/MM/yyyy}", FechaInicial);
        public String FechaFinSprint => string.Format("{0:dd/MM/yyyy}", FechaFinal);
        public string SprintInicioFullText => string.Format("{0} - {1:yyyy}", SprintInicio, SprintInicioFechaGeneracion);
        public string SprintFinFullText => string.Format("{0} - {1:yyyy}", SprintFin, SprintFinFechaGeneracion);
    }
    public class ListarSolicitudDTO : PaginadorDTO
    {
        public IEnumerable<SolicitudDTO> Solicitudes { get; set; }
    }
}