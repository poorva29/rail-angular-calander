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
               name: "topics",
               url: "topics",
               defaults: new { controller = "Home", action = "topics" }
               );

            routes.MapRoute(
               name: "reset-password",
               url: "reset-password",
               defaults: new { controller = "user", action = "resetpassword" }
               );

            routes.MapRoute(
                name: "PatientProfile",
                url: "patient-profile",
                defaults: new { controller = "Patients", action = "PatientProfile" }
                );

            routes.MapRoute(
               name: "forgotpassword",
               url: "forgot-password",
               defaults: new { controller = "User", action = "forgotpassword" }
           );

            routes.MapRoute(
               name: "Howweworks",
               url: "how-we-works",
               defaults: new { controller = "Home", action = "HowWeWorks" }
           );

            routes.MapRoute(
                name: "Signup-Patient",
                url: "patient-signup",
                defaults: new { controller = "User", action = "PatientSignUp" }
            );

            routes.MapRoute(
                name: "Signup-Doctor",
                url: "doctor-signup",
                defaults: new { controller = "User", action = "DoctorSignUp" }
            );

            routes.MapRoute(
                name: "Login",
                url: "login",
                defaults: new { controller = "User", action = "Login" }
            );
            routes.MapRoute(
               name: "PreRegistrationUser",
               url: "question/{QuestionId}",
               defaults: new { controller = "Questions", action = "PreRegistrationUser" },
               constraints: new { 
                   QuestionId = @"\d+"
                    }
               );
            routes.MapRoute(
               name: "PrivacyPolicy",
               url: "privacy-policy",
               defaults: new { controller = "Home", action = "PrivacyPolicy" }
           );

            routes.MapRoute(
               name: "Contact",
               url: "contact-us",
               defaults: new { controller = "Home", action = "Contact" }
           );

            routes.MapRoute(
               name: "terms-Of-Use",
               url: "terms-of-use",
               defaults: new { controller = "Home", action = "TermsOfUse" }
           );

            routes.MapRoute(
                name: "AdminQuestionList",
                url: "admin-questions/{questionsType}",
                defaults: new { controller = "admin", action = "QuestionList", questionsType = UrlParameter.Optional }
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
                url: "change-password",
                defaults: new { controller = "User", action = "Changepassword" }
            );

            routes.MapRoute(
                name: "InviteFriend",
                url: "invite-friends",
                defaults: new { controller = "Patients", action = "InviteFriend" }
            );

            routes.MapRoute(
               name: "AskDoctor",
               url: "ask-doctor",
               defaults: new { controller = "Patients", action = "AskDoctor" }
           );

            routes.MapRoute(
               name: "Myactivity",
               url: "my-activity",
               defaults: new { controller = "Patients", action = "Myactivity" }
           );

            routes.MapRoute(
               name: "FindDoctors",
               url: "find-doctors",
               defaults: new { controller = "Home", action = "FindDoctors" }
           );

            routes.MapRoute(
                name: "Questions3",
                url: "questions/{QuestionId}",
                defaults: new { controller = "Questions", action = "DoctorQuestionDetails" },
                constraints: new { QuestionId = @"\d+" }
            );

            routes.MapRoute(
                name: "Questions2",
                url: "question/{seoQuestionText}",
                defaults: new { controller = "Questions", action = "DoctorQuestions" }
            );

            
            routes.MapRoute(
                name: "Questions1",
                url: "questions/{questionsType}",
                defaults: new { controller = "Questions", action = "DoctorQuestionList", questionsType = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "assignquestion1",
                url: "assign-question/{questionid}",
                defaults: new { controller = "admin", action = "assignquestion" },
                constraints: new { QuestionId = @"\d+" }
                );

            routes.MapRoute(
                name: "assignquestion",
                url: "assign-question/{seoQuestionText}",
                defaults: new {controller="admin",action="assignquestions" }
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
                url: "doctor-profile",
                defaults: new { controller = "Doctors", action = "DoctorProfile" }
                
            );

            routes.MapRoute(
               name: "DoctorProfile1",
               url: "doctor-profile/{*UserId}",
               defaults: new { controller = "Doctors", action = "DoctorProfile" }
               
           );
            
            routes.MapRoute(
              name: "DoctorProfile2",
              url: "doctor/{seo_name}",
              defaults: new { controller = "Doctors", action = "doctorsPublicProfile" }
           );


            routes.MapRoute(
                name: "Answers4",
                url: "answers/{QuestionId}",
                defaults: new { controller = "Patients", action = "PatientQuestionDetails"},
                constraints: new { QuestionId = @"\d+" }
            );

            routes.MapRoute(
                name: "Answers3",
                url: "answers/{*questiontext}",
                defaults: new { controller = "Patients", action = "PatientQuestionDetail", questiontext = UrlParameter.Optional }
            );


            routes.MapRoute(
                name: "similarQuestions",
                url: "similar-questions",
                defaults: new { controller = "Patients", action = "similarQuestions" }
                );

            routes.MapRoute(
               name: "similarQuestions1",
               url: "similar-questions/{*question}",
               defaults: new { controller = "Patients", action = "similarQuestions" }
           );

            routes.MapRoute(
               name: "AssignDoctor",
               url: "assign-doctor/{QuestionId}/{AssignDoctorIds}",
               defaults: new { controller = "admin", action = "AssignDoctor" }

           );

            routes.MapRoute(
               name: "RemoveAssignDoctorToQuetion",
               url: "removeassigndoctortoquetion/{userid}/{questionId}",
               defaults: new { controller = "admin", action = "RemoveAssignDoctorToQuetion" }

           );

            routes.MapRoute(
               name: "topics1",
               url: "topics/{tagseo}",
               defaults: new { controller = "home", action = "topicdetails" }
              
               );

            routes.MapRoute(
                name: "RejectQuestionByQuestionID",
                url: "RejectQuestionByQuestionID/{qusetionID}",
                defaults: new { controller = "admin", action = "RejectQuestionByQuestionID" }
           );

            routes.MapRoute(
              name: "seo-question",
              url: "{seoQuestionText}",
              defaults: new { controller = "Patients", action = "seoQuestionDetails" }
              );

            routes.MapRoute(
               name: "Default",
               url: "{controller}/{action}/{id}",
               defaults: new { controller = "Home", action = "Home", id = UrlParameter.Optional }
           );
        }
    }
}