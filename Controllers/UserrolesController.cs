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
    //[Authorize]
    [HandleError]
    public class UserrolesController : Controller
    {
        private MyDosa_dbEntities1 db = new MyDosa_dbEntities1();

        // GET: Userroles
        public ActionResult Index()
        {
            var userroles = db.Userroles.Include(u => u.Role).Include(u => u.Student);
            return View(userroles.ToList());
        }

        // GET: Userroles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Userrole userrole = db.Userroles.Find(id);
            if (userrole == null)
            {
                return HttpNotFound();
            }
            return View(userrole);
        }

        // GET: Userroles/Create
        public ActionResult Create()
        {
            ViewBag.Roleid = new SelectList(db.Roles, "Role_id", "Role1");
            ViewBag.AccessNo = new SelectList(db.Students, "AccessNumber", "UserName");
            return View();
        }

        // POST: Userroles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Roleid,AccessNo,userroleid")] Userrole userrole)
        {
            if (ModelState.IsValid)
            {
                db.Userroles.Add(userrole);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Roleid = new SelectList(db.Roles, "Role_id", "Role1", userrole.Roleid);
            ViewBag.AccessNo = new SelectList(db.Students, "AccessNumber", "UserName", userrole.AccessNo);
            return View(userrole);
        }

        // GET: Userroles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Userrole userrole = db.Userroles.Find(id);
            if (userrole == null)
            {
                return HttpNotFound();
            }
            ViewBag.Roleid = new SelectList(db.Roles, "Role_id", "Role1", userrole.Roleid);
            ViewBag.AccessNo = new SelectList(db.Students, "AccessNumber", "UserName", userrole.AccessNo);
            return View(userrole);
        }

        // POST: Userroles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Roleid,AccessNo,userroleid")] Userrole userrole)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userrole).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Roleid = new SelectList(db.Roles, "Role_id", "Role1", userrole.Roleid);
            ViewBag.AccessNo = new SelectList(db.Students, "AccessNumber", "UserName", userrole.AccessNo);
            return View(userrole);
        }

        // GET: Userroles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Userrole userrole = db.Userroles.Find(id);
            if (userrole == null)
            {
                return HttpNotFound();
            }
            return View(userrole);
        }

        // POST: Userroles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Userrole userrole = db.Userroles.Find(id);
            db.Userroles.Remove(userrole);
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
