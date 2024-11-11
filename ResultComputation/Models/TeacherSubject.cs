using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LightWay.Models
{
    public class TeacherSubject
    {
        [Key]
        public int Id { get; set; }

        
        public string TeacherNum { get; set; }

       
        public string TeacherName { get; set; }

       
        public string  Subject  { get; set; }

       
        public string  Class{ get; set; }

        public string EmailAddress { get; set; }

        [HiddenInput]
        public string CreatedBy { get; set; }

        [HiddenInput]
        [Column(TypeName = "datetime2")]
        [DataType(DataType.Date)]
        [Display(Name = "Date Recorded")]
        public DateTime DateRecorded { get; set; }


        
    }
}