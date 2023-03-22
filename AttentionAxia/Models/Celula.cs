namespace AttentionAxia.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    [Table("celula")]
    public partial class Celula
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("descripcion")]
        [Required]
        [StringLength(500)]
        public string Descripcion { get; set; }
    }
}