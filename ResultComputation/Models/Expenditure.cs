using System;
using System.ComponentModel.DataAnnotations;

namespace LightWay.Models
{
    public class Expenditure
    {
        [Key]
        public int Id { get; set; }


        [Display(Name = "Requested Amt.")]
        public decimal AmountRequested { get; set; }

        [Display(Name = "Approved Amt.")]
        public decimal AmountReleased { get; set; }


        [Display(Name = "Reason")]
        public string Purpose { get; set; }


        [Display(Name = "To")]
        public string Vissibility { get; set; }

        [Display(Name = "Statues")]
        public string Statues { get; set; }


        public string Term { get; set; }

        public string Session { get; set; }

        [Display(Name = "Requested By")]
        public string RequestedBy { get; set; }

        [Display(Name = "ApprovedBy")]
        public string ApprovedBy { get; set; }

        [Display(Name = "Expenditure Type")]
        public string ExpenditureType { get; set; }

        [Display(Name = "Reason for Decline")]
        public string DeclineReason { get; set; }


        [Display(Name = "Date Recorded")]
        [DataType(DataType.Date)]
        public DateTime DateRecorded { get; set; }

        [Display(Name = "Date Requested")]
        [DataType(DataType.Date)]
        public DateTime DateRequested { get; set; }

    }
}
