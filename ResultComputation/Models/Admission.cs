using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LightWay.Models
{
    public class Admission
    {
        // Section A: pupil
        [Key]
        public int Id { get; set; }

        [Display(Name = "FORM NO:")]
        public string FormNo { get; set; }

        //[Required]
        [Display(Name = "Surname")]
        public string Surname { get; set; }

        //[Required]
        [Display(Name = "Firstname")]
        public string Firstname { get; set; }

        //[Required]
        [Display(Name = "Middlename")]
        public string Middlename { get; set; }

        [Display(Name = "Gender")]
        public string Gender { get; set; }

        //[Required]
        [DataType(DataType.Date)]
        public DateTime DateTime { get; set; }

        [Display(Name = "Date Of Birth")]
        public DateTime DOB { get; set; }

        [Display(Name = "Passport")]
        public byte[] Passport { get; set; }

        [Display(Name = "Allergic")]
        public string Allergic { get; set; }

        [Required]
        [Display(Name = "Place of Birth")]
        public string PoB { get; set; }

        [Required]
        [Display(Name = "State of Origin")]
        public string StateOfOrigin { get; set; }

        [Display(Name = "L.G.A")]
        public string Lga { get; set; }

        [Display(Name = "Nationality")]
        public string Nationality { get; set; }

        [Display(Name = "Religion")]
        public string Religion { get; set; }

        [Display(Name = "Language")]
        public string Language { get; set; }

        [Display(Name = "Previous School Attended")]
        public string Psa { get; set; }


        // Section B : Details of parents and gaurdians (father)
        //[Display(Name ="Title and Initials")]
        //public string Mtitle { get; set; }


        //[Display(Name ="Surname")]
        //public string MsurnName { get; set; }

        //[Display(Name ="Relationships")]
        //public string Mrelationships { get; set; }

        //[Display(Name ="Telephone Number")]
        //[DataType(DataType.PhoneNumber)]
        //public string Mtelephone { get; set; }

        //[Display(Name = "Address")]
        //[DataType(DataType.MultilineText)]
        //public string Maddress { get; set; }

        //[Display(Name = "GSM Number")]
        //[DataType(DataType.PhoneNumber)]
        //public string Mgsmnumber { get; set; }


        //[Display(Name = "Email")]
        //[DataType(DataType.EmailAddress)]
        //public string MemailAddress { get; set; }

        //[Display(Name = "Occupation")]
        //public string Moccupation { get; set; }

        //[Display(Name = "Employer")]
        //public string Memployer { get; set; }


        // Section B : Details of parents and gaurdians (mother)
        [Display(Name = "Title and Initials")]
        public string Ftitle { get; set; }


        [Display(Name = "Surname")]
        public string FsurnName { get; set; }

        [Display(Name = "Relationships")]
        public string Frelationships { get; set; }

        //[Display(Name = " Telephone Number")]
        //[DataType(DataType.PhoneNumber)]
        //public string Ftelephone { get; set; }

        [Display(Name = " Address")]
        [DataType(DataType.MultilineText)]
        public string Faddress { get; set; }

        [Display(Name = "GSM Number")]
        [DataType(DataType.PhoneNumber)]
        public string Fgsmnumber { get; set; }


        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string FemailAddress { get; set; }

        [Display(Name = "Occupation")]
        public string Foccupation { get; set; }

        [Display(Name = "Employer")]
        public string Femployer { get; set; }


        [Display(Name = "Discovery")]
        public string Discovery { get; set; }

        public string Status { get; set; }

        //office use

        //[Display(Name = "Date Received")]
        //[Column(TypeName = "datetime2")]
        //[DataType(DataType.Date)]
        //public DateTime DateRecieved { get; set; }

        //[Display(Name = "Date of Birth")]
        //[Column(TypeName = "datetime2")]
        //[DataType(DataType.Date)]
        //public DateTime Dateofbirth { get; set; }

        //[Display(Name = "Date of Assessment")]
        //[Column(TypeName = "datetime2")]
        //[DataType(DataType.Date)]
        //public DateTime DateAssessment { get; set; }

        //[Display(Name = "Year Group")]
        //[Column(TypeName = "datetime2")]
        //[DataType(DataType.Date)]
        //public DateTime YearGroup{ get; set; }

        //[Display(Name = "Place Offered")]
        //public string Place { get; set; }

        //[Display(Name = "Starting Date")]
        //[Column(TypeName = "datetime2")]
        //[DataType(DataType.Date)]
        //public DateTime Startingdate { get; set; }

    }
}