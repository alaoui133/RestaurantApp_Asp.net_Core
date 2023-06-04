using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Models
{
    public class ShoppingCart
    {
        [Key]
        public int Id { get; set; }
        public int MenuItemId { get; set; }
        [ForeignKey("MenuItemId")]
        public MenuItem MenuItem { get; set; }
        [Range(0, 100, ErrorMessage ="Count must be less than 100")]
        [Required]
        public int Count { get; set; }  
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser ApplicationUser { get; set; }
    }
}
