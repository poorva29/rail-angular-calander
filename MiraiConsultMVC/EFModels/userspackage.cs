namespace MiraiConsultMVC.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class userspackage
    {
        [Key]
        public int userpackageid { get; set; }

        public int userid { get; set; }

        public int? packageid { get; set; }

        public DateTime? fromdate { get; set; }

        public DateTime? todate { get; set; }

        public bool? istrial { get; set; }

        public int? createdby { get; set; }

        public DateTime? createdon { get; set; }

        public int? modifiedby { get; set; }

        public DateTime? modifiedon { get; set; }

        public bool? IsActive { get; set; }

        public int? packagesubscriptionmonthsid { get; set; }

        public double? amounttopay { get; set; }

        public double? miscellaneousamount { get; set; }

        public double? totalpackageamount { get; set; }

        public virtual package package { get; set; }
    }
}
