using AttentionAxia.Core.Data;
using AttentionAxia.Helpers;
using AttentionAxia.Models;
using AttentionAxia.Repositories;
using Microsoft.Ajax.Utilities;
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
            return View();
        }

        // POST: Sprints/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Sigla,Periodo,FechaGeneracion")] Sprint sprint, int CantidadSprints)
        {
            if (ModelState.IsValid)
            {
                var res1 = _sprintRepository.Table.Where(x => x.Periodo == "Q1").OrderByDescending(x => x.Id).Take(1).Select(x => x.Sigla).ToList().FirstOrDefault();
                string[] ultimaSiglaPeriodo1 = { "0", "0" };
                if (res1 != null) { ultimaSiglaPeriodo1 = res1.Split('-'); }
                var res2 = _sprintRepository.Table.Where(x => x.Periodo == "Q2").OrderByDescending(x => x.Id).Take(1).Select(x => x.Sigla).ToList().FirstOrDefault();
                string[] ultimaSiglaPeriodo2 = { "0", "0" };
                if (res2 != null) { ultimaSiglaPeriodo2 = res2.Split('-'); }
                var res3 = _sprintRepository.Table.Where(x => x.Periodo == "Q3").OrderByDescending(x => x.Id).Take(1).Select(x => x.Sigla).ToList().FirstOrDefault();
                string[] ultimaSiglaPeriodo3 = { "0", "0" };
                if (res3 != null) { ultimaSiglaPeriodo3 = res3.Split('-'); }
                var res4 = _sprintRepository.Table.Where(x => x.Periodo == "Q4").OrderByDescending(x => x.Id).Take(1).Select(x => x.Sigla).ToList().FirstOrDefault();
                string[] ultimaSiglaPeriodo4 = {"0", "0"};
                if (res4 != null) { ultimaSiglaPeriodo4 = res4.Split('-'); }

                int valorSiglaFinal = 0;

                if (sprint.Periodo == "Q1")
                {
                    valorSiglaFinal = Convert.ToInt32(ultimaSiglaPeriodo1[1]);
                }
                else if (sprint.Periodo == "Q2")
                {
                    valorSiglaFinal = Convert.ToInt32(ultimaSiglaPeriodo2[1]);
                }
                else if (sprint.Periodo == "Q3")
                {
                    valorSiglaFinal = Convert.ToInt32(ultimaSiglaPeriodo3[1]);
                }
                else
                {
                    valorSiglaFinal = Convert.ToInt32(ultimaSiglaPeriodo4[1]);
                }
                var sig = sprint.Sigla;
                for (int i = 0; i <= CantidadSprints-1; i++)
                {
                    valorSiglaFinal++;
                    var existe = await _sprintRepository.AnyWithCondition(x => x.Sigla == sprint.Sigla + "-" + valorSiglaFinal.ToString() && x.Periodo == sprint.Periodo);
                    if (existe)
                    {
                        SetAlert(GetConstants.ALERT_ERROR);
                        SetMessage($"Ya existe un registro con la descripción {sprint.Sigla.ToLower()}");
                        return View(sprint);
                    }
                    sprint.Sigla = sig + "-" + valorSiglaFinal.ToString();
                    _sprintRepository.Insert(sprint);
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
