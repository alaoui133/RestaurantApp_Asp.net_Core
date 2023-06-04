using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Restaurant.Models
{
    
    public class Category
    {
        [Key]
        //[Column("Categ_ID")] in database
        public int Id { get; set; }

        //******************************************************
        [Required(ErrorMessage ="Category Name is required")]
        [StringLength(maximumLength:50,MinimumLength=3,ErrorMessage ="3-50")]
        [Display(Name ="Category Name")]
        public string Name { get; set; }
        //******************************************************
        [Display(Name="Display Order")]
        [Required(ErrorMessage = "DisplayOrder is required")]
        [Range(1,100,ErrorMessage = "Display Order must be between 1-100")]
        public int DisplayOrder { get; set; }
    }
}
