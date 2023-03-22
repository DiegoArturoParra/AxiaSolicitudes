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
    public class CelulaController : Controller
    {
        private AxiaContext db = new AxiaContext();

        // GET: Celula
        public ActionResult Index()
        {
            return View(db.TablaCelula.ToList());
        }

        // GET: Celula/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Celula celula = db.TablaCelula.Find(id);
            if (celula == null)
            {
                return HttpNotFound();
            }
            return View(celula);
        }

        // GET: Celula/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Celula/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Descripcion")] Celula celula)
        {
            if (ModelState.IsValid)
            {
                Celula data = db.TablaCelula.Where(x => x.Descripcion == celula.Descripcion).FirstOrDefault();
                if (data == null)
                {
                    db.TablaCelula.Add(celula);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return RedirectToAction("Index");
            }

            return View(celula);
        }

        // GET: Celula/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Celula celula = db.TablaCelula.Find(id);
            if (celula == null)
            {
                return HttpNotFound();
            }
            return View(celula);
        }

        // POST: Celula/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Descripcion")] Celula celula)
        {
            if (ModelState.IsValid)
            {
                db.Entry(celula).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(celula);
        }

        // GET: Celula/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Celula celula = db.TablaCelula.Find(id);
            if (celula == null)
            {
                return HttpNotFound();
            }
            return View(celula);
        }

        // POST: Celula/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Celula celula = db.TablaCelula.Find(id);
            db.TablaCelula.Remove(celula);
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
