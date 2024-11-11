using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LightWay.Models
{
    public class DeductionType
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Deduction Type ")]
        public string DeductionTypeName { get; set; }

    }
}