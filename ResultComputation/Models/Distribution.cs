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
    public class Distribution
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Receiver")]
        public string Receiver { get; set; }

        [Required]
        [Display(Name = "Department")]
        public string Department { get; set; }

        [Required]
        [Display(Name = "Item Name")]
        public string ItemName { get; set; }

        [Required]
        [Display(Name = "Quantity")]
        [DisplayFormat(DataFormatString = "{0:n0}")]
        public decimal Quantity { get; set; }

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
