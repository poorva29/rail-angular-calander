namespace MiraiConsultMVC.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class user
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public user()
        {
            hospital_admin = new HashSet<hospital_admin>();
        }

        public int userid { get; set; }

        [Required]
        [StringLength(50)]
        public string firstname { get; set; }

        [Required]
        [StringLength(50)]
        public string lastname { get; set; }

        [Required]
        [StringLength(50)]
        public string email { get; set; }

        [Required]
        [StringLength(15)]
        public string mobileno { get; set; }

        [StringLength(50)]
        public string secondaryemail { get; set; }

        [StringLength(15)]
        public string secondarymobileno { get; set; }

        public int? gender { get; set; }

        public DateTime? dateofbirth { get; set; }

        public int? totalexperience { get; set; }

        [StringLength(100)]
        public string username { get; set; }

        [StringLength(100)]
        public string password { get; set; }

        public int? countryid { get; set; }

        public int? stateid { get; set; }

        public int? cityid { get; set; }

        public int? locationid { get; set; }

        public int? height { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? weight { get; set; }

        public DateTime? registrationdate { get; set; }

        public int? status { get; set; }

        public int? usertype { get; set; }

        [StringLength(256)]
        public string photopath { get; set; }

        public bool? isemailverified { get; set; }

        public bool? ismobileverified { get; set; }

        public int? pincode { get; set; }

        [StringLength(500)]
        public string address { get; set; }

        [StringLength(20)]
        public string registrationnumber { get; set; }

        public int? registrationcouncil { get; set; }

        [StringLength(200)]
        public string aboutme { get; set; }

        public int? appointmentbuttonhits { get; set; }

        public DateTime? registrationvalidity { get; set; }

        public bool? apptsmsnotification { get; set; }

        public int? created_by { get; set; }

        [StringLength(100)]
        public string patientcity { get; set; }

        [StringLength(100)]
        public string cgname { get; set; }

        [StringLength(100)]
        public string cgemail { get; set; }

        [StringLength(15)]
        public string cgmobileno { get; set; }

        public bool? isremonedayprior { get; set; }

        public bool? isremmorningonapptday { get; set; }

        public bool? isremoncancellationoflasttwo { get; set; }

        public DateTime? joiningdate { get; set; }

        public int? reference_id { get; set; }

        public bool? isdocconnectuser { get; set; }

        public int? existing_doctor { get; set; }

        public int? existing_patient { get; set; }

        public int? existing_assitant { get; set; }

        public int? askmiraiappointmentcount { get; set; }

        public int? askmirai_userid { get; set; }

        public bool? isgoogleaccount { get; set; }

        public bool? isfacebookaccount { get; set; }

        public int? countrycode1 { get; set; }

        public int? countrycode2 { get; set; }

        public int? cgcountrycode { get; set; }

        [StringLength(200)]
        public string photourl { get; set; }

        [StringLength(150)]
        public string name_seo { get; set; }

        public int? modified_by { get; set; }

        public DateTime? modified_on { get; set; }

        [StringLength(200)]
        public string deviceid { get; set; }

        public bool? autoDownload { get; set; }

        public double? econsultfee { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<hospital_admin> hospital_admin { get; set; }
    }
}
