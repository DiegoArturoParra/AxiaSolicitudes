using System.ComponentModel;

namespace AttentionAxia.Helpers
{
    public enum EstadosSolicitudEnum
    {
        None,
        [Description("Por hacer")]
        PorHacer,
        [Description("En progreso")]
        EnProgreso,
        [Description("Finalizado")]
        Finalizado
    }
}