using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LightWay.Models
{
    public class score
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Student Number")]
        public string StudentNumber { get; set; }


        [Display(Name = "Surname")]
        public string Surname { get; set; }


        [Display(Name = "Other Name")]
        public string OtherName { get; set; }


        [Display(Name = "Class")]
        public string Class { get; set; }

        [Display(Name = "Academic Year")]
        public string AcademicYear { get; set; }

        [HiddenInput]
        public string HODVerify { get; set; }

        [Display(Name = "Term")]
        public string Term { get; set; }

        [Display(Name = "Subject")]
        public string Subject { get; set; }

        [Display(Name = "Cognitive")]
        [DisplayFormat(DataFormatString = "{0:n0}")]
        public decimal FirstCA { get; set; }

        [Display(Name = "Affective")]
        [DisplayFormat(DataFormatString = "{0:n0}")]
        public decimal SecondCA { get; set; }

        [Display(Name = "Psychomoto")]
        [DisplayFormat(DataFormatString = "{0:n0}")]
        public decimal Psychomoto { get; set; }

        [Display(Name = "EXAM")]
        [DisplayFormat(DataFormatString = "{0:n0}")]
        public decimal Exam { get; set; }


        [Display(Name = "TOTAL ")]
        [DisplayFormat(DataFormatString = "{0:n0}")]
        public decimal Total { get; set; }

        [Display(Name = "Obtainable Score")]
        [DisplayFormat(DataFormatString = "{0:n0}")]
        public decimal ObtainableScore { get; set; }

        [Display(Name = "AVERAGE")]
        public decimal AVERAGE { get; set; }




        [Display(Name = "HIGHEST")]
        [DisplayFormat(DataFormatString = "{0:n0}")]
        public decimal HIGHEST { get; set; }


        [Display(Name = "LOWEST")]
        [DisplayFormat(DataFormatString = "{0:n0}")]
        public decimal LOWEST { get; set; }


        [Display(Name = "GRADE")]
        public string GRADE { get; set; }


        [Display(Name = "REMARK")]
        public string REMARK { get; set; }

        [HiddenInput]
        public string PreparedBy { get; set; }

        [HiddenInput]
        [Column(TypeName = "datetime2")]
        [DataType(DataType.Date)]
        [Display(Name = "Date Recorded")]
        public DateTime DateRecorded { get; set; }



    }
}