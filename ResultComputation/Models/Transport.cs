using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace LightWay.Models
{
    public class Transport
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Route Name")]
        public string RouteName { get; set; }

        [Required]
        [Display(Name = "Route Fare")]
        public string RouteFare { get; set; }

        [Required]
        [Display(Name = "Vehicle Number")]
        public string VehicleNumber { get; set; }

        [Required]
        [Display(Name = "Driver Name")]
        public string DriverName { get; set; }


        [Required]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [HiddenInput]
        public string RegisteredBy { get; set; }

        [HiddenInput]
        public string  DateRecorded { get; set; }
    }
}
