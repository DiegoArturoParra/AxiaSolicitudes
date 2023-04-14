using AttentionAxia.Helpers;
using AttentionAxia.Models;
using AttentionAxia.Repositories;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AttentionAxia.Controllers
{
    public class ResponsablesController : BaseController
    {
        private ResponsableRepository _responsableRepository;
        private CelulaRepository _celulaRepository;
        private LineaRepository _lineaRepository;
        public ResponsablesController()
        {
            _responsableRepository = new ResponsableRepository(_db);
            _celulaRepository = new CelulaRepository(_db);
            _lineaRepository = new LineaRepository(_db);
        }

        // GET: Responsables
        public ActionResult Index()
        {
            var tablaResponsables = _responsableRepository.Table.Include(r => r.LineaPertenece);
            return View(tablaResponsables.ToList());
        }

        // GET: Responsables/Create
        public async Task<ActionResult> Create()
        {
            ViewBag.DDL_Lineas = new SelectList(await _lineaRepository.Table.OrderBy(x => x.Descripcion).ToListAsync(), "Id", "Descripcion");
            return View();
        }

        // POST: Responsables/Create.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Nombres,LineaPerteneceId")] Responsable responsable)
        {
            if (ModelState.IsValid)
            {
                var existe = await _responsableRepository.AnyWithCondition(r => r.Nombres.ToUpper() == responsable.Nombres.ToUpper());
                if (existe)
                {
                    SetAlert(GetConstants.ALERT_ERROR);
                    SetMessage(message: $"Ya existe un registro con el nombre {responsable.Nombres.ToLower()}");
                    return View(responsable);
                }

                _responsableRepository.Insert(responsable);
                await _responsableRepository.Save();
                SetAlert(GetConstants.ALERT_SUCCESS);
                SetMessage("Creado satisfactoriamente.");
                return RedirectToAction("Index");
            }
            if (responsable.LineaPerteneceId > 0)
            {
                ViewBag.DDL_Lineas = new SelectList(await _lineaRepository.Table.OrderBy(x => x.Descripcion).ToListAsync(), "Id", "Descripcion", responsable.LineaPerteneceId);
            }
            else
            {
                ViewBag.DDL_Lineas = new SelectList(await _lineaRepository.Table.OrderBy(x => x.Descripcion).ToListAsync(), "Id", "Descripcion");
            }
            return View();
        }

        // GET: Responsables/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            Responsable responsable = _responsableRepository.FindById(id);
            if (responsable == null)
            {
                SetAlert(GetConstants.ALERT_ERROR);
                SetMessage("No existe el registro.");
                return RedirectToAction("Index");
            }
            ViewBag.DDL_Lineas = new SelectList(await _lineaRepository.Table.OrderBy(x => x.Descripcion).ToListAsync(), "Id", "Descripcion", responsable.LineaPerteneceId);
            return View(responsable);
        }

        // POST: Responsables/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Nombres,CelulaPerteneceId,LineaPerteneceId")] Responsable responsable)
        {
            if (ModelState.IsValid)
            {
                if (await _lineaRepository.AnyWithCondition(x => x.Descripcion.ToLower() == responsable.Nombres && x.Id != responsable.Id))
                {
                    SetAlert(GetConstants.ALERT_ERROR);
                    SetMessage($"Ya existe un registro con la descripción {responsable.Nombres.ToUpper()}");
                    return View(responsable);
                }
                _responsableRepository.Update(responsable);
                await _responsableRepository.Save();
                SetAlert(GetConstants.ALERT_SUCCESS);
                SetMessage("Actualizado satisfactoriamente.");
                return RedirectToAction("Index");
            }
            ViewBag.DDL_Lineas = new SelectList(await _lineaRepository.Table.OrderBy(x => x.Descripcion).ToListAsync(), "Id", "Descripcion", responsable.LineaPerteneceId);
            return View(responsable);
        }

        // GET: Responsables/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            Responsable responsable = _responsableRepository.FindById(id);
            if (responsable == null)
            {
                SetAlert(GetConstants.ALERT_ERROR);
                SetMessage("No existe el registro.");
                return RedirectToAction("Index");
            }
            bool haySolicitudes = await new SolicitudRepository(_db).AnyWithCondition(x => x.ResponsableId == responsable.Id);
            if (haySolicitudes)
            {
                SetAlert(GetConstants.ALERT_WARNING);
                SetMessage($"Hay solicitudes vinculadas de la persona {responsable.Nombres}, elimine toda vinculación.");
                return RedirectToAction("Index");
            }
            return View(responsable);
        }

        // POST: Responsables/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {

            Responsable responsable = _responsableRepository.FindById(id);
            if (responsable == null)
            {
                SetAlert(GetConstants.ALERT_ERROR);
                SetMessage("No existe el registro.");
                return RedirectToAction("Index");
            }
            bool haySolicitudes = await new SolicitudRepository(_db).AnyWithCondition(x => x.ResponsableId == responsable.Id);
            if (haySolicitudes)
            {
                SetAlert(GetConstants.ALERT_WARNING);
                SetMessage($"Hay solicitudes vinculadas de la persona {responsable.Nombres}, elimine toda vinculación.");
                return RedirectToAction("Index");
            }
            _responsableRepository.Delete(responsable);
            await _responsableRepository.Save();
            SetAlert(GetConstants.ALERT_SUCCESS);
            SetMessage("Eliminado satisfactoriamente.");
            return RedirectToAction("Index");
        }
    }
}
