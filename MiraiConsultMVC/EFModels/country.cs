namespace MiraiConsultMVC.EFModels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("country")]
    public partial class country
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public country()
        {
            hospitals = new HashSet<hospital>();
        }

        public int countryid { get; set; }

        [Required]
        [StringLength(100)]
        public string name { get; set; }

        [StringLength(20)]
        public string countrycode { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<hospital> hospitals { get; set; }
    }
}
