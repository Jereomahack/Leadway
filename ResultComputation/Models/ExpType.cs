using System.ComponentModel.DataAnnotations;

namespace LightWay.Models
{
    public class ExpType
    {
        [Key]
        public int Id { get; set; }
        public string ExpensesName { get; set; }
    }
}