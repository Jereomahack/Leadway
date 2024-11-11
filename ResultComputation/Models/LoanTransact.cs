using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LightWay.Models
{
    public class LoanTransact
    {
        [Key]
        public int Id { get; set; }
       
        [Display(Name = "Staff Number")]
        public string StaffNumber { get; set; }
       
        [Display(Name = "Staff Name")]
        public string StaffName { get; set; }
       
        [Display(Name = "Amount")]
        public decimal AmountCollected { get; set; }

        public string  Reason { get; set; }

        public decimal Payable { get; set; }

        [HiddenInput]
        public string CreatedBy { get; set; }

        [HiddenInput]
        [Column(TypeName = "datetime2")]
        [DataType(DataType.Date)]
        [Display(Name = "Date Recorded")]
        public DateTime DateRecorded { get; set; }

    }
}