using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;

using PagedList.Mvc;
using PagedList;
using System.Web.Mvc;
//using FullCalendar;
using Event = sheduler.Models.Event;
using sheduler.Models;
using System.Collections.Generic;
using sheduler.ViewModels;
using System.Net.Mail;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;

namespace sheduler.Controllers
{
    public class EventsController : Controller
    {
        private MyDosa_dbEntities1 db = new MyDosa_dbEntities1();

        public ActionResult UserReports(string id)
        {
            var personalinfo = Get_myinquiries(id);
            var studt = Get_mystudent(id);
            var myevents = Get_UserEvents(id);
            var response = Get_all_responses();
            var userDetails = new UserReport();
            userDetails.Inquiry = personalinfo;
            userDetails.students = studt;
            userDetails.events = myevents;
            userDetails.Response = response;
            return View(userDetails);
        }
        public ActionResult PersonalReports(string id)
        {
            var personalinfo = Get_myinquiries();
            var studt = Get_mystudent();
            var myevents = Get_UserEvents();
            var response = Get_all_responses();
            var userDetails = new UserReport();
            userDetails.Inquiry = personalinfo;
            userDetails.students = studt;
            userDetails.events = myevents;
            userDetails.Response = response;
            return View(userDetails);
        }
        public ActionResult Extract_General_data()
        {
            var users = Get_students();
            var inquiry = Get_all_Inquiries();
            var response = Get_all_responses();
            var calendar_events = Get_all_events();
            var genaralreports = new General_reports();
            genaralreports.students = users;
            genaralreports.Inquiry = inquiry;
            genaralreports.Response = response;
            genaralreports.events = calendar_events;

            return View(genaralreports);
        }
        public ActionResult Extract_Persoanl_data(string id)
        {
            var personalinfo = Get_myinquiries(id);
            var studt = Get_mystudent(id);
            var myevents = Get_UserEvents(id);
            var response = Get_all_responses();
            var userDetails = new UserReport();
            userDetails.Inquiry = personalinfo;
            userDetails.students = studt;
            userDetails.events = myevents;
            userDetails.Response = response;
            return View(userDetails);
        }
        public List<Inquiry> Get_myinquiries(string id)
        {
           

            try
            {
                TempData["inquiries"] = db.Inquiries.Where(a => a.UserId == id).ToList().Count();
                db.Inquiries.Where(a => a.UserId == id).ToList();

            }
            catch (Exception e)
            {
                TempData["error"] = e.Message;
            }

            return (db.Inquiries.Where(a => a.UserId == id).OrderBy(a => a.Dateposteed).ToList());
        }
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

            return (db.Inquiries.Where(a => a.UserId == id).OrderBy(a => a.Dateposteed).ToList());
        }
        public List<Student> Get_mystudent(string id)
        {
            try
            {
               
                TempData["students"] = db.Students.Where(a => a.AccessNumber == id).ToList().Count();
                db.Students.Where(a => a.AccessNumber == id).ToList();

            }
            catch (Exception e)
            {
                TempData["error"] = e.Message;
            }

            return (db.Students.Where(a => a.AccessNumber == id).ToList());
        }
        public List<Student> Get_mystudent()
        {
           var id = (Session["userid"]).ToString();
            try
            {
                
                TempData["students"] = db.Students.Where(a => a.AccessNumber == id).ToList().Count();
                db.Students.Where(a => a.AccessNumber == id).ToList();

            }
            catch (Exception e)
            {
                TempData["error"] = e.Message;
            }

            return (db.Students.Where(a => a.AccessNumber == id).ToList());
        }
        public List<Event> Get_UserEvents(string id)
        {
            try
            {
              
                TempData["events"] = db.Events.Where(a => a.userid == id).ToList().Count();
                db.Events.Where(a => a.userid == id).ToList();

            }
            catch (Exception e)
            {
                TempData["error"] = e.Message;
            }

            return (db.Events.Where(a => a.userid == id).OrderBy(a => a.Start).ToList());
        }
        public List<Event> Get_UserEvents()
        {
            var id = (Session["userid"]).ToString();
            try
            {
               
                TempData["events"] = db.Events.Where(a => a.userid == id).ToList().Count();
                db.Events.Where(a => a.userid == id).ToList();

            }
            catch (Exception e)
            {
                TempData["error"] = e.Message;
            }

            return (db.Events.Where(a => a.userid == id).OrderBy(a => a.Start).ToList());
        }
        public List<Event> Get_all_events()
        {
            return (db.Events.ToList());
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
            return (db.Inquiries.OrderBy(a => a.Dateposteed).ToList());
        }
        public List<Response> Get_all_responses()
        {
            return (db.Responses.OrderBy(a => a.DatetimeOfReply).ToList());
        }
     
        public ActionResult Index()
        {

            return View();
        }
       
        public ActionResult viewallevents(string sortOrder, string currentFilter, string searchString, int? page)
        {
            try
            {
                ViewBag.CurrentSort = sortOrder;
                ViewBag.eventdescription = String.IsNullOrEmpty(sortOrder) ? "Description_desc" : "";
                //ViewBag.eventsubject = String.IsNullOrEmpty(sortOrder) ? "subject_desc" : "";
                ViewBag.Date = sortOrder == "Date" ? "date_desc" : "Date";
                if (searchString != null)
                {
                    page = 1;
                }
                else
                {
                    searchString = currentFilter;
                }

                ViewBag.CurrentFilter = searchString;

                var result = from s in db.Events
                             select s;
                if (!String.IsNullOrEmpty(searchString))
                {
                    result = result.Where(s => s.Description.Contains(searchString)
                                           || s.Subject.Contains(searchString));
                }


                switch (sortOrder)
                {
                    case "Description_desc":
                        result = result.OrderByDescending(s => s.Description);
                        break;
                    case "Date":
                        result = result.OrderBy(s => s.Start);
                        break;
                    case "date_desc":
                        result = result.OrderByDescending(s => s.Start);
                        break;
                    default:
                        result = result.OrderBy(s => s.Start);
                        break;
                }

                int pageSize = 6;
                int pageNumber = (page ?? 1);
                return View(result.ToPagedList(pageNumber, pageSize));
            }
            catch(Exception E)
            {
                TempData["error"] = E.Message;
            }
            return View();
        }
        public ActionResult Myshedules(string sortOrder, string currentFilter, string searchString, int? page)
        {
        try
            {
                string id = Session["userid"].ToString();
                if (id != null)
                {
                    ViewBag.CurrentSort = sortOrder;
                    ViewBag.eventdescription = String.IsNullOrEmpty(sortOrder) ? "Description_desc" : "";
                    //ViewBag.eventsubject = String.IsNullOrEmpty(sortOrder) ? "subject_desc" : "";
                    ViewBag.Date = sortOrder == "Date" ? "date_desc" : "Date";
                    if (searchString != null)
                    {
                        page = 1;
                    }
                    else
                    {
                        searchString = currentFilter;
                    }

                    ViewBag.CurrentFilter = searchString;
                    //var shedule = db.Events.Where(a => a.userid == id).OrderBy(a => a.Start).ToList();

                    var result = from s in db.Events
                                 where s.userid == id
                                 select s;

                    if (!String.IsNullOrEmpty(searchString))
                    {
                        result = result.Where(s => s.Description.Contains(searchString)
                                               || s.Subject.Contains(searchString));
                    }
                    switch (sortOrder)
                    {
                        case "Description_desc":
                            result = result.OrderByDescending(s => s.Description);
                            break;
                        case "Date":
                            result = result.OrderBy(s => s.Start);
                            break;
                        case "date_desc":
                            result = result.OrderByDescending(s => s.Start);
                            break;
                        default:
                            result = result.OrderBy(s => s.Start);
                            break;
                    }

                    int pageSize = 6;
                    int pageNumber = (page ?? 1);
                    return View(result.ToPagedList(pageNumber, pageSize));
                }
            }
            catch (Exception E)
            {
                TempData["error"] = E.Message;
            }
            return View();
        }
        public ActionResult Mycalendar()
        {
            
            return View();
        }
        public ActionResult usercalendar(string id)
        {
            Session["calendarid"] = id;
            return View();
        }
        public JsonResult GetuserEvents()
        {
            using (MyDosa_dbEntities1 dc = new MyDosa_dbEntities1())
            {
                try
                {
                    string id = Session["calendarid"].ToString();
                    if (id == null)
                    {

                        TempData["error"] = "ERROR WHILE VALIDATING USER";
                    }
                    else
                    {
                        var shedule = db.Events.Where(a => a.userid == id).ToList();
                        return new JsonResult { Data = shedule, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                    }

                }
                catch (Exception E)
                {
                    TempData["error"] = E.Message;

                }
                return new JsonResult { Data = null, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
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
                   var id = Session["userid"].ToString();

                    var v = db.Events.Where(a => a.EventID == eventID).FirstOrDefault();
                    if (v != null)
                    {
                        //GENERATE DATA TO SEND MAIL
                        var Adminid = db.Roles.Where(a => a.Role1 == "SUPER ADMINISTRATOR").Select(A => A.Role_id).FirstOrDefault();
                        var adminemailid = db.Userroles.Where(a => a.Roleid == Adminid).Select(a => a.AccessNo).FirstOrDefault();
                        var email = db.Students.Where(a => a.AccessNumber == adminemailid).Select(a => a.Email).FirstOrDefault();
                        var adminname = db.Students.Where(a => a.AccessNumber == adminemailid).Select(a => a.UserName).FirstOrDefault();
                        var username = db.Students.Where(a => a.AccessNumber == id).Select(a => a.UserName).FirstOrDefault();
                        var start = db.Events.Where((a => a.EventID == eventID && a.userid == id)).Select(a => a.Start).FirstOrDefault();
                        var subjects = db.Events.Where((a => a.EventID == eventID && a.userid == id)).Select(a => a.Subject).FirstOrDefault();
                        var reason = db.Events.Where((a => a.EventID == eventID && a.userid == id)).Select(a => a.Description).FirstOrDefault();
                        if (id != null && email != null && username != null && start != null && subjects != null && reason != null && adminname != null)
                        {
                            db.Events.Remove(v);
                            db.SaveChanges();
                            status = true;
                            Send_Delete_Event_Email(email, username, adminname, id, start, subjects, reason);
                            TempData["success"] = "OPERATION SUCCESSFULL, DOSA OFICE HAS BEEN NOTIFIED";

                        }
                        else
                        {
                            TempData["error"] = "ERROR OCCURED WHILE NOTIFYING  DOSA OFFICE KINDLYTRY AGAIN";
                        }

                    }
                    else
                    {
                        TempData["error"] = "INVALID REFRENCE";
                    }
                
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
            }
            return new JsonResult { Data = new { status = status } };
        }

        public void Send_Delete_Event_Email(string emailId, string studentname, string usernames, string id, DateTime start, string subjects, string reason)
        {
            var emailsending = ConfigurationManager.AppSettings["email_sender"];
            var pass = ConfigurationManager.AppSettings["fromEmailPassword"];
            var fromport = ConfigurationManager.AppSettings["emailport"];
            var smtpclient = ConfigurationManager.AppSettings["SmtpClient"];


            var fromEmail = new MailAddress(emailsending, "UGANDA CHRISTIAN UNIVERSITY(DOSA)");
            var toEmail = new MailAddress(emailId);
            //this password is generated by u in ur email account
            var fromEmailPassword = pass;

            var subject = "STUDENT TRAVEL SCHEDULE UPDATE";
            var body1 = "Grettings" + usernames + ", " + studentname + " With Access Number : " + id + " Has WITHDRAWN Their Travel Schedule that was intended to happen" + "<br/>"

                 + "ON " + start + "<br/>" + "<br/>"
                   + "The schedule had the folowing details :" + "<br/>" +
                  "SUBJECT: "+ subjects + "<br/>" + "<br/>"
            + " REASON :" + reason + ".<br/> <br/> Kindly check the Platform for more details";

            var smtp = new SmtpClient(smtpclient)
            {
                Port = Convert.ToInt32(fromport),
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new System.Net.NetworkCredential(fromEmail.Address, fromEmailPassword),
            };

            string body = string.Empty;
            var root = AppDomain.CurrentDomain.BaseDirectory;

            using (var reader = new System.IO.StreamReader(root + @"/Template/html/email.html"))
            {
                string readFile = reader.ReadToEnd();
                string StrContent = string.Empty;
                StrContent = readFile;
                //Assing the field values in the template
                StrContent = StrContent.Replace("[body]", body1);

                body = StrContent.ToString();
            }

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
                //Fetching Email Body Text from EmailTemplate File.  
            })
                try
                {
                    smtp.Send(message);
                }
                catch (Exception e)
                {
                    TempData["error"] = e.Message;
                }
        }

        public void SendSheduleEmail(string emailId,string studentname, string usernames,string id, DateTime start,string subjects, string reason)
        {
            var emailsending = ConfigurationManager.AppSettings["email_sender"];
            var pass = ConfigurationManager.AppSettings["fromEmailPassword"];
            var fromport = ConfigurationManager.AppSettings["emailport"];
            var smtpclient = ConfigurationManager.AppSettings["SmtpClient"];

            var fromEmail = new MailAddress(emailsending, "UGANDA CHRISTIAN UNIVERSITY(DOSA)");
            var toEmail = new MailAddress(emailId);
            //this password is generated by u in ur email account
            var fromEmailPassword = pass;

            var subject = "STUDENT TRAVEL SCHEDULE UPDATE";
            var body1 = "Grettings" +  usernames + ", "+ studentname+ " With Access Number : " + id + " Has shedule Their Travel Intensions," + "<br/> <br/>"

                 + "From " + start + " with an estimated arrival on ... <br/> <br/>"
                   + " TRAVEL SUBJECT :" + subjects + "<br/>"
            + " REASON :" + reason + ".<br/><br/> Kindly check the Platform for more details";

            var smtp = new SmtpClient(smtpclient)
            {
                Port =Convert.ToInt32(fromport),
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new System.Net.NetworkCredential(fromEmail.Address, fromEmailPassword),
            };

            string body = string.Empty;
            var root = AppDomain.CurrentDomain.BaseDirectory;

            using (var reader = new System.IO.StreamReader(root + @"/Template/html/email.html"))
            {
                string readFile = reader.ReadToEnd();
                string StrContent = string.Empty;
                StrContent = readFile;
                //Assing the field values in the template
                StrContent = StrContent.Replace("[body]", body1);

                body = StrContent.ToString();
            }

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
                //Fetching Email Body Text from EmailTemplate File.  
            })
                try
                {
                    smtp.Send(message);
                }
                catch (Exception e)
                {
                    TempData["error"] = e.Message;
                }
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EventID,Subject,Description,Start,End,ThemeColor,IsFullDay")] Event @event)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if ((@event.Start < DateTime.Now || @event.End < DateTime.Now))
                    {
                        TempData["error"] = "INVALID SET-OFF / ARRIVAL DATE";
 
                    }
                    else
                    {
                        var id = Session["userid"].ToString();
                        @event.userid = Session["userid"].ToString();
                        db.Events.Add(@event);
                        db.SaveChanges();

                        //GENERATE DATA TO SEND MAIL
                        var Adminid = db.Roles.Where(a => a.Role1 == "SUPER ADMINISTRATOR").Select(A => A.Role_id).FirstOrDefault();
                        var adminemailid = db.Userroles.Where(a => a.Roleid == Adminid).Select(a => a.AccessNo).FirstOrDefault();
                        var email = db.Students.Where(a => a.AccessNumber == adminemailid).Select(a => a.Email).FirstOrDefault();
                        var adminname = db.Students.Where(a => a.AccessNumber == adminemailid).Select(a => a.UserName).FirstOrDefault();

                        var username = db.Students.Where(a => a.AccessNumber == id).Select(a => a.UserName).FirstOrDefault();
                        var start = db.Events.Where((a => a.Start == @event.Start && a.userid == id)).Select(a => a.Start).FirstOrDefault();
                        var subjects = db.Events.Where((a => a.Start == @event.Start && a.userid == id)).Select(a => a.Subject).FirstOrDefault();
                        var reason = db.Events.Where((a => a.Start == @event.Start && a.userid == id)).Select(a => a.Description).FirstOrDefault();

                        if (id != null && email != null && username != null && start != null && subjects != null && reason != null && adminname != null)
                        {
                            SendSheduleEmail(email, username, adminname, id, start, subjects, reason);
                            TempData["success"] = "OPERATION SUCCESSFULL, DOSA's OFICE HAS BEEN NOTIFIED";
                            return RedirectToAction("MyCalendar");
                        }
                        else
                        {
                            TempData["error"] = "ERROR OCCURED WHILE NOTIFYING  DOSA OFFICE KINDLYTRY AGAIN";
                        }
                    }
                }
                else
                {
                    //var modelStateErrors = this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors);
                    //TempData["error"] = getallerrors(ModelState);                  
                    var modelStateErrors = this.ModelState.Values.SelectMany(m => m.Errors);
                    TempData["error"] = modelStateErrors;
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
            }
            return View(@event);
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
