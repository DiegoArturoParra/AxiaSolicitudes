using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AttentionAxia.Models
{
    [Table("estado")]
    public class Estados
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("descripcion")]
        [Required]
        [StringLength(500)]
        public string Descripcion { get; set; }
        [Column("nivel")]
        [Required]
        [StringLength(7)]
        public string Nivel { get; set; }
    }
}