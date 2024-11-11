using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LightWay.Models
{
    public class Grades
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Grade Name")]
        public string GradeName { get; set; }

        [Required]
        [Display(Name = "Grade Point")]
        public decimal GradePoint { get; set; }

        [Required]
        [Display(Name = "Mark From")]
        public decimal MarkFrom { get; set; }

        [Required]
        [Display(Name = "Mark Upto")]
        public decimal MarkUpto { get; set; }

        [Required]
        [Display(Name = "Note")]
        public string Note { get; set; }
    }
}