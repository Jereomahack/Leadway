using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LightWay.Models
{
    public class StudentOldNumbers
    {
        [Key]
        public int Id { get; set; }

        public string StudentName { get; set; }

        public string  StudentNumber { get; set; }
    }
}