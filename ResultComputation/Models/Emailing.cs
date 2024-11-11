using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightWay.Models
{
    public class Emailing
    {
        [Key]
        public int Id { get; set; }

        [Display(Name ="Sender")]
        public string  Sender { get; set; }

        [Display(Name = "Subject")]
        public string Subject { get; set; }

        [Display(Name = "Statues")]
        public string Statues { get; set; }
       
        public string Term { get; set; }
       
        [Display(Name = "Sent Date")]
        public string DateSent { get; set; }

        [Display(Name = "Message")]
        [DataType(DataType.MultilineText)]
        public string Message { get; set; }

        [Display(Name = "Reciever")]
        public string Reciever { get; set; }
    }
}
