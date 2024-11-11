using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LightWay.Models
{
    public class Term
    {
        [Key]
        public int Id { get; set; }

        public string TermName { get; set; }
    }
}
