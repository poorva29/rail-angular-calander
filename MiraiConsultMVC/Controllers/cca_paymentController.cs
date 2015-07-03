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
using System.Collections.Specialized;
using System.Globalization;

namespace MiraiConsultMVC.Controllers
{
    public class cca_paymentController : Controller
    {
        private EFModelContext db = new EFModelContext();
        private CCACrypto ccaCrypto = new CCACrypto();

        // Get: complete_payment
        [AllowAnonymous]
        public ActionResult CompletePayment(cca_payment payment)
        {
            string workingKey = ConfigurationManager.AppSettings["WorkingKey"].ToString();//put in the 32bit alpha numeric key in the quotes provided here
            CCACrypto ccaCrypto = new CCACrypto();
            string encResponse = ccaCrypto.Decrypt(Request.Form["encResp"], workingKey);
            NameValueCollection Params = new NameValueCollection();
            string[] segments = encResponse.Split('&');
            //split response into key-value pair
            foreach (string seg in segments)
            {
                string[] parts = seg.Split('=');
                if (parts.Length > 0)
                {
                    string Key = parts[0].Trim();
                    string Value = parts[1].Trim();
                    Params.Add(Key, Value);
                }
            }
            int appointmentId = 0;
            using (var context = new EFModelContext())
            {
                cca_payment ccapayment = new cca_payment();
                ccapayment.amount = float.Parse(Convert.ToString(Params["amount"]), CultureInfo.InvariantCulture.NumberFormat);
                ccapayment.order_id = Convert.ToString(Params["order_id"]);
                ccapayment.currency = Convert.ToString(Params["currency"]);
                ccapayment.card_name = Convert.ToString(Params["card_name"]);
                ccapayment.payment_mode = Convert.ToString(Params["payment_mode"]);
                ccapayment.bank_ref_no = Convert.ToString(Params["bank_ref_no"]);
                ccapayment.tracking_id = Convert.ToString(Params["tracking_id"]);
                ccapayment.order_status = Convert.ToString(Params["order_status"]);
                ccapayment.failure_message = Convert.ToString(Params["failure_message"]);
                ccapayment.status_message = Convert.ToString(Params["status_message"]);
                ccapayment.billing_name = Convert.ToString(Params["billing_name"]);
                ccapayment.billing_address = Convert.ToString(Params["billing_address"]);
                ccapayment.billing_city = Convert.ToString(Params["billing_city"]);
                ccapayment.billing_state = Convert.ToString(Params["billing_state"]);
                ccapayment.billing_zip = Convert.ToString(Params["billing_zip"]);
                ccapayment.billing_country = Convert.ToString(Params["billing_country"]);
                ccapayment.billing_tel = Convert.ToString(Params["billing_tel"]);
                ccapayment.billing_email = Convert.ToString(Params["billing_email"]);
                ccapayment.delivery_name = Convert.ToString(Params["delivery_name"]);
                ccapayment.delivery_address = Convert.ToString(Params["delivery_address"]);
                ccapayment.delivery_city = Convert.ToString(Params["delivery_city"]);
                ccapayment.delivery_state = Convert.ToString(Params["delivery_state"]);
                ccapayment.delivery_zip = Convert.ToString(Params["delivery_zip"]);
                ccapayment.delivery_country = Convert.ToString(Params["delivery_country"]);
                ccapayment.delivery_tel = Convert.ToString(Params["delivery_tel"]);
                ccapayment.status_code = Convert.ToString(Params["status_code"]);
                context.cca_payments.Add(ccapayment);
                context.SaveChanges();
                if (Params["order_id"].Contains("APPT"))
                {
                    appointmentId = Convert.ToInt32(Convert.ToString(Params["order_id"]).Split('-')[1]);
                    appointment appointment = context.appointments.Find(appointmentId);
                    if (appointment != null)
                    {
                        appointment.cca_order = Convert.ToString(Params["order_id"]);
                        appointment.ispaid = true;
                        context.SaveChanges();
                    }
                }
            }
            if (appointmentId == 0)
            {
                return Content("INVALID REQUEST!!");
            }
            else
            {
                return Redirect(Url.RouteUrl("appt_paid", new { id = appointmentId }));
            }
        }
    }
}
