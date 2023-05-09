using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AttentionAxia.Models
{
    [Table("EXPLORA_FESTIVOS")]
    public class Festivo
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("festivo", TypeName = "Date")]
        public DateTime FechaFestivo { get; set; }
    }
}