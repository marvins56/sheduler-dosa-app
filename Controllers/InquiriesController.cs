using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using sheduler.Models;

namespace sheduler.Controllers
{
    public class InquiriesController : Controller
    {
        private MyDosa_dbEntities1 db = new MyDosa_dbEntities1();

        // GET: Inquiries
        
        public ActionResult Index()
        {
        //    var inquiryList = (from t in db.Inquiries
        //                       join r in db.Responses on t.Inquirery_id equals r.Inquirery_id
        //                       select new
        //                       {
        //                          Inquiry =  t.inquirry,
        //                           Inquirery_id =  t.Inquirery_id,
        //                           UserId = t.UserId,
        //                           ResponseId =    r.ResponseId,
        //                           Response1 = r.Response1,
        //                           DatetimeOfReply =   r.DatetimeOfReply,
        //                           Dateposteed = t.Dateposteed
        //                       }).ToList();

            return View(db.Inquiries.ToList());
        }
        public ActionResult userResponse(int id)
        {
            try
            {
                var responseid = db.Responses.Where(a => a.Inquirery_id == id).FirstOrDefault();
                var res = db.Responses.Where(a => a.Inquirery_id == id).Select(a => a.Response1).FirstOrDefault();
                var date = db.Responses.Where(a => a.Inquirery_id == id).Select(a => a.DatetimeOfReply).FirstOrDefault();
                if (responseid == null)
                {
                    TempData["nullvalue"] = "RESPONSE PENDING";
                    var ROLE = Session["userroles"];

                    if (ROLE.Equals(3)|| ROLE.Equals(6))
                    {
                        return RedirectToAction("index");
                    }
                    else
                    {
                        return RedirectToAction("myInquiries");
                    }

                }
                else
                {
                    TempData["values"] = "RESPONSE: " + res + " On " + date;
                    return RedirectToAction("index");
                }
            }catch(Exception E)
            {
                TempData["error"] = E.Message;
            }

            return View();

        }
        public ActionResult userInquiries(String id)
        {
            var myInquiries = db.Inquiries.Where(a => a.UserId == id).ToList();
            if (myInquiries == null)
            {
                TempData["nullvalue"] = id +" Has NO INQUIRIES";
                return RedirectToAction("index");
            }
          
            return View(myInquiries);
        }

      
        public ActionResult myInquiries()
        {
           string id = Session["userid"].ToString();
            var myInquiries = db.Inquiries.Where(a => a.UserId == id).ToList();
            if (myInquiries == null)
            {
                TempData["nullvalue"] = id + " Has NO INQUIRIES";
                return RedirectToAction("index");
            }

            return View(myInquiries);
        }
        // GET: Inquiries/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inquiry inquiry = db.Inquiries.Find(id);
            if (inquiry == null)
            {
                return HttpNotFound();
            }
            return View(inquiry);
        }

        // GET: Inquiries/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Inquiries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Inquirery_id,UserId,inquirry,Dateposteed")] Inquiry inquiry)
        {
            if (ModelState.IsValid)
            {
                inquiry.UserId = "a90648";
                inquiry.Dateposteed = DateTime.Now;
                db.Inquiries.Add(inquiry);
                db.SaveChanges();
                return RedirectToAction("muInquireies");
            }

            return View(inquiry);
        }

        // GET: Inquiries/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inquiry inquiry = db.Inquiries.Find(id);
            if (inquiry == null)
            {
                return HttpNotFound();
            }
            return View(inquiry);
        }

        // POST: Inquiries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Inquirery_id,UserId,inquirry,Dateposteed")] Inquiry inquiry)
        {
            if (ModelState.IsValid)
            {
                db.Entry(inquiry).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(inquiry);
        }

        // GET: Inquiries/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inquiry inquiry = db.Inquiries.Find(id);
            if (inquiry == null)
            {
                return HttpNotFound();
            }
            return View(inquiry);
        }

        // POST: Inquiries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Inquiry inquiry = db.Inquiries.Find(id);
            db.Inquiries.Remove(inquiry);
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
