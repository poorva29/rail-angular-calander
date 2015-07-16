using System;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IUserService" in both code and config file together.
    [ServiceContract]
    public interface IUserService
    {
        #region GET
        
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/getFeedList?lastQuestionNo={lastQuestionNo}&RecordSize={RecordSize}&UserId={UserId}&myFeed={myFeed}&isEncrypt={isEncrypt}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        String getFeedList(string lastQuestionNo, string RecordSize, int UserId, int myFeed, Boolean isEncrypt);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/getQuestionList?RecordSize={RecordSize}&Flag={Flag}&lastQuestionNo={lastQuestionNo}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        String getQuestionList(string RecordSize, int Flag,int lastQuestionNo);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/fillDegreeDropdowns", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        String fillDegreeDropdowns();

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/getAllTags", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        String getAllTags();

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/get_allTagsWithCountOfAnsweredQuestions", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        String get_allTagsWithCountOfAnsweredQuestions();

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/addNewTags?newTag={newTag}&questionid={questionid}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        String addNewTags(string newTag, string questionid);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/removeTags?tag={tag}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        String removeTags(string tag);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/fillDropdowns", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        String fillDropdowns();

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/getAllCountryList", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        String getAllCountryList();

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/getStatebyCountry?countryid={countryid}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        String getStatebyCountry(string countryid);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/getCitybyState?stateId={stateId}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        String getCitybyState(int stateId);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/getCountryStateCityData?countryId={countryId}&stateId={stateId}&cityId={cityId}&locationId={locationId}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        String getCountryStateCityData(int countryId,int stateId,int cityId,int locationId);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/sendReminderNotiForPrepaidAppointment", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        void sendReminderNotiForPrepaidAppointment();

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/cancelUnpaidAppointment", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        void cancelUnpaidAppointment();

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/PaymentConfirmationOn2Days", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        void PaymentConfirmationOn2Days();
        #endregion GET

        #region POST
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/searchQuestions", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        String searchQuestion(string searchstr);
         
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/getLocationsbyCity", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        String getLocationsbyCity(string cityId);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/getDoctorList", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        String getDoctorListByCriteria(string cityid, string locationid, string specialityOrName, Boolean isEncrypt);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/IncrementAppointmentHitCnt", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        String IncrementAppointmentHitCnt(string userid);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/thanktodoctor", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        String thankdoctor(string userid, string answerid, string lastname, string emailid, string mobileno, string questiontext, string thanxcount);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/endorsetodoctor", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        String endorsedoctoranswer(string userid, string answerid, string lastname, string Email, string answerreplyedby, string mobileno, string questiontext, string endorsecount);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/get_AuthenticateData", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        void get_AuthenticateData(string username, string password);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/insertQuestion", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        void insertQuestion(int userId, string questionText);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/UpdateProfile", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        void UpdateProfile(int userId, string firstName, string LastName, string mobile, string Email, string IsNewEmail);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/SaveDoctorAnswer", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        void SaveDoctorAnswer(int questionId, int userId, string title, string answer);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/UpdateQuestionTags", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        void UpdateQuestionTags(string Addedtags, string DeletedTags, string QuestionID);
        #endregion POST
    }
}
