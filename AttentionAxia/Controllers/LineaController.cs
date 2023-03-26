using AttentionAxia.Helpers;
using AttentionAxia.Models;
using AttentionAxia.Repositories;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AttentionAxia.Controllers
{
    [Authorize(Roles = "Administrador-Axia")]
    public class LineaController : BaseController
    {
        private readonly LineaRepository _lineaRepository;

        public LineaController()
        {
            _lineaRepository = new LineaRepository(_db);
        }
        // GET: Lineas
        public async Task<ActionResult> Index()
        {
            return View(await _lineaRepository.GetAll());
        }

        // GET: Lineas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Lineas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Descripcion")] Linea linea)
        {
            if (ModelState.IsValid)
            {
                var existe = await _lineaRepository.AnyWithCondition(x => x.Descripcion.ToLower() == linea.Descripcion);
                if (existe)
                {
                    SetAlert(GetConstants.ALERT_ERROR);
                    SetMessage($"Ya existe un registro con la descripción {linea.Descripcion.ToLower()}");
                    return View(linea);
                }
                _lineaRepository.Insert(linea);
                await _lineaRepository.Save();
                SetAlert(GetConstants.ALERT_SUCCESS);
                SetMessage("Creado satisfactoriamente.");
                return RedirectToAction("Index");
            }

            return View(linea);
        }

        // GET: Lineas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            Linea linea = _lineaRepository.FindById(id);
            if (linea == null)
            {
                SetAlert(GetConstants.ALERT_ERROR);
                SetMessage("No existe el registro.");
                return RedirectToAction("Index");
            }
            return View(linea);
        }

        // POST: Lineas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Descripcion")] Linea linea)
        {
            if (ModelState.IsValid)
            {
                if (await _lineaRepository.AnyWithCondition(x => x.Descripcion.ToLower() == linea.Descripcion && x.Id != linea.Id))
                {
                    SetAlert(GetConstants.ALERT_ERROR);
                    SetMessage($"Ya existe un registro con la descripción {linea.Descripcion.ToUpper()}");
                    return View(linea);
                }
                _lineaRepository.Update(linea);
                await _lineaRepository.Save();
                SetAlert(GetConstants.ALERT_SUCCESS);
                SetMessage("Actualizado satisfactoriamente.");
                return RedirectToAction("Index");
            }
            return View(linea);
        }

        // GET: Lineas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            Linea linea = _lineaRepository.FindById(id);
            if (linea == null)
            {
                SetAlert(GetConstants.ALERT_ERROR);
                SetMessage("No existe el registro.");
                return RedirectToAction("Index");
            }
            return View(linea);
        }

        // POST: Lineas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {

            Linea linea = _lineaRepository.FindById(id);
            _lineaRepository.Delete(linea);
            await _lineaRepository.Save();
            SetAlert(GetConstants.ALERT_SUCCESS);
            SetMessage("Eliminado satisfactoriamente.");
            return RedirectToAction("Index");
        }
    }
}
