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
    public class Products
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; }


        [Required]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Description")]
        //[StringLength(500, MinimumLength = 11)]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Unit Price")]
        public decimal UnitPrice { get; set; }
       
        [Display(Name = "No. In a Pack")]
        [DisplayFormat(DataFormatString = "{0:n0}")]
        public decimal NoInaPack { get; set; }
     
        [HiddenInput]
        public string RegisteredBy { get; set; }

        [HiddenInput]
        public string DateRecorded { get; set; }

    }
}
