using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace LightWay.Models
{
    public class TransactionInventory
    {
        [Key]
        public int Id { get; set; }
        
        [Display(Name = "Product")]
        public string Product { get; set; }

        [Display(Name = "Quantity")]
        public decimal Quantity { get; set; }

        [Display(Name = "Vendor")]
        public string  Vendor { get; set; }

        [Display(Name = "Total Price")]
        public decimal TotalPrice { get; set; }

        [HiddenInput]
        public string Term { get; set; }

        [HiddenInput]
        [Display(Name = "Month")]
        public string  Month { get; set; }

        [HiddenInput]
        [Display(Name = "Year")]
        public string Year { get; set; }

        [HiddenInput]
        public string EnteredBy { get; set; }
    }
}
