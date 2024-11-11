using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightWay.Models
{
   public  class SchoolSubjects
    {
        [Key]
        public int Id { get; set; }

        public string Subject { get; set; }

    }
}
