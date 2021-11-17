using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FirmaPersonal.Models;

namespace FirmaPersonal.Controllers
{
    public class WorkersController : Controller
    {
        private FirmaPersonal2020Entities db = new FirmaPersonal2020Entities();

        // GET: Workers
        [Authorize(Roles = "moderator")]
        public ActionResult Index()
        {
            var workers = db.Workers.Include(w => w.Job);
            return View(workers.ToList());
        }

        // GET: Workers/Details/5
        [Authorize(Roles = "moderator")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Worker worker = db.Workers.Find(id);
            worker = db.Workers.Include(p => p.Job).FirstOrDefault(t => t.Id == id);
            if (worker == null)
            {
                return HttpNotFound();
            }
            return View(worker);
        }

        // GET: Workers/Create
        [Authorize(Roles = "admin")]
        public ActionResult Create()
        {
            ViewBag.JobId = new SelectList(db.Jobs, "Id", "JobName");
            return View();
        }

        // POST: Workers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public ActionResult Create([Bind(Include = "Id,LastName,Phone,Photo,PhotoType,JobId,Salary")] Worker worker, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    worker.PhotoType = image.ContentType;
                    worker.Photo = new byte[image.ContentLength];
                    image.InputStream.Read(worker.Photo, 0, image.ContentLength);
                }
                db.Workers.Add(worker);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.JobId = new SelectList(db.Jobs, "Id", "JobName", worker.JobId);
            return View(worker);
        }

        // GET: Workers/Edit/5
        [Authorize(Roles = "moderator")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Worker worker = db.Workers.Find(id);
            if (worker == null)
            {
                return HttpNotFound();
            }
            ViewBag.JobId = new SelectList(db.Jobs, "Id", "JobName", worker.JobId);
            return View(worker);
        }

        // POST: Workers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "moderator")]
        public ActionResult Edit([Bind(Include = "Id,LastName,Phone,Photo,PhotoType,JobId,Salary")] Worker worker, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    worker.PhotoType = image.ContentType;
                    worker.Photo = new byte[image.ContentLength];
                    image.InputStream.Read(worker.Photo, 0, image.ContentLength);
                }
                db.Entry(worker).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.JobId = new SelectList(db.Jobs, "Id", "JobName", worker.JobId);
            return View(worker);
        }

        // GET: Workers/Delete/5
        [Authorize(Roles = "admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Worker worker = db.Workers.Find(id);
            if (worker == null)
            {
                return HttpNotFound();
            }
            return View(worker);
        }

        // POST: Workers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Worker worker = db.Workers.Find(id);
            db.Workers.Remove(worker);
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

        public FileContentResult GetImage(int id)
        {
            Worker worker = db.Workers.FirstOrDefault(g => g.Id == id);
            if (worker != null)
            {
                return File(worker.Photo, worker.PhotoType);
            }

            return null;
        }
    }
}
