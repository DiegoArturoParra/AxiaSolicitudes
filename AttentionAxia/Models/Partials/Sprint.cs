using System.ComponentModel.DataAnnotations.Schema;

namespace AttentionAxia.Models
{
    public partial class Sprint
    {
        [NotMapped]
        public string DescripcionSprint => $"{this.Sigla} {this.Periodo} {this.FechaGeneracion:yyyy}";
        [NotMapped]
        public string FechaInicioParse => this.FechaInicio.HasValue ? $"{this.FechaInicio:dd/MM/yyyy}" : "N/A";
        [NotMapped]
        public string FechaFinParse => this.FechaFin.HasValue ? $"{this.FechaFin:dd/MM/yyyy}" : "N/A";

    }
}