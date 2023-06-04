using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Models
{
    public class FoodType
    {
        [Key]
        [Display(Name ="Food Type Id")]
        public int FoodTypeId { get; set ; }

        [Required]
        [Display(Name ="Food Type Name")]
        [StringLength(30,MinimumLength =3)]
        public string FoodTypeName { get; set; }
    }
}
