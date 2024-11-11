using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ResultComputation.Models
{
    public class SchoolClass
    {
        [Key]
        public int Id { get; set; }

        public int ClassCategoryId { get; set; }

        public string ClassName { get; set; }
    }
}