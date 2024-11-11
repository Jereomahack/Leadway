using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ResultComputation.Models
{
    public class ClassCategory
    {
        [Key]
        public string Id { get; set; }

        [Display(Name ="Class Category")]
        public string CategoryName { get; set; }
    }
}