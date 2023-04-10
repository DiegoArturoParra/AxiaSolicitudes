using System.ComponentModel.DataAnnotations.Schema;

namespace AttentionAxia.Models
{
    public partial class Sprint
    {
        [NotMapped]
        public string SiglaPeriodo => $"{this.Sigla} {this.Periodo} {this.FechaGeneracion:yyyy}";
    }
}