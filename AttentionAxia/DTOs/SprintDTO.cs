using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace AttentionAxia.DTOs
{
    public class SprintDTO
    {
        [Required(ErrorMessage = "Campo requerido.")]
        public int Id { get; set; }
        [Required(ErrorMessage = "Campo requerido.")]
        [Display(Name = "Sigla")]
        [StringLength(10)]
        public string Sigla { get; set; }
        [Display(Name = "Periodo")]
        public string Period { get; set; }
        public bool Activo { get; set; }
        public string FechaInicial { get; set; }
        public string FechaFinal { get; set; }
        public DateTime FechaInicialParse => !string.IsNullOrWhiteSpace(this.FechaInicial) ?
                    DateTime.ParseExact(FechaInicial, "dd/MM/yyyy", CultureInfo.InvariantCulture) : DateTime.MinValue;
        public DateTime FechaFinalParse => !string.IsNullOrWhiteSpace(this.FechaFinal) ?
            DateTime.ParseExact(FechaFinal, "dd/MM/yyyy", CultureInfo.InvariantCulture) : DateTime.MinValue;
    }
    public class CreateSprintDTO
    {
        public int CantidadSprints { get; set; }
        public string Periodo { get; set; }
        public string Sigla { get; set; }
        public int DuracionSprint { get; set; }
        public string FechaInicio { get; set; }
        public DateTime FechaInicialParse => !string.IsNullOrWhiteSpace(this.FechaInicio) ?
                 DateTime.ParseExact(FechaInicio, "dd/MM/yyyy", CultureInfo.InvariantCulture) : DateTime.MinValue;
    }
}
