using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LightWay.Models
{
    public class CodeGenerator
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Enter Code")]
        public string codes { get; set; }

        [Display(Name = "Parent Name")]
        public string Parent { get; set; }
    }
}