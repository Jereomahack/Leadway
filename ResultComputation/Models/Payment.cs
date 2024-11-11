using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace LightWay.Models
{
    public class Payment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Student Number")]
        public string StudentNum { get; set; }

        [Required]
        [Display(Name = "Student Name")]
        public string StudentName { get; set; }


        [Required]
        [Display(Name = "Class")]
        public string Class { get; set; }

        [Required]
        [Display(Name = "Term")]
        public string Term { get; set; }

        [Required]
        [Display(Name = "Session")]
        public string Session { get; set; }

        [Required]
        [Display(Name = "Amount")]
        public decimal Amount { get; set; }

        [Required]
        [Display(Name = "Balance")]
        public decimal Balance { get; set; }

        [Required]
        [Display(Name = "Payment For")]
        public string PaymentFor { get; set; }

        [HiddenInput]
        public string EnteredBy { get; set; }

        [HiddenInput]
        [Column(TypeName = "datetime2")]
        [DataType(DataType.Date)]
        [Display(Name = "Date Recorded")]
        public DateTime DateRecorded { get; set; }


    }
}
