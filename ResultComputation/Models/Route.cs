using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightWay.Models
{
    public class Route
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name ="Route Name")]
        public string RouteName { get; set; }

        [Required]
        [Display(Name = "Route Address")]
        public string RouteAddress { get; set; }
    }
}
