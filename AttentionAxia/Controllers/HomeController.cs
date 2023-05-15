using AttentionAxia.Repositories;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AttentionAxia.Controllers
{
    public class HomeController : BaseController
    {
        private readonly FestivosRepository _festivoRepository;
        public HomeController()
        {
            _festivoRepository = new FestivosRepository(_db);
        }
        public ActionResult Index()
        {
            //await _festivoRepository.InsertHolidays();
            return View();
        }
    }
}