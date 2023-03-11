namespace AttentionAxia.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("usuario")]
    public partial class Usuario
    {
        public int id { get; set; }

        [Required]
        [StringLength(100)]
        public string nombre { get; set; }

        [Required]
        [StringLength(100)]
        public string apellido { get; set; }

        [Required]
        [StringLength(100)]
        public string email { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string clave { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string semilla { get; set; }

        public int rol_id { get; set; }

        public virtual Rol rol { get; set; }
    }
}
