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
    public class ResponsesController : Controller
    {
        private MyDosa_dbEntities1 db = new MyDosa_dbEntities1();

        // GET: Responses
        public ActionResult Index()
        {
            var responses = db.Responses.Include(r => r.Inquiry);
            return View(responses.ToList());
        }

        // GET: Responses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Response response = db.Responses.Find(id);
            if (response == null)
            {
                return HttpNotFound();
            }
            return View(response);
        }

        // GET: Responses/Create
        public ActionResult Create()
        {
            ViewBag.Inquirery_id = new SelectList(db.Inquiries, "Inquirery_id", "inquirry");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ResponseId,Inquirery_id,Response1,DatetimeOfReply")] Response response)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var exixtsresponse = responseExixts(response.ResponseId);
                    if (exixtsresponse)
                    {
                        TempData["responseexists"] = "PLEASE MAKE CHAGES TO THE PREVIOUS RESPONSE";
                    }
                    else
                    {
                        response.DatetimeOfReply = DateTime.Now;
                        db.Responses.Add(response);
                        db.SaveChanges();
                    }

                    return RedirectToAction("Index");
                }
            }catch(Exception e)
            {
                TempData["errorsaving"]  = e.Message;
            }

            ViewBag.Inquirery_id = new SelectList(db.Inquiries, "Inquirery_id", "inquirry", response.Inquirery_id);
            return View(response);
        }
        [NonAction]
        public bool responseExixts(int id)
        {
             var v = db.Responses.Where(a => a.ResponseId == id).FirstOrDefault();
                return v != null;
            
        }
        // GET: Responses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Response response = db.Responses.Find(id);
            if (response == null)
            {
                return HttpNotFound();
            }
            ViewBag.Inquirery_id = new SelectList(db.Inquiries, "Inquirery_id", "inquirry", response.Inquirery_id);
            return View(response);
        }

        // POST: Responses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ResponseId,Inquirery_id,Response1,DatetimeOfReply")] Response response)
        {
            if (ModelState.IsValid)
            {
                db.Entry(response).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Inquirery_id = new SelectList(db.Inquiries, "Inquirery_id", "inquirry", response.Inquirery_id);
            return View(response);
        }

        // GET: Responses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Response response = db.Responses.Find(id);
            if (response == null)
            {
                return HttpNotFound();
            }
            return View(response);
        }

        // POST: Responses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Response response = db.Responses.Find(id);
            db.Responses.Remove(response);
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
