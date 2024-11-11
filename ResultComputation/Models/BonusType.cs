using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LightWay.Models
{
    public class BonusType
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Bonus Type ")]
        public string BonusTypeName { get; set; }

    }
}