using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Restaurant.Models.ViewModels
{
    public class MenuItemViewModel
    {
       
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = String.Empty;
        public string Description { get; set; }
        [Display(Name = "MenuItem Image")]
        public string? ImageUrl { get; set; }
        [Range(0, 100, ErrorMessage = "Price should be btween 1-100")]
        public double Price { get; set; }
        [Display(Name = "Category")]
        public int CategoryId { get; set; }
     
        [Display(Name = "Food Type")]
        public int FoodTypeId { get; set; }
      

    }
}
