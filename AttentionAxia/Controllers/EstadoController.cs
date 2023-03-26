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
    [Authorize(Roles = "Administrador-Axia")]
    public class EstadoController : BaseController
    {
        private readonly EstadoRepository _estadoRepository;
        public EstadoController()
        {
            _estadoRepository = new EstadoRepository(_db);
        }

        // GET: Estados
        public async Task<ActionResult> Index()
        {
            return View(await _estadoRepository.GetAll());
        }


        // GET: Estados/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Estados/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Descripcion,Nivel")] Estado estado)
        {
            if (ModelState.IsValid)
            {
                var existe = await _estadoRepository.AnyWithCondition(x => x.Descripcion.ToLower() == estado.Descripcion);
                if (existe)
                {
                    SetAlert(GetConstants.ALERT_ERROR);
                    SetMessage($"Ya existe un registro con la descripción {estado.Descripcion.ToLower()}");
                    return View(estado);
                }
                _estadoRepository.Insert(estado);
                await _estadoRepository.Save();
                SetAlert(GetConstants.ALERT_SUCCESS);
                SetMessage("Creado satisfactoriamente.");
                return RedirectToAction("Index");
            }

            return View(estado);
        }

        // GET: Estados/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            Estado estado = _estadoRepository.FindById(id);
            if (estado == null)
            {
                SetAlert(GetConstants.ALERT_ERROR);
                SetMessage("No existe el registro.");
                return RedirectToAction("Index");
            }
            return View(estado);
        }

        // POST: Estados/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Descripcion,Nivel")] Estado estado)
        {
            if (ModelState.IsValid)
            {
                if (await _estadoRepository.AnyWithCondition(x => x.Descripcion.ToLower() == estado.Descripcion && x.Id != estado.Id))
                {
                    SetAlert(GetConstants.ALERT_ERROR);
                    SetMessage($"Ya existe un registro con la descripción {estado.Descripcion.ToUpper()}");
                    return View(estado);
                }
                _estadoRepository.Update(estado);
                await _estadoRepository.Save();
                SetAlert(GetConstants.ALERT_SUCCESS);
                SetMessage("Actualizado satisfactoriamente.");
                return RedirectToAction("Index");
            }
            return View(estado);
        }

        // GET: Estados/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            Estado estado = _estadoRepository.FindById(id);
            if (estado == null)
            {
                SetAlert(GetConstants.ALERT_ERROR);
                SetMessage("No existe el registro.");
                return RedirectToAction("Index");
            }
            return View(estado);
        }

        // POST: Estados/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Estado estado = _estadoRepository.FindById(id);
            _estadoRepository.Delete(estado);
            await _estadoRepository.Save();
            SetAlert(GetConstants.ALERT_SUCCESS);
            SetMessage("Eliminado satisfactoriamente.");
            return RedirectToAction("Index");
        }
    }
}
