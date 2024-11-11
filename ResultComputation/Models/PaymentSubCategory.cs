using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LightWay.Models
{
    public class PaymentSubCategory
    {
        [Key]
        public int Id { get; set; }

        public string  PaymentCat { get; set; }

        public string SubCatName { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        [DisplayFormat(DataFormatString = "{0:N}")]
        public decimal Amount { get; set; }
    }
}