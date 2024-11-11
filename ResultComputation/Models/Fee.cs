using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LightWay.Models
{
    public class Fee
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string PaymentType { get; set; }

        [Display(Name = "Payment Type")]
        public string PaymentCat { get; set; }

        [Display(Name = "Sub")]
        public string PaymentSubCat { get; set; }

        [Display(Name = "Reg No")]
        public string StudentReg { get; set; }

        [Display(Name = "Student Name")]
        public string StudentName { get; set; }
        public string TransactionRef { get; set; }

        [Display(Name = "Accademic Term")]
        public string AccTerm { get; set; }

        [Display(Name = "Session")]
        public string AccSession { get; set; }

        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        public DateTime PaymentDate { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        [DisplayFormat(DataFormatString = "{0:N}")]
        public decimal amount { get; set; }

        public string PaymentStatues { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        public string OPR { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public string Payref { get; set; }
    }
}