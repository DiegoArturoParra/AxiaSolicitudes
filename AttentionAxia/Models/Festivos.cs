using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AttentionAxia.Models
{
    [Table("EXPLORA_FESTIVOS")]
    public class Festivo
    {
        [Column("id")]
        public int Id { get; set; }
        [Display(Name = "Fecha del festivo")]
        [Column("festivo", TypeName = "Date")]
        public DateTime FechaFestivo { get; set; }
        [NotMapped]
        public string FechaFestivoFormat => $"{this.FechaFestivo:dddd, dd MMMM yyyy}";
    }
}