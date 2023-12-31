using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NToastNotify;
using Restaurant.DAL.Interfaces;
using Restaurant.Models;
using Restaurant.Utilitiy;
using System.Security.Claims;

namespace RestaurantUI.Pages.Customer.Cart
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        public IEnumerable<ShoppingCart> ShoppingCartList { get; set; }
        public double CartTotal { get; set; }
        private readonly IToastNotification _notify;
        public bool IsClicked=false;

        public IndexModel(IUnitOfWork unitOfWork , IToastNotification notify)
        {
            _unitOfWork = unitOfWork;
            CartTotal = 0;
            _notify = notify;
        }
        public async Task OnGet()
        {
            // User => the current User
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (userId != null)
            {
                ShoppingCartList = await _unitOfWork.ShoppingCartRepo.GetAll(
                    filter: u => u.UserId == userId,
                    includeProperties: "MenuItem,MenuItem.Category,MenuItem.FoodType"
                    );
                foreach (ShoppingCart item in ShoppingCartList)
                {
                    CartTotal += (item.Count * item.MenuItem.Price);
                }
            }
            //if (id==0)
            //{
            //    _notify.AddErrorToastMessage("Shopping Cart is Empty!! ");
            //}
        
        }

        public async Task<IActionResult> OnPostPlus(int cartID)
        {
           ShoppingCart cart = await _unitOfWork.ShoppingCartRepo.GetById(
              c=>c.Id == cartID);
             _unitOfWork.ShoppingCartRepo.IncrementCount(cart,1);
            return RedirectToPage("/Customer/Cart/Index");

        }

        public async Task<IActionResult> OnPostMinus(int cartID)
        {
            ShoppingCart cart = await _unitOfWork.ShoppingCartRepo.GetById(
                c=>c.Id == cartID);
            if (cart.Count >1)
            {
                _unitOfWork.ShoppingCartRepo.DecrementCount(cart, 1);
            }
            else
            {
                string userId = cart.UserId;
                _unitOfWork.ShoppingCartRepo.remove(cart);
                await _unitOfWork.Save();
                // after remove item from shoppinCart
                int count = (await _unitOfWork.ShoppingCartRepo.GetAll(
                  o=>o.UserId == userId)).ToList().Count;
                HttpContext.Session.SetInt32(ConstRoleDef.SessionCart, count);
            }
            
            return RedirectToPage("/Customer/Cart/Index");

        }

        public async Task<IActionResult> OnPostRemove(int cartID)
        {
            ShoppingCart cart = await _unitOfWork.ShoppingCartRepo.GetById(
                c => c.Id == cartID);
            string userId = cart.UserId;
            _unitOfWork.ShoppingCartRepo.remove(cart);
            if (await _unitOfWork.Save())
            {
                // after remove item from shoppinCart
                int count = (await _unitOfWork.ShoppingCartRepo.GetAll(
                  o => o.UserId == userId)).ToList().Count;
                HttpContext.Session.SetInt32(ConstRoleDef.SessionCart, count);
                return RedirectToPage("/Customer/Cart/Index");
            }
            else
            {
                return Page();
            }

        }
    }
}
