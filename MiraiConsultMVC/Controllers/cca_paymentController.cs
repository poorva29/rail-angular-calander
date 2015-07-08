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
                int doctorId;

                if (Params["order_id"].Contains("APPT"))
                {
                    appointmentId = Convert.ToInt32(Convert.ToString(Params["order_id"]).Split('-')[1]);
                    appointment appointment = context.appointments.Find(appointmentId);
                    if (appointment != null)
                    {
                        appointment.cca_order = Convert.ToString(Params["order_id"]);
                        appointment.ispaid = true;
                        appointment.cca_paid_on = System.DateTime.Now;
                        context.SaveChanges();
                    }
                    doctorId = appointment.doctorid;
                    var patients = new
                        {
                            name = string.Empty
                        };
                    if(appointment.patientid != -1)
                    {                       
                        if (appointment.patientid == 0)
                        {
                            patients = (from un in context.unregpatients
                                        join app in context.appointments on un.id equals app.unregpatientid
                                        where app.appointmentid == appointment.appointmentid
                                        select new { name = un.name }).FirstOrDefault();
                        }
                        else if (appointment.patientid != 0)
                        {
                            patients = (from u in context.users
                                        join app in context.appointments on u.userid equals app.unregpatientid
                                        where app.appointmentid == appointment.appointmentid
                                        select new { name = u.firstname + " " + u.lastname }).FirstOrDefault();
                        }
                    }
                    user docDetail = context.users.Find(appointment.doctorid);
                    string doctorFullName = docDetail.firstname + " " + docDetail.lastname;
                    string[] dateArray = Convert.ToString(appointment.starttime).Split(' ');                   
                    doctorlocation docLocation = context.doctorlocations.Find(appointment.doclocationid);
                    city city = context.cities.Find(docLocation.cityid);
                   
                    string fromEmail = ConfigurationManager.AppSettings["FromEmail"].ToString();
                    string subject = "Mirai Health - Successful payment for pre-paid appoinment";
                    string Logoimage = Server.MapPath(@"~/Content/image/LogoForMail.png");
                    string body = EmailTemplates.SendSuccessfulNotificationToDoctorForPrepaidAppt(doctorFullName, patients.name, appointment.starttime, docLocation.clinicname, city.name, Convert.ToDecimal(appointment.prepayamount));
                    if(appointment.txncode != null)
                    Mail.SendHTMLMailWithImage(fromEmail, docDetail.email, subject, body, Logoimage);   
                    
                    string textMsg = ConfigurationManager.AppSettings["SuccessfulPaymentNotificationToDoctor"].ToString();                    
                    textMsg = textMsg.Replace("@doctor", doctorFullName);
                    textMsg = textMsg.Replace("@patientname", patients.name);
                    textMsg = textMsg.Replace("@date", Utilities.GetDisplayDate(Convert.ToDateTime(dateArray[0])));
                    textMsg = textMsg.Replace("@time", Utilities.GetDisplayTime(dateArray[1]));
                    textMsg = textMsg.Replace("@clinicName", docLocation.clinicname + ", " + city.name);
                    if (appointment.txncode != null)
                    SMS.SendSMS(docDetail.mobileno, textMsg);
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
