using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace LightWay.Models
{
    public class ClassList
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [Display(Name = "Class Name")]
        public string ClassName { get; set; }
      
        [Display(Name = "Staff Number")]
        public string StaffNumber { get; set; }

        [Required]
        [Display(Name = "Teacher Name")]
        public string FormMaster{ get; set; }
    }
}