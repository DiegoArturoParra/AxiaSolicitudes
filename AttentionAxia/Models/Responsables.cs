using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AttentionAxia.Models
{
    [Table("reposanbles")]
    public class Responsables
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("nombres")]
        [Required]
        [StringLength(500)]
        public string Nombres { get; set; }
        [Column("celulaPerteneceId")]
        [Required]
        [StringLength(500)]
        public string CelulaPertenceId { get; set; }
        [Column("lineaPerteneceId")]
        [Required]
        [StringLength(500)]
        public string LineaPerteneceId { get; set; }
    }
}