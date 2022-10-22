using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.NetworkInformation;
using System.Web;
using System.Web.Mvc;
using PagedList;
using sheduler.Models;
using PagedList.Mvc;
using System.Configuration;

namespace sheduler.Controllers
{
    //[Authorize]
    [HandleError]
    public class ResponsesController : Controller
    {
        private MyDosa_dbEntities1 db = new MyDosa_dbEntities1();

        // GET: Responses

        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            try
            {
                ViewBag.CurrentSort = sortOrder;
                ViewBag.responsedesc = String.IsNullOrEmpty(sortOrder) ? "responsedesc" : "";
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

                var result = from s in db.Responses
                            
                             select s;
                result = db.Responses.Include(r => r.Inquiry);
                if (!String.IsNullOrEmpty(searchString))
                {
                    result = result.Where(s => s.Response1.Contains(searchString)
                                         );
                }
                switch (sortOrder)
                {
                    case "Description_desc":
                        result = result.OrderByDescending(s => s.Response1);
                        break;
                    case "Date":
                        result = result.OrderBy(s => s.DatetimeOfReply);
                        break;
                    case "date_desc":
                        result = result.OrderByDescending(s => s.DatetimeOfReply);
                        break;
                    default:
                        result = result.OrderBy(s => s.DatetimeOfReply);
                        break;
                }

                int pageSize = 6;
                int pageNumber = (page ?? 1);
               
                return View(result.ToPagedList(pageNumber, pageSize));
            }
            catch (Exception E)
            {
                TempData["error"] = E.Message;
            }
            return View();
        }
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
                    var id = Session["userid"].ToString();
               
                    //GENERATE DATA TO SEND MAIL
                    var inqid = response.Inquirery_id;

                    var userid = db.Inquiries.Where(a=>a.Inquirery_id == inqid).Select(a=>a.UserId).FirstOrDefault();
                    
                    if (userid !=null && inqid > 0)
                    {
                        var username = db.Students.Where(a => a.AccessNumber == userid).Select(a => a.UserName).FirstOrDefault();
                        var useremail = db.Students.Where(a => a.AccessNumber == userid).Select(a => a.Email).FirstOrDefault();
                        var PostedDate = db.Inquiries.Where(a => a.Inquirery_id == inqid).Select(a => a.Dateposteed).FirstOrDefault();
                        var inq = db.Inquiries.Where(a => a.Inquirery_id == inqid).Select(a => a.inquirry).FirstOrDefault();
                        var adminusername = db.Students.Where(a => a.AccessNumber == id).Select(a => a.UserName).FirstOrDefault();
                        response.DatetimeOfReply = DateTime.Now;
                        db.Responses.Add(response);
                        db.SaveChanges();
                        TempData["success"] = "SUCCESSFULY RESPONDED";
                        Send_Response_Email(useremail, username, adminusername, PostedDate, inq,response.Response1);
                    }
                    else
                    {
                        TempData["error"] = "ERROR VALIDATING SENDER DETAILS";
                    }

                    return RedirectToAction("Index","Inquiries");
                }

            }catch(Exception e)
            {
                TempData["errorsaving"]  = e.Message;
            }
            ViewBag.Inquirery_id = new SelectList(db.Inquiries, "Inquirery_id", "inquirry", response.Inquirery_id);
            return View(response);
        }

        public void Send_Response_Email(string emailId, string username, string adminusername, DateTime PostedDate, string inq,string response)
        {
            var emailsending = ConfigurationManager.AppSettings["email_sender"];
            var pass = ConfigurationManager.AppSettings["fromEmailPassword"];
            var fromport = ConfigurationManager.AppSettings["emailport"];
            var smtpclient = ConfigurationManager.AppSettings["SmtpClient"];

            var fromEmail = new MailAddress(emailsending, "UGANDA CHRISTIAN UNIVERSITY(DOSA)");
            var toEmail = new MailAddress(emailId);
            //this password is generated by u in ur email account
            var fromEmailPassword = pass;

            var subject = "INQUIRY UPDATE";
            var body1 = "Grettings" + username + ", " + "<br/>" + "<br/>"
                +"With Reference to your inquiry that was posted on  " + PostedDate + " To OurTeam, We are glad to inform you that we recieved it and we have presponded to it " + "<br/> <br/>"
                 + "YOUR INQUIRY <br/>"
                 +inq+ "<br/><br/>"
                 + "RESPONSE <br/>"
                   + response + ".<br/> <br/>" +
               
           "BY " + "<br/>" + adminusername;

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
