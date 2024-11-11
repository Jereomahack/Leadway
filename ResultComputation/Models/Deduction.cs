using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LightWay.Models
{
    public class Deduction
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Staff Number")]
        public string StaffNumber { get; set; }

        [Display(Name = "Staff Name")]
        public string StaffName { get; set; }

        [Required]
        [Display(Name = "Deduction Type")]
        public string DeductionType { get; set; }

        [Required]
        [Display(Name = "Deduction Amount")]
        public decimal DeductionAmount { get; set; }

        [Required]
        [Display(Name = "Session")]
        public string Session { get; set; }

        [Required]
        [Display(Name = "Term")]
        public string Term { get; set; }

        [Required]
        [Display(Name = "Month")]
        public string Month { get; set; }

        public string Statues { get; set; }

        public string AllowDeduction { get; set; }

        [HiddenInput]
        public string EnteredBy { get; set; }

        [HiddenInput]
        [Display(Name = "Deduction Date")]
        [DataType(DataType.Date)]
        public DateTime DateofDdeduction { get; set; }
    }
}