using System.ComponentModel.DataAnnotations;

namespace LightWay.Models
{
    public class PaymentCategory
    {
        [Key]
        public int Id { get; set; }

        public string CategoryName { get; set; }

        public string ClassName { get; set; }
    }
}