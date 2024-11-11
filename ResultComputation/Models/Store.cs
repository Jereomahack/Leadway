using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightWay.Models
{
    public class Store
    {
        [Key]
        public int Id { get; set; }
        public string Product { get; set; }
        public decimal  Quantity { get; set; }
        public string StoredBy { get; set; }
       
        [DataType(DataType.Date)]
        public DateTime DateStored { get; set; }
    }
}
