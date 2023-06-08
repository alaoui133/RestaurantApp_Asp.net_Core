using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Restaurant.DAL.Interfaces;
using Restaurant.Models;
using System.Security.Claims;

namespace RestaurantUI.Pages.Customer.Cart
{
    [Authorize]
    public class SummaryModel : PageModel
    {
        public readonly IUnitOfWork _UnitOfWork;

        [BindProperty]
        public Order Order { get; set; }
        public IEnumerable<ShoppingCart>? ShoppingCartList { get; set; }

        public SummaryModel( IUnitOfWork unitOfWork)
        {
            _UnitOfWork = unitOfWork;
            Order = new Order();
        }
        public async void OnGet()
        {
            var UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (UserId != null)
            {
                ShoppingCartList = await _UnitOfWork.ShoppingCartRepo.GetAll(
                    
                    filter: u =>u.UserId == UserId ,
                    includeProperties: "ApplicationUser,MenuItem"
                    );

                foreach (var cart in ShoppingCartList)
                {
                    Order.OrderTotal += (cart.MenuItem.Price * cart.Count);
                }

                ApplicationUser AppUser = ShoppingCartList.First().ApplicationUser;
                Order.PickUpName = AppUser.FirstName +" "+ AppUser.LastName;
                Order.PhoneNumber = AppUser.PhoneNumber;
                Order.OrderDate = DateTime.Now;
                Order.PickUpTime = DateTime.Now;
            }
        }
    }
}
