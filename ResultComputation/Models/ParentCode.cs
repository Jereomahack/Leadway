using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace LightWay.Models
{
    public class ParentCode
    {
        [Key]
        public int Id { get; set; }

        //[Display(Name = "Student Number")]
        //public string StudentNumber { get; set; }

        //[HiddenInput]
        //[Display(Name ="Parent/Guardian Name")]
        //public string ParentName { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone Number")]
        //[StringLength(23, MinimumLength = 11)]
        public string PhoneNumber { get; set; }

        [Display(Name = "Enter Code")]
        public string codes { get; set; }
    }
}
