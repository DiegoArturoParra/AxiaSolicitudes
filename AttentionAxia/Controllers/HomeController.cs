using AttentionAxia.Helpers;
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
        public async Task<ActionResult> Index()
        {
            var data = await _festivoRepository.GetAll();
            return View(data);
        }
        [HttpPost]
        [Authorize(Roles = "Administrador-Axia")]
        public async Task<ActionResult> GenerarFestivos()
        {
            var response = await _festivoRepository.InsertHolidays();
            if (!response.IsSuccess)
            {
                SetAlert(GetConstants.ALERT_ERROR);
                SetMessage(response.Message);
                return RedirectToAction("Index");
            }
            SetAlert(GetConstants.ALERT_SUCCESS);
            SetMessage(response.Message);
            return RedirectToAction("Index");
        }
    }
}