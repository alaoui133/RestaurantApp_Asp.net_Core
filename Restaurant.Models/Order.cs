using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser ApplicationUser { get; set; }

        [Required]
        [Display(Name = "Order Date")]   
        public DateTime OrderDate { get; set; }

        [Required]
        [Display(Name = "Order Total")]
        public double OrderTotal { get; set; }

        [Required]
        [Display(Name = "Order Date")]
        public DateTime PickUpDate { get; set; }
        [Required]
        [Display(Name = "Order Time")]
        public DateTime PickUpTime { get; set; }

        public string Status { get; set; }  

        public string? Comments { get; set; }   

        public string? sessionId { get; set; } //from Stripe

        public string? PaymentIntentId { get; set; }    // from stripe

        [Required]
        [Display(Name = "Order Name")]
        public string? PickUpName { get; set; }

        [Required]
        [Display(Name = "Order Number")]
        public string? PhoneNumber { get; set; }




    }
}
