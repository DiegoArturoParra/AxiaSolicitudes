using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AttentionAxia.DTOs
{
    public class CreateSolicitudDTO
    {
        public int ResponsableId { get; set; }
        public int EstadoId { get; set; }
        public int SprintId { get; set; }
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
        public string Sprint { get; set; }
        public string Celula { get; set; }
        public string Linea { get; set; }
        public string Iniciativa { get; set; }
        public byte Avance { get; set; }
        [JsonIgnore]
        public DateTime FechaInicial { get; set; }
        [JsonIgnore]
        public DateTime FechaFinal { get; set; }
        public String FechaInicialSprint
        {
            get
            {
                return string.Format("{0:dd/MM/yyyy}", this.FechaInicial);
            }
        }
        public String FechaFinSprint
        {
            get
            {
                return string.Format("{0:dd/MM/yyyy}", this.FechaFinal);
            }
        }
    }
    public class ListarSolicitudDTO: PaginadorDTO
    {
        public IEnumerable<SolicitudDTO> Solciitudes { get; set; }
    }
}