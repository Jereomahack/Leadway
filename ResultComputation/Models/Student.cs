using LightWay.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LightWay.Models
{
    public class Student
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Student Number")]
        public string StudentNumber { get; set; }

        [Display(Name = "Surname ")]
        public string Surname { get; set; }

        [Display(Name = "Other Name ")]
        public string OtherName { get; set; }

        [Display(Name = "Gender ")]
        public string Gender { get; set; }

        [Display(Name = "Category ")]
        public string Category { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        public DateTime DOB { get; set; }

        [Display(Name = "Class")]
        public string Class { get; set; }

        public string Session { get; set; }

        [Display(Name = "Parent/Guardian Name ")]
        public string GuardianName { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Parent/Guardian Address")]
        //[StringLength(500, MinimumLength = 11)]
        public string GuardianAddress { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone Number")]
        //[StringLength(23, MinimumLength = 11)]
        public string PhoneNumber { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Alternative Phone")]
        //[StringLength(23, MinimumLength = 11)]
        public string AlternativePhoneNumber { get; set; }

        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }

        [Display(Name = "Check Result")]
        public string CheckResult { get; set; }

        [Display(Name = "Weight")]
        public string Weight { get; set; }

        [Display(Name = "Height")]
        public string Height { get; set; }

        [Display(Name = "General Satisfaction")]
        public string GeneralSactisfaaction { get; set; }

        [Display(Name = "Transistion")]
        public string Transistion { get; set; }

        [Display(Name = "Upload Passport")]
        public byte[] Passport { get; set; }

        [HiddenInput]
        public string RegisteredBy { get; set; }

        [HiddenInput]

        [Column(TypeName = "datetime2")]
        [DataType(DataType.Date)]
        [Display(Name = "Date Recorded")]
        public DateTime DateRecorded { get; set; }



    }
}