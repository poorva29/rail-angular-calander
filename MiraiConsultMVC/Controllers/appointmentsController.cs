using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MiraiConsultMVC.EFModels;

namespace MiraiConsultMVC.Controllers
{
    [Authorize]
    public class appointmentsController : Controller
    {
        private EFModelContext db = new EFModelContext();

        // GET: appointments
        public ActionResult Index()
        {
            return View(db.appointments.ToList());
        }

        // GET: prepay
        [AllowAnonymous]
        public ActionResult prepay(string code)
        {
            appointment appointment = db.appointments.Where(a => a.txncode == code).
                Include(a => a.doclocation).FirstOrDefault();
            if (appointment != null)
            {
                ViewBag.doc = db.users.Find(appointment.doctorid);
            }
            return View(appointment);
        }

        // GET: appointments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            appointment appointment = db.appointments.Find(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }
            return View(appointment);
        }

        // GET: appointments/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: appointments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "appointmentid,subject,patientid,doctorid,docassistantid,doclocationid,description,starttime,endtime,isalldayevent,appointmenttypeid,status,createdat,createdby,lastmodifiedat,lastmodifiedby,patientname,patientemail,patientmobile,comments,feedback,rating,unregpatientid,note,instruction,deviceid,prepayamount,prepay_by,txncode,ispaid,cca_order,cca_status,cca_paid_on,cca_amount")] appointment appointment)
        {
            if (ModelState.IsValid)
            {
                db.appointments.Add(appointment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(appointment);
        }

        // GET: appointments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            appointment appointment = db.appointments.Find(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }
            return View(appointment);
        }

        // POST: appointments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "appointmentid,subject,patientid,doctorid,docassistantid,doclocationid,description,starttime,endtime,isalldayevent,appointmenttypeid,status,createdat,createdby,lastmodifiedat,lastmodifiedby,patientname,patientemail,patientmobile,comments,feedback,rating,unregpatientid,note,instruction,deviceid,prepayamount,prepay_by,txncode,ispaid,cca_order,cca_status,cca_paid_on,cca_amount")] appointment appointment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(appointment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(appointment);
        }

        // GET: appointments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            appointment appointment = db.appointments.Find(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }
            return View(appointment);
        }

        // POST: appointments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            appointment appointment = db.appointments.Find(id);
            db.appointments.Remove(appointment);
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
