﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
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

                    if (ROLE.ToString() == "ADMINISTRATOR" || ROLE.ToString() == "SUPER ADMINISTRATOR")
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
                //GENERATE DATA TO SEND MAIL
                var Adminid = db.Roles.Where(a => a.Role1 == "SUPER ADMINISTRATOR").Select(A => A.Role_id).FirstOrDefault();
                var adminemailid = db.Userroles.Where(a => a.Roleid == Adminid).Select(a => a.AccessNo).FirstOrDefault();
                var email = db.Students.Where(a => a.AccessNumber == adminemailid).Select(a => a.Email).FirstOrDefault();
                var adminname = db.Students.Where(a => a.AccessNumber == adminemailid).Select(a => a.UserName).FirstOrDefault();

                var username = db.Students.Where(a => a.AccessNumber == inquiry.UserId).Select(a => a.UserName).FirstOrDefault();
                var dateposted = inquiry.Dateposteed;
                var inq = inquiry.inquirry;

                if (inquiry.UserId != null && email != null && username != null && dateposted != null && inq != null && adminname != null)
                {
                    SendEmail(inquiry.UserId, email, username, adminname, dateposted, inq);
                    TempData["success"] = "THANK YOU FOR SUBMIITING YOUR INQUIRY WE SHALL GET BACK TO YOU.";

                }
                else
                {
                    TempData["error"] = "ERROR VALIDATING SENDER DETAILS";
                }

                return RedirectToAction("myInquiries");
            }

            return View(inquiry);
        }

        public void SendEmail(string id, string emailId, string studentname, string adminname,  DateTime dateposted,  string inq)
        {
            var fromEmail = new MailAddress("testmarvinug@gmail.com", "UGANDA CHRISTIAN UNIVERSITY(DOSA)");
            var toEmail = new MailAddress(emailId);
            //this password is generated by u in ur email account
            var fromEmailPassword = "kcywjucbmujbrycc";

            var subject = "STUDENT INQUIRY UPDATE";
            var body1 = "Grettings " + adminname + ", " + studentname + " With Access Number : " + id + " Has added an inquiry to your Space," + " <br/>"

                 + "On " + dateposted +" <br/> <br/>"
                   + "EXACT INQUIRY :" + inq + "<br/>"+
           "<br/> Kindly check the Platform for more details";

            var smtp = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
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
