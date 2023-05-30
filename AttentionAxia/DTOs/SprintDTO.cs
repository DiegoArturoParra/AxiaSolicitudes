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
        public string Periodo { get; set; }
        public bool Activo { get; set; }
        public string FechaInicial { get; set; }
        public string FechaFinal { get; set; }
        public DateTime FechaInicialParse => DateTime.ParseExact(FechaInicial, "dd/MM/yyyy", CultureInfo.InvariantCulture);
        public DateTime FechaFinalParse => DateTime.ParseExact(FechaFinal, "dd/MM/yyyy", CultureInfo.InvariantCulture);
    }
}