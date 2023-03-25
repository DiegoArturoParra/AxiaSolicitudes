using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AttentionAxia.Core.Data;
using AttentionAxia.Models;

namespace AttentionAxia.Controllers
{
    public class ResponsablesController : Controller
    {
        private AxiaContext db = new AxiaContext();

        // GET: Responsables
        public ActionResult Index()
        {
            return View(db.TablaResponsables.ToList());
        }

        // GET: Responsables/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Responsables responsables = db.TablaResponsables.Find(id);
            if (responsables == null)
            {
                return HttpNotFound();
            }
            return View(responsables);
        }

        // GET: Responsables/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Responsables/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nombres,CelulaPertenceId,LineaPerteneceId")] Responsables responsables)
        {
            if (ModelState.IsValid)
            {
                db.TablaResponsables.Add(responsables);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(responsables);
        }

        // GET: Responsables/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Responsables responsables = db.TablaResponsables.Find(id);
            if (responsables == null)
            {
                return HttpNotFound();
            }
            return View(responsables);
        }

        // POST: Responsables/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nombres,CelulaPertenceId,LineaPerteneceId")] Responsables responsables)
        {
            if (ModelState.IsValid)
            {
                db.Entry(responsables).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(responsables);
        }

        // GET: Responsables/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Responsables responsables = db.TablaResponsables.Find(id);
            if (responsables == null)
            {
                return HttpNotFound();
            }
            return View(responsables);
        }

        // POST: Responsables/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Responsables responsables = db.TablaResponsables.Find(id);
            db.TablaResponsables.Remove(responsables);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
