using AttentionAxia.DTOs;
using AttentionAxia.Helpers;
using AttentionAxia.Models;
using AttentionAxia.Repositories;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AttentionAxia.Controllers
{
    public class SolicitudController : BaseController
    {
        private readonly SolicitudRepository _solicitudRepository;
        private readonly EstadoRepository _estadoRepository;
        private readonly ResponsableRepository _responsableRepository;
        private readonly SprintRepository _sprintRepository;
        private readonly LineaRepository _lineaRepository;
        private readonly CelulaRepository _celulaRepository;

        public SolicitudController()
        {
            _solicitudRepository = new SolicitudRepository(_db);
            _estadoRepository = new EstadoRepository(_db);
            _responsableRepository = new ResponsableRepository(_db);
            _sprintRepository = new SprintRepository(_db);
            _lineaRepository = new LineaRepository(_db);
            _celulaRepository = new CelulaRepository(_db);
        }

        // GET: Solicitud
        public async Task<ActionResult> Index(SolicitudFilterDTO filtro)
        {
            LoadLists();
            var solicitudes = await _solicitudRepository.GetSolicitudes(filtro);
            return View(solicitudes);
        }



        private void LoadLists()
        {
            ViewBag.DDL_Estados = new SelectList(_estadoRepository.Table, "Id", "Descripcion");
            ViewBag.DDL_Lineas = new SelectList(_lineaRepository.Table, "Id", "Descripcion");
            ViewBag.DDL_Celulas = new SelectList(_celulaRepository.Table, "Id", "Descripcion");
            ViewBag.DDL_Sprints = new SelectList(_sprintRepository.Table, "Id", "DescripcionSprint");
            List<int> items = new List<int>()
            {
                100,250,500
            };

            ViewBag.DDL_Items = new SelectList(items);
        }

        // GET: Solicitud/Create
        [Authorize(Roles = "Administrador-Axia")]
        public async Task<ActionResult> Create()
        {
            await LoadListsCreateAsync(null);
            return View();
        }

        // POST: Solicitud/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador-Axia")]
        public async Task<ActionResult> Create(CreateSolicitudDTO solicitud)
        {
            solicitud.FechaFinal = solicitud.FechaFinal.AddHours(24).AddSeconds(-1);
            var response = await _solicitudRepository.ValidationsOfBusiness(solicitud);
            if (!response.IsSuccess)
            {
                SetAlert(GetConstants.ALERT_ERROR);
                SetMessage(response.Message);
                return Json(new { response.IsSuccess, response.Message });
            }
            var entidadInsert = new Solicitud()
            {
                ResponsableId = solicitud.ResponsableId,
                EstadoId = solicitud.EstadoId,
                SprintId = solicitud.SprintId,
                FechaInicioSprint = solicitud.FechaInicial,
                FechaFinSprint = solicitud.FechaFinal,
                Iniciativa = solicitud.Iniciativa,
                Avance = 0
            };
            _solicitudRepository.Insert(entidadInsert);
            await _solicitudRepository.Save();
            SetAlert(GetConstants.ALERT_SUCCESS);
            SetMessage("Creado satisfactoriamente.");
            return Json(new { response.IsSuccess, response.Message });
        }

        private async Task LoadListsCreateAsync(Solicitud solicitud)
        {
            if (solicitud == null)
            {
                var estados = _estadoRepository.Table;
                var primerEstado = await estados.Where(x => x.Descripcion.ToUpper().Contains("HACER")).FirstOrDefaultAsync();
                if (primerEstado == null)
                {
                    ViewBag.EstadoId = new SelectList(estados, "Id", "Descripcion");
                }
                else
                {
                    ViewBag.EstadoId = new SelectList(estados, "Id", "Descripcion", primerEstado.Id);
                }
                ViewBag.ResponsableId = new SelectList(_responsableRepository.Table, "Id", "Nombres");
                ViewBag.SprintId = new SelectList(_sprintRepository.Table, "Id", "DescripcionSprint");
            }
            else
            {
                ViewBag.EstadoId = new SelectList(_estadoRepository.Table, "Id", "Descripcion", solicitud.EstadoId);
                ViewBag.ResponsableId = new SelectList(_responsableRepository.Table, "Id", "Nombres", solicitud.ResponsableId);
                ViewBag.SprintId = new SelectList(_sprintRepository.Table, "Id", "DescripcionSprint", solicitud.SprintId);
            }

        }

        // GET: Solicitud/Edit/5
        [Authorize(Roles = "Administrador-Axia")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                SetAlert(GetConstants.ALERT_ERROR);
                SetMessage("No existe el registro.");
                return RedirectToAction("Index");
            }
            Solicitud solicitud = _solicitudRepository.FindById(id);
            if (solicitud == null)
            {
                SetAlert(GetConstants.ALERT_ERROR);
                SetMessage("No existe el registro.");
                return RedirectToAction("Index");
            }
            await LoadListsCreateAsync(solicitud);
            return View(solicitud);
        }

        // POST: Solicitud/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador-Axia")]
        public async Task<ActionResult> Edit([Bind(Include = "Id,ResponsableId,EstadoId,SprintId,Iniciativa,FechaInicioSprint,FechaFinSprint,Avance")] Solicitud solicitud)
        {
            if (ModelState.IsValid)
            {
                if (await _solicitudRepository.AnyWithCondition(x => x.Id == solicitud.Id))
                {
                    SetAlert(GetConstants.ALERT_ERROR);
                    SetMessage($"Ya existe un registro con la descripción {solicitud.Iniciativa.ToUpper()}");
                    return View(solicitud);
                }
                _solicitudRepository.Update(solicitud);
                await _solicitudRepository.Save();
                SetAlert(GetConstants.ALERT_SUCCESS);
                SetMessage("Actualizado satisfactoriamente.");
                return RedirectToAction("Index");
            }
            await LoadListsCreateAsync(solicitud);
            return View(solicitud);
        }

        // GET: Solicitud/Delete/5
        [Authorize(Roles = "Administrador-Axia")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                SetAlert(GetConstants.ALERT_ERROR);
                SetMessage("No existe el registro.");
                return RedirectToAction("Index");
            }
            Solicitud solicitud = _solicitudRepository.FindById(id);
            if (solicitud == null)
            {
                SetAlert(GetConstants.ALERT_ERROR);
                SetMessage("No existe el registro.");
                return RedirectToAction("Index");
            }
            return View(solicitud);
        }

        // POST: Solicitud/Delete/5
        [Authorize(Roles = "Administrador-Axia")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Solicitud solicitud = _solicitudRepository.FindById(id);
            _solicitudRepository.Delete(solicitud);
            await _solicitudRepository.Save();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _solicitudRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
