namespace MiraiConsultMVC.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("hospital")]
    public partial class hospital
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public hospital()
        {
            hospital_admin = new HashSet<hospital_admin>();
        }

        public int id { get; set; }

        [Required]
        [StringLength(500)]
        public string name { get; set; }

        public int countryid { get; set; }

        public int stateid { get; set; }

        public int cityid { get; set; }

        [Required]
        [StringLength(1000)]
        public string address1 { get; set; }

        [Required]
        [StringLength(15)]
        public string mobileno { get; set; }

        public DateTime created_date { get; set; }

        public int location_id { get; set; }

        public int? countrycode { get; set; }

        public virtual city city { get; set; }

        public virtual country country { get; set; }

        public virtual state state { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<hospital_admin> hospital_admin { get; set; }

        public virtual location location { get; set; }
    }
}
