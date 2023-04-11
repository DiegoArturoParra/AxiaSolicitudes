using System.Web.Routing;

namespace AttentionAxia.DTOs
{
    public class PaginadorDTO
    {
        public int PaginaActual { get; set; }
        public int TotalDeRegistros { get; set; }
        public int RegistrosPorPagina { get; set; }
        public RouteValueDictionary ValoresQueryString { get; set; }
    }
    public class PaginacionDTO
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 100;
    }
}