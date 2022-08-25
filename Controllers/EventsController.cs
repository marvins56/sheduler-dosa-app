using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;

using System.Web.Mvc;
//using FullCalendar;
using Event = sheduler.Models.Event;
using sheduler.Models;
using System.Collections.Generic;
using sheduler.ViewModels;

namespace sheduler.Controllers
{
    public class EventsController : Controller
    {
        private MyDosa_dbEntities1 db = new MyDosa_dbEntities1();

        //public ActionResult UserReports()
        //{
        //    var personalinfo = Get_myinquiries();
        //    var userDetails = new userReports();
        //    userDetails.Inquiry = personalinfo;
        //    return View();
        //}
        public List<Inquiry> Get_myinquiries()
        {
            var id = (Session["userid"]).ToString();
            try
            {
                TempData["inquiries"] = db.Inquiries.Where(a => a.UserId == id).ToList().Count();
                db.Inquiries.Where(a => a.UserId == id).ToList();

            }
            catch (Exception e)
            {
                TempData["error"] = e.Message;
            }

            return (db.Inquiries.Where(a => a.UserId == id).ToList());
        }

        public List<Student> Get_students()
        {
            return (db.Students.ToList());
        }
        public ActionResult Generalreport()
        {
            var users = Get_students();
            var inquiry = Get_all_Inquiries();
            var response = Get_all_responses();
            var calendar_events = Get_all_events();
            var genaralreports = new General_reports();
            genaralreports.students=users;
            genaralreports.Inquiry = inquiry;
            genaralreports.Response = response;
            genaralreports.events = calendar_events;

            return View(genaralreports);
        }
        public List<Inquiry> Get_all_Inquiries()
        {
            return (db.Inquiries.ToList());
        }
        public List<Response> Get_all_responses()
        {
            return (db.Responses.ToList());
        }

        public List<Event> Get_all_events()
        {
            return (db.Events.ToList());
        }

        public ActionResult Index()
        {

            return View();
        }
        public ActionResult Generate_All_Events_PDF()
        {
            return new Rotativa.ActionAsPdf("viewallevents");
        }
        public ActionResult Generate_personal_Events_PDF()
        {
            return new Rotativa.ActionAsPdf("Myshedules");
        }
        public ActionResult viewallevents()
        {
            try
            {
                    var result = db.Events.ToList();
                    return View(result);
            }catch(Exception E)
            {
                TempData["error"] = E.Message;
            }
            return View();
        }
        public ActionResult Myshedules()
        {
            try
            {
                string id = Session["userid"].ToString();
                if(id!= null)
                {
                    var shedule = db.Events.Where(a => a.userid == id).ToList();
                    return View(shedule);
                }
              
            }
            catch (Exception e)
            {
                TempData["error"] = e.Message;
            }
            return View();
        }
        public ActionResult Mycalendar()
        {
            
            return View();
        }
        public JsonResult GetEvents()
        {
            using (MyDosa_dbEntities1 dc = new MyDosa_dbEntities1())
            {
                try
                {
                    var events = dc.Events.ToList();
                    return new JsonResult { Data = events, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }catch(Exception e)
                {
                    TempData["error"] = e.Message;
}
                return new JsonResult { Data = null, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            }
        }
        public JsonResult GetmyEvents()
        {
            using (MyDosa_dbEntities1 dc = new MyDosa_dbEntities1())
            {
                try
                {
                    string id = Session["userid"].ToString();
                    if(id == null) {

                        TempData["error"] = "ERROR WHILE VALIDATING USER";
                    } else
                    {
                        var shedule = db.Events.Where(a => a.userid == id).ToList();
                        return new JsonResult { Data = shedule, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                    }
                   
                }catch(Exception E)
                {
                    TempData["error"] = E.Message;
                   
                }
                return new JsonResult { Data = null, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }
        [HttpPost]
        public JsonResult SaveEvent(Event e)
        {
            var status = false;
            
                using (MyDosa_dbEntities1 dc = new MyDosa_dbEntities1())
                {
                var id = dc.Events.Where(a => a.EventID == e.EventID).Select(a=>a.EventID).FirstOrDefault();
                if (id > 0)
                {
                    //Update the event
                    var v = dc.Events.Where(a => a.EventID == e.EventID).FirstOrDefault();
                    if (v != null)
                    {
                        v.Subject = e.Subject;
                        v.Start = e.Start;
                        v.End = e.End;
                        v.Description = e.Description;
                        v.IsFullDay = e.IsFullDay;
                        v.ThemeColor = e.ThemeColor;
                    }
                }
                else
                {
                    dc.Events.Add(e);
                }

                dc.SaveChanges();
                    status = true;
                }
            
            return new JsonResult { Data = new { status = status } };
        }
        [HttpPost]
        public JsonResult DeleteEvent(int eventID)
        {
            var status = false;
            try
            {
                using (MyDosa_dbEntities1 dc = new MyDosa_dbEntities1())
                {
                    var v = dc.Events.Where(a => a.EventID == eventID).FirstOrDefault();
                    if (v != null)
                    {
                        dc.Events.Remove(v);
                        dc.SaveChanges();
                        status = true;
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
            }
            return new JsonResult { Data = new { status = status } };
        }

        // GET: Events/Create
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EventID,Subject,Description,Start,End,ThemeColor,IsFullDay,userid")] Event @event)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var start = @event.Start;
                    var end = @event.End;
                    if(start > end)
                    {
                        TempData["error"] = "INVALID  SET START DATE";
                    }
                    else
                    {
                        @event.userid = Session["userid"].ToString();
                        db.Events.Add(@event);
                        db.SaveChanges();
                        TempData["success"] = "OPERATION SUCCESSFULL";
                        return RedirectToAction("MyCalendar");
                    }
                
                }

            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
            }
            return RedirectToAction("MyCalendar", @event);
        }
        public ActionResult AdminCreate()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdminCreate([Bind(Include = "EventID,Subject,Description,Start,End,ThemeColor,IsFullDay,userid")] Event @event)
        {
            try
            {
                var ROLE = Session["userroles"].ToString();
                if (ROLE == "ADMINISTRATOR" || ROLE == "SUPER ADMINISTRATOR")
                {
                    if (ModelState.IsValid)
                    {
                        db.Events.Add(@event);
                        db.SaveChanges();
                        TempData["success"] = "OPERATIO SUCCESSFULL";
                        return RedirectToAction("Index");
                    }

                    return View(@event);
                }
                else
                {
                    TempData["error"] = "ACCESS DENIED";
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
            }
            
                return View(@event);
        }

        // GET: Events/Edit/5
        public ActionResult Edit(int? id)
        {
            try
            {

                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Event @event = db.Events.Find(id);
               

                if (@event == null)
                {
                    return HttpNotFound();
                }
                return View(@event);
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
            }
            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EventID,Subject,Description,Start,End,ThemeColor,IsFullDay,userid")] Event @event)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(@event).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
            }
            return View(@event);
        }

        // GET: Events/Delete/5
        public ActionResult Delete(int? id)
        {
            try
            {

                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Event @event = db.Events.Find(id);
                if (@event == null)
                {
                    return HttpNotFound();
                }
                return View(@event);
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
            }
            return View();
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Event @event = db.Events.Find(id);
            db.Events.Remove(@event);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var shedule = db.Events.Find(id);
                if (shedule == null)
                {
                    return HttpNotFound();
                }
                return View(shedule);
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
            }
            return View();
        }
        public ActionResult AdminDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var shedule = db.Events.Find(id);
            if (shedule == null)
            {
                return HttpNotFound();
            }
            return View(shedule);
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
