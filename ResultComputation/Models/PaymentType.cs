using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightWay.Models
{
    public class PaymentType
    {
        [Key]
        public int Id { get; set; }

        [Display(Name ="Payment Type")]
        public string PaymentTypeName { get; set; }
    }
}
