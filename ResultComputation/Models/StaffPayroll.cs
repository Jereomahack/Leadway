using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace LightWay.Models
{
    public class StaffPayroll
    {
        [Key]
        public int Id { get; set; }

        
        [Display(Name = "Staff Number")]
        public string StaffNumber { get; set; }

       
        [Display(Name = "Staff Name")]
        public string StaffName { get; set; }

        [Required]
        [Display(Name = "Session")]
        public string Session { get; set; }

        [Required]
        [Display(Name = "Term")]
        public string Term { get; set; }

        [Required]
        [Display(Name = "Month")]
        public string Month { get; set; }

        [Required]
        [Display(Name = "Amount")]
        public decimal Amount { get; set; }
        
        [Display(Name = "Amount Paid After Deduction")]
        public decimal AmountPaidAfterDeduction { get; set; }

        [Display(Name = "Bonus")]
        public decimal BonusAmount { get; set; }

        [Display(Name = "Bonus Deducted")]
        public decimal AmountDeducted { get; set; }


        [Display(Name = "OutStandings")]
        public decimal Outstandings { get; set; }

        [HiddenInput]
        public string EnteredBy { get; set; }

        [HiddenInput]
        [DataType(DataType.Date)]
        [Display(Name ="Date of Payment")]
        public DateTime DateofPayment { get; set; }

    }
}
