using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace LightWay.Models
{
    public class Hostel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Hostel Name")]
        public string HostelName { get; set; }

        [Required]
        [Display(Name = "Hostel Class")]
        public string Class { get; set; }


        [Required]
        [Display(Name = "Patron/Matron")]
        public string HostelAttendant { get; set; }

        [Required]
        [Display(Name = "Patron/Matron Phone")]
        [DataType(DataType.PhoneNumber)]
        public string HostelAttendantPhone { get; set; }

        [HiddenInput]
        public string RegisteredBy { get; set; }

        [HiddenInput]
        public string DateRecorded { get; set; }
    }
}
