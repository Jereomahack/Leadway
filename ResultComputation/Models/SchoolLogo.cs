using System.ComponentModel.DataAnnotations;

namespace LightWay.Models
{
    public class SchoolLogo
    {
        [Key]
        public int Id { get; set; }
        public byte[] logo { get; set; }
    }
}