using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AttentionAxia.Core.Data;
using AttentionAxia.Helpers;
using AttentionAxia.Models;
using AttentionAxia.Repositories;

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
            var tablaResponsables = _responsableRepository.Context.TablaResponsables.Include(r => r.CelulaPertenece).Include(r => r.LineaPertenece);
            return View(tablaResponsables.ToList());
        }

        // GET: Responsables/Create
        public ActionResult Create()
        {
            ViewBag.CelulaPerteneceId = new SelectList(_celulaRepository.Table, "Id", "Descripcion");
            ViewBag.LineaPerteneceId = new SelectList(_lineaRepository.Table, "Id", "Descripcion");
            return View();
        }

        // POST: Responsables/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Nombres,CelulaPerteneceId,LineaPerteneceId")] Responsable responsable)
        {
            if (ModelState.IsValid)
            {
                var existe = await _responsableRepository.AnyWithCondition(r => r.Nombres.ToLower() == responsable.Nombres);
                if (existe)
                {
                    SetAlert(GetConstants.ALERT_ERROR);
                    SetMessage($"Ya existe un registro con la descripción {responsable.Nombres.ToLower()}");
                    return View(responsable);
                }

                _responsableRepository.Insert(responsable);
                await _responsableRepository.Save();
                SetAlert(GetConstants.ALERT_SUCCESS);
                SetMessage("Creado satisfactoriamente.");
                return RedirectToAction("Index");
            }

            ViewBag.CelulaPerteneceId = new SelectList(_celulaRepository.Table, "Id", "Descripcion", responsable.CelulaPerteneceId);
            ViewBag.LineaPerteneceId = new SelectList(_lineaRepository.Table, "Id", "Descripcion", responsable.LineaPerteneceId);
            return View(responsable);
        }

        // GET: Responsables/Edit/5
        public ActionResult Edit(int? id)
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
            ViewBag.CelulaPerteneceId = new SelectList(_celulaRepository.Table, "Id", "Descripcion", responsable.CelulaPerteneceId);
            ViewBag.LineaPerteneceId = new SelectList(_lineaRepository.Table, "Id", "Descripcion", responsable.LineaPerteneceId);
            return View(responsable);
        }

        // POST: Responsables/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
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
            ViewBag.CelulaPerteneceId = new SelectList(_celulaRepository.Table, "Id", "Descripcion", responsable.CelulaPerteneceId);
            ViewBag.LineaPerteneceId = new SelectList(_lineaRepository.Table, "Id", "Descripcion", responsable.LineaPerteneceId);
            return View(responsable);            
        }

        // GET: Responsables/Delete/5
        public ActionResult Delete(int? id)
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
            return View(responsable);          
        }

        // POST: Responsables/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Responsable responsable = _responsableRepository.FindById(id);
            _responsableRepository.Delete(responsable);
            await _responsableRepository.Save();
            SetAlert(GetConstants.ALERT_SUCCESS);
            SetMessage("Eliminado satisfactoriamente.");
            return RedirectToAction("Index");           
        }  
    }
}
