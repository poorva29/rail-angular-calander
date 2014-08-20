using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.ServiceModel.Activation;

namespace MiraiConsultMVC
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("Services/UserService.svc/{*pathInfo}");
            routes.IgnoreRoute("Content/image/DoctorPhotos/{*pathInfo}");

            routes.MapRoute(
                name: "PatientProfile",
                url: "patientprofile",
                defaults: new { controller = "Patients", action = "PatientProfile" }
                );

            routes.MapRoute(
               name: "forgotpassword",
               url: "forgotpassword",
               defaults: new { controller = "User", action = "forgotpassword" }
           );

            routes.MapRoute(
               name: "Howweworks",
               url: "howweworks",
               defaults: new { controller = "Home", action = "HowWeWorks" }
           );

            routes.MapRoute(
                name: "Signup-Patient",
                url: "signup-patient",
                defaults: new { controller = "User", action = "PatientSignUp" }
            );

            routes.MapRoute(
                name: "Signup-Doctor",
                url: "signup-doctor",
                defaults: new { controller = "User", action = "DoctorSignUp" }
            );

            routes.MapRoute(
                name: "Login",
                url: "Login",
                defaults: new { controller = "User", action = "Login" }
            );

            routes.MapRoute(
               name: "PrivacyPolicy",
               url: "privacypolicy",
               defaults: new { controller = "Home", action = "PrivacyPolicy" }
           );

            routes.MapRoute(
               name: "Contact",
               url: "contact",
               defaults: new { controller = "Home", action = "Contact" }
           );

            routes.MapRoute(
               name: "termsOfUse",
               url: "termsofuse",
               defaults: new { controller = "Home", action = "TermsOfUse" }
           );

            routes.MapRoute(
                name: "AdminQuestionList",
                url: "admin/{filter}",
                defaults: new { controller = "admin", action = "QuestionList", filter = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "AdminManageTags",
                url: "tags",
                defaults: new { controller = "admin", action = "managetag" }
            );

            routes.MapRoute(
                name: "AdminReport",
                url: "report",
                defaults: new { controller = "admin", action = "Report" }
            );

            routes.MapRoute(
                name: "AdminRegistrationRequests",
                url: "doctors",
                defaults: new { controller = "User", action = "ManageDoctors" }
            );
            
            

            routes.MapRoute(
                name: "Changepassword",
                url: "changepassword",
                defaults: new { controller = "User", action = "Changepassword" }
            );

            routes.MapRoute(
                name: "InviteFriend",
                url: "invitefriend",
                defaults: new { controller = "Patients", action = "InviteFriend" }
            );

            routes.MapRoute(
               name: "AskDoctor",
               url: "askdoctor",
               defaults: new { controller = "Patients", action = "AskDoctor" }
           );

            routes.MapRoute(
               name: "Myactivity",
               url: "myactivity",
               defaults: new { controller = "Patients", action = "Myactivity" }
           );

            routes.MapRoute(
               name: "FindDoctors",
               url: "finddoctors",
               defaults: new { controller = "Home", action = "FindDoctors" }
           );

            routes.MapRoute(
                name: "Questions2",
                url: "questions/{QuestionId}",
                defaults: new { controller = "Questions", action = "DoctorQuestionDetails" },
                constraints: new { QuestionId = @"\d+" }
            );

            routes.MapRoute(
                name: "Questions1",
                url: "questions/{filter}",
                defaults: new { controller = "Questions", action = "DoctorQuestionList", filter = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "assignquestion",
                url: "assignquestion/{questionid}",
                defaults: new {controller="admin",action="assignquestion" },
                constraints: new { QuestionId = @"\d+" }
                );
            

            routes.MapRoute(
                name: "Answers1",
                url: "answers",
                defaults: new { controller = "feed", action = "feed" }
            );

            routes.MapRoute(
                name: "Answers2",
                url: "doctor-answers",
                defaults: new { controller = "feed", action = "doctorfeed" }
            );

            routes.MapRoute(
                name: "DoctorProfile",
                url: "doctorprofile",
                defaults: new { controller = "Doctors", action = "DoctorProfile" }
                
            );

            routes.MapRoute(
               name: "DoctorProfile1",
               url: "doctorprofile/{UserId}",
               defaults: new { controller = "Doctors", action = "DoctorProfile" }
               
           );

            routes.MapRoute(
                name: "Answers3",
                url: "answers/{QuestionId}",
                defaults: new { controller = "Patients", action = "PatientQuestionDetails" },
                constraints: new { QuestionId = @"\d+" }
            );

            routes.MapRoute(
                name: "similarQuestions",
                url: "similarquestions",
                defaults: new { controller = "Patients", action = "similarQuestions" }
                );

            routes.MapRoute(
               name: "similarQuestions1",
               url: "similarquestions/{question}",
               defaults: new { controller = "Patients", action = "similarQuestions" }
           );

            routes.MapRoute(
               name: "AssignDoctor",
               url: "assigndoctor/{QuestionId}/{AssignDoctorIds}",
               defaults: new { controller = "admin", action = "AssignDoctor" }

           );

            routes.MapRoute(
               name: "RemoveAssignDoctorToQuetion",
               url: "removeassigndoctortoquetion/{userid}/{questionId}",
               defaults: new { controller = "admin", action = "RemoveAssignDoctorToQuetion" }

           );

            routes.MapRoute(
                name: "RejectQuestionByQuestionID",
                url: "RejectQuestionByQuestionID/{qusetionID}",
                defaults: new { controller = "admin", action = "RejectQuestionByQuestionID" }
           );

            routes.MapRoute(
               name: "Default",
               url: "{controller}/{action}/{id}",
               defaults: new { controller = "Home", action = "Home", id = UrlParameter.Optional }
           );
        }
    }
}