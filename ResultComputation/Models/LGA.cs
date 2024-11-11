using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LightWay.Models
{
    public class LGA
    {
        [Key]
        public int Id { get; set; }
        public int StateId { get; set; }

        [Display(Name ="LGA")]
        public string LGAName { get; set; }
    }
}