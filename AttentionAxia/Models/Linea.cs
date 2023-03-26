﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AttentionAxia.Models
{
    [Table("linea")]
    public class Linea
    {
        public Linea()
        {
            Responsables = new HashSet<Responsable>();
        }
        [Column("id")]
        public int Id { get; set; }
        [Column("descripcion")]
        [Required]
        [Display(Name = "Descripción")]
        [StringLength(500)]
        public string Descripcion { get; set; }
        public virtual ICollection<Responsable> Responsables { get; set; }
    }
}