using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AttentionAxia.Models
{
    [Table("SPRINT", Schema = "AXIA")]
    public partial class Sprint
    {
        public Sprint()
        {
            DetalleSolicitudesSprintInicio = new HashSet<Solicitud>();
            DetalleSolicitudesSprintFin = new HashSet<Solicitud>();
        }
        [Column("id_sprint")]
        public int Id { get; set; }
        [Column("sigla")]
        [Required(ErrorMessage = "Campo requerido.")]
        [Display(Name = "Sigla")]
        [StringLength(10)]
        public string Sigla { get; set; }
        [Column("periodo")]
        [Required(ErrorMessage = "Campo requerido.")]
        [Display(Name = "Periodo")]
        [StringLength(15)]
        public string Periodo { get; set; }
        [Column("fecha_generacion")]
        [Display(Name = "Fecha Generación")]
        [Required(ErrorMessage = "Campo requerido.")]
        public DateTime FechaGeneracion { get; set; }
        public virtual ICollection<Solicitud> DetalleSolicitudesSprintInicio { get; set; }
        public virtual ICollection<Solicitud> DetalleSolicitudesSprintFin { get; set; }
    }
}