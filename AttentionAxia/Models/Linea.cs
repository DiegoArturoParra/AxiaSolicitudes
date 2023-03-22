using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AttentionAxia.Models
{
    [Table("linea")]
    public class Linea
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("descripcion")]
        [Required]
        [StringLength(500)]
        public string Descripcion { get; set; }
    }
}