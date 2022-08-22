using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using sheduler.Models;


namespace sheduler.Controllers
{
    ////[Authorize]
    [HandleError]
    public class Campus_BranchesController : Controller
    {
        private MyDosa_dbEntities1 db = new MyDosa_dbEntities1();

        // GET: Campus_Branches
        public ActionResult Index()
        {
            return View(db.Campus_Branches.ToList());
        }

        // GET: Campus_Branches/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Campus_Branches campus_Branches = db.Campus_Branches.Find(id);
            if (campus_Branches == null)
            {
                return HttpNotFound();
            }
            return View(campus_Branches);
        }

        // GET: Campus_Branches/Create
        public ActionResult Create()
        {
            return View();
        }

 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "campus_id,Campus_branch")] Campus_Branches campus_Branches)
        {
            if (ModelState.IsValid)
            {
                db.Campus_Branches.Add(campus_Branches);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(campus_Branches);
        }

        // GET: Campus_Branches/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Campus_Branches campus_Branches = db.Campus_Branches.Find(id);
            if (campus_Branches == null)
            {
                return HttpNotFound();
            }
            return View(campus_Branches);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "campus_id,Campus_branch")] Campus_Branches campus_Branches)
        {
            if (ModelState.IsValid)
            {
                db.Entry(campus_Branches).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(campus_Branches);
        }

        // GET: Campus_Branches/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Campus_Branches campus_Branches = db.Campus_Branches.Find(id);
            if (campus_Branches == null)
            {
                return HttpNotFound();
            }
            return View(campus_Branches);
        }

        // POST: Campus_Branches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Campus_Branches campus_Branches = db.Campus_Branches.Find(id);
            db.Campus_Branches.Remove(campus_Branches);
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
