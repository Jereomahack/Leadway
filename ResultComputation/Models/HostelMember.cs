using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace LightWay.Models
{
    public class HostelMember
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
        [Display(Name = "Hostel Name")]
        public string HostelName { get; set; }


        [Required]
        [Display(Name = "Hostel Class")]
        public string Class { get; set; }

        [Required]
        [Display(Name = "Hostel Fee")]
        public decimal HostelFee { get; set; }

        [Required]
        [Display(Name = "Gender")]
        public string Gender { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone Number")]
        //[StringLength(23, MinimumLength = 11)]
        public string PhoneNumber { get; set; }

        [HiddenInput]
        public string RegisteredBy { get; set; }

        [HiddenInput]
        public string DateRecorded { get; set; }
    }
}
