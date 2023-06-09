﻿using AttentionAxia.DTOs;
using AttentionAxia.DTOs.Filters;
using AttentionAxia.Helpers;
using AttentionAxia.Models;
using AttentionAxia.Repositories;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
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
        [ValidateInput(false)]
        [HttpGet]
        public async Task<ActionResult> Index(SolicitudFilterDTO filtro)
        {
            LoadLists(filtro.Linea, filtro.Responsable);
            var solicitudes = await _solicitudRepository.GetSolicitudes(filtro);
            TempData[GetConstants.FILTRO_RANGO_FECHA] = filtro.FiltroFecha;
            return View(solicitudes);
        }
        [HttpGet]
        public async Task<ActionResult> GetDateBySprintInitial(int SprintInicioId)
        {
            var fecha = await _sprintRepository.GetDateBySprint(true, SprintInicioId);
            return Json(new { hayFecha = fecha.HasValue, fecha = $"{fecha:dd/MM/yyyy}" }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> GetDateBySprintFinal(int SprintFinId)
        {
            var fecha = await _sprintRepository.GetDateBySprint(false, SprintFinId);
            return Json(new { hayFecha = fecha.HasValue, fecha = $"{fecha:dd/MM/yyyy}" }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public async Task<ActionResult> ConsultaResponsables(int lineaId = 0)
        {
            var listaResponsables = await _responsableRepository.GetPersonsByLineId(lineaId);
            return Json(listaResponsables, JsonRequestBehavior.AllowGet);
        }
        // GET: Solicitud/Create
        [Authorize(Roles = "Administrador-Axia")]
        public async Task<ActionResult> Create()
        {
            await LoadListsForCreateAsync(null);
            return View();
        }

        // POST: Solicitud/Create
        [HttpPost]
        [Authorize(Roles = "Administrador-Axia")]
        public async Task<ActionResult> CreateSolicitud()
        {
            var response = new ResponseDTO
            {
                Message = "Ha ocurrido un error, comuniquese con el administrador."
            };
            var req = Request;
            HttpPostedFileBase FileDocument = req.Files["FileDocument"];
            var datos = req["json"];
            if (!string.IsNullOrWhiteSpace(datos))
            {
                var solicitud = JsonConvert.DeserializeObject<CreateSolicitudDTO>(datos);
                response = await _solicitudRepository.InsertWithArchive(solicitud, FileDocument, PathActual);
                if (!response.IsSuccess)
                {
                    SetAlert(GetConstants.ALERT_ERROR);
                    SetMessage(response.Message);
                    return Json(new { response.IsSuccess, response.Message });
                }
                SetAlert(GetConstants.ALERT_SUCCESS);
                SetMessage("Creado satisfactoriamente.");
            }
            return Json(new { response.IsSuccess, response.Message });
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
            await LoadListsForCreateAsync(solicitud);
            return View(solicitud);
        }

        /// <summary>
        /// Editar solicitud
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Administrador-Axia")]
        public async Task<ActionResult> EditSolicitud()
        {
            var response = new ResponseDTO
            {
                Message = "Ha ocurrido un error, comuniquese con el administrador."
            };
            var req = Request;
            HttpPostedFileBase FileDocument = req.Files["filedocument"];
            var datos = req["json"];
            if (!string.IsNullOrWhiteSpace(datos))
            {
                var solicitud = JsonConvert.DeserializeObject<EditSolicitudDTO>(datos);
                response = await _solicitudRepository.UpdateWithArchive(solicitud, FileDocument, PathActual);
                if (!response.IsSuccess)
                {
                    SetAlert(GetConstants.ALERT_ERROR);
                    SetMessage(response.Message);
                    return Json(new { response.IsSuccess, response.Message });
                }
                SetAlert(GetConstants.ALERT_SUCCESS);
                SetMessage("Editado satisfactoriamente.");
            }
            return Json(new { response.IsSuccess, response.Message });
        }

        [HttpPost]
        [Authorize(Roles = "Administrador-Axia")]
        public async Task<ActionResult> EditSolicitudByState(EditSolicitudByStateDTO solicitud)
        {
            var response = new ResponseDTO
            {
                Message = "Ha ocurrido un error, comuniquese con el administrador."
            };
            if (solicitud != null)
            {
                response = await _solicitudRepository.UpdateByState(solicitud);
            }

            return Json(new { response.IsSuccess, response.Message });
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
            SetAlert(GetConstants.ALERT_SUCCESS);
            SetMessage("Eliminado satisfactoriamente.");
            return RedirectToAction("Index");
        }
        private void LoadLists(int? linea, int? responsable)
        {
            ViewBag.DDL_Responsables = new SelectList("");
            if (linea.HasValue && responsable.HasValue)
            {
                ViewBag.DDL_Responsables = new SelectList(_responsableRepository.Table.Where(x => x.LineaPerteneceId == linea), "Id", "Nombres", responsable.Value);
            }
            ViewBag.DDL_Estados = new SelectList(_estadoRepository.Table, "Id", "Descripcion");
            ViewBag.DDL_Lineas = new SelectList(_lineaRepository.Table, "Id", "Descripcion");
            ViewBag.DDL_Celulas = new SelectList(_celulaRepository.Table, "Id", "Descripcion");
            ViewBag.DDL_Sprints = new SelectList(_sprintRepository.Table, "Id", "DescripcionSprint");
            List<int> items = new List<int>()
            {
                25, 50, 100, 250, 500
            };

            ViewBag.DDL_Items = new SelectList(items);
        }
        private async Task LoadListsForCreateAsync(Solicitud solicitud)
        {
            if (solicitud == null)
            {
                var estados = _estadoRepository.Table;
                var primerEstado = await estados.Where(x => x.Descripcion.ToUpper().Contains("HACER")).FirstOrDefaultAsync();
                if (primerEstado == null)
                {
                    ViewBag.EstadoId = new SelectList(await estados.OrderBy(x => x.Descripcion).ToListAsync(), "Id", "Descripcion");
                }
                else
                {
                    ViewBag.EstadoId = new SelectList(await estados.OrderBy(x => x.Descripcion).ToListAsync(), "Id", "Descripcion", primerEstado.Id);
                }
                ViewBag.ResponsableId = new SelectList(await _responsableRepository.Table.OrderBy(y => y.Nombres).ToListAsync(), "Id", "Nombres");
                ViewBag.DDL_Sprints = new SelectList(await _sprintRepository.Table.Where(y => y.IsActivo == true).ToListAsync(), "Id", "DescripcionSprint");
                ViewBag.DDL_Celulas = new SelectList(await _celulaRepository.Table.OrderBy(x => x.Descripcion).ToListAsync(), "Id", "Descripcion");
            }
            else
            {
                ViewBag.EstadoId = new SelectList(await _estadoRepository.Table.OrderBy(x => x.Descripcion).ToListAsync(), "Id", "Descripcion", solicitud.EstadoId);
                ViewBag.ResponsableId = new SelectList(await _responsableRepository.Table.OrderBy(y => y.Nombres).ToListAsync(), "Id", "Nombres", solicitud.ResponsableId);

                var sprints = await _sprintRepository.Table.ToListAsync();
                ViewBag.DDL_SprintsInicio = new SelectList(sprints, "Id", "DescripcionSprint", solicitud.SprintInicioId);
                ViewBag.DDL_SprintsFin = new SelectList(sprints, "Id", "DescripcionSprint", solicitud.SprintFinId);
                ViewBag.DDL_Celulas = new SelectList(await _celulaRepository.Table.OrderBy(x => x.Descripcion).ToListAsync(), "Id", "Descripcion", solicitud.CelulaId);
            }

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
