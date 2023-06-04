using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Models
{
    public class MenuItem
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage ="Name is required")]
        public string Name { get; set; }=String.Empty;
        public string Description { get; set; }
        [Display(Name = "MenuItem Image")]
       
        public string? ImageUrl { get; set; }
       // [Range(0,100,ErrorMessage ="Price should be btween 1-100")]
        public double Price { get; set; }
        [Display(Name ="Category")]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
        [Display(Name="Food Type")]
        public int FoodTypeId { get; set; }
        [ForeignKey("FoodTypeId")]
        public FoodType FoodType { get; set; }

    }
}
