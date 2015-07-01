namespace MiraiConsultMVC{
    public enum UserStatus
    {
        Pending, //0
        Registered,//1
        Approved,//2
        Rejected//3
    }
    public enum QuestionStatus
    {
        Pending,//0
        Approved,//1
        Rejected//2
    }
    public enum UserType
    {
        SuperAdmin, //0
        Doctor,//1
        Patient,//2
        Assistent//3
    }
    public enum Privileges
    {
        doctorquestiondetails, //doctorquestiondetails.aspx
        questionlist, //questionlist.aspx
        askdoctor,//askdoctor.aspx
        patientprofile,//patientprofile.aspx
        patientfeed, //patientfeed.aspx
        changepassword,//changepassword.aspx
        Invitefriend,//invitefriends.aspx
        Myactivity,//myactivity.aspx
        doctorprofile,//doctorprofile.aspx
        manageDoctor, //ManageDoctors.aspx
        managetags, //ManageTags
        reports, //reports.aspx
        assignQuestion,//assignquestion.aspx
        DoctorFeed//DoctorFeed
  }
    public enum NotificationStatus
    {
        Insert, //0
        Update,//1
        Reminder,//2
        Cancel//3
    }
}