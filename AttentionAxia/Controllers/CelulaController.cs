using AttentionAxia.Helpers;
using AttentionAxia.Models;
using AttentionAxia.Repositories;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AttentionAxia.Controllers
{
    [Authorize(Roles = "Administrador-Axia")]
    public class CelulaController : BaseController
    {
        private readonly CelulaRepository _celulaRepository;
        public CelulaController()
        {
            _celulaRepository = new CelulaRepository(_db);
        }
        // GET: Celula
        public async Task<ActionResult> Index()
        {
            return View(await _celulaRepository.GetAll());
        }

        // GET: Celula/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Celula/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Descripcion")] Celula celula)
        {
            if (ModelState.IsValid)
            {
                var existe = await _celulaRepository.AnyWithCondition(x => x.Descripcion.ToLower() == celula.Descripcion);
                if (existe)
                {
                    SetAlert(GetConstants.ALERT_ERROR);
                    SetMessage($"Ya existe un registro con la descripción {celula.Descripcion.ToLower()}");
                    return View(celula);
                }
                _celulaRepository.Insert(celula);
                await _celulaRepository.Save();
                SetAlert(GetConstants.ALERT_SUCCESS);
                SetMessage("Creado satisfactoriamente.");
                return RedirectToAction("Index");
            }

            return View(celula);
        }

        // GET: Celula/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            Celula celula = _celulaRepository.FindById(id);
            if (celula == null)
            {
                SetAlert(GetConstants.ALERT_ERROR);
                SetMessage("No existe el registro.");
                return RedirectToAction("Index");
            }
            return View(celula);
        }

        // POST: Celula/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Descripcion")] Celula celula)
        {
            if (ModelState.IsValid)
            {
                if (await _celulaRepository.AnyWithCondition(x => x.Descripcion.ToLower() == celula.Descripcion && x.Id != celula.Id))
                {
                    SetAlert(GetConstants.ALERT_ERROR);
                    SetMessage($"Ya existe un registro con la descripción {celula.Descripcion.ToUpper()}");
                    return View(celula);
                }
                _celulaRepository.Update(celula);
                await _celulaRepository.Save();
                SetAlert(GetConstants.ALERT_SUCCESS);
                SetMessage("Actualizado satisfactoriamente.");
                return RedirectToAction("Index");
            }
            return View(celula);
        }

        // GET: Celula/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            Celula celula = _celulaRepository.FindById(id);
            if (celula == null)
            {
                SetAlert(GetConstants.ALERT_ERROR);
                SetMessage("No existe el registro.");
                return RedirectToAction("Index");
            }
            bool haySolicitudes = await new SolicitudRepository(_db).AnyWithCondition(x => x.CelulaId == celula.Id);
            if (haySolicitudes)
            {
                SetAlert(GetConstants.ALERT_WARNING);
                SetMessage($"Hay solicitudes vinculadas a la célula {celula.Descripcion}, elimine toda vinculación.");
                return RedirectToAction("Index");
            }
            return View(celula);
        }

        // POST: Celula/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Celula celula = _celulaRepository.FindById(id);
            if (celula == null)
            {
                SetAlert(GetConstants.ALERT_ERROR);
                SetMessage("No existe el registro.");
                return RedirectToAction("Index");
            }
            bool haySolicitudes = await new SolicitudRepository(_db).AnyWithCondition(x => x.CelulaId == celula.Id);
            if (haySolicitudes)
            {
                SetAlert(GetConstants.ALERT_WARNING);
                SetMessage($"Hay solicitudes vinculadas a la célula {celula.Descripcion}, elimine toda vinculación.");
                return RedirectToAction("Index");
            }
            _celulaRepository.Delete(celula);
            await _celulaRepository.Save();
            SetAlert(GetConstants.ALERT_SUCCESS);
            SetMessage("Eliminado satisfactoriamente.");
            return RedirectToAction("Index");
        }
    }
}
