using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AttentionAxia.Models
{
    [Table("LINEA_DE_TRABAJO", Schema = "AXIA")]
    public class Linea
    {
        public Linea()
        {
            Responsables = new HashSet<Responsable>();
        }
        [Column("id_linea_trabajo")]
        public int Id { get; set; }
        [Column("descripcion")]
        [Required(ErrorMessage = "Campo requerido.")]
        [Display(Name = "Descripción")]
        [StringLength(500)]
        public string Descripcion { get; set; }
        public virtual ICollection<Responsable> Responsables { get; set; }
    }
}