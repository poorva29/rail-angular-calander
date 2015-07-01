namespace MiraiConsultMVC.EFModels
{
    using System.Data.Entity;

    public partial class EFModelContext : DbContext
    {
        public EFModelContext()
            : base("name=EFModelContext")
        {
        }

        public virtual DbSet<answer> answers { get; set; }
        public virtual DbSet<appointment> appointments { get; set; }
        public virtual DbSet<appointmenttype> appointmenttypes { get; set; }
        public virtual DbSet<cca_payment> cca_payments { get; set; }
        public virtual DbSet<city> cities { get; set; }
        public virtual DbSet<country> countries { get; set; }
        public virtual DbSet<degree> degrees { get; set; }
        public virtual DbSet<delayedappointmentsrecord> delayedappointmentsrecords { get; set; }
        public virtual DbSet<doctoragree> doctoragrees { get; set; }
        public virtual DbSet<doctorlocation> doctorlocations { get; set; }
        public virtual DbSet<doctorlocationworkinghour> doctorlocationworkinghours { get; set; }
        public virtual DbSet<doctorquestion> doctorquestions { get; set; }
        public virtual DbSet<doctorsdetail> doctorsdetails { get; set; }
        public virtual DbSet<doctorspeciality> doctorspecialities { get; set; }
        public virtual DbSet<healthinformation> healthinformations { get; set; }
        public virtual DbSet<hospital> hospitals { get; set; }
        public virtual DbSet<hospital_admin> hospital_admin { get; set; }
        public virtual DbSet<location> locations { get; set; }
        public virtual DbSet<message_read_log> message_read_log { get; set; }
        public virtual DbSet<mobilelog> mobilelogs { get; set; }
        public virtual DbSet<package> packages { get; set; }
        public virtual DbSet<packagesubscriptionmonth> packagesubscriptionmonths { get; set; }
        public virtual DbSet<patientthank> patientthanks { get; set; }
        public virtual DbSet<question> questions { get; set; }
        public virtual DbSet<questiontag> questiontags { get; set; }
        public virtual DbSet<registrationcouncil> registrationcouncils { get; set; }
        public virtual DbSet<setting> settings { get; set; }
        public virtual DbSet<speciality> specialities { get; set; }
        public virtual DbSet<state> states { get; set; }
        public virtual DbSet<tag> tags { get; set; }
        public virtual DbSet<tbl_appointment_details> tbl_appointment_details { get; set; }
        public virtual DbSet<tbl_chat_dialogs> tbl_chat_dialogs { get; set; }
        public virtual DbSet<tbl_doctor_patient> tbl_doctor_patient { get; set; }
        public virtual DbSet<tbl_messages> tbl_messages { get; set; }
        public virtual DbSet<tbl_pre_registration> tbl_pre_registration { get; set; }
        public virtual DbSet<tbl_user_device> tbl_user_device { get; set; }
        public virtual DbSet<unregpatient> unregpatients { get; set; }
        public virtual DbSet<user> users { get; set; }
        public virtual DbSet<userspackage> userspackages { get; set; }
        public virtual DbSet<doctorqualification> doctorqualifications { get; set; }
        public virtual DbSet<patientfollowuphistory> patientfollowuphistories { get; set; }
        public virtual DbSet<tbl_doctor_patient_backup> tbl_doctor_patient_backup { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<cca_payment>()
                .Property(e => e.order_id)
                .IsUnicode(false);

            modelBuilder.Entity<cca_payment>()
                .Property(e => e.currency)
                .IsUnicode(false);

            modelBuilder.Entity<cca_payment>()
                .Property(e => e.card_name)
                .IsUnicode(false);

            modelBuilder.Entity<cca_payment>()
                .Property(e => e.payment_mode)
                .IsUnicode(false);

            modelBuilder.Entity<cca_payment>()
                .Property(e => e.bank_ref_no)
                .IsUnicode(false);

            modelBuilder.Entity<cca_payment>()
                .Property(e => e.tracking_id)
                .IsUnicode(false);

            modelBuilder.Entity<cca_payment>()
                .Property(e => e.order_status)
                .IsUnicode(false);

            modelBuilder.Entity<cca_payment>()
                .Property(e => e.failure_message)
                .IsUnicode(false);

            modelBuilder.Entity<cca_payment>()
                .Property(e => e.status_message)
                .IsUnicode(false);

            modelBuilder.Entity<cca_payment>()
                .Property(e => e.billing_name)
                .IsUnicode(false);

            modelBuilder.Entity<cca_payment>()
                .Property(e => e.billing_address)
                .IsUnicode(false);

            modelBuilder.Entity<cca_payment>()
                .Property(e => e.billing_city)
                .IsUnicode(false);

            modelBuilder.Entity<cca_payment>()
                .Property(e => e.billing_state)
                .IsUnicode(false);

            modelBuilder.Entity<cca_payment>()
                .Property(e => e.billing_zip)
                .IsUnicode(false);

            modelBuilder.Entity<cca_payment>()
                .Property(e => e.billing_country)
                .IsUnicode(false);

            modelBuilder.Entity<cca_payment>()
                .Property(e => e.billing_tel)
                .IsUnicode(false);

            modelBuilder.Entity<cca_payment>()
                .Property(e => e.billing_email)
                .IsUnicode(false);

            modelBuilder.Entity<cca_payment>()
                .Property(e => e.delivery_name)
                .IsUnicode(false);

            modelBuilder.Entity<cca_payment>()
                .Property(e => e.delivery_address)
                .IsUnicode(false);

            modelBuilder.Entity<cca_payment>()
                .Property(e => e.delivery_city)
                .IsUnicode(false);

            modelBuilder.Entity<cca_payment>()
                .Property(e => e.delivery_state)
                .IsUnicode(false);

            modelBuilder.Entity<cca_payment>()
                .Property(e => e.delivery_zip)
                .IsUnicode(false);

            modelBuilder.Entity<cca_payment>()
                .Property(e => e.delivery_country)
                .IsUnicode(false);

            modelBuilder.Entity<cca_payment>()
                .Property(e => e.delivery_tel)
                .IsUnicode(false);

            modelBuilder.Entity<cca_payment>()
                .Property(e => e.status_code)
                .IsUnicode(false);

            modelBuilder.Entity<appointment>()
                .Property(e => e.subject)
                .IsUnicode(false);

            modelBuilder.Entity<appointment>()
                .Property(e => e.description)
                .IsUnicode(false);

            modelBuilder.Entity<appointment>()
                .Property(e => e.patientname)
                .IsUnicode(false);

            modelBuilder.Entity<appointment>()
                .Property(e => e.patientemail)
                .IsUnicode(false);

            modelBuilder.Entity<appointment>()
                .Property(e => e.patientmobile)
                .IsUnicode(false);

            modelBuilder.Entity<appointment>()
                .Property(e => e.comments)
                .IsUnicode(false);

            modelBuilder.Entity<appointment>()
                .Property(e => e.feedback)
                .IsUnicode(false);

            modelBuilder.Entity<appointment>()
                .Property(e => e.note)
                .IsUnicode(false);

            modelBuilder.Entity<appointment>()
                .Property(e => e.instruction)
                .IsUnicode(false);

            modelBuilder.Entity<appointment>()
                .Property(e => e.deviceid)
                .IsUnicode(false);

            modelBuilder.Entity<appointment>()
                .Property(e => e.prepayamount)
                .HasPrecision(10, 2);

            modelBuilder.Entity<appointment>()
                .Property(e => e.txncode)
                .IsUnicode(false);

            modelBuilder.Entity<appointmenttype>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<city>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<city>()
                .HasMany(e => e.hospitals)
                .WithRequired(e => e.city)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<country>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<country>()
                .Property(e => e.countrycode)
                .IsUnicode(false);

            modelBuilder.Entity<country>()
                .HasMany(e => e.hospitals)
                .WithRequired(e => e.country)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<degree>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<delayedappointmentsrecord>()
                .Property(e => e.delayedby)
                .IsUnicode(false);

            modelBuilder.Entity<delayedappointmentsrecord>()
                .Property(e => e.starttime)
                .IsUnicode(false);

            modelBuilder.Entity<delayedappointmentsrecord>()
                .Property(e => e.endtime)
                .IsUnicode(false);

            modelBuilder.Entity<delayedappointmentsrecord>()
                .Property(e => e.delay)
                .IsUnicode(false);

            modelBuilder.Entity<doctorlocation>()
                .Property(e => e.clinicname)
                .IsUnicode(false);

            modelBuilder.Entity<doctorlocation>()
                .Property(e => e.address)
                .IsUnicode(false);

            modelBuilder.Entity<doctorlocation>()
                .Property(e => e.telephone)
                .IsUnicode(false);

            modelBuilder.Entity<doctorlocation>()
                .Property(e => e.hospitalname)
                .IsUnicode(false);

            modelBuilder.Entity<doctorlocationworkinghour>()
                .Property(e => e.fromtime)
                .IsUnicode(false);

            modelBuilder.Entity<doctorlocationworkinghour>()
                .Property(e => e.totime)
                .IsUnicode(false);

            modelBuilder.Entity<doctorsdetail>()
                .Property(e => e.details)
                .IsUnicode(false);

            modelBuilder.Entity<doctorsdetail>()
                .Property(e => e.certification)
                .IsUnicode(false);

            modelBuilder.Entity<doctorsdetail>()
                .Property(e => e.society)
                .IsUnicode(false);

            modelBuilder.Entity<healthinformation>()
                .Property(e => e.healthmessagesubject)
                .IsUnicode(false);

            modelBuilder.Entity<healthinformation>()
                .Property(e => e.healthmessagedetails)
                .IsUnicode(false);

            modelBuilder.Entity<healthinformation>()
                .Property(e => e.createdby)
                .IsUnicode(false);

            modelBuilder.Entity<healthinformation>()
                .Property(e => e.modiifiedby)
                .IsUnicode(false);

            modelBuilder.Entity<hospital>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<hospital>()
                .Property(e => e.address1)
                .IsUnicode(false);

            modelBuilder.Entity<hospital>()
                .Property(e => e.mobileno)
                .IsUnicode(false);

            modelBuilder.Entity<hospital>()
                .HasMany(e => e.hospital_admin)
                .WithRequired(e => e.hospital)
                .HasForeignKey(e => e.hospital_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<location>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<location>()
                .HasMany(e => e.hospitals)
                .WithRequired(e => e.location)
                .HasForeignKey(e => e.location_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<mobilelog>()
                .Property(e => e.filename)
                .IsUnicode(false);

            modelBuilder.Entity<mobilelog>()
                .Property(e => e.filextension)
                .IsUnicode(false);

            modelBuilder.Entity<mobilelog>()
                .Property(e => e.Comment)
                .IsUnicode(false);

            modelBuilder.Entity<mobilelog>()
                .Property(e => e.username)
                .IsUnicode(false);

            modelBuilder.Entity<mobilelog>()
                .Property(e => e.logtype)
                .IsUnicode(false);

            modelBuilder.Entity<package>()
                .Property(e => e.packagename)
                .IsUnicode(false);

            modelBuilder.Entity<package>()
                .Property(e => e.createdby)
                .IsUnicode(false);

            modelBuilder.Entity<package>()
                .Property(e => e.modifiedby)
                .IsUnicode(false);

            modelBuilder.Entity<question>()
                .Property(e => e.questiontext)
                .IsUnicode(false);

            modelBuilder.Entity<question>()
                .Property(e => e.question_seo)
                .IsUnicode(false);

            modelBuilder.Entity<question>()
                .Property(e => e.deviceid)
                .IsUnicode(false);

            modelBuilder.Entity<question>()
                .Property(e => e.dialogueid)
                .IsUnicode(false);

            modelBuilder.Entity<registrationcouncil>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<setting>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<setting>()
                .Property(e => e.value)
                .IsUnicode(false);

            modelBuilder.Entity<speciality>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<state>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<state>()
                .HasMany(e => e.hospitals)
                .WithRequired(e => e.state)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tag>()
                .Property(e => e.tagname)
                .IsUnicode(false);

            modelBuilder.Entity<tag>()
                .Property(e => e.tag_seo)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_appointment_details>()
                .Property(e => e.notes)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_appointment_details>()
                .Property(e => e.instruction)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_doctor_patient>()
                .Property(e => e.deviceid)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_pre_registration>()
                .Property(e => e.firstname)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_pre_registration>()
                .Property(e => e.lastname)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_pre_registration>()
                .Property(e => e.email)
                .IsUnicode(false);

            modelBuilder.Entity<unregpatient>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<unregpatient>()
                .Property(e => e.email)
                .IsUnicode(false);

            modelBuilder.Entity<unregpatient>()
                .Property(e => e.mobileno)
                .IsUnicode(false);

            modelBuilder.Entity<unregpatient>()
                .Property(e => e.deviceid)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.firstname)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.lastname)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.email)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.mobileno)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.secondaryemail)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.secondarymobileno)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.username)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.password)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.weight)
                .HasPrecision(6, 2);

            modelBuilder.Entity<user>()
                .Property(e => e.photopath)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.address)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.registrationnumber)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.aboutme)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.patientcity)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.cgname)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.cgemail)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.cgmobileno)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.photourl)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.name_seo)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.deviceid)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .HasMany(e => e.hospital_admin)
                .WithRequired(e => e.user)
                .HasForeignKey(e => e.assistant_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<doctorqualification>()
                .Property(e => e.university)
                .IsUnicode(false);

            modelBuilder.Entity<doctorqualification>()
                .Property(e => e.otherdegree)
                .IsUnicode(false);

            modelBuilder.Entity<patientfollowuphistory>()
                .Property(e => e.patienttype)
                .IsUnicode(false);

            modelBuilder.Entity<patientfollowuphistory>()
                .Property(e => e.note)
                .IsUnicode(false);

            modelBuilder.Entity<patientfollowuphistory>()
                .Property(e => e.instruction)
                .IsUnicode(false);

            modelBuilder.Entity<patientfollowuphistory>()
                .Property(e => e.deviceid)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_doctor_patient_backup>()
                .Property(e => e.deviceid)
                .IsUnicode(false);
        }
    }
}
