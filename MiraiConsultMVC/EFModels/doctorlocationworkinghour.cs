namespace MiraiConsultMVC.EFModels
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class doctorlocationworkinghour
    {
        [Key]
        public int doclocworkinghoursid { get; set; }

        public int doclocationid { get; set; }

        [Required]
        [StringLength(4)]
        public string fromtime { get; set; }

        [Required]
        [StringLength(4)]
        public string totime { get; set; }

        public bool? Monday { get; set; }

        public bool? Tuesday { get; set; }

        public bool? Wednesday { get; set; }

        public bool? Thursday { get; set; }

        public bool? Friday { get; set; }

        public bool? Saturday { get; set; }

        public bool? Sunday { get; set; }

        public int? created_by { get; set; }

        public DateTime? created_on { get; set; }

        public int? modified_by { get; set; }

        public DateTime? modified_on { get; set; }

        public int fromtimeFor(DayOfWeek day)
        {
            switch(day)
            {
                case DayOfWeek.Monday:
                    return Monday.GetValueOrDefault() ? Int32.Parse(fromtime) : -1;
                case DayOfWeek.Tuesday:
                    return Tuesday.GetValueOrDefault() ? Int32.Parse(fromtime) : -1;
                case DayOfWeek.Wednesday:
                    return Wednesday.GetValueOrDefault() ? Int32.Parse(fromtime) : -1;
                case DayOfWeek.Thursday:
                    return Thursday.GetValueOrDefault() ? Int32.Parse(fromtime) : -1;
                case DayOfWeek.Friday:
                    return Friday.GetValueOrDefault() ? Int32.Parse(fromtime) : -1;
                case DayOfWeek.Saturday:
                    return Saturday.GetValueOrDefault() ? Int32.Parse(fromtime) : -1;
                case DayOfWeek.Sunday:
                    return Sunday.GetValueOrDefault() ? Int32.Parse(fromtime) : -1;
                default:
                    return -1;
            }
        }

        public int totimeFor(DayOfWeek day)
        {
            switch (day)
            {
                case DayOfWeek.Monday:
                    return Monday.GetValueOrDefault() ? Int32.Parse(totime) : -1;
                case DayOfWeek.Tuesday:
                    return Tuesday.GetValueOrDefault() ? Int32.Parse(totime) : -1;
                case DayOfWeek.Wednesday:
                    return Wednesday.GetValueOrDefault() ? Int32.Parse(totime) : -1;
                case DayOfWeek.Thursday:
                    return Thursday.GetValueOrDefault() ? Int32.Parse(totime) : -1;
                case DayOfWeek.Friday:
                    return Friday.GetValueOrDefault() ? Int32.Parse(totime) : -1;
                case DayOfWeek.Saturday:
                    return Saturday.GetValueOrDefault() ? Int32.Parse(totime) : -1;
                case DayOfWeek.Sunday:
                    return Sunday.GetValueOrDefault() ? Int32.Parse(totime) : -1;
                default:
                    return -1;
            }
        }
    }
}
