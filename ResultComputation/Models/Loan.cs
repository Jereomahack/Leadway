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
    public class Loan
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Staff Number")]
        public string StaffNumber { get; set; }

        [Required]
        [Display(Name = "Staff Name")]
        public string StaffName { get; set; }

        [Required]
        [Display(Name = "Amount")]
        public decimal AmountCollected { get; set; }
       
        public decimal AmountLeft { get; set; }

        [Required]
        [Display(Name = "Payable")]
        public decimal AmountPayable { get; set; }

        [Required]
        [Display(Name = "Amount Paid")]
        public decimal AmountPaid { get; set; }

        [Required]
        [Display(Name = "Reason")]
        [DataType(DataType.MultilineText)]
        public string Reason { get; set; }

        public string Statues { get; set; }

        public string AllowDeduction { get; set; }

        [HiddenInput]
        public string CreatedBy { get; set; }

        [HiddenInput]
        [Column(TypeName = "datetime2")]
        [DataType(DataType.Date)]
        [Display(Name = "Date Recorded")]
        public DateTime DateRecorded { get; set; }

    }
}
