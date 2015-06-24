namespace MiraiConsultMVC.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class package
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public package()
        {
            userspackages = new HashSet<userspackage>();
        }

        public int packageid { get; set; }

        [Required]
        [StringLength(50)]
        public string packagename { get; set; }

        public double? costpermonth { get; set; }

        public bool bookappointment { get; set; }

        public bool viewappointment { get; set; }

        public bool blockcalendar { get; set; }

        public bool cancelappointment { get; set; }

        public bool delayappointment { get; set; }

        public bool followupreminder { get; set; }

        public bool assistantlogin { get; set; }

        public bool searchpatients { get; set; }

        public bool patienthistory { get; set; }

        public bool publicquestions { get; set; }

        public bool econsultquestions { get; set; }

        public bool miraihealthapp { get; set; }

        [StringLength(50)]
        public string createdby { get; set; }

        public DateTime? createdon { get; set; }

        [StringLength(50)]
        public string modifiedby { get; set; }

        public DateTime? modifiedon { get; set; }

        public bool? managehealthinformation { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<userspackage> userspackages { get; set; }
    }
}
