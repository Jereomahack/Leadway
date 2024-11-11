using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightWay.Models
{
    public class Transactions
    {
        [Key]
        public int Id { get; set; }
        public string Number { get; set; }
        public string Name { get; set; }
        public string year { get; set; }
        public string Terms { get; set; }
        public string Month { get; set; }
        public decimal AmountPaid { get; set; }
        public decimal Balance { get; set; }
        public string  DatePaid { get; set; }
        public string EnteredBy { get; set; }

    }
}
