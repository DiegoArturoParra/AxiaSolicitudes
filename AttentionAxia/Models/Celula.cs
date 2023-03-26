namespace AttentionAxia.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    [Table("celula")]
    public partial class Celula
    {
        public Celula()
        {
            Responsables = new HashSet<Responsable>();
        }
        [Column("id")]
        public int Id { get; set; }
        [Column("descripcion")]
        [Required]
        [StringLength(500)]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }
        public virtual ICollection<Responsable> Responsables { get; set; }
    }
}