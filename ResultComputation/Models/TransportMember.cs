using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace LightWay.Models
{
    public class TransportMember
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Student Number")]
        public string StudentNumber { get; set; }

        [Required]
        [Display(Name = "Student Name")]
        public string StudentName { get; set; }

        [Required]
        [Display(Name = "Class")]
        public string Class { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone Number")]
        //[StringLength(23, MinimumLength = 11)]
        public string PhoneNumber { get; set; }

        [Required]
        [Display(Name = "Route")]
        public string Route { get; set; }

        [Required]
        [Display(Name = "Driver")]
        public string Driver { get; set; }

        [HiddenInput]
        public string RegisteredBy { get; set; }

        [HiddenInput]
        public string DateRecorded { get; set; }
    }
}
