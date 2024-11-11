using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LightWay.Models
{
    public class StudentTransistion
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
       
        [Display(Name = "Class")]
        public string Class { get; set; }

        [Display(Name = "Class")]
        [DataType(DataType.MultilineText)]
        public string Reason { get; set; }

        [Column(TypeName = "datetime2")]
        [DataType(DataType.Date)]
        [Display(Name = "Date Left")]
        public DateTime DateLeft { get; set; }

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