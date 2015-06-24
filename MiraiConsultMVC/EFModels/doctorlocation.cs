namespace MiraiConsultMVC.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class doctorlocation
    {
        public int doctorlocationid { get; set; }

        public int doctorid { get; set; }

        public int countryid { get; set; }

        public int stateid { get; set; }

        public int cityid { get; set; }

        public int locationid { get; set; }

        [StringLength(100)]
        public string clinicname { get; set; }

        [StringLength(250)]
        public string address { get; set; }

        [StringLength(15)]
        public string telephone { get; set; }

        public int? timeslot { get; set; }

        public double? consultationfees { get; set; }

        public bool? isprimary { get; set; }

        public int? hospitalid { get; set; }

        [StringLength(100)]
        public string hospitalname { get; set; }

        public int? created_by { get; set; }

        public DateTime? created_on { get; set; }

        public int? modified_by { get; set; }

        public DateTime? modified_on { get; set; }
    }
}
