using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Restaurant.Models
{
    public class OrderDetails
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        [ForeignKey("OrderId")]
        [ValidateNever]
        public Order Order { get; set; }


        [Required]
        public int MenuItemId { get; set; }
        [ForeignKey("MenuItemId")]
        public MenuItem MenuItem { get; set; }

        [Required]
        public int Count { get; set; }

        [Required]
        public double Price { get; set; }

        public string? Name { get; set; }
    }
}
