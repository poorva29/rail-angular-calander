using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MiraiConsultMVC.EFModels;
using System.Configuration;
using CCA.Util;

namespace MiraiConsultMVC.Controllers
{
    public class cca_paymentController : Controller
    {
        private EFModelContext db = new EFModelContext();
        private CCACrypto ccaCrypto = new CCACrypto();

        // Get: complete_payment
        public ActionResult CompletePayment(cca_payment payment)
        {
            return View();
        }

        // GET: cca_payment/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            cca_payment cca_payment = db.cca_payments.Find(id);
            if (cca_payment == null)
            {
                return HttpNotFound();
            }
            return View(cca_payment);
        }

        // GET: cca_payment/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: cca_payment/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,amount,order_id,currency,card_name,payment_mode,bank_ref_no,tracking_id,order_status,failure_message,status_message,billing_name,billing_address,billing_city,billing_state,billing_zip,billing_country,billing_tel,billing_email,delivery_name,delivery_address,delivery_city,delivery_state,delivery_zip,delivery_country,delivery_tel,status_code,answerid,questionid,userid,title,answertext,createdate,image,deviceid")] cca_payment cca_payment)
        {
            if (ModelState.IsValid)
            {
                db.cca_payments.Add(cca_payment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cca_payment);
        }
    }
}
