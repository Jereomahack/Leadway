using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LightWay.Models
{
    public class Teachers
    {
        [Key]
        public int Id { get; set; }


        [Display(Name = "Staff Number")]
        public string StaffNumber { get; set; }


        [Display(Name = "Full Name")]
        public string FullName { get; set; }


        [Display(Name = "Gender ")]
        public string Gender { get; set; }

        [Required]
        [Display(Name = "Teacher Role")]
        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Residential Address")]
        public string ResidentialAddress { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone Number")]
        [StringLength(23, MinimumLength = 11)]
        public string PhoneNumber { get; set; }

        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }

        [Column(TypeName = "datetime2")]
        [DataType(DataType.Date)]
        [Display(Name = "Date of Appointment")]
        public DateTime DOA { get; set; }

        [Required]
        [Display(Name = "Basic Salary")]
        [DisplayFormat(DataFormatString = "{0:N}")]
        public decimal BasicSalary { get; set; }

        [Display(Name = "Upload Passport")]
        public byte[] Passport { get; set; }

        [HiddenInput]
        public string RegisteredBy { get; set; }

        [HiddenInput]
        [Column(TypeName = "datetime2")]
        [DataType(DataType.Date)]
        [Display(Name = "Date Recorded")]
        public DateTime DateRecorded { get; set; }

        [Required]
        public string UserName { get; set; }
    }
}