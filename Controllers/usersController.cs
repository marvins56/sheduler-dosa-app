using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using sheduler.Models;

namespace sheduler.Controllers
{
    //[Authorize]
    //[HandleError]
    public class usersController : Controller
    {
        private MyDosa_dbEntities1 db = new MyDosa_dbEntities1();

        public ActionResult Index()
        {
            var students = db.Students.Include(s => s.Campus_Branches).Include(s => s.Cours).Include(s => s.semester).Include(s => s.UserLocation).Include(s => s.Year);
            return View(students.ToList());
        }
        public ActionResult Admin()
        {
            try
            {
                ViewBag.inquiries_no = db.Inquiries.ToList().Count();
                ViewBag.responsed_to = db.Responses.ToList().Count();
                var n1 = Convert.ToInt32(ViewBag.inquiries_no);
                var n2 = Convert.ToInt32(ViewBag.responsed_to);
                int pending = sum( n1,n2 );
                ViewBag.pending = pending;
                ViewBag.allusers = db.Students.ToList().Count();
                ViewBag.events = db.Events.ToList().Count();
            }
            catch(Exception E)
            {
                TempData["error"] = E.Message;
            }

            return View();
        }
        public ActionResult Users()
        {
            var students = db.Students.Include(s => s.Campus_Branches).Include(s => s.Cours).Include(s => s.semester).Include(s => s.UserLocation).Include(s => s.Year);
            return View(students.ToList());
        }
        public int  sum(int a, int b)
        {

            return (a - b);
        }
        public ActionResult UserInfo()
        {
            var students = db.Students.Include(s => s.Campus_Branches).Include(s => s.Cours).Include(s => s.semester).Include(s => s.UserLocation).Include(s => s.Year);
            return View(students.ToList());
        }
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }
        public ActionResult Create()
        {
            ViewBag.campus_id = new SelectList(db.Campus_Branches, "campus_id", "Campus_branch");
            ViewBag.Course_code = new SelectList(db.Courses, "Course_code", "courseName");
            ViewBag.sem_id = new SelectList(db.semesters, "sem_id", "Semester_taken");
            ViewBag.AccessNumber = new SelectList(db.UserLocations, "AccessNumber", "From_Address");
            ViewBag.Year_Id = new SelectList(db.Years, "Year_id", "yearName");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AccessNumber,UserName,Email,Course_code,sem_id,Year_Id,campus_id,Country,Passcode")] Student student)
        {
            if (ModelState.IsValid)
            {
                db.Students.Add(student);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.campus_id = new SelectList(db.Campus_Branches, "campus_id", "Campus_branch", student.campus_id);
            ViewBag.Course_code = new SelectList(db.Courses, "Course_code", "courseName", student.Course_code);
            ViewBag.sem_id = new SelectList(db.semesters, "sem_id", "Semester_taken", student.sem_id);
            ViewBag.AccessNumber = new SelectList(db.UserLocations, "AccessNumber", "From_Address", student.AccessNumber);
            ViewBag.Year_Id = new SelectList(db.Years, "Year_id", "yearName", student.Year_Id);
            return View(student);
        }
        public ActionResult SelfRegister()
        {
            ViewBag.campus_id = new SelectList(db.Campus_Branches, "campus_id", "Campus_branch");
            ViewBag.Course_code = new SelectList(db.Courses, "Course_code", "courseName");
            ViewBag.sem_id = new SelectList(db.semesters, "sem_id", "Semester_taken");
            ViewBag.AccessNumber = new SelectList(db.UserLocations, "AccessNumber", "From_Address");
            ViewBag.Year_Id = new SelectList(db.Years, "Year_id", "yearName");
            return View();
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SelfRegister([Bind(Include = "AccessNumber,UserName,Email,Course_code,sem_id,Year_Id,campus_id,Country,Passcode")] Student student)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Students.Add(student);
                    db.SaveChanges();
                    return RedirectToAction("Login");
                }

            }
            catch (Exception e)
            {
                TempData["regerror"] = e.Message;
            }

            ViewBag.campus_id = new SelectList(db.Campus_Branches, "campus_id", "Campus_branch", student.campus_id);
            ViewBag.Course_code = new SelectList(db.Courses, "Course_code", "courseName", student.Course_code);
            ViewBag.sem_id = new SelectList(db.semesters, "sem_id", "Semester_taken", student.sem_id);
            ViewBag.AccessNumber = new SelectList(db.UserLocations, "AccessNumber", "From_Address", student.AccessNumber);
            ViewBag.Year_Id = new SelectList(db.Years, "Year_id", "yearName", student.Year_Id);
            return View(student);
        }
        [HttpGet]
        public ActionResult Login()
        {

            return View();
        }
        public ActionResult Login(Student users)
        {
            try
            { //check if email and access number exist

                var accessno = db.Students.Where(a => a.AccessNumber == users.AccessNumber).Select(a => a.AccessNumber).FirstOrDefault();
                var Email = db.Students.Where(a => a.AccessNumber == users.AccessNumber).Select(a => a.Email).FirstOrDefault();
                var passcode = db.Students.Where(a => a.AccessNumber == users.AccessNumber).Select(a => a.Passcode).FirstOrDefault();
                //check if email and access number are not empty
                if(accessno != null)
                { //generate code
                    Session["userid"] = accessno;
                    var pass = Generate_Pass();
                    passcode = pass;
                    int res = db.Database.ExecuteSqlCommand("Update Students  set Passcode = '" + pass + "' where AccessNumber = '" + accessno + "'");
                    db.SaveChanges();
                    if (res > 0)
                    {
                        SendEmailPasscode(Email, passcode);
                        TempData["success"] = "HELLO " + Session["userid"]+ ". Security Code has been sent to " + Email;
                        return RedirectToAction("verify");
                    }
                }
                else
                {
                    TempData["error"] = "USER NOT FOUND";                  
                }
            }
            catch(Exception e)
            {
                TempData["logineror"] = e.Message;
            }

            return View();
        }

        [NonAction]
        public string Generate_Pass()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()";
            var stringChars = new char[6];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            string finalString = new String(stringChars);

            return finalString;
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Student student = db.Students.Find(id);
            db.Students.Remove(student);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public void SendEmailPasscode(string emailId, string passcode)
        {
            var fromEmail = new MailAddress("testmarvinug@gmail.com", "MyDosa webapp");
            var toEmail = new MailAddress(emailId);
            //this password is generated by u in ur email account
            var fromEmailPassword = "kcywjucbmujbrycc";
            var subject = "DOSA SECURITY CODE";
            var body =  Session["userid"] + ", YOUR  SECURITY CODE IS :   " + passcode;/* + "<br /> <a href='" + link + "'> click here</a>";*/
            var smtp = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,

                Credentials = new System.Net.NetworkCredential(fromEmail.Address, fromEmailPassword),

            };
            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true

            })
                try
                {
                    smtp.Send(message);
                }
                catch (Exception ex)
                {
                    TempData["emailerror"] = ex.Message;
                }
        }

        [HttpGet]
        public ActionResult verify()
        {
            return View();
        }
        [HttpPost]
        public ActionResult verify(Student passcodes)
        {

            var userid = Session["userid"].ToString();
            var userrolesz = db.Userroles.Where(a => a.AccessNo == userid).Select(a =>a.Roleid).FirstOrDefault();
            var actualrole = db.Roles.Where(a => a.Role_id == userrolesz).Select(a => a.Role1).FirstOrDefault();
            try
            {
                var Passcode = db.Students.Where(a => a.AccessNumber == userid).Select(a => a.Passcode).FirstOrDefault();
                var idz = db.Students.Where(a => a.AccessNumber == userid).Select(a => a.AccessNumber).FirstOrDefault();
                if (Passcode == passcodes.Passcode)
                {
                    
                    if(idz == userid)
                    {
                        Session["userid"] = idz.ToString();
                        //TempData["success"] = "verified";
                        if (actualrole == "ADMINISTRATOR" || actualrole == "SUPER ADMINISTRATOR")
                        {
                            Session["userroles"] = actualrole;
                            return RedirectToAction("Admin", "Users");
                        }
                        else
                        {
                            Session["userroles"] = actualrole;
                            return RedirectToAction("index", "Home");
                        }
                    }
                    else
                    {
                        TempData["error"] = "ACCESS DENIED";
                    }



                }
                else
                {
                    TempData["error"] = "invalid code";
                }
            }
            catch (Exception e)
            {
                TempData["error"] = e.Message;
            }


            return View(passcodes);
        }
        public ActionResult logout()
        {
            Session.Clear();
            return RedirectToAction("login");
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
