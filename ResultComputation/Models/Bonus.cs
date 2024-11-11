using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LightWay.Models
{
    public class Bonus
    {
        [Key]
        public int Id { get; set; }


        [Display(Name = "Staff Number")]
        public string StaffNumber { get; set; }


        [Display(Name = "Staff Name")]
        public string StaffName { get; set; }


        [Display(Name = "Bonus Type")]
        public string BonusType { get; set; }


        [Display(Name = "Amount")]
        public decimal BonusAmount { get; set; }

        [Display(Name = "Session")]
        public string Session { get; set; }

        [Display(Name = "Term")]
        public string Term { get; set; }

        [Display(Name = "Month")]
        public string Month { get; set; }

        public string Statues { get; set; }

        public string AllowDeduction { get; set; }

        [HiddenInput]
        public string EnteredBy { get; set; }

        [HiddenInput]
        [Display(Name = " Date")]
        [DataType(DataType.Date)]
        public DateTime DateCreated { get; set; }
    }
}