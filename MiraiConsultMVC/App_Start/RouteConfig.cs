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
                url: "PatientProfile",
                defaults: new { controller = "Patients", action = "PatientProfile" }
                );

            routes.MapRoute(
               name: "forgotpassword",
               url: "forgotpassword",
               defaults: new { controller = "User", action = "forgotpassword" }
           );

            routes.MapRoute(
               name: "Howweworks",
               url: "HowWeWorks",
               defaults: new { controller = "Home", action = "HowWeWorks" }
           );

            routes.MapRoute(
                name: "Signup-Patient",
                url: "Signup-Patient",
                defaults: new { controller = "User", action = "PatientSignUp" }
            );

            routes.MapRoute(
                name: "Signup-Doctor",
                url: "Signup-Doctor",
                defaults: new { controller = "User", action = "DoctorSignUp" }
            );

            routes.MapRoute(
                name: "Login",
                url: "Login",
                defaults: new { controller = "User", action = "Login" }
            );

            routes.MapRoute(
               name: "PrivacyPolicy",
               url: "PrivacyPolicy",
               defaults: new { controller = "Home", action = "PrivacyPolicy" }
           );

            routes.MapRoute(
               name: "Contact",
               url: "Contact",
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
                url: "Tags",
                defaults: new { controller = "admin", action = "managetag" }
            );

            routes.MapRoute(
                name: "AdminReport",
                url: "Report",
                defaults: new { controller = "admin", action = "Report" }
            );

            routes.MapRoute(
                name: "AdminRegistrationRequests",
                url: "Doctors",
                defaults: new { controller = "User", action = "ManageDoctors" }
            );
            
            

            routes.MapRoute(
                name: "Changepassword",
                url: "Changepassword",
                defaults: new { controller = "User", action = "Changepassword" }
            );

            routes.MapRoute(
                name: "InviteFriend",
                url: "InviteFriend",
                defaults: new { controller = "Patients", action = "InviteFriend" }
            );

            routes.MapRoute(
               name: "AskDoctor",
               url: "AskDoctor",
               defaults: new { controller = "Patients", action = "AskDoctor" }
           );

            routes.MapRoute(
               name: "Myactivity",
               url: "Myactivity",
               defaults: new { controller = "Patients", action = "Myactivity" }
           );

            routes.MapRoute(
               name: "FindDoctors",
               url: "FindDoctors",
               defaults: new { controller = "Home", action = "FindDoctors" }
           );

            routes.MapRoute(
                name: "Questions2",
                url: "Questions/{QuestionId}",
                defaults: new { controller = "Questions", action = "DoctorQuestionDetails" },
                constraints: new { QuestionId = @"\d+" }
            );

            routes.MapRoute(
                name: "Questions1",
                url: "Questions/{filter}",
                defaults: new { controller = "Questions", action = "DoctorQuestionList", filter = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "assignquestion",
                url: "AssignQuestion/{questionid}",
                defaults: new {controller="admin",action="assignquestion" },
                constraints: new { QuestionId = @"\d+" }
                );
            

            routes.MapRoute(
                name: "Answers1",
                url: "Answers",
                defaults: new { controller = "feed", action = "feed" }
            );

            routes.MapRoute(
                name: "Answers2",
                url: "Doctor-Answers",
                defaults: new { controller = "feed", action = "doctorfeed" }
            );

            routes.MapRoute(
                name: "DoctorProfile",
                url: "DoctorProfile",
                defaults: new { controller = "Doctors", action = "DoctorProfile" }
                
            );

            routes.MapRoute(
               name: "DoctorProfile1",
               url: "DoctorProfile/{UserId}",
               defaults: new { controller = "Doctors", action = "DoctorProfile" }
               
           );

            routes.MapRoute(
                name: "Answers3",
                url: "Answers/{QuestionId}",
                defaults: new { controller = "Patients", action = "PatientQuestionDetails" },
                constraints: new { QuestionId = @"\d+" }
            );

            routes.MapRoute(
               name: "similarQuestions",
               url: "similarQuestions/{question}",
               defaults: new { controller = "Patients", action = "similarQuestions" },
               constraints: new { question = @"([a-z]+-?)+" }
           );

            routes.MapRoute(
               name: "AssignDoctor",
               url: "AssignDoctor/{QuestionId}/{AssignDoctorIds}",
               defaults: new { controller = "admin", action = "AssignDoctor" }

           );

            routes.MapRoute(
               name: "RemoveAssignDoctorToQuetion",
               url: "RemoveAssignDoctorToQuetion/{userid}/{questionId}",
               defaults: new { controller = "admin", action = "RemoveAssignDoctorToQuetion" }

           );

            routes.MapRoute(
               name: "Default",
               url: "{controller}/{action}/{id}",
               defaults: new { controller = "Home", action = "Home", id = UrlParameter.Optional }
           );
        }
    }
}