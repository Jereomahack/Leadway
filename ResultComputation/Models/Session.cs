using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LightWay.Models
{
    public class Session
    {
        [Key]
        public int Id { get; set; }


        [Display(Name = "Academic Year")] 
        public string AcademicYear { get; set; }

    }
}