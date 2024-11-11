using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LightWay.Models
{
    public class HOD
    {
        [Key]
        public int Id { get; set; }

        [Display(Name ="Staff Number")]
        public string StaffNumber { get; set; }

        [Display(Name = "Category")]
        public string Category { get; set; }
    }
}