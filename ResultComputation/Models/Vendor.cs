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
    public class Vendor
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Full Name")]
        public string VendorName { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone Number")]
        [StringLength(23, MinimumLength = 11)]
        public string PhoneNumber { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Product Descriptions")]
        public string ProductDescription { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Address")]
        //[StringLength(500, MinimumLength = 11)]
        public string VendorAddress { get; set; }

        [HiddenInput]
        public string RegisteredBy { get; set; }

        [HiddenInput]
        public string DateRecorded { get; set; }

    }
}
