using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace LightWay.Models
{
    public class Department
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Department")]
        public string DepartmentName { get; set; }

        [Required]
        [Display(Name = "HOD")]
        public string HOD { get; set; }

        [HiddenInput]
        public string EnteredBy { get; set; }

        [HiddenInput]
        public string DateRecorded { get; set; }
    }
}
