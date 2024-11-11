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
    public class Inventory
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Vendor Name")]
        public string VendorName { get; set; }

        [Required]
        [Display(Name = "Product")]
        public string Product { get; set; }

        [Required]
        [Display(Name = "Unit Price")]
        public decimal UnitPrice { get; set; }

        [Required]
        [Display(Name = "Quantity")]
        public decimal Quantity { get; set; }

        [Required]
        [Display(Name = "Total Price")]
        public decimal TotalPrice { get; set; }

        [Required]
        [Display(Name = "Payment")]
        public string Payment { get; set; }

        public string Month { get; set; }
        public string AcademicYear { get; set; }
        public string Term { get; set; }

        [HiddenInput]
        public string EnteredBy { get; set; }

        [HiddenInput]
        [Column(TypeName = "datetime2")]
        [DataType(DataType.Date)]
        [Display(Name = "Date Recorded")]
        public DateTime DateRecorded { get; set; }

    }
}
