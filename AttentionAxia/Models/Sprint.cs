using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AttentionAxia.Models
{
    [Table("sprint")]
    public class Sprint
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("descripcion")]
        [Required]
        [StringLength(10)]
        public string Sigla { get; set; }
        [Column("periodo")]
        [Required]
        [StringLength(15)]
        public string Periodo { get; set; }
        [Column("fechageneracion")]
        [Required]
        public DateTime FechaGeneracion { get; set; }
    }
}