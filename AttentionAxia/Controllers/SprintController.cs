using AttentionAxia.Helpers;
using AttentionAxia.Models;
using AttentionAxia.Repositories;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AttentionAxia.Controllers
{
    [Authorize(Roles = "Administrador-Axia")]
    public class SprintController : BaseController
    {
        private SprintRepository _sprintRepository;
        public SprintController()
        {
            _sprintRepository = new SprintRepository(_db);
        }

        // GET: Sprints
        public async Task<ActionResult> Index()
        {
            GetYears();
            return View(await _sprintRepository.GetAll());
        }

        private void GetYears()
        {
            var fechaActual = DateTime.Now;
            var years = Enumerable.Range(0, 4).Select(x => fechaActual.AddYears(-x).ToString("yyyy")).ToList();
            ViewBag.DDL_Years = new SelectList(years, "Year");
        }

        // GET: Sprints/Create
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Sprints/CreateMultipleSprints
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CrearMultipleSprints([Bind(Include = "Id,Sigla,Periodo,FechaGeneracion")] Sprint sprint, int CantidadSprints)
        {
            if (ModelState.IsValid)
            {
                var response = await _sprintRepository.CreateMultipeSprints(sprint, CantidadSprints);
                if (!response.IsSuccess)
                {
                    SetAlert(GetConstants.ALERT_ERROR);
                    SetMessage(response.Message);
                    return Json(new { response.IsSuccess, response.Message });
                }
                SetAlert(GetConstants.ALERT_SUCCESS);
                SetMessage("Creado satisfactoriamente.");
                return Json(new { response.IsSuccess, response.Message });
            }
            return Json(false);
        }

        // GET: Sprints/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            Sprint sprint = _sprintRepository.FindById(id);
            if (sprint == null)
            {
                SetAlert(GetConstants.ALERT_ERROR);
                SetMessage("No existe el registro.");
                return RedirectToAction("Index");
            }
            return View(sprint);
        }

        // POST: Sprints/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Sigla,Periodo,FechaGeneracion")] Sprint sprint)
        {
            if (ModelState.IsValid)
            {
                if (await _sprintRepository.AnyWithCondition(x => x.Sigla.ToLower() == sprint.Sigla && x.Id != sprint.Id))
                {
                    SetAlert(GetConstants.ALERT_WARNING);
                    SetMessage($"Ya existe un registro con la descripción {sprint.Sigla.ToUpper()}");
                    return View(sprint);
                }
                sprint.FechaGeneracion = DateTime.Now;
                _sprintRepository.Update(sprint);
                await _sprintRepository.Save();
                SetAlert(GetConstants.ALERT_SUCCESS);
                SetMessage("Actualizado satisfactoriamente.");
                return RedirectToAction("Index");
            }
            return View(sprint);
        }


        [HttpPost]
        public async Task<JsonResult> DeleteMultiple(string Year, string Period)
        {

            var response = await _sprintRepository.DeleteMultipeSprints(Year, Period);
            if (!response.IsSuccess)
            {
                SetAlert(GetConstants.ALERT_WARNING);
                SetMessage(response.Message);
                return Json(new { response.IsSuccess, response.Message });
            }
            SetAlert(GetConstants.ALERT_SUCCESS);
            SetMessage(response.Message);
            return Json(new { response.IsSuccess, response.Message });
        }


        // GET: Sprints/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            Sprint sprint = _sprintRepository.FindById(id);
            if (sprint == null)
            {
                SetAlert(GetConstants.ALERT_ERROR);
                SetMessage("No existe el registro.");
                return RedirectToAction("Index");
            }
            return View(sprint);
        }

        // POST: Sprints/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Sprint sprint = _sprintRepository.FindById(id);
            _sprintRepository.Delete(sprint);
            await _sprintRepository.Save();
            SetAlert(GetConstants.ALERT_SUCCESS);
            SetMessage("Eliminado satisfactoriamente.");
            return RedirectToAction("Index");
        }
    }
}
