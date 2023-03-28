using AttentionAxia.Core.Data;
using AttentionAxia.Helpers;
using AttentionAxia.Models;
using AttentionAxia.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
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
            return View(await _sprintRepository.GetAll());
        }

        // GET: Sprints/Details/5       

        // GET: Sprints/Create
        public ActionResult Create()
        {
            string ultimaSigla = _sprintRepository.Table.OrderByDescending(x => x.Id).Take(1).Select(x => x.Sigla).ToList().FirstOrDefault();
            ViewBag.sigla = ultimaSigla.Substring(ultimaSigla.Length - 1);
            return View();
        }

        // POST: Sprints/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Sigla,Periodo,FechaGeneracion")] List<Sprint> sprint)
        {
            if (ModelState.IsValid)
            {
                foreach (var item in sprint)
                {
                    var existe = await _sprintRepository.AnyWithCondition(x => x.Sigla.ToLower() == item.Sigla);
                    if (existe)
                    {
                        SetAlert(GetConstants.ALERT_ERROR);
                        SetMessage($"Ya existe un registro con la descripción {item.Sigla.ToLower()}");
                        return View(sprint);
                    }
                    _sprintRepository.Insert(item);
                    await _sprintRepository.Save();
                }
                SetAlert(GetConstants.ALERT_SUCCESS);
                SetMessage("Creado satisfactoriamente.");
                return RedirectToAction("Index");
            }
            return View();
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
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Sigla,Periodo,FechaGeneracion")] Sprint sprint)
        {
            if (ModelState.IsValid)
            {
                if (await _sprintRepository.AnyWithCondition(x => x.Sigla.ToLower() == sprint.Sigla && x.Id != sprint.Id))
                {
                    SetAlert(GetConstants.ALERT_ERROR);
                    SetMessage($"Ya existe un registro con la descripción {sprint.Sigla.ToUpper()}");
                    return View(sprint);
                }
                _sprintRepository.Update(sprint);
                await _sprintRepository.Save();
                SetAlert(GetConstants.ALERT_SUCCESS);
                SetMessage("Actualizado satisfactoriamente.");
                return RedirectToAction("Index");
            }
            return View(sprint);
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
