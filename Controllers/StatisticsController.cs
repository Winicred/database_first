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
    public class StatisticsController : Controller
    {
        private FirmaPersonal2020Entities db = new FirmaPersonal2020Entities();

        // GET: Statistics
        public ActionResult Index()
        {
            //var job = db.Jobs.Include(m => m.Workers).ToList();
            var workers = db.Workers.ToList();
            ViewBag.Workers = workers; 
            return View(db.Jobs.ToList());
        }

        // GET: Statistics/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Job job = db.Jobs.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);
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
