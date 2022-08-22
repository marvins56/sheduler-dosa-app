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
    public class UserLocationsController : Controller
    {
        private MyDosa_dbEntities1 db = new MyDosa_dbEntities1();

        // GET: UserLocations
        public ActionResult Index()
        {
            var userLocations = db.UserLocations.Include(u => u.Student);
            return View(userLocations.ToList());
        }

        // GET: UserLocations/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserLocation userLocation = db.UserLocations.Find(id);
            if (userLocation == null)
            {
                return HttpNotFound();
            }
            return View(userLocation);
        }

        // GET: UserLocations/Create
        public ActionResult Create()
        {
            ViewBag.AccessNumber = new SelectList(db.Students, "AccessNumber", "UserName");
            return View();
        }

        // POST: UserLocations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AccessNumber,From_Address,To_Adddress,Distance,Estimated_Arrival_time,Set_Off_Time")] UserLocation userLocation)
        {
            if (ModelState.IsValid)
            {
                db.UserLocations.Add(userLocation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AccessNumber = new SelectList(db.Students, "AccessNumber", "UserName", userLocation.AccessNumber);
            return View(userLocation);
        }

        // GET: UserLocations/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserLocation userLocation = db.UserLocations.Find(id);
            if (userLocation == null)
            {
                return HttpNotFound();
            }
            ViewBag.AccessNumber = new SelectList(db.Students, "AccessNumber", "UserName", userLocation.AccessNumber);
            return View(userLocation);
        }

        // POST: UserLocations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AccessNumber,From_Address,To_Adddress,Distance,Estimated_Arrival_time,Set_Off_Time")] UserLocation userLocation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userLocation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AccessNumber = new SelectList(db.Students, "AccessNumber", "UserName", userLocation.AccessNumber);
            return View(userLocation);
        }

        // GET: UserLocations/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserLocation userLocation = db.UserLocations.Find(id);
            if (userLocation == null)
            {
                return HttpNotFound();
            }
            return View(userLocation);
        }

        // POST: UserLocations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            UserLocation userLocation = db.UserLocations.Find(id);
            db.UserLocations.Remove(userLocation);
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
