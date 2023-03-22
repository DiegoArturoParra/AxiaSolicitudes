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
    public class LineasController : Controller
    {
        private AxiaContext db = new AxiaContext();

        // GET: Lineas
        public ActionResult Index()
        {
            return View(db.TablaLinea.ToList());
        }

        // GET: Lineas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Linea linea = db.TablaLinea.Find(id);
            if (linea == null)
            {
                return HttpNotFound();
            }
            return View(linea);
        }

        // GET: Lineas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Lineas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Descripcion")] Linea linea)
        {
            if (ModelState.IsValid)
            {
                Linea data = db.TablaLinea.Where(x => x.Descripcion == linea.Descripcion).FirstOrDefault();
                if (data == null)
                {
                    db.TablaLinea.Add(linea);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return RedirectToAction("Index");
            }

            return View(linea);
        }

        // GET: Lineas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Linea linea = db.TablaLinea.Find(id);
            if (linea == null)
            {
                return HttpNotFound();
            }
            return View(linea);
        }

        // POST: Lineas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Descripcion")] Linea linea)
        {
            if (ModelState.IsValid)
            {
                db.Entry(linea).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(linea);
        }

        // GET: Lineas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Linea linea = db.TablaLinea.Find(id);
            if (linea == null)
            {
                return HttpNotFound();
            }
            return View(linea);
        }

        // POST: Lineas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Linea linea = db.TablaLinea.Find(id);
            db.TablaLinea.Remove(linea);
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
